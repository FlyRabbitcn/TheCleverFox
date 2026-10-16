using DG.Tweening;
using FlyRabbit.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameRateSettings : MonoBehaviour
{
    [SerializeField]
    private LayoutElement m_LayoutElement;
    [SerializeField]
    private Toggle m_Toggle;
    [SerializeField]
    private TMP_InputField m_InputField;
    [SerializeField]
    private Slider m_Slider;
    [SerializeField]
    private float m_CollapseHeight = 60;
    [SerializeField]
    private float m_ExpandHeight = 130;

    private float m_AnimationDuration = 0.1f;

    private Tween m_HeightTween;
    private void Awake()
    {
        //读取设置
        m_Toggle.SetIsOnWithoutNotify(SaveManager.GetValue(SaveDefine.UseDisplayRefreshRate));
        m_InputField.SetTextWithoutNotify(SaveManager.GetValue(SaveDefine.CustomFrameRate).ToString());
        m_Slider.SetValueWithoutNotify(SaveManager.GetValue(SaveDefine.CustomFrameRate));
        if (m_Toggle.isOn)
        {
            m_LayoutElement.preferredHeight = m_CollapseHeight;
        }
        else
        {
            m_LayoutElement.preferredHeight = m_ExpandHeight;
        }
        //事件绑定
        m_Toggle.onValueChanged.AddListener(OnToggleValueChanged);

        m_InputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        m_InputField.onEndEdit.AddListener(OnInputFieldEndEdit);

        m_Slider.onValueChanged.AddListener(OnSliderValueChanged);
    }



    private void OnToggleValueChanged(bool arg0)
    {
        //保存设置
        SaveManager.SetValue(SaveDefine.UseDisplayRefreshRate, arg0, false);
        //点击音效
        AudioManager.PlaySFX(AudioPath.SFX.Open);
        //根据Toggle状态展开或收起，使用DoTween动画
        float targetHeight = arg0 ? m_CollapseHeight : m_ExpandHeight;
        m_HeightTween?.Kill();
        m_HeightTween = m_LayoutElement.DOPreferredSize(new Vector2(m_LayoutElement.preferredWidth, targetHeight), m_AnimationDuration).SetEase(Ease.Linear).SetLink(gameObject);
        
    }
    private void OnInputFieldValueChanged(string arg0)
    {
        //只允许输入3位数字
        if (arg0.Length > 3)
        {
            m_InputField.SetTextWithoutNotify(arg0.Substring(0, 3));
        }
    }
    private void OnInputFieldEndEdit(string arg0)
    {
        //只允许输入30-600的整数
        int value = Convert.ToInt32(arg0);
        Sanitizers.ClampInt30To600(ref value);
        //修改表现
        m_InputField.SetTextWithoutNotify(value.ToString());
        m_Slider.SetValueWithoutNotify(value);
        //保存设置
        SaveManager.SetValue(SaveDefine.CustomFrameRate, value, false);
    }
    private void OnSliderValueChanged(float arg0)
    {
       m_InputField.SetTextWithoutNotify(((int)arg0).ToString());
        //保存设置
        SaveManager.SetValue(SaveDefine.CustomFrameRate, (int)arg0, false);
    }
}
