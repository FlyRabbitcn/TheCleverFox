using FlyRabbit.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SettingsManager
{
    /// <summary>当前可用的分辨率方案，从高到低排序</summary>
    public static Resolution[] Resolutions { get; private set; }

    /// <summary>
    /// 初始化设置管理器并注册保存项变更回调
    /// </summary>
    public static void Init()
    {
        GetResolutions();
        // 注册 BGM 音量变更回调
        SaveManager.Register(SaveDefine.BGMVolume, OnBGMVolumeChanged);
        // 注册 SFX 音量变更回调
        SaveManager.Register(SaveDefine.SFXVolume, OnSFXVolumeChanged);
        // 注册 是否使用显示刷新率 变更回调
        SaveManager.Register(SaveDefine.UseDisplayRefreshRate, OnUseDisplayRefreshRateChanged);
        // 注册 自定义帧率 变更回调
        SaveManager.Register(SaveDefine.CustomFrameRate, OnCustomFrameRateChanged);
        // 注册 是否全屏 变更回调
        SaveManager.Register(SaveDefine.IsFullScreen, OnIsFullScreenChanged);
        // 注册 分辨率方案 变更回调
        SaveManager.Register(SaveDefine.ResolutionScheme, OnResolutionSchemeChanged);
    }
    /// <summary>
    /// 获取可用分辨率方案
    /// </summary>
    private static void GetResolutions()
    {
        //Resolutions = Screen.resolutions
        //.GroupBy(x => new { x.width, x.height })
        //.Select(x => x.OrderByDescending(r => r.refreshRateRatio).First())
        //.OrderByDescending(x => x.width)
        //.ThenByDescending(x => x.height)
        //.ToArray();


        Dictionary<Vector2Int, Resolution> resolutionMap = new();

        foreach (var item in Screen.resolutions)
        {
            Vector2Int key = new Vector2Int(item.width, item.height);

            if ((resolutionMap.TryGetValue(key, out Resolution current) == false) || (item.refreshRateRatio.value > current.refreshRateRatio.value))
            {
                resolutionMap[key] = item;
            }
        }

        Resolutions = resolutionMap.Values
        .OrderByDescending(x => x.width)
        .ThenByDescending(x => x.height)
        .ToArray();
    }


    private static void OnResolutionSchemeChanged(int obj)
    {
        Resolution resolution = Resolutions[obj];
        // 设置屏幕分辨率，保持当前全屏模式状态
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    private static void OnIsFullScreenChanged(bool obj)
    {
        if (obj)
        {
            //Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
            //SaveManager.SetValue(SaveDefine.ResolutionScheme, 0, true); // 设置为100%分辨率方案

            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            // 设置为窗口模式
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    private static void OnUseDisplayRefreshRateChanged(bool Use)
    {
        // 当 Use 为 true 时使用显示器刷新率，否则使用自定义帧率
        int targetFrameRate = 0;
        if (Use)
        {
            RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
            targetFrameRate = Mathf.RoundToInt((float)refreshRate.value / refreshRate.denominator);
        }
        else
        {
            targetFrameRate = SaveManager.GetValue(SaveDefine.CustomFrameRate);
        }
        Application.targetFrameRate = targetFrameRate;
    }

    private static void OnSFXVolumeChanged(int volume)
    {
        AudioManager.SFXVolume = volume;
    }

    private static void OnBGMVolumeChanged(int volume)
    {
        AudioManager.BGMVolume = volume;
    }

    /// <summary>
    /// 自定义帧率变更处理
    /// </summary>
    private static void OnCustomFrameRateChanged(int frameRate)
    {
        // 检查当前是否使用显示器刷新率
        bool useDisplay = SaveManager.GetValue(SaveDefine.UseDisplayRefreshRate);
        // 如果没有使用显示器刷新率，则将应用的目标帧率设置为自定义帧率
        if (!useDisplay)
        {
            Application.targetFrameRate = frameRate;
        }
    }
}
