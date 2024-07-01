using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.UI
{
    public class AreYouShurePopupController : MonoBehaviour
    {
        private readonly int FadeIn = Animator.StringToHash("FadeIn");
        private readonly int FadeOut = Animator.StringToHash("FadeOut");

        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Animator _animator;

        public event Action GoToMainMenu;
        private CancellationTokenSource _cancellationTokenSource;

        public async UniTask PlayFadeInAnimAsync(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            _cancellationTokenSource.Cancel();
            await _animator.SetTriggerAsync(FadeIn, _cancellationTokenSource.Token);
        }

        public async UniTask PlayFadeOutAnimAsync(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            await _animator.SetTriggerAsync(FadeOut, _cancellationTokenSource.Token);
        }

        private void Awake()
        {
            _yesButton.onClick.AddListener(ExitFromApp);
            _noButton.onClick.AddListener(() => GoToMainMenu?.Invoke());
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoToMainMenu?.Invoke();
            }
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cancellationTokenSource.Cancel();
        }

        private void ExitFromApp()
        {
            Application.Quit();
        }
    }
}
