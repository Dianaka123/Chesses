using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Extensions;
using Assets.Scripts.Game.Signal;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Game.Managers
{
    public class GameCanvasManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly StepUIView.Pool _pool;
        private readonly ScrollRect _scrollView;

        private StepUIView _currentStep;
        private List<StepUIView> _steps = new List<StepUIView>();

        public GameCanvasManager(SignalBus signalBus, StepUIView.Pool pool, [Inject(Id = "CoreUIScrollRect")] ScrollRect scrollView)
        {
            _signalBus = signalBus;
            _pool = pool;
            _scrollView = scrollView;
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

        private async void OnStep(StepSignal stepSignal)
        {
            if(_currentStep == null)
            {
                var step = _pool.Spawn();
                step.transform.SetSiblingIndex(0);
                _steps.Add(step);

                _currentStep = step;
            }

            _currentStep.SetTextByColor(stepSignal.Color, stepSignal.Type.FigureMarkFormat() + stepSignal.Step.CordinateFormat());

            await UniTask.WaitForEndOfFrame();
            Canvas.ForceUpdateCanvases();
            _scrollView.horizontalNormalizedPosition = 1f;
            Canvas.ForceUpdateCanvases();

            if (_currentStep.FieldIsFull())
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