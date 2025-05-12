using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public interface ISceneLoadingProcessor
    {
        UniTask BeforeLoadingStart(CancellationToken cancellationToken);
        UniTask BeforeSceneActivation(CancellationToken cancellationToken);
        UniTask AfterSceneActivation(CancellationToken cancellationToken);
        void UpdateProgress(float progress);
        void UpdateInstructionName(string instructionName);
    }
}