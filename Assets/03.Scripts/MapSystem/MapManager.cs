using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MapManager : MonoBehaviour
{
    #region 单例
    private static MapManager m_Instance;
    public static MapManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject gameObject = new GameObject("[单例]" + typeof(MapManager).Name);
                DontDestroyOnLoad(gameObject);
                m_Instance = gameObject.AddComponent<MapManager>();
            }
            return m_Instance;
        }
    }
    #endregion

    #region Backing Fields
    #endregion



    /// <summary>地图宽</summary>
    public readonly int MapWidth = 20;
    /// <summary>地图高</summary>
    public readonly int MapHeight = 11;
    /// <summary>地图格子世界尺寸</summary>
    public readonly float CellSize = 1f;

    private MapGrid m_MapGrid;
    private Item[,] m_MapData;
    /// <summary>地图格子可见性计数</summary>
    private int m_MapGridVisibleCount;
    #region Unity Methods
    private void Awake()
    {
        m_MapData = new Item[MapWidth, MapHeight];
        LoadMapGrid();
        InitMapGridState();
    }
    #endregion

    #region Public Methods
    /// <summary>设置地图格子可见性</summary>
    /// <param name="enable"></param>
    public void SetMapGridVisible(bool enable)
    {
        m_MapGridVisibleCount += enable ? 1 : -1;
        m_MapGrid.SetVisible(m_MapGridVisibleCount >= 0);
    }
    /// <summary>
    /// 世界坐标转换为地图坐标，地图坐标以地图左下角为原点。
    /// 如果转换后的坐标不在合法地图范围内，返回 false，否则返回 true。
    /// </summary>
    /// <param name="worldPosition">世界坐标</param>
    /// <param name="mapPosition">转换后的地图坐标</param>
    /// <returns>坐标是否在合法地图范围内</returns>
    public bool WorldToMap(Vector2 worldPosition, out Vector2Int mapPosition)
    {
        Vector2 mapOrigin = -new Vector2(MapWidth, MapHeight) * 0.5f;
        mapPosition = Vector2Int.FloorToInt((worldPosition - mapOrigin) / CellSize);
        return CheckMapPositionValid(mapPosition);
    }

    /// <summary>
    /// 地图坐标转换为世界坐标。
    /// </summary>
    /// <param name="mapPosition">地图坐标</param>
    /// <returns>世界坐标（格子中心）</returns>
    public Vector2 MapToWorld(Vector2Int mapPosition)
    {
        Vector2 mapOrigin = -new Vector2(MapWidth, MapHeight) * 0.5f;

        return mapOrigin + ((Vector2)mapPosition + Vector2.one * 0.5f) * CellSize;
    }
    /// <summary>
    /// 检查地图坐标是否可有效（合法）。
    /// </summary>
    /// <param name="mapPosition"></param>
    /// <returns></returns>
    public bool CheckMapPositionValid(Vector2Int mapPosition)
    {
        return mapPosition.x >= 0 && mapPosition.x < MapWidth && mapPosition.y >= 0 && mapPosition.y < MapHeight;
    }
    /// <summary>
    /// 在指定位置放置一个Item
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="name"></param>
    public void PlaceItem(int x, int y, string name, bool IsFrozen = true)
    {
        if (!CheckMapPositionValid(new Vector2Int(x, y)))
        {
            Debug.LogError($"[Map System]：无效的地图坐标：({x}, {y})");
            return;
        }
        //先清空位置
        RemoveItem(x, y);
        //获取Item实例
        Item item = ItemPool.Get(name);
        if (item == null)
        {
            return;
        }
        //设置Item位置
        item.transform.position = MapToWorld(new Vector2Int(x, y));
        item.transform.SetParent(this.transform, true);
        //设置冻结
        item.SetFrozen(IsFrozen);
        //加入地图中
        m_MapData[x, y] = item;
    }
    /// <summary>
    /// 移除地图中某个位置的Item。
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RemoveItem(int x, int y)
    {
        if (!CheckMapPositionValid(new Vector2Int(x, y)))
        {
            Debug.LogError($"[Map System]：无效的地图坐标：({x}, {y})");
            return;
        }
        if (m_MapData[x, y] == null)
        {
            return;
        }
        Item item = m_MapData[x, y];
        m_MapData[x, y] = null;
        ItemPool.Release(item);
    }
    /// <summary>删除指定Item</summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        for (int i = 0; i < m_MapData.GetLength(0); i++)
        {
            for (int j = 0; j < m_MapData.GetLength(1); j++)
            {
                if (m_MapData[i, j] == gameObject)
                {
                    RemoveItem(i, j);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 获取指定地图位置的Item，如果为空则返回null。
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Item GetItem(int x, int y)
    {
        if (!CheckMapPositionValid(new Vector2Int(x, y)))
        {
            Debug.LogError($"[Map System]：无效的地图坐标：({x}, {y})");
            return null;
        }
        return m_MapData[x, y];
    }
    /// <summary>
    /// 清空地图。
    /// </summary>
    public void ClearMap()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                RemoveItem(x, y);
            }
        }
    }
    /// <summary>获取当前地图中某个种类的Item的数量,这里每次调用都要遍历地图，消耗较高，但其实地图很小，所以我就不优化了。</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public int GetItemCount<T>() where T : Item
    {
        int result = 0;
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                if (m_MapData[x, y] is T)
                {
                    result++;
                }
            }
        }
        return result;
    }
    /// <summary>保存当前地图</summary>
    /// <param name="name">地图名字</param>
    public void SaveMap(string name)
    {
        string mapFolderPath = Path.Combine(FileManager.CustomMapRootDirectory, name);
        Directory.CreateDirectory(mapFolderPath);
        string mapDataFilePath = Path.Combine(mapFolderPath, FileManager.CustomMapDataFileName);
        string mapThumbnailFilePath = Path.Combine(mapFolderPath, FileManager.CustomMapThumbnailFileName);

        //保存地图数据
        File.WriteAllText(mapDataFilePath, MapDataToJson());
        //关闭格子
        SetMapGridVisible(false);
        // 地图世界尺寸
        Vector2 mapSize = new Vector2(MapWidth, MapHeight) * CellSize;
        // 地图左下角和右上角的世界坐标
        Vector3 mapMin = MapToWorld(Vector2Int.zero) - new Vector2(CellSize, CellSize) * 0.5f;
        Vector3 mapMax = MapToWorld(new Vector2Int(MapWidth - 1, MapHeight - 1)) + new Vector2(CellSize, CellSize) * 0.5f;
        // 转换为屏幕坐标
        Vector3 screenMin = Camera.main.WorldToScreenPoint(mapMin);
        Vector3 screenMax = Camera.main.WorldToScreenPoint(mapMax);
        // 计算截图矩形
        Rect screenshotRect = Rect.MinMaxRect(screenMin.x, screenMin.y, screenMax.x, screenMax.y);
        //截图
        Texture2D texture2D = Utility.ScreenShotHaveNoUI(Camera.main, screenshotRect);
        //开启格子
        SetMapGridVisible(true);
        //缩放为640x352
        texture2D.ScaleBilinear(640, 352);
        //保存缩略图
        Utility.WriteImage(mapThumbnailFilePath, texture2D);
        //销毁截图纹理
        Destroy(texture2D);
    }
    /// <summary>打开地图</summary>
    public void OpenMap(MapType mapType, string mapName)
    {
        //首先获取到json数据
        string mapDataJson = string.Empty;
        switch (mapType)
        {
            case MapType.Level:
                mapDataJson = GetLevelMapDataJson(mapName);
                break;
            case MapType.Custom:
                mapDataJson = GetCustomMapDataJson(mapName);
                break;
            default:
                break;
        }
        //如果json数据为空，则说明地图不存在，直接返回
        if (mapDataJson == null)
        {
            Debug.LogError($"[Map System]：打开地图失败，地图数据不存在：{mapType} - {mapName}");
            return;
        }
        //获取地图元素数据
        List<MapElementData> mapElements = JsonToMapElementData(mapDataJson);
        //如果地图元素数据为空，则说明地图为空，直接返回
        if (mapElements == null || mapElements.Count == 0)
        {
            Debug.LogError($"[Map System]：打开地图失败，地图数据为空：{mapType} - {mapName}");
            return;
        }
        //清空地图
        ClearMap();
        //遍历地图元素数据，放置Item
        foreach (var item in mapElements)
        {
            PlaceItem(item.X, item.Y, item.Name);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>加载地图格子</summary>
    private void LoadMapGrid()
    {
        GameObject Prefab = Addressables.LoadAssetAsync<GameObject>("MapGrid").WaitForCompletion();
        GameObject instance = Instantiate<GameObject>(Prefab);
        instance.transform.SetParent(this.transform);
        m_MapGrid = instance.GetComponent<MapGrid>();
    }
    /// <summary>设置地图格子初始状态</summary>
    private void InitMapGridState()
    {
        if (m_MapGrid == null)
        {
            return;
        }

        m_MapGrid.SetSize(MapWidth, MapHeight);
        SetMapGridVisible(false);
    }
    /// <summary>将当前地图内容保存为json文件</summary>
    private string MapDataToJson()
    {
        List<MapElementData> mapElements = new List<MapElementData>(20);
        for (int i = 0; i < m_MapData.GetLength(0); i++)
        {
            for (int j = 0; j < m_MapData.GetLength(1); j++)
            {
                Item item = m_MapData[i, j];
                if (item == null)
                {
                    continue;
                }
                mapElements.Add(new MapElementData(i, j, item.name));
            }
        }

        return JsonConvert.SerializeObject(mapElements, Formatting.Indented);
    }
    /// <summary>将Json转化为地图信息</summary>
    private List<MapElementData> JsonToMapElementData(string json)
    {
        return JsonConvert.DeserializeObject<List<MapElementData>>(json);
    }
    /// <summary>获取自定义地图的MapDataJson</summary>
    private string GetCustomMapDataJson(string mapName)
    {
        string path = FileManager.GetCustomMapDataPath(mapName);
        if (File.Exists(path) == false)
        {
            return null;
        }
        return File.ReadAllText(path);
    }
    /// <summary>获取关卡地图的MapDataJso</summary>
    private string GetLevelMapDataJson(string mapName)
    {
        //路径
        string path = FileManager.GetLevelMapDataPath(mapName);
        //如果路径对应的资源不存在，则返回null
        IList<IResourceLocation> locations = Addressables.LoadResourceLocationsAsync(path, typeof(TextAsset)).WaitForCompletion();
        if (locations == null || locations.Count == 0)
        {
            return null;
        }
        //加载并返回
        return Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion().text;
    }
    #endregion
}
