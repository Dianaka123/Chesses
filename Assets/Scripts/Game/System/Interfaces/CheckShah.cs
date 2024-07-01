using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.System.Interfaces;

namespace Assets.Scripts.Game.System
{
    public class CheckShah : ICheckShah
    {
        private readonly BoardManager boardManager;
        private readonly IPossibleStepsSystem possibleStepsSystem;

        public bool IsShah(PlayersColor playerColor)
        {
            //foreach (var figureByPosition in _figureToPositionDictionary)
            //{
            //    var possibleSteps = _possibleStepsSystem.GetPossibleSteps(figureByPosition.Key, figureByPosition.Value);
            //    foreach (var step in possibleSteps)
            //    {
            //        var figure = _figureArray[step.x, step.y];
            //        if (figure != null && figure.Type == FigureType.King && figure.Color != playerColor)
            //        {
            //            return true;
            //        }
            //    }
            //}

            return false;
        }
    }
}