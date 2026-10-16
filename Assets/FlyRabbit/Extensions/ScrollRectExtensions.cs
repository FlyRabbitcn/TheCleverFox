using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions
{
    private const float m_DefaultDuration = 0.2f;

    /// <summary>
    /// 在指定时间内将 ScrollRect 滚动，使 target 尽量处于 viewport 中心；无法居中时钳制到可滚动边界。
    /// </summary>
    public static Tweener ScrollToItem(this ScrollRect scrollRect, RectTransform target, float duration = m_DefaultDuration)
    {
        if (scrollRect == null || target == null || scrollRect.content == null || scrollRect.viewport == null)
            return null;

        if (!target.IsChildOf(scrollRect.content))
            return null;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        Vector2 targetNormalized = GetNormalizedPositionToCenter(scrollRect, target);

        scrollRect.DOKill();

        if (duration <= 0f)
        {
            if (scrollRect.horizontal)
                scrollRect.horizontalNormalizedPosition = targetNormalized.x;
            if (scrollRect.vertical)
                scrollRect.verticalNormalizedPosition = targetNormalized.y;
            return null;
        }

        return scrollRect.DONormalizedPos(targetNormalized, duration).SetEase(Ease.OutCubic);
    }

    private static Vector2 GetNormalizedPositionToCenter(ScrollRect scrollRect, RectTransform target)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        Vector2 itemCenterInViewport = (Vector2)viewport.InverseTransformPoint(
            target.TransformPoint(target.rect.center));
        Vector2 offset = viewport.rect.center - itemCenterInViewport;

        Vector2 targetAnchoredPos = content.anchoredPosition;
        if (scrollRect.horizontal)
            targetAnchoredPos.x += offset.x;
        if (scrollRect.vertical)
            targetAnchoredPos.y += offset.y;

        return ClampToNormalizedPosition(scrollRect, targetAnchoredPos);
    }

    private static Vector2 ClampToNormalizedPosition(ScrollRect scrollRect, Vector2 anchoredPosition)
    {
        RectTransform content = scrollRect.content;
        Vector2 original = content.anchoredPosition;

        content.anchoredPosition = anchoredPosition;
        Canvas.ForceUpdateCanvases();

        Vector2 normalized = new Vector2(
            scrollRect.horizontalNormalizedPosition,
            scrollRect.verticalNormalizedPosition);

        if (scrollRect.horizontal)
            normalized.x = Mathf.Clamp01(normalized.x);
        if (scrollRect.vertical)
            normalized.y = Mathf.Clamp01(normalized.y);

        content.anchoredPosition = original;
        return normalized;
    }
}
