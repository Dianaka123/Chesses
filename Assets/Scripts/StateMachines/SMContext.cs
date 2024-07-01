using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class SMContext : MonoBehaviour
    {
        public State CurrentState { get; private set; }

        private Stack<State> _states = new Stack<State>();

        public void Run(CancellationToken token)
        {
            CurrentState.Run(token);
        }

        public async void GoTo(State state, CancellationToken token)
        {
            _states.Push(state);
            await StateTransition(state, token);
        }

        public async void Back(CancellationToken token)
        {
            if (_states.Count > 1)
            {
                _states.Pop();
                await StateTransition(_states.Peek(), token);
            }
        }

        private async UniTask StateTransition(State newState, CancellationToken token = default)
        {
            lock (this)
            {
                if (CurrentState == newState)
                {
                    return;
                }

                if (CurrentState != null)
                {
                    CurrentState.Exit(token);
                }

                CurrentState = newState;
                CurrentState.Enter(token);
            }
        }
    }
}