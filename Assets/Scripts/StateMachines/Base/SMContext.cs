using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Base
{
    public class SMContext
    {
        public State CurrentState { get; private set; }

        private Stack<State> _states = new Stack<State>();

        public void Run(CancellationToken token)
        {
            try
            {
                CurrentState.Run(token);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
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

        private UniTask StateTransition(State newState, CancellationToken token = default)
        {
            lock (this)
            {
                if (CurrentState == newState)
                {
                    return UniTask.CompletedTask;
                }

                if (CurrentState != null)
                {
                    CurrentState.Exit(token);
                }

                CurrentState = newState;
                CurrentState.Enter(token);
            }

            return UniTask.CompletedTask;
        }
    }
}