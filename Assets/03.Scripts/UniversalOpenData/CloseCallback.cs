using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlyRabbit.UIFramework;
using System;

public class CloseCallback : OpenData
{
    public Action Action;

    public CloseCallback(Action action)
    {
        Action = action;
    }
}
