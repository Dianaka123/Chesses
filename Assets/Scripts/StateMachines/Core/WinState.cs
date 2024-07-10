using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.Signal;
using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.StateMachines.Core
{
    public class WinState : State
    {
        private readonly BoardManager _boardManager;
        private readonly SignalBus _signalBus;
        private readonly LazyInject<ChooseChessState> _chooseChessState;

        public WinState(ChessSM chessSM, BoardManager boardManager, LazyInject<ChooseChessState> chooseChessState, SignalBus signalBus) : base(chessSM)
        {
            _boardManager = boardManager;
            _chooseChessState = chooseChessState;
            _signalBus = signalBus;
        }

        public async override UniTask Run(CancellationToken token)
        {
            _signalBus.Fire(new ResetSignal());
            //TODO: Popup which ask Restart or Exit
            _boardManager.ResetAllFiguresPositions();

            await GoTo(_chooseChessState.Value, token);
        }

        public override UniTask Exit(CancellationToken token)
        {
            return base.Exit(token);
        }
    }
}