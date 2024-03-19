using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSample.UI
{
    /// <summary>
    /// オーディオ設定UI
    /// </summary>
    public class UIAudioConfig : MonoBehaviour
    {
        /// <summary>
        /// 背景タップイベント
        /// </summary>
        [SerializeField] private EventTrigger _bgImageEventTrigger;

        public void SetListenerBgImage(UnityAction action)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(call => action());
            _bgImageEventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// ボリューム調整スライダー
        /// </summary>
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _bgmVolumeSlider;
        [SerializeField] private Slider _seVolumeSlider;

        public void SetValueMasterVolumeSlider(float value)
        {
            _masterVolumeSlider.value = value;
        }

        public void SetValueBgmVolumeSlider(float value)
        {
            _bgmVolumeSlider.value = value;
        }

        public void SetValueSeVolumeSlider(float value)
        {
            _seVolumeSlider.value = value;
        }

        public void SetListenerMasterVolumeSlider(UnityAction<float> callback)
        {
            if (_masterVolumeSlider.onValueChanged != null)
            {
                _masterVolumeSlider.onValueChanged.RemoveAllListeners();
            }
            _masterVolumeSlider.onValueChanged.AddListener(callback);
        }

        public void SetListenerBgmVolumeSliderCallback(UnityAction<float> callback)
        {
            if (_bgmVolumeSlider.onValueChanged != null)
            {
                _bgmVolumeSlider.onValueChanged.RemoveAllListeners();
            }
            _bgmVolumeSlider.onValueChanged.AddListener(callback);
        }

        public void SetListenerSeVolumeSliderCallback(UnityAction<float> callback)
        {
            if (_seVolumeSlider.onValueChanged != null)
            {
                _seVolumeSlider.onValueChanged.RemoveAllListeners();
            }
            _seVolumeSlider.onValueChanged.AddListener(callback);
        }
    }
}