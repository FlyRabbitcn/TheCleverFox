using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public enum ButtonClickSoundType
    {
        Open,
        Close,
    }

    public ButtonClickSoundType SoundType;

    private Button m_Button;


    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        switch (SoundType)
        {
            case ButtonClickSoundType.Open:
                AudioManager.PlaySFX(AudioPath.SFX.Open);
                break;
            case ButtonClickSoundType.Close:
                AudioManager.PlaySFX(AudioPath.SFX.Close);
                break;
            default:
                break;
        }
    }
}
