using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.Signal;
using Assets.Scripts.Game.System;
using Assets.Scripts.StateMachines.Base;
using Assets.Scripts.StateMachines.Core;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private HighliterStep _highliterStep;

        [SerializeField]
        private StepUIView _stepViewUi;

        [SerializeField]
        private Transform _stepUIContainer;
        
        public override void InstallBindings()
        {
            InstallPools();
            InstallSignals();
            InstallStates();
            InstallSystems();
            InstallerManagers();
            InstallFacade();
        }
        
        void SetupSM()
        {
            Container.BindInterfacesAndSelfTo<ChessSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChessClient>().AsSingle();
        }

        void SetupSignals()
        {
            SignalBusInstaller.Install(Container);
        }

        void InstallSignals()
        {
            SetupSignals();
            Container.DeclareSignal<StepSignal>();
            Container.DeclareSignal<ResetSignal>();
        }

        void InstallPools()
        {
            Container.BindMemoryPool<HighliterStep, HighliterStep.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_highliterStep)
                .UnderTransformGroup("Steps");

            Container.BindMemoryPool<StepUIView, StepUIView.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_stepViewUi)
                .UnderTransform(_stepUIContainer);
        }

        void InstallStates()
        {
            SetupSM();

            Container.Bind<SetupChessState>().AsSingle();
            Container.Bind<ChooseChessState>().AsSingle();
            Container.Bind<ChooseStepState>().AsSingle();
            Container.Bind<DoStepState>().AsSingle();
            Container.Bind<WinState>().AsSingle();
        }

        void InstallerManagers()
        {
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameCanvasManager>().AsSingle();
        }

        void InstallFacade()
        {
            Container.Bind<GameConfigurationFacade>().AsSingle();
        }

        void InstallSystems()
        {
            Container.BindInterfacesAndSelfTo<BoardCoordinateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PossibleStepsSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChessPositionerSystem>().AsSingle();
        }
    }
}