using FlyRabbit.EventBus;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MapViewer : MonoBehaviour, IPoolReleaseHandler
{
    [SerializeField]
    private RawImage m_Thumbnail;
    [SerializeField]
    private TextMeshProUGUI m_MapNameText;

    private Button m_Button;
    private Texture m_DefaultThumbnail;

    public MapType MapType { get; private set; }
    public string MapName { get; private set; }
    private static EventMessage.UI.OnMapViewerSelected m_Message = new EventMessage.UI.OnMapViewerSelected();
    /// <summary>图片文件读取并发限制</summary>
    private static readonly SemaphoreSlim m_LoadSemaphore = new SemaphoreSlim(4);
    /// <summary>当前缩略图加载任务的取消令牌</summary>
    private CancellationTokenSource m_LoadCancellation;
    /// <summary>当前缩略图加载任务的版本号</summary>
    private int m_LoadVersion;
    /// <summary>已加载的缩略图</summary>
    private Texture2D m_LoadedThumbnail;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnButtonClick);

        m_DefaultThumbnail = m_Thumbnail.texture;
    }
    /// <summary>设置地图信息</summary>
    /// <param name="mapType"></param>
    /// <param name="mapName"></param>
    public void SetMapInfo(MapType mapType, string mapName)
    {
        // 取消之前的加载任务
        CancelThumbnailLoading();
        MapType = mapType;
        MapName = mapName;
        //赋值显示的地图名称，关卡地图显示为第N关，自定义地图显示为文件夹名称
        string displayName = mapType == MapType.Level ? string.Format(LocalizationManager.GetLocalizedString(LocalizationKey.LevelName), mapName) : mapName;
        m_MapNameText.text = displayName;
        // 恢复默认缩略图
        m_Thumbnail.texture = m_DefaultThumbnail;
        ReleaseLoadedThumbnail();
        //根据地图类型加载缩略图
        switch (MapType)
        {
            case MapType.Level:
                LoadThumbnailByLevelMap();
                break;
            case MapType.Custom:
                LoadThumbnailByCustomMap();
                break;
            default:
                break;
        }
    }
    /// <summary>加载自定义地图的缩略图</summary>
    private void LoadThumbnailByCustomMap()
    {
        //获取路径
        string thumbnailPath = FileManager.GetCustomMapThumbnailPath(MapName);
        //开始加载,此部分多线程加载由AI编写代码，测试通过，感谢AI的帮助
        StartThumbnailLoading(thumbnailPath);
    }
    /// <summary>加载关卡地图的缩略图</summary>
    private void LoadThumbnailByLevelMap()
    {
        string address = FileManager.GetLevelMapThumbnailPath(MapName);
        m_Thumbnail.texture = Addressables.LoadAssetAsync<Sprite>(address).WaitForCompletion().texture;
    }

    /// <summary>开始异步加载缩略图</summary>
    private async void StartThumbnailLoading(string imagePath)
    {
        int version = m_LoadVersion;

        CancellationTokenSource cancellation = new CancellationTokenSource();
        m_LoadCancellation = cancellation;

        bool semaphoreAcquired = false;

        try
        {
            // 等待获取并发许可
            await m_LoadSemaphore.WaitAsync(cancellation.Token);
            semaphoreAcquired = true;

            // 在线程池中读取图片文件
            byte[] imageBytes = await Task.Run(() => File.ReadAllBytes(imagePath), cancellation.Token);

            // 释放并发许可
            m_LoadSemaphore.Release();
            semaphoreAcquired = false;

            // 检查任务是否已经失效
            if (cancellation.IsCancellationRequested || version != m_LoadVersion || !this || !isActiveAndEnabled)
            {
                return;
            }

            // Unity 主线程创建纹理并解码
            Texture2D texture = new Texture2D(2, 2);

            if (!texture.LoadImage(imageBytes))
            {
                Destroy(texture);
                Debug.LogError($"加载地图缩略图失败：{imagePath}");
                return;
            }

            // 再次检查任务有效性
            if (cancellation.IsCancellationRequested || version != m_LoadVersion || !this || !isActiveAndEnabled)
            {
                Destroy(texture);
                return;
            }

            // 保存并赋值纹理
            m_LoadedThumbnail = texture;
            m_Thumbnail.texture = texture;
        }
        catch (OperationCanceledException)
        {
            // 加载任务被取消
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"加载地图缩略图失败：{imagePath}\n{exception}");
        }
        finally
        {
            if (semaphoreAcquired)
            {
                m_LoadSemaphore.Release();
            }
            if (m_LoadCancellation == cancellation)
            {
                m_LoadCancellation = null;
            }
            cancellation.Dispose();
        }
    }

    /// <summary>取消当前缩略图加载</summary>
    private void CancelThumbnailLoading()
    {
        m_LoadVersion++;

        if (m_LoadCancellation != null)
        {
            m_LoadCancellation.Cancel();
            m_LoadCancellation = null;
        }
    }
    /// <summary>释放已加载的缩略图</summary>
    private void ReleaseLoadedThumbnail()
    {
        if (m_LoadedThumbnail != null)
        {
            Destroy(m_LoadedThumbnail);
            m_LoadedThumbnail = null;
        }
    }
    private void OnButtonClick()
    {
        m_Message.MapViewer = this;
        EventBus.TriggerEvent<EventMessage.UI.OnMapViewerSelected>(m_Message);
    }

    public void OnPoolRelease()
    {
        // 取消尚未完成的加载任务
        CancelThumbnailLoading();

        // 恢复默认图片
        m_Thumbnail.texture = m_DefaultThumbnail;

        // 释放之前加载的纹理
        ReleaseLoadedThumbnail();

        MapType = MapType.Level;
        MapName = null;
    }

    private void OnDestroy()
    {
        CancelThumbnailLoading();
        ReleaseLoadedThumbnail();
    }
}
