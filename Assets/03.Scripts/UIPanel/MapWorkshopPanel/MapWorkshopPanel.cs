using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;

public class MapWorkshopPanel : UIPanel
{


    #region Unity Methods
    private void OnEnable()
    {
        MapManager.Instance.SetMapGridVisible(true);
    }

    private void OnDisable()
    {
        MapManager.Instance.SetMapGridVisible(false);
    }
    #endregion



    #region Override
    public override void OnEscape()
    {
        CloseSelf();
        UIManager.OpenPanel<MainPanel>();
    }
    #endregion
}
