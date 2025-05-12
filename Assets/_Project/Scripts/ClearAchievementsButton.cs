using System;
using R3;
using TSS.Tweening.UI;
using TSS.Achievements;
using TSS.Utils.Saving;
using UnityEngine;

namespace Siberian25
{
    public class ClearAchievementsButton : MonoBehaviour
    {
        [SerializeField] private ScriptableButton _button;

        private IDisposable _disposable;
        
        private void OnEnable()
        {
            _disposable = _button.ObserveClick().Subscribe(_ =>
            {
                Achievements.ClearAchievements();
            });
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}
