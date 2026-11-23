using UnityEngine;

namespace FlyRabbit.StateMachine
{
    public class TestPlayer : MonoBehaviour
    {
        private StateMachine<TestPlayer> m_StateMachine;

        private TestPalyerIdleState m_TestPalyerIdleState;
        private void Awake()
        {
            m_StateMachine = new StateMachine<TestPlayer>(this);

            m_TestPalyerIdleState = new TestPalyerIdleState(m_StateMachine);
        }

        private void Start()
        {
            m_StateMachine.ChangeState(m_TestPalyerIdleState);
        }

        private void Update()
        {
            m_StateMachine.Update();
        }

        private void FixedUpdate()
        {
            m_StateMachine.FixedUpdate();
        }
    }
}
