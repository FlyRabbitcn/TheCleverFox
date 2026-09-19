using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, GlobalInput.IUIActions
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


    private GlobalInput m_GlobalInput;


    #region Unity Methods
    private void Awake()
    {
        //初始化m_GlobalInput
        m_GlobalInput = new GlobalInput();
        m_GlobalInput.Enable();
        m_GlobalInput.UI.SetCallbacks(this);
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
    #endregion

}
