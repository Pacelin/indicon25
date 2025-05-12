using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TSS.Achievements.View;
using UnityEngine;

namespace TSS.Achievements
{
    public static class Achievements
    {
        public static IEnumerable<string> CompletedAchievements => _local.ClaimedAchievements;
        public static IReadOnlyDictionary<string, AchievementConfig> AllAchievements => _config;

        private const float FETCH_COOLDOWN = 2f;
        
        private static AchievementsConfig _config;
        private static AchievementsPanelView _panelView;
        private static LocalAchievements _local;
        private static RemoteAchievements _remote;

        private static float _ratiousFetchExpiration;
        private static IDisposable _localDisposables;
        private static Dictionary<string, float> _ratious;

        private static UniTaskCompletionSource _ratioSource;

        internal static void Initialize(AchievementsConfig config)
        {
            _config = config;
            _ratious = new Dictionary<string, float>();
            _local = new LocalAchievements(_config);
            _local.Load();
            _remote = new RemoteAchievements(_config);
            _localDisposables = _local.OnClaimAchievement.Subscribe(OnClaimAchievementLocal);
            SyncAchievements().Forget();
        }

        internal static void SetPanel(AchievementsPanelView panelView) => _panelView = panelView;

        internal static void Dispose()
        {
            _localDisposables.Dispose();
        }

        public static IReadOnlyDictionary<string, AchievementConfig> GetAllAchievements() => _config;
        public static Observable<string> ObserveAchievements() => _local.OnClaimAchievement;
        
        public static void Report(string achievement) => _local.AddReport(achievement);
        public static bool IsClaimed(string achievement) => _local.AchievementClaimed(achievement);
        public static bool IsSecret(string achievement) => _config[achievement].IsSecret;
        public static void ShowPanel() => _panelView.OpenPanel();
        public static void HidePanel() => _panelView.ClosePanel();

        public static void ClearAchievements(Action onError = null)
        {
            _local.ClearAchievements();
            _remote.ClearAchievements().Forget(e =>
            {
                onError?.Invoke();
                Debug.LogException(e);
            });
        }

        public static void GetAchievementsRatio(Action<IReadOnlyDictionary<string, float>> onResult, Action onError, CancellationToken cancellationToken)
        {
            if (_ratioSource != null)
            {
                UniTask.Void(async () =>
                {
                    try
                    {
                        await _ratioSource.Task;
                        if (!cancellationToken.IsCancellationRequested)
                            onResult(_ratious);
                    }
                    catch
                    {
                        if (!cancellationToken.IsCancellationRequested)
                            onError();
                    }
                });
                return;
            }

            if (Time.time >= _ratiousFetchExpiration)
            {
                UniTask.Void(async () =>
                {
                    _ratioSource = new UniTaskCompletionSource();
                    _ratious.Clear();
                    try
                    {
                        await _remote.GetAchievementsRatio(_ratious);
                        if (!cancellationToken.IsCancellationRequested)
                            onResult(_ratious);
                        _ratiousFetchExpiration = Time.time + FETCH_COOLDOWN;
                        _ratioSource.TrySetResult();
                    }
                    catch (Exception e)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                            onError();
                        Debug.LogException(e);
                        _ratioSource.TrySetException(e);
                    }
                    _ratioSource = null;
                });
                return;    
            }

            if (!cancellationToken.IsCancellationRequested)
                onResult(_ratious);
        }
        
        private static async UniTask SyncAchievements()
        {
            try
            {
                var claimedRemote = await _remote.GetClaimedAchievements();
                foreach (var achievement in claimedRemote)
                    _local.Claim(achievement);
                var claimedLocal = _local.ClaimedAchievements.ToArray();
                var notClaimedRemote = claimedLocal.Except(claimedRemote);
                foreach (var achievement in notClaimedRemote)
                    await _remote.GrantAchievement(achievement);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private static void OnClaimAchievementLocal(string achievementId)
        {
            _remote.GrantAchievement(achievementId).Forget(Debug.LogException);
        }
    }
}