using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FlyRabbit.SaveSystem
{
    public static class SaveManager
    {
        private static JObject m_SaveData;
        private static JObject SaveData
        {
            get
            {
                CheckSaveData();
                return m_SaveData;
            }
        }
        /// <summary>存储所有键的值变更回调</summary>
        private static readonly Dictionary<string, Delegate> m_Listeners = new Dictionary<string, Delegate>();

        /// <summary>存档文件的本地文件存储路径</summary>
        public static readonly string SaveFilePath;

        /// <summary>存档文件的本地存储目录</summary>
        public static readonly string SaveDirectoryPath;

        #region Public Methods
        /// <summary>
        /// 获取值
        /// </summary>
        public static T GetValue<T>(SaveKey<T> saveKey)
        {
            string key = saveKey.KeyName;
            T result;
            if (SaveData.ContainsKey(key) == false)
            {
                result = saveKey.DefaultValue;
                Sanitize<T>(saveKey, ref result);
            }
            else
            {
                result = SaveData[key].ToObject<T>();
            }
            return result;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        public static void SetValue<T>(SaveKey<T> saveKey, T value, bool autoSave = true)
        {
            //消毒
            Sanitize(saveKey, ref value);
            // 写入新值
            SaveData[saveKey.KeyName] = JToken.FromObject(value);
            // 自动保存
            if (autoSave)
            {
                SaveToFile();
            }
            //通知监听者值已变更
            NotifyValueChanged(saveKey, value);

        }
        /// <summary>
        /// 注册值变更监听，在注册后会立刻执行一次回调，传入当前值，以确保监听者能够及时获取到最新的存档数据状态
        /// </summary>
        public static void Register<T>(SaveKey<T> saveKey, Action<T> callback)
        {
            // 获取键名
            string key = saveKey.KeyName;
            // 合并委托并存入字典
            if (m_Listeners.TryGetValue(key, out Delegate existing))
            {
                m_Listeners[key] = Delegate.Combine(existing, callback);
            }
            else
            {
                m_Listeners[key] = callback;
            }
            // 注册后立即调用一次回调，传入当前值
            callback?.Invoke(GetValue(saveKey));
        }
        /// <summary>
        /// 取消值变更监听
        /// </summary>
        public static void Unregister<T>(SaveKey<T> saveKey, Action<T> callback)
        {
            // 获取键名
            string key = saveKey.KeyName;
            // 从字典中移除委托
            if (m_Listeners.TryGetValue(key, out Delegate existing))
            {
                Delegate updated = Delegate.Remove(existing, callback);
                if (updated == null)
                {
                    m_Listeners.Remove(key);
                }
                else
                {
                    m_Listeners[key] = updated;
                }
            }
        }

        /// <summary>
        /// 设置为默认值
        /// </summary>
        public static void SetToDefault<T>(SaveKey<T> saveKey, bool autoSave = true)
        {
            SetValue(saveKey, saveKey.DefaultValue, autoSave);
        }

        /// <summary>
        /// 清空数据并保存到文件，清空数据会删除内存中的所有存档数据，并将一个新的空的存档文件写入磁盘。
        /// 这相当于重置了存档系统，所有之前保存的数据都会被丢弃，存档文件将被重置为初始状态。
        /// 请谨慎使用此方法，因为它会导致数据丢失。
        /// </summary>
        public static void Clear()
        {
            SaveData.RemoveAll();
            SaveToFile();
        }

        /// <summary>
        /// 将当前内存中的数据保存到本地文件内
        /// </summary>
        public static void SaveToFile()
        {
            File.WriteAllText(SaveFilePath, SaveData.ToString(Formatting.Indented));
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 触发指定键的值变更通知
        /// </summary>
        private static void NotifyValueChanged<T>(SaveKey<T> saveKey, T newValue)
        {
            // 检查是否有注册的监听
            if (m_Listeners.TryGetValue(saveKey.KeyName, out Delegate existing))
            {
                // 转换为强类型委托并调用
                (existing as Action<T>)?.Invoke(newValue);
            }
        }

        /// <summary>
        /// 检查存储数据，确保数据已加载，如果没有则创建一个新的存档文件并加载
        /// </summary>
        private static void CheckSaveData()
        {
            if (m_SaveData != null)
            {
                return;
            }
            try
            {
                m_SaveData = JObject.Parse(File.ReadAllText(SaveFilePath));
            }
            catch
            {
                m_SaveData = new JObject();
                SaveToFile();
            }
        }

        /// <summary>消毒</summary>
        private static void Sanitize<T>(SaveKey<T> saveKey, ref T value)
        {
            if (saveKey.Sanitizer != null)
            {
                T input = value;
                bool sanitized = saveKey.Sanitizer.Invoke(ref value);
                if (sanitized)
                {
                    Debug.LogWarning($"[FlyRabbit.SaveSystem] Key: {saveKey.KeyName} 被修正， 从 {input} 修正为 {value}");
                }
            }
        }
        #endregion

        static SaveManager()
        {
            SaveDirectoryPath = Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "FlyRabbit", "SaveSystem")).FullName;
            SaveFilePath = Path.Combine(SaveDirectoryPath, "Save.json");
        }

    }
}
