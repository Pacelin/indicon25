using R3;
using UnityEngine.Localization;

namespace TSS.Utils
{
    public static class LocalizedExtensions
    {
        public static Observable<string> Observe(this LocalizedString localizedString)
        {
            return Observable.Create<string>(observer =>
            {
                observer.OnNext(localizedString.GetLocalizedString());
                
                void OnChange(string name) => observer.OnNext(name);
                localizedString.StringChanged += OnChange;
                return Disposable.Create(() => localizedString.StringChanged -= OnChange);
            });
        }
    }
}