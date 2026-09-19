using FlyRabbit.AssetManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System.IO;
using UnityEngine.Pool;
namespace FlyRabbit.UIFramework
{
    public static class UIManager
    {

        /// <summary>根节点</summary>
        private static readonly Transform m_Root;

        /// <summary>资源提供者列表</summary>
        private static readonly List<IAssetProvider> m_AssetProviders = new List<IAssetProvider>(5);

        /// <summary>面板预制体字典,Key：面板类型名称，Value：面板预制体</summary>
        private static readonly Dictionary<string, GameObject> m_PanelPrefabs = new Dictionary<string, GameObject>();

        /// <summary>画布字典,Key：排序顺序，Value：画布</summary>
        private static readonly Dictionary<int, Canvas> m_CanvasDict = new Dictionary<int, Canvas>();

        /// <summary>已打开的面板集合</summary>
        private static readonly HashSet<UIPanel> m_OpenedPanels = new HashSet<UIPanel>();

        /// <summary>面板资源路径</summary>
        private static readonly string m_AssetPath = "UIPanel";

        #region Public Methods
        /// <summary>
        /// 添加一个资源提供者
        /// </summary>
        public static void AddAssetProvider(IAssetProvider Provider)
        {
            if (Provider == null)
            {
                Debug.LogError("尝试添加一个空的资源提供者，操作被忽略。");
                return;
            }
            if (m_AssetProviders.Contains(Provider))
            {
                Debug.LogWarning("尝试添加一个已存在的资源提供者，操作被忽略。");
                return;
            }
            m_AssetProviders.Add(Provider);
        }
        /// <summary>
        /// 打开指定类型的面板
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <param name="openData">打开面板时传递的数据</param>
        /// <param name="sortingOrder">面板所在Canvas的排序顺序</param>
        public static void OpenPanel<T>(OpenData openData = null, int sortingOrder = 0) where T : UIPanel
        {
            // 获取面板实例
            GameObject panelInstance = GetUIPanelInstance<T>(sortingOrder);
            // 如果实例化失败，直接返回
            if (panelInstance == null)
            {
                return;
            }
            // 获取面板组件
            T panel = panelInstance.GetComponent<T>();
            // 如果未找到对应的面板组件，销毁实例并返回
            if (panel == null)
            {
                Debug.LogError($"面板实例上未找到类型为 {typeof(T).Name} 的组件，请确保预制体上挂载了正确的脚本。");
                Object.Destroy(panelInstance);
                return;
            }
            // 加入画布
            Canvas canvas = GetCanvas(sortingOrder);
            panelInstance.transform.SetParent(canvas.transform, false);
            // 添加到已打开面板集合
            m_OpenedPanels.Add(panel);
            // 调用面板的OnOpen方法
            panel.OnOpen(openData);
        }
        /// <summary>
        /// 关闭指定的面板
        /// </summary>
        /// <param name="panel">要关闭的面板</param>
        public static void ClosePanel(UIPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError("尝试关闭一个空的面板，操作被忽略。");
                return;
            }
            if (m_OpenedPanels.Contains(panel) == false)
            {
                Debug.LogError($"尝试关闭一个未打开的面板：{panel.GetType().Name}，操作被忽略。");
                return;
            }
            // 调用面板的OnClose方法
            panel.OnClose();
            // 从已打开面板集合中移除
            m_OpenedPanels.Remove(panel);
            // 销毁面板实例
            Object.Destroy(panel.gameObject);
        }
        /// <summary>
        /// 关闭所有已打开的面板
        /// </summary>
        public static void CloseAllPanels()
        {
            // 创建一个临时列表来存储当前已打开的面板
            List<UIPanel> panelsToClose = ListPool<UIPanel>.Get();
            panelsToClose.AddRange(m_OpenedPanels);

            // 遍历临时列表并关闭每个面板
            foreach (var panel in panelsToClose)
            {
                ClosePanel(panel);
            }
            // 将临时列表归还到对象池
            ListPool<UIPanel>.Release(panelsToClose);
        }

        /// <summary>
        /// 最上层的UIPanel响应Escape键按下
        /// </summary>
        public static void ResponseEscape()
        {
            if (m_OpenedPanels.Count == 0)
            {
                return;
            }

            //获取所有sortingOrder并按从大到小排序
            List<int> sortingOrders = ListPool<int>.Get();
            sortingOrders.AddRange(m_CanvasDict.Keys);
            sortingOrders.Sort((a, b) => b.CompareTo(a));
            //遍历所有画布，从上到下查找最上层的UIPanel
            UIPanel topPanel = null;
            foreach (var item in sortingOrders)
            {
                // 获取当前画布的Transform
                Transform canvasTransform = m_CanvasDict[item].transform;
                // 如果画布下没有子物体，继续查找下一个画布
                if (canvasTransform.childCount == 0)
                {
                    continue;
                }
                // 获取当前画布显示在最上层的UIPanel，然后就可以退出循环了
                topPanel = canvasTransform.GetChild(canvasTransform.childCount - 1).GetComponent<UIPanel>();
                break;
            }

            // 将临时列表归还到对象池
            ListPool<int>.Release(sortingOrders);

            //如果最终没有找到最上层的UIPanel，则直接返回
            if (topPanel == null)
            {
                return;
            }
            //调用最上层UIPanel的OnEscape方法
            topPanel.OnEscape();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 获取指定排序顺序的画布，如果不存在则创建一个新的画布
        /// </summary>
        private static Canvas GetCanvas(int sortingOrder)
        {
            if (m_CanvasDict.ContainsKey(sortingOrder) == false)
            {
                Canvas canvas = CanvasSettings.CreateCanvas(sortingOrder, m_Root);
                m_CanvasDict.Add(sortingOrder, canvas);
            }
            return m_CanvasDict[sortingOrder];
        }
        /// <summary>
        /// 获取指定类型的面板实例
        /// </summary>
        private static GameObject GetUIPanelInstance<T>(int sortingOrder) where T : UIPanel
        {
            string panelTypeName = typeof(T).Name;
            if (m_PanelPrefabs.ContainsKey(panelTypeName) == false)
            {
                GameObject prefab = LoadPanelPrefab<T>();
                if (prefab == null)
                {
                    return null;
                }
                m_PanelPrefabs.Add(panelTypeName, prefab);
            }

            GameObject panelInstance = Object.Instantiate(m_PanelPrefabs[panelTypeName]);
            return panelInstance;
        }
        /// <summary>
        /// 加载指定类型的面板预制体
        /// </summary>
        private static GameObject LoadPanelPrefab<T>() where T : UIPanel
        {
            string panelTypeName = typeof(T).Name;
            string assetPath = Path.Combine(m_AssetPath, panelTypeName);
            foreach (var provider in m_AssetProviders)
            {
                GameObject prefab = provider.Load<GameObject>(assetPath);
                if (prefab != null)
                {
                    return prefab;
                }
            }
            Debug.LogError($"未能找到面板预制体：{assetPath}");
            return null;
        }
        #endregion
        static UIManager()
        {
            // 创建UI根节点
            {
                GameObject root = new GameObject("UIManager");
                m_Root = root.transform;
                Object.DontDestroyOnLoad(root);
            }
            // 创建Event System
            {
                if (EventSystem.current == null)
                {
                    GameObject eventSystem = new GameObject("Event System");
                    eventSystem.transform.SetParent(m_Root, false);

                    eventSystem.AddComponent<EventSystem>();
                    //新版输入系统
                    eventSystem.AddComponent<InputSystemUIInputModule>();
                    //旧版输入系统
                    //gameObject.AddComponent<StandaloneInputModule>();
                }
            }
        }
    }
}