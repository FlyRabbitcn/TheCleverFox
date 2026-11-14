using FlyRabbit.UIFramework;
using TMPro;
using UnityEngine;

public class RebindControlsPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI m_BindText;

    private RebindControlsPanelOpenData m_OpenData;

    private void Awake()
    {

    }


    public override void OnOpen(OpenData openData)
    {
        m_OpenData = openData as RebindControlsPanelOpenData;


        m_BindText.text = string.Format(LocalizationManager.GetLocalizedString(LocalizationKey.Bind), m_OpenData.ActionDisplayName);

        bool temp = InputManager.Instance.PerformInteractiveRebinding(m_OpenData.ActionName, m_OpenData.Index, CompleteCallback, CancelCallback);

        if (temp == false)
        {
            Debug.LogError("开启交互式按键重映射失败");
            CloseSelf();
        }
    }

    private void CompleteCallback()
    {
        CloseSelf();
        m_OpenData.Callback?.Invoke();
    }

    private void CancelCallback()
    {
        CloseSelf();
    }
}
