using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.System.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Managers
{
    public class BoardManager : IInitializable
    {
        private readonly IChessPositioner _positioner;
        private readonly IBoardCoordinateSystem _boardCoordinateSystem;
        private readonly IPossibleStepsSystem _possibleStepsSystem;

        public BoardManager(IBoardCoordinateSystem boardCoordinateSystem, IPossibleStepsSystem possibleStepsSystem, IChessPositioner positioner)
        {
            _boardCoordinateSystem = boardCoordinateSystem;
            _possibleStepsSystem = possibleStepsSystem;
            _positioner = positioner;
        }
        
        public void Initialize()
        {
            _positioner.ForceChangeChessPosition += ForceChangeChessPosition;
        }

        public void SetupBoard(Board board)
        {
            _boardCoordinateSystem.SetupBoardSize(board.BoardSize);
        }

        public void InitChessPosition(Figure figure, Vector2Int position) => _positioner.InitiChessPosition(figure, position);

        public Figure GetChessByPosition(Vector2Int position) => _positioner.Figures[position.x, position.y];

        public Figure GetChessByPosition(int x, int y) => _positioner.Figures[x, y];

        public Vector2Int GetPositionByChess(Figure figure) => _positioner.FigureToPositionDictionary[figure];

        public void ChangeChessPosition(Figure figure, Vector2Int newPosition) => _positioner.ChangeChessPosition(figure, newPosition);

        public void ResetAllFiguresPositions() => _positioner.ResetAllFiguresPositions();

        public Vector3 GetCellPositionByIndexes(Vector2Int position) => _boardCoordinateSystem.GetCellPositionByIndexes(position);

        public Vector2Int GetIndexesByPosition(Vector3 position) => _boardCoordinateSystem.GetIndexesByPosition(position);

        public Vector2 GetCellSize() => _boardCoordinateSystem.GetCellSize();
        
        public bool IsShah(PlayersColor playerColor)
        {
            foreach (var figureByPosition in _positioner.FigureToPositionDictionary)
            {
                var possibleSteps = _possibleStepsSystem.GetPossibleSteps(figureByPosition.Key, figureByPosition.Value);
                foreach (var step in possibleSteps)
                {
                    var figure = _positioner.Figures[step.x, step.y];
                    if (figure != null && figure.Type == FigureType.King && figure.Color != playerColor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void ForceChangeChessPosition(Figure figure, Vector2Int newPosition)
        {
            var positionV3 = _boardCoordinateSystem.GetCellPositionByIndexes(newPosition);
            figure.ResetFigure(positionV3);
        }
    }
}