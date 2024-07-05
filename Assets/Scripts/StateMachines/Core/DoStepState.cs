using Assets.Scripts.Game;
using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.StateMachines.Core
{
    public class DoStepState : State
    {
        private readonly BoardManager _boardManager;
        private readonly GameManager _gameManager;
        private readonly LazyInject<ChooseChessState> _chooseChessState;
        private readonly LazyInject<WinState> _winState;

        private Figure _figure;
        private Vector3 _step;
        private Figure _enemy;

        public DoStepState(ChessSM chessSM, BoardManager boardManager, GameManager gameManager, LazyInject<ChooseChessState> chooseChessState, LazyInject<WinState> winState) : base(chessSM)
        {
            _boardManager = boardManager;
            _gameManager = gameManager;
            _chooseChessState = chooseChessState;
            _winState = winState;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _figure = _gameManager.SelectedFigure;
            _step = _gameManager.SelectedFigureStep;

            return base.Enter(token);
        }

        public override async UniTask Run(CancellationToken token)
        {
            Vector2Int newPosition = _boardManager.GetIndxesByCellPosition(_step);
            _enemy = _boardManager.GetChessByPosition(newPosition);
            _boardManager.ChangeChessPosition(_figure, newPosition);

            if (_enemy != null && _enemy.Type == FigureType.King)
            {
                await GoTo(_winState.Value, token);
            }
            else
            {
                _figure.ChangeColorBySelect(false);

                //_boardManager.IsShah(_figure.Color);

                _gameManager.StepComplited();

                await GoTo(_chooseChessState.Value, token);
            }
        }

        public async override UniTask Exit(CancellationToken token)
        {
            await _figure.MoveTo(_step);

            if (_enemy != null)
            {
                await _enemy.FallAnimation();
            }
            await base.Exit(token);
        }
    }
}