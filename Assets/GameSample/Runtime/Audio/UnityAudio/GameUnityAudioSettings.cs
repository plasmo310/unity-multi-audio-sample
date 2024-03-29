using System.Collections.Generic;
using System.Linq;
using AudioLib.UnityAudio;

namespace GameSample.Audio
{
    /// <summary>
    /// ゲーム内サウンド設定
    /// </summary>
    public class GameUnityAudioSettings : IGameAudioSettings
    {
        /// <summary>
        /// SoundEvent名
        /// </summary>
        public string SoundEventName_BgmSpaceWould => UnityAudio.AudioName.BgmSpaceWould;
        public string SoundEventName_BgmShotThunder => UnityAudio.AudioName.BgmShotThunder;
        public string SoundEventName_BgmAtomChain => UnityAudio.AudioName.BgmAtomChain;
        public string SoundEventName_SeAttack => UnityAudio.AudioName.SeAttack;
        public string SoundEventName_SeMove => UnityAudio.AudioName.SeMove;

        /// <summary>
        /// SoundSheet名
        /// </summary>
        public string SoundSheetName_Bgm => UnityAudio.SoundSheetName.Bgm;
        public string SoundSheetName_Se => UnityAudio.SoundSheetName.Se;

        /// <summary>
        /// GameParameter名
        /// </summary>
        public string GameParameterName_Battle => null;

        /// <summary>
        /// ビート同期ラベル名
        /// </summary>
        public string BeatSyncLabelName_AtomChain => null;

        /// <summary>
        /// カスタムイベント名
        /// </summary>
        public string CustomEventName_StartAtomChainMainLoop => null;

        /// <summary>
        /// UnityAudio固有
        /// </summary>
        public static class UnityAudio
        {
            /// <summary>
            /// サウンドシート名
            /// </summary>
            public static class SoundSheetName
            {
                public const string Bgm = "BGM";
                public const string Se = "SE";
            }

            /// <summary>
            /// サウンドシート情報
            /// </summary>
            public static IUnityAudioApiService.SoundSheetInfo[] SoundSheetInfoArray = new IUnityAudioApiService.SoundSheetInfo[]
            {
                new IUnityAudioApiService.SoundSheetInfo()
                {
                    Name = SoundSheetName.Bgm,
                    IsPlayLoop = true,
                },
                new IUnityAudioApiService.SoundSheetInfo()
                {
                    Name = SoundSheetName.Se,
                    IsPlayLoop = false,
                }
            };

            /// <summary>
            /// オーディオファイル名
            /// </summary>
            public static class AudioName
            {
                public static readonly string BgmSpaceWould = "BGM-SpaceWould";
                public static readonly string BgmShotThunder = "BGM-ShotThunder";
                public static readonly string BgmAtomChain = "BGM-AtomChain";
                public static readonly string SeAttack = "SE-Attack";
                public static readonly string SeMove = "SE-Move";
            }

            /// <summary>
            /// サウンドシートに紐づくオーディオ情報
            /// </summary>
            public static readonly Dictionary<string, string[]> SoundSheetAudioDictionary = new ()
            {
                {
                    SoundSheetName.Bgm,
                    new[]
                    {
                        AudioName.BgmSpaceWould,
                        AudioName.BgmShotThunder,
                        AudioName.BgmAtomChain,
                    }
                },
                {
                    SoundSheetName.Se,
                    new[]
                    {
                        AudioName.SeAttack,
                        AudioName.SeMove
                    }
                },
            };

            /// <summary>
            /// Audio名から対象のSoundSheet名を取得する
            /// </summary>
            /// <param name="audioName"></param>
            /// <returns></returns>
            public static string GetSoundSheetName(string audioName)
            {
                foreach (var soundSheetName in SoundSheetAudioDictionary.Keys)
                {
                    var eventNameArray = SoundSheetAudioDictionary[soundSheetName];
                    if (eventNameArray.Contains(audioName))
                    {
                        return soundSheetName;
                    }
                }
                UnityEngine.Debug.LogError($"not found cueSheetName by audioName=> {audioName}");
                return null;
            }

            /// <summary>
            /// Snapshot名
            /// </summary>
            public static class SnapshotName
            {
                public static readonly string Normal = "Normal";
                public static readonly string Reverb = "Reverb";
                public static readonly string Distortion = "Distortion";
            }

            /// <summary>
            /// AudioMixer名
            /// </summary>
            public static class AudioMixerName
            {
                public static string Main = "MainAudioMixer";
                public static string EffectBgm = "EffectBgmAudioMixer";
                public static string EffectSe = "EffectSeAudioMixer";
            }

            /// <summary>
            /// AudioMixerGroup名
            /// </summary>
            public static class AudioMixerGroupName
            {
                public static string Master = "Master";
                public static string BGM = "BGM";
                public static string SE = "SE";
            }

            /// <summary>
            /// AudioMixerでExposeしたパラメータ名
            /// </summary>
            public static class AudioMixerParamName
            {
                public static readonly string MasterVolume = "MasterVolume";
                public static readonly string BgmVolume = "BGMVolume";
                public static readonly string SeVolume = "SeVolume";
            }

            /// <summary>
            /// AudioMixer情報
            /// </summary>
            public static IUnityAudioApiService.AudioMixerInfo[] AudioMixerInfoArray = new[]
            {
                new IUnityAudioApiService.AudioMixerInfo()
                {
                    MixerName = AudioMixerName.Main,
                    MixerGroupName = AudioMixerGroupName.Master,
                    VolumeParamName = AudioMixerParamName.MasterVolume,
                    SoundSheetName = null, // Mainはnull
                },
                new IUnityAudioApiService.AudioMixerInfo()
                {
                    MixerName = AudioMixerName.EffectBgm,
                    MixerGroupName = AudioMixerGroupName.Master,
                    VolumeParamName = AudioMixerParamName.BgmVolume,
                    SoundSheetName = SoundSheetName.Bgm,
                },
                new IUnityAudioApiService.AudioMixerInfo()
                {
                    MixerName = AudioMixerName.EffectSe,
                    MixerGroupName = AudioMixerGroupName.Master,
                    VolumeParamName = AudioMixerParamName.SeVolume,
                    SoundSheetName = SoundSheetName.Se,
                }
            };

            /// <summary>
            /// 初期生成情報
            /// </summary>
            public static readonly IUnityAudioApiService.InitializeSetting InitializeSetting = new()
            {
                // サービス生成時に設定する
                ListenerObject = null,
                MonoBehaviourHandler = null,
                // ゲームオーディオ設定
                AudioAssetPath = "UnityAudio",
                AudioMixerAssetPath = "UnityAudio/Mixer",
                SoundSheetInfoArray = SoundSheetInfoArray,
                AudioMixerInfoArray = AudioMixerInfoArray,
            };
        }
    }
}
