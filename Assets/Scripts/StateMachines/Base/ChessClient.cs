using System.Threading;
using Zenject;

namespace Assets.Scripts.StateMachines.Base
{
    public class ChessClient : ITickable
    {
        private ChessSM _stateMachine;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public ChessClient(ChessSM stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
            _stateMachine.Run(cancellationTokenSource.Token);
        }
    }
}