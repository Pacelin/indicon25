using System.Threading;
using JetBrains.Annotations;
using VContainer;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public interface ISceneLoadingInstruction
    {
        ELoadingTiming LoadingTiming { get; }
        string InstructionName { get; }
        float ProgressWeight { get; }

        void Execute(CancellationToken cancellationToken);
        float GetProgress();

        void InstallResult(IContainerBuilder builder);
    }
}