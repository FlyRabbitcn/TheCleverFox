using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlyRabbit.StateMachine
{
    public class StateBase<T> where T : class
    {
        /// <summary>此状态所属的状态机</summary>
        public StateMachine<T> StateMachine { get; private set; }

        /// <summary>进入状态时</summary>
        public virtual void OnEnter() { }
        /// <summary>离开状态时</summary>
        public virtual void OnExit() { }
        /// <summary>状态中Update</summary>
        public virtual void Update() { }
        /// <summary>状态中FixedUpdate</summary>
        public virtual void FixedUpdate() { }


        public StateBase(StateMachine<T> stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}