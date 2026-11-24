using DG.Tweening;
using FlyRabbit.ExtensionMethods;
using FlyRabbit.UIFramework;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIPanel
{
    [SerializeField]
    private RectTransform m_Menu;
    [SerializeField]
    private Button m_OpenMenuButton;
    [SerializeField]
    [BoxGroup("Menu页按钮")]
    private Button m_RestartButton;
    [SerializeField]
    [BoxGroup("Menu页按钮")]
    private Button m_SelectMapButton;
    [SerializeField]
    [BoxGroup("Menu页按钮")]
    private Button m_BackToMainMenuButton;
    [SerializeField]
    [BoxGroup("Menu页按钮")]
    private Button m_ContinueGameButton;
    [SerializeField]
    [BoxGroup("Menu页按钮")]
    private Button m_AutoSolveButton;


    private UIEntranceEffect[] m_MenuItems;
    private PlayPanelOpenData m_OpenData;

    private void Awake()
    {
        m_MenuItems = m_Menu.GetComponentsInChildren<UIEntranceEffect>();

        m_OpenMenuButton.onClick.AddListener(OnOpenMenuButtonClick);
        m_RestartButton.onClick.AddListener(OnRestartButtonClick);
        m_SelectMapButton.onClick.AddListener(OnSelectMapButtonClick);
        m_BackToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClick);
        m_ContinueGameButton.onClick.AddListener(OnContinueGameButtonClick);
        m_AutoSolveButton.onClick.AddListener(OnAutoSolveButtonClick);
    }
    private void Start()
    {
        //Menu起始为关闭状态
        DOMenuClose();
        //播放音乐
        AudioManager.PlayBGM(AudioPath.BGM.Appreciate);
    }

    #region Override Methods
    public override void OnOpen(OpenData openData)
    {
        m_OpenData = openData as PlayPanelOpenData;

        MapManager.Instance.OpenMap(m_OpenData.MapType, m_OpenData.MapName);
    }

    public override void OnClose()
    {
        MapManager.Instance.ClearMap();
    }

    public override void OnEscape()
    {
        m_OpenMenuButton.SimulateClick();
    }
    #endregion

    [Button("Do Menu Entrance")]
    private void DoMenuEntrance()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.SetEase(Ease.Linear);
        sequence.Pause();
        m_Menu.gameObject.SetActive(true);
        foreach (var item in m_MenuItems)
        {
            sequence.AppendInterval(0.015f);
            sequence.AppendCallback(item.Play);
        }

        sequence.Play();
    }
    [Button("Do Menu Close")]
    private void DOMenuClose()
    {
        m_Menu.gameObject.SetActive(false);
        foreach (var item in m_MenuItems)
        {
            item.ResetState();
        }
    }

    #region Listeners


    private void OnOpenMenuButtonClick()
    {
        if (m_Menu.gameObject.activeSelf == true)
        {
            DOMenuClose();
        }
        else
        {
            DoMenuEntrance();
        }
       
    }
    private void OnRestartButtonClick()
    {
        MapManager.Instance.OpenMap(m_OpenData.MapType, m_OpenData.MapName);
        DOMenuClose();
    }
    private void OnSelectMapButtonClick()
    {
        void OnOpenMapCallback(MapType mapType, string mapName)
        {
            UIManager.CloseAllPanels();
            UIManager.OpenPanel<PlayPanel>(new PlayPanelOpenData(mapType, mapName));
        }

        SelectMapPanelOpenData openData = new SelectMapPanelOpenData(OnOpenMapCallback);
        UIManager.OpenPanel<SelectMapPanel>(openData);
    }
    private void OnBackToMainMenuButtonClick()
    {
        UIManager.CloseAllPanels();
        UIManager.OpenPanel<MainPanel>();
    }
    private void OnContinueGameButtonClick()
    {
        DOMenuClose();
    }
    private void OnAutoSolveButtonClick()
    {

    }
    #endregion
}
