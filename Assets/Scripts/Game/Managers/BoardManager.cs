using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.System.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class BoardManager
    {
        private Figure[,] _figureArray = new Figure[8, 8];
        private Dictionary<Figure, Vector2Int> _figureToPositionDictionary = new Dictionary<Figure, Vector2Int>(16);
        private Dictionary<Figure, Vector2Int> _initionalFigureToPositionDictionary = new Dictionary<Figure, Vector2Int>(16);

        private IBoardCoordinateSystem _boardCoordinateSystem;
        private IPossibleStepsSystem _possibleStepsSystem;
        private Board _board;

        public BoardManager(IBoardCoordinateSystem boardCoordinateSystem, IPossibleStepsSystem possibleStepsSystem)
        {
            _boardCoordinateSystem = boardCoordinateSystem;
            _possibleStepsSystem = possibleStepsSystem;
        }

        public void Init(Board board)
        {
            _board = board;
            _boardCoordinateSystem.Init(_board.BoardSize);
        }

        public void AddInitionalChessPosition(Figure figure, Vector2Int vector)
        {
            _figureArray[vector.x, vector.y] = figure;
            _figureToPositionDictionary.Add(figure, vector);
            _initionalFigureToPositionDictionary.Add(figure, vector);
        }

        public Vector2Int GetPositionByChess(Figure figure) => _figureToPositionDictionary[figure];

        public Figure GetChessByPosition(Vector2Int position) => _figureArray[position.x, position.y];

        public void ChangeChessPosition(Figure figure, Vector2Int newPosition)
        {
            if (_figureToPositionDictionary.TryGetValue(figure, out var oldPosition))
            {
                _figureArray[oldPosition.x, oldPosition.y] = null;

                var enemyFigure = _figureArray[newPosition.x, newPosition.y];
                if (enemyFigure != null)
                {
                    _figureToPositionDictionary.Remove(enemyFigure);
                }

                _figureArray[newPosition.x, newPosition.y] = figure;
                _figureToPositionDictionary[figure] = newPosition;
            }
        }

        public Vector3 GetCellPositionByIndexes(Vector2Int index)
        {
            return _boardCoordinateSystem.GetCellPositionByIndexes(index);
        }
        
        public Vector2Int GetIndxesByCellPosition(Vector3 position)
        {
            return _boardCoordinateSystem.GetIndexesByPosition(position);
        }

        public Vector2 GetCellSize()
        {
            return _boardCoordinateSystem.GetCellSize();
        }

        public void ResetAllFigures()
        {
            foreach (var figureAndPos in _initionalFigureToPositionDictionary)
            {
                var initionalPos = _boardCoordinateSystem.GetCellPositionByIndexes(figureAndPos.Value);
                var figure = figureAndPos.Key;

                figure.ResetFigure(initionalPos);
            }

            _figureToPositionDictionary = new Dictionary<Figure, Vector2Int>(_initionalFigureToPositionDictionary); ;
        }

        public bool IsShah(PlayersColor playerColor)
        {
            foreach (var figureByPosition in _figureToPositionDictionary)
            {
                var possibleSteps = _possibleStepsSystem.GetPossibleSteps(figureByPosition.Key, figureByPosition.Value);
                foreach (var step in possibleSteps)
                {
                    var figure = _figureArray[step.x, step.y];
                    if (figure != null && figure.Type == FigureType.King && figure.Color != playerColor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}