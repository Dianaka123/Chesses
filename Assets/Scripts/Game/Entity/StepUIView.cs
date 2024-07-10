using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Entity
{
    public class StepUIView: MonoBehaviour
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

        public void ResetText()
        {
            _whiteStep.text = string.Empty;
            _blackStep.text = string.Empty;
        }

        public class Pool : MonoMemoryPool<StepUIView>
        {
            protected override void OnDespawned(StepUIView item)
            {
                item.ResetText();
                base.OnDespawned(item);
            }
        }
    }
}