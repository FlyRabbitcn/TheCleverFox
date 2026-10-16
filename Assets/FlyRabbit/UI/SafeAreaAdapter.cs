using UnityEngine;
#if UNITY_EDITOR    
using UnityEditor;
#endif
namespace FlyRabbit.UI
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SafeAreaAdapter : MonoBehaviour
    {
        private RectTransform m_RectTransform;
        #region Unity Methods

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            SetFullStretch();
            FitToSafeArea();
        }
#if UNITY_EDITOR
        private void Update()
        {
            FitToSafeArea();
        }
        [MenuItem("GameObject/UI/SafeArea", false, 10)]
        public static void CreateEmpty(MenuCommand menuCommand)
        {
            //创建新物体
            GameObject gameObject = new GameObject("SafeArea");
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
            gameObject.AddComponent<SafeAreaAdapter>();
        }
#endif
        #endregion

        #region Private Methods
        /// <summary>
        /// 全拉伸
        /// </summary>
        private void SetFullStretch()
        {
            m_RectTransform.anchorMin = new Vector2(0f, 0f);
            m_RectTransform.anchorMax = new Vector2(1f, 1f);
            m_RectTransform.sizeDelta = Vector2.zero;
        }
        /// <summary>
        /// 应用安全区
        /// </summary>
        private void FitToSafeArea()
        {
            m_RectTransform.offsetMin = new Vector2(LeftUnsafe(), BottomUnsafe());
            m_RectTransform.offsetMax = new Vector2(-RightUnsafe(), -TopUnsafe());
        }
        /// <summary>
        /// 返回底部不安全区域
        /// </summary>
        /// <returns></returns>
        private float BottomUnsafe()
        {
            float result = Screen.safeArea.y;
            return result;
        }
        /// <summary>
        /// 返回顶部不安全区域
        /// </summary>
        /// <returns></returns>
        private float TopUnsafe()
        {
            float result = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
            return result;
        }
        /// <summary>
        /// 返回左边的不安全区域
        /// </summary>
        /// <returns></returns>
        private float LeftUnsafe()
        {
            float result = Screen.safeArea.x;
            return result;
        }
        /// <summary>
        /// 返回右边的不安全区域
        /// </summary>
        /// <returns></returns>
        private float RightUnsafe()
        {
            float result = Screen.width - (Screen.safeArea.x + Screen.safeArea.width);
            return result;
        }
        #endregion
    }
}
