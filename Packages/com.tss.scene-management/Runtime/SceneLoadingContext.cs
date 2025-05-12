using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace TSS.SceneManagement
{
    internal class SceneLoadingContext
    {
        public SceneEntryArgsInstaller ArgsInstaller { get; }
        public AssetReference SceneAssetReference { get; }
        public string SceneAssetAddress { get; }
        
        public ISceneLoadingProcessor Processor { get; set; } = new NullSceneLoadingProcessor();
        public List<ISceneLoadingInstruction> Instructions { get; } = new();
        public LoadSceneMode LoadMode { get; set; } = LoadSceneMode.Single;

        public SceneLoadingContext(AssetReference assetReference)
        {
            ArgsInstaller = new SceneEntryArgsInstaller();
            SceneAssetReference = assetReference;
            SceneAssetAddress = null;
        }
        
        public SceneLoadingContext(string sceneAssetAddress)
        {
            ArgsInstaller = new SceneEntryArgsInstaller();
            SceneAssetReference = null;
            SceneAssetAddress = sceneAssetAddress;
        }
    }
}