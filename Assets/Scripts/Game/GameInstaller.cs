using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.System;
using Assets.Scripts.Game.System.Interfaces;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BoardCoordinateSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PossibleStepsSystem>().AsSingle();
        Container.Bind<BoardManager>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
    }
}