using Assets.Scripts.StateMachines.Core;
using System;
using System.Threading;
using Zenject;

namespace Assets.Scripts.StateMachines.Base
{
    public class ChessSM : SMContext, IInitializable, IDisposable
    {
        private readonly LazyInject<SetupChessState> _setup;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public ChessSM(LazyInject<SetupChessState> setup)
        {
            _setup = setup;
        }

        public async void Initialize()
        {
            await GoTo(_setup.Value, cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }
    }
}