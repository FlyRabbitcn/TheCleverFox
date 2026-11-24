using UnityEngine;

namespace FlyRabbit.StateMachine.FoxState
{
    public class Attack : FoxStateBase
    {
        /// <summary>状态持续时间</summary>
        private float m_Duration = 0.25f;
        /// <summary>离开时的时间点</summary>
        private float m_ExitTime;
        public override void OnEnter()
        {
            //播放动画
            //StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_Attack);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_Attack;
            //计算离开时的时间点
            m_ExitTime = Time.time + m_Duration;
        }
        public override void FixedUpdate()
        {
            if (Time.time >= m_ExitTime)
            {
                StateMachine.ChangeState(StateMachine.Owner.IdleState);
            }
        }
        public Attack(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}