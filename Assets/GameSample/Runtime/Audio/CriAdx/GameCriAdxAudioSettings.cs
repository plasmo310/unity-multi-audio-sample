using System.Collections.Generic;
using System.Linq;
using AudioLib.CriAdx;

namespace GameSample.Audio
{
    /// <summary>
    /// ゲーム内サウンド設定 (CRI固有)
    /// </summary>
    public class GameCriAdxAudioSettings : IGameAudioSettings
    {
        /// <summary>
        /// SoundEvent名
        /// </summary>
        public string SoundEventName_BgmSpaceWould => CriAdx.CueName.BgmSpaceWould;
        public string SoundEventName_BgmShotThunder => CriAdx.CueName.BgmShotThunder;
        public string SoundEventName_BgmAtomChain => CriAdx.CueName.BgmAtomChain;
        public string SoundEventName_SeAttack => CriAdx.CueName.SeAttack;
        public string SoundEventName_SeMove => CriAdx.CueName.SeMove;

        /// <summary>
        /// SoundSheet名
        /// </summary>
        public string SoundSheetName_Bgm => CriAdx.CueSheetName.Bgm;
        public string SoundSheetName_Se => CriAdx.CueSheetName.Se;

        /// <summary>
        /// GameParameter名
        /// </summary>
        public string GameParameterName_Battle => CriAdx.AisacName.Battle;

        /// <summary>
        /// ビート同期ラベル名
        /// </summary>
        public string BeatSyncLabelName_AtomChain => CriAdx.BeatSyncLabelName.AtomChain;

        /// <summary>
        /// カスタムイベント名
        /// </summary>
        public string CustomEventName_StartAtomChainMainLoop => CriAdx.EventName.StartAtomChainMainLoop;

        /// <summary>
        /// CRI固有設定
        /// </summary>
        public static class CriAdx
        {
            /// <summary>
            /// CueSheet名
            /// </summary>
            public static class CueSheetName
            {
                public const string Bgm = "BGM";
                public const string BgmBlock = "BGM_Block";
                public const string Se = "SE";
            }

            /// <summary>
            /// CueSheet情報
            /// </summary>
            public static readonly ICriAdxApiService.CueSheetInfo[] CueSheetInfoArray = new []
            {
                new ICriAdxApiService.CueSheetInfo()
                {
                    Name = CueSheetName.Bgm,
                    IsLoadOnInitialize = true,
                    IsExistAwbFile = true,
                    IsPlayLoop = true,
                },
                new ICriAdxApiService.CueSheetInfo()
                {
                    Name = CueSheetName.BgmBlock,
                    IsLoadOnInitialize = true,
                    IsExistAwbFile = true,
                    IsPlayLoop = false,
                },
                new ICriAdxApiService.CueSheetInfo()
                {
                    Name = CueSheetName.Se,
                    IsLoadOnInitialize = true,
                    IsExistAwbFile = false,
                    IsPlayLoop = false,
                },
            };

            /// <summary>
            /// Cue名
            /// </summary>
            public static class CueName
            {
                public const string BgmSpaceWould = "BGM_SpaceWould";
                public const string BgmShotThunder = "BGM_ShotThunder";
                public const string BgmMogTheme = "BGM_MogTheme";
                public const string BgmAtomChain = "BGM_AtomChain";
                public const string SeMove = "SE_Move";
                public const string SeAttack = "SE_Attack";
                public const string SeRandom = "SE_Random";
            }

            /// <summary>
            /// CueSheetに紐づくCue情報
            /// </summary>
            public static readonly Dictionary<string, string[]> CueSheetCueInfoDictionary = new ()
            {
                {
                    CueSheetName.Bgm,
                    new[]
                    {
                        CueName.BgmSpaceWould,
                        CueName.BgmShotThunder,
                        CueName.BgmMogTheme,
                    }
                },
                {
                    CueSheetName.BgmBlock,
                    new[]
                    {
                        CueName.BgmAtomChain,
                    }
                },
                {
                    CueSheetName.Se,
                    new[]
                    {
                        CueName.SeAttack,
                        CueName.SeMove
                    }
                },
            };

            /// <summary>
            /// Cue名から対象のCueSheet名を取得する
            /// </summary>
            /// <param name="cueName"></param>
            /// <returns></returns>
            public static string GetCueSheetName(string cueName)
            {
                foreach (var soundBankName in CueSheetCueInfoDictionary.Keys)
                {
                    var eventNameArray = CueSheetCueInfoDictionary[soundBankName];
                    if (eventNameArray.Contains(cueName))
                    {
                        return soundBankName;
                    }
                }
                UnityEngine.Debug.LogError($"not found cueSheetName by cueName=> {cueName}");
                return null;
            }

            /// <summary>
            /// DSPバス名
            /// </summary>
            public static class BusName
            {
                public const string Master = "MasterOut";
                public const string BgmReverb = "BGMReverb";
                public const string BgmDistortion = "BGMDistortion";
                public const string SeReverb = "SEReverb";
                public const string SeDistortion = "SEDistortion";
            }

            /// <summary>
            /// DSPバス スナップショット名
            /// </summary>
            public static class BusSnapshotName
            {
                public const string Normal = "Normal";
                public const string BgmReverb = "BGM_Reverb";
                public const string BgmDistortion = "BGM_Distortion";
                public const string SeReverb = "SE_Reverb";
                public const string SeDistortion = "SE_Distortion";
            }

            /// <summary>
            /// カテゴリー名
            /// </summary>
            public static class CategoryName
            {
                public const string Bgm = "BGM";
                public const string Se = "SE";
            }

            /// <summary>
            /// AISACコントール名
            /// </summary>
            public static class AisacName
            {
                public const string Battle = "Battle";
            }

            /// <summary>
            /// イベント名
            /// </summary>
            public static class EventName
            {
                public const string StartAtomChainMainLoop = "StartAtomChainMainLoop";
            }

            /// <summary>
            /// BeatSyncラベル名
            /// </summary>
            public static class BeatSyncLabelName
            {
                public const string AtomChain = "AtomChainBeatSyncLabel";
            }

            /// <summary>
            /// 初期生成情報
            /// </summary>
            public static readonly ICriAdxApiService.InitializeSetting InitializeSetting = new()
            {
                // サービス生成時に設定する
                ListenerObject = null,
                // ゲームオーディオ設定
                AssetFilePath = "Audio/CriAdx",
                AcfFileName = "UnityCriSample.acf",
                CueSheetInfoArray = CueSheetInfoArray,
            };
        }
    }
}
