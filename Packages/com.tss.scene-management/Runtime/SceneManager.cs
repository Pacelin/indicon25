using JetBrains.Annotations;
using UnityEngine.AddressableAssets;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public static class SceneManager
    {
        public static SceneLoadingBuilderInstructionsLoadModeProcessor Scene(AssetReference sceneReference)
        {
            return new SceneLoadingBuilderInstructionsLoadModeProcessor(
                new SceneLoadingContext(sceneReference));
        }
        public static SceneLoadingBuilderInstructionsLoadModeProcessor Scene(string sceneReference)
        {
            return new SceneLoadingBuilderInstructionsLoadModeProcessor(
                new SceneLoadingContext(sceneReference));
        }
    }
}