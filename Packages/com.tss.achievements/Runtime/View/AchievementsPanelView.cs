using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TSS.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace TSS.Achievements.View
{
    public class AchievementsPanelView : MonoBehaviour
    {
        [SerializeField] private ScriptableTween _openTween;
        [SerializeField] private ScriptableTween _closeTween;
        [SerializeField] private RectTransform _achievementsContainer;
        [SerializeField] private AchievementsPanelItemView _achivementItemPrefab;
        [SerializeField] private UnityEvent _onLoadStart;
        [SerializeField] private UnityEvent _onLoadFinish;
        [SerializeField] private UnityEvent _onLoadFinishFailed;

        private Dictionary<string, AchievementsPanelItemView> _items = new();
        private CancellationTokenSource _cts;

        public void OpenPanel()
        {
            if (_closeTween.IsPlaying)
                _closeTween.Pause();
            _openTween.Play();
        }

        public void ClosePanel()
        {
            if (_openTween.IsPlaying)
                _openTween.Pause();
            _closeTween.Play();
        }
        
        private void Awake()
        {
            var allAchievements = Achievements.GetAllAchievements();
            foreach (var pair in allAchievements)
            {
                var view = Instantiate(_achivementItemPrefab, _achievementsContainer);
                _items.Add(pair.Key, view);
                view.SetInfo(pair.Value);
            }
        }

        private void OnEnable()
        {
            _cts = new();
            foreach (var pair in _items)
                pair.Value.SetLoadStart();
            var allAchievements = Achievements.GetAllAchievements();
            _onLoadStart.Invoke();
            Achievements.GetAchievementsRatio(result =>
            {
                _onLoadFinish.Invoke();
                var ordered = allAchievements.OrderByDescending(pair =>
                {
                    bool isUnlocked = Achievements.IsClaimed(pair.Key);
                    float ratio = result.ContainsKey(pair.Key) ? result[pair.Key] : 0;
                    ApplyState(isUnlocked, pair.Value.IsSecret, pair.Key);
                    _items[pair.Key].SetLoadFinishedSuccess(ratio);
 
                    if (isUnlocked)
                        return 2 + ratio;
                    return ratio;
                }).ToArray();
                for (int i = 0; i < ordered.Length; i++)
                    _items[ordered[i].Key].transform.SetSiblingIndex(i);
            }, () =>
            {
                _onLoadFinishFailed.Invoke();
                var ordered = allAchievements.OrderByDescending(pair =>
                {
                    bool isUnlocked = Achievements.IsClaimed(pair.Key);
                    bool isSecret = pair.Value.IsSecret;
                    ApplyState(isUnlocked, isSecret, pair.Key);
                    _items[pair.Key].SetLoadFinishedFailure();
 
                    if (isUnlocked)
                        return "3" + pair.Value.Caption;
                    return "1" + pair.Value.Caption;
                }).ToArray();
                for (int i = 0; i < ordered.Length; i++)
                    _items[ordered[i].Key].transform.SetSiblingIndex(i);
            }, _cts.Token);
        }

        private void OnDisable()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        private void ApplyState(bool unlocked, bool secret, string key)
        {
            if (unlocked)
            {
                if (secret)
                    _items[key].SetSecretUnlocked();
                else
                    _items[key].SetUnlocked();
            }
            else
            {
                if (secret)
                    _items[key].SetSecretLocked();
                else
                    _items[key].SetLocked();
            }
        }
    }
}