using Newtonsoft.Json.Linq;
using System;
namespace FlyRabbit.SaveSystem
{
    public class SaveKey<T>
    {
        public string KeyName { get; }
        public T DefaultValue { get; }
        public Sanitizer<T> Sanitizer { get; }

        public SaveKey(string keyName, T defaultValue, Sanitizer<T> sanitizer = null)
        {
            KeyName = keyName;
            DefaultValue = defaultValue;
            Sanitizer = sanitizer;
        }
    }
}
