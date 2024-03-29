using System;
using UnityEngine;

namespace AudioLib.CriAdx
{
    /// <summary>
    /// CriAtom API操作クラス interface
    /// </summary>
    public interface ICriAdxApiService
    {
        /// <summary>
        /// CRI関連リソースの初期設定情報
        /// </summary>
        public class InitializeSetting
        {
            /// <summary>
            /// Listenerオブジェクト
            /// </summary>
            public UnityEngine.GameObject ListenerObject;

            /// <summary>
            /// アセットファイルパス
            /// </summary>
            public string AssetFilePath;

            /// <summary>
            /// ACFファイル名
            /// </summary>
            public string AcfFileName;

            /// <summary>
            /// CueSheet情報
            /// </summary>
            public CueSheetInfo[] CueSheetInfoArray;
        }

        /// <summary>
        /// CueSheet情報
        /// </summary>
        public class CueSheetInfo
        {
            /// <summary>
            /// CueSheet名
            /// </summary>
            public string Name;

            /// <summary>
            /// 初期化時にロードするか？
            /// </summary>
            public bool IsLoadOnInitialize;

            /// <summary>
            /// AWBファイルが存在するか？
            /// </summary>
            public bool IsExistAwbFile;

            /// <summary>
            /// ループ再生するか？
            /// </summary>
            public bool IsPlayLoop = false;
        }

        /// <summary>
        /// 再生オプション
        /// </summary>
        public class SoundPlayOption
        {
            /// <summary>
            /// ボリューム
            /// </summary>
            public float Volume = 1f;

            /// <summary>
            /// ピッチ
            /// ※デフォルトは0でセント単位での指定、100で半音分
            /// </summary>
            public float Pitch = 1f;

            /// <summary>
            /// フェードさせる時間
            /// </summary>
            public int FadeTimeMs = 0;
        }

        /// <summary>
        /// 停止オプション
        /// </summary>
        public class SoundStopOption
        {
            /// <summary>
            /// フェードさせる時間
            /// </summary>
            public int FadeTimeMs = 0;
        }

        /// <summary>
        /// Listenerオブジェクトの設定
        /// </summary>
        /// <param name="listenerObject"></param>
        public void RegisterListenerObject(UnityEngine.GameObject listenerObject);

        /// <summary>
        /// CueSheetの登録
        /// </summary>
        /// <param name="cueSheetInfo">CueSheet情報</param>
        public void RegisterCueSheet(CueSheetInfo cueSheetInfo);

        /// <summary>
        /// CueSheetの削除
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        public void RemoveCueSheet(string cueSheetName);

        /// <summary>
        /// 登録済の全てのCueSheetを削除
        /// </summary>
        public void RemoveAllCueSheet();

        /// <summary>
        /// 音声再生の一時停止
        /// </summary>
        public void Pause();

        /// <summary>
        /// 音声再生の一時停止解除
        /// </summary>
        public void Resume();

        /// <summary>
        /// 指定したCueを再生する
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="cueName">Cue名</param>
        /// <param name="option">再生オプション</param>
        public void Play(string cueSheetName, string cueName, SoundPlayOption option = null);

        /// <summary>
        /// 指定したCueを再生する(3D位置再生)
        /// </summary>
        /// <param name="gameObject">対象GameObject</param>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="cueName">Cue名</param>
        /// <param name="option">再生オプション</param>
        public void Play3d(GameObject gameObject, string cueSheetName, string cueName, ICriAdxApiService.SoundPlayOption option = null);

        /// <summary>
        /// 指定したCueSheetの再生を停止する
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="option">停止オプション</param>
        public void Stop(string cueSheetName, ICriAdxApiService.SoundStopOption option = null);

        /// <summary>
        /// 再生中のサウンドを全て止める
        /// </summary>
        /// <param name="option">停止オプション</param>
        public void StopAll(ICriAdxApiService.SoundStopOption option = null);

        /// <summary>
        /// DSPバスのスナップショットを切り替える
        /// </summary>
        /// <param name="snapshotName">スナップショット名</param>
        /// <param name="fadeTimeMs">フェードさせる時間</param>
        public void ChangeBusSnapshot(string snapshotName, int fadeTimeMs);

        /// <summary>
        /// 指定バスのボリューム取得
        /// </summary>
        /// <param name="busName">バス名</param>
        /// <returns></returns>
        public float GetBusVolume(string busName);

        /// <summary>
        /// 指定バスのボリューム設定
        /// </summary>
        /// <param name="busName">バス名</param>
        /// <param name="volume">ボリューム</param>
        public void SetBusVolume(string busName, float volume);

        /// <summary>
        /// 指定カテゴリのボリューム取得
        /// </summary>
        /// <param name="categoryName">カテゴリ名</param>
        /// <returns></returns>
        public float GetCategoryVolume(string categoryName);

        /// <summary>
        /// 指定カテゴリのボリューム設定
        /// </summary>
        /// <param name="categoryName">カテゴリ名</param>
        /// <param name="volume">ボリューム</param>
        public void SetCategoryVolume(string categoryName, float volume);

        /// <summary>
        /// AISACコントロール値の設定
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="controlName">AISACコントロール名</param>
        /// <param name="value">設定値</param>
        public void SetAisacControl(string cueSheetName, string controlName, float value);

        /// <summary>
        /// AISACコントロール値の設定
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="cueName">Cue名</param>
        /// <param name="controlName">AISACコントロール名</param>
        /// <param name="value">設定値</param>
        public void SetAisacControl(string cueSheetName, string cueName, string controlName, float value);

        /// <summary>
        /// イベントコールバックの設定
        /// </summary>
        /// <param name="tagName">タグ名</param>
        /// <param name="callback">コールバック</param>
        public void SetSequenceCallback(string tagName, Action callback);

        /// <summary>
        /// ビート同期イベントコールバックの設定
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="labelName">ラベル名</param>
        /// <param name="callback">コールバック</param>
        public void SetBeatSyncCallback(string cueSheetName, string labelName, Action callback);

        /// <summary>
        /// 遷移するブロックインデックスを設定する
        /// </summary>
        /// <param name="cueName">Cue名</param>
        /// <param name="nextBlockIndex">ブロックインデックス</param>
        public void SetNextBlockIndex(string cueName, int nextBlockIndex);

        /// <summary>
        /// スペクトラムアナライザの作成
        /// </summary>
        /// <param name="cueSheetName">CueSheet名</param>
        /// <param name="sampleCount">サンプル数</param>
        public void SetSpectrumAnalyzer(string cueSheetName, int sampleCount);

        /// <summary>
        /// スペクトラムアナライザから周波数データを取得する
        /// </summary>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル値に変換するか？</param>
        /// <returns></returns>
        public float[] GetSpectrumData(int sampleCount, bool isConvertDecibel);
    }
}
