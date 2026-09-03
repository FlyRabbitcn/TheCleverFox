using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetProvider 
{
    /// <summary>加载资源</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object;

    /// <summary>释放资源</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="asset"></param>
    public void Release<T>(T asset) where T : Object;

}
