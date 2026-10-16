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

        /// <summary>
        /// 将int值限制在30~600之间
        /// </summary>
        public static bool ClampInt30To600(ref int value)
        {
            bool result = false;
            if (value < 30 || value > 600)
            {
                value = Mathf.Clamp(value, 30, 600);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 将int值限制在0~3之间
        /// </summary>
        public static bool ClampInt0To3(ref int value)
        {
            bool result = false;
            if (value < 0 || value > 3)
            {
                value = Mathf.Clamp(value, 0, 3);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 限制分辨率方案索引值在有效范围内（0~Screen.resolutions.Length-1）
        /// </summary>
        public static bool ClampResolutionSchemeIndex(ref int value)
        {
            int maxIndex = Screen.resolutions.Length - 1;
            bool result = false;
            if (value < 0 || value > maxIndex)
            {
                value = Mathf.Clamp(value, 0, maxIndex);
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