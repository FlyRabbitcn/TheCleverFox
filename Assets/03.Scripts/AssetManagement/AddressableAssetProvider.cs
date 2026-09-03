using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableAssetProvider : IAssetProvider
{
    public T Load<T>(string path) where T : Object
    {
        IList<IResourceLocation> Locations =  Addressables.LoadResourceLocationsAsync(path, typeof(T)).WaitForCompletion();
        if(Locations == null || Locations.Count == 0)
        {
            return null;
        }

        return Addressables.LoadAssetAsync<T>(Locations[0]).WaitForCompletion();
    }

    public void Release<T>(T asset) where T : Object
    {
        Addressables.Release(asset);
    }
}
