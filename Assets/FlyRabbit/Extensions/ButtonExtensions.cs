using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace FlyRabbit.ExtensionMethods
{
    public static class ButtonExtensions
    {
        /// <summary>
        /// 模拟鼠标左键点击按钮。
        /// </summary>
        public static void SimulateClick(this Button button)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left
            };

            ExecuteEvents.Execute<IPointerClickHandler>(button.gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
    }
}