using FlyRabbit.EventBus;
using FlyRabbit.UIFramework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    /// <param name="offset"></param>
    /// <returns></returns>
    public string GetHumanReadableBindingString(string actionName, int offset)
    {
        if (GetInputActionAndBindingIndex(actionName, offset, out InputAction inputAction, out int bindingIndex) == false)
        {
            return "";
        }
        return InputControlPath.ToHumanReadableString(inputAction.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    ///// <summary>判断指定绑定是否为空</summary>
    ///// <param name="actionName">输入操作名称</param>
    ///// <param name="offset">绑定偏移值</param>
    ///// <returns>绑定为空或获取失败时返回 false</returns>
    //public bool IsBindingEmpty(string actionName, int offset)
    //{
    //    if (!GetInputActionAndBindingIndex(actionName, offset, out InputAction inputAction, out int bindingIndex))
    //    {
    //        return false;
    //    }

    //    return string.IsNullOrEmpty(inputAction.bindings[bindingIndex].effectivePath);
    //}

    /// <summary>执行交互式重映射，返回是否成功执行</summary>
    /// <param name="actionName"></param>
    /// <param name="offset"></param>
    /// <param name="complateCallback"></param>
    /// <param name="cancelCallback"></param>
    /// <returns></returns>
    public bool PerformInteractiveRebinding(string actionName, int offset, Action complateCallback, Action cancelCallback)
    {
        if (GetInputActionAndBindingIndex(actionName, offset, out InputAction inputAction, out int bindingIndex) == false)
        {
            return false;
        }

        foreach (var item in inputAction.bindings)
        {
            Debug.Log(item.name);
        }

        //开始重绑定
        inputAction.Disable();
        inputAction.PerformInteractiveRebinding(bindingIndex)
            //这段可以允许设置组合按键，这里就不允许了
            //.OnMatchWaitForAnother(0.15f)
            .OnComplete((operation) =>
            {
                inputAction.Enable();
                operation.Dispose();
                complateCallback?.Invoke();
                // 清除其他操作中的冲突绑定
                RemoveConflictingBindings(inputAction, bindingIndex);
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
    public bool IsBindingChanged(string actionName, int offset)
    {
        if (GetInputActionAndBindingIndex(actionName, offset, out InputAction inputAction, out int bindingIndex) == false)
        {
            return false;
        }
        string currentPath = inputAction.bindings[bindingIndex].effectivePath;
        string originalPath = inputAction.bindings[bindingIndex].path;

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
    public void ResetBinding(string actionName, int offset)
    {
        if (GetInputActionAndBindingIndex(actionName, offset, out InputAction inputAction, out int bindingIndex) == false)
        {
            return;
        }
        inputAction.RemoveBindingOverride(bindingIndex);
        RemoveConflictingBindings(inputAction, bindingIndex);
    }
    #endregion


    #region Private Methods

    /// <summary>获取InputAction和绑定索引</summary>
    /// <param name="actionName">Action名称</param>
    /// <param name="offset">Binding索引偏移</param>
    /// <param name="inputAction">获取到的InputAction</param>
    /// <param name="bindingIndex">实际Binding索引</param>
    /// <returns>是否获取成功</returns>
    private bool GetInputActionAndBindingIndex(string actionName, int offset, out InputAction inputAction, out int bindingIndex)
    {
        inputAction = GlobalInput.FindAction(actionName);
        bindingIndex = -1;

        if (inputAction == null || inputAction.bindings.Count == 0)
        {
            return false;
        }

        int startIndex = inputAction.bindings[0].isComposite ? 1 : 0;
        int bindingCount = inputAction.bindings.Count - startIndex;

        if (offset < 0 || offset >= bindingCount)
        {
            Debug.LogError("offset越界，请检查");
            return false;
        }

        bindingIndex = startIndex + offset;
        return true;
    }

    /// <summary>移除与指定绑定冲突的其他绑定</summary>
    /// <param name="inputAction">当前绑定所属的 InputAction</param>
    /// <param name="bindingIndex">当前绑定的索引</param>
    private void RemoveConflictingBindings(InputAction inputAction, int bindingIndex)
    {
        if (inputAction == null || bindingIndex < 0 || bindingIndex >= inputAction.bindings.Count)
        {
            return;
        }

        string targetPath = inputAction.bindings[bindingIndex].effectivePath;

        if (string.IsNullOrEmpty(targetPath))
        {
            return;
        }

        bool HasConflicting = false;

        // 遍历所有 InputBinding
        foreach (InputActionMap actionMap in GlobalInput.asset.actionMaps)
        {
            foreach (InputAction action in actionMap.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    InputBinding binding = action.bindings[i];

                    // 跳过当前绑定
                    if (action == inputAction && i == bindingIndex)
                    {
                        continue;
                    }

                    // 跳过复合绑定根节点
                    if (binding.isComposite)
                    {
                        continue;
                    }

                    // 判断是否存在相同的绑定路径
                    if (string.Equals(targetPath, binding.effectivePath, StringComparison.OrdinalIgnoreCase))
                    {
                        // 清除冲突绑定
                        action.ApplyBindingOverride(i, "");
                        HasConflicting = true;
                    }
                }
            }
        }

        if (HasConflicting)
        {
            EventBus.TriggerEvent<OnBindingCleared>(OnBindingCleared.Instance);
        }
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
