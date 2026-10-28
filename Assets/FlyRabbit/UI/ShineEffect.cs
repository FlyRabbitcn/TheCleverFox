using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlyRabbit.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(RectMask2D))]
    public class ShineEffect : MonoBehaviour
    {
        public Color Color = new Color(1, 1, 1, 0.5f);
        [Min(10)]
        public float Width = 35;
        [Range(0f, 10f)]
        public float Interval = 0.5f;
        [Range(0.1f, 10f)]
        public float Duration = 0.5f;
        public bool PlayOnEnable = true;
        private RectTransform m_RectTransform;
        private GameObject m_Light;
        private Vector2 m_StartPosition = new Vector2();
        private Vector2 m_EndPosition = new Vector2();

        Coroutine m_Coroutine;
        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();      
        }
        private void OnEnable()
        {
            if (PlayOnEnable)
            {
                Play();
            }
        }

        public void Play()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
            }
            m_Coroutine = StartCoroutine(PlayShineAnimation());
        }

        /// <summary>初始化</summary>
        private void InitializeShineEffect()
        {
            //创建Light物体
            {
                if (m_Light == null)
                {
                    m_Light = new GameObject("Light", typeof(RectTransform), typeof(Image));
                    m_Light.transform.SetParent(this.transform, false);
                }
            }
            //使此物体撑满其父物体
            {
                m_RectTransform.anchorMin = Vector2.zero;
                m_RectTransform.anchorMax = Vector2.one;
                m_RectTransform.offsetMin = Vector2.zero;
                m_RectTransform.offsetMax = Vector2.zero;
            }
            //设置Light颜色
            {
                Image image = m_Light.GetComponent<Image>();
                image.color = Color;
            }
            //设置Light物体的旋转、大小和初始位置
            {
                //初始角度
                RectTransform rectTransform = m_Light.GetComponent<RectTransform>();
                rectTransform.localEulerAngles = new Vector3(0, 0, 45f);
                //初始大小
                Vector2 newSizeDelta = new Vector2();
                float diagonalLength = new Vector2(m_RectTransform.rect.width, m_RectTransform.rect.height).magnitude;
                newSizeDelta.x = Width;
                newSizeDelta.y = diagonalLength * 1.1f;
                rectTransform.sizeDelta = newSizeDelta;

                // 初始位置：完全隐藏在父物体左侧
                // 计算旋转后扫光物体的水平包围范围
                float parentWidth = m_RectTransform.rect.width;
                float lightWidth = rectTransform.rect.width;
                float lightHeight = rectTransform.rect.height;
                float angle = rectTransform.localEulerAngles.z * Mathf.Deg2Rad;
                float rotatedWidth = Mathf.Abs(lightWidth * Mathf.Cos(angle)) + Mathf.Abs(lightHeight * Mathf.Sin(angle));
                //赋值起始位置
                rectTransform.anchoredPosition = new Vector2(-(parentWidth + rotatedWidth) / 2f, 0f);

                //赋值起始位置和终点位置，为后续动画做准备
                m_StartPosition = rectTransform.anchoredPosition;
                m_EndPosition = rectTransform.anchoredPosition * new Vector2(-1, 1);
            }

        }

        private IEnumerator PlayShineAnimation()
        {
            yield return null;
            InitializeShineEffect();

            RectTransform rectTransform = m_Light.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = m_StartPosition;

            float endTime;
            float lerp;
            Vector2 newAnchorPosition = new Vector2();

            while (true)
            {
                endTime = Time.time + Duration;
                lerp = 0;
                while (Time.time < endTime)
                {
                    yield return null;
                    lerp += Time.deltaTime / Duration;
                    newAnchorPosition.x = Mathf.Lerp(m_StartPosition.x, m_EndPosition.x, lerp);
                    newAnchorPosition.y = Mathf.Lerp(m_StartPosition.y, m_EndPosition.y, lerp);
                    rectTransform.anchoredPosition = newAnchorPosition;
                }
                yield return new WaitForSeconds(Interval);
            }

        }
    }
}