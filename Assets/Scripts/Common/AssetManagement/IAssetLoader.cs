using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.AssetManagement
{
    public interface IAssetLoader
    {
        List<AssetRequest> AssetRequests();

        void LoadAsset(string key, UnityEngine.Object asset);
    }

    public static class AssetLoaderExtension
    {
        public static void LoadAssets(this GameObject obj)
        {
            var assetLoader = obj.GetComponent<IAssetLoader>();
            var addresable = obj.GetComponentInParent<AddressableLoader>();
            foreach(var request in assetLoader.AssetRequests())
            {
                addresable.GetAssets(obj, assetLoader, request);
            }
        }
    }
}
