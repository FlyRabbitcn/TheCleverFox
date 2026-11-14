using FlyRabbit.UIFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindControlsPanelOpenData : OpenData
{
    /// <summary>控制名</summary>
    public string ActionDisplayName;
    /// <summary>动作名</summary>
    public string ActionName;
    /// <summary>索引</summary>
    public int Index;
    /// <summary>重新绑定按键后的回调</summary>
    public Action Callback;

    public RebindControlsPanelOpenData(string actionDisplayName, string actionName, int index, Action callback)
    {
        ActionDisplayName = actionDisplayName;
        ActionName = actionName;
        Index = index;
        Callback = callback;
    }

    public RebindControlsPanelOpenData()
    {
        
    }
}
