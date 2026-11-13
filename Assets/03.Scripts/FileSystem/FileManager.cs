using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{
    #region 自定义地图相关
    /// <summary>自定义地图根目录文件夹</summary>
    public static string CustomMapRootDirectory { get => Path.Combine(Application.persistentDataPath, "CustomMap"); }
    /// <summary>自定义地图的数据的文件名</summary>
    public const string CustomMapDataFileName = "MapData.json";
    /// <summary>自定义地图的缩略图的文件名</summary>
    public const string CustomMapThumbnailFileName = "MapThumbnail.png";
    /// <summary>自定义地图的回放数据的文件名</summary>
    public const string CustomMapReplayFileName = "Replay.txt";

    /// <summary>检查自定义地图是否已经存在,
    /// 注意：在Windows和Android系统下文件名是不分大小写的，IOS和Mac系统是区分大小写的。
    /// </summary>
    public static bool CheckCustomMapExists(string name)
    {
        return Directory.Exists(Path.Combine(CustomMapRootDirectory, name));
    }
    /// <summary>获取自定义地图MapData文件路径</summary>
    /// <param name="mapName"></param>
    /// <returns></returns>
    public static string GetCustomMapDataPath(string mapName)
    {
        return Path.Combine(CustomMapRootDirectory, mapName, CustomMapDataFileName);
    }
    /// <summary>获取自定义地图缩略图文件路径</summary>
    /// <param name="mapName"></param>
    /// <returns></returns>
    public static string GetCustomMapThumbnailPath(string mapName)
    {
        return Path.Combine(CustomMapRootDirectory, mapName, CustomMapThumbnailFileName);
    }
    /// <summary>删除自定义地图的文件</summary>
    public static void DeleteCustomMap(string mapName)
    {
        string mapDirectory = Path.Combine(CustomMapRootDirectory, mapName);
        if (Directory.Exists(mapDirectory))
        {
            Directory.Delete(mapDirectory, true);
        }
    }
    #endregion

    #region 关卡地图相关
    /// <summary>关卡地图的数据的加载路径</summary>
    public static string LevelMapDataPath = "Assets/09.LevelData/MapData/{0}.json";
    /// <summary>关卡地图的缩略图的加载路径</summary>
    public static string LevelMapThumbnailPath = "Assets/09.LevelData/Thumbnail/{0}.png";
    /// <summary>获取关卡地图的数据的加载路径</summary>
    public static string GetLevelMapDataPath(string mapName)
    {
        return string.Format(LevelMapDataPath, mapName);
    }
    /// <summary>获取关卡地图的缩略图的加载路径</summary>
    public static string GetLevelMapThumbnailPath(string mapName)
    {
        return string.Format(LevelMapThumbnailPath, mapName);
    }

    #endregion


}
