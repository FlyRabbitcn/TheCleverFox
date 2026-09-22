using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
namespace FlyRabbit
{
    [Preserve]//此特性用于防止在打包的时候这个脚本没有被打包进程序
    public class SkipSplashImage
    {
        //此特性用于在启动画面显示之前执行这个方法
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Skip()
        {
#if UNITY_WEBGL
            Application.focusChanged += OnFocusChanged;
#else
            Task.Run(SkipCore);
#endif
        }
        private static void SkipCore()
        {
            SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
        }
#if UNITY_WEBGL
        private static void OnFocusChanged(bool hasFocus)
        {
            Application.focusChanged -= OnFocusChanged;
            SkipCore();
        }
#endif

    }
}
