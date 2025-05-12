namespace TSS.Utils.Saving
{
    public interface ISavePrefs
    {
        void SetString(string key, string data);
        string GetString(string key, string defaultValue = "");
        void SetInt(string key, int data);
        int GetInt(string key, int defaultValue = 0);
        
        bool HasKey(string key);
        void DeleteKey(string key);
        void ClearData();
        void Save();
    }
}