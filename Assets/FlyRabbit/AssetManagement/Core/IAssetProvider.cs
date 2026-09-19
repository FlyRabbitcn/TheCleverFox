using UnityEngine;
namespace FlyRabbit.AssetManagement
{
    public interface IAssetProvider
    {
        /// <summary>
        /// 加载资源。
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>加载的实体</returns>
        public T Load<T>(string path) where T : Object;
        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="asset">资源实体</param>
        /// <returns>是否成功释放</returns>
        public bool Release<T>(T asset) where T : Object;
    }
}
