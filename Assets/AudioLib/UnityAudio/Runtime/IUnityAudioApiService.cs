using UnityEngine;

namespace AudioLib.UnityAudio
{
    /// <summary>
    /// UnityAudio API操作クラス interface
    /// </summary>
    public interface IUnityAudioApiService
    {
        /// <summary>
        /// UnityAudio関連リソースの初期設定情報
        /// </summary>
        public class InitializeSetting
        {
            /// <summary>
            /// Listenerオブジェクト
            /// </summary>
            public UnityEngine.GameObject ListenerObject;

            /// <summary>
            /// MonoBehaviourハンドラ(コルーチン実行用)
            /// </summary>
            public MonoBehaviour MonoBehaviourHandler;

            /// <summary>
            /// Audioファイル格納パス
            /// </summary>
            public string AudioAssetPath;

            /// <summary>
            /// AudioMixerファイル格納パス
            /// </summary>
            public string AudioMixerAssetPath;

            /// <summary>
            /// サウンドシート情報
            /// </summary>
            public SoundSheetInfo[] SoundSheetInfoArray;

            /// <summary>
            /// AudioMixer情報
            /// </summary>
            public AudioMixerInfo[] AudioMixerInfoArray;
        }

        /// <summary>
        /// サウンドシート情報
        /// </summary>
        public class SoundSheetInfo
        {
            /// <summary>
            /// サウンドシート名
            /// </summary>
            public string Name;

            /// <summary>
            /// 初期化時にロードするか？
            /// </summary>
            public bool IsLoadOnInitialize;

            /// <summary>
            /// ループ再生するか？
            /// </summary>
            public bool IsPlayLoop = false;
        }

        /// <summary>
        /// AudioMixer情報
        /// </summary>
        public class AudioMixerInfo
        {
            /// <summary>
            /// AudioMixer名
            /// </summary>
            public string MixerName;

            /// <summary>
            /// AudioMixerGroup名
            /// </summary>
            public string MixerGroupName;

            /// <summary>
            /// ボリュームパラメータ名
            /// </summary>
            public string VolumeParamName;

            /// <summary>
            /// 紐づくサウンドシート名
            /// </summary>
            public string SoundSheetName;
        }

        /// <summary>
        /// 再生オプション
        /// </summary>
        public class SoundPlayOption
        {
            /// <summary>
            /// フェード時間(秒)
            /// </summary>
            public int FadeTimeSec = 0;

            /// <summary>
            /// ボリューム
            /// </summary>
            public float Volume = 1f;

            /// <summary>
            /// ピッチ
            /// </summary>
            public float Pitch = 1f;
        }

        /// <summary>
        /// 停止オプション
        /// </summary>
        public class SoundStopOption
        {
            /// <summary>
            /// フェード時間(秒)
            /// </summary>
            public int FadeTimeSec = 0;
        }

        /// <summary>
        /// マスターボリューム取得
        /// </summary>
        /// <returns>ボリューム</returns>
        public float GetMasterVolume();

        /// <summary>
        /// マスターボリューム設定
        /// </summary>
        /// <param name="volume">ボリューム</param>
        public void SetMasterVolume(float volume);

        /// <summary>
        /// サウンドシートのボリューム取得
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <returns>ボリューム</returns>
        public float GetSoundSheetVolume(string soundSheetName);

        /// <summary>
        /// サウンドシートのボリューム設定
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="volume">ボリューム</param>
        public void SetSoundSheetVolume(string soundSheetName, float volume);

        /// <summary>
        /// Listenerオブジェクトの設定
        /// </summary>
        /// <param name="listenerObject">Listenerオブジェクト</param>
        public void RegisterListenerObject(UnityEngine.GameObject listenerObject);

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause();

        /// <summary>
        /// 一時停止解除
        /// </summary>
        public void Resume();

        /// <summary>
        /// オーディオ再生
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="audioName">オーディオ名</param>
        /// <param name="option">再生オプション</param>
        public void Play(string soundSheetName, string audioName, IUnityAudioApiService.SoundPlayOption option = null);

        /// <summary>
        /// オーディオ再生(3D位置再生)
        /// </summary>
        /// <param name="targetObject">対象オブジェクト</param>
        /// <param name="spatialRatio">3D再生のブレンド値</param>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="audioName">オーディオ名</param>
        /// <param name="option">再生オプション</param>
        public void Play3d(GameObject targetObject, float spatialRatio, string soundSheetName, string audioName, IUnityAudioApiService.SoundPlayOption option = null);

        /// <summary>
        /// オーディオ停止
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="option">停止オプション</param>
        public void Stop(string soundSheetName, IUnityAudioApiService.SoundStopOption option = null);

        /// <summary>
        /// サウンドシートのスナップショット切替
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="snapshotName">スナップショット名</param>
        /// <param name="fadeTimeSec">フェード時間(秒)</param>
        public void ChangeSoundSheetSnapshot(string soundSheetName, string snapshotName, float fadeTimeSec);

        /// <summary>
        /// 指定サウンドシートのスペクトラム周波数を取得
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル変換するか？</param>
        /// <returns></returns>
        public float[] GetBgmSpectrumData(string soundSheetName, int sampleCount, bool isConvertDecibel);
    }
}