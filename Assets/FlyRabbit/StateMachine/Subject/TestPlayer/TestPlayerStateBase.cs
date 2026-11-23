namespace FlyRabbit.StateMachine
{
    public class TestPlayerStateBase : StateBase<TestPlayer>
    {
        public TestPlayerStateBase(StateMachine<TestPlayer> stateMachine) : base(stateMachine)
        {
        }
    }
}