using UnityEngine;

namespace Assets.Scripts.Menu.Camera
{
    internal interface ICameraDirectionResolver
    {
        Vector3 Resolve(Vector2 mousePosition, Vector2 screenSize, float deviation, Vector3 initialCameraDirection);
    }
}
