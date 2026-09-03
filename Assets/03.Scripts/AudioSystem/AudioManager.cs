using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AudioManager
{
    /// <summary>
    /// 音频管理器的根对象
    /// </summary>
    private static readonly GameObject m_Root;
    /// <summary>
    /// BGM播放器
    /// </summary>
    private static readonly AudioSource m_BGMPlayer;
    
    /// <summary>
    /// SFX播放器
    /// </summary>
    private static readonly AudioSource m_SFXPlayer;

    /// <summary>
    /// 资源提供者列表
    /// </summary>
    private static readonly List<IAssetProvider> m_AssetProviders = new List<IAssetProvider>();

    /// <summary>
    /// 音频剪辑缓存
    /// </summary>
    private static readonly Dictionary<string, AudioClip> m_AudioClipCache = new Dictionary<string, AudioClip>();

    #region Backing Fields
    private static int m_BGMVolume = 100;
    private static int m_SFXVolume = 100;
    #endregion



    #region Properties
    /// <summary>
    /// 获取或设置BGM音量(0~100)
    /// </summary>
    public static int BGMVolume
    {
        get
        {
            return m_BGMVolume;
        }
        set
        {
            m_BGMVolume = Mathf.Clamp(value, 0, 100);
            m_BGMPlayer.volume = m_BGMVolume / 100f;
        }
    }
    /// <summary>
    /// 获取或设置SFX音量(0~100)
    /// </summary>
    public static int SFXVolume
    {
        get
        {
            return m_SFXVolume;
        }
        set
        {
            m_SFXVolume = Mathf.Clamp(value, 0, 100);
            m_SFXPlayer.volume = m_SFXVolume / 100f;
        }
    }
    #endregion


    #region Public Methods
    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="path"></param>
    public static void PlayBGM(string path)
    {
        AudioClip audioClip = GetAudioClip(path);
        //如果当前播放的BGM和要播放的BGM是同一个，就不需要重新播放了
        if (m_BGMPlayer.clip == audioClip)
        {
            return;
        }
        //播放
        m_BGMPlayer.clip = audioClip;
        m_BGMPlayer.Play();
    }

    /// <summary>
    /// 播放SFX
    /// </summary>
    /// <param name="path"></param>
    public static void PlaySFX(string path)
    {
        AudioClip audioClip = GetAudioClip(path);
        if (audioClip == null)
        {
            return;
        }
        m_SFXPlayer.PlayOneShot(audioClip);
    }


    /// <summary>
    /// 添加一个AssetProvider
    /// </summary>
    /// <param name="assetProvider"></param>
    public static void AddAssetProvider(IAssetProvider assetProvider)
    {
        if (assetProvider == null)
        {
            Debug.LogError("尝试添加一个空的AssetProvider，操作已被忽略");
            return;
        }
        if (m_AssetProviders.Contains(assetProvider))
        {
            Debug.LogWarning("尝试添加一个已存在的AssetProvider，操作已被忽略");
            return;
        }
        m_AssetProviders.Add(assetProvider);
    }

    #endregion


    #region Private Methods
    /// <summary>
    /// 获取音频剪辑,如果获取不到就返回null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static AudioClip GetAudioClip(string path)
    {
        //先尝试从缓存中获取
        if (m_AudioClipCache.TryGetValue(path, out AudioClip result))
        {
            return result;
        }
        //缓存中没有，从assetProvider中获取,然后缓存
        result = LoadAudioClip(path);
        if (result != null)
        {
            m_AudioClipCache[path] = result;
        }
        return result;
    }

    /// <summary>
    /// 加载音频剪辑,如果加载不到就返回null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static AudioClip LoadAudioClip(string path)
    {
        //如果没有注册任何assetProvider，直接返回null
        if (m_AssetProviders.Count == 0)
        {
            Debug.LogError("没有注册任何assetProvider，无法加载音频剪辑");
            return null;
        }
        //遍历所有注册的assetProvider，尝试加载音频剪辑
        foreach (var assetProvider in m_AssetProviders)
        {
            AudioClip audioClip = assetProvider.Load<AudioClip>(path);
            if (audioClip != null)
            {
                return audioClip;
            }
        }
        //所有assetProvider都无法加载音频剪辑，返回null
        Debug.LogError($"所有assetProvider都无法加载音频剪辑，路径：{path}");
        return null;
    }
    #endregion


    static AudioManager()
    {
        //初始化根节点
        {
            m_Root = new GameObject("[Audio Manager]");
            Object.DontDestroyOnLoad(m_Root);
        }
        //初始化BGM播放器
        {
            GameObject gameObject = new GameObject("BGM Player");
            gameObject.transform.SetParent(m_Root.transform);
            m_BGMPlayer = gameObject.AddComponent<AudioSource>();
            m_BGMPlayer.loop = true;
        }
        //初始化SFX播放器
        {
            GameObject gameObject = new GameObject("SFX Player");
            gameObject.transform.SetParent(m_Root.transform);
            m_SFXPlayer = gameObject.AddComponent<AudioSource>();
            m_SFXPlayer.loop = false;
        }
    }
}
