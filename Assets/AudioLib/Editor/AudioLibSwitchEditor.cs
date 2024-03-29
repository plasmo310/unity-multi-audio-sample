using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace AudioLib.Editor
{
    /// <summary>
    /// Audioライブラリ切替エディタ
    /// </summary>
    public class AudioLibSwitchEditor : EditorWindow
    {
        /// <summary>
        /// Audioライブラリ種類
        /// </summary>
        private enum AudioLibType
        {
            UnityAudio,
            CriAtom,
            Wwise,
        }

        /// <summary>
        /// Define定義
        /// </summary>
        private const string DefineSymbolPrefix = "AUDIO_LIB";
        private const string DefineSymbolUnityAudio = DefineSymbolPrefix + "_UNITY_AUDIO";
        private const string DefineSymbolCriAtom = DefineSymbolPrefix + "_CRI";
        private const string DefineSymbolWwise = DefineSymbolPrefix + "_WWISE";

        /// <summary>
        /// ビルド対象プラットフォーム
        /// </summary>
        private static readonly List<BuildTargetGroup> BuildTargetGroupList = new List<BuildTargetGroup>()
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
        };

        [MenuItem("Tools/Switch AudioLib/UnityAudio")]
        private static void SwitchUnityAudioLibrary()
        {
            SwitchAudioLibrary(BuildTargetGroupList, AudioLibType.UnityAudio);
            UnityEngine.Debug.Log("complete switch audio UnityAudio.");
        }

        [MenuItem("Tools/Switch AudioLib/CriAtom")]
        private static void SwitchCriAtomLibrary()
        {
            SwitchAudioLibrary(BuildTargetGroupList, AudioLibType.CriAtom);
            UnityEngine.Debug.Log("complete switch audio CRI.");
        }

        [MenuItem("Tools/Switch AudioLib/Wwise")]
        private static void SwitchWwiseLibrary()
        {
            SwitchAudioLibrary(BuildTargetGroupList, AudioLibType.Wwise);
            UnityEngine.Debug.Log("complete switch audio Wwise.");
        }

        /// <summary>
        /// 選択されたライブラリに切り替える
        /// </summary>
        /// <param name="buildTargetGroupList"></param>
        /// <param name="audioLibType"></param>
        private static void SwitchAudioLibrary(List<BuildTargetGroup> buildTargetGroupList, AudioLibType audioLibType)
        {
            // コンパイル停止
            EditorApplication.LockReloadAssemblies();

            // Define適用
            foreach (var buildTargetGroup in buildTargetGroupList)
            {
                SwitchDefineAudioLibrary(buildTargetGroup, audioLibType);
            }

            // コンパイル再開
            EditorApplication.UnlockReloadAssemblies();
        }

        /// <summary>
        /// 選択されたライブラリのDefineを適用する
        /// </summary>
        /// <param name="buildTargetGroup"></param>
        /// <param name="audioLibType"></param>
        private static void SwitchDefineAudioLibrary(BuildTargetGroup buildTargetGroup, AudioLibType audioLibType)
        {
            var newSymbolNameList = new List<string>();

            // 既存のDefineを保持
            var currentDefineSymbolNames = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!string.IsNullOrEmpty(currentDefineSymbolNames))
            {
                foreach (var currentSymbolName in currentDefineSymbolNames.Split(";"))
                {
                    if (currentSymbolName.Contains(DefineSymbolPrefix))
                    {
                        continue;
                    }
                    newSymbolNameList.Add(currentSymbolName);
                }
            }

            // シンボル名取得、UnityAudio有効/無効設定
            var symbolName = "";
            switch (audioLibType)
            {
                case AudioLibType.UnityAudio:
                    symbolName = DefineSymbolUnityAudio;
                    SetDisableUnityAudio(false);
                    break;
                case AudioLibType.CriAtom:
                    symbolName = DefineSymbolCriAtom;
                    SetDisableUnityAudio(true);
                    break;
                case AudioLibType.Wwise:
                    symbolName = DefineSymbolWwise;
                    SetDisableUnityAudio(true);
                    break;
            }

            // 選択されたDefineを追加して適用
            newSymbolNameList.Add(symbolName);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", newSymbolNameList));
        }

        /// <summary>
        /// ProjectSettings - Audio - Disable Unity Audio 設定
        /// </summary>
        /// <param name="isDisable"></param>
        private static void SetDisableUnityAudio(bool isDisable)
        {
            const string audioManagerPath = "ProjectSettings/AudioManager.asset";
            var audioManager = AssetDatabase.LoadAllAssetsAtPath(audioManagerPath).FirstOrDefault();
            var audioManagerObject = new SerializedObject(audioManager);
            var prop= audioManagerObject.FindProperty("m_DisableAudio");
            prop.boolValue = isDisable;
            audioManagerObject.ApplyModifiedProperties();
        }
    }
}
