using UnityEngine;
using UnityEngine.Pool;
using Assets.Scripts.Game.Managers;

namespace Assets.Scripts.Game.Pool
{
    public class HighliterPool : Singletone<HighliterPool>
    {
        [SerializeField]
        private int _defaultCapacity = 4;

        [SerializeField]
        private int _maxCapacity = 8 * 4;

        private BoardManager _boardManager;

        private Material _material;

        private IObjectPool<GameObject> _pool;
        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<GameObject>(CreatePooledObject, TakeFromPool, ReturnToPool, DestroyPooledObject, true, _defaultCapacity, _maxCapacity);
                }

                return _pool;
            }
        }

        private void Awake()
        {
            _material = Resources.Load<Material>("HighlightCell");
        }

        private GameObject CreatePooledObject()
        {
            var cellSize = _boardManager.GetCellSize();

            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.SetParent(transform);
            quad.transform.Rotate(90, 0, 0);

            quad.transform.localScale = new Vector3(cellSize.x, cellSize.z, 1);

            if (_material == null)
            {
                Debug.LogError("Material don't loaded");
            }

            quad.GetComponent<Renderer>().material = _material;

            quad.SetActive(false);
            return quad;
        }

        private void TakeFromPool(GameObject highliter)
        {
            highliter.SetActive(true);
        }

        private void ReturnToPool(GameObject highliter)
        {
            highliter.SetActive(false);
        }

        private void DestroyPooledObject(GameObject highliter)
        {
            Destroy(highliter);
        }
    }
}