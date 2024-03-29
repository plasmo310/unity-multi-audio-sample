using System;

namespace GameSample.Audio
{
    /// <summary>
    /// GameAudio管理クラス interface
    /// </summary>
    public interface IGameAudioService
    {
        /// <summary>
        /// 初期化設定
        /// </summary>
        public class InitializeSettings
        {
            public SoundSheetInfo[] SoundSheetInfoArray;
        }

        /// <summary>
        /// サウンドシート情報
        /// (CueSheet or SoundBank)
        /// </summary>
        public class SoundSheetInfo
        {
            /// <summary>
            /// SoundBank名
            /// </summary>
            public string Name;

            /// <summary>
            /// UnityAudio固有オプション
            /// </summary>
            public class UnityAudioOption
            {
                /// <summary>
                /// ループ再生するか？
                /// </summary>
                public bool IsPlayLoop;
            }
            public UnityAudioOption UnityAudio;

            /// <summary>
            /// CRI固有オプション
            /// </summary>
            public class CriAtomOption
            {
                /// <summary>
                /// AWBファイルが存在するか？
                /// </summary>
                public bool IsExistAwbFile;

                /// <summary>
                /// ループ再生するか？
                /// </summary>
                public bool IsPlayLoop;
            }
            public CriAtomOption CriAtom;

            /// <summary>
            /// Wwise固有オプション
            /// </summary>
            public class WwiseOption
            {
                /// <summary>
                /// デコードするか？
                /// </summary>
                public bool IsDecodeBank;

                /// <summary>
                /// デコードして保存するか？
                /// </summary>
                public bool IsSaveDecodedBank;
            }
            public WwiseOption Wwise;
        }

        /// <summary>
        /// サウンド再生オプション
        /// </summary>
        public class SoundPlayOption
        {
            /// <summary>
            /// フェード時間
            /// </summary>
            public int FadeTimeMs;

            /// <summary>
            /// ビート同期イベント コールバック
            /// </summary>
            public Action BeatSyncCallback = null;

            /// <summary>
            /// ビート同期イベントラベル(optional)
            /// </summary>
            public string BeatSyncLabel;

            /// <summary>
            /// カスタム追加イベント コールバック
            /// </summary>
            public Action CustomEventCallback = null;

            /// <summary>
            /// カスタム追加イベント名 (optional)
            /// </summary>
            public string CustomEventName;
        }

        /// <summary>
        /// サウンド停止オプション
        /// </summary>
        public class SoundStopOption
        {
            /// <summary>
            /// フェード時間
            /// </summary>
            public int FadeTimeMs;
        }

        /// <summary>
        /// Masterボリューム
        /// </summary>
        public float MasterVolume
        {
            get;
            set;
        }

        /// <summary>
        /// BGMボリューム
        /// </summary>
        public float BgmVolume
        {
            get;
            set;
        }

        /// <summary>
        /// SEボリューム
        /// </summary>
        public float SeVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause();

        /// <summary>
        /// 再開
        /// </summary>
        public void Resume();

        /// <summary>
        /// サウンドシートロード処理
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        public void LoadSoundSheet(string soundSheetName);

        /// <summary>
        /// サウンドシートアンロード処理
        /// </summary>
        public void UnLoadAllSoundSheet();

        /// <summary>
        /// BGM全停止
        /// </summary>
        /// <param name="option">停止オプション</param>
        public void StopAllBgm(IGameAudioService.SoundStopOption option = null);

        /// <summary>
        /// サウンドイベント停止
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">停止オプション</param>
        public void StopSoundEvent(string eventName, IGameAudioService.SoundStopOption option = null);

        /// <summary>
        /// サウンドイベント再生
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, IGameAudioService.SoundPlayOption option = null);

        /// <summary>
        /// サウンドイベント再生(3D)
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="gameObject">再生するゲームオブジェクト</param>
        /// <param name="option">再生オプション</param>
        public void PlaySoundEvent(string eventName, UnityEngine.GameObject gameObject,
            IGameAudioService.SoundPlayOption option = null);

        /// <summary>
        /// エフェクトタイプ
        /// </summary>
        public enum EffectType
        {
            Normal,
            Reverb,
            Distortion,
        }

        /// <summary>
        /// BGMエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeBgmEffect(IGameAudioService.EffectType effectType);

        /// <summary>
        /// SEエフェクト変更
        /// </summary>
        /// <param name="effectType">エフェクトタイプ</param>
        public void ChangeSeEffect(IGameAudioService.EffectType effectType);

        /// <summary>
        /// ブロック遷移状態
        /// </summary>
        public enum NextBlockState
        {
            BgmAtomChainIntro,
            BgmAtomChainMain,
        }

        /// <summary>
        /// ブロック遷移状態を設定する
        /// </summary>
        /// <param name="state">遷移状態</param>
        public void SetNextBlockState(IGameAudioService.NextBlockState state);

        /// <summary>
        /// ゲーム内パラメータを設定する
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        public void SetGameParameter(string eventName, string parameterName, float value);

        /// <summary>
        /// スペクトラムアナライザの設定
        /// </summary>
        /// <param name="eventName">サウンドイベント名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <returns>作成に成功したか？</returns>
        public bool SetSpectrumAnalyzer(string eventName, int sampleCount);

        /// <summary>
        /// スペクトラムアナライザから周波数データを取得する
        /// </summary>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル値に変換するか？</param>
        /// <returns></returns>
        public float[] GetSpectrumData(int sampleCount, bool isConvertDecibel);
    }
}
