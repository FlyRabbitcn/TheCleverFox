using FlyRabbit.EventBus;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{
    [Range(0f, 0.2f)]
    public float Speed = 0.02f;

    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        AdjustToCameraSize();
    }
    private void OnEnable()
    {
        EventBus.AddListener<OnMainCameraSizeChanged>(OnMainCameraSizeChanged);
    }
    private void OnDisable()
    {
        EventBus.RemoveListener<OnMainCameraSizeChanged>(OnMainCameraSizeChanged);
    }
    private void Update()
    {
#if UNITY_EDITOR
        AdjustToCameraSize();
#endif

        Roll();
    }

    [ContextMenu("Adjust To Camera Size")]
    private void AdjustToCameraSize()
    {
        Camera camera = Camera.main;
        SpriteRenderer spriteRenderer = m_SpriteRenderer;

        if (camera == null)
        {
            return;
        }
        //因为在编辑器下获取不到，所以这里手动获取一次
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float cameraHeight = camera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * camera.aspect;

        // 取宽度和高度所需缩放比例中的较大值，确保完全覆盖
        float widthScale = cameraWidth / spriteSize.x;
        float heightScale = cameraHeight / spriteSize.y;
        float scale = Mathf.Max(widthScale, heightScale);

        transform.localScale = Vector3.one * scale;

        // 背景上边缘与摄像机上边缘对齐
        float scaledSpriteHeight = spriteSize.y * scale;
        float cameraTop = camera.transform.position.y + camera.orthographicSize;

        float spriteCenterY = cameraTop - scaledSpriteHeight * 0.5f;

        transform.position = new Vector3(transform.position.x, spriteCenterY, transform.position.z);
    }

    private void Roll()
    {
        m_SpriteRenderer.material.mainTextureOffset += new Vector2(Speed * Time.deltaTime, 0f);

        if (m_SpriteRenderer.material.mainTextureOffset.x > 1f)
        {
            m_SpriteRenderer.material.mainTextureOffset -= Vector2.right;
        }
    }

    private void OnMainCameraSizeChanged(OnMainCameraSizeChanged message)
    {
        AdjustToCameraSize();
    }
}
