using UnityEngine;

namespace FlyRabbit.StateMachine.FoxState
{
    public class Frozen : FoxStateBase
    {

        public override void OnEnter()
        {
            //进入冻结状态时，重置动画，禁用重力，归零速度
            //StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_Idle);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_Idle;
            StateMachine.Owner.Rigidbody2D.gravityScale = 0;
            StateMachine.Owner.Rigidbody2D.velocity = Vector2.zero;
        }
        public override void OnExit()
        {
            //离开时复原重力
            StateMachine.Owner.Rigidbody2D.gravityScale = 1;
        }


        public Frozen(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}