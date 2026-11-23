using FlyRabbit.EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    #region Private Methods
    /// <summary>添加监听</summary>
    private static void AddListener()
    {
        EventBus.AddListener<EventMessage.Gameplay.OnPlayerTryOpenDoor>(OnPlayerTryOpenDoor);
    }
    /// <summary>移除监听</summary>
    private static void RemoveListener()
    {
        EventBus.RemoveListener<EventMessage.Gameplay.OnPlayerTryOpenDoor>(OnPlayerTryOpenDoor);
    }
    #endregion

    #region Listeners
    private static void OnPlayerTryOpenDoor(EventMessage.Gameplay.OnPlayerTryOpenDoor arg)
    {

    }
    #endregion


    static LevelManager()
    {
        
    }
}
