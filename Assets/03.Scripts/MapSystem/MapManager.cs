using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

    private MapGrid m_MapGrid;

    /// <summary>地图宽</summary>
    public static int MapWidth { get; } = 20;
    /// <summary>地图高</summary>
    public static int MapHeight { get; } = 11;

    #region Unity Methods
    private void Awake()
    {
        LoadMapGrid();
        InitMapGridState();
    }
    #endregion

    #region Public Methods
    /// <summary>设置地图格子可见性</summary>
    /// <param name="enable"></param>
    public void SetMapGridVisible(bool enable)
    {
        m_MapGrid.SetVisible(enable);
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
        m_MapGrid.SetVisible(false);
    }
    #endregion
}
