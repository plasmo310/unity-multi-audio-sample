namespace GameSample.Audio
{
    /// <summary>
    /// ゲーム内サウンド設定 共通アクセス項目
    /// </summary>
    public interface IGameAudioSettings
    {
        /// <summary>
        /// SoundEvent名
        /// </summary>
        public string SoundEventName_BgmSpaceWould { get; }
        public string SoundEventName_BgmShotThunder  { get; }
        public string SoundEventName_BgmAtomChain  { get; }
        public string SoundEventName_SeAttack { get; }
        public string SoundEventName_SeMove { get; }

        /// <summary>
        /// SoundSheet名
        /// </summary>
        public string SoundSheetName_Bgm { get; }
        public string SoundSheetName_Se { get; }

        /// <summary>
        /// GameParameter名
        /// </summary>
        public string GameParameterName_Battle { get; }

        /// <summary>
        /// ビート同期ラベル名
        /// </summary>
        public string BeatSyncLabelName_AtomChain { get; }

        /// <summary>
        /// カスタムイベント名
        /// </summary>
        public string CustomEventName_StartAtomChainMainLoop { get; }
    }
}
