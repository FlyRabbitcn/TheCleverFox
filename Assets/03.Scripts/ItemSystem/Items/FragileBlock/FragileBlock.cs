using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileBlock : Item
{
    public override void OnAttacked(MonoBehaviour attacker)
    {
        MapManager.Instance.RemoveItem(this);
    }
}
