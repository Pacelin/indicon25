using System;
using System.Collections.Generic;
using R3;
using TSS.Utils.Saving;

namespace TSS.Achievements
{
    internal class LocalAchievements
    {
        private const string KEY_PREFIX = "achievements_";

        public IEnumerable<string> ClaimedAchievements
        {
            get
            {
                foreach (var pair in _reports)
                    if (pair.Value >= _config[pair.Key].ReportsCount)
                        yield return pair.Key;
            }
        }

        public Observable<string> OnClaimAchievement => _achievementClaimSubject;
        
        private readonly AchievementsConfig _config;
        private readonly Dictionary<string, int> _reports;
        private readonly Subject<string> _achievementClaimSubject;
        
        public LocalAchievements(AchievementsConfig config)
        {
            _config = config;
            _reports = new Dictionary<string, int>(config.Count);
            _achievementClaimSubject = new Subject<string>();
        }

        public bool AchievementClaimed(string key) => _reports[key] >= _config[key].ReportsCount;
        
        public void Load()
        {
            _reports.Clear();
            foreach (var pair in _config)
                _reports.Add(pair.Key, SaveSystem.Load<int>(KEY_PREFIX + pair.Key, 0));
        }
        
        public void AddReport(string key)
        {
            if (AchievementClaimed(key))
                return;
            _reports[key]++;
            SaveSystem.Save(KEY_PREFIX + key, _reports[key]);
            if (AchievementClaimed(key))
                _achievementClaimSubject.OnNext(key);
        }

        public void Claim(string key)
        {
            if (AchievementClaimed(key))
                return;
            _reports[key] = _config[key].ReportsCount;
            SaveSystem.Save(KEY_PREFIX + key, _reports[key]);
        }

        public void ClearAchievements()
        {
            foreach (var pair in _reports)
                SaveSystem.Save(KEY_PREFIX + pair.Key, 0);
            Load();
        }
    }
}
