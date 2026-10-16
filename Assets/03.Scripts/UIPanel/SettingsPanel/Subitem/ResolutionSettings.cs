using DG.Tweening;
using FlyRabbit.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField]
    private Toggle m_Toggle;
    [SerializeField]
    private TMP_Dropdown m_Dropdown;
    [SerializeField]
    private LayoutElement m_LayoutElement;
    private void Awake()
    {
        //初始化m_Dropdown的选项
        m_Dropdown.ClearOptions();
        //正序遍历
        for (int i = 0; i < SettingsManager.Resolutions.Length; i++)
        {
            Resolution resolution = SettingsManager.Resolutions[i];
            string option = $"{resolution.width} x {resolution.height}";
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }
        //读取设置
        m_Toggle.SetIsOnWithoutNotify(SaveManager.GetValue(SaveDefine.IsFullScreen));
        m_Dropdown.SetValueWithoutNotify(SaveManager.GetValue(SaveDefine.ResolutionScheme));
        //事件绑定
        m_Toggle.onValueChanged.AddListener(OnFullScreenToggleChanged);
        m_Dropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
    }

    private void OnResolutionDropdownChanged(int arg0)
    {
        SaveManager.SetValue(SaveDefine.ResolutionScheme, arg0, false);
    }

    private void OnFullScreenToggleChanged(bool arg0)
    {
        //保存设置
        SaveManager.SetValue(SaveDefine.IsFullScreen, arg0, false);
        //点击音效
        AudioManager.PlaySFX(AudioPath.SFX.Open);
    }


}
