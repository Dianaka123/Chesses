using UnityEngine;

namespace Assets.Scripts.Menu.Camera
{
    public class MenuCameraController : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera _camera;

        [SerializeField]
        [Range(0, 1)]
        private float _deviation = 0.5f;

        //private readonly ICameraDirectionResolver _directionResolver;

        private Vector3 _initialCameraDirection;

        private void Start()
        {
            _initialCameraDirection = gameObject.transform.forward;
        }

        private void FixedUpdate()
        {
            var mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var screenSize = new Vector2(Screen.width, Screen.height);
            //gameObject.transform.forward = _directionResolver.Resolve(mousePosition, screenSize, _deviation, _initialCameraDirection);
        }
    }
}

