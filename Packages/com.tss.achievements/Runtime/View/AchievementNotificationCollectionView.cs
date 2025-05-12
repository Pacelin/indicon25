using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace TSS.Achievements.View
{
    public class AchievementNotificationCollectionView : MonoBehaviour
    {
        [SerializeField] private List<AchievementNotificationView> _notificationViews;

        private readonly Queue<string> _achievementsQueue = new();
        private IDisposable _disposable;
        private CancellationTokenSource _cts;
        
        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            _disposable = Achievements.ObserveAchievements()
                .Subscribe(RecieveNotification);
            Lifetime(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _achievementsQueue.Clear();
            _cts.Cancel();
            _cts.Dispose();
        }

        public IEnumerable<string> GetContainerKeys() => _notificationViews.Select(v => v.ViewId);
        
        private void RecieveNotification(string achievementKey) => _achievementsQueue.Enqueue(achievementKey);

        private async UniTaskVoid Lifetime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_achievementsQueue.Count > 0)
                {
                    var achievement = Achievements.GetAllAchievements()[_achievementsQueue.Dequeue()];
                    var container = _notificationViews.First(v => v.ViewId == achievement.NotificationContainer);
                    await container.Claim(achievement);
                }

                await UniTask.NextFrame(cancellationToken)
                    .SuppressCancellationThrow();
            }
        }
    }
}