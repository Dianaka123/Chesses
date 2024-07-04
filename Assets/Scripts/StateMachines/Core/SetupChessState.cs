using Assets.Scripts.Game;
using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.System;
using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.StateMachines.Core
{
    public class SetupChessState : State
    {
        private readonly BoardManager _boardManager;
        private readonly GameConfigurationFacade _chessConfigurationFacade;
        private readonly LazyInject<ChooseChessState> _chooseChessState;

        public SetupChessState(ChessSM chessSM, BoardManager boardManager, GameConfigurationFacade chessConfigurationFacade, LazyInject<ChooseChessState> chooseChessState) : base(chessSM)
        {
            _boardManager = boardManager;
            _chessConfigurationFacade = chessConfigurationFacade;
            _chooseChessState = chooseChessState;
        }

        public override UniTask Run(CancellationToken token)
        {
            var board = MonoBehaviour.Instantiate<Board>(_chessConfigurationFacade.BoardConfiguration.Board);
            _boardManager.Init(board);

            CreateChessSet(board.transform);

            GoTo(_chooseChessState.Value, token);
            return UniTask.CompletedTask;
        }

        private void CreateChessSet(Transform parent)
        {
            var initFigureSequence = new FigureType[] { FigureType.Rook, FigureType.Knight, FigureType.Bishop, FigureType.King, FigureType.Queen, FigureType.Bishop, FigureType.Knight, FigureType.Rook };

            Action<FigureType, Vector2Int, PlayersColor> SpawnFigure = (type, position, color) => 
            {
                var figureReference = _chessConfigurationFacade.FigureByType[type];
                var material = _chessConfigurationFacade.GetMaterialByPlayerColor(color);

                Figure figure = MonoBehaviour.Instantiate(figureReference, parent);
                _boardManager.AddInitionalChessPosition(figure, position);
                var positionV3 = _boardManager.GetCellPositionByIndexes(position);

                figure.Init(material, _chessConfigurationFacade.HighlighterChessColor, color, type);
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