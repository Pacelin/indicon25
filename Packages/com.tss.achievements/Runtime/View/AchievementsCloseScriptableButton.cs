using System;
using R3;
using TSS.Tweening.UI;
using UnityEngine;

namespace TSS.Achievements.View
{
    public class AchievementsCloseScriptableButton : MonoBehaviour
    {
        [SerializeField] private ScriptableButton _button;

        private IDisposable _disposable;
        
        private void OnEnable() =>
            _disposable = _button.ObserveClick().Subscribe(_ => Achievements.HidePanel());
        private void OnDisable() =>
            _disposable?.Dispose();
    }
}