using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using FlyRabbit.SaveSystem;
using System;

public class Temp : MonoBehaviour
{
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;
    public Button BGMButton;
    public Button SFXButton;

    public Text BGMVolumeText;
    private void Awake()
    {
        AudioManager.AddAssetProvider(new AddressableAssetProvider());


        BGMVolumeSlider.onValueChanged.AddListener(OnBGMVolumeSliderValueChanged);
        SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderValueChanged);
        BGMButton.onClick.AddListener(OnBGMButtonClick);
        SFXButton.onClick.AddListener(OnSFXButtonClick);


        BGMVolumeSlider.value = SaveManager.GetValue(SaveDefine.BGMVolume);
    }
    private void OnEnable()
    {
        SaveManager.Register(SaveDefine.BGMVolume, OnBGMVolumeChanged);
    }

    private void OnBGMVolumeChanged(int obj)
    {
        BGMVolumeText.text = obj.ToString();
    }

    private void OnDisable()
    {
        SaveManager.Unregister(SaveDefine.BGMVolume, OnBGMVolumeChanged);
    }


    private void OnBGMButtonClick()
    {
        AudioManager.PlayBGM(AudioPath.BGM.Home);
    }

    private void OnSFXButtonClick()
    {
        AudioManager.PlaySFX(AudioPath.SFX.Bubble);
    }

    private void OnBGMVolumeSliderValueChanged(float value)
    {
        AudioManager.BGMVolume = (int)value;

        SaveManager.SetValue(SaveDefine.BGMVolume, (int)value);
    }

    private void OnSFXVolumeSliderValueChanged(float value)
    {
        AudioManager.SFXVolume = (int)value;
    }
}
