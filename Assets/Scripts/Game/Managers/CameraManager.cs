using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Camera CurrentCamera => _gameCamera;

        public Button Button;

        [SerializeField]
        private Camera _gameCamera;
    }
}