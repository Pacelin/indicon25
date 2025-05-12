using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSS.Achievements.View;
using TSS.Utils;
using UnityEngine;
using UnityEngine.Scripting;

namespace TSS.Achievements
{
    [CreateSingletonAsset("Assets/_Project/Configs/SO_Achievements.asset", "Achievements Config")]
    public class AchievementsConfig : ScriptableObject, IReadOnlyDictionary<string, AchievementConfig>
    {
        public AchievementNotificationCollectionView ViewPrefab => _viewPrefab;
        public AchievementsPanelView PanelPrefab => _panelPrefab;
        public string GrantApi => _grantApi;
        public string ListApi => _listApi;
        public string RatioApi => _ratioApi;
        public string ClearApi => _clearApi;
        
        [SerializeField] private AchievementNotificationCollectionView _viewPrefab;
        [SerializeField] private AchievementsPanelView _panelPrefab;
        [SerializeField] private List<AchievementConfig> _collection;
        [Header("Remote")]
        [SerializeField] private string _grantApi = "grant-achievement";
        [SerializeField] private string _listApi = "user-achievements";
        [SerializeField] private string _ratioApi = "achievements-ratio";
        [SerializeField] private string _clearApi = "clear-achievements";

        private Dictionary<string, AchievementConfig> _dictionary;

        public int Count
        {
            get
            {
                SureDict();
                return _collection.Count;
            }
        }

        [Preserve]
        public IEnumerator<KeyValuePair<string, AchievementConfig>> GetEnumerator()
        {
            SureDict();
            return _dictionary.GetEnumerator();
        }

        [Preserve]
        IEnumerator IEnumerable.GetEnumerator()
        {
            SureDict();
            return _dictionary.GetEnumerator();
        } 

        public bool ContainsKey(string key)
        {
            SureDict();
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(string key, out AchievementConfig value)
        {
            SureDict();
            return _dictionary.TryGetValue(key, out value);
        }

        public AchievementConfig this[string key]
        {
            get
            {
                SureDict();
                return _dictionary[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                SureDict();
                return _dictionary.Keys;
            }
        }

        public IEnumerable<AchievementConfig> Values
        {
            get
            {
                SureDict();
                return _dictionary.Values;
            }    
        } 

        private void SureDict()
        {
            if (_dictionary == null)
                _dictionary = _collection.ToDictionary(i => i.Id);
        }
    }
}