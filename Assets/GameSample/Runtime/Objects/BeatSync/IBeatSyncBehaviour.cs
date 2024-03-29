namespace GameSample.Objects.BeatSync
{
    /// <summary>
    /// ビート同期オブジェクト
    /// </summary>
    public interface IBeatSyncBehaviour
    {
        /// <summary>
        /// ビート同期
        /// </summary>
        /// <param name="beatCount">ビート数</param>
        public void OnBeatSync(int beatCount);
    }
}
