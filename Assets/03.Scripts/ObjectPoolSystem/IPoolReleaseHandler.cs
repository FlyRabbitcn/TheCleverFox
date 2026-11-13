public interface IPoolReleaseHandler 
{
    /// <summary>对象被回收到对象池时调用</summary>
    void OnPoolRelease();
}
