using System;
using UnityEngine;

namespace Assets.Scripts.Game.Entity
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;

        public Vector3 BoardSize { get => _boxCollider.bounds.size; }
        public Action<Vector3> ClickOnBoard;
        
        private void OnMouseDown()
        {
            ClickOnBoard?.Invoke(Input.mousePosition);
        }
    }
}