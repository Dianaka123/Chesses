using Assets.Scripts.Game.Configuration;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Installers
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "SO/GameConfiguration")]
    public class ChessScriptableObjectInstaller : ScriptableObjectInstaller<ChessScriptableObjectInstaller>
    {
        public GameConfiguration Configuration;

        public override void InstallBindings()
        {
            Container.BindInstance(Configuration);
        }

    }
}