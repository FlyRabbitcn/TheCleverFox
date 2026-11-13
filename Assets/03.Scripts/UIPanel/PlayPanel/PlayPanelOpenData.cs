using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPanelOpenData : OpenData
{
    public MapType MapType;
    public string MapName;

    public PlayPanelOpenData(MapType mapType, string mapName)
    {
        MapType = mapType;
        MapName = mapName;
    }
}
