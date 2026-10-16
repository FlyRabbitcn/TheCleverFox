using FlyRabbit.AssetManagement;
using FlyRabbit.SaveSystem;
using FlyRabbit.UIFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;

public class GameLauncher : MonoBehaviour
{
    private void Awake()
    {
        //初始化Addressable
        Addressables.InitializeAsync().WaitForCompletion();
        //初始化Localization
        LocalizationSettings.InitializationOperation.WaitForCompletion();
        //初始化AudioManager
        AudioManager.AddAssetProvider(new AddressableAssetProvider());
        //初始化UIManager
        UIManager.AddAssetProvider(new AddressableAssetProvider());
        //初始化InputManager
        InputManager.Instance.OnEscapeEvent += UIManager.ResponseEscape;
        //初始化SettingsManager
        SettingsManager.Init();
        //打开第一个UI界面
        if (SaveManager.GetValue(SaveDefine.HasSelectedLanguage) == false)
        {
            Action action = () => { UIManager.OpenPanel<MainPanel>(); };
            CloseCallback closeCallback = new CloseCallback(action);
            UIManager.OpenPanel<SelectLanguagePanel>(closeCallback);
        }
        else
        {
            UIManager.OpenPanel<MainPanel>();
        }

    }

    private void Start()
    {
        Destroy(gameObject);
    }
}
