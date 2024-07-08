using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.Entity
{
    public class StepView: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _whiteStep;

        [SerializeField]
        private TMP_Text _blackStep;

        public void SetTextByColor(PlayersColor playerColor, string text)
        {
            var textField = playerColor == PlayersColor.White ? _whiteStep : _blackStep;
            textField.text = text;
        }

        public bool FieldIsFull()
        {
            return _whiteStep.text != string.Empty && _blackStep.text != string.Empty;
        }
    }
}