namespace FlyRabbit.StateMachine.FoxState
{
    public class InAir : FoxStateBase
    {

        public override void OnEnter()
        {
            //更新动画参数参数
            StateMachine.Owner.UpdateVerticalState();
            //切换动画
            //StateMachine.Owner.Animator.Play(StateMachine.Owner.AnimationHash_InAir);
            StateMachine.Owner.CurrentAnimationHash = StateMachine.Owner.AnimationHash_InAir;
            //监听事件,让玩家可以攻击
            InputManager.Instance.OnAttackEvent += StateMachine.Owner.Attack;
        }

        public override void Update()
        {
            //更新动画参数参数
            StateMachine.Owner.UpdateVerticalState();
        }
        public override void FixedUpdate()
        {
            //如果玩家落地了切换状态
            if (StateMachine.Owner.CheckGrounded2D.Grounded == true)
            {
                StateMachine.ChangeState(StateMachine.Owner.IdleState);
                return;
            }


            //让玩家在空中移动
            StateMachine.Owner.MoveInAir();
        }
        public override void OnExit()
        {
            InputManager.Instance.OnAttackEvent -= StateMachine.Owner.Attack;
        }
        public InAir(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}