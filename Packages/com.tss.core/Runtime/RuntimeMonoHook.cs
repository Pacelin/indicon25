using R3;
using UnityEngine;

namespace TSS.Core
{
    internal class RuntimeMonoHook : MonoBehaviour
    {
        internal static readonly ReactiveProperty<bool> _pauseState = new(false);
        internal static readonly ReactiveProperty<bool> _overridePauseState = new(false);
        internal static readonly ReactiveProperty<bool> _focusState = new(true);
        internal static readonly Subject<ApplicationQuitCancellationToken> _quitRequestSubject = new();
        internal static readonly Subject<Unit> _quitSubject = new();
        
        private static readonly ApplicationQuitCancellationToken _quitCancellationToken = new (() => _quitCancelled = true);
        private static bool _quitCancelled;
        
        private void OnEnable() =>
            Application.wantsToQuit += OnApplicationWantsToQuit;
        private void OnDisable() =>
            Application.wantsToQuit -= OnApplicationWantsToQuit;

        private bool OnApplicationWantsToQuit()
        {
            _quitCancelled = false;
            _quitRequestSubject.OnNext(_quitCancellationToken);
            return !_quitCancelled;
        }

        private void OnApplicationPause(bool pauseStatus) => _pauseState.Value = pauseStatus;
        private void OnApplicationFocus(bool hasFocus) => _focusState.Value = hasFocus;
        private void OnApplicationQuit() => _quitSubject.OnNext(Unit.Default);
    }
}