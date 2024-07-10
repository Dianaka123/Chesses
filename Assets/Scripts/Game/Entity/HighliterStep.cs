using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Entity
{
    public class HighliterStep : MonoBehaviour
    {
        internal void ChangePosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        internal void SetupRotation(Vector3 rotate)
        {
            transform.Rotate(rotate);
        }

        internal void SetupSize(Vector2 size)
        {
            transform.localScale = new Vector3(size.x, size.y, 1);
        }

        public class Pool : MonoMemoryPool<Vector3, Vector2, HighliterStep>
        {
            protected override void OnCreated(HighliterStep step)
            {
                step.SetupRotation(new Vector3(90, 0, 0));

                base.OnCreated(step);
            }

            protected override void Reinitialize(Vector3 initialPosition, Vector2 cellSize, HighliterStep step)
            {
                step.ChangePosition(initialPosition);
                step.SetupSize(cellSize);
            }
        }
    }
}