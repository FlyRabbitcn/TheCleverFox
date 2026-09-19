using UnityEngine;

namespace FlyRabbit.UIFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        private CanvasGroup m_CanvasGroup;

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (m_CanvasGroup == null)
                {
                    m_CanvasGroup = GetComponent<CanvasGroup>();
                }
                return m_CanvasGroup;
            }
        }


        #region Protected Methods
        /// <summary>
        /// 关闭自己
        /// </summary>
        protected void CloseSelf()
        {
            UIManager.ClosePanel(this);
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// 当面板被打开时，由UIManager调用
        /// </summary>
        public virtual void OnOpen(OpenData openData) { }
        /// <summary>
        /// 当面板被关闭时，由UIManager调用
        /// </summary>
        public virtual void OnClose() { }
        /// <summary>
        /// 当用户点击Escape键时，由UIManager调用
        /// </summary>
        public virtual void OnEscape() { }
        #endregion
    }
}
