using JetBrains.Annotations;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public enum ELoadingTiming
    {
        BeforeSceneLoadingStart,
        AfterSceneLoadingStart,
        BeforeSceneActivation
    }
}