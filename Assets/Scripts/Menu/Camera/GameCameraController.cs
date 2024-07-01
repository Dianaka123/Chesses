
using UnityEngine;

namespace Assets.Scripts.Menu.Camera
{
    internal class GameCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        [SerializeField] private float _cameraInitDistance = 0.5f;

        [SerializeField]
        [Range(0, 1)]
        private float _sensitive = 0.1f;

        [SerializeField]
        [Min(0)]
        private float _speed = 1;

        [SerializeField]
        [Min(0)]
        private float _maxZoom = 1;

        private float _zoomLevel;
        private float _zoomPos;

        void FixedUpdate()
        {
            _zoomLevel += Input.mouseScrollDelta.y * _sensitive;
            _zoomLevel = Mathf.Clamp(_zoomLevel, 0,_maxZoom);
            _zoomPos = Mathf.MoveTowards(_zoomPos, _zoomLevel, _speed * Time.fixedDeltaTime);
            transform.position = _parent.position + (transform.forward * _zoomPos);
        }

        public void InitCameraPosition(Vector3 chessPos)
        {
            _parent.transform.position = chessPos - new Vector3(0, 0, _cameraInitDistance);
        }
    }
}
