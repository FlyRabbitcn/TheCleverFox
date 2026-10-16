using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;
using UnityEngine.UI;
using FlyRabbit.SaveSystem;

public class SelectLanguagePanel : UIPanel
{
    [SerializeField]
    private Button m_ComfirmButton;

    private CloseCallback m_CloseCallback;
    private void Awake()
    {
        m_ComfirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    override public void OnOpen(OpenData openData)
    {
        m_CloseCallback = openData as CloseCallback;
    }

    private void OnConfirmButtonClick()
    {
        if (SaveManager.GetValue(SaveDefine.HasSelectedLanguage) == false)
        {
            SaveManager.SetValue(SaveDefine.HasSelectedLanguage, true);
        }    
        CloseSelf();
        m_CloseCallback?.Action?.Invoke();
    }
}
