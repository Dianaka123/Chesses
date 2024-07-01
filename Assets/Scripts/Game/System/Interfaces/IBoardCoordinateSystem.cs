using UnityEngine;

namespace Assets.Scripts.Game.System.Interfaces
{
    public interface IBoardCoordinateSystem
    {
        void Init(Vector3 boardSize);
        Vector3 GetCellPositionByIndexes(Vector2Int position);
        Vector2Int GetIndexesByPosition(Vector3 position);
        Vector3 GetCellSize();
    }
}
