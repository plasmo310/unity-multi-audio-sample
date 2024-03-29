/****************************************************************************
 *
 * Copyright (c) CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
#if !UNITY_EDITOR && UNITY_WEBGL
#define CRI_UNSUPPORTED_OUTPUTPORT
#endif

using System;
using System.Runtime.InteropServices;

namespace CriWare {
	/**
	 * <summary>出力ポート</summary>
	 * <remarks>
	 * <para header='説明'>出力ポートは、音声の出力先の管理や制御を容易にするためのクラスです。<br/>
	 * メインの出力先とは別のデバイスでの再生や、プラットフォーム機能を用いた特殊な再生の時に役に立ちます。</para>
	 * </remarks>
	 */
	public class CriAtomExOutputPort : CriDisposable
	{
		/**
		 * <summary>出力ポートタイプ</summary>
		 * <remarks>
		 * <para header='説明'>出力ポートの種別を示す値です。<br/></para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.Config'/>
		 */
		public enum Type : System.Int32 {
			/**
			 * <summary>サウンドタイプ</summary>
			 * <remarks>
			 * <para header='説明'>通常の音声を再生する出力ポートタイプです。<br/></para>
			 * </remarks>
			 */
			Audio = 0,

			/**
			 * <summary>振動タイプ</summary>
			 * <remarks>
			 * <para header='説明'>オーディオベースの振動を再生する出力ポートタイプです。<br/>
			 * <br/></para>
			 * </remarks>
			 */
			Vibration = 1
		}

		/**
		 * <summary>出力ポートの名前の長さの最大値</summary>
		 * <remarks>
		 * <para header='説明'><see cref='CriWare.CriAtomExOutputPort.Config.name'/> に名前として指定できる文字列の最大長です。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.Config'/> 
		 */
		public const uint MaxNameLength = 64;

		/**
		 * <summary>出力ポート作成用コンフィグ構造体</summary>
		 * <remarks>
		 * <para header='説明'>出力ポートを作成するための構造体です。<br/>
		 * <see cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/> の引数に指定します。<br/></para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/>
		 */
		public struct Config {
			/**
			 * <summary>出力ポート名</summary>
			 * <remarks>
			 * <para header='説明'>出力ポートの名前を指定します。<br/></para>
			 * <para header='備考'>文字列の長さは <see cref='CriWare.CriAtomExOutputPort.MaxNameLength'/> 以下である必要があります。<br/>
			 * 一度指定したポート名をあとから変更することはできません。<br/></para>
			 * </remarks>
			 */
			public string name;

			/**
			 * <summary>出力ポートタイプ</summary>
			 * <remarks>
			 * <para header='説明'>出力ポートのタイプを指定します。</para>
			 * <para header='備考'>一度指定したポートタイプをあとから変更することはできません。<br/></para>
			 * </remarks>
			 */
			public CriAtomExOutputPort.Type type;

			/**
			 * <summary>デフォルト値のコンフィグ構造体を取得</summary>
			 * <returns>デフォルト値のコンフィグ構造体</returns>
			 * <remarks>
			 * <para header='説明'><see cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/> に設定するコンフィグ構造体のデフォルト値を取得します。</para>
			 * </remarks>
			 */
			public static Config Default() {
				var config = new Config();
				config.name = String.Empty;
				config.type = Type.Audio;
				return config;
			}
		}

		/**
		 * <summary>出力ポートオブジェクトが有効かどうか</summary>
		 * <returns>オブジェクトが有効かどうか</returns>
		 * <remarks>
		 * <para header='説明'>出力ポートオブジェクトが有効かどうか取得します。<br/>
		 * 生成済みオブジェクトの内部で保持しているネイティブハンドルがDispose済みかどうか判定できます。<br/></para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.Dispose'/>
		 */
		public bool isAvailable { get { return this.NativeHandle != IntPtr.Zero; } }

		/**
		 * <summary>出力ポートオブジェクト作成用ワーク領域サイズの計算</summary>
		 * <param name='config'>出力ポート作成用コンフィグ構造体</param>
		 * <returns>ワーク領域サイズ</returns>
		 * <remarks>
		 * <para header='説明'>出力ポートオブジェクトの作成に必要なワーク領域のサイズを計算します。<br/>
		 * 計算した値は本クラスのコンストラクタを呼び出した際に確保される、アンマネージドメモリのサイズに相当します。<br/>
		 * <br/>
		 * ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。<br/>
		 * ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。<br/></para>
		 * <para header='注意'><see cref='CriWare.CriAtomExOutputPort.Config.name'/> に指定する出力ポート名の長さは<br/>
		 * <see cref='CriWare.CriAtomExOutputPort.MaxNameLength'/> 以下である必要があります。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/>
		 */
		public Int32 CalculateWorkSize(CriAtomExOutputPort.Config config) {
			return NativeMethods.criAtomExOutputPort_CalculateWorkSize(config);
		}

		/**
		 * <summary>出力ポートオブジェクトの作成</summary>
		 * <param name='config'>出力ポート作成用コンフィグ構造体</param>
		 * <returns>出力ポートオブジェクト</returns>
		 * <remarks>
		 * <para header='説明'>出力ポートオブジェクトの作成を行います。<br/>
		 * 出力ポートはASRラックと紐付けられ、出力ポートが指定されたボイスはその出力ポートに紐付けられた<br/>
		 * ASRラックにて再生されるようになります。<br/></para>
		 * <code>CriAtomExOutputPort CreateOutputPort(int rackId) {
		 * 	var outputPortConfig = CriAtomExOutputPort.Config.Default();
		 * 	outputPortConfig.name = "SampleOutputPort1";
		 * 	var outputPort = new CriAtomExOutputPort(outputPortConfig);
		 * 
		 * 	// Set AsrRack ID
		 * 	outputPort.SetAsrRackId(rackId);
		 * 
		 * 	return outputPort;
		 * }</code>
		 * <para header='備考'>ACFファイルに設定された出力ポートオブジェクトは <see cref='CriAtomEx.RegisterAcf'/> などを用いて<br/>
		 * ACFファイルを登録したとき、ACF内に自動的に作成されるため、本関数で新たに作成する必要はありません。<br/>
		 * 上記で生成されたの出力ポートオブジェクトは <see cref='CriWare.CriAtomExAcf.GetOutputPort'/> で取得できます。<br/>
		 * そのため、本関数はアプリケーション上で新たに出力ポートオブジェクトが必要になった場合に使用してください。<br/>
		 * <br/>
		 * 出力ポートオブジェクトの生成に成功した場合は、本関数は生成した出力ポートオブジェクトを返します。<br/>
		 * 生成に失敗した場合は null を返します。<br/></para>
		 * <para header='注意'>本関数で作成された出力ポートオブジェクトには、 <see cref='CriWare.CriAtomExOutputPort.Config.type'/> で指定したタイプによって<br/>
		 * 以下のASRラックIDが初期値としてセットされています。<br/>
		 * - <see cref='CriWare.CriAtomExOutputPort.Type.Audio'/> を指定： <see cref='CriWare.CriAtomExAsrRack.defaultRackId'/>
		 * - <see cref='CriWare.CriAtomExOutputPort.Type.Vibration'/> を指定： <see cref='CriWare.CriAtomExAsrRack.IllegalRackId'/>
		 * .
		 * 出力ポートオブジェクトを使用する前に、必ず <see cref='CriWare.CriAtomExOutputPort.SetAsrRackId'/> <br/>
		 * で適切なASRラックを設定してください。<br/>
		 * <br/>
		 * 本APIで生成したオブジェクトは必ず <see cref='CriWare.CriAtomExOutputPort.Dispose'/> で破棄してください。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.Config'/>
		 * <seealso cref='CriWare.CriAtomExOutputPort.Dispose'/>
		 * <seealso cref='CriWare.CriAtomExAsrRack.rackId'/>
		 * <seealso cref='CriWare.CriAtomExAcf.GetOutputPort'/>
		 * <seealso cref='CriWare.CriAtomExPlayer.AddOutputPort'/>
		 * <seealso cref='CriWare.CriAtomExPlayer.AddPreferredOutputPort'/>
		 */
		public CriAtomExOutputPort(Config config) {
			this.NativeHandle = NativeMethods.criAtomExOutputPort_Create(ref config, IntPtr.Zero, 0);

			CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
		}

		internal CriAtomExOutputPort(IntPtr existingNativeHandle) {
			this.NativeHandle = existingNativeHandle;
			hasExistingNativeHandle = true;
			CriDisposableObjectManager.Register(this, CriDisposableObjectManager.ModuleType.Atom);
		}

		~CriAtomExOutputPort() {
			Dispose(false);
		}

		/**
		 * <summary>出力ポートオブジェクトの破棄</summary>
		 * <remarks>
		 * <para header='説明'>出力ポートオブジェクトの破棄を行います。<br/></para>
		 * <para header='備考'>以下のAPIを使用してプレーヤーに追加中の出力ポートオブジェクトはそのままでは破棄することができません。<br/>
		 * - <see cref='CriWare.CriAtomExPlayer.AddOutputPort'/>
		 * - <see cref='CriWare.CriAtomExPlayer.AddPreferredOutputPort'/>
		 * .
		 * <br/>
		 * この場合は以下のAPIを使用し、プレーヤーから取り外すことで出力ポートオブジェクトを破棄できます。<br/>
		 * - <see cref='CriWare.CriAtomExPlayer.RemoveOutputPort'/>
		 * - <see cref='CriWare.CriAtomExPlayer.RemovePreferredOutputPort'/>
		 * .</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/>
		 * <seealso cref='CriWare.CriAtomExOutputPort.isAvailable'/>
		 */
		public override void Dispose() {
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			CriDisposableObjectManager.Unregister(this);
			if (hasExistingNativeHandle) {
				this.NativeHandle = IntPtr.Zero;
			}

			if (isAvailable) {
				NativeMethods.criAtomExOutputPort_Destroy(NativeHandle);
				this.NativeHandle = IntPtr.Zero;
			}

			if (disposing) {
				GC.SuppressFinalize(this);
			}
		}

		/**
		 * <summary>ASRラックIDの指定</summary>
		 * <param name='rackId'>ASRラックID</param>
		 * <remarks>
		 * <para header='説明'>出力ポートにASRラック指定します。<br/>
		 * 出力ポートが指定されたボイスは、その出力ポートに指定されているASRラックで再生されます。</para>
		 * <para header='備考'>ACFファイル登録時に作成された出力ポートオブジェクト（<see cref='CriWare.CriAtomExAcf.GetOutputPort'/>）や<br/>
		 * <see cref='CriWare.CriAtomExOutputPort.CriAtomExOutputPort'/> で作成された<br/>
		 * 出力ポートオブジェクトには、必ず本関数で適切なASRラックを指定する必要があります。<br/>
		 * <br/>
		 * 出力ポートのタイプなどによって、指定できるASRラックに制限がある場合があります。<br/>
		 * 詳細に関しましてはマニュアルを参照してください。</para>
		 * <para header='注意'>本関数で出力ポートのASRラックIDを変更しても、既に再生されている音声には影響しません。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExAsrRack.rackId'/>
		 * <seealso cref='CriWare.CriAtomExAcf.GetOutputPort'/>
		 */
		public void SetAsrRackId(Int32 rackId) {
			NativeMethods.criAtomExOutputPort_SetAsrRackId(NativeHandle, rackId);
		}


		/**
		 * <summary>振動タイプの出力ポートのチャンネルレベルの設定</summary>
		 * <param name='channel'>チャンネルインデックス（0 = L, 1 = R）</param>
		 * <param name='level'>レベル(0 ~ 2.0)</param>
		 * <remarks>
		 * <para header='説明'>振動タイプの出力ポートに対し、振動デバイスの各チャンネルへの出力レベルを設定します。<br/></para>
		 * <para header='備考'>振動タイプの出力ポートは２チャンネルで動作しており、最終出力デバイスがモノラルの場合-3dBのダウンミックスが適用されます。<br/>
		 * この関数で設定した値は、音が再生中でも即時反映されます。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.SetMonauralMix'/>
		 */
		public void SetVibrationChannelLevel(Int32 channel, Single level) {
			NativeMethods.criAtomExOutputPort_SetVibrationChannelLevel(NativeHandle, channel, level);
		}

		/**
		 * <summary>振動タイプの出力ポートのモノラルミックス有無設定</summary>
		 * <param name='monauralMix'>モノラルミックス有無（CRI_TRUE = 有効, CRI_FALSE = 無効）</param>
		 * <remarks>
		 * <para header='説明'>振動タイプの出力ポートは２チャンネルで動作するため、入力されるボイスがステレオ以上の音声データか、<br/>
		 * 3Dパンが設定されている場合、その結果が振動デバイスの左右に伝わります。<br/>
		 * モノラルミックスを有効にすると、振動デバイスへ出力する前に一度モノラルにダウンミックスを行うことでそれらの影響をなくすことができます。<br/>
		 * <see cref='CriWare.CriAtomExOutputPort.SetVibrationChannelLevel'/> を使用して<br/>
		 * モノラルミックス後振動デバイスへ送られるレベルを設定することも可能です。<br/></para>
		 * <para header='備考'>この関数で設定した値は、音が再生中でも即時反映されます。</para>
		 * </remarks>
		 * <seealso cref='CriWare.CriAtomExOutputPort.SetVibrationChannelLevel'/>
		 */
		public void SetMonauralMix(Boolean monauralMix) {
			NativeMethods.criAtomExOutputPort_SetMonauralMix(NativeHandle, monauralMix);
		}

		internal IntPtr NativeHandle { get; private set; }
		private bool hasExistingNativeHandle = false;

		private partial class NativeMethods {
#if !CRI_ENABLE_HEADLESS_MODE && !CRI_UNSUPPORTED_OUTPUTPORT
			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern Int32 criAtomExOutputPort_CalculateWorkSize([In] CriAtomExOutputPort.Config config);

			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern IntPtr criAtomExOutputPort_Create(ref CriAtomExOutputPort.Config config, IntPtr work, Int32 workSize);

			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern void criAtomExOutputPort_Destroy(IntPtr outputPort);

			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern void criAtomExOutputPort_SetAsrRackId(IntPtr outputPort, Int32 rackId);

			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern void criAtomExOutputPort_SetVibrationChannelLevel(IntPtr outputPort, Int32 channel, Single level);

			[DllImport(CriWare.Common.pluginName, CallingConvention = CriWare.Common.pluginCallingConvention)]
			internal static extern void criAtomExOutputPort_SetMonauralMix(IntPtr outputPort, Boolean monauralMix);
#else
			internal static Int32 criAtomExOutputPort_CalculateWorkSize([In] CriAtomExOutputPort.Config config)
			{
				return default(Int32);
			}

			internal static IntPtr criAtomExOutputPort_Create(ref CriAtomExOutputPort.Config config, IntPtr work, Int32 workSize)
			{
				return default(IntPtr);
			}

			internal static void criAtomExOutputPort_Destroy(IntPtr outputPort)
			{
				return;
			}

			internal static void criAtomExOutputPort_SetAsrRackId(IntPtr outputPort, Int32 rackId)
			{
				return;
			}

			internal static void criAtomExOutputPort_SetVibrationChannelLevel(IntPtr outputPort, Int32 channel, Single level)
			{
				return;
			}

			internal static void criAtomExOutputPort_SetMonauralMix(IntPtr outputPort, Boolean monauralMix)
			{
				return;
			}
#endif
		}
	}
} // namespace CriWare