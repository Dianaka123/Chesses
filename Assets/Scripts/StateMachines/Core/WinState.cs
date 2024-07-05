using Assets.Scripts.Game.Managers;
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
        private readonly LazyInject<ChooseChessState> _chooseChessState;

        public WinState(ChessSM chessSM, BoardManager boardManager, LazyInject<ChooseChessState> chooseChessState) : base(chessSM)
        {
            _boardManager = boardManager;
            _chooseChessState = chooseChessState;
        }

        public async override UniTask Run(CancellationToken token)
        {
            _boardManager.ResetAllFigures();

            await GoTo(_chooseChessState.Value, token);
        }

        public override UniTask Exit(CancellationToken token)
        {
            return base.Exit(token);
        }
    }
}