using System.Linq;
using AudioLib.Wwise;
using UnityEngine;

namespace GameSample.Audio
{
    /// <summary>
    /// GameAudio管理クラス (Wwise)
    /// </summary>
    public class GameWwiseAudioService : IGameAudioService
    {
        /// <summary>
        /// オーディオAPIサービス
        /// </summary>
        private readonly IWwiseApiService _wwiseApiService;

        public GameWwiseAudioService()
        {
            // 初期生成情報
            var initializeSettings = GameWwiseAudioSettings.Wwise.InitializeSettings;

            // Listenerオブジェクト
            var listenerObject = Camera.main.gameObject;
            initializeSettings.ListenerObject = listenerObject;

            // サービス生成
            _wwiseApiService = new WwiseApiService(initializeSettings);
        }

        /// <summary>
        /// Masterボリューム
        /// </summary>
        public float MasterVolume
        {
            get => _wwiseApiService.GetMasterVolume();
            set => _wwiseApiService.SetMasterVolume(value);
        }

        /// <summary>
        /// BGMボリューム
        /// </summary>
        public float BgmVolume
        {
            get => _wwiseApiService.GetSoundBankVolume(GameWwiseAudioSettings.Wwise.SoundBankName.Bgm);
            set => _wwiseApiService.SetSoundBankVolume(GameWwiseAudioSettings.Wwise.SoundBankName.Bgm, value);
        }

        /// <summary>
        /// SEボリューム
        /// </summary>
        public float SeVolume
        {
            get => _wwiseApiService.GetSoundBankVolume(GameWwiseAudioSettings.Wwise.SoundBankName.Se);
            set => _wwiseApiService.SetSoundBankVolume(GameWwiseAudioSettings.Wwise.SoundBankName.Se, value);
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            var eventName = GameWwiseAudioSettings.Wwise.SoundEventName.GlobalPauseAll;
            _wwiseApiService.DoEvent(GameWwiseAudioSettings.Wwise.GetSoundBankName(eventName), eventName);
        }

        /// <summary>
        /// 再開
        /// </summary>
        public void Resume()
        {
            var eventName = GameWwiseAudioSettings.Wwise.SoundEventName.GlobalResumeAll;
            _wwiseApiService.DoEvent(GameWwiseAudioSettings.Wwise.GetSoundBankName(eventName), eventName);
        }

        /// <summary>
        /// サウンドシートロード処理
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        public void LoadSoundSheet(string soundSheetName)
        {
            var soundBankInfo = GameWwiseAudioSettings.Wwise.SoundBankInfoArray.FirstOrDefault(soundBankInfo => soundBankInfo.Name == soundSheetName);
            _wwiseApiService.LoadSoundBank(soundBankInfo);
        }

        /// <summary>
        /// サウンドシートアンロード処理
        /// </summary>
        public void UnLoadAllSoundSheet()
        {
            _wwiseApiService.UnLoadAllSoundBank();
        }

        /// <summary>
        /// BGM全停止
        /// </summary>
        /// <param name="option">停止オプション</param>
        public void StopAllBgm(IGameAudioService.SoundStopOption option = null)
        {
            var bgmSoundBankName = GameWwiseAudioSettings.Wwise.SoundBankName.Bgm;
            if (option != null)
            {
                // オプション設定がある場合、個々のEventに対して行う
                var eventNameArray = GameWwiseAudioSettings.Wwise.SoundBankEventInfoDictionary[bgmSoundBankName];
                foreach (var eventName in eventNameArray)
                {
                    StopSoundEvent(eventName, option);
                }
            }
            else
            {
                _wwiseApiService.StopAllSoundBank(GameWwiseAudioSettings.Wwise.SoundBankName.Bgm);
            }
        }

        /// <summary>
        /// サウンドイベント停止
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">停止オプション</param>
        public void StopSoundEvent(string eventName, IGameAudioService.SoundStopOption option = null)
        {
            _wwiseApiService.StopEvent(GameWwiseAudioSettings.Wwise.GetSoundBankName(eventName), eventName, new IWwiseApiService.SoundStopOption()
            {
                FadeTimeMs = option?.FadeTimeMs ?? 0,
            });
        }

        /// <summary>
        /// サウンドイベント再生
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, IGameAudioService.SoundPlayOption option = null)
        {
            _wwiseApiService.PlayEvent(GameWwiseAudioSettings.Wwise.GetSoundBankName(eventName), eventName,
                new IWwiseApiService.SoundPlayOption()
                {
                    IsStopOther = true,
                    FadeTimeMs = option?.FadeTimeMs ?? 0,
                    BeatSyncCallback = option?.BeatSyncCallback,
                    CustomEventCallback = option?.CustomEventCallback,
                });
        }

        /// <summary>
        /// サウンドイベント再生(3D)
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="gameObject">再生するゲームオブジェクト</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, UnityEngine.GameObject gameObject, IGameAudioService.SoundPlayOption option = null)
        {
            _wwiseApiService.PlayEvent(GameWwiseAudioSettings.Wwise.GetSoundBankName(eventName), eventName, gameObject,
                new IWwiseApiService.SoundPlayOption()
                {
                    IsStopOther = true,
                    FadeTimeMs = option?.FadeTimeMs ?? 0,
                    BeatSyncCallback = option?.BeatSyncCallback,
                    CustomEventCallback = option?.CustomEventCallback,
                });
        }

        /// <summary>
        /// BGMエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeBgmEffect(IGameAudioService.EffectType effectType)
        {
            switch (effectType)
            {
                case IGameAudioService.EffectType.Normal:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmReverb, 0);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmDistortion, 0);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmReverb, 1);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmDistortion, 0);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmReverb, 0);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.BgmDistortion, 1);
                    break;
            }
        }

        /// <summary>
        /// SEエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeSeEffect(IGameAudioService.EffectType effectType)
        {
            switch (effectType)
            {
                case IGameAudioService.EffectType.Normal:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeReverb, 0);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeDistortion, 0);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeReverb, 1);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeDistortion, 0);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeReverb, 0);
                    _wwiseApiService.SetGameParameter(GameWwiseAudioSettings.Wwise.GameParameterName.SeDistortion, 1);
                    break;
            }
        }

        /// <summary>
        /// ブロック遷移状態を設定する
        /// </summary>
        /// <param name="state">遷移状態</param>
        public void SetNextBlockState(IGameAudioService.NextBlockState state)
        {
            switch (state)
            {
                case IGameAudioService.NextBlockState.BgmAtomChainIntro:
                    _wwiseApiService.SetState(GameWwiseAudioSettings.Wwise.StateGroupName.BgmAtomChain,
                        GameWwiseAudioSettings.Wwise.StateName.BgmAtomChainIntro);
                    break;
                case IGameAudioService.NextBlockState.BgmAtomChainMain:
                    _wwiseApiService.SetState(GameWwiseAudioSettings.Wwise.StateGroupName.BgmAtomChain,
                        GameWwiseAudioSettings.Wwise.StateName.BgmAtomChainMain);
                    break;
            }
        }

        /// <summary>
        /// ゲーム内パラメータを設定する
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        public void SetGameParameter(string eventName, string parameterName, float value)
        {
            _wwiseApiService.SetGameParameter(parameterName, value);
        }

        /// <summary>
        /// スペクトラムアナライザの設定
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <returns>作成に成功したか？</returns>
        public bool SetSpectrumAnalyzer(string eventName, int sampleCount)
        {
            UnityEngine.Debug.LogWarning("not support Wwise => SetSpectrumAnalyzer.");
            return false;
        }

        /// <summary>
        /// スペクトラムアナライザから周波数データを取得する
        /// </summary>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル値に変換するか？</param>
        /// <returns></returns>
        public float[] GetSpectrumData(int sampleCount, bool isConvertDecibel)
        {
            UnityEngine.Debug.LogWarning("not support Wwise GetSpectrumData.");
            return new float[sampleCount];
        }
    }
}
