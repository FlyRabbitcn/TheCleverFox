using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;
using UnityEngine.UI;

public class MapWorkshopPanel : UIPanel
{
    [SerializeField]
    private Button m_ExitButton;




    private CameraSizeAdapter m_CameraSizeAdapter;

    #region Unity Methods

    private void Awake()
    {
        m_CameraSizeAdapter = Camera.main.GetComponent<CameraSizeAdapter>();


        m_ExitButton.onClick.AddListener(OnExitButtonClick);
    }
    private void OnEnable()
    {
        MapManager.Instance.SetMapGridVisible(true);
        m_CameraSizeAdapter?.SetEnableOffsset(true);
    }

    private void OnDisable()
    {
        MapManager.Instance.SetMapGridVisible(false);
        m_CameraSizeAdapter?.SetEnableOffsset(false);
    }
    #endregion



    #region Override
    #endregion


    #region Listeners

    private void OnExitButtonClick()
    {
        UIManager.OpenPanel<PopupMessagePanel>(PopupMessageOpenData.ExitMapWorkshop);
    }
    #endregion
}