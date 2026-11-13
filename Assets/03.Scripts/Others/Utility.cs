using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
public class Utility
{
    /// <summary>
    /// 获得一个中心点在屏幕中心，指定大小的rect
    /// </summary>
    /// <param name="size">x:宽，y:高</param>
    /// <returns></returns>
    public static Rect GetScreenCenterRect(Vector2 size)
    {
        return new Rect((Screen.width - size.x) / 2,(Screen.height - size.y) / 2,size.x,size.y);
    }
    /// <summary>
    /// 截取摄像机画面中指定区域，不包含 UI
    /// </summary>
    public static Texture2D ScreenShotHaveNoUI(Camera camera, Rect rect)
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

        RenderTexture originalTargetTexture = camera.targetTexture;
        RenderTexture originalActive = RenderTexture.active;

        try
        {
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;

            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            return screenShot;
        }
        finally
        {
            camera.targetTexture = originalTargetTexture;
            RenderTexture.active = originalActive;

            renderTexture.Release();
            GameObject.Destroy(renderTexture);
        }
    }
    /// <summary>
    /// 异步写入图片
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="texture2D"></param>
    /// <returns></returns>
    public static async Task WriteImageAsync(string filePath,Texture2D texture2D)
    {
        if (texture2D == null) return;

        GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;

        uint width = (uint)texture2D.width;
        uint height = (uint)texture2D.height;

        Color32[] array = texture2D.GetPixels32();

        await Task.Run(() =>
        {
            File.WriteAllBytes(filePath,ImageConversion.EncodeArrayToPNG(array,graphicsFormat,width,height));
        });
    }
    /// <summary>
    /// 写入图片
    /// </summary>
    public static void WriteImage(string filePath, Texture2D texture2D)
    {
        if (texture2D == null) return;
        GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
        uint width = (uint)texture2D.width;
        uint height = (uint)texture2D.height;
        Color32[] array = texture2D.GetPixels32();
        File.WriteAllBytes(filePath, ImageConversion.EncodeArrayToPNG(array, graphicsFormat, width, height));
    }
}