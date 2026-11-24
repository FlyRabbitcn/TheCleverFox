using UnityEngine;

namespace FlyRabbit.StateMachine.FoxState
{
    public class Dead : FoxStateBase
    {

        public override void OnEnter()
        {
            //进入状态时，播放动画和音效，禁用重力，归零速度
            //StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_Dead);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_Dead;
            AudioManager.PlaySFX(AudioPath.SFX.Die);
            StateMachine.Owner.Rigidbody2D.gravityScale = 0;
            StateMachine.Owner.Rigidbody2D.velocity = Vector2.zero;
        }
        public override void OnExit()
        {
            //离开时复原重力
            StateMachine.Owner.Rigidbody2D.gravityScale = 1;
        }


        public Dead(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}