using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Menu.UI
{
    public class MenuController : MonoBehaviour
    {
        private enum MenuState
        {
            Menu, Play, Settings, Exit
        }

        [SerializeField]
        private MainMenuController _mainMenuController;

        [SerializeField]
        private SettingMenuController _settingMenuController;

        [SerializeField]
        private AreYouShurePopupController _exitMenuController;

        public event Action Play;

        private MenuState _currentState;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private void Awake()
        {
            _mainMenuController.Play += StartPlay;
            _mainMenuController.Settings += OpenSettingMenu;
            _mainMenuController.Exit += AreYouSurePopupShow;
            _settingMenuController.GoToMainMenu += OpenMainMenu;
            _exitMenuController.GoToMainMenu += OpenMainMenu;
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
        }

        private async void StartPlay()
        {
            await SetStateAsync(MenuState.Play);
            Play?.Invoke();
        }

        private async void OpenSettingMenu()
        {
            await SetStateAsync(MenuState.Settings);

            _settingMenuController.gameObject.SetActive(true);
            await _settingMenuController.PlayFadeInAnimAsync(cancellationTokenSource.Token);
        }

        private async void AreYouSurePopupShow()
        {
            await SetStateAsync(MenuState.Exit);

            _exitMenuController.gameObject.SetActive(true);
            await _exitMenuController.PlayFadeInAnimAsync(cancellationTokenSource.Token);
        }

        private async void OpenMainMenu()
        {
            await SetStateAsync(MenuState.Menu);

            _mainMenuController.gameObject.SetActive(true);
            await _mainMenuController.PlayFadeInAnimAsync(cancellationTokenSource.Token);
        }

        private async UniTask SetStateAsync(MenuState state)
        {
            switch (_currentState)
            {
                case MenuState.Settings:
                   await _settingMenuController.PlayFadeOutAnimAsync(cancellationTokenSource.Token);
                   _settingMenuController.gameObject.SetActive(false);
                    break;
                case MenuState.Exit:
                    await _exitMenuController.PlayFadeOutAnimAsync(cancellationTokenSource.Token);
                    _exitMenuController.gameObject.SetActive(false);
                    break;
                case MenuState.Menu:
                    await _mainMenuController.PlayFadeOutAnimAsync(cancellationTokenSource.Token);
                    _mainMenuController.gameObject.SetActive(false);
                    break;
            }

            _currentState = state;
        }
    }
}
