using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSS.Rest;

namespace TSS.Achievements
{
    internal class RemoteAchievements
    {
        [System.Serializable]
        private struct AchievementRatioResponse
        {
            public string achievement_id;
            public float percentage;
        }

        private readonly AchievementsConfig _config;
        
        public RemoteAchievements(AchievementsConfig config)
        {
            _config = config;
        }

        public async UniTask GetAchievementsRatio(Dictionary<string, float> dict)
        {
            var result = await RestAPI.Get<AchievementRatioResponse[]>(_config.RatioApi);
            foreach (var item in result)
                dict[item.achievement_id] = item.percentage;
        }

        public async UniTask GrantAchievement(string achievementId)
        {
            await RestAPI.Post(_config.GrantApi, new
            {
                user_id = RestAPI.GetUserIdentity(),
                achievement_id = achievementId
            });
        }

        public UniTask ClearAchievements() =>
            RestAPI.Post(_config.ClearApi, new { id = RestAPI.GetUserIdentity() });

        public async UniTask<string[]> GetClaimedAchievements()
        {
            return await RestAPI.Post<string[]>(_config.ListApi, new { id = RestAPI.GetUserIdentity() });
        }
    }
}