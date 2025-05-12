using TSS.Tweening;
using UnityEngine;

namespace Siberian25.UI.ButtonHandlers
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private ScriptableTween _openTween;
        [SerializeField] private ScriptableTween _closeTween;

        private static SettingsMenu _instance;

        private void Awake() => _instance = this;

        public static void Open()
        {
            if (_instance._closeTween.IsPlaying)
                _instance._closeTween.Pause();
            _instance._openTween.Play();
        }

        public static void Close()
        {
            if (_instance._openTween.IsPlaying)
                _instance._openTween.Pause();
            _instance._closeTween.Play();
        }
    }
}