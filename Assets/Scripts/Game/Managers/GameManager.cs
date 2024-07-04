using Assets.Scripts.Game.Entity;
using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class GameManager
    {
        [HideInInspector]
        public PlayersColor PlayerStep { get; private set; } = PlayersColor.White;

        [HideInInspector]
        public Figure SelectedFigure { get; set; }

        [HideInInspector]
        public Vector3 SelectedFigureStep { get; set; }

        public void StepComplited()
        {
            PlayerStep = PlayerStep == PlayersColor.White ? PlayersColor.Black : PlayersColor.White;
        }
    }
}