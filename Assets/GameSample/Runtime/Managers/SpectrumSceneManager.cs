using System;
using GameSample.Audio;
using GameSample.Audio.Settings;
using GameSample.Common;
using GameSample.Spectrum;
using UnityEngine;
using UnityEngine.UI;

namespace GameSample.Managers
{
    /// <summary>
    /// SpectrumScene管理クラス
    /// </summary>
    public class SpectrumSceneManager : MonoBehaviour
    {
        /// <summary>
        /// テスト用ボタン群
        /// </summary>
        [SerializeField] private Button _uiStopBgmButton;
        [SerializeField] private Button _uiPlayBgm01Button;
        [SerializeField] private Button _uiPlayBgm02Button;

        [SerializeField] private Button _uiSpectrumTypeNoneButton;
        [SerializeField] private Button _uiSpectrumTypeLineButton;
        [SerializeField] private Button _uiSpectrumTypeCubeButton;

        [SerializeField] private SpectrumVisualizer _lineSpectrumVisualizer;
        [SerializeField] private SpectrumVisualizer _cubeSpectrumVisualizer;

        private IGameAudioSettings GameAudioSettings => ServiceLocator.Resolve<IGameAudioSettings>();

        private enum SpectrumType
        {
            None,
            Line,
            Cube,
        }

        private IGameAudioService AudioService => ServiceLocator.Resolve<IGameAudioService>();

        private Func<int, bool> CreateSpectrumAnalyzer => (resolution) => AudioService.SetSpectrumAnalyzer(GameAudioSettings.SoundEventName_BgmSpaceWould, resolution);
        private Func<int, bool, float[]> GetSpectrumData => (resolution, isConvertDecibel) => AudioService.GetSpectrumData(resolution, isConvertDecibel);

        private bool _isInitialized = false;

        private void Start()
        {
            _uiStopBgmButton.onClick.AddListener(StopBgm);
            _uiPlayBgm01Button.onClick.AddListener(PlayBgm01);
            _uiPlayBgm02Button.onClick.AddListener(PlayBgm02);

            _uiSpectrumTypeNoneButton.onClick.AddListener(() => ChangeSpectrumType(SpectrumType.None));
            _uiSpectrumTypeLineButton.onClick.AddListener(() => ChangeSpectrumType(SpectrumType.Line));
            _uiSpectrumTypeCubeButton.onClick.AddListener(() => ChangeSpectrumType(SpectrumType.Cube));

            var isLineSuccess = _lineSpectrumVisualizer.Initialize(CreateSpectrumAnalyzer, GetSpectrumData);
            var isCubeSuccess = _cubeSpectrumVisualizer.Initialize(CreateSpectrumAnalyzer, GetSpectrumData);
            _isInitialized = isLineSuccess && isCubeSuccess;

            ChangeSpectrumType(SpectrumType.None);
        }

        private void ChangeSpectrumType(SpectrumType type)
        {
            if (!_isInitialized)
            {
                return;
            }
            _lineSpectrumVisualizer.gameObject.SetActive(type == SpectrumType.Line);
            _cubeSpectrumVisualizer.gameObject.SetActive(type == SpectrumType.Cube);
        }

        private void StopBgm()
        {
            AudioService.StopAllBgm();
        }

        private void PlayBgm01()
        {
            var eventName = GameAudioSettings.SoundEventName_BgmSpaceWould;
            AudioService.PlaySoundEvent(eventName);
        }

        private void PlayBgm02()
        {
            var eventName = GameAudioSettings.SoundEventName_BgmShotThunder;
            AudioService.PlaySoundEvent(eventName);
        }
    }
}
