/*******************************************************************************
The content of this file includes portions of the proprietary AUDIOKINETIC Wwise
Technology released in source code form as part of the game integration package.
The content of this file may not be used without valid licenses to the
AUDIOKINETIC Wwise Technology.
Note that the use of the game engine is subject to the Unity(R) Terms of
Service at https://unity3d.com/legal/terms-of-service
 
License Usage
 
Licensees holding valid licenses to the AUDIOKINETIC Wwise Technology may use
this file in accordance with the end user license agreement provided with the
software or, alternatively, in accordance with the terms contained
in a written agreement between you and Audiokinetic Inc.
Copyright (c) 2024 Audiokinetic Inc.
*******************************************************************************/

﻿#if UNITY_ANDROID && !UNITY_EDITOR
public partial class AkCommonUserSettings
{
	partial void SetSampleRate(AkPlatformInitSettings settings)
	{
		settings.uSampleRate = m_SampleRate;
	}
}
#endif

public class AkAndroidSettings : AkWwiseInitializationSettings.PlatformSettings
{
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
	private static void AutomaticPlatformRegistration()
	{
		if (UnityEditor.AssetDatabase.IsAssetImportWorkerProcess())
		{
			return;
		}

		RegisterPlatformSettingsClass<AkAndroidSettings>("Android");
	}
#endif // UNITY_EDITOR

	public AkAndroidSettings()
	{
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_PanningRule", false);
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelConfigType", false);
		SetUseGlobalPropertyValue("UserSettings.m_MainOutputSettings.m_ChannelConfig.m_ChannelMask", false);
	}

	protected override AkCommonUserSettings GetUserSettings()
	{
		return UserSettings;
	}

	protected override AkCommonAdvancedSettings GetAdvancedSettings()
	{
		return AdvancedSettings;
	}

	protected override AkCommonCommSettings GetCommsSettings()
	{
		return CommsSettings;
	}

	[System.Serializable]
	public class PlatformAdvancedSettings : AkCommonAdvancedSettings
	{
		public enum AudioAPI
		{
			None = 0,
			AAudio = 1 << 0,
			OpenSL_ES = 1 << 1,
			Default = ~0
		}

		[UnityEngine.Tooltip("Main audio API to use. Leave set to \"Default\" for the default audio sink.")]
		[AkEnumFlag(typeof(AudioAPI))]
		public AudioAPI m_AudioAPI = AudioAPI.Default;

		[UnityEngine.Tooltip("Used when hardware-preferred frame size and user-preferred frame size are not compatible. If true (default), the sound engine will initialize to a multiple of the HW setting, close to the user setting. If false, the user setting is used as is, regardless of the HW preference (might incur a performance hit).")]
		public bool m_RoundFrameSizeToHardwareSize = true;

		[UnityEngine.Tooltip("Use the lowest output latency possible for the current hardware. If true (default), the output audio device will be initialized in low-latency operation, allowing for more responsive audio playback on most devices. However, when operating in low-latency mode, some devices may have differences in audio reproduction. If false, the output audio device will be initialized without low-latency operation.")]
		public bool m_UseLowLatencyMode = true;

		public override void CopyTo(AkPlatformInitSettings settings)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			settings.eAudioAPI = (AkAudioAPI)m_AudioAPI;
			settings.bRoundFrameSizeToHWSize = m_RoundFrameSizeToHardwareSize;
			settings.bEnableLowLatency = m_UseLowLatencyMode;
#endif		
		}
	}

	[UnityEngine.HideInInspector]
	public AkCommonUserSettings UserSettings = new AkCommonUserSettings
	{
		m_MainOutputSettings = new AkCommonOutputSettings
		{
			m_PanningRule = AkCommonOutputSettings.PanningRule.Headphones,
			m_ChannelConfig = new AkCommonOutputSettings.ChannelConfiguration
			{
				m_ChannelConfigType = AkCommonOutputSettings.ChannelConfiguration.ChannelConfigType.Standard,
				m_ChannelMask = AkCommonOutputSettings.ChannelConfiguration.ChannelMask.SETUP_STEREO,
			},
		},
	};

	[UnityEngine.HideInInspector]
	public PlatformAdvancedSettings AdvancedSettings;

	[UnityEngine.HideInInspector]
	public AkCommonCommSettings CommsSettings;
}
