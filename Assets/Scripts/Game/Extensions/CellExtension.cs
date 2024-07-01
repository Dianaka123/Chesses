using UnityEngine;

public static class CellExtension
{
    public const int MAX_CELL_COUNT = 8;
    public const int MIN_CELL_COUNT = 1;

    public static bool IsCoordinateValid(this Vector2Int coordinate)
    {
        return coordinate.x >= MIN_CELL_COUNT - 1 && coordinate.y >= MIN_CELL_COUNT - 1 && coordinate.x < MAX_CELL_COUNT && coordinate.y < MAX_CELL_COUNT;
    } 
}