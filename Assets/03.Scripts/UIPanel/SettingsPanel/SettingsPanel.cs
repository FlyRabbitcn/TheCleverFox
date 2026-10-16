using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;
using DG.Tweening;
using UnityEngine.UI;
using FlyRabbit.SaveSystem;

public class SettingsPanel : UIPanel
{
    [SerializeField]
    private RectTransform m_Body;
    [SerializeField]
    private Button m_CloseButton;


    private float m_AnimationDuration = 0.15f;

    private void Awake()
    {
        //事件绑定
        m_CloseButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void Start()
    {
        //播放打开动画
        Vector2 BodySizeDelta = m_Body.sizeDelta;
        m_Body.sizeDelta = new Vector2(BodySizeDelta.x, 0);
        m_Body.DOSizeDelta(BodySizeDelta, m_AnimationDuration).SetEase(Ease.Linear).SetLink(gameObject);
    }


    private void OnDisable()
    {
        SaveManager.SaveToFile();
    }

    private void OnCloseButtonClick()
    {
        //播放关闭动画，然后关闭页面
        CanvasGroup.interactable = false;
        Vector2 sizeDelta = new Vector2(m_Body.sizeDelta.x, 0);
        m_Body.DOSizeDelta(sizeDelta, m_AnimationDuration).SetEase(Ease.Linear).SetLink(gameObject).onComplete = CloseSelf;
    }
}
