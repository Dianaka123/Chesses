using Assets.Scripts.Game.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.System.Interfaces
{
    public interface IPossibleStepsSystem
    {
        public List<Vector2Int> GetPossibleSteps(Figure figure, Vector2Int startPosition);
    }
}