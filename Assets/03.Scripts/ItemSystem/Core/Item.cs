
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>是否被冻结</summary>
    public bool IsFrozen { get; private set; }

    /// <summary>设置物体的冻结状态</summary>
    /// <param name="isFrozen">是否冻结</param>
    public void SetFrozen(bool isFrozen)
    {
        if (isFrozen == IsFrozen)
        {
            return;
        }

        IsFrozen = isFrozen;

        if (isFrozen)
        {
            OnFreeze();
        }
        else
        {
            OnUnfreeze();
        }
    }

    #region Virtual Methods

    /// <summary>冻结时调用</summary>
    protected virtual void OnFreeze() { }

    /// <summary>解冻时调用</summary>
    protected virtual void OnUnfreeze() { }

    /// <summary>受到攻击时调用</summary>
    /// <param name="attacker">攻击者</param>
    public virtual void OnAttacked(MonoBehaviour attacker) { }

    /// <summary>初始化物体</summary>
    public virtual void Initialize() { }

    #endregion
}

