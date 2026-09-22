using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{
    [Range(0f,0.2f)]
    public float Speed = 0.02f;

    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        AdjustToCameraSize();
        Shader.PropertyToID("_MainTex");
    }

    void Update()
    {
        Roll();
    }

    private void OnValidate()
    {
        AdjustToCameraSize();
    }
    [ContextMenu("Adjust To Camera Size")]
    private void AdjustToCameraSize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        //适应规则：
        //背景图片宽高比不能改变。
        //适应宽度，保证左右两边不留空白。
        //这会导致背景图片高度溢出，此时向下移动背景图片，使得背景图片的上边缘与摄像机的上边缘对齐。

        Camera camera = Camera.main;
        //1. 缩放背景图片
        float cameraWidth = camera.orthographicSize * 2f * camera.aspect;
        float spriteWidth = spriteSize.x;
        float scale = cameraWidth / spriteWidth;
        transform.localScale = Vector2.one * scale;
        //2.对齐上边缘
        float scaledSpriteHeight = spriteSize.y * scale;
        float cameraTop = camera.transform.position.y + camera.orthographicSize;
        float spriteCenterY = cameraTop - scaledSpriteHeight / 2f;
        transform.position = new Vector3(transform.position.x, spriteCenterY, transform.position.z);

    }

    private void Roll()
    {
        m_SpriteRenderer.material.mainTextureOffset += new Vector2(Speed * Time.deltaTime, 0);
        if (m_SpriteRenderer.material.mainTextureOffset.x > 1)
        {
            m_SpriteRenderer.material.mainTextureOffset -= Vector2.right;
        }
    }
}
