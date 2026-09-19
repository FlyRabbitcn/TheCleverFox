using UnityEngine;
using UnityEngine.UI;

namespace FlyRabbit.UIFramework
{
    public static class CanvasSettings
    {
        /// <summary>参考分辨率</summary>
        public static readonly Vector2 ReferenceResolution = new Vector2(1920, 1080);

        /// <summary>宽高比匹配值</summary>
        public static readonly float MatchWidthOrHeight = 1;

        /// <summary>
        /// 根据设置创建Canvas
        /// </summary>
        public static Canvas CreateCanvas(int sortingOrder, Transform parrent)
        {
            GameObject gameObject = new GameObject($"Canvas[{sortingOrder}]");

            gameObject.AddComponent<RectTransform>();
            gameObject.transform.SetParent(parrent, false);

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;
            canvas.vertexColorAlwaysGammaSpace = true;

            CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = ReferenceResolution;
            canvasScaler.matchWidthOrHeight = MatchWidthOrHeight;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

            return canvas;
        }
    }
}
