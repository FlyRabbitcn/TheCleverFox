using FlyRabbit.StateMachine;

namespace FlyRabbit.StateMachine.FoxState
{
    public class FoxStateBase : StateBase<Fox>
    {
        public FoxStateBase(StateMachine<Fox> stateMachine) : base(stateMachine) { }
    }
}