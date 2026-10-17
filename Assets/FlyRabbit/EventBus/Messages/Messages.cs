/*
 * 你可以将消息类全部写在这个文件里避免零碎文件过多
 */
namespace FlyRabbit.EventBus
{
    //public class TestEventMessage : IEventMessage
    //{
    //    public string Text = "这是一个测试用事件消息,实际使用时你可以删除";
    //}

    /// <summary>当分辨率改变时</summary>
    public class OnResolutionChanged : IEventMessage
    {
        public static readonly OnResolutionChanged Instance = new OnResolutionChanged();
    }
    /// <summary>当主摄像机大小变化时</summary>
    public class OnMainCameraSizeChanged : IEventMessage
    {
        public static readonly OnMainCameraSizeChanged Instance = new OnMainCameraSizeChanged();
    }
}
