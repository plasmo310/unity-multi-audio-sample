///<summary></summary>
namespace UnityCriSample_acf {
	namespace Category {
		///<summary></summary>
		public enum SoundCategory {
			///<summary></summary>
			BGM = 0,
			///<summary></summary>
			SE = 1,
		}
	}
	public enum AisacControl {
		///<summary></summary>
		Battle = 0,
		///<summary></summary>
		AisacControl_01 = 1,
		///<summary></summary>
		AisacControl_02 = 2,
		///<summary></summary>
		AisacControl_03 = 3,
		///<summary></summary>
		AisacControl_04 = 4,
		///<summary></summary>
		AisacControl_05 = 5,
		///<summary></summary>
		AisacControl_06 = 6,
		///<summary></summary>
		AisacControl_07 = 7,
		///<summary></summary>
		AisacControl_08 = 8,
		///<summary></summary>
		AisacControl_09 = 9,
		///<summary></summary>
		AisacControl_10 = 10,
		///<summary></summary>
		AisacControl_11 = 11,
		///<summary></summary>
		AisacControl_12 = 12,
		///<summary></summary>
		AisacControl_13 = 13,
		///<summary></summary>
		AisacControl_14 = 14,
		///<summary></summary>
		AisacControl_15 = 15,
	}
	public enum VoiceLimitGroup {
		///<summary></summary>
		VoiceLimitGroup_0,
	}
	namespace DspSetting {
		///<summary></summary>
		public enum DspBusSetting_0 {
			///<summary></summary>
			Normal,
			///<summary></summary>
			BGM_Reverb,
			///<summary></summary>
			BGM_Distortion,
			///<summary></summary>
			SE_Reverb,
			///<summary></summary>
			SE_Distortion,
		}
	}
	public enum DspBusName {
		BGMDistortion,
		BGMReverb,
		SEDistortion,
		SEReverb,
	}
	public enum GameVariable {
		///<summary></summary>
		Default,
	}
	namespace Selector {
		///<summary></summary>
		public enum MusicType {
			///<summary></summary>
			Normal,
			///<summary></summary>
			Special,
		}
	}
	public class UnityCriSample_acf {
		public const int REACTNUM = (1);
	}
}
