using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

public static class ItemPool
{
    private static readonly Dictionary<string, Queue<Item>> m_Pools = new Dictionary<string, Queue<Item>>();

    private static readonly string m_PrefabPath = "Assets/02.Prefabs/Items/{0}.prefab";
    private static readonly Transform m_Root;

    /// <summary>获取一个实例</summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static Item Get(string itemName)
    {
        //如果没有对应的池子，初始化一个
        if (m_Pools.ContainsKey(itemName) == false)
        {
            m_Pools.Add(itemName, new Queue<Item>());
        }

        //如果池子里空着，实例化一个
        if (m_Pools[itemName].Count == 0)
        {
            GameObject instance = Addressables.InstantiateAsync(GetPrefabPath(itemName)).WaitForCompletion();
            instance.name = itemName;
            Item item = instance.GetComponent<Item>();
            if (item == null)
            {
                Debug.LogError($"[Item Pool]：实例化的物体上未发现名为{itemName}的继承自Item的脚本");
                GameObject.Destroy(instance);
                return null;
            }
            m_Pools[itemName].Enqueue(item);
        }
        Item result = m_Pools[itemName].Dequeue();
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>释放一个实例</summary>
    public static void Release(Item item)
    {
        string key = item.GetType().Name;
        if (m_Pools.ContainsKey(key) == false)
        {
            Debug.LogError($"[Item Pool]：释放的物体{item.name}不在池子中，已销毁。");
            GameObject.Destroy(item.gameObject);
            return;
        }

        item.gameObject.SetActive(false);
        item.transform.SetParent(m_Root);
        m_Pools[key].Enqueue(item);
    }


    /// <summary>
    /// 获取Item对应的Prefab路径
    /// </summary>
    private static string GetPrefabPath(string itemName)
    {
        return string.Format(m_PrefabPath, itemName);
    }

    static ItemPool()
    {
        m_Root = new GameObject("Item Pool").transform;
        GameObject.DontDestroyOnLoad(m_Root.gameObject);
    }
}
