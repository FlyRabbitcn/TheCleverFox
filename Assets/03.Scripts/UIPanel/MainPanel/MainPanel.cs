using UnityEngine;
using FlyRabbit.UIFramework;
using DG.Tweening;
using UnityEngine.UI;
public class MainPanel : UIPanel
{
    [SerializeField]
    private RectTransform m_Title;
    [SerializeField]
    private RectTransform m_ButtonList;
    [Space(25)]
    [SerializeField]
    private Button m_SettingsButton;


    private float m_AnimationOffset = 100;
    private float m_AnimationDuration = 0.2f;


    private void Awake()
    {
        m_SettingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void Start()
    {
        AudioManager.PlayBGM(AudioPath.BGM.Home);
        PlayOpenAnimation();

    }

    public override void OnEscape()
    {
        AudioManager.PlaySFX(AudioPath.SFX.WaterDrop);
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.QuitGame);
    }



    private void PlayOpenAnimation()
    {
        //初始化
        CanvasGroup.alpha = 0;
        m_Title.position += Vector3.up * m_AnimationOffset;
        m_ButtonList.position -= Vector3.up * m_AnimationOffset;


        //淡入动画
        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(gameObject);
        sequence.Append(CanvasGroup.DOFade(1, m_AnimationDuration));
        sequence.Join(m_Title.DOMoveY(m_Title.position.y - m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
        sequence.Join(m_ButtonList.DOMoveY(m_ButtonList.position.y + m_AnimationOffset, m_AnimationDuration).SetEase(Ease.Linear));
    }



    private void OnSettingsButtonClick()
    {
        UIManager.OpenPanel<SettingsPanel>();
    }
}
