using FlyRabbit.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_BGMInputField;
    [SerializeField]
    private Slider m_BGMSlider;
    [Space(25)]
    [SerializeField]
    private TMP_InputField m_SFXInputField;
    [SerializeField]
    private Slider m_SFXSlider;

    private void Awake()
    {
        //读取设置
        m_BGMInputField.SetTextWithoutNotify(SaveManager.GetValue(SaveDefine.BGMVolume).ToString());
        m_BGMSlider.SetValueWithoutNotify(SaveManager.GetValue(SaveDefine.BGMVolume));
        m_SFXInputField.SetTextWithoutNotify(SaveManager.GetValue(SaveDefine.SFXVolume).ToString());
        m_SFXSlider.SetValueWithoutNotify(SaveManager.GetValue(SaveDefine.SFXVolume));
        //事件绑定
        m_BGMInputField.onValueChanged.AddListener(OnBGMInputFieldValueChanged);
        m_BGMInputField.onEndEdit.AddListener(OnBGMInputFieldEndEdit);
        m_BGMSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);

        m_SFXInputField.onValueChanged.AddListener(OnSFXInputFieldValueChanged);
        m_SFXInputField.onEndEdit.AddListener(OnSFXInputFieldEndEdit);
        m_SFXSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
    }


    private void OnBGMInputFieldValueChanged(string arg0)
    {
        //只允许输入3位数字
        if (arg0.Length > 3)
        {
            m_BGMInputField.SetTextWithoutNotify(arg0.Substring(0, 3));
        }
    }

    private void OnBGMInputFieldEndEdit(string arg0)
    {
        //只允许输入0-100的整数
        int value = Convert.ToInt32(arg0);
        Sanitizers.ClampInt0To100(ref value);
        //修改表现
        m_BGMInputField.SetTextWithoutNotify(value.ToString());
        m_BGMSlider.SetValueWithoutNotify(value);
        //保存设置
        SaveManager.SetValue(SaveDefine.BGMVolume, value, false);
    }

    private void OnBGMSliderValueChanged(float arg0)
    {
        m_BGMInputField.SetTextWithoutNotify(((int)arg0).ToString());
        //保存设置
        SaveManager.SetValue(SaveDefine.BGMVolume, (int)arg0, false);
    }

    private void OnSFXInputFieldValueChanged(string arg0)
    {
        //只允许输入3位数字
        if (arg0.Length > 3)
        {
            m_BGMInputField.SetTextWithoutNotify(arg0.Substring(0, 3));
        }
    }
    private void OnSFXInputFieldEndEdit(string arg0)
    {
        //只允许输入0-100的整数
        int value = Convert.ToInt32(arg0);
        Sanitizers.ClampInt0To100(ref value);
        //修改表现
        m_SFXInputField.SetTextWithoutNotify(value.ToString());
        m_SFXSlider.SetValueWithoutNotify(value);
        //保存设置
        SaveManager.SetValue(SaveDefine.SFXVolume, value, false);
    }
    private void OnSFXSliderValueChanged(float arg0)
    {
        m_SFXInputField.SetTextWithoutNotify(((int)arg0).ToString());
        //保存设置
        SaveManager.SetValue(SaveDefine.SFXVolume, (int)arg0, false);
    }




}
