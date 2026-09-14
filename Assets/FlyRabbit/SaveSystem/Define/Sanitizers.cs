using UnityEngine;
namespace FlyRabbit.SaveSystem
{
    public static class Sanitizers
    {
        #region int值处理
        /// <summary>
        /// 将int值限制在0~正无穷之间
        /// </summary>
        public static bool ClampIntToNonNegative(ref int value)
        {
            bool result = false;
            if (value < 0)
            {
                value = 0;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 将int值限制在0~100之间
        /// </summary>
        public static bool ClampInt0To100(ref int value)
        {
            bool result = false;
            if (value < 0 || value > 100)
            {
                value = Mathf.Clamp(value, 0, 100);
                result = true;
            }
            return result;
        }
        #endregion

        #region float值处理

        /// <summary>
        /// 将float值限制在0~1之间
        /// </summary>
        public static bool ClampFloat0To1(ref float value)
        {
            bool result = false;
            if (value < 0f || value > 1f)
            {
                value = Mathf.Clamp01(value);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 将float值限制在0~正无穷之间
        /// </summary>
        public static bool ClampFloatToNonNegative(ref float value)
        {
            bool result = false;
            if (value < 0f)
            {
                value = 0f;
                result = true;
            }
            return result;
        }
        #endregion
    }
}