using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.Extensions
{
    public static class StepFormatExtension
    {
        private static char[] horizontal = Enumerable.Range('a', 'h' - 'a' + 1).Select(i => (char)i).ToArray();

        public static string CordinateFormat(this Vector2Int position)
        {
            var x = horizontal[position.x];
            var y = position.y + 1;
            
            return $"{x}{y}";
        }

        public static char FigureMarkFormat(this FigureType type)
        {
            switch (type)
            {
                case FigureType.Bishop:
                    return 'B';
                case FigureType.Queen:
                    return 'Q';
                case FigureType.Knight:
                    return 'N';
                case FigureType.King:
                    return 'K';
                case FigureType.Rook:
                    return 'R';
                default:
                    return ' ';
            }

        }
    }
}
