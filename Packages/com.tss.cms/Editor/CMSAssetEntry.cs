namespace TSS.ContentManagement.Editor
{
    [System.Serializable]
    internal struct CMSAssetEntry
    {
        public bool IsValid() => !string.IsNullOrEmpty(GUID);
        public bool IsValidName() => !string.IsNullOrWhiteSpace(CMSName);
            
        public string GUID;
        public string CMSName;
        public string ComponentName;
        public string Namespace;
    }
}