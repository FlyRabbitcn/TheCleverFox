using FlyRabbit.UIFramework;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindItem : MonoBehaviour
{
    [SerializeField]
    private Button m_ResetButton;
    [SerializeField]
    private Button m_RebindButton;
    [SerializeField]
    private TextMeshProUGUI m_ActionDisplayText;
    [SerializeField]
    private TextMeshProUGUI m_BindingText;

    [BoxGroup("Action参数")]
    [SerializeField]
    private string m_ActionName;
    [BoxGroup("Action参数")]
    [SerializeField]
    private int m_Index;

    private void Awake()
    {
        m_ResetButton.onClick.AddListener(OnResetButtonClick);
        m_RebindButton.onClick.AddListener(OnRebindButtonClick);
    }

    private void Start()
    {
        Refresh();
    }
    private void Refresh()
    {
        //更新键位显示
        m_BindingText.text = InputManager.Instance.GetHumanReadableBindingString(m_ActionName, m_Index);
        //更新重置按钮是否显示
        m_ResetButton.gameObject.SetActive(InputManager.Instance.IsBindingChanged(m_ActionName, m_Index));
    }

    #region Listeners
    private void OnRebindButtonClick()
    {
        RebindControlsPanelOpenData openData = new RebindControlsPanelOpenData(m_ActionDisplayText.text, m_ActionName, m_Index, RebindingCallback);
        UIManager.OpenPanel<RebindControlsPanel>(openData);
    }

    private void OnResetButtonClick()
    {
        InputManager.Instance.ResetBinding(m_ActionName, m_Index);
        Refresh();
    }
    private void RebindingCallback()
    {
        Refresh();
    }
    #endregion
}
