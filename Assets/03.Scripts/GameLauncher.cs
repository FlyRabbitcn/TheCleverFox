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
        InputManager.Instance.LoadBindings();
        //初始化SettingsManager
        SettingsManager.Init();
        //打开第一个UI界面
        if (SaveManager.GetValue(SaveDefine.HasSelectedLanguage) == false)
        {
            Action closeCallback = () => { UIManager.OpenPanel<MainPanel>(); };
            UIManager.OpenPanel<SelectLanguagePanel>(null, closeCallback);
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
