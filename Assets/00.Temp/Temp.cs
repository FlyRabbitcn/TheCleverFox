using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;
    public Button BGMButton;

    public Button SFXButton;
    private void Awake()
    {
        AudioManager.AddAssetProvider(new AddressableAssetProvider());


        BGMVolumeSlider.onValueChanged.AddListener(OnBGMVolumeSliderValueChanged);
        SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderValueChanged);
        BGMButton.onClick.AddListener(OnBGMButtonClick);
        SFXButton.onClick.AddListener(OnSFXButtonClick);
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
    }

    private void OnSFXVolumeSliderValueChanged(float value)
    {
        AudioManager.SFXVolume = (int)value;
    }
}
