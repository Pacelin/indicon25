using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer.Unity;

namespace TSS.SceneManagement
{
    internal static class SceneLoadingService
    {
        private const float SCENE_LOADING_WEIGHT = 1;
        
        public static async UniTask Load(SceneLoadingContext context, CancellationToken cancellationToken)
        {
            var beforeLoadingStart = new List<ISceneLoadingInstruction>();
            var afterLoadingStart = new List<ISceneLoadingInstruction>();
            var beforeSceneActivation = new List<ISceneLoadingInstruction>();
            var progressCheckpoint = 0f;
            var maxProgress = context.Instructions.Sum(i => i.ProgressWeight) + SCENE_LOADING_WEIGHT;
            
            BuildInstructions(context.Instructions, beforeLoadingStart, afterLoadingStart, beforeSceneActivation);

            try
            {
                await context.Processor.BeforeLoadingStart(cancellationToken);

                progressCheckpoint = await LoadInstructions(context.Processor, context.ArgsInstaller,
                    progressCheckpoint, maxProgress, beforeLoadingStart, cancellationToken,
                    () => 0f);

                AsyncOperationHandle<SceneInstance> handle;
                if (context.SceneAssetReference != null)
                    handle = context.SceneAssetReference.LoadSceneAsync(context.LoadMode, false);
                else
                    handle = Addressables.LoadSceneAsync(context.SceneAssetAddress, context.LoadMode, false);
                
                progressCheckpoint = await LoadInstructions(context.Processor, context.ArgsInstaller,
                    progressCheckpoint, maxProgress, afterLoadingStart, cancellationToken,
                    () => SCENE_LOADING_WEIGHT * handle.PercentComplete);

                while (!handle.IsDone)
                {
                    context.Processor.UpdateProgress(
                        (progressCheckpoint + SCENE_LOADING_WEIGHT * handle.PercentComplete)
                        / maxProgress);
                    await UniTask.NextFrame(cancellationToken);
                }

                await LoadInstructions(context.Processor, context.ArgsInstaller,
                    progressCheckpoint, maxProgress, beforeSceneActivation, cancellationToken,
                    () => 1f);

                await context.Processor.BeforeSceneActivation(cancellationToken);
                using (LifetimeScope.Enqueue(context.ArgsInstaller))
                    await handle.Result.ActivateAsync().ToUniTask(cancellationToken: cancellationToken);
                await context.Processor.AfterSceneActivation(cancellationToken);
            }
            catch
            {
                // ignored
            }
        }

        private static void BuildInstructions(IEnumerable<ISceneLoadingInstruction> instructions,
            List<ISceneLoadingInstruction> beforeLoadingStart,
            List<ISceneLoadingInstruction> afterLoadingStart,
            List<ISceneLoadingInstruction> beforeSceneActivation)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.LoadingTiming == ELoadingTiming.BeforeSceneLoadingStart)
                    beforeLoadingStart.Add(instruction);
                else if (instruction.LoadingTiming == ELoadingTiming.AfterSceneLoadingStart)
                    afterLoadingStart.Add(instruction);
                else
                    beforeSceneActivation.Add(instruction);
            }
        }

        private static async UniTask<float> LoadInstructions(
            ISceneLoadingProcessor processor,
            SceneEntryArgsInstaller argsInstaller,
            float progressCheckpoint,
            float maxProgress,
            List<ISceneLoadingInstruction> instructions,
            CancellationToken cancellationToken, 
            Func<float> loadingSceneProgress)
        {
            if (instructions.Count <= 0)
                return progressCheckpoint;
            
            foreach (var instruction in instructions)
            {
                float instructionProgress;
                
                processor.UpdateInstructionName(instruction.InstructionName);
                instruction.Execute(cancellationToken);
                
                while ((instructionProgress = instruction.GetProgress()) < 1)
                {
                    processor.UpdateProgress(
                        (progressCheckpoint + loadingSceneProgress() + instructionProgress * instruction.ProgressWeight) 
                        / maxProgress);
                    await UniTask.NextFrame(cancellationToken);
                }

                progressCheckpoint += instruction.ProgressWeight;
                argsInstaller.Add(instruction.InstallResult);
                processor.UpdateProgress((progressCheckpoint + loadingSceneProgress()) / maxProgress);
            }

            return progressCheckpoint;
        }
    }
}