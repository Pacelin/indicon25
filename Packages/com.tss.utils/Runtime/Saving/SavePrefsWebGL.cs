#if UNITY_WEBGL && !UNITY_EDITOR

using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TSS.Utils.Saving
{
    internal class SavePrefsWebGL : ISavePrefs
    {
        public void SetString(string key, string data) => SaveToLocalStorage(key, data);
        public string GetString(string key, string defaultValue = "")
        {
            var result = LoadFromLocalStorage(key);
            if (string.IsNullOrEmpty(result))
                return defaultValue;
            return result;
        }

        public void SetInt(string key, int data) => SaveToLocalStorage(key, data.ToString());
        public int GetInt(string key, int defaultValue = 0)
        {
            var str = GetString(key);
            if (int.TryParse(str, out var value))
                return value;
            return defaultValue;
        }

        public bool HasKey(string key)
        {
            var data = HasKeyInLocalStorage(key);
            return data == 1;
        }

        public void DeleteKey(string key) => RemoveFromLocalStorage(key);
        public void ClearData() { }
        public void Save() { }

        private static void SaveToLocalStorage(string key, string value)
        {
            var path = Application.persistentDataPath + $"/{key}.save";
            File.WriteAllText(path, value);
        }

        private static string LoadFromLocalStorage(string key)
        {
            var path = Application.persistentDataPath + $"/{key}.save";
            if (File.Exists(path))
                return File.ReadAllText(path);
            return "";
        }

        private static void RemoveFromLocalStorage(string key)
        {
            var path = Application.persistentDataPath + $"/{key}.save";
            if (File.Exists(path))
                File.Delete(path);
        }

        private static int HasKeyInLocalStorage(string key)
        {
            var path = Application.persistentDataPath + $"/{key}.save";
            if (File.Exists(path))
                return 1;
            return 0;
        }
    }
    /*internal class SavePrefsWebGL : ISavePrefs
    {
        private const string SAVE_PATH = "idbfs/some_unique_path/";
        
        public void SetString(string key, string data) => SaveToLocalStorage(PrefixKey(key), data);
        public string GetString(string key, string defaultValue = "")
        {
            var result = LoadFromLocalStorage(PrefixKey(key));
            if (string.IsNullOrEmpty(result))
                return defaultValue;
            return result;
        }

        public void SetInt(string key, int data) => SaveToLocalStorage(PrefixKey(key), data.ToString());
        public int GetInt(string key, int defaultValue = 0)
        {
            var str = GetString(key);
            if (int.TryParse(str, out var value))
                return value;
            return defaultValue;
        }

        public bool HasKey(string key)
        {
            var data = HasKeyInLocalStorage(PrefixKey(key));
            return data == 1;
        }

        public void DeleteKey(string key) => RemoveFromLocalStorage(PrefixKey(key));
        public void ClearData() { }
        public void Save() { }
        
        private static string PrefixKey(string key) => SAVE_PATH + key;
        
        [DllImport("__Internal")]
        private static extern void SaveToLocalStorage(string key, string value);
        [DllImport("__Internal")]
        private static extern string LoadFromLocalStorage(string key);
        [DllImport("__Internal")]
        private static extern void RemoveFromLocalStorage(string key);
        [DllImport("__Internal")]
        private static extern int HasKeyInLocalStorage(string key);
    }*/
}
#endif