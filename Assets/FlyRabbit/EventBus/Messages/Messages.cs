/*
 * 你可以将消息类全部写在这个文件里避免零碎文件过多
 */
namespace FlyRabbit.EventBus
{
    public class EventMessage
    {
        /// <summary>UI相关事件</summary>
        public class UI
        {
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
            /// <summary>当地图工坊内选中Item时</summary>
            public class OnMapWorkshopItemSelected : IEventMessage
            {
                public MapWorkshopItemSelector ItemSelector;
            }
            /// <summary>当选择地图页面选中MapViewer时</summary>
            public class OnMapViewerSelected : IEventMessage
            {
                public MapViewer MapViewer;
            }
        }
        /// <summary>输入系统相关事件</summary>
        public class InputSystem
        {
            /// <summary>当有binding被清空后触发</summary>
            public class OnBindingCleared : IEventMessage
            {
                public static readonly OnBindingCleared Instance = new OnBindingCleared();
            }
        }
        /// <summary>游戏玩法相关事件</summary>
        public class Gameplay
        {
            /// <summary>当玩家尝试打开门（每帧调用）</summary>
            public class OnPlayerTryOpenDoor : IEventMessage
            {
                public static readonly OnPlayerTryOpenDoor Instance = new OnPlayerTryOpenDoor();
            }
            /// <summary>当玩家收集了Gem之后</summary>
            public class OnPlayerCollectedGem : IEventMessage
            {
                public static readonly OnPlayerCollectedGem Instance = new OnPlayerCollectedGem();
            }
        }
    }
}
