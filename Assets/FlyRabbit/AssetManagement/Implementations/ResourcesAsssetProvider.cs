using System.Collections.Generic;
using UnityEngine;

namespace FlyRabbit.AssetManagement
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        private readonly Dictionary<Object, int> m_ReferenceCount = new Dictionary<Object, int>();
        public T Load<T>(string path) where T : Object
        {
            T result = Resources.Load<T>(path);

            if (result == null)
            {
                return null;
            }

            if (m_ReferenceCount.TryGetValue(result, out int count))
            {
                m_ReferenceCount[result] = count + 1;
            }
            else
            {
                m_ReferenceCount[result] = 1;
            }

            return result;
        }

        public bool Release<T>(T asset) where T : Object
        {
            if (m_ReferenceCount.ContainsKey(asset) == false)
            {
                return false;
            }
            m_ReferenceCount[asset]--;
            if (m_ReferenceCount[asset] <= 0)
            {
                m_ReferenceCount.Remove(asset);
                Resources.UnloadAsset(asset);
            }       
            return true;
        }
    }
}
