using UnityEngine;
using System.IO;

namespace FlyRabbit.SaveSystem
{
    public class SaveSystemEditorExtension
    {
        [UnityEditor.MenuItem("FlyRabbit/Save System/Open File", priority = 0)]
        public static void OpenFIle()
        {
            if (File.Exists(SaveManager.SaveFilePath) == false)
            {
                UnityEditor.EditorUtility.DisplayDialog("警告", "存档文件不存在。", "确定");
                return;
            }
            Application.OpenURL("file://" + SaveManager.SaveFilePath);
        }
        [UnityEditor.MenuItem("FlyRabbit/Save System/Delete File", priority = 1)]
        public static void DeleteFIle()
        {
            File.Delete(SaveManager.SaveFilePath);
            Debug.Log("存档文件已删除。");
        }
        [UnityEditor.MenuItem("FlyRabbit/Save System/Open Folder", priority = 2)]
        public static void OpenFolder()
        {
            Application.OpenURL("file://" + SaveManager.SaveDirectoryPath);
        }
    }
}
