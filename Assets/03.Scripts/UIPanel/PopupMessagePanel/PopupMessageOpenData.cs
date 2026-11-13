using FlyRabbit.UIFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessageOpenData : OpenData
{
    public string Message;
    public bool IsHideCancelButton;
    public Action ConfirmCallback;

    public PopupMessageOpenData(string message, bool isHideCancelButton, Action confirmCallback)
    {
        Message = message;
        IsHideCancelButton = isHideCancelButton;
        ConfirmCallback = confirmCallback;
    }


    /// <summary>退出游戏时的弹窗</summary>
    public static PopupMessageOpenData QuitGame
    {
        get
        {
            m_QuitGame.Message = LocalizationManager.GetLocalizedString(LocalizationKey.QuitGameMessage);
            return m_QuitGame;
        }
    }
    /// <summary>退出地图工坊时的弹窗</summary>
    public static PopupMessageOpenData ExitMapWorkshop
    {
        get
        {
            m_ExitMapWorkshop.Message = LocalizationManager.GetLocalizedString(LocalizationKey.ExitMapWorkshopMessage);
            return m_ExitMapWorkshop;
        }

    }

    /// <summary>清空地图时的弹窗</summary>
    public static PopupMessageOpenData ClearMap
    {
        get
        {
            m_ClearMap.Message = LocalizationManager.GetLocalizedString(LocalizationKey.ClearMapMessage);
            return m_ClearMap;
        }
    }
    /// <summary>保存成功弹窗</summary>
    public static PopupMessageOpenData SaveSuccess
    {
        get
        {
            m_SaveSuccess.Message = LocalizationManager.GetLocalizedString(LocalizationKey.SaveSuccess);
            return m_SaveSuccess;
        }
    }
    private static readonly PopupMessageOpenData m_QuitGame = new PopupMessageOpenData(string.Empty, false, () =>
    {
        Application.Quit();
    });

    private static readonly PopupMessageOpenData m_ExitMapWorkshop = new PopupMessageOpenData(string.Empty, false, () =>
    {
        UIManager.CloseAllPanels();
        UIManager.OpenPanel<MainPanel>();
    });

    private static readonly PopupMessageOpenData m_ClearMap = new PopupMessageOpenData(string.Empty, false, () =>
    {
        MapManager.Instance.ClearMap();
    });

    private static readonly PopupMessageOpenData m_SaveSuccess = new PopupMessageOpenData(string.Empty, true, null);
}
