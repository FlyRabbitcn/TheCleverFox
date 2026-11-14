using FlyRabbit.UIFramework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, GlobalInput.IUIActions, GlobalInput.IPlayerActions
{
    #region Singleton
    private static InputManager m_Instance;

    /// <summary>单例</summary>
	public static InputManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject gameObject = new GameObject(typeof(InputManager).Name);
                DontDestroyOnLoad(gameObject);
                m_Instance = gameObject.AddComponent<InputManager>();
            }
            return m_Instance;
        }
    }
    #endregion

    #region Events
    public event Action OnEscapeEvent;
    #endregion


    private GlobalInput GlobalInput { get; set; }

    private string BindingsFilePath { get => Path.Combine(Application.persistentDataPath, "FlyRabbit", "InputManager", "InputBindings.json"); }

    #region Unity Methods
    private void Awake()
    {
        //初始化m_GlobalInput
        GlobalInput = new GlobalInput();
        GlobalInput.Enable();
        GlobalInput.UI.SetCallbacks(this);
        GlobalInput.Player.SetCallbacks(this);
    }
    #endregion


    #region Public Methods
    /// <summary>获取人类可读的当前绑定的按键名</summary>
    /// <param name="actionName"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetHumanReadableBindingString(string actionName, int index)
    {
        InputAction inputAction = GlobalInput.FindAction(actionName);

        if (inputAction == null || inputAction.bindings.Count == 0)
        {
            return "";
        }
        if (index < 0 || index >= inputAction.bindings.Count)
        {
            Debug.LogError("参数越界，请检查");
            return "";
        }
        index += inputAction.GetBindingIndexForControl(inputAction.controls[0]);

        return InputControlPath.ToHumanReadableString(inputAction.bindings[index].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
    /// <summary>执行交互式重映射，返回是否成功执行</summary>
    /// <param name="actionName"></param>
    /// <param name="index"></param>
    /// <param name="complateCallback"></param>
    /// <param name="cancelCallback"></param>
    /// <returns></returns>
    public bool PerformInteractiveRebinding(string actionName, int index, Action complateCallback, Action cancelCallback)
    {
        InputAction inputAction = GlobalInput.FindAction(actionName);
        if (inputAction == null || inputAction.bindings.Count == 0)
        {
            return false;
        }
        if (index < 0 || index >= inputAction.bindings.Count)
        {
            Debug.LogError("参数越界，请检查");
            return false;
        }
        index += inputAction.GetBindingIndexForControl(inputAction.controls[0]);

        //开始重绑定
        inputAction.Disable();
        inputAction.PerformInteractiveRebinding(index)
            //这段可以允许设置组合按键，这里就不允许了
            //.OnMatchWaitForAnother(0.15f)
            .OnComplete((operation) =>
            {
                inputAction.Enable();
                operation.Dispose();
                complateCallback?.Invoke();
            }).OnCancel((operation) =>
            {
                inputAction.Enable();
                operation.Dispose();
                cancelCallback?.Invoke();
            }).WithCancelingThrough(GlobalInput.UI.Escape.bindings[0].effectivePath)
            .Start();

        return true;
    }
    /// <summary>判断某个键位是否被重新绑定了</summary>
    /// <param name="actionName"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsBindingChanged(string actionName, int index)
    {
        InputAction inputAction = GlobalInput.FindAction(actionName);

        if (inputAction == null || inputAction.bindings.Count == 0)
        {
            return false;
        }
        if (index < 0 || index >= inputAction.bindings.Count)
        {
            Debug.LogError("参数越界，请检查");
            return false;
        }
        index += inputAction.GetBindingIndexForControl(inputAction.controls[0]);

        string currentPath = inputAction.bindings[index].effectivePath;
        string originalPath = inputAction.bindings[index].path;

        return currentPath != originalPath;
    }

    /// <summary>读取按键配置</summary>
    public void LoadBindings()
    {
        if (!File.Exists(BindingsFilePath))
        {
            return;
        }

        string json = File.ReadAllText(BindingsFilePath);

        GlobalInput.LoadBindingOverridesFromJson(json);
    }

    /// <summary>保存按键配置</summary>
    public void SaveBindings()
    {
        //获取Json字符串
        string json = GlobalInput.SaveBindingOverridesAsJson();
        // 确保目录存在
        Directory.CreateDirectory(Path.GetDirectoryName(BindingsFilePath));
        // 保存 JSON 文件
        File.WriteAllText(BindingsFilePath, json);
    }

    /// <summary>重置指定按键绑定</summary>
    public void ResetBinding(string actionName, int index)
    {
        InputAction inputAction = GlobalInput.FindAction(actionName);

        if (inputAction == null || inputAction.bindings.Count == 0)
        {
            return;
        }
        if (index < 0 || index >= inputAction.bindings.Count)
        {
            Debug.LogError("参数越界，请检查");
            return;
        }
        index += inputAction.GetBindingIndexForControl(inputAction.controls[0]);
        inputAction.RemoveBindingOverride(index);
    }
    #endregion

    #region 接口实现
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.canceled == false)
        {
            return;
        }

        OnEscapeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started == false && context.canceled == false)
        {
            return;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started == false)
        {
            return;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started == false)
        {
            return;
        }
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.started == false)
        {
            return;
        }
    }
    #endregion

}
