using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.System;
using Assets.Scripts.StateMachines.Base;
using Assets.Scripts.StateMachines.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _highliterStep;

        
        
        public override void InstallBindings()
        {
            Container.BindMemoryPool<HighliterStep, HighliterStep.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_highliterStep)
                .UnderTransformGroup("Steps");

            SetupSM();
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

        void InstallStates()
        {
            Container.Bind<SetupChessState>().AsSingle();
            Container.Bind<ChooseChessState>().AsSingle();
            Container.Bind<ChooseStepState>().AsSingle();
            Container.Bind<DoStepState>().AsSingle();
            Container.Bind<WinState>().AsSingle();
        }

        void InstallerManagers()
        {
            Container.Bind<BoardManager>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
        }

        void InstallFacade()
        {
            Container.Bind<GameConfigurationFacade>().AsSingle();
        }

        void InstallSystems()
        {
            Container.BindInterfacesAndSelfTo<BoardCoordinateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PossibleStepsSystem>().AsSingle();
        }
    }
}