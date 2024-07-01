using System.Threading;

namespace Assets.Scripts.StateMachines
{
    public class ChessSM : SMContext
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private void Start()
        {
            GoTo(new SetupChessState(this), cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
        }
    }
}