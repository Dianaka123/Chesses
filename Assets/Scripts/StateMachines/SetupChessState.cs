using Assets.Scripts.Game;
using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class SetupChessState : State
    {
        private readonly ChessSM _chessSM;
        private readonly BoardManager _boardManager;

        private ChessConfiguration _chessConfiguration;
        private Board _board;

        public SetupChessState(ChessSM sm)
        {
            _chessSM = sm;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _chessConfiguration = Resources.Load<ChessConfiguration>("ChessConfiguration");
            return base.Enter(token);
        }

        public override UniTask Run(CancellationToken token)
        {
            _board = MonoBehaviour.Instantiate<Board>(_chessConfiguration.Board);
            _boardManager.Init(_board);

            CreateChessSet(_board.transform);

            _chessSM.GoTo(new ChooseChessState(_chessSM), default);
            return UniTask.CompletedTask;
        }

        private void CreateChessSet(Transform parent)
        {
            var initFigureSequence = new FigureType[] { FigureType.Rook, FigureType.Knight, FigureType.Bishop, FigureType.King, FigureType.Queen, FigureType.Bishop, FigureType.Knight, FigureType.Rook };

            Action<FigureType, Vector2Int, PlayersColor> SpawnFigure = (type, position, color) => 
            {
                var figureReference = _chessConfiguration.FigureByType[type];
                var material = _chessConfiguration.GetMaterialByColor(color);

                Figure figure = MonoBehaviour.Instantiate(figureReference, parent);
                _boardManager.AddInitionalChessPosition(figure, position);
                var positionV3 = _boardManager.GetCellPositionByIndexes(position);

                figure.Init(material, _chessConfiguration.selectedColor, color, type);
                figure.SetInitialPosition(positionV3);

            };

            var firstRowFigure = FigureType.Pawn;

            for (int i = 0; i < 8; i++)
            {
                var secondRowFigureType = initFigureSequence[i];

                SpawnFigure(firstRowFigure, new Vector2Int(i, 1), PlayersColor.White);
                SpawnFigure(secondRowFigureType, new Vector2Int(i, 0), PlayersColor.White);

                SpawnFigure(firstRowFigure, new Vector2Int(i, 6), PlayersColor.Black);
                SpawnFigure(secondRowFigureType, new Vector2Int(i, 7), PlayersColor.Black);
            }
        }
    }
}