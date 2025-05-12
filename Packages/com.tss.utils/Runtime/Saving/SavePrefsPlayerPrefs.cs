#if !UNITY_WEBGL || UNITY_EDITOR
using UnityEngine;

namespace TSS.Utils.Saving
{
    internal class SavePrefsPlayerPrefs : ISavePrefs
    {
        public void SetString(string key, string data) => PlayerPrefs.SetString(key, data);
        public string GetString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);
        public void SetInt(string key, int data) => PlayerPrefs.SetInt(key, data);
        public int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);

        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
        public void ClearData() => PlayerPrefs.DeleteAll();
        public void Save() => PlayerPrefs.Save();
    }
}
#endif