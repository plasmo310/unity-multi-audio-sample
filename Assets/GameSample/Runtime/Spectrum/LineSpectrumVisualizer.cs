using UnityEngine;

namespace GameSample.Spectrum
{
    /// <summary>
    /// LineRendererによるスペクトラム周波数の描画
    /// </summary>
    public class LineSpectrumVisualizer : SpectrumVisualizer
    {
        [Tooltip("LineRendererの初期幅")]
        [Range(0f, 1f)]
        [SerializeField] float _initLineRendererWidth = 0.05f;

        private LineRenderer _lineRenderer;
        private Vector3[] _lineRendererPositionArray;

        protected override void InitializeRenderer()
        {
            base.InitializeRenderer();

            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.startWidth = _initLineRendererWidth;
            _lineRenderer.endWidth = _initLineRendererWidth;

            CreateLineRenderer(_rendererSampleCount, _rendererRange, RendererRootPosition);
        }

        protected override void UpdateRenderer(float[] dataArray)
        {
            base.UpdateRenderer(dataArray);

            // 情報の更新があったら作り直す
            if (UpdateLineRendererInfo())
            {
                CreateLineRenderer(_rendererSampleCount, _rendererRange, RendererRootPosition);
            }

            // Rendererの更新
            UpdateLineRenderer(dataArray);
        }

        private int _lineRendererSampleCount;
        private float _lineRendererRange;
        private Vector3 _lineRendererRootPosition;

        /// <summary>
        /// LineRendererに必要な情報の更新
        /// </summary>
        /// <returns></returns>
        private bool UpdateLineRendererInfo()
        {
            if (_rendererSampleCount != _lineRendererSampleCount
                || !Mathf.Approximately(_rendererRange, _lineRendererRange)
                || RendererRootPosition != _lineRendererRootPosition)
            {
                _lineRendererSampleCount = _rendererSampleCount;
                _lineRendererRange = _rendererRange;
                _lineRendererRootPosition = RendererRootPosition;
                return true;
            }
            return false;
        }

        /// <summary>
        /// LineRendererの作成
        /// </summary>
        /// <param name="sampleCount"></param>
        /// <param name="range"></param>
        /// <param name="rootPosition"></param>
        private void CreateLineRenderer(int sampleCount, float range, Vector3 rootPosition)
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = gameObject.GetComponent<LineRenderer>();
            }

            // 描画する点の数を設定
            _lineRenderer.positionCount = sampleCount;

            // 点の位置を計算する
            _lineRendererPositionArray = new Vector3[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                _lineRendererPositionArray[i] = GetLineRendererPosition(i, sampleCount, range, rootPosition, 0f);
            }
            _lineRenderer.SetPositions(_lineRendererPositionArray);

            _lineRendererSampleCount = sampleCount;
            _lineRendererRange = range;
            _lineRendererRootPosition = rootPosition;
        }

        /// <summary>
        /// LineRendererの更新処理
        /// </summary>
        /// <param name="dataArray"></param>
        private void UpdateLineRenderer(float[] dataArray)
        {
            // Renderer用にデータを整形して表示
            for (var i = 0; i < _lineRendererPositionArray.Length; i++)
            {
                _lineRendererPositionArray[i] = GetLineRendererPosition(i, _rendererSampleCount, _rendererRange, gameObject.transform.localPosition, dataArray[i]);
            }
            _lineRenderer.SetPositions(_lineRendererPositionArray);
        }

        /// <summary>
        /// LineRendererの位置情報を取得
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sampleCount"></param>
        /// <param name="range"></param>
        /// <param name="rootPosition"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        private Vector3 GetLineRendererPosition(int index, int sampleCount, float range, Vector3 rootPosition, float dataValue)
        {
            var x = range * (index / ((float) sampleCount / 2) - 1);
            var y = Mathf.Min(_rendererMaxHeight, (dataValue * _rendererFrequencyGain));
            return rootPosition + transform.right * x + transform.up * y;
        }
    }
}
