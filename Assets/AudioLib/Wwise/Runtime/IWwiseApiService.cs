using System;

namespace AudioLib.Wwise
{
    /// <summary>
    /// Wwise API操作クラス interface
    /// </summary>
    public interface IWwiseApiService
    {
        /// <summary>
        /// Wwise関連リソースの初期設定情報
        /// </summary>
        public class InitializeSettings
        {
            /// <summary>
            /// Listenerオブジェクト
            /// </summary>
            public UnityEngine.GameObject ListenerObject;

            /// <summary>
            /// SoundBank情報
            /// </summary>
            public SoundBankInfo[] SoundBankInfoArray;
        }

        /// <summary>
        /// SoundBank情報
        /// </summary>
        public class SoundBankInfo
        {
            /// <summary>
            /// SoundBank名
            /// </summary>
            public string Name;

            /// <summary>
            /// 初期化時にロードするか？
            /// </summary>
            public bool IsLoadOnInitialize;

            /// <summary>
            /// デコードするか？
            /// </summary>
            public bool IsDecodeBank;

            /// <summary>
            /// デコードして保存するか？
            /// </summary>
            public bool IsSaveDecodedBank;
        }

        /// <summary>
        /// 再生オプション
        /// </summary>
        public class SoundPlayOption
        {
            /// <summary>
            /// 再生中の音声を停止するか？
            /// </summary>
            public bool IsStopOther;

            /// <summary>
            /// フェード時間
            /// </summary>
            public int FadeTimeMs;

            /// <summary>
            /// ビート同期イベント コールバック
            /// </summary>
            public Action BeatSyncCallback = null;

            /// <summary>
            /// カスタムで追加したイベント コールバック
            /// </summary>
            public Action CustomEventCallback = null;
        }

        /// <summary>
        /// 停止オプション
        /// </summary>
        public class SoundStopOption
        {
            /// <summary>
            /// フェード時間
            /// </summary>
            public int FadeTimeMs;
        }

        /// <summary>
        /// Listenerオブジェクトの設定
        /// </summary>
        /// <param name="listenerObject"></param>
        public void RegisterListenerObject(UnityEngine.GameObject listenerObject);

        /// <summary>
        /// SoundBankのロード
        /// </summary>
        /// <param name="soundBankInfo"></param>
        public void LoadSoundBank(IWwiseApiService.SoundBankInfo soundBankInfo);

        /// <summary>
        /// SoundBankのアンロード
        /// </summary>
        /// <param name="soundBankName"></param>
        public void UnLoadSoundBank(string soundBankName);

        /// <summary>
        /// 全てのSoundBankのアンロード
        /// </summary>
        public void UnLoadAllSoundBank();

        /// <summary>
        /// Eventの実行
        /// 停止・再開など、再生以外のEventを想定
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public uint DoEvent(string soundBankName, string eventName);

        /// <summary>
        /// Eventの再生
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public uint PlayEvent(string soundBankName, string eventName, IWwiseApiService.SoundPlayOption option = null);

        /// <summary>
        /// Eventの再生
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="gameObject"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public uint PlayEvent(string soundBankName, string eventName, UnityEngine.GameObject gameObject, IWwiseApiService.SoundPlayOption option = null);

        /// <summary>
        /// Eventの停止
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="option"></param>
        public void StopEvent(string soundBankName, string eventName, SoundStopOption option = null);

        /// <summary>
        /// SoundBankの全Eventの停止
        /// </summary>
        /// <param name="soundBankName"></param>
        public void StopAllSoundBank(string soundBankName);

        /// <summary>
        /// 全てのEventの停止
        /// </summary>
        public void StopAll();

        /// <summary>
        /// SoundBankボリュームの取得
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <returns></returns>
        public float GetSoundBankVolume(string soundBankName);

        /// <summary>
        /// SoundBankボリュームの設定
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        public bool SetSoundBankVolume(string soundBankName, float volume);

        /// <summary>
        /// マスターボリュームの取得
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume();

        /// <summary>
        /// マスターボリュームの設定
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(float volume);

        /// <summary>
        /// GameParameterの設定
        /// </summary>
        /// <param name="gameParameterName"></param>
        /// <param name="value"></param>
        public void SetGameParameter(string gameParameterName, float value);

        /// <summary>
        /// GameParameterの取得
        /// </summary>
        /// <param name="gameParameterName"></param>
        /// <returns></returns>
        public float GetGameParameter(string gameParameterName);

        /// <summary>
        /// Stateの設定
        /// </summary>
        /// <param name="stateGroupName"></param>
        /// <param name="stateName"></param>
        public void SetState(string stateGroupName, string stateName);
    }
}
