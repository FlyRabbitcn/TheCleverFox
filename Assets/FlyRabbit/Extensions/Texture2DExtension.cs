using UnityEngine;

public static class Texture2DExtension
{
    /// <summary>
    /// 使用双线性过滤缩放纹理
    /// </summary>
    /// <param name="texture">原始纹理</param>
    /// <param name="newWidth">目标宽度</param>
    /// <param name="newHeight">目标高度</param>
    public static void ScaleBilinear(this Texture2D texture, int newWidth, int newHeight)
    {
        Scale(texture, newWidth, newHeight, true);
    }

    /// <summary>
    /// 使用点过滤缩放纹理
    /// </summary>
    /// <param name="texture">原始纹理</param>
    /// <param name="newWidth">目标宽度</param>
    /// <param name="newHeight">目标高度</param>
    public static void ScalePoint(this Texture2D texture, int newWidth, int newHeight)
    {
        Scale(texture, newWidth, newHeight, false);
    }

    /// <summary>
    /// 缩放纹理
    /// </summary>
    private static void Scale(Texture2D texture, int newWidth, int newHeight, bool useBilinear)
    {
        if (texture == null)
        {
            Debug.LogError("无法缩放空纹理！");
            return;
        }

        if (newWidth <= 0 || newHeight <= 0)
        {
            Debug.LogError("目标纹理尺寸必须大于 0！");
            return;
        }

        int originalWidth = texture.width;
        int originalHeight = texture.height;

        Color[] originalPixels = texture.GetPixels();
        Color[] newPixels = new Color[newWidth * newHeight];

        // 计算缩放比例，保证目标纹理首尾像素对应原纹理首尾像素
        float ratioX = newWidth > 1 ? (originalWidth - 1f) / (newWidth - 1) : 0;

        float ratioY = newHeight > 1 ? (originalHeight - 1f) / (newHeight - 1) : 0;

        for (int y = 0; y < newHeight; y++)
        {
            float sourceY = y * ratioY;
            int y0 = Mathf.FloorToInt(sourceY);
            int y1 = Mathf.Min(y0 + 1, originalHeight - 1);

            float yLerp = sourceY - y0;

            for (int x = 0; x < newWidth; x++)
            {
                float sourceX = x * ratioX;
                int x0 = Mathf.FloorToInt(sourceX);
                int x1 = Mathf.Min(x0 + 1, originalWidth - 1);

                if (useBilinear)
                {
                    float xLerp = sourceX - x0;
                    Color bottom = Color.LerpUnclamped(originalPixels[y0 * originalWidth + x0], originalPixels[y0 * originalWidth + x1], xLerp);
                    Color top = Color.LerpUnclamped(originalPixels[y1 * originalWidth + x0], originalPixels[y1 * originalWidth + x1], xLerp);
                    
                    newPixels[y * newWidth + x] = Color.LerpUnclamped(bottom, top, yLerp);
                }
                else
                {
                    int nearestX = Mathf.RoundToInt(sourceX);
                    int nearestY = Mathf.RoundToInt(sourceY);

                    newPixels[y * newWidth + x] = originalPixels[nearestY * originalWidth + nearestX];
                }
            }
        }

        texture.Reinitialize(newWidth, newHeight);
        texture.SetPixels(newPixels);
        texture.Apply();
    }
}
