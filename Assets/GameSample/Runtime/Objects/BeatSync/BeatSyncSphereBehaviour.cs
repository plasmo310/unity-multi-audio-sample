using UnityEngine;

namespace GameSample.Objects.BeatSync
{
    /// <summary>
    /// ビート同期Sphereオブジェクト
    /// OnBeatSyncが呼ばれる度に弾むようにscaleが変化する
    /// </summary>
    public class BeatSyncSphereBehaviour : MonoBehaviour, IBeatSyncBehaviour
    {
        /// <summary>
        /// 初期化済か？
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// 初期Scale
        /// </summary>
        private float _initScale;

        /// <summary>
        /// 最大Scale
        /// </summary>
        private float _maxScale;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="initScale"></param>
        /// <param name="maxScale"></param>
        public void OnInitialize(float initScale, float maxScale)
        {
            _isInitialized = true;
            _maxScale = maxScale;
            _initScale = initScale;
            SetScale(initScale);

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
        }
        private BeatInfo _currentBeatInfo = null;

        /// <summary>
        /// 移動させるまでの時間
        /// </summary>
        private const float MoveDuration = 0.15f;

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

            // 最大スケール -> 初期スケール に変化させる
            var startScale = _maxScale;
            var targetScale = _initScale;
            var scale = Mathf.Lerp(startScale, targetScale,
                Mathf.Min(1f, _currentBeatInfo.Duration / MoveDuration));
            SetScale(scale);
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
            };

            // 表示する
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Scale設定
        /// </summary>
        /// <param name="scale"></param>
        private void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
