using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace FlyRabbit.AssetManagement
{
    public class AddressableAssetProvider : IAssetProvider
    {
        private readonly Dictionary<Object, int> m_ReferenceCount = new Dictionary<Object, int>();
        public T Load<T>(string path) where T : Object
        {
            IList<IResourceLocation> locations = Addressables.LoadResourceLocationsAsync(path, typeof(T)).WaitForCompletion();
            if (locations == null || locations.Count == 0)
            {
                return null;
            }

            T result = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();

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
            }
            Addressables.Release(asset);
            return true;
        }
    }
}