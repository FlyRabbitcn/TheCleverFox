using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;
using System;

public class SelectMapPanelOpenData : OpenData
{
    public Action<MapType, string> OnOpenMapCallback;

    public SelectMapPanelOpenData(Action<MapType, string> onOpenMapCallback)
    {
        OnOpenMapCallback = onOpenMapCallback;
    }
}
