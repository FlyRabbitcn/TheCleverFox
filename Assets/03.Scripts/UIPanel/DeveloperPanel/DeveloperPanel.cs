using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeveloperPanel : UIPanel
{
    [SerializeField]
    private Image m_Shadow;
    [SerializeField]
    private Image m_Background;
    [SerializeField]
    private Image m_Curtain;
    [SerializeField]
    private Button m_ExitButton;
    [SerializeField]
    private VerticalLayoutGroup m_ContentVerticalLayoutGroup;
    [SerializeField]
    private ScrollRect m_ScrollRect;
    [SerializeField]
    private RectTransform m_Viewport;
    [SerializeField]
    private RectTransform m_Content;
    [SerializeField]
    private RectTransform m_BottomSpace;
    //一行数据从底部滚动到顶部所需的时间
    private readonly float m_Speed = 15f;

    private string m_PreviousBGM;

    private void Awake()
    {
        m_ExitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void Start()
    {
        //隐藏退出按钮
        m_ExitButton.gameObject.SetActive(false);
        //让m_ContentVerticalLayoutGroup的padding.top等于视窗的高度,留出一个屏幕的空白
        m_ContentVerticalLayoutGroup.padding.top =  Mathf.RoundToInt(m_Viewport.rect.height);
        //Bottom等于视窗高度的一半,让最后一行数据滚动到屏幕中间时停下
        //m_ContentVerticalLayoutGroup.padding.bottom = Mathf.RoundToInt(m_Viewport.rect.height/2);
        // 将 BottomSpace 高度设置为视窗高度的一半
        m_BottomSpace.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Round(m_Viewport.rect.height / 2f));
        //重置ScrollRect垂直位置
        m_ScrollRect.verticalNormalizedPosition = 1;
        //停止播放BGM，播放一次水滴音效
        m_PreviousBGM = AudioManager.CurrentBGM;
        AudioManager.PlayBGM(null);
        AudioManager.PlaySFX(AudioPath.SFX.WaterDrop);
        //将一些UI元素的透明度设置为0
        m_Shadow.color = new Color(m_Shadow.color.r, m_Shadow.color.g, m_Shadow.color.b, 0);
        m_Curtain.color = new Color(m_Curtain.color.r, m_Curtain.color.g, m_Curtain.color.b, 0);
        m_Background.color = new Color(m_Background.color.r, m_Background.color.g, m_Background.color.b, 0);
        //开始入场动画
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.Append(m_Shadow.DOFade(1f, 0.5f));
        sequence.Append(m_Background.DOFade(1f, 0.2f));
        sequence.AppendCallback(() =>
        {
            AudioManager.PlayBGM(AudioPath.BGM.PenguinPop);
            float duration = ((m_Content.rect.height - Screen.currentResolution.height) / Screen.currentResolution.height) * m_Speed;
            m_ScrollRect.DOVerticalNormalizedPos(0, duration).SetLink(gameObject).SetEase(Ease.Linear);
        });
        sequence.AppendInterval(1.25f);
        sequence.AppendCallback(() =>
        {
            m_ExitButton.gameObject.SetActive(true);
        });
    }


    private void OnExitButtonClick()
    {
        CanvasGroup.interactable = false;
        AudioManager.PlayBGM(null);
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.Append(m_Curtain.DOFade(1f, 0.75f).SetEase(Ease.OutQuad));
        sequence.AppendCallback(() =>
        {
            CloseSelf();
            AudioManager.PlayBGM(m_PreviousBGM);
        });
    }
}
