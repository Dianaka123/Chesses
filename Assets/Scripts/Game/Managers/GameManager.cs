using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class GameManager
    {
        [HideInInspector]
        public PlayersColor PlayerStep { get; private set; } = PlayersColor.White;

        public void StepComplited()
        {
            PlayerStep = PlayerStep == PlayersColor.White ? PlayersColor.Black : PlayersColor.White;
        }


    }
}