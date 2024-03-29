using TMPro;
using UnityEngine;

namespace GameSample.UI
{
    /// <summary>
    /// 選択中のAudioLibを表示する
    /// </summary>
    public class UIAudioLibText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _audioLibText;

        [SerializeField]
        private string _unityAudioText = "UnityAudio";

        [SerializeField]
        private string _criAdxText = "CRI ADX";

        [SerializeField]
        private string _wwiseText = "Wwise";

        private string _noSelectedText = "NoSelected";

        private void Start()
        {
            var text = "Audio: ";
#if AUDIO_LIB_UNITY_AUDIO
            text += _unityAudioText;
#elif AUDIO_LIB_CRI
            text += _criAdxText;
#elif AUDIO_LIB_WWISE
            text += _wwiseText;
#else
            text += _noSelectedText;
#endif
            _audioLibText.SetText(text);
        }
    }
}
