using FlyRabbit.EventBus;
using FlyRabbit.UI;
using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using System.IO;
using NaughtyAttributes;
using DG.Tweening;
using FlyRabbit.SaveSystem;
using System;

public class SelectMapPanel : UIPanel
{
    [SerializeField]
    private Button m_BackButton;
    [SerializeField]
    private Button m_OpenButton;
    [SerializeField]
    private Button m_DeleteButton;
    [SerializeField]
    private ShineEffect m_ShineEffect;
    [SerializeField]
    private MapViewer m_MapViewerTemplate;
    [SerializeField]
    private RectTransform m_Content;
    [SerializeField]
    [BoxGroup("切换Toggle相关")]
    private Toggle m_LevelToggle;
    [SerializeField]
    [BoxGroup("切换Toggle相关")]
    private Toggle m_CustomToggle;
    [SerializeField]
    [BoxGroup("切换Toggle相关")]
    private RectTransform m_Foreground;

    private MapViewer m_SelectedMapViewer;
    private Transform m_MapViewerPoolRoot;
    private ObjectPool<MapViewer> m_MapViewerPool;
    private SelectMapPanelOpenData m_OpenData;
    /// <summary>最后一次选中的切换Toggle，用于处理重复加载问题，我真吐了这Unity Toggle Group的逻辑</summary>
    private Toggle m_LastSelectedToggle;
    /// <summary>当前选中的MapViewer</summary>
    private MapViewer SelectedMapViewer
    {
        get
        {
            return m_SelectedMapViewer;
        }
        set
        {
            m_SelectedMapViewer = value;
            //根据是否有选中地图，设置打开和删除按钮的可交互性
            bool hasSelectedMap = m_SelectedMapViewer != null;
            m_OpenButton.interactable = hasSelectedMap;
            m_DeleteButton.interactable = hasSelectedMap;
            if (hasSelectedMap && m_SelectedMapViewer.MapType == MapType.Level)
            {
                m_DeleteButton.interactable = false;
            }
        }
    }
    private void Awake()
    {
        //添加监听
        m_BackButton.onClick.AddListener(OnBackButtonClick);
        m_OpenButton.onClick.AddListener(OnOpenButtonClick);
        m_DeleteButton.onClick.AddListener(OnDeleteButtonClick);

        m_LevelToggle.onValueChanged.AddListener(OnValueChange_LevelToggle);
        m_CustomToggle.onValueChanged.AddListener(OnValueChange_CustomToggle);
        //初始化对象池
        m_MapViewerPoolRoot = new GameObject("MapViewerPool").transform;
        m_MapViewerPoolRoot.SetParent(this.transform, false);
        m_MapViewerPool = new ObjectPool<MapViewer>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 50);
    }
    private void Start()
    {
        //打开时默认加载关卡地图
        LoadMapViewers(MapType.Level);
        //因为打卡时没有选中任何地图，因此禁用打开和删除按钮
        SelectedMapViewer = null;
    }
    private void OnEnable()
    {
        EventBus.AddListener<OnMapViewerSelected>(OnMapViewerSelected);
    }

    private void OnDisable()
    {
        EventBus.RemoveListener<OnMapViewerSelected>(OnMapViewerSelected);
    }


    public override void OnOpen(OpenData openData)
    {
        m_OpenData = openData as SelectMapPanelOpenData;
    }


    /// <summary>加载指定MapType的所有地图，会先清空后加载</summary>
    /// <param name="mapType"></param>
    private void LoadMapViewers(MapType mapType)
    {
        ClearContent();
        switch (mapType)
        {
            case MapType.Level:
                LoadLevelMapViewers();
                break;
            case MapType.Custom:
                LoadCustomMapViewers();
                break;
            default:
                break;
        }
    }
    /// <summary>清空目录，回收所有MapViewer</summary>
    private void ClearContent()
    {


        //回收所有MapViewer
        foreach (var mapViewer in m_Content.GetComponentsInChildren<MapViewer>(true))
        {
            if (mapViewer != null)
            {
                m_MapViewerPool.Release(mapViewer);
            }
        }
    }
    /// <summary>加载关卡地图数据，使用的Addressable</summary>
    private void LoadLevelMapViewers()
    {
        //获取关卡进度
        int levelProgress = SaveManager.GetValue(SaveDefine.LevelProgress);
        //生成关卡地图Viewer
        for (int i = 1; i <= levelProgress; i++)
        {
            MapViewer mapViewer = m_MapViewerPool.Get();
            mapViewer.transform.SetParent(m_Content, false);
            mapViewer.SetMapInfo(MapType.Level, i.ToString());
        }
    }
    /// <summary>加载自定义地图数据，使用的本地文件</summary>
    private void LoadCustomMapViewers()
    {
        //获取自定义地图根目录
        string rootDirectoryPath = FileManager.CustomMapRootDirectory;
        //如果目录不存在，说明没有自定义地图，直接返回
        if (Directory.Exists(rootDirectoryPath) == false)
        {
            return;
        }
        //获取所有自定义地图文件夹
        string[] mapDirectories = Directory.GetDirectories(rootDirectoryPath);
        //遍历所有自定义地图文件夹，创建MapViewer
        foreach (string mapDirectory in mapDirectories)
        {
            MapViewer mapViewer = m_MapViewerPool.Get();
            mapViewer.transform.SetParent(m_Content, false);
            mapViewer.SetMapInfo(MapType.Custom, Path.GetFileName(mapDirectory));
        }
    }

    /// <summary>前景跟随</summary>
    private void ForegroundFollow(Transform target)
    {
        m_Foreground.SetParent(target);
        m_Foreground.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.Linear).SetLink(this.gameObject);
    }

    #region Listeners
    private void OnBackButtonClick()
    {
        CloseSelf();
    }


    private void OnOpenButtonClick()
    {
        CloseSelf();
        m_OpenData.OnOpenMapCallback?.Invoke(SelectedMapViewer.MapType, SelectedMapViewer.MapName);
    }


    private void OnDeleteButtonClick()
    {
        if (SelectedMapViewer == null || SelectedMapViewer.MapType == MapType.Level)
        {
            return;
        }

        string message = LocalizationManager.GetLocalizedString(LocalizationKey.ConfirmDelete);
        void ConfirmCallback()
        {
            FileManager.DeleteCustomMap(SelectedMapViewer.MapName);
            m_MapViewerPool.Release(SelectedMapViewer);
            SelectedMapViewer = null;
        }
        PopupMessageOpenData openData = new PopupMessageOpenData(message,false, ConfirmCallback);

        UIManager.OpenPanel<PopupMessagePanel>(openData);
    }

    private void OnValueChange_LevelToggle(bool isOn)
    {
        if (isOn == false)
        {
            return;
        }
        if (m_LastSelectedToggle == m_LevelToggle)
        {
            return;
        }
        SelectedMapViewer = null;
        m_LastSelectedToggle = m_LevelToggle;
        AudioManager.PlaySFX(AudioPath.SFX.Knock);
        ForegroundFollow(m_LevelToggle.transform);
        LoadMapViewers(MapType.Level);
    }

    private void OnValueChange_CustomToggle(bool isOn)
    {
        if (isOn == false)
        {
            return;
        }
        if (m_LastSelectedToggle == m_CustomToggle)
        {
            return;
        }
        SelectedMapViewer = null;
        m_LastSelectedToggle = m_CustomToggle;
        AudioManager.PlaySFX(AudioPath.SFX.Knock);
        ForegroundFollow(m_CustomToggle.transform);
        LoadMapViewers(MapType.Custom);
    }
    private void OnMapViewerSelected(OnMapViewerSelected message)
    {
        m_ShineEffect.transform.SetParent(message.MapViewer.transform, false);
        m_ShineEffect.Play();
        SelectedMapViewer = message.MapViewer;
    }
    #endregion

    #region MapViewerPool Method
    private MapViewer CreateFunc()
    {
        MapViewer mapViewer = Instantiate(m_MapViewerTemplate, m_MapViewerPoolRoot);
        mapViewer.gameObject.SetActive(true);
        return mapViewer;
    }

    private void ActionOnGet(MapViewer mapViewer)
    {
        mapViewer.gameObject.SetActive(true);
    }

    private void ActionOnRelease(MapViewer mapViewer)
    {
        (mapViewer as IPoolReleaseHandler)?.OnPoolRelease();
        mapViewer.gameObject.SetActive(false);
        mapViewer.transform.SetParent(m_MapViewerPoolRoot, false);
    }

    private void ActionOnDestroy(MapViewer mapViewer)
    {
        Destroy(mapViewer.gameObject);
    }
    #endregion
}
