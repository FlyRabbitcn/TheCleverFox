using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyRabbit.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, LinkedList<Delegate>> m_EventTable = new Dictionary<Type, LinkedList<Delegate>>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        public static void AddListener<T>(Action<T> listener) where T : IEventMessage
        {
            Type type = typeof(T);

            if (m_EventTable.ContainsKey(type) == false)
            {
                m_EventTable[type] = new LinkedList<Delegate>();
            }

#if UNITY_EDITOR
            if (m_EventTable[type].Contains(listener))
            {
                Debug.LogWarning("你在试图重复添加监听，已被阻止，此功能只在编辑器下有效，请在发布前修复此问题。");
                return;
            }
#endif
            m_EventTable[type].AddLast(listener);
        }
        /// <summary>
        /// 移出事件监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        public static void RemoveListener<T>(Action<T> listener) where T : IEventMessage
        {
            Type type = typeof(T);
            if (m_EventTable.ContainsKey(type) == false)
            {
                return;
            }

            m_EventTable[type].Remove(listener);
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public static void TriggerEvent<T>(T message) where T : IEventMessage
        {
            Type type = typeof(T);
            if (m_EventTable.TryGetValue(type, out LinkedList<Delegate> delegates) == false)
            {
                return;
            }

            LinkedListNode<Delegate> currentNode = delegates.Last;

            while (currentNode != null)
            {
                LinkedListNode<Delegate> previous = currentNode.Previous;

                ((Action<T>)currentNode.Value).Invoke(message);

                currentNode = previous;
            }
        }
    }
}
