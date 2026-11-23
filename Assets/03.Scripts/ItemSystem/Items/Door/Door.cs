using FlyRabbit.EventBus;
using UnityEngine;

public class Door : Item
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        //如果我已经被冻结则不做响应
        if (IsFrozen)
        {
            return;
        }
        //如果碰撞的物体不在我的中心位置附近则不作响应
        if (Mathf.Abs(collision.transform.position.x - transform.position.x) > 0.05)
        {
            return;
        }
        //如果碰撞的物体不是Fox则不做响应
        if (collision.gameObject.GetComponent<Fox>() == null)
        {
            return;
        }
        //发送事件
        EventBus.TriggerEvent(EventMessage.Gameplay.OnPlayerTryOpenDoor.Instance);
    }
}
