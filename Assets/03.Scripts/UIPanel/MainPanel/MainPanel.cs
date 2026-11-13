using UnityEngine;
using FlyRabbit.UIFramework;
using DG.Tweening;
using UnityEngine.UI;
using System;
public class MainPanel : UIPanel
{
    [SerializeField]
    private RectTransform m_Title;
    [SerializeField]
    private RectTransform m_ButtonList;
    [Space(25)]
    [SerializeField]
    private Button m_StartGameButton;
    [SerializeField]
    private Button m_MapWorkshopButton;
    [SerializeField]
    private Button m_SettingsButton;


    private float m_AnimationOffset = 100;
    private float m_AnimationDuration = 0.2f;


    private void Awake()
    {
        m_StartGameButton.onClick.AddListener(OnStartGameButtonClick);
        m_SettingsButton.onClick.AddListener(OnSettingsButtonClick);
        m_MapWorkshopButton.onClick.AddListener(OnMapWorkshopButtonClick);
    }

    private void Start()
    {
        AudioManager.PlayBGM(AudioPath.BGM.Home);
        PlayOpenAnimation();

    }

    public override void OnEscape()
    {
        AudioManager.PlaySFX(AudioPath.SFX.WaterDrop);
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.QuitGame);
    }


    /// <summary>播放打开动画</summary>
    private void PlayOpenAnimation()
    {
        //初始化
        CanvasGroup.alpha = 0;
        m_Title.position += Vector3.up * m_AnimationOffset;
        m_ButtonList.position -= Vector3.up * m_AnimationOffset;


        //淡入动画
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.Append(CanvasGroup.DOFade(1, m_AnimationDuration));
        sequence.Join(m_Title.DOMoveY(m_Title.position.y - m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
        sequence.Join(m_ButtonList.DOMoveY(m_ButtonList.position.y + m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
    }
    /// <summary>播放关闭动画</summary>
    /// <param name="callback"></param>
    private void PlayCloseAnimation(Action callback)
    {
        CanvasGroup.interactable = false;

        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.Append(CanvasGroup.DOFade(0, m_AnimationDuration));
        sequence.Join(m_Title.DOMoveY(m_Title.position.y + m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
        sequence.Join(m_ButtonList.DOMoveY(m_ButtonList.position.y - m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
        sequence.AppendCallback(() => { callback?.Invoke(); });
    }

    #region Listeners
    private void OnSettingsButtonClick()
    {
        UIManager.OpenPanel<SettingsPanel>();
    }

    private void OnMapWorkshopButtonClick()
    {
        PlayCloseAnimation(() =>
        {
            UIManager.CloseAllPanels();
            UIManager.OpenPanel<MapWorkshopPanel>();
        });
    }


    private void OnStartGameButtonClick()
    {
        void OnOpenMapCallback(MapType mapType, string mapName)
        {
            PlayPanelOpenData playPanelOpenData = new PlayPanelOpenData(mapType, mapName);
            UIManager.CloseAllPanels();
            UIManager.OpenPanel<PlayPanel>(playPanelOpenData);
        }

        SelectMapPanelOpenData selectMapPanelOpenData = new SelectMapPanelOpenData(OnOpenMapCallback);

        UIManager.OpenPanel<SelectMapPanel>(selectMapPanelOpenData);
    }
    #endregion


}
