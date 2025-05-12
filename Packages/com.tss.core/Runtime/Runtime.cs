using System;
using System.Threading;
using JetBrains.Annotations;
using R3;

namespace TSS.Core
{
    [PublicAPI]
    public static class Runtime
    {
        public static bool IsPaused => RuntimeMonoHook._pauseState.CurrentValue || RuntimeMonoHook._overridePauseState.CurrentValue;
        public static bool IsFocused => RuntimeMonoHook._focusState.CurrentValue;
        public static CancellationToken CancellationToken => _cancellationDisposable.Token;

        private static CancellationDisposable _cancellationDisposable;

        internal static void InitializeInternal() => _cancellationDisposable = new CancellationDisposable();
        internal static void DisposeInternal() => _cancellationDisposable.Dispose();

        public static Observable<bool> ObservePause() => RuntimeMonoHook._pauseState.CombineLatest(RuntimeMonoHook._overridePauseState, (p1, p2) => p1 || p2);
        public static Observable<bool> ObserveFocus() => RuntimeMonoHook._focusState;

        public static void SetPause(bool pause) => RuntimeMonoHook._overridePauseState.Value = pause;
        public static IDisposable SubscribeQuitRequest(Observer<ApplicationQuitCancellationToken> observer) =>
            RuntimeMonoHook._quitRequestSubject.Subscribe(observer);
        public static IDisposable SubscribeQuit(Observer<Unit> observer) =>
            RuntimeMonoHook._quitSubject.Subscribe(observer);
        public static IDisposable SubscribeQuit(Action observer) =>
            RuntimeMonoHook._quitSubject.Subscribe(_ => observer());

        public static void ShutdownApplication()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.Debug.LogError("Application in WebGL cannot be shutdown!");
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }

        public static bool CanShutdownApplication()
        {
#if UNITY_WEBGL
            return false;
#else
            return true;
#endif
        }
    }
}