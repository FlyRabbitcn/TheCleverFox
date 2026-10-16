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



    public static PopupMessageOpenData QuitGame
    {
        get
        {
            m_QuitGame.Message = LocalizationManager.GetLocalizedString(LocalizationKey.QuitGameMessage);
            return m_QuitGame;
        }
    }

    private static readonly PopupMessageOpenData m_QuitGame = new PopupMessageOpenData(string.Empty, false, () =>
    {
        Application.Quit();
    });
}
