using FlyRabbit.EventBus;
using FlyRabbit.UI;
using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapWorkshopPanel : UIPanel
{
    [SerializeField]
    private Button m_InputCanvasButton;
    [SerializeField]
    private Button m_SaveButton;
    [SerializeField]
    private Button m_OpenButton;
    [SerializeField]
    private Button m_ClearButton;
    [SerializeField]
    private Button m_ExitButton;

    [SerializeField]
    private ShineEffect m_ShineEffect;


    private CameraSizeAdapter m_CameraSizeAdapter;
    private PopupMessageOpenData m_SaveMapTips = new PopupMessageOpenData(null, true, null);

    private SelectMapPanelOpenData m_SelectMapPanelOpenData = new SelectMapPanelOpenData((mapType, mapName) =>
    {
        MapManager.Instance.OpenMap(mapType, mapName);
    });
    private string m_SelectedItemName;

    #region Unity Methods

    private void Awake()
    {
        m_CameraSizeAdapter = Camera.main.GetComponent<CameraSizeAdapter>();


        m_InputCanvasButton.onClick.AddListener(OnInputCanvasButtonClick);
        m_SaveButton.onClick.AddListener(OnSaveButtonClick);
        m_OpenButton.onClick.AddListener(OnOpenButtonClick);
        m_ExitButton.onClick.AddListener(OnExitButtonClick);
        m_ClearButton.onClick.AddListener(OnClearButtonClick);

    }
    private void Start()
    {
        AudioManager.PlayBGM(AudioPath.BGM.Appreciate);
    }
    private void OnEnable()
    {
        EventBus.AddListener<EventMessage.UI.OnMapWorkshopItemSelected>(OnMapWorkshopItemSelected);


        MapManager.Instance.SetMapGridVisible(true);
        m_CameraSizeAdapter?.SetEnableOffsset(true);
    }

    private void OnDisable()
    {
        EventBus.RemoveListener<EventMessage.UI.OnMapWorkshopItemSelected>(OnMapWorkshopItemSelected);

        MapManager.Instance.SetMapGridVisible(false);
        m_CameraSizeAdapter?.SetEnableOffsset(false);
    }
    #endregion



    #region Override
    public override void OnClose()
    {
        MapManager.Instance.ClearMap();
    }
    #endregion


    #region Listeners


    private void OnSaveButtonClick()
    {
        int foxCount = MapManager.Instance.GetItemCount<Fox>();
        int gemCount = MapManager.Instance.GetItemCount<Gem>();
        int doorCount = MapManager.Instance.GetItemCount<Door>();

        if (foxCount == 1 && gemCount > 0 && doorCount == 1)
        {
            UIManager.OpenPanel<SaveMapPanel>();
        }
        else
        {
            StringBuilder message = new StringBuilder(50);
            message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.InvalidMap));

            if (foxCount == 0)
            {
                message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.RequireOneFox));
            }
            else if (foxCount > 1)
            {
                message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.CanOnlyHaveOneFox));
            }

            if (gemCount == 0)
            {
                message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.RequireOneGem));
            }

            if (doorCount == 0)
            {
                message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.RequireOneDoor));
            }
            else if (doorCount > 1)
            {
                message.AppendLine(LocalizationManager.GetLocalizedString(LocalizationKey.CanOnlyHaveOneDoor));
            }
            m_SaveMapTips.Message = message.ToString();
            UIManager.OpenPanel<PopupMessagePanel>(m_SaveMapTips);
        }

    }


    private void OnOpenButtonClick()
    {
        UIManager.OpenPanel<SelectMapPanel>(m_SelectMapPanelOpenData);
    }
    private void OnClearButtonClick()
    {
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.ClearMap);
    }

    private void OnExitButtonClick()
    {
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.ExitMapWorkshop);
    }

    private void OnInputCanvasButtonClick()
    {
        Vector2 worldPosition;
        Vector2Int mapPosition;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
#elif UNITY_ANDROID
        worldPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.touches[0].startPosition.ReadValue());
#endif
        bool isInsideMap = MapManager.Instance.WorldToMap(worldPosition, out mapPosition);

        if (isInsideMap == false)
        {
            return;
        }
        //如果没有选中物体，则移除该位置的物体，否则放置选中的物体
        if (string.IsNullOrEmpty(m_SelectedItemName))
        {
            if (MapManager.Instance.GetItem(mapPosition.x, mapPosition.y) != null)
            {
                AudioManager.PlaySFX(AudioPath.SFX.Close);
                MapManager.Instance.RemoveItem(mapPosition.x, mapPosition.y);
                Debug.Log($"[地图工坊]：移除了地图坐标{mapPosition}上的物体");
            }
        }
        else
        {
            AudioManager.PlaySFX(AudioPath.SFX.Open);
            MapManager.Instance.PlaceItem(mapPosition.x, mapPosition.y, m_SelectedItemName);
            Debug.Log($"[地图工坊]：在地图坐标{mapPosition}上放置了物体{m_SelectedItemName}");
        }

    }

    private void OnMapWorkshopItemSelected(EventMessage.UI.OnMapWorkshopItemSelected message)
    {
        m_ShineEffect.transform.SetParent(message.ItemSelector.transform, false);
        m_ShineEffect.Play();
        m_SelectedItemName = message.ItemSelector.ItemName;
    }
    #endregion
}