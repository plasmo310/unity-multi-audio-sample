using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioLib.UnityAudio
{
    /// <summary>
    /// UnityAudio API操作クラス
    /// </summary>
    public class UnityAudioApiService : IUnityAudioApiService
    {
        /// <summary>
        /// AudioManagerオブジェクト名
        /// </summary>
        private static readonly string AudioManagerObjectName = "AudioManager";

        /// <summary>
        /// AudioManagerオブジェクト
        /// </summary>
        private readonly GameObject _audioManagerObject;

        /// <summary>
        /// 初期設定
        /// </summary>
        private readonly IUnityAudioApiService.InitializeSetting _initializeSetting;

        /// <summary>
        /// MonoBehaviourハンドラ
        /// </summary>
        private MonoBehaviour MonoBehaviourHandler => _initializeSetting.MonoBehaviourHandler;

        /// <summary>
        /// Audioアセットパス
        /// </summary>
        private string AudioAssetPath => _initializeSetting.AudioAssetPath;

        /// <summary>
        /// AudioMixerアセットパス
        /// </summary>
        private string AudioMixerAssetPath => _initializeSetting.AudioMixerAssetPath;

        /// <summary>
        /// SoundSheetに紐づくAudioSourceリスト
        /// </summary>
        private readonly Dictionary<string, AudioSource[]> _soundSheetAudioSourceDictionary = new Dictionary<string, AudioSource[]>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initializeSetting"></param>
        public UnityAudioApiService(IUnityAudioApiService.InitializeSetting initializeSetting)
        {
            _initializeSetting = initializeSetting;

            // AudioMixerの初期化
            InitializeAudioMixer(initializeSetting.AudioMixerInfoArray, initializeSetting.SoundSheetInfoArray);

            // Listener登録
            RegisterListenerObject(initializeSetting.ListenerObject);

            // AudioManagerオブジェクトを作成
            _audioManagerObject = new GameObject(AudioManagerObjectName);
            Object.DontDestroyOnLoad(_audioManagerObject);

            // SoundSheetを初期化
            InitializeSoundSheets(initializeSetting.SoundSheetInfoArray);

            _isPause = false;
        }

#region AudioMixer関連

        /// <summary>
        /// MainAudioMixer
        /// </summary>
        private AudioMixer _mainAudioMixer;

        /// <summary>
        /// MainVolumeパラメータ名
        /// </summary>
        private string _mainVolumeParamName;

        /// <summary>
        /// SoundSheetに紐づくAudioMixer情報
        /// </summary>
        private class SoundSheetAudioMixerInfo
        {
            public IUnityAudioApiService.SoundSheetInfo SoundSheetInfo;
            public AudioMixer AudioMixer;
            public AudioMixerGroup AudioMixerGroup;
            public string VolumeParamName;
        }

        /// <summary>
        /// SoundSheetに紐づくAudioMixer情報
        /// key: SoundSheet名
        /// </summary>
        private readonly Dictionary<string, SoundSheetAudioMixerInfo> _soundSheetAudioMixerInfoDictionary = new Dictionary<string, SoundSheetAudioMixerInfo>();

        /// <summary>
        /// AudioMixerの初期化
        /// </summary>
        /// <param name="audioMixerInfoArray"></param>
        /// <param name="soundSheetInfoArray"></param>
        private void InitializeAudioMixer(IUnityAudioApiService.AudioMixerInfo[] audioMixerInfoArray, IUnityAudioApiService.SoundSheetInfo[] soundSheetInfoArray)
        {
            foreach (var audioMixerInfo in audioMixerInfoArray)
            {
                // AudioMixer読み込み
                var audioMixer = LoadAudioMixer(audioMixerInfo.MixerName);

                var soundSheetName = audioMixerInfo.SoundSheetName;
                var volumeParamName = audioMixerInfo.VolumeParamName;
                if (string.IsNullOrEmpty(soundSheetName))
                {
                    // SoundSheet名が指定されていない場合はメイン扱い
                    _mainAudioMixer = audioMixer;
                    _mainVolumeParamName = volumeParamName;
                }
                else
                {
                    // SoundSheet名に紐づけてAudioMixerGroupを登録する
                    var audioMixerGroup = audioMixer.FindMatchingGroups(audioMixerInfo.MixerGroupName)[0];
                    _soundSheetAudioMixerInfoDictionary[soundSheetName] = new SoundSheetAudioMixerInfo()
                    {
                        SoundSheetInfo = soundSheetInfoArray.FirstOrDefault(soundSheetInfo => soundSheetInfo.Name == soundSheetName),
                        AudioMixer = audioMixer,
                        AudioMixerGroup = audioMixerGroup,
                        VolumeParamName = volumeParamName,
                    };
                }
            }
        }

        /// <summary>
        /// マスターボリューム取得
        /// </summary>
        /// <returns>ボリューム</returns>
        public float GetMasterVolume()
        {
            return GetAudioMixerVolume(_mainAudioMixer, _mainVolumeParamName);
        }

        /// <summary>
        /// マスターボリューム設定
        /// </summary>
        /// <param name="volume">ボリューム</param>
        public void SetMasterVolume(float volume)
        {
            SetAudioMixerVolume(_mainAudioMixer, _mainVolumeParamName, volume);
        }

        /// <summary>
        /// サウンドシートのボリューム取得
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <returns>ボリューム</returns>
        public float GetSoundSheetVolume(string soundSheetName)
        {
            return GetAudioMixerVolume(_mainAudioMixer, _soundSheetAudioMixerInfoDictionary[soundSheetName].VolumeParamName);
        }

        /// <summary>
        /// サウンドシートのボリューム設定
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="volume">ボリューム</param>
        public void SetSoundSheetVolume(string soundSheetName, float volume)
        {
            SetAudioMixerVolume(_mainAudioMixer, _soundSheetAudioMixerInfoDictionary[soundSheetName].VolumeParamName, volume);
        }

        /// <summary>
        /// AudioMixerのボリューム取得
        /// </summary>
        /// <param name="audioMixer"></param>
        /// <param name="exposedParamName"></param>
        /// <returns></returns>
        private static float GetAudioMixerVolume(AudioMixer audioMixer, string exposedParamName)
        {
            audioMixer.GetFloat(exposedParamName, out var decibel);
            if (decibel <= -96f)
            {
                return 0f;
            }
            return Mathf.Pow(10f, decibel / 20f);
        }

        /// <summary>
        /// AudioMixerのボリューム設定
        /// </summary>
        /// <param name="audioMixer"></param>
        /// <param name="exposedParamName"></param>
        /// <param name="volume"></param>
        private static void SetAudioMixerVolume(AudioMixer audioMixer, string exposedParamName, float volume)
        {
            var decibel = 20f * Mathf.Log10(volume);
            if (float.IsNegativeInfinity(decibel))
            {
                decibel = -96f;
            }
            audioMixer.SetFloat(exposedParamName, decibel);
        }

#endregion

        /// <summary>
        /// SoundSheetの初期化
        /// </summary>
        /// <param name="soundSheetInfoArray"></param>
        private void InitializeSoundSheets(IUnityAudioApiService.SoundSheetInfo[] soundSheetInfoArray)
        {
            foreach (var soundSheetInfo in soundSheetInfoArray)
            {
                // フェード用で2つ生成しておく
                var audioSource1 = CreateSoundSheetAudioSource(soundSheetInfo);
                var audioSource2 = CreateSoundSheetAudioSource(soundSheetInfo);
                _soundSheetAudioSourceDictionary[soundSheetInfo.Name] = new[] { audioSource1, audioSource2 };
            }
        }

        /// <summary>
        /// SoundSheet用AudioSource生成
        /// </summary>
        /// <param name="soundSheetInfo">サウンドシート情報</param>
        /// <returns></returns>
        private AudioSource CreateSoundSheetAudioSource(IUnityAudioApiService.SoundSheetInfo soundSheetInfo)
        {
            var audioMixerGroup = _soundSheetAudioMixerInfoDictionary[soundSheetInfo.Name].AudioMixerGroup;
            return CreateAudioSource(_audioManagerObject, soundSheetInfo.IsPlayLoop, audioMixerGroup);
        }

        /// <summary>
        /// Listenerオブジェクトの設定
        /// </summary>
        /// <param name="listenerObject">Listenerオブジェクト</param>
        public void RegisterListenerObject(UnityEngine.GameObject listenerObject)
        {
            if (listenerObject.GetComponent<AudioListener>() != null)
            {
                return;
            }
            listenerObject.AddComponent<AudioListener>();
        }

        /// <summary>
        /// AudioSource生成
        /// </summary>
        /// <param name="parentObject">親オブジェクト</param>
        /// <param name="isLoop">ループ再生するか？</param>
        /// <param name="audioMixerGroup">AudioMixerGroup</param>
        /// <returns></returns>
        private AudioSource CreateAudioSource(GameObject parentObject, bool isLoop, AudioMixerGroup audioMixerGroup)
        {
            var audioSource = parentObject.AddComponent<AudioSource>();
            audioSource.loop = isLoop;
            audioSource.playOnAwake = false; // デフォルトでtrueのため明示的にオフにする
            if (audioMixerGroup != null)
            {
                audioSource.outputAudioMixerGroup = audioMixerGroup;
            }
            return audioSource;
        }

        /// <summary>
        /// 一時停止中か？
        /// </summary>
        private bool _isPause = false;

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            if (_isPause)
            {
                return;
            }
            _isPause = true;

            foreach (var audioSourceArray in _soundSheetAudioSourceDictionary.Values)
            {
                foreach (var audioSource in audioSourceArray)
                {
                    audioSource.Pause();
                }
            }
        }

        /// <summary>
        /// 一時停止解除
        /// </summary>
        public void Resume()
        {
            if (!_isPause)
            {
                return;
            }
            _isPause = false;

            foreach (var audioSourceArray in _soundSheetAudioSourceDictionary.Values)
            {
                foreach (var audioSource in audioSourceArray)
                {
                    audioSource.UnPause();
                }
            }
        }

        /// <summary>
        /// オーディオ再生
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="audioName">オーディオ名</param>
        /// <param name="option">再生オプション</param>
        public void Play(string soundSheetName, string audioName, IUnityAudioApiService.SoundPlayOption option = null)
        {
            var audioSource = _soundSheetAudioSourceDictionary[soundSheetName].FirstOrDefault(audioSource => !audioSource.isPlaying);
            if (audioSource == null)
            {
                return;
            }
            PlaySource(audioSource, soundSheetName, audioName, option);
        }

        /// <summary>
        /// オーディオ再生(3D位置再生)
        /// </summary>
        /// <param name="targetObject">対象オブジェクト</param>
        /// <param name="spatialRatio">3D再生のブレンド値</param>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="audioName">オーディオ名</param>
        /// <param name="option">再生オプション</param>
        public void Play3d(GameObject targetObject, float spatialRatio, string soundSheetName, string audioName, IUnityAudioApiService.SoundPlayOption option = null)
        {
            // AudioSourceの生成
            var audioSource = targetObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                var soundSheetAudioMixerInfo = _soundSheetAudioMixerInfoDictionary[soundSheetName];
                var soundSheetInfo = soundSheetAudioMixerInfo.SoundSheetInfo;
                var audioMixerGroup = soundSheetAudioMixerInfo.AudioMixerGroup;
                audioSource = CreateAudioSource(targetObject, soundSheetInfo.IsPlayLoop, audioMixerGroup);
                audioSource.spatialBlend = spatialRatio;
            }
            PlaySource(audioSource, soundSheetName, audioName, option);
        }

        /// <summary>
        /// AudioSourceの再生
        /// </summary>
        /// <param name="audioSource">AudioSource</param>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="audioName">オーディオ名</param>
        /// <param name="option">再生オプション</param>
        private void PlaySource(AudioSource audioSource, string soundSheetName, string audioName, IUnityAudioApiService.SoundPlayOption option = null)
        {
            audioSource.clip = LoadAudioClip(audioName);
            audioSource.volume = option?.Volume ?? 1f;
            audioSource.pitch = option?.Pitch ?? 1f;

            var fadeTimeSec = option?.FadeTimeSec ?? 0;
            if (fadeTimeSec > 0)
            {
                // フェード再生
                Stop(soundSheetName, new IUnityAudioApiService.SoundStopOption() { FadeTimeSec = fadeTimeSec});
                MonoBehaviourHandler.StartCoroutine(FadeInCoroutine(audioSource, fadeTimeSec, option?.Volume ?? 1f));
            }
            else
            {
                // 通常再生
                Stop(soundSheetName);
                audioSource.Play();
            }
        }

        /// <summary>
        /// オーディオ停止
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="option">停止オプション</param>
        public void Stop(string soundSheetName, IUnityAudioApiService.SoundStopOption option = null)
        {
            // 再生中のオーディオを全て取得して停止する
            var audioSource = _soundSheetAudioSourceDictionary[soundSheetName].FirstOrDefault(audioSource => audioSource.isPlaying);
            if (audioSource == null)
            {
                return;
            }

            var fadeTimeSec = option?.FadeTimeSec ?? 0;
            if (fadeTimeSec > 0)
            {
                // フェードアウト
                MonoBehaviourHandler.StartCoroutine(FadeOutCoroutine(audioSource, fadeTimeSec));
            }
            else
            {
                // 通常停止
                audioSource.Stop();
                audioSource.clip = null;
            }
        }

        /// <summary>
        /// フェード中のAudioSource
        /// </summary>
        private readonly HashSet<AudioSource> _fadeAudioSources = new HashSet<AudioSource>();

        /// <summary>
        /// フェードイン再生
        /// </summary>
        /// <param name="audioSource">AudioSource</param>
        /// <param name="fadeTimeSec">フェード時間(秒)</param>
        /// <param name="targetVolume">ターゲットボリューム</param>
        /// <returns></returns>
        private IEnumerator FadeInCoroutine(AudioSource audioSource, float fadeTimeSec, float targetVolume = 1f)
        {
            if (_fadeAudioSources.Contains(audioSource))
            {
                yield break;
            }
            _fadeAudioSources.Add(audioSource);

            var startVolume = 0f;

            audioSource.volume = startVolume;
            audioSource.Play();

            for (var t = 0f; t < fadeTimeSec; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, Mathf.Clamp01(t / fadeTimeSec));
                yield return null;
            }
            audioSource.volume = targetVolume;

            _fadeAudioSources.Remove(audioSource);
        }

        /// <summary>
        /// フェードアウト再生
        /// </summary>
        /// <param name="audioSource">AudioSource</param>
        /// <param name="fadeTime">フェード時間(秒)</param>
        /// <returns></returns>
        private IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeTime)
        {
            if (_fadeAudioSources.Contains(audioSource))
            {
                yield break;
            }
            _fadeAudioSources.Add(audioSource);

            var startVolume = audioSource.volume;
            var targetVolume = 0f;
            for (var t = 0f; t < fadeTime; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, Mathf.Clamp01(t / fadeTime));
                yield return null;
            }
            audioSource.volume = targetVolume;

            audioSource.Stop();
            audioSource.clip = null;

            _fadeAudioSources.Remove(audioSource);
        }

        /// <summary>
        /// サウンドシートのスナップショット切替
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="snapshotName">スナップショット名</param>
        /// <param name="fadeTimeSec">フェード時間(秒)</param>
        public void ChangeSoundSheetSnapshot(string soundSheetName, string snapshotName, float fadeTimeSec)
        {
            var audioMixer = _soundSheetAudioMixerInfoDictionary[soundSheetName].AudioMixer;
            var snapshot = audioMixer.FindSnapshot(snapshotName);
            if (snapshot != null)
            {
                snapshot.TransitionTo(fadeTimeSec);
            }
        }

        /// <summary>
        /// 指定サウンドシートのスペクトラム周波数を取得
        /// </summary>
        /// <param name="soundSheetName">サウンドシート名</param>
        /// <param name="sampleCount">サンプル数</param>
        /// <param name="isConvertDecibel">デシベル変換するか？</param>
        /// <returns></returns>
        public float[] GetBgmSpectrumData(string soundSheetName, int sampleCount, bool isConvertDecibel)
        {
            // 周波数データを取得
            var spectrumDataArray = new float[sampleCount];
            var bgmAudioResource = _soundSheetAudioSourceDictionary[soundSheetName].FirstOrDefault(audioSource => audioSource.isPlaying);
            if (bgmAudioResource == null)
            {
                return spectrumDataArray;
            }
            bgmAudioResource.GetSpectrumData(spectrumDataArray, 0, FFTWindow.BlackmanHarris);

            spectrumDataArray = spectrumDataArray.Select((value) =>
            {
                // 他サウンドミドルウェアとの計算単位を合わせる
                var newValue = value * 1000f;

                // デシベル変換
                if (isConvertDecibel)
                {
                    newValue = 20.0f * Mathf.Log10(newValue);
                    if (float.IsInfinity(newValue))
                    {
                        return 0f;
                    }
                    return newValue;
                }
                return newValue;
            }).ToArray();
            return spectrumDataArray;
        }

#region ファイル読込処理

        // とりあえずResources配下からの読込にしている.

        /// <summary>
        /// キャッシュしたAudioClip
        /// key: ファイル名
        /// </summary>
        private readonly IDictionary<string, AudioClip> _cachedAudioDictionary = new Dictionary<string, AudioClip>();

        /// <summary>
        /// AudioClipの読み込み
        /// </summary>
        /// <param name="audioName"></param>
        /// <returns></returns>
        private AudioClip LoadAudioClip(string audioName)
        {
            // ファイル名をキーとしてキャッシュする
            if (!_cachedAudioDictionary.ContainsKey(audioName))
            {
                var audioAssetPath = Path.Combine(AudioAssetPath, audioName);
                var audioClip = Resources.Load(audioAssetPath) as AudioClip;
                if (audioClip == null)
                {
                    Debug.LogError($"not found audio clip => {audioAssetPath}");
                }
                _cachedAudioDictionary.Add(audioName, audioClip);
            }
            return _cachedAudioDictionary[audioName];
        }

        /// <summary>
        /// AudioMixerの読み込み
        /// </summary>
        /// <returns></returns>
        private AudioMixer LoadAudioMixer(string audioMixerName)
        {
            var audioMixerPath = Path.Combine(AudioMixerAssetPath, audioMixerName);
            return Resources.Load(audioMixerPath) as AudioMixer;
        }

#endregion

    }
}