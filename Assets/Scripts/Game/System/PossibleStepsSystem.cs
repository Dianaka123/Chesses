using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Extensions;
using Assets.Scripts.Game.System.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.System
{
    public class PossibleStepsSystem : IPossibleStepsSystem
    {
        private readonly IChessPositioner _chessPositioner;

        public PossibleStepsSystem(IChessPositioner chessPositioner)
        {
            _chessPositioner = chessPositioner;
        }

        private Dictionary<FigureType, Vector2Int[]> stepsByType = new Dictionary<FigureType, Vector2Int[]>()
        {
            { FigureType.Bishop,
                new[] { new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1)}
            },
            { FigureType.King,
                new[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 0),
                new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) }
            },
            { FigureType.Knight,
                new[] { new Vector2Int(-1, 2), new Vector2Int(-2, 1), new Vector2Int(1, 2), new Vector2Int(2, 1),
            new Vector2Int(-1, -2), new Vector2Int(-2, -1), new Vector2Int(1, -2), new Vector2Int(2, -1)}
            },
            { FigureType.Pawn,
                new [] {new Vector2Int(0,1), new Vector2Int(0, 2) }
            },
            { FigureType.Queen,
                new[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 0),
                new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) }
            },
            {
                FigureType.Rook,
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1) }
            }
        };

        private Dictionary<FigureType, Vector2Int[]> attackDirectionByType = new Dictionary<FigureType, Vector2Int[]>()
        {
            { FigureType.Pawn,
                new [] { new Vector2Int(-1, 1), new Vector2Int(1, 1) }
            },
        };

        public List<Vector2Int> GetPossibleSteps(Figure figure, Vector2Int startPosition)
        {
            switch (figure.Type)
            {
                case FigureType.Queen:
                case FigureType.Bishop:
                case FigureType.Rook:
                    return GoThroughAllField(figure.Type, startPosition, figure.Color);
                case FigureType.Knight:
                case FigureType.King:
                    return GoToOneCell(figure.Type, startPosition, figure.Color);
                case FigureType.Pawn:
                    return GoLikePawn(figure.Type, startPosition, figure.Color);
                default:
                    return new List<Vector2Int>();
            }
        }

        private List<Vector2Int> GoThroughAllField(FigureType type, Vector2Int startPosition, PlayersColor color)
        {
            var steps = new List<Vector2Int>();
            var currentPosition = startPosition;
            var stepDirection = stepsByType[type];

            foreach (var step in stepDirection)
            {
                var possibleStep = currentPosition;
                var isMeetEnemy = false;

                for (var i = 0; i < CellExtension.MAX_CELL_COUNT; i++)
                {
                    if (isMeetEnemy) { break; }

                    possibleStep += step;
                    if (possibleStep.IsCoordinateValid())
                    {
                        var chess = _chessPositioner.Figures[possibleStep.x, possibleStep.y];
                        if (chess != null && chess.Color == color)
                        {
                            break;
                        }
                        else if (chess != null && chess.Color != color)
                        {
                            isMeetEnemy = true;
                        }

                        steps.Add(possibleStep);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return steps;
        }

        private List<Vector2Int> GoToOneCell(FigureType type, Vector2Int startPosition, PlayersColor color)
        {
            var steps = new List<Vector2Int>();
            var currentPosition = startPosition;
            var stepDirection = stepsByType[type];

            foreach (var step in stepDirection)
            {
                var possiblePosition = currentPosition + step;
                if (possiblePosition.IsCoordinateValid())
                {
                    var chess = _chessPositioner.Figures[possiblePosition.x, possiblePosition.y];
                    if (chess != null && chess.Color == color)
                    {
                        continue;
                    }
                    steps.Add(possiblePosition);
                }
            }

            return steps;
        }

        private List<Vector2Int> GoLikePawn(FigureType type, Vector2Int startPosition, PlayersColor color)
        {
            var steps = new List<Vector2Int>();
            var currentPosition = startPosition;
            var stepDirection = stepsByType[type];
            var attackDirection = attackDirectionByType[type];

            foreach (var step in stepDirection)
            {
                var availablePosition = CalculateStraightAheadStep(color, currentPosition, step);
                var chess = _chessPositioner.Figures[availablePosition.x, availablePosition.y];

                if (chess != null)
                {
                    break;
                }

                if (availablePosition.IsCoordinateValid())
                {
                    steps.Add(availablePosition);
                }
            }

            foreach (var attack in attackDirection)
            {
                var availablePosition = CalculateStraightAheadStep(color, currentPosition, attack);

                if (!availablePosition.IsCoordinateValid())
                {
                    continue;
                }

                var potencialEnemy = _chessPositioner.Figures[availablePosition.x, availablePosition.y];

                if (potencialEnemy != null && potencialEnemy.Type == FigureType.Pawn && potencialEnemy.Color != color)
                {
                    steps.Add(availablePosition);
                }
            }
            return steps;
        }

        private Vector2Int CalculateStraightAheadStep(PlayersColor color, Vector2Int startPosition, Vector2Int direction)
        {
            return color == PlayersColor.White ? startPosition + direction : startPosition - direction;
        }
    }
}