namespace FlyRabbit.SaveSystem
{
    public static class SaveDefine
    {
        #region 程序设置相关
        /// <summary>背景音乐音量</summary>
        public static readonly SaveKey<int> BGMVolume = new SaveKey<int>(nameof(BGMVolume), 100, Sanitizers.ClampInt0To100);

        /// <summary>音效音量</summary>
        public static readonly SaveKey<int> SFXVolume = new SaveKey<int>(nameof(SFXVolume), 100, Sanitizers.ClampInt0To100);

        /// <summary>目标帧率</summary>
        public static readonly SaveKey<int> TargetFrameRate = new SaveKey<int>(nameof(TargetFrameRate), 60, Sanitizers.ClampIntToNonNegative);
       
        #endregion
    }
}
