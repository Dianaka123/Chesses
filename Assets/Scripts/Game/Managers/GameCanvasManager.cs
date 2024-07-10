using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Extensions;
using Assets.Scripts.Game.Signal;
using System;
using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts.Game.Managers
{
    public class GameCanvasManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly StepUIView.Pool _pool;

        private StepUIView _currentStep;
        private List<StepUIView> _steps = new List<StepUIView>();

        public GameCanvasManager(SignalBus signalBus, StepUIView.Pool pool)
        {
            _signalBus = signalBus;
            _pool = pool;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<StepSignal>(OnStep);
            _signalBus.Subscribe<ResetSignal>(OnReset);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<StepSignal>(OnStep);
            _signalBus.Unsubscribe<ResetSignal>(OnReset);
        }

        private void OnStep(StepSignal stepSignal)
        {
            if(_currentStep == null)
            {
                var step = _pool.Spawn();
                step.transform.SetSiblingIndex(0);
                _steps.Add(step);

                _currentStep = step;
            }

            _currentStep.SetTextByColor(stepSignal.Color, stepSignal.Type.FigureMarkFormat() + stepSignal.Step.CordinateFormat());

            if(_currentStep.FieldIsFull())
            {
                _currentStep = null;
            }
        }

        private void OnReset(ResetSignal resetSignal)
        {
            foreach(var step in _steps)
            {
                _pool.Despawn(step);
            }
            _steps.Clear();
            _currentStep = null;
        }
    }
}