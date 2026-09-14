namespace FlyRabbit.SaveSystem
{
    /// <summary>“消毒器”，用于确保数据合法，如果数据不合法会被修正</summary>
    /// <returns>是否触发了消毒操作</returns>
    public delegate bool Sanitizer<T>(ref T value);
}
