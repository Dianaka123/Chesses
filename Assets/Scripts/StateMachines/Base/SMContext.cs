using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Base
{
    internal enum Stage
    {
        Enter,
        Run,
        Exit
    }

    public class SMContext
    {
        public State CurrentState { get; private set; }

        private Stack<State> _states = new Stack<State>();
        private Stage _stage;

        public async void Run(CancellationToken token)
        {
            if(_stage == Stage.Enter || _stage == Stage.Exit)
            {
                return;
            }

            try
            {
                await CurrentState.Run(token);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async UniTask GoTo(State state, CancellationToken token)
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
            if (CurrentState == newState)
            {
                return;
            }

            if (CurrentState != null)
            {
                Debug.Log("Exit " + CurrentState.GetType().Name);
                _stage = Stage.Exit;
                await CurrentState.Exit(token);
            }

            CurrentState = newState;
            Debug.Log("Inter " + CurrentState.GetType().Name);

            _stage = Stage.Enter;
            await CurrentState.Enter(token);

            _stage = Stage.Run;
        }
    }
}