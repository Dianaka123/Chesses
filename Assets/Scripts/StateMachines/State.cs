using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachines
{
    public abstract class State
    {
        public virtual UniTask Enter(CancellationToken token) { return UniTask.CompletedTask; }
        public virtual UniTask Exit(CancellationToken token) { return UniTask.CompletedTask; }

        public abstract UniTask Run(CancellationToken token);
    }
}