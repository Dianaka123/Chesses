using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachines.Core
{
    public class WinState : State
    {
        public WinState(ChessSM chessSM) : base(chessSM)
        {
        }

        public override UniTask Run(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}