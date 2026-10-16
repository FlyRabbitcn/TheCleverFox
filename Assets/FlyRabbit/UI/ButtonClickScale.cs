using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FlyRabbit.UI
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class ButtonClickScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float Scale = 0.9f;
        public float Duration = 0.06f;

        private Button m_Button;
        private Tween m_ScaleTween;

        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanInteract() == false)
            {
                return;
            }
            PlayPressAnimation();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (CanInteract() == false)
            {
                return;
            }
            PlayReleaseAnimation();
        }

        #region Private Methods
        /// <summary>
        /// 当前按钮是否可以交互
        /// </summary>
        /// <returns></returns>
        private bool CanInteract()
        {
            return m_Button.IsActive() && m_Button.IsInteractable();
        }
        /// <summary>
        /// 停止之前的动画
        /// </summary>
        private void KillPreviousTween()
        {
            m_ScaleTween?.Kill();
        }
        /// <summary>
        /// 播放按压动画
        /// </summary>
        private void PlayPressAnimation()
        {
            KillPreviousTween();
            m_ScaleTween = transform.DOScale(Scale, Duration).SetLink(gameObject);

        }
        /// <summary>
        /// 播放释放动画
        /// </summary>
        private void PlayReleaseAnimation()
        {
            KillPreviousTween();
            m_ScaleTween = transform.DOScale(1, Duration).SetLink(gameObject);
        }
        #endregion
    }
}
