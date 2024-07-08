using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Extensions;
using Assets.Scripts.Game.Signal;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Managers
{
    public class GameCanvasManager : IInitializable, IDisposable
    {
        private readonly GameCanvas _canvas;
        private readonly SignalBus _signalBus;

        //TODO: Should release by Pool
        private StepView _currentStep;

        public GameCanvasManager(GameCanvas gameCanvas, SignalBus signalBus)
        {
            _canvas = gameCanvas;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<StepSignal>(OnStep);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<StepSignal>(OnStep);
        }

        private void OnStep(StepSignal stepSignal)
        {
            if(_currentStep == null)
            {
                _currentStep = GameObject.Instantiate(_canvas.StepsView, _canvas.Content.transform);
            }

            _currentStep.SetTextByColor(stepSignal.Color, stepSignal.Type.FigureMarkFormat() + stepSignal.Step.CordinateFormat());

            if(_currentStep.FieldIsFull())
            {
                _currentStep = null;
            }
        }

        //private void OnReset(ResetSignal resetSignal)
        //{
        //    _canvas.Content.Reset();
        //}
    }
}