using System.Linq;
using AudioLib.CriAdx;
using UnityEngine;

namespace GameSample.Audio
{
    /// <summary>
    /// GameAudio管理クラス (CRI)
    /// </summary>
    public class GameCriAdxAudioService : IGameAudioService
    {
        /// <summary>
        /// オーディオAPIサービス
        /// </summary>
        private readonly ICriAdxApiService _criAdxApiService;

        public GameCriAdxAudioService()
        {
            // 初期生成情報
            var initializeSettings = GameCriAdxAudioSettings.CriAdx.InitializeSetting;

            // Listenerオブジェクト設定
            var listenerObject = Camera.main.gameObject;
            initializeSettings.ListenerObject = listenerObject;

            // サービス生成
            _criAdxApiService = new CriAdxApiService(initializeSettings);
        }

        /// <summary>
        /// Masterボリューム
        /// </summary>
        public float MasterVolume
        {
            get => _criAdxApiService.GetBusVolume(GameCriAdxAudioSettings.CriAdx.BusName.Master);
            set => _criAdxApiService.SetBusVolume(GameCriAdxAudioSettings.CriAdx.BusName.Master, value);
        }

        /// <summary>
        /// BGMボリューム
        /// </summary>
        public float BgmVolume
        {
            get => _criAdxApiService.GetCategoryVolume(GameCriAdxAudioSettings.CriAdx.CategoryName.Bgm);
            set => _criAdxApiService.SetCategoryVolume(GameCriAdxAudioSettings.CriAdx.CategoryName.Bgm, value);
        }

        /// <summary>
        /// SEボリューム
        /// </summary>
        public float SeVolume
        {
            get => _criAdxApiService.GetCategoryVolume(GameCriAdxAudioSettings.CriAdx.CategoryName.Se);
            set => _criAdxApiService.SetCategoryVolume(GameCriAdxAudioSettings.CriAdx.CategoryName.Se, value);
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            _criAdxApiService.Pause();
        }

        /// <summary>
        /// 再開
        /// </summary>
        public void Resume()
        {
            _criAdxApiService.Resume();
        }

        /// <summary>
        /// サウンドシートロード処理
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        public void LoadSoundSheet(string soundSheetName)
        {
            var cueSheetInfo = GameCriAdxAudioSettings.CriAdx.CueSheetInfoArray
                    .FirstOrDefault(cueSheetInfo => cueSheetInfo.Name == soundSheetName);
            _criAdxApiService.RegisterCueSheet(cueSheetInfo);
        }

        /// <summary>
        /// サウンドシートアンロード処理
        /// </summary>
        public void UnLoadAllSoundSheet()
        {
            _criAdxApiService.RemoveAllCueSheet();
        }

        /// <summary>
        /// BGM全停止
        /// </summary>
        /// <param name="option">停止オプション</param>
        public void StopAllBgm(IGameAudioService.SoundStopOption option = null)
        {
            _criAdxApiService.StopAll(new ICriAdxApiService.SoundStopOption()
            {
                FadeTimeMs = option?.FadeTimeMs ?? 0,
            });
        }

        /// <summary>
        /// サウンドイベント停止
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">停止オプション</param>
        public void StopSoundEvent(string eventName, IGameAudioService.SoundStopOption option = null)
        {
            _criAdxApiService.Stop(GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), new ICriAdxApiService.SoundStopOption()
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
            _criAdxApiService.Play(GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), eventName,
                new ICriAdxApiService.SoundPlayOption()
            {
                FadeTimeMs = option?.FadeTimeMs ?? 0,
            });

            // コールバックイベントの追加
            if (option?.BeatSyncCallback != null)
            {
                _criAdxApiService.SetBeatSyncCallback(
                    GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), option.BeatSyncLabel, option.BeatSyncCallback);
            }
            if (option?.CustomEventCallback != null)
            {
                _criAdxApiService.SetSequenceCallback(option.CustomEventName, option.CustomEventCallback);
            }
        }

        /// <summary>
        /// サウンドイベント再生(3D)
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="gameObject">再生するゲームオブジェクト</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, GameObject gameObject, IGameAudioService.SoundPlayOption option = null)
        {
            _criAdxApiService.Play3d(gameObject, GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), eventName,
                new ICriAdxApiService.SoundPlayOption()
            {
                FadeTimeMs = option?.FadeTimeMs ?? 0,
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
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.Normal, 1000);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.BgmReverb, 1000);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.BgmDistortion, 1000);
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
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.Normal, 1000);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.SeReverb, 1000);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _criAdxApiService.ChangeBusSnapshot(GameCriAdxAudioSettings.CriAdx.BusSnapshotName.SeDistortion, 1000);
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
                    // イントロ再生時は何もしない
                    break;
                case IGameAudioService.NextBlockState.BgmAtomChainMain:
                    // 対象インデックスに進める
                    const int playBlock = 2;
                    var eventName = GameCriAdxAudioSettings.CriAdx.CueName.BgmAtomChain;
                    _criAdxApiService.SetNextBlockIndex(eventName, playBlock);
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
            _criAdxApiService.SetAisacControl(GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), eventName, parameterName, value);

        }

        /// <summary>
        /// スペクトラムアナライザの設定
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <returns>作成に成功したか？</returns>
        public bool SetSpectrumAnalyzer(string eventName, int sampleCount)
        {
            _criAdxApiService.SetSpectrumAnalyzer(GameCriAdxAudioSettings.CriAdx.GetCueSheetName(eventName), sampleCount);
            return true;
        }

        /// <summary>
        /// スペクトラムアナライザから周波数データを取得する
        /// </summary>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル値に変換するか？</param>
        /// <returns></returns>
        public float[] GetSpectrumData(int sampleCount, bool isConvertDecibel)
        {
            return _criAdxApiService.GetSpectrumData(sampleCount, isConvertDecibel);
        }
    }
}
