using DG.Tweening;
using FlyRabbit.UIFramework;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveMapPanel : UIPanel
{
    [SerializeField]
    private Button m_CloseButton;
    [SerializeField]
    private TMP_InputField m_MapNameInputField;
    [SerializeField]
    private TextMeshProUGUI m_TipsText;
    [SerializeField]
    private Button m_ConfirmButton;

    private bool m_IsValid = false;
    private void Awake()
    {
        m_CloseButton.onClick.AddListener(OnCloseButtonClick);
        m_MapNameInputField.onValueChanged.AddListener(OnMapNameInputFieldValueChanged);
        m_ConfirmButton.onClick.AddListener(OnConfirmButtonClick);
    }
    private void Start()
    {
        CheckValidAndUpdateTipsText();
    }

    #region Listeners

    private void OnCloseButtonClick()
    {
        CloseSelf();
    }

    private void OnMapNameInputFieldValueChanged(string value)
    {
        CheckValidAndUpdateTipsText();
    }

    private void OnConfirmButtonClick()
    {
        if (m_IsValid == false)
        {
            AudioManager.PlaySFX(AudioPath.SFX.Denied);
            m_TipsText.transform.DOShakePosition(0.4f, 10f, 20, 90f).SetLink(gameObject);
            return;
        }
        AudioManager.PlaySFX(AudioPath.SFX.Open);
        MapManager.Instance.SaveMap(m_MapNameInputField.text);
        CloseSelf();
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.SaveSuccess);
    }
    #endregion
    /// <summary>检查地图是否合法并更新Tips文本</summary>
    private void CheckValidAndUpdateTipsText()
    {
        string name = m_MapNameInputField.text;
        //检查名称是否为空
        if (string.IsNullOrEmpty(name))
        {
            m_TipsText.color = Color.red;
            m_TipsText.text = LocalizationKey.NameCannotBeEmpty;
            m_IsValid = false;
            return;
        }
        //检查名称长度
        if (name.Length > 16)
        {
            m_TipsText.color = Color.red;
            m_TipsText.text = LocalizationKey.NameTooLong;
            m_IsValid = false;
            return;
        }
        // 检查名称是否包含非法字符
        char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
        StringBuilder check = new StringBuilder(5);

        foreach (var invalidChar in invalidChars)
        {
            foreach (var nameChar in name)
            {
                if (nameChar == invalidChar)
                {
                    check.Append(invalidChar);
                    break;
                }
            }
        }
        if (check.Length > 0)
        {
            m_TipsText.color = Color.red;
            m_TipsText.text = LocalizationKey.NameContainsInvalidCharacters + check.ToString();
            m_IsValid = false;
            return;
        }
        //检查是否存在同名地图
        if (FileManager.CheckCustomMapExists(name))
        {
            m_TipsText.color = Color.blue;
            m_TipsText.text = LocalizationKey.DuplicateName;
            m_IsValid = true;
            return;
        }

        m_TipsText.color = Color.green;
        m_TipsText.text = LocalizationKey.NameAvailable;
        m_IsValid = true;
    }
}
