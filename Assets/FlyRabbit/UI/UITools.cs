using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace FlyRabbit.UITools
{
    public static class UITools
    {
        /// <summary>
        /// UI元素是否能交互
        /// </summary>
        public static bool CanInteract(Selectable selectable)
        {
            if (selectable.IsActive() == false || selectable.IsInteractable() == false)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 鼠标是否悬停在UI元素之上
        /// </summary>
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null || Pointer.current == null)
            {
                return false;
            }

            bool result;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Pointer.current.position.ReadValue();

            List<RaycastResult> raycastResult = ListPool<RaycastResult>.Get();
            raycastResult.Clear();

            EventSystem.current.RaycastAll(eventData, raycastResult);
            result = raycastResult.Count > 0;
            ListPool<RaycastResult>.Release(raycastResult);

            return result;
        }
    }
}
