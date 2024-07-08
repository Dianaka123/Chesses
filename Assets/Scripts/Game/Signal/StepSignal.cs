using UnityEngine;

namespace Assets.Scripts.Game.Signal
{
    public class StepSignal
    {
        public Vector2Int Step {  get; private set; }

        public PlayersColor Color { get; private set; }

        public FigureType Type { get; private set; }

        public StepSignal(Vector2Int step, PlayersColor color, FigureType type)
        {
            Step = step;
            Color = color;
            Type = type;

        }
    }
}