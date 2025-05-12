using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace TSS.Core
{
    [UsedImplicitly]
    internal class RuntimeEntryPoint : IInitializable, IDisposable
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Run()
        {
            #if UNITY_EDITOR
            if (SceneManager.GetActiveScene().buildIndex != 0)
                return;
            #endif
            var runtimeGO = new GameObject("[TSS Runtime]");
            Object.DontDestroyOnLoad(runtimeGO);

            runtimeGO.SetActive(false);
            runtimeGO.AddComponent<RuntimeScope>();
            runtimeGO.AddComponent<RuntimeMonoHook>();
            runtimeGO.SetActive(true);
        }
        
        private readonly IEnumerable<IRuntimeLoader> _runtimeLoaders;

        [Inject]
        public RuntimeEntryPoint(IEnumerable<IRuntimeLoader> runtimeLoaders)
        {
            _runtimeLoaders = runtimeLoaders
                .Select(loader => (loader, loader.GetType().GetCustomAttribute<RuntimeOrderAttribute>()))
                .Where(pair => pair.Item2 != null)
                .OrderBy(pair => pair.Item2.Order)
                .Select(pair => pair.loader);
        }

        public void Initialize()
        {
            UniTask.Void(async () =>
            {
                try
                {
                    Runtime.InitializeInternal();
                    foreach (var runtimeLoader in _runtimeLoaders)
                    {
                        if (Runtime.CancellationToken.IsCancellationRequested)
                            break;
                        await runtimeLoader.Initialize(Runtime.CancellationToken);
                        RuntimeLogger.LogInitialized(runtimeLoader);
                    }
                    RuntimeLogger.LogInitialized();
                    var settings = await Addressables.LoadAssetAsync<RuntimeSettings>("Runtime Settings");
                    await Addressables.LoadSceneAsync(settings.InitialSceneReference);
                    Addressables.Release(settings);
                }
                catch
                {
                    // ignored
                }
            });
        }
        
        public void Dispose()
        {
            Runtime.DisposeInternal();
            foreach (var runtimeLoader in _runtimeLoaders.Reverse())
            {
                runtimeLoader.Dispose();
                RuntimeLogger.LogDisposed(runtimeLoader);
            }

            RuntimeLogger.LogDisposed();
        }
    }
}