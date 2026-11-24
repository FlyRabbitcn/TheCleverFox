using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace FlyRabbit.StateMachine.FoxState
{
    public class Move : FoxStateBase
    {

        public override void OnEnter()
        {
            //播放动画
            //StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_Move);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_Move;
            //监听事件,让玩家可以跳、攻击、放置
            InputManager.Instance.OnJumpEvent += StateMachine.Owner.Jump;
            InputManager.Instance.OnAttackEvent += StateMachine.Owner.Attack;
            InputManager.Instance.OnPlaceEvent += StateMachine.Owner.Place;
        }

        public override void FixedUpdate()
        {
            //如果玩家离开了地面，切换至InAir状态
            if (StateMachine.Owner.CheckGrounded2D.Grounded == false)
            {
                StateMachine.ChangeState(StateMachine.Owner.InAirState);
                return;
            }
            //如果在地面上，玩家停止了移动输入，切换到Idle状态
            if (InputManager.Instance.MovementFactor == 0)
            {
                StateMachine.ChangeState(StateMachine.Owner.IdleState);
                return;
            }
            //让玩家在地面上移动,并根据当前速度调整动画速度
            Fox fox = StateMachine.Owner;
            fox.MoveOnGround();
            //fox.Animator.speed = Mathf.Abs(fox.Rigidbody2D.velocity.x) / fox.MaxMovementSpeed;
            fox.AnimationSpeed = Mathf.Abs(fox.Rigidbody2D.velocity.x) / fox.MaxMovementSpeed;
        }
        public override void OnExit()
        {
            //恢复动画的播放速度
            //StateMachine.Owner.Animator.speed = 1;
            StateMachine.Owner.AnimationSpeed = 1;
            InputManager.Instance.OnJumpEvent -= StateMachine.Owner.Jump;
            InputManager.Instance.OnAttackEvent -= StateMachine.Owner.Attack;
            InputManager.Instance.OnPlaceEvent -= StateMachine.Owner.Place;
        }
        public Move(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}