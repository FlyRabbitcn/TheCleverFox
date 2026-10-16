using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSettings : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;


    private void Awake()
    {
        m_Button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        UIManager.OpenPanel<SelectLanguagePanel>();
    }
}
