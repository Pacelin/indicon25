using Cysharp.Threading.Tasks;
using R3;
using TSS.Core.Extensions;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace TSS.Core.Localization
{
    public static class LocaleSwitcher
    {
        private static int _currentLocaleIndex;
        public static void NextLocale()
        {
            _currentLocaleIndex = MathUtils.IncrementRepeat(_currentLocaleIndex, LocalizationSettings.AvailableLocales.Locales.Count);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleIndex];
        }
        
        public static void PreviousLocale()
        {
            _currentLocaleIndex = MathUtils.DecrementRepeat(_currentLocaleIndex, LocalizationSettings.AvailableLocales.Locales.Count);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleIndex];
        }
    }
}
