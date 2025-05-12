using TSS.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TSS.Core
{
    [CreateSingletonAsset("Assets/TSS/Runtime Settings.asset", "Runtime Settings")]
    internal class RuntimeSettings : ScriptableObject
    {
        public AssetReference InitialSceneReference => _initialSceneReference;
        
        [SerializeField] private AssetReference _initialSceneReference;
    }
}