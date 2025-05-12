using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using mixpanel;
using TSS.Core;
using TSS.Utils.Saving;

namespace Siberian25
{
    [RuntimeOrder(ERuntimeOrder.SystemRegistration)]
    [UsedImplicitly]
    public class RuntimeMixpanel : IRuntimeLoader
    {
        private IDisposable _disposable;
        
        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            Mixpanel.SetPreferencesSource(new MixpanelPreferenceSources());
            Mixpanel.Init();
            await UniTask.WaitUntil(Mixpanel.IsInitialized, cancellationToken: cancellationToken);
            Mixpanel.Track("@session_start");
            _disposable = Runtime.SubscribeQuit(() =>
            {
                Mixpanel.Track("@session_end");
                Mixpanel.Flush();
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }

    public class MixpanelPreferenceSources : IPreferences
    {
        public void DeleteKey(string key) => SaveSystem.DeleteKey(key);
        public int GetInt(string key) => SaveSystem.LoadInt(key);
        public int GetInt(string key, int defaultValue) => SaveSystem.LoadInt(key, defaultValue);
        public string GetString(string key) => SaveSystem.LoadString(key, "");
        public string GetString(string key, string defaultValue) => SaveSystem.LoadString(key, defaultValue);
        public bool HasKey(string key) => SaveSystem.HasKey(key);
        public void SetInt(string key, int value) => SaveSystem.SaveInt(key, value);
        public void SetString(string key, string value) => SaveSystem.SaveString(key, value);
    }
}