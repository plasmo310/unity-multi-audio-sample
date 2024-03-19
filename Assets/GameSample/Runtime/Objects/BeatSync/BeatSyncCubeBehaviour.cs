using System.Linq;
using UnityEngine;

namespace GameSample.Objects.BeatSync
{
    /// <summary>
    /// ビート同期Cubeオブジェクト
    /// OnBeatSyncが呼ばれる度にランダムに伸縮する
    /// </summary>
    public class BeatSyncCubeBehaviour : MonoBehaviour, IBeatSyncBehaviour
    {
        /// <summary>
        /// 初期化済か？
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// Scaleさせる値の配列
        /// </summary>
        private float[] _scaleHeightArray;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="maxBeatCount"></param>
        /// <param name="maxHeight"></param>
        public void OnInitialize(int maxBeatCount, float maxHeight)
        {
            _isInitialized = true;

            // ビート数分の数字を配列に格納
            _scaleHeightArray = new float[maxBeatCount];
            for (var i = 0; i < _scaleHeightArray.Length; i++)
            {
                _scaleHeightArray[i] = maxHeight / (i + 1);
            }

            // 適当にシャッフルしておく
            var random = new System.Random();
            _scaleHeightArray = _scaleHeightArray.OrderBy(x => random.Next()).ToArray();

            // 最初は非表示
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ビート情報
        /// </summary>
        private class BeatInfo
        {
            public int BeatCount = -1;
            public float Duration = 0f;
            public float StartScaleHeight = 0f;
        }
        private BeatInfo _currentBeatInfo = null;

        /// <summary>
        /// 移動させるまでの時間
        /// </summary>
        private const float MoveDuration = 0.05f;

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update()
        {
            if (!_isInitialized || _currentBeatInfo == null)
            {
                return;
            }

            // 経過時間に合わせて補間
            _currentBeatInfo.Duration += Time.deltaTime;

            // ビート数に対応するindexの値を取得し、補間しながら移動させる
            var targetScaleHeight = _scaleHeightArray[_currentBeatInfo.BeatCount - 1];
            var scaleHeight = Mathf.Lerp(_currentBeatInfo.StartScaleHeight, targetScaleHeight,
                Mathf.Min(1f, _currentBeatInfo.Duration / MoveDuration));
            SetScaleHeight(scaleHeight);
        }

        /// <summary>
        /// ビート同期
        /// </summary>
        /// <param name="beatCount"></param>
        public void OnBeatSync(int beatCount)
        {
            if (!_isInitialized)
            {
                return;
            }

            // ビート情報を設定
            _currentBeatInfo = new BeatInfo()
            {
                BeatCount = beatCount,
                Duration = 0f,
                StartScaleHeight = transform.localScale.y,
            };

            // 表示する
            if (!gameObject.activeSelf)
            {
                SetScaleHeight(_scaleHeightArray[beatCount - 1]); // 最初だけ設定しておく
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Scale高さを設定
        /// </summary>
        /// <param name="scaleHeight"></param>
        private void SetScaleHeight(float scaleHeight)
        {
            // スケール、位置を変更する
            var scale = transform.localScale;
            scale.y = scaleHeight;
            transform.localScale = scale;

            var position = transform.position;
            position.y = scaleHeight / 2f;
            transform.position = position;
        }
    }
}
