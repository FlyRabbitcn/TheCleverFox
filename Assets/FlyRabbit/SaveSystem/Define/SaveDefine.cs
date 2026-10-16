namespace FlyRabbit.SaveSystem
{
    public static class SaveDefine
    {
        #region 声音设置
        /// <summary>背景音乐音量</summary>
        public static readonly SaveKey<int> BGMVolume = new SaveKey<int>(nameof(BGMVolume), 100, Sanitizers.ClampInt0To100);
        /// <summary>音效音量</summary>
        public static readonly SaveKey<int> SFXVolume = new SaveKey<int>(nameof(SFXVolume), 100, Sanitizers.ClampInt0To100);  
        #endregion
        #region 显示设置
        /// <summary>是否使用显示刷新率</summary>
        public static readonly SaveKey<bool> UseDisplayRefreshRate = new SaveKey<bool>(nameof(UseDisplayRefreshRate), true);
        /// <summary>自定义帧率</summary>
        public static readonly SaveKey<int> CustomFrameRate = new SaveKey<int>(nameof(CustomFrameRate), 60, Sanitizers.ClampInt30To600);
        /// <summary>是否全屏(无边框窗口)</summary>
        public static readonly SaveKey<bool> IsFullScreen = new SaveKey<bool>(nameof(IsFullScreen), true);
        /// <summary>
        /// 分辨率方案
        /// 记录了当前显示器支持的分辨率方案中当前使用的那个方案的索引值
        /// </summary>
        public static readonly SaveKey<int> ResolutionScheme = new SaveKey<int>(nameof(ResolutionScheme), 0, Sanitizers.ClampResolutionSchemeIndex);
        #endregion
        #region 语言设置
        /// <summary>是否已选择语言</summary>
        public static readonly SaveKey<bool> HasSelectedLanguage = new SaveKey<bool>(nameof(HasSelectedLanguage), false);
        #endregion
    }
}
