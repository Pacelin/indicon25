using System;
using Cysharp.Threading.Tasks;
using R3;
using TSS.Tweening.UI;
using TSS.ContentManagement;
using TSS.Core;
using TSS.SceneManagement;
using TSS.Tweening;
using UnityEngine;

namespace Siberian25.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private ScriptableButton _menuButton;
        [SerializeField] private ScriptableButton _resumeButton;
        [SerializeField] private ScriptableTween _openTween;
        [SerializeField] private ScriptableTween _closeTween;
        
        private static PauseMenu _instance;
        private IDisposable _disposable;
        
        private void Awake()
        {
            _instance = this;
            _instance.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (Runtime.IsPaused)
                Runtime.SetPause(false);
            _instance = null;
        }

        private void OnEnable()
        {
            _disposable = Disposable.Combine(
                _resumeButton.ObserveClick().Subscribe(_ => Close()),
                _menuButton.ObserveClick().Subscribe(_ => SceneManager.Scene(CMS.Scenes.MainMenu)
                    .Single().Load(Runtime.CancellationToken).Forget()));
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        public static void Open()
        {
            _instance.gameObject.SetActive(true);
            Runtime.SetPause(true);
            if (_instance._closeTween.IsPlaying)
                _instance._closeTween.Pause();
            _instance._openTween.Play();
        }

        public static void Close()
        {
            if (_instance._openTween.IsPlaying)
                _instance._openTween.Pause();
            _instance._closeTween.Play();
            _instance._closeTween.WaitWhilePlay().ContinueWith(() =>
            {
                _instance.gameObject.SetActive(false);
                Runtime.SetPause(false);
            }).Forget();
        }
    }
}