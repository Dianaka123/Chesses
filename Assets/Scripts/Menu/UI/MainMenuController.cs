using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private readonly int FadeOutMainAnimKey = Animator.StringToHash("FadeOut");
        private readonly int FadeInMainAnimKey = Animator.StringToHash("FadeIn");

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private Animator _animator;

        public event Action Play;
        public event Action Settings;
        public event Action Exit;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private void Awake()
        {
            _playButton.onClick.AddListener(() => Play?.Invoke());
            _settingsButton.onClick.AddListener(() => Settings?.Invoke());
            _exitButton.onClick.AddListener(() => Exit?.Invoke());
        }

        public async UniTask PlayFadeOutAnimAsync(CancellationToken cancellationToken = default)
        {
            await _animator.SetTriggerAsync(FadeOutMainAnimKey, cancellationToken);
        }

        public async UniTask PlayFadeInAnimAsync(CancellationToken cancellationToken = default)
        {
            await _animator.SetTriggerAsync(FadeInMainAnimKey, cancellationToken);
        }
    }
}
