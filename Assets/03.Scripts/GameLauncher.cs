using FlyRabbit.AssetManagement;
using FlyRabbit.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private void Awake()
    {
        //设置帧率为屏幕刷新率
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        int targetFrameRate = Mathf.RoundToInt((float)refreshRate.value / refreshRate.denominator);
        Application.targetFrameRate = targetFrameRate;
        //初始化AudioManager
        AudioManager.AddAssetProvider(new AddressableAssetProvider());
        //初始化UIManager
        UIManager.AddAssetProvider(new AddressableAssetProvider());
        //初始化InputManager
        InputManager.Instance.OnEscapeEvent += UIManager.ResponseEscape;
    }

    private void Start()
    {
        Destroy(gameObject);
    }
}
