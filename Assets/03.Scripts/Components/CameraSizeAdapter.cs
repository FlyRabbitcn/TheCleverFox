using FlyRabbit.EventBus;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeAdapter : MonoBehaviour
{
    private Camera m_Camera;
    private Vector2 m_SceneSize;


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

    /// <summary>更新摄像机大小，确保可以显示地图所有内容</summary>
    private void UpdateCameraSize()
    {
        if (Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        float screenAspect = (float)Screen.width / Screen.height;
        // 以完整显示场景为目标，取能够容纳场景的最小尺寸
        m_Camera.orthographicSize = Mathf.Max(m_SceneSize.y * 0.5f, m_SceneSize.x / (2f * screenAspect));
        EventBus.TriggerEvent<OnMainCameraSizeChanged>(OnMainCameraSizeChanged.Instance);
    }


    private void OnResolutionChanged(OnResolutionChanged message)
    {
        UpdateCameraSize();
    }
}
