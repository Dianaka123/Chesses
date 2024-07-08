using Assets.Scripts.Game.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.System.Interfaces
{
    public interface IChessPositioner
    {
        event Action<Figure, Vector2Int> ForceChangeChessPosition;
        Dictionary<Figure, Vector2Int> FigureToPositionDictionary {  get; }
        Figure[,] Figures { get; }

        void InitiChessPosition(Figure figure, Vector2Int vector);
        void ChangeChessPosition(Figure figure, Vector2Int newPosition);
        void ResetAllFiguresPositions();
    }
}
