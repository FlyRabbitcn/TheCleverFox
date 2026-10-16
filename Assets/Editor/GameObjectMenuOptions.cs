using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectMenuOptions
{
    [MenuItem("GameObject/UI/Shadow", false, 10)]
    public static void CreateShadow(MenuCommand menuCommand)
    {
        //创建新物体
        GameObject gameObject = new GameObject("Shadow");
        //设置父节点
        GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
        //支持撤销操作
        Undo.RegisterCreatedObjectUndo(gameObject, "Create Game Object");
        //选中物体
        Selection.activeObject = gameObject;
        //添加组件
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.sizeDelta = Vector2.zero;
        //添加并设置阴影颜色
        gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.9f);
    }
}
