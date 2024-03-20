using AudioLib.UnityAudio;
using UnityEngine;

namespace GameSample.Audio
{
    /// <summary>
    /// GameAudio管理クラス (UnityAudio)
    /// </summary>
    public class GameUnityAudioService : IGameAudioService
    {
        /// <summary>
        /// オーディオAPIサービス
        /// </summary>
        private readonly IUnityAudioApiService _unityAudioApiService;

        public GameUnityAudioService()
        {
            // 初期生成情報
            var initializeSettings = GameUnityAudioSettings.UnityAudio.InitializeSetting;

            // MonoBehaviourハンドラの生成
            var monoBehaviourHandlerObject = new GameObject(nameof(GameUnityAudioMonoBehaviour));
            var monoBehaviourHandler = monoBehaviourHandlerObject.AddComponent<GameUnityAudioMonoBehaviour>();
            Object.DontDestroyOnLoad(monoBehaviourHandlerObject);
            initializeSettings.MonoBehaviourHandler = monoBehaviourHandler;

            // Listenerオブジェクト取得
            var listenerObject = Camera.main.gameObject;
            initializeSettings.ListenerObject = listenerObject;

            // サービス生成
            _unityAudioApiService = new UnityAudioApiService(initializeSettings);
        }

        /// <summary>
        /// Masterボリューム
        /// </summary>
        public float MasterVolume
        {
            get => _unityAudioApiService.GetMasterVolume();
            set => _unityAudioApiService.SetMasterVolume(value);
        }

        /// <summary>
        /// BGMボリューム
        /// </summary>
        public float BgmVolume
        {
            get => _unityAudioApiService.GetSoundSheetVolume(GameUnityAudioSettings.UnityAudio.SoundSheetName.Bgm);
            set => _unityAudioApiService.SetSoundSheetVolume(GameUnityAudioSettings.UnityAudio.SoundSheetName.Bgm, value);
        }

        /// <summary>
        /// SEボリューム
        /// </summary>
        public float SeVolume
        {
            get => _unityAudioApiService.GetSoundSheetVolume(GameUnityAudioSettings.UnityAudio.SoundSheetName.Se);
            set => _unityAudioApiService.SetSoundSheetVolume(GameUnityAudioSettings.UnityAudio.SoundSheetName.Se, value);
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            _unityAudioApiService.Pause();
        }

        /// <summary>
        /// 再開
        /// </summary>
        public void Resume()
        {
            _unityAudioApiService.Resume();
        }

        /// <summary>
        /// サウンドシートロード処理
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        public void LoadSoundSheet(string soundSheetName)
        {
            UnityEngine.Debug.LogWarning("not support UnityAudio => LoadSoundSheet.");
        }

        /// <summary>
        /// サウンドシートアンロード処理
        /// </summary>
        public void UnLoadAllSoundSheet()
        {
            UnityEngine.Debug.LogWarning("not support UnityAudio => UnLoadAllSoundSheet.");
        }

        /// <summary>
        /// BGM全停止
        /// </summary>
        /// <param name="option">停止オプション</param>
        public void StopAllBgm(IGameAudioService.SoundStopOption option = null)
        {
            var fadeTimeMs = option?.FadeTimeMs ?? 0;
            _unityAudioApiService.Stop(GameUnityAudioSettings.UnityAudio.SoundSheetName.Bgm, new IUnityAudioApiService.SoundStopOption()
            {
                FadeTimeSec = Mathf.RoundToInt(fadeTimeMs * 0.001f),
            });
        }

        /// <summary>
        /// サウンドイベント停止
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">停止オプション</param>
        public void StopSoundEvent(string eventName, IGameAudioService.SoundStopOption option = null)
        {
            var fadeTimeMs = option?.FadeTimeMs ?? 0;
            _unityAudioApiService.Stop(GameUnityAudioSettings.UnityAudio.GetSoundSheetName(eventName), new IUnityAudioApiService.SoundStopOption()
            {
                FadeTimeSec = Mathf.RoundToInt(fadeTimeMs * 0.001f),
            });
        }

        /// <summary>
        /// サウンドイベント再生
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, IGameAudioService.SoundPlayOption option = null)
        {
            var fadeTimeMs = option?.FadeTimeMs ?? 0;
            _unityAudioApiService.Play(GameUnityAudioSettings.UnityAudio.GetSoundSheetName(eventName), eventName, new IUnityAudioApiService.SoundPlayOption()
            {
                FadeTimeSec = Mathf.RoundToInt(fadeTimeMs * 0.001f),
            });
        }

        /// <summary>
        /// サウンドイベント再生(3D)
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="gameObject">再生するゲームオブジェクト</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, GameObject gameObject, IGameAudioService.SoundPlayOption option = null)
        {
            var spatial3dBlend = 1.0f; // 3D再生のブレンド値
            var fadeTimeMs = option?.FadeTimeMs ?? 0;
            _unityAudioApiService.Play3d(gameObject, spatial3dBlend, GameUnityAudioSettings.UnityAudio.GetSoundSheetName(eventName), eventName, new IUnityAudioApiService.SoundPlayOption()
            {
                FadeTimeSec = Mathf.RoundToInt(fadeTimeMs * 0.001f),
            });
        }

        /// <summary>
        /// BGMエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeBgmEffect(IGameAudioService.EffectType effectType)
        {
            var soundSheetName = GameUnityAudioSettings.UnityAudio.SoundSheetName.Bgm;
            switch (effectType)
            {
                case IGameAudioService.EffectType.Normal:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Normal, 1f);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Reverb, 1f);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Distortion, 1f);
                    break;
            }
        }

        /// <summary>
        /// SEエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeSeEffect(IGameAudioService.EffectType effectType)
        {
            var soundSheetName = GameUnityAudioSettings.UnityAudio.SoundSheetName.Se;
            switch (effectType)
            {
                case IGameAudioService.EffectType.Normal:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Normal, 1f);
                    break;
                case IGameAudioService.EffectType.Reverb:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Reverb, 1f);
                    break;
                case IGameAudioService.EffectType.Distortion:
                    _unityAudioApiService.ChangeSoundSheetSnapshot(soundSheetName, GameUnityAudioSettings.UnityAudio.SnapshotName.Distortion, 1f);
                    break;
            }
        }

        /// <summary>
        /// ブロック遷移状態を設定する
        /// </summary>
        /// <param name="state">遷移状態</param>
        public void SetNextBlockState(IGameAudioService.NextBlockState state)
        {
            UnityEngine.Debug.LogWarning("not support UnityAudio => SetNextBlockState.");
        }

        /// <summary>
        /// ゲーム内パラメータを設定する
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        public void SetGameParameter(string eventName, string parameterName, float value)
        {
            UnityEngine.Debug.LogWarning("not support UnityAudio => SetGameParameter.");
        }

        /// <summary>
        /// スペクトラムアナライザの設定
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <returns>作成に成功したか？</returns>
        public bool SetSpectrumAnalyzer(string eventName, int sampleCount)
        {
            // UnityAudioでは設定不要
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
            return _unityAudioApiService.GetBgmSpectrumData(GameUnityAudioSettings.UnityAudio.SoundSheetName.Bgm, sampleCount, isConvertDecibel);
        }
    }

    /// <summary>
    /// MonoBehaviour実行用
    /// </summary>
    public class GameUnityAudioMonoBehaviour : MonoBehaviour
    {
    }
}
