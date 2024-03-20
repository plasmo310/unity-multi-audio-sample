using System.Collections.Generic;
using System.Linq;
using AudioLib.Wwise;

namespace GameSample.Audio
{
    /// <summary>
    /// ゲーム内サウンド設定 (Wwise固有)
    /// </summary>
    public class GameWwiseAudioSettings : IGameAudioSettings
    {
        /// <summary>
        /// SoundEvent名
        /// </summary>
        public string SoundEventName_BgmSpaceWould => Wwise.SoundEventName.BgmSpaceWould;
        public string SoundEventName_BgmShotThunder => Wwise.SoundEventName.BgmShotThunder;
        public string SoundEventName_BgmAtomChain => Wwise.SoundEventName.BgmAtomChain;
        public string SoundEventName_SeAttack => Wwise.SoundEventName.SeAttack;
        public string SoundEventName_SeMove => Wwise.SoundEventName.SeMove;

        /// <summary>
        /// SoundSheet名
        /// </summary>
        public string SoundSheetName_Bgm => Wwise.SoundBankName.Bgm;
        public string SoundSheetName_Se => Wwise.SoundBankName.Se;

        /// <summary>
        /// GameParameter名
        /// </summary>
        public string GameParameterName_Battle => Wwise.GameParameterName.Battle;

        /// <summary>
        /// ビート同期ラベル名
        /// ※Wwiseでは設定無し
        /// </summary>
        public string BeatSyncLabelName_AtomChain => null;

        /// <summary>
        /// カスタムイベント名
        /// ※Wwiseでは設定無し
        /// </summary>
        public string CustomEventName_StartAtomChainMainLoop => null;

        /// <summary>
        /// Wwise固有設定
        /// </summary>
        public static class Wwise
        {
            /// <summary>
            /// Audioアセットパス
            /// </summary>
            public const string AudioAssetPath = "Audio/Wwise";

            /// <summary>
            /// SoundBank名
            /// </summary>
            public static class SoundBankName
            {
                public const string Global = "Global";
                public const string Bgm = "BGM";
                public const string Se = "Se";
            }

            /// <summary>
            /// SoundBank情報
            /// </summary>
            public static readonly IWwiseApiService.SoundBankInfo[] SoundBankInfoArray = new[]
            {
                new IWwiseApiService.SoundBankInfo()
                {
                    Name = SoundBankName.Global,
                    IsLoadOnInitialize = true,
                    IsDecodeBank = false,
                    IsSaveDecodedBank = false,
                },
                new IWwiseApiService.SoundBankInfo()
                {
                    Name = SoundBankName.Bgm,
                    IsLoadOnInitialize = true,
                    IsDecodeBank = false,
                    IsSaveDecodedBank = false,
                },
                new IWwiseApiService.SoundBankInfo()
                {
                    Name = SoundBankName.Se,
                    IsLoadOnInitialize = true,
                    IsDecodeBank = false,
                    IsSaveDecodedBank = false,
                }
            };

            /// <summary>
            /// SoundEvent名
            /// </summary>
            public static class SoundEventName
            {
                // 大文字小文字混在だとダメだった...
                // Assets/StreamingAssets/Audio/GeneratedSoundBanks/Wwise_IDs.h 内の定義名と合わせるのが無難そう.
                // https://www.audiokinetic.com/ja/library/edge/?source=SDK&id=soundengine_banks_general.html
                public const string GlobalPauseAll = "GLOBAL_PAUSE_ALL";
                public const string GlobalResumeAll = "GLOBAL_RESUME_ALL";
                public const string BgmSpaceWould = "BGM_SPACEWORLD";
                public const string BgmShotThunder = "BGM_SHOTTHUNDER";
                public const string BgmAtomChain = "BGM_ATOMCHAIN";
                public const string SeAttack = "SE_ATTACK";
                public const string SeMove = "SE_MOVE";
            }

            /// <summary>
            /// SoundBankに紐づくEvent情報
            /// </summary>
            public static readonly Dictionary<string, string[]> SoundBankEventInfoDictionary = new ()
            {
                {
                    SoundBankName.Global,
                    new[]
                    {
                        SoundEventName.GlobalPauseAll,
                        SoundEventName.GlobalResumeAll
                    }
                },
                {
                    SoundBankName.Bgm,
                    new[]
                    {
                        SoundEventName.BgmSpaceWould,
                        SoundEventName.BgmShotThunder,
                        SoundEventName.BgmAtomChain,
                    }
                },
                {
                    SoundBankName.Se,
                    new[]
                    {
                        SoundEventName.SeAttack,
                        SoundEventName.SeMove
                    }
                },
            };

            /// <summary>
            /// Event名に対応するSoundBank名を取得
            /// </summary>
            /// <param name="eventName"></param>
            /// <returns></returns>
            public static string GetSoundBankName(string eventName)
            {
                foreach (var soundBankName in SoundBankEventInfoDictionary.Keys)
                {
                    var eventNameArray = SoundBankEventInfoDictionary[soundBankName];
                    if (eventNameArray.Contains(eventName))
                    {
                        return soundBankName;
                    }
                }
                UnityEngine.Debug.LogError($"not found soundBankName by eventName=> {eventName}");
                return null;
            }

            /// <summary>
            /// GameParameter名
            /// </summary>
            public static class GameParameterName
            {
                public const string BgmReverb = "BGM_REVERB";
                public const string BgmDistortion = "BGM_DISTORTION";
                public const string SeReverb = "SE_REVERB";
                public const string SeDistortion = "SE_DISTORTION";
                public const string Battle = "BATTLE";
            }

            /// <summary>
            /// StateGroup名
            /// </summary>
            public static class StateGroupName
            {
                public const string BgmAtomChain = "BGM_ATOMCHAIN";
            }

            /// <summary>
            /// State名
            /// </summary>
            public static class StateName
            {
                public const string BgmAtomChainIntro = "BGM_ATOMCHAIN_INTRO";
                public const string BgmAtomChainMain = "BGM_ATOMCHAIN_MAIN";
            }

            /// <summary>
            /// 初期生成情報
            /// </summary>
            public static readonly IWwiseApiService.InitializeSettings InitializeSettings = new()
            {
                // サービス生成時に設定する
                ListenerObject = null,
                // ゲームオーディオ設定
                SoundBankInfoArray = SoundBankInfoArray.Where(soundBankInfo => soundBankInfo.IsLoadOnInitialize).ToArray()
            };
        }
    }
}
