using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.UI
{
    public class SettingMenuController: MonoBehaviour
    {
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _ui;
        [SerializeField] private TMP_Dropdown _resolution;
        [SerializeField] private Toggle _isFullScreen;
        [SerializeField] private Animator _animator;

        private readonly int FadeOutSetting = Animator.StringToHash("FadeOut");
        private readonly int FadeInSetting = Animator.StringToHash("FadeIn");

        public event Action GoToMainMenu;

        public async UniTask PlayFadeOutAnimAsync(CancellationToken cancellationToken = default)
        {
            await _animator.SetTriggerAsync(FadeOutSetting, cancellationToken);
        }

        public async UniTask PlayFadeInAnimAsync(CancellationToken cancellationToken = default)
        {
            await _animator.SetTriggerAsync(FadeInSetting, cancellationToken);
        }

        private void Awake()
        {
            _music.onValueChanged.AddListener(ChangeMusicVolume);
            _ui.onValueChanged.AddListener(ChangeUIMusicVolume);
            _resolution.onValueChanged.AddListener(ChangeResolution);
            _isFullScreen.onValueChanged.AddListener(CheckFullScreenMode);

            InitResolutionsDropDown();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoToMainMenu?.Invoke();
            }
        }

        private void InitResolutionsDropDown()
        {
            var currentResolution = Screen.currentResolution;
            var resolutions = Screen.resolutions;
            var index = GetIndexOfResolution(currentResolution, resolutions);

            _resolution.AddOptions(resolutions.Select(a => $"{a.width} x {a.height}").ToList());
            _resolution.value = index;
        }

        private void CheckFullScreenMode(bool arg0)
        {
            var width = PlayerPrefs.GetInt("winw");
            var height = PlayerPrefs.GetInt("winh");
            Debug.Log($"{width}  {height}");
            Screen.SetResolution(width, height, arg0);
        }

        private void ChangeResolution(int arg0)
        {
            var resolution = GetResolutionFromDropdown(_resolution.options[arg0].text);
            PlayerPrefs.SetInt("winw", resolution.width);
            PlayerPrefs.SetInt("winh", resolution.height);
            Screen.SetResolution(resolution.width, resolution.height, _isFullScreen.isOn);
        }

        private void ChangeUIMusicVolume(float arg0)
        {
            //TODO: change ui volume
        }

        private void ChangeMusicVolume(float arg0)
        {
            //TODO: change music volume
        }

        private Resolution GetResolutionFromDropdown(string value)
        {
            string[] resolution = value.Split(" x ");
            return new Resolution()
            {
                height = int.Parse(resolution[1]),
                width = int.Parse(resolution[0]),
            };
        }

        private int GetIndexOfResolution(Resolution resolution, Resolution[] resolutions)
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == resolution.width && resolutions[i].height == resolution.height)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
