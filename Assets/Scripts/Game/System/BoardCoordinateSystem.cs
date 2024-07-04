using Assets.Scripts.Game.System.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.System
{
    public class BoardCoordinateSystem : IBoardCoordinateSystem
    {
        private Vector3 _boardSize;

        public void Init(Vector3 boardSize)
        {
            _boardSize = boardSize;
        }

        public Vector3 GetCellPositionByIndexes(Vector2Int pos)
        {
            if (!pos.IsCoordinateValid())
            {
                throw new IndexOutOfRangeException();
            }

            pos.x = pos.x - 4;
            pos.y = pos.y - 4;

            var cellSizeX = _boardSize.x / CellExtension.MAX_CELL_COUNT;
            var cellSizeZ = _boardSize.z / CellExtension.MAX_CELL_COUNT;
            var halfCellX = cellSizeX / 2;
            var halfCellZ = cellSizeZ / 2;
            var yOffset = _boardSize.y / 10;

            return new Vector3(cellSizeX * pos.x + halfCellX, _boardSize.y + yOffset, cellSizeZ * pos.y + halfCellZ);
        }

        public Vector2Int GetIndexesByPosition(Vector3 position)
        {
            var cellSizeX = _boardSize.x / CellExtension.MAX_CELL_COUNT;
            var cellSizeZ = _boardSize.z / CellExtension.MAX_CELL_COUNT;

            var x = Mathf.FloorToInt(position.x / cellSizeX) + 4;
            var y = Mathf.FloorToInt(position.z / cellSizeZ) + 4;

            return new Vector2Int(x, y);
        }

        public Vector2 GetCellSize()
        {
            var cellSizeX = _boardSize.x / CellExtension.MAX_CELL_COUNT;
            var cellSizeZ = _boardSize.z / CellExtension.MAX_CELL_COUNT;

            return new Vector2(cellSizeX, cellSizeZ);
        }
    }
}