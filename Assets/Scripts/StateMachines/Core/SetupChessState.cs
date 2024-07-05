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
        private readonly CameraManager _cameraManager;

        public SetupChessState(ChessSM chessSM, BoardManager boardManager, GameConfigurationFacade chessConfigurationFacade, LazyInject<ChooseChessState> chooseChessState,
            CameraManager cameraManager) : base(chessSM)
        {
            _boardManager = boardManager;
            _chessConfigurationFacade = chessConfigurationFacade;
            _chooseChessState = chooseChessState;
            _cameraManager = cameraManager;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _cameraManager.Button.onClick.AddListener(OnClick);
            return base.Enter(token);
        }

        private void OnClick()
        {
            _boardManager.ResetAllFigures();
        }

        public async override UniTask Run(CancellationToken token)
        {
            var board = MonoBehaviour.Instantiate<Board>(_chessConfigurationFacade.BoardConfiguration.Board);
            _boardManager.Init(board);

            CreateChessSet(board.transform);

            await GoTo(_chooseChessState.Value, token);
        }

        private void CreateChessSet(Transform parent)
        {
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


            for (int i = 0; i < 8; i++)
            {
                var firstRowFigureType = _chessConfigurationFacade.FirstRowInitSequence[i];

                SpawnFigure(_chessConfigurationFacade.SecondRowFigure, new Vector2Int(i, 1), PlayersColor.White);
                SpawnFigure(firstRowFigureType, new Vector2Int(i, 0), PlayersColor.White);

                SpawnFigure(_chessConfigurationFacade.SecondRowFigure, new Vector2Int(i, 6), PlayersColor.Black);
                SpawnFigure(firstRowFigureType, new Vector2Int(i, 7), PlayersColor.Black);
            }
        }
    }
}