
namespace FlyRabbit.StateMachine
{
    public class StateMachine<T> where T : class
    {
        /// <summary>状态机的所有者</summary>
        public T Owner { get; private set; }
        /// <summary>当前状态</summary>
        public StateBase<T> CurrentState { get; private set; }
        /// <summary>改变当前状态</summary>
        public void ChangeState(StateBase<T> state)
        {
            if (state.StateMachine != this)
            {
                return;
            }
            if (CurrentState == state)
            {
                return;
            }
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }
            CurrentState = state;
            CurrentState.OnEnter();
        }

        public void Update()
        {
            if (CurrentState == null)
            {
                return;
            }
            CurrentState.Update();
        }

        public void FixedUpdate()
        {
            if (CurrentState == null)
            {
                return;
            }
            CurrentState.FixedUpdate();
        }
        public StateMachine(T owner)
        {
            Owner = owner;
        }
    }
}