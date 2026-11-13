using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIEntranceEffect : MonoBehaviour
{
    public float Duration = 0.1f;
    public Vector2 Offset = new Vector2(-100f, 0f);

    private CanvasGroup m_CanvasGroup;
    private RectTransform m_RectTransform;

    private Vector2 m_HomePosition;
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_RectTransform = GetComponent<RectTransform>();

        m_HomePosition = m_RectTransform.anchoredPosition;
    }
    /// <summary>播放入场</summary>
    [Button("Play")]
    public void Play()
    {
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.SetEase(Ease.Linear);
        sequence.Append(m_RectTransform.DOAnchorPos(m_HomePosition, Duration).From(m_HomePosition + Offset));
        sequence.Join(m_CanvasGroup.DOFade(1f, Duration).From(0f));
    }
    /// <summary>重置到退场状态</summary>
    public void ResetState()
    {
        m_RectTransform.anchoredPosition = m_HomePosition;
        gameObject.SetActive(false);
    }
}
