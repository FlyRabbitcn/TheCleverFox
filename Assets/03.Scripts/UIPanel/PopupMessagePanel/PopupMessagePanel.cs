using FlyRabbit.ExtensionMethods;
using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessagePanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI m_MessageText;
    [SerializeField]
    private Button m_CancelButton;
    [SerializeField]
    private Button m_ConfirmButton;

    private PopupMessageOpenData m_OpenData;


    private void Awake()
    {
        m_CancelButton.onClick.AddListener(CloseSelf);
        m_ConfirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnEnable()
    {
        m_OpenData = null;
    }
    public override void OnOpen(OpenData openData)
    {
        m_OpenData = openData as PopupMessageOpenData;
        m_MessageText.text = m_OpenData.Message;
        m_CancelButton.gameObject.SetActive(!m_OpenData.IsHideCancelButton);
    }

    public override void OnEscape()
    {
        m_CancelButton.SimulateClick();
    }

    private void OnConfirmButtonClick()
    {
        m_OpenData.ConfirmCallback?.Invoke();
        CloseSelf();
    }
}
