using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;

/// <summary>语言选择下拉菜单组件</summary>
[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageSelectionDropdownMenu : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;
    private ScrollRect m_ScrollRect;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.onValueChanged.AddListener(OnSelectionChanged);
        m_ScrollRect = m_Dropdown.template.GetComponent<ScrollRect>();

        InitOptions();
    }




    private void OnSelectionChanged(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    /// <summary>初始化选项</summary>
    /// <returns></returns>
    private void InitOptions()
    {
        List<string> options = ListPool<string>.Get();
        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        int SelectedIndex = 0;

        for (int i = 0; i < locales.Count; i++)
        {
            Locale item = locales[i];
            if (LocalizationSettings.SelectedLocale == item)
            {
                SelectedIndex = i;
            }

            options.Add(item.LocaleName);
        }

        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(options);
        ListPool<string>.Release(options);

        m_Dropdown.SetValueWithoutNotify(SelectedIndex);
    }
}
