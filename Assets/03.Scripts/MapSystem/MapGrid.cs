using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class MapGrid : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>设置大小</summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetSize(int x, int y)
    {
        x = Mathf.Max(1, x);
        y = Mathf.Max(1, y);
        m_SpriteRenderer.size = new Vector2(x, y);
    }
    /// <summary>设置可见性</summary>
    /// <param name="enable"></param>
    public void SetVisible(bool enable)
    {
        m_SpriteRenderer.enabled = enable;
    }
}
