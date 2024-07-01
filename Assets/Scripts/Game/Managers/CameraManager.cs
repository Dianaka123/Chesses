using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Camera CurrentCamera => _gameCamera;

        [SerializeField]
        private Camera _gameCamera;
    }
}