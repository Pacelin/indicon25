using System.Threading;
using Cysharp.Threading.Tasks;

namespace TSS.SceneManagement
{
    internal class NullSceneLoadingProcessor : ISceneLoadingProcessor
    {
        public UniTask BeforeLoadingStart(CancellationToken _) => UniTask.CompletedTask;
        public UniTask BeforeSceneActivation(CancellationToken _) => UniTask.CompletedTask;
        public UniTask AfterSceneActivation(CancellationToken _) => UniTask.CompletedTask;
        public void UpdateProgress(float progress) { }
        public void UpdateInstructionName(string instructionName) { }
    }
}