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

﻿#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class AkiOSPluginActivator
{
	static AkiOSPluginActivator()
	{
		if (UnityEditor.AssetDatabase.IsAssetImportWorkerProcess())
		{
			return;
		}

		AkPluginActivator.RegisterBuildTarget(UnityEditor.BuildTarget.iOS, new AkPluginActivator.PlatformConfiguration
		{
			WwisePlatformName = "iOS",
			PluginDirectoryName = "iOS",
			DSPDirectoryPath = "/iOS/DSP/",
			StaticPluginRegistrationName = "AkiOSPlugins",
			StaticPluginDefine = "AK_IOS",
			RequiresStaticPluginRegistration = true
		});
		AkPluginActivator.RegisterBuildTarget(UnityEditor.BuildTarget.tvOS, new AkPluginActivator.PlatformConfiguration
		{
			// For tvOS, we use the plugin info for iOS, since they share banks.
			WwisePlatformName = "iOS",
			PluginDirectoryName = "tvOS",
			DSPDirectoryPath = "/tvOS/DSP/",
			StaticPluginRegistrationName = "AktvOSPlugins",
			StaticPluginDefine = "AK_IOS",
			RequiresStaticPluginRegistration = true
		});

		var buildConfig = new AkBuildPreprocessor.PlatformConfiguration
		{
			WwisePlatformName = "iOS" // iOS and tvOS share the same banks
		};
		AkBuildPreprocessor.RegisterBuildTarget(UnityEditor.BuildTarget.iOS, buildConfig);
		AkBuildPreprocessor.RegisterBuildTarget(UnityEditor.BuildTarget.tvOS, buildConfig);
		WwiseSetupWizard.AddBuildTargetGroup(UnityEditor.BuildTargetGroup.iOS);
		WwiseSetupWizard.AddBuildTargetGroup(UnityEditor.BuildTargetGroup.tvOS);
	}
}
#endif