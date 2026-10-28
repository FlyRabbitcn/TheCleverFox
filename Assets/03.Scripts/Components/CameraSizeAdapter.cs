using FlyRabbit.EventBus;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeAdapter : MonoBehaviour
{
    private Camera m_Camera;
    private Vector2 m_SceneSize;

    private readonly Vector2 m_OffsetSize = new Vector2(0, 3);
    private bool m_EnableOffset = false;
    private void Awake()
    {
        m_SceneSize = new Vector2(MapManager.MapWidth, MapManager.MapHeight);

        m_Camera = GetComponent<Camera>();

        UpdateCameraSize();
    }
    private void Update()
    {
#if UNITY_EDITOR
        UpdateCameraSize();
#endif
    }

    private void OnEnable()
    {
        EventBus.AddListener<OnResolutionChanged>(OnResolutionChanged);
    }

    private void OnDisable()
    {
        EventBus.RemoveListener<OnResolutionChanged>(OnResolutionChanged);
    }


    public void SetEnableOffsset(bool value)
    {
        m_EnableOffset = value;
        UpdateCameraSize();
    }

    /// <summary>更新摄像机大小，确保可以显示地图所有内容</summary>
    private void UpdateCameraSize()
    {
        if (Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        float screenAspect = (float)Screen.width / Screen.height;
        // 以完整显示场景为目标，取能够容纳场景的最小尺寸
        Vector2 size = m_EnableOffset ? (m_SceneSize + m_OffsetSize) : m_SceneSize;
        m_Camera.orthographicSize = Mathf.Max(size.y * 0.5f, size.x / (2f * screenAspect));
        EventBus.TriggerEvent<OnMainCameraSizeChanged>(OnMainCameraSizeChanged.Instance);
    }


    private void OnResolutionChanged(OnResolutionChanged message)
    {
        UpdateCameraSize();
    }
}
