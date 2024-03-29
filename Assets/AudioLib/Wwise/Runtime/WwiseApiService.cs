using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AudioLib.Wwise
{
    /// <summary>
    /// Wwise API操作クラス
    /// </summary>
    public class WwiseApiService : IWwiseApiService
    {
#if AUDIO_LIB_WWISE

        /// <summary>
        /// Wwiseグローバルオブジェクトの生成
        /// シーンロード前に実行する
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateAudioGlobalObject()
        {
            const string audioGlobalPrefabName = "WwiseGlobal";
            var audioGlobalPrefab = Resources.Load(audioGlobalPrefabName);
            var audioGlobalObject = UnityEngine.Object.Instantiate(audioGlobalPrefab);
            UnityEngine.Object.DontDestroyOnLoad(audioGlobalObject);
        }

#endif

        /// <summary>
        /// AudioListener
        /// </summary>
        private AkAudioListener _listenerObject;

        /// <summary>
        /// SoundBankオブジェクトのルートオブジェクト
        /// </summary>
        private GameObject _soundBankRootObject;

        /// <summary>
        /// SoundBankに対応するオブジェクトのキャッシュ
        /// </summary>
        private readonly Dictionary<string, HashSet<GameObject>> _soundBankObjectCache = new Dictionary<string, HashSet<GameObject>>();

        /// <summary>
        /// SoundBankに対応するボリューム情報のキャッシュ
        /// </summary>
        private readonly Dictionary<string, float> _soundBankVolumeCache = new Dictionary<string, float>();

        /// <summary>
        /// マスターボリューム
        /// </summary>
        private float _masterVolume = 1f;

        /// <summary>
        /// SoundBankに対応するメインオブジェクトの取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private GameObject GetMainSoundBankObject(string name) => _soundBankObjectCache.ContainsKey(name) ? _soundBankObjectCache[name].FirstOrDefault() : null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initializeSettings"></param>
        public WwiseApiService(IWwiseApiService.InitializeSettings initializeSettings)
        {
            InitializeAudioSettings(initializeSettings);
        }

        /// <summary>
        /// Wwise関連リソースの設定
        /// </summary>
        /// <param name="initializeSettings"></param>
        /// <param name="listenerObject"></param>
        private void InitializeAudioSettings(IWwiseApiService.InitializeSettings initializeSettings)
        {
            // AkAudioListenerの取得
            // 基本はカメラに設定する
            RegisterListenerObject(initializeSettings.ListenerObject);

            // SoundBankの初期化
            if (_soundBankRootObject == null)
            {
                _soundBankRootObject = new GameObject("WwiseSoundBank");
                Object.DontDestroyOnLoad(_soundBankRootObject);
            }

            if (initializeSettings.SoundBankInfoArray != null)
            {
                foreach (var soundBankInfo in initializeSettings.SoundBankInfoArray)
                {
                    if (!soundBankInfo.IsLoadOnInitialize)
                    {
                        continue;
                    }
                    LoadSoundBank(soundBankInfo);
                }
            }
        }

        /// <summary>
        /// Listenerオブジェクトの設定
        /// </summary>
        /// <param name="listenerObject"></param>
        public void RegisterListenerObject(UnityEngine.GameObject listenerObject)
        {
            _listenerObject = listenerObject.AddComponent<AkAudioListener>();
        }

        /// <summary>
        /// SoundBankのロード
        /// </summary>
        /// <param name="soundBankInfo"></param>
        public void LoadSoundBank(IWwiseApiService.SoundBankInfo soundBankInfo)
        {
            var soundBankName = soundBankInfo.Name;
            AkBankManager.LoadBank(soundBankName, soundBankInfo.IsDecodeBank, soundBankInfo.IsSaveDecodedBank);

            // GameObjectを生成しておく
            if (!_soundBankObjectCache.ContainsKey(soundBankName))
            {
                var soundBankObject = new GameObject(soundBankName);
                soundBankObject.transform.SetParent(_soundBankRootObject.transform);
                _soundBankObjectCache.Add(soundBankName, new HashSet<GameObject>{ soundBankObject });
                AkSoundEngine.RegisterGameObj(soundBankObject); // AkGameObjectとしても登録しておく

                // ボリュームが先に設定されている場合もあるため、GameObjectに反映するため再設定
                SetSoundBankVolume(soundBankName, GetSoundBankVolume(soundBankName));
            }
        }

        /// <summary>
        /// SoundBankのアンロード
        /// </summary>
        /// <param name="soundBankName"></param>
        public void UnLoadSoundBank(string soundBankName)
        {
            AkBankManager.UnloadBank(soundBankName);

            // 生成したGameObjectを破棄する
            if (_soundBankObjectCache.TryGetValue(soundBankName, out var soundBankObjectList))
            {
                Object.Destroy(soundBankObjectList.FirstOrDefault());
                _soundBankObjectCache.Remove(soundBankName);
            }
        }

        /// <summary>
        /// 全てのSoundBankのアンロード
        /// </summary>
        public void UnLoadAllSoundBank()
        {
            if (_soundBankObjectCache.Count <= 0)
            {
                return;
            }

            var soundBankNameArray = _soundBankObjectCache.Select(cache => cache.Key).ToArray();
            foreach (var soundBankName in soundBankNameArray)
            {
                UnLoadSoundBank(soundBankName);
            }
        }

        /// <summary>
        /// Eventの実行
        /// 停止・再開など、再生以外のEventを想定
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public uint DoEvent(string soundBankName, string eventName)
        {
            var mainSoundBankObject = GetMainSoundBankObject(soundBankName);
            return AkSoundEngine.PostEvent(eventName, mainSoundBankObject);
        }

        /// <summary>
        /// Eventの再生
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public uint PlayEvent(string soundBankName, string eventName, IWwiseApiService.SoundPlayOption option = null)
        {
            return PlayEvent(soundBankName, eventName, GetMainSoundBankObject(soundBankName), option);
        }

        /// <summary>
        /// Eventの再生
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="gameObject"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public uint PlayEvent(string soundBankName, string eventName, GameObject gameObject, IWwiseApiService.SoundPlayOption option = null)
        {
            if (option != null && option.IsStopOther)
            {
                StopAllSoundBank(soundBankName);
            }

            // メイン再生オブジェクト以外の場合、ボリューム調整のためキャッシュしておく
            var mainSoundBankObject = GetMainSoundBankObject(soundBankName);
            if (mainSoundBankObject != null && mainSoundBankObject != gameObject)
            {
                AkSoundEngine.RegisterGameObj(gameObject);
                _soundBankObjectCache[soundBankName].Add(gameObject);
                SetSoundBankVolume(soundBankName, GetSoundBankVolume(soundBankName));
            }

            // Event再生
            uint playId = 0;
            if (option?.BeatSyncCallback != null
                || option?.CustomEventCallback != null)
            {
                uint callbackTypeValue = default;
                if (option?.BeatSyncCallback != null) callbackTypeValue |= (uint) AkCallbackType.AK_MusicSyncBeat;
                if (option?.CustomEventCallback != null) callbackTypeValue |= (uint) AkCallbackType.AK_MusicSyncUserCue;
                AkCallbackManager.EventCallback eventCallback = (cookie, type, info) =>
                {
                    switch (type)
                    {
                        case AkCallbackType.AK_MusicSyncBeat:
                            option?.BeatSyncCallback?.Invoke();
                            break;
                        case AkCallbackType.AK_MusicSyncUserCue:
                            option?.CustomEventCallback?.Invoke();
                            break;
                    }
                };
                playId = AkSoundEngine.PostEvent(eventName, gameObject, callbackTypeValue, eventCallback, null);
            }
            else
            {
                playId = AkSoundEngine.PostEvent(eventName, gameObject);
            }
            if (option != null && option.FadeTimeMs > 0)
            {
                // 再生後の Pause -> Resume による強制フェード
                // 恐らく想定した使い方ではない...
                AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Pause, gameObject);
                AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Resume, gameObject, option.FadeTimeMs);
            }
            return playId;
        }

        /// <summary>
        /// Eventの停止
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="eventName"></param>
        /// <param name="option"></param>
        public void StopEvent(string soundBankName, string eventName, IWwiseApiService.SoundStopOption option = null)
        {
            if (option != null && option.FadeTimeMs > 0)
            {
                AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, GetMainSoundBankObject(soundBankName), option.FadeTimeMs);
                return;
            }
            AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, GetMainSoundBankObject(soundBankName));
        }

        /// <summary>
        /// SoundBankの全Eventの停止
        /// </summary>
        /// <param name="soundBankName"></param>
        public void StopAllSoundBank(string soundBankName)
        {
            AkSoundEngine.StopAll(GetMainSoundBankObject(soundBankName));
        }

        /// <summary>
        /// 全Eventの停止
        /// </summary>
        public void StopAll()
        {
            AkSoundEngine.StopAll();
        }

        /// <summary>
        /// SoundBankボリュームの取得
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <returns></returns>
        public float GetSoundBankVolume(string soundBankName)
        {
            // ボリューム取得の関数は調べた感じだと用意されていないため、キャッシュしたボリュームをそのまま返している
            // 永続的に扱うにはゲーム内で保存した値を使用する必要がある
            return _soundBankVolumeCache.ContainsKey(soundBankName) ? _soundBankVolumeCache[soundBankName] : 1f;
        }

        /// <summary>
        /// SoundBankボリュームの設定
        /// </summary>
        /// <param name="soundBankName"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        public bool SetSoundBankVolume(string soundBankName, float volume)
        {
            _soundBankVolumeCache[soundBankName] = volume;

            // GameObjectがまだ生成されていない場合、キャッシュのみ行う
            var mainSoundBankObject = GetMainSoundBankObject(soundBankName);
            if (mainSoundBankObject == null)
            {
                return true;
            }

            // SoundBankに紐づく全てのオブジェクトに対してボリュームを設定する
            foreach (var soundBankObject in _soundBankObjectCache[soundBankName])
            {
                if (soundBankObject == null)
                {
                    continue;
                }
                var emitterObjectId = AkSoundEngine.GetAkGameObjectID(soundBankObject);
                var listenerObjectId = AkSoundEngine.GetAkGameObjectID(_listenerObject.gameObject);
                var result = AkSoundEngine.SetGameObjectOutputBusVolume(emitterObjectId, listenerObjectId, volume * _masterVolume);
                if (result != AKRESULT.AK_Success)
                {
                    Debug.LogError($"failed set volume => {soundBankName}: {result}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// マスターボリュームの取得
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume()
        {
            return _masterVolume;
        }

        /// <summary>
        /// マスターボリュームの設定
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;

            // 各SoundBankのボリュームにも反映
            foreach (var soundBankName in _soundBankObjectCache.Keys)
            {
                SetSoundBankVolume(soundBankName, GetSoundBankVolume(soundBankName));
            }
        }

        /// <summary>
        /// GameParameterの設定
        /// </summary>
        /// <param name="gameParameterName"></param>
        /// <param name="value"></param>
        public void SetGameParameter(string gameParameterName, float value)
        {
            AkSoundEngine.SetRTPCValue(AkSoundEngine.GetIDFromString(gameParameterName), value);
        }

        /// <summary>
        /// GameParameterの取得
        /// </summary>
        /// <param name="gameParameterName"></param>
        /// <returns></returns>
        public float GetGameParameter(string gameParameterName)
        {
            var playingId = AkSoundEngine.AK_INVALID_PLAYING_ID;
            var queryValue = (int)AkQueryRTPCValue.RTPCValue_Global;
            AkSoundEngine.GetRTPCValue(AkSoundEngine.GetIDFromString(gameParameterName), null, playingId, out var value, ref queryValue);
            return value;
        }

        /// <summary>
        /// Stateの設定
        /// </summary>
        /// <param name="stateGroupName"></param>
        /// <param name="stateName"></param>
        public void SetState(string stateGroupName, string stateName)
        {
            AkSoundEngine.SetState(stateGroupName, stateName);
        }
    }
}
