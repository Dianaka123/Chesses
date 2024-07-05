using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachines.Base
{
    public abstract class State
    {
        protected ChessSM ChessSM;

        protected State(ChessSM chessSM) {
            ChessSM = chessSM;
        }

        public virtual UniTask Enter(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
        
        public virtual UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }

        public abstract UniTask Run(CancellationToken token);

        public UniTask GoTo(State state, CancellationToken token)
        {
            return ChessSM.GoTo(state, token);
        }

        public void Back(CancellationToken token)
        {
            ChessSM.Back(token);
        }
    }
}