using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
namespace FlyRabbit.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("FlyRabbit/UI/PopupEffect")]
    public class PopupEffect : MonoBehaviour
    {
        public UnityEvent OnStart;
        public UnityEvent OnComplete;
        public Vector3 FromScale = Vector3.zero;
        public Vector3 ToScale = Vector3.one;
        [Min(0.01f)]
        public float Duration = 0.15f;
        public Ease Ease = Ease.OutBack;

        private Tween m_Tween;

        private void OnEnable()
        {
            ClearTween();
            OnStart?.Invoke();
            transform.localScale = FromScale;
            m_Tween = transform.DOScale(ToScale, Duration);
            m_Tween.SetLink(gameObject);
            m_Tween.SetEase(Ease);
            m_Tween.OnComplete(() =>
            {
                m_Tween = null;
                OnComplete?.Invoke();
            });
        }
        private void OnDisable()
        {
            ClearTween();
        }

        private void ClearTween()
        {
            m_Tween?.Kill();
            m_Tween = null;
        }
    }
}
