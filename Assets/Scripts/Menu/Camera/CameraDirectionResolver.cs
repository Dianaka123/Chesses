using UnityEngine;

namespace Assets.Scripts.Menu.Camera
{
    internal class CameraDirectionResolver : ICameraDirectionResolver
    {
        public Vector3 Resolve(Vector2 mousePosition, Vector2 screenSize, float deviation, Vector3 initialCameraDirection)
        {
            var mouseX = mousePosition.x - screenSize.x / 2;
            var mouseY = mousePosition.y - screenSize.y / 2;

            var x = mouseX / screenSize.x;
            var y = mouseY / screenSize.y;

            var relativeOffset = new Vector3(x, y) * deviation;
            var angles = new Quaternion(-relativeOffset.y, relativeOffset.x, 0, 1);

            return Matrix4x4.Rotate(angles).MultiplyVector(initialCameraDirection);
        }
    }
}
