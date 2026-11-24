using System.Diagnostics;

namespace FlyRabbit.StateMachine.FoxState
{
    public class Idle : FoxStateBase
    {
        public override void OnEnter()
        {
            //播放动画
            // StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_Idle);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_Idle;
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
            //当玩家在地面上移动时切换到Move状态
            if (InputManager.Instance.MovementFactor != 0)
            {
                StateMachine.ChangeState(StateMachine.Owner.MoveState);
                return;
            }
            StateMachine.Owner.Idle();
        }

        public override void OnExit()
        {
            InputManager.Instance.OnJumpEvent -= StateMachine.Owner.Jump;
            InputManager.Instance.OnAttackEvent -= StateMachine.Owner.Attack;
            InputManager.Instance.OnPlaceEvent -= StateMachine.Owner.Place;
        }


        public Idle(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}