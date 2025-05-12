using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TSS.Utils.Saving
{
    [PublicAPI]
    public static class SaveSystem
    {
        private static ISavePrefs _savePrefs;

        static SaveSystem()
        {
#if UNITY_EDITOR
            _savePrefs = new SavePrefsPlayerPrefs();
#elif UNITY_WEBGL
            _savePrefs = new SavePrefsWebGL();
#else
            _savePrefs = new SavePrefsPlayerPrefs();
#endif
        }

        public static void Save<T>(string key, T data) =>
            _savePrefs.SetString(key, JsonConvert.SerializeObject(data));

        public static void DeleteKey(string key) =>
            _savePrefs.DeleteKey(key);
        public static bool HasKey(string key) =>
            _savePrefs.HasKey(key);

        public static string LoadString(string key, string defaultValue = "") =>
            _savePrefs.GetString(key, defaultValue);
        public static void SaveString(string key, string value) =>
            _savePrefs.SetString(key, value);

        public static int LoadInt(string key, int defaultValue = 0) =>
            _savePrefs.GetInt(key, defaultValue);
        public static void SaveInt(string key, int value) =>
            _savePrefs.SetInt(key, value);        

        public static T Load<T>(string key, T defaultValue = default)
        {
            if (_savePrefs.HasKey(key))
                return JsonConvert.DeserializeObject<T>(_savePrefs.GetString(key));
            return defaultValue;
        }

        public static void ClearSaves() => _savePrefs.ClearData();
    }
}