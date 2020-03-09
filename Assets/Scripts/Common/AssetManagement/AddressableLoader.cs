using Assets.Scripts.Common.Pooling;
using Assets.Scripts.Common.Pooling.Poolers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using AssetLoader = System.Action<string, UnityEngine.Object>;

namespace Assets.Scripts.Common.AssetManagement
{
    public class AddressableLoader : MonoBehaviour
    {
        [SerializeField] private List<AddressableAssetGroup> assetGroups;

        private readonly HashSet<GameObject> Collection = new HashSet<GameObject>();

        private readonly Dictionary<string, UnityEngine.Object> loadedAssets =
            new Dictionary<string, UnityEngine.Object>();

        private readonly Dictionary<string, List<GameObject>> loadedObjects =
            new Dictionary<string, List<GameObject>>();

        public void GetAssets(GameObject obj, AssetRequest request)
        {
            var assetLoader = obj.GetComponent<IAssetLoader>();
            GetAssets(obj, assetLoader, request);
        }

        public void GetAssets(GameObject obj, IAssetLoader assetLoader, AssetRequest request)
        {
            var poolableKey = new AssetLoaderKey(obj, assetLoader, request.key);
            LoadAsset(request.path, poolableKey);
        }

        private void LoadAsset(string path, AssetLoaderKey assetKey)
        {
            UnityEngine.Object asset;
            if (loadedAssets.ContainsKey(path) == false)
            {
                asset = Resources.Load<Sprite>(path);
                loadedAssets[path] = asset;
                loadedObjects[path] = new List<GameObject>();
            }
            else
                asset = loadedAssets[path];

            if (asset != null)
            {
                AssetLoader loadAsset = assetKey.loader.LoadAsset;
                loadAsset.Invoke(assetKey.key, asset);

                if (Collection.Contains(assetKey.obj) == false)
                {
                    Collection.Add(assetKey.obj);
                    var notify = assetKey.obj.AddComponent<NotifyOnDestroy>();
                    loadedObjects[path].Add(assetKey.obj);
                    notify.Destroyed += Remove;
                    notify.Path = path;
                }
            }
        }

        private void Remove(string path, NotifyOnDestroy notify)
        {
            Collection.Remove(notify.gameObject);
            loadedObjects[path].Remove(notify.gameObject);
            if(loadedObjects[path].Count == 0)
            {
                Resources.UnloadAsset(loadedAssets[path]);
            }
        }

        private class AssetLoaderKey
        {
            public GameObject obj;
            public IAssetLoader loader;
            public string key;

            public AssetLoaderKey(GameObject obj, IAssetLoader loader, string key)
            {
                this.obj = obj;
                this.loader = loader;
                this.key = key;
            }
        }
    }
}

public class AssetRequest
{
    public string key;
    public string path;

    public AssetRequest(string key, string address)
    {
        this.key = key;
        this.path = address;
    }
}
