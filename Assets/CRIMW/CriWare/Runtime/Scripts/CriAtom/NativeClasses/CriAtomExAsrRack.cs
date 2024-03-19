/****************************************************************************
 *
 * Copyright (c) 2016 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/*---------------------------
 * Asr Support Defines
 *---------------------------*/
#if !UNITY_PSP2
#define CRIWARE_SUPPORT_ASR
#endif

using System;
using System.Runtime.InteropServices;
using CriAtomExAsrRackId = System.Int32;

/*==========================================================================
 *      CRI Atom Native Wrapper
 *=========================================================================*/
/**
 * \addtogroup CRIATOM_NATIVE_WRAPPER
 * @{
 */

namespace CriWare {

/**
 * <summary>ASRラック</summary>
 */
public partial class CriAtomExAsrRack : CriDisposable
{
	#region Data Types

	/**
	 * <summary>ミキサーのスピーカーマッピング種別</summary>
	 * <remarks>
	 * <para header='説明'>CriAtomExAsrRack のスピーカーマッピングを指定します。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack.Config.speakerMapping'/>
	 */
	public enum SpeakerMapping : System.Int32 {
		Auto = 0,    /**< 自動設定 */
		Monaural,    /**< 1ch */
		Stereo,      /**< 2ch */
		Ch5_1,       /**< 5.1ch */
		Ch7_1,       /**< 7.1ch */
		Ch5_1_2,     /**< Tentative: 5.1.2ch */
		Ch7_1_4,     /**< Tentative: 7.1.4ch */
		Ambisonics1p = 7,	/**< Tentative: 1st order Ambisonics */
		Ambisonics2p = 8,	/**< Tentative: 2st order Ambisonics */
		Ambisonics3p = 9,	/**< Tentative: 3st order Ambisonics */
	}

	/**
	 * <summary>ASRラック作成用コンフィグ構造体</summary>
	 * <remarks>
	 * <para header='説明'>CriAtomExAsrRack の動作仕様を指定するための構造体です。<br/>
	 * モジュール作成時（CriWare.CriAtomExAsrRack::CriAtomExAsrRack 関数）に引数として本構造体を指定します。<br/></para>
	 * <para header='備考'>CriWare.CriAtomExAsrRack::defaultConfig で取得したデフォルトコンフィギュレーションを必要に応じて変更して
	 * ください。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::CriAtomExAsrRack'/>
	 * <seealso cref='CriAtomExAsrRack::defaultConfig'/>
	 */
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct Config
	{
		/**
		 * <summary>サーバ処理の実行頻度</summary>
		 * <remarks>
		 * <para header='説明'>サーバ処理を実行する頻度を指定します。</para>
		 * <para header='注意'>CriWareInitializer に指定した CriAtomConfig::serverFrequency と同じ値を指定してください。</para>
		 * </remarks>
		 */
		public float serverFrequency;

		/**
		 * <summary>バス数</summary>
		 * <remarks>
		 * <para header='説明'>ASRが作成するバスの数を指定します。<br/>
		 * バスはサウンドのミックスや、DSPエフェクトの管理等を行います。</para>
		 * </remarks>
		 */
		public int numBuses;

		/**
		 * <summary>出力チャンネル数</summary>
		 * <remarks>
		 * <para header='説明'>ASRラックの出力チャンネル数を指定します。<br/>
		 * パン3Dもしくは3Dポジショニング機能を使用する場合は6ch以上を指定します。</para>
		 * </remarks>
		 */
		public int outputChannels;

		/**
		 * <summary>ミキサーのスピーカーマッピング</summary>
		 * <remarks>
		 * <para header='説明'>ASRラックのスピーカーマッピングを指定します。<br/></para>
		 * </remarks>
		 */
		public SpeakerMapping speakerMapping;

		/**
		 * <summary>出力サンプリングレート</summary>
		 * <remarks>
		 * <para header='説明'>ASRラックの出力および処理過程のサンプリングレートを指定します。<br/>
		 * 通常、ターゲット機のサウンドデバイスのサンプリングレートを指定します。</para>
		 * <para header='備考'>低くすると処理負荷を下げることができますが音質が落ちます。</para>
		 * </remarks>
		 */
		public int outputSamplingRate;

		/**
		 * <summary>サウンドレンダラタイプ</summary>
		 * <remarks>
		 * <para header='説明'>ASRラックの出力先サウンドレンダラの種別を指定します。<br/>
		 * soundRendererType に CriAtomEx.SoundRendererType.Native を指定した場合、
		 * 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。</para>
		 * </remarks>
		 */
		public CriAtomEx.SoundRendererType soundRendererType;

		/**
		 * <summary>出力先ASRラックID</summary>
		 * <remarks>
		 * <para header='説明'>ASRラックの出力先ASRラックIDを指定します。<br/>
		 * soundRendererType に CriAtomEx.SoundRendererType.Asr を指定した場合のみ有効です。</para>
		 * </remarks>
		 */
		public int outputRackId;

		/**
		 * <summary>プラットフォーム固有のパラメータへのポインタ</summary>
		 * <remarks>
		 * <para header='説明'>プラットフォーム固有のパラメータへのポインタを指定します。<br/>
		 * CriAtomExAsrRack::CriAtomExAsrRack 関数の引数に用いる場合は、第二引数の
		 * PlatformContext で上書きされるため、 IntPtr.Zero を指定してください。</para>
		 * </remarks>
		 */
		public IntPtr context;

		/**
		 * <summary>デフォルト値のコンフィグ構造体を取得</summary>
		 * <returns>デフォルト値のコンフィグ構造体</returns>
		 * <remarks>
		 * <para header='説明'><see cref='CriWare.CriAtomExAsrRack.CriAtomExAsrRack'/> に設定するコンフィグ構造体のデフォルト値を取得します。</para>
		 * </remarks>
		 */
		public static Config Default() {
			var config = new Config();
			SetDefaultConfig(ref config);
			return config;
		}
	}

	/**
	 * \deprecated
	 * 削除予定の非推奨APIです。
	 * <see cref='CriWare.CriAtomExAsrRack.IPlatformConfig'/> を継承した機種固有構造体の使用を検討してください。
	 * <summary>ASRラック作成用プラットフォーム固有コンフィグ構造体</summary>
	 * <remarks>
	 * <para header='説明'>CriAtomExAsrRack の動作仕様を指定するための構造体です。<br/>
	 * モジュール作成時（CriWare.CriAtomExAsrRack::CriAtomExAsrRack 関数）に引数として本構造体を指定します。<br/>
	 * 詳細についてはプラットフォーム毎のマニュアルを参照してください。</para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::CriAtomExAsrRack'/>
	 */
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct PlatformConfig
	{
	#if !UNITY_EDITOR && UNITY_PS4
		public int userId;
		public CriWarePS4.AudioPortType portType;
		public CriWarePS4.AudioPortAttribute portAttr;
	#elif !UNITY_EDITOR && UNITY_PS5
		public int userId;
		public CriWarePS5.AudioPortType portType;
		public uint portFlag;
		public uint portAttr;
	#elif !UNITY_EDITOR && UNITY_SWITCH
		public UInt32 npadId;
	#else
		public byte reserved;
	#endif
	}

	/**
	 * <summary>パフォーマンス情報</summary>
	 * <remarks>
	 * <para header='説明'>パフォーマンス情報を取得するための構造体です。<br/>
	 * CriWare.CriAtomExAsrRack::GetPerformanceInfo 、 CriWare.CriAtomExAsrRack::GetPerformanceInfoByRackId 関数で利用します。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfo'/>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfoByRackId'/>
	 */
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct PerformanceInfo
	{
		public UInt32 processCount;            /**< 信号生成処理回数 */
		public UInt32 lastProcessTime;         /**< 処理時間の最終計測値（マイクロ秒単位） */
		public UInt32 maxProcessTime;          /**< 処理時間の最大値（マイクロ秒単位） */
		public UInt32 averageProcessTime;      /**< 処理時間の平均値（マイクロ秒単位） */
		public UInt32 lastProcessInterval;     /**< 処理間隔の最終計測値（マイクロ秒単位） */
		public UInt32 maxProcessInterval;      /**< 処理間隔の最大値（マイクロ秒単位） */
		public UInt32 averageProcessInterval;  /**< 処理間隔の平均値（マイクロ秒単位） */
		public UInt32 lastProcessSamples;      /**< 単位処理で生成されたサンプル数の最終計測値 */
		public UInt32 maxProcessSamples;       /**< 単位処理で生成されたサンプル数の最大値 */
		public UInt32 averageProcessSamples;   /**< 単位処理で生成されたサンプル数の平均値 */
	}
	#endregion

	/**
	 * <summary>ASRラック作成用プラットフォーム固有コンフィグ構造体インターフェース</summary>
	 * <remarks>
	 * <para header='説明'>CriAtomExAsrRack の動作仕様を指定するための構造体です。<br/>
	 * モジュール作成時（CriWare.CriAtomExAsrRack::CriAtomExAsrRack 関数）に引数として<br/>
	 * 本インターフェースを継承した機種固有構造体を指定します。<br/>
	 * 詳細についてはプラットフォーム毎のマニュアルを参照してください。</para>
	 * </remarks>
	 * <seealso cref='CriWare.CriAtomExAsrRack.CriAtomExAsrRack'/>
	*/
	public interface IPlatformConfig {
		/**
		 * <summary>本機種固有構造体が有効なプラットフォームかどうか</summary>
		 * <remarks>
		 * <para header='説明'>現在のプラットフォームにおいてインターフェースを継承している本構造体が、有効なプラットフォームかどうか確認します。<br/>
		 * 本APIが false を返す場合、この構造体を使用して CriAtomExAsrRack のコンストラクタを呼ぶことはできません。<br/></para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExAsrRack.CriAtomExAsrRack'/>
		*/
		bool IsSupportedPlatform();
	}

	/**
	 * <summary>ASRラックの作成</summary>
	 * <param name='config'>コンフィグ構造体</param>
	 * <param name='platformConfig'>プラットフォーム固有パラメータ構造体</param>
	 * <returns>ASRラック</returns>
	 * <remarks>
	 * <para header='説明'>ASRラックを作成します。<br/>
	 * 本関数で作成したASRラックは、必ず Dispose 関数で破棄してください。<br/>
	 * プラットフォーム固有パラメータを使用しない場合は platformConfig にnullを指定してください。<br/></para>
	 * </remarks>
	 */
	public CriAtomExAsrRack(Config config, IPlatformConfig platformConfig)
	{
	#if CRIWARE_SUPPORT_ASR
		IntPtr platformPtr = IntPtr.Zero;
		if (platformConfig != null) {
			if (!platformConfig.IsSupportedPlatform()) {
				throw new InvalidOperationException("[CRIWARE] Not Supported PlatfromConfig.");
			}
			var platformStructType = platformConfig.GetType();
			platformPtr = Marshal.AllocHGlobal(Marshal.SizeOf(platformStructType));
			Marshal.StructureToPtr(platformConfig, platformPtr, false);
			config.context = platformPtr;
		}

		this._rackId = criAtomExAsrRack_Create(config, IntPtr.Zero, 0);

		if (platformPtr != IntPtr.Zero) {
			Marshal.FreeHGlobal(platformPtr);
		}
		CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
	#else
		this._rackId = IllegalRackId;
	#endif
	}

	/**
	 * \deprecated
	 * 削除予定の非推奨APIです。
	 * <see cref='CriWare.CriAtomExAsrRack.CriAtomExAsrRack(Config, IPlatformConfig)'/> の使用を検討してください。
	 * <summary>ASRラックの作成</summary>
	 * <param name='config'>コンフィグ構造体</param>
	 * <param name='platformConfig'>プラットフォーム固有パラメータ構造体</param>
	 * <returns>ASRラック</returns>
	 * <remarks>
	 * <para header='説明'>ASRラックを作成します。<br/>
	 * 本関数で作成したASRラックは、必ず Dispose 関数で破棄してください。</para>
	 * </remarks>
	 */
	[Obsolete("Use CriAtomExAsrRack.CriAtomExAsrRack(Config config, IPlatformConfig platformConfig)")]
	public CriAtomExAsrRack(Config config, PlatformConfig platformConfig)
	{
	#if CRIWARE_SUPPORT_ASR
		this._rackId = criAtomUnityAsrRack_Create(ref config, ref platformConfig);
		if (config.context != IntPtr.Zero) {
			Marshal.FreeHGlobal(config.context);
		}
		if (this._rackId == IllegalRackId) {
			throw new Exception("CriAtomExAsrRack() failed.");
		}

		CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
	#else
		this._rackId = IllegalRackId;
	#endif
	}

	public CriAtomExAsrRack(CriAtomExAsrRackId existingRackId)
	{
	#if CRIWARE_SUPPORT_ASR
		if (existingRackId == IllegalRackId) {
			throw new Exception("Illegal rack id.");
		}
	#endif
		this._rackId = existingRackId;
		this.hasExistingRackId = true;
		CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
	}

	/**
	 * <summary>DSPバス設定のアタッチ</summary>
	 * <param name='settingName'>DSPバス設定の名前</param>
	 * <remarks>
	 * <para header='説明'>DSPバス設定からDSPバスを構築してASRラックにアタッチします。<br/>
	 * 本関数を実行するには、あらかじめ CriAtomEx::RegisterAcf
	 * 関数でACF情報を登録しておく必要があります<br/></para>
	 * <para header='注意'>本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::DetachDspBusSetting'/>
	 * <seealso cref='CriAtomEx::RegisterAcf'/>
	 */
	public void AttachDspBusSetting(string settingName)
	{
		criAtomExAsrRack_AttachDspBusSetting(this.rackId, settingName, IntPtr.Zero, 0);
	}

	/**
	 * <summary>DSPバス設定のデタッチ</summary>
	 * <remarks>
	 * <para header='説明'>DSPバス設定をデタッチします。<br/></para>
	 * <para header='注意'>本関数は完了復帰型の関数です。<br/>
	 * 本関数を実行すると、しばらくの間Atomライブラリのサーバ処理がブロックされます。<br/>
	 * 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
	 * 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::AttachDspBusSetting'/>
	 */
	public void DetachDspBusSetting()
	{
		criAtomExAsrRack_DetachDspBusSetting(this.rackId);
	}

	/**
	 * <summary>DSPバススナップショットの適用</summary>
	 * <param name='snapshotName'>DSPバススナップショット名</param>
	 * <param name='timeMs'>スナップショットが完全に反映されるまでの時間（ミリ秒）</param>
	 * <remarks>
	 * <para header='説明'>DSPバススナップショットをASRラックに適用します。<br/>
	 * 本関数を呼び出すとスナップショットのパラメータに変化します。
	 * 完全に変化を終えるまでに、timeMsミリ秒かかります。</para>
	 * </remarks>
	 */
	public void ApplyDspBusSnapshot(string snapshotName, int timeMs)
	{
		criAtomExAsrRack_ApplyDspBusSnapshot(this.rackId, snapshotName, timeMs);
	}

	/**
	 * <summary>スナップショット名の取得</summary>
	 * <param name='rackId'>ラックのID</param>
	 * <returns>スナップショット名</returns>
	 * <remarks>
	 * <para header='説明'>現在設定されているスナップショット名を取得します。設定されていない場合、nullを返しします。<br/></para>
	 * </remarks>
	 */
	public static string GetAppliedDspBusSnapshotName(CriAtomExAsrRackId rackId)
	{
		string snapshotName;
		IntPtr ptr = criAtomExAsrRack_GetAppliedDspBusSnapshotName(rackId);
		if (ptr == IntPtr.Zero) {
			return null;
		}
		snapshotName = Marshal.PtrToStringAnsi(ptr);
		return snapshotName;
	}

	/**
	 * <summary>スナップショット名の取得</summary>
	 * <returns>スナップショット名</returns>
	 * <remarks>
	 * <para header='説明'>現在設定されているスナップショット名を取得します。設定されていない場合、nullを返しします。<br/></para>
	 * </remarks>
	 */
	public string GetAppliedDspBusSnapshotName()
	{
		string snapshotName;
		IntPtr ptr = criAtomExAsrRack_GetAppliedDspBusSnapshotName(this.rackId);
		if (ptr == IntPtr.Zero) {
			return null;
		}
		snapshotName = Marshal.PtrToStringAnsi(ptr);
		return snapshotName;
	}

	/**
	 * <summary>ASRラックのパフォーマンス情報を取得</summary>
	 * <returns>ASRラックのパフォーマンス情報</returns>
	 * <remarks>
	 * <para header='説明'>現在のASRラックインスタンスからパフォーマンス情報を取得します。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::ResetPerformanceMonitor'/>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfoByRackId'/>
	 */
	public PerformanceInfo GetPerformanceInfo()
	{
		PerformanceInfo info = new PerformanceInfo();
		if(this._rackId < 0) {
			UnityEngine.Debug.LogError("[CRIWARE] This ASR Rack is not initialized.");
			return info;
		}

		criAtomExAsrRack_GetPerformanceInfo(this._rackId, out info);
		return info;
	}

	/**
	 * <summary>ASRラックのパフォーマンス情報を取得</summary>
	 * <param name='rackId'>ラックのID</param>
	 * <returns>ASRラックのパフォーマンス情報</returns>
	 * <remarks>
	 * <para header='説明'>指定したIDのASRラックのパフォーマンス情報を取得します。<br/>
	 * ラックIDを指定しない場合、ライブラリ初期化時に生成されるデフォルトASRラックのパフォーマンス情報が返されます。<br/>
	 * 不正なラックIDが指定された場合、メンバー変数の値がすべて0の構造体が返されます。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::ResetPerformanceMonitorByRackId'/>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfo'/>
	 */
	public static PerformanceInfo GetPerformanceInfoByRackId(CriAtomExAsrRackId rackId = CriAtomExAsrRack.defaultRackId)
	{
		PerformanceInfo info = new PerformanceInfo();
		criAtomExAsrRack_GetPerformanceInfo(rackId, out info);
		return info;
	}

	/**
	 * <summary>ASRラックのパフォーマンス計測をリセット</summary>
	 * <remarks>
	 * <para header='説明'>現在のASRラックインスタンスのパフォーマンス計測をリセットします。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfo'/>
	 * <seealso cref='CriAtomExAsrRack::ResetPerformanceMonitorByRackId'/>
	 */
	public void ResetPerformanceMonitor()
	{
		criAtomExAsrRack_ResetPerformanceMonitor(this._rackId);
	}

	/**
	 * <summary>ASRラックのパフォーマンス計測をリセット</summary>
	 * <param name='rackId'>ラックのID</param>
	 * <remarks>
	 * <para header='説明'>指定したIDのASRラックのパフォーマンス計測をリセットします。<br/>
	 * ラックIDを指定しない場合、ライブラリ初期化時に生成されるデフォルトASRラックのパフォーマンス情報がリセットされます。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::GetPerformanceInfoByRackId'/>
	 * <seealso cref='CriAtomExAsrRack::ResetPerformanceMonitor'/>
	 */
	public static void ResetPerformanceMonitorByRackId(CriAtomExAsrRackId rackId = CriAtomExAsrRack.defaultRackId)
	{
		criAtomExAsrRack_ResetPerformanceMonitor(rackId);
	}

	/**
	 * <summary>名前指定によるAISACコントロール値設定</summary>
	 * <param name='rackId'>ラックのID</param>
	 * <param name='controlName'>AISACコントロール名</param>
	 * <param name='value'>AISACコントロール値</param>
	 * <remarks>
	 * <para header='説明'>名前指定でAISACコントロール値を設定します。</para>
	 * </remarks>
	 */
	public static void SetAisacControl(CriAtomExAsrRackId rackId, string controlName, float value)
	{
		criAtomExAsrRack_SetAisacControlByName(rackId, controlName, value);
	}

	/**
	 * <summary>ID指定によるAISACコントロール値設定</summary>
	 * <param name='rackId'>ラックのID</param>
	 * <param name='controlId'>AISACコントロールID</param>
	 * <param name='value'>AISACコントロール値</param>
	 * <remarks>
	 * <para header='説明'>ID指定でAISACコントロール値を設定します。</para>
	 * </remarks>
	 */
	public static  void SetAisacControl(CriAtomExAsrRackId rackId, int controlId, float value)
	{
		criAtomExAsrRack_SetAisacControlById(rackId, (ushort)controlId, value);
	}

	/**
	 * <summary>デフォルトパラメーターをセット</summary>
	 * <param name='config'>初期化コンフィグ</param>
	 * <remarks>
	 * <para header='説明'><see cref='CriWare.CriAtomExAsrRack.CriAtomExAsrRack'/> で使用する初期化コンフィグにデフォルトパラメーターをセットします。</para>
	 * </remarks>
	 */
	public static void SetDefaultConfig(ref Config config) {
		criAtomExAsrRack_SetDefaultConfig_Macro(ref config);
	}

	/**
	 * <summary>ASRラックの破棄</summary>
	 * <remarks>
	 * <para header='説明'>ASRラックを破棄します。</para>
	 * </remarks>
	 */
	public override void Dispose()
	{
	#if CRIWARE_SUPPORT_ASR
		CriDisposableObjectManager.Unregister(this);

		if (this._rackId != -1 && !this.hasExistingRackId) {
			criAtomExAsrRack_Destroy(this._rackId);
		}
		this._rackId = IllegalRackId;
	#endif
		GC.SuppressFinalize(this);
	}

	/**
	 * <summary>ASRラックIDの取得</summary>
	 * <remarks>
	 * <para header='説明'>ASRラックオブジェクトのIDを取得します。</para>
	 * </remarks>
	 */
	public CriAtomExAsrRackId rackId {
		get { return this._rackId; }
	}

	#region Static Properties
	/**
	 * \deprecated
	 * 削除予定の非推奨APIです。
	 * <see cref='CriWare.CriAtomExAsrRack.Config.Default'/> の使用を検討してください。
	 * <summary>デフォルトコンフィギュレーション</summary>
	 * <remarks>
	 * <para header='説明'>デフォルトコンフィグです。</para>
	 * <para header='備考'>本プロパティで取得したデフォルトコンフィギュレーションを必要に応じて変更して
	 * CriWare.CriAtomExAsrRack::CriAtomExAsrRack 関数に指定してください。<br/></para>
	 * </remarks>
	 * <seealso cref='CriAtomExAsrRack::CriAtomExAsrRack'/>
	 */
	[Obsolete("Use CriAtomExAsrRack.Config.Default")]
	public static Config defaultConfig {
		get {
			Config config;
			config.serverFrequency = 60.0f;
			config.numBuses = 8;
			config.soundRendererType = CriAtomEx.SoundRendererType.Native;
			config.outputRackId = 0;
			config.context = System.IntPtr.Zero;
			config.speakerMapping = SpeakerMapping.Auto;
	#if !UNITY_EDITOR && UNITY_PS4
			config.outputChannels = 8;
			config.outputSamplingRate = 48000;
	#elif !UNITY_EDITOR && UNITY_IOS || UNITY_ANDROID
			config.outputChannels = 2;
			config.outputSamplingRate = 44100;
	#elif !UNITY_EDITOR && UNITY_PSP2
			config.outputChannels = 2;
			config.outputSamplingRate = 48000;
	#else
			config.outputChannels = 6;
			config.outputSamplingRate = 48000;
	#endif
			return config;
		}
	}

	/**
	 * <summary>デフォルトASRラックID</summary>
	 * <remarks>
	 * <para header='説明'>デフォルトのASRラックIDです。
	 * 通常出力に戻す場合や生成したASRラックを破棄する場合には、各種プレーヤに対して
	 * この定数を利用してASRラックIDの指定を行ってください。</para>
	 * </remarks>
	 * <seealso cref='CriAtomExPlayer::SetAsrRackId'/>
	 * <seealso cref='CriMana::Player::SetAsrRackId'/>
	 */
	public const CriAtomExAsrRackId defaultRackId = 0;

	/**
	 * <summary>不正なラックID</summary>
	 * <remarks>
	 * <para header='説明'>デフォルトのASRラックIDです。
	 * ASRラックの生成に失敗した場合に返る値です。</para>
	 * </remarks>
	 */
	public const CriAtomExAsrRackId IllegalRackId = -1;
	#endregion


	#region internal members
	~CriAtomExAsrRack()
	{
		this.Dispose();
	}

	private CriAtomExAsrRackId _rackId = -1;
	private bool hasExistingRackId = false;
	#endregion

	#region DLL Import
	#if CRIWARE_SUPPORT_ASR

	#if !CRIWARE_ENABLE_HEADLESS_MODE
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern CriAtomExAsrRackId criAtomExAsrRack_Create(in Config config, IntPtr work, System.Int32 work_size);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern Int32 criAtomUnityAsrRack_Create([In] ref Config config, [In] ref PlatformConfig platformConfig);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_Destroy(Int32 rackId);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_AttachDspBusSetting(Int32 rackId, string setting, IntPtr work, Int32 workSize);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_DetachDspBusSetting(Int32 rackId);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern IntPtr criAtomExAsrRack_GetAppliedDspBusSnapshotName(int rackId);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_ApplyDspBusSnapshot(Int32 rackId, string snapshotName, Int32 timeMs);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_SetDefaultConfig_Macro(ref Config config);
	#else
	private static CriAtomExAsrRackId criAtomExAsrRack_Create(in Config config, IntPtr work, System.Int32 work_size) { return IllegalRackId; }
	private static Int32 criAtomUnityAsrRack_Create([In] ref Config config, [In] ref PlatformConfig platformConfig) { return 0; }
	private static void criAtomExAsrRack_Destroy(Int32 rackId) { }
	private static void criAtomExAsrRack_AttachDspBusSetting(Int32 rackId, string setting, IntPtr work, Int32 workSize) { }
	private static void criAtomExAsrRack_DetachDspBusSetting(Int32 rackId) { }
	private static void criAtomExAsrRack_ApplyDspBusSnapshot(Int32 rackId, string snapshotName, Int32 timeMs) { }
	private static IntPtr criAtomExAsrRack_GetAppliedDspBusSnapshotName(int rackId) { return IntPtr.Zero; }
	private static void criAtomExAsrRack_SetDefaultConfig_Macro(ref Config config) { }
	#endif

	#if !CRIWARE_ENABLE_HEADLESS_MODE && !UNITY_WEBGL
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_GetPerformanceInfo(Int32 rackId, out PerformanceInfo perfInfo);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_ResetPerformanceMonitor(Int32 rackId);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_SetAisacControlById(Int32 rackId, ushort controlId, float value);
	[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
	private static extern void criAtomExAsrRack_SetAisacControlByName(Int32 rackId, string controlName, float value);
	#else
	private static void criAtomExAsrRack_GetPerformanceInfo(Int32 rackId, out PerformanceInfo perfInfo) { perfInfo = new PerformanceInfo(); }
	private static void criAtomExAsrRack_ResetPerformanceMonitor(Int32 rackId) { }
	private static void criAtomExAsrRack_SetAisacControlById(Int32 rackId, ushort controlId, float value) { }
	private static void criAtomExAsrRack_SetAisacControlByName(Int32 rackId, string controlName, float value) { }
	#endif

	#endif
	#endregion
}

} //namespace CriWare
/**
 * @}
 */

/* --- end of file --- */
