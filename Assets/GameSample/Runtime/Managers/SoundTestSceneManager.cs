using System.Collections.Generic;
using GameSample.Audio;
using GameSample.Common;
using GameSample.Objects;
using GameSample.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameSample.Managers
{
    /// <summary>
    /// SoundTestシーン管理クラス
    /// </summary>
    public class SoundTestSceneManager : MonoBehaviour
    {
        /// <summary>
        /// テスト用ボタン群
        /// </summary>
        [SerializeField] private Button _uiStopBgmButton;
        [SerializeField] private Button _uiStopFadeBgmButton;
        [SerializeField] private Button _uiPlayBgm01Button;
        [SerializeField] private Button _uiPlayBgm02Button;
        [SerializeField] private Button _uiPlayFadeBgm01Button;
        [SerializeField] private Button _uiPlayFadeBgm02Button;

        [SerializeField] private Button _uiStart3dSeButton;
        [SerializeField] private Button _uiStop3dSeButton;

        [SerializeField] private Button _uiEffectBgmNormalButton;
        [SerializeField] private Button _uiEffectBgmReverbButton;
        [SerializeField] private Button _uiEffectBgmDistortionButton;
        [SerializeField] private Button _uiEffectSeNormalButton;
        [SerializeField] private Button _uiEffectSeReverbButton;
        [SerializeField] private Button _uiEffectSeDistortionButton;

        [SerializeField] private Button _uiPauseButton;
        [SerializeField] private Button _uiResumeButton;
        [SerializeField] private Button _uiOpenConfigButton;

        /// <summary>
        /// オーディオ設定UI
        /// </summary>
        [SerializeField] private UIAudioConfig _uiAudioConfig;

        /// <summary>
        /// 3DSE確認用のロボット
        /// </summary>
        [SerializeField] private List<RobotBehaviour> _robots;

        /// <summary>
        /// Audioサービス
        /// </summary>
        private IGameAudioService GameAudioService => ServiceLocator.Resolve<IGameAudioService>();
        private IGameAudioSettings GameAudioSettings => ServiceLocator.Resolve<IGameAudioSettings>();

        private void Start()
        {
            // UIイベント登録
            _uiStopBgmButton.onClick.AddListener(StopBgm);
            _uiStopFadeBgmButton.onClick.AddListener(StopFadeBgm);
            _uiPlayBgm01Button.onClick.AddListener(PlayBgm01);
            _uiPlayBgm02Button.onClick.AddListener(PlayBgm02);
            _uiPlayFadeBgm01Button.onClick.AddListener(PlayFadeBgm01);
            _uiPlayFadeBgm02Button.onClick.AddListener(PlayFadeBgm02);

            _uiStart3dSeButton.onClick.AddListener(Start3dSeSample);
            _uiStop3dSeButton.onClick.AddListener(Stop3dSeSample);

            _uiPauseButton.onClick.AddListener(Pause);
            _uiResumeButton.onClick.AddListener(Resume);
            _uiOpenConfigButton.onClick.AddListener(() => _uiAudioConfig.gameObject.SetActive(true));

            _uiEffectBgmNormalButton.onClick.AddListener(ChangeEffectBgmNormal);
            _uiEffectBgmReverbButton.onClick.AddListener(ChangeEffectBgmReverb);
            _uiEffectBgmDistortionButton.onClick.AddListener(ChangeEffectBgmDistortion);
            _uiEffectSeNormalButton.onClick.AddListener(ChangeEffectSeNormal);
            _uiEffectSeReverbButton.onClick.AddListener(ChangeEffectSeReverb);
            _uiEffectSeDistortionButton.onClick.AddListener(ChangeEffectSeDistortion);

            _uiAudioConfig.gameObject.SetActive(false);
            _uiAudioConfig.SetListenerBgImage(() => _uiAudioConfig.gameObject.SetActive(false));
            _uiAudioConfig.SetValueMasterVolumeSlider(GameAudioService.MasterVolume);
            _uiAudioConfig.SetValueBgmVolumeSlider(GameAudioService.BgmVolume);
            _uiAudioConfig.SetValueSeVolumeSlider(GameAudioService.SeVolume);
            _uiAudioConfig.SetListenerMasterVolumeSlider(value => GameAudioService.MasterVolume = value);
            _uiAudioConfig.SetListenerBgmVolumeSliderCallback(value => GameAudioService.BgmVolume = value);
            _uiAudioConfig.SetListenerSeVolumeSliderCallback(value => GameAudioService.SeVolume = value);
        }
        private void StopBgm()
        {
            GameAudioService.StopAllBgm();
        }

        private void StopFadeBgm()
        {
            GameAudioService.StopAllBgm(new IGameAudioService.SoundStopOption() { FadeTimeMs = 1000 });
        }

        private void PlayBgm01()
        {
            GameAudioService.PlaySoundEvent(GameAudioSettings.SoundEventName_BgmSpaceWould);
        }

        private void PlayBgm02()
        {
            GameAudioService.PlaySoundEvent(GameAudioSettings.SoundEventName_BgmShotThunder);
        }

        private void PlayFadeBgm01()
        {
            GameAudioService.PlaySoundEvent(GameAudioSettings.SoundEventName_BgmSpaceWould,
                new IGameAudioService.SoundPlayOption() { FadeTimeMs = 1000});
        }

        private void PlayFadeBgm02()
        {
            GameAudioService.PlaySoundEvent(GameAudioSettings.SoundEventName_BgmShotThunder,
                new IGameAudioService.SoundPlayOption() { FadeTimeMs = 1000});
        }

        private void Start3dSeSample()
        {
            foreach (var robot in _robots)
            {
                robot.StartMove();
            }
        }

        private void Stop3dSeSample()
        {
            foreach (var robot in _robots)
            {
                robot.StopMove();
            }
        }

        private void ChangeEffectBgmNormal()
        {
            GameAudioService.ChangeBgmEffect(IGameAudioService.EffectType.Normal);
        }

        private void ChangeEffectBgmReverb()
        {
            GameAudioService.ChangeBgmEffect(IGameAudioService.EffectType.Reverb);
        }

        private void ChangeEffectBgmDistortion()
        {
            GameAudioService.ChangeBgmEffect(IGameAudioService.EffectType.Distortion);
        }

        private void ChangeEffectSeNormal()
        {
            GameAudioService.ChangeSeEffect(IGameAudioService.EffectType.Normal);
        }

        private void ChangeEffectSeReverb()
        {
            GameAudioService.ChangeSeEffect(IGameAudioService.EffectType.Reverb);
        }

        private void ChangeEffectSeDistortion()
        {
            GameAudioService.ChangeSeEffect(IGameAudioService.EffectType.Distortion);
        }

        private void Pause()
        {
            GameAudioService.Pause();
            foreach (var robot in _robots)
            {
                robot.Pause();
            }
        }

        private void Resume()
        {
            GameAudioService.Resume();
            foreach (var robot in _robots)
            {
                robot.Resume();
            }
        }
    }
}
