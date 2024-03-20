using GameSample.Audio;
using GameSample.Common;
using UnityEngine;

namespace GameSample
{
    /// <summary>
    /// プロジェクト初期化クラス
    /// </summary>
    public static class ProjectInitializer
    {
        /// <summary>
        /// シーンのロード後の初期化処理
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            // サービス登録
#if AUDIO_LIB_UNITY_AUDIO
            ServiceLocator.Register<IGameAudioSettings>(new GameUnityAudioSettings());
            ServiceLocator.Register<IGameAudioService>(new GameUnityAudioService());
#elif AUDIO_LIB_CRI
            ServiceLocator.Register<IGameAudioSettings>(new GameCriAdxAudioSettings());
            ServiceLocator.Register<IGameAudioService>(new GameCriAdxAudioService());
#elif AUDIO_LIB_WWISE
            ServiceLocator.Register<IGameAudioSettings>(new GameWwiseAudioSettings());
            ServiceLocator.Register<IGameAudioService>(new GameWwiseAudioService());
#endif
        }
    }
}
