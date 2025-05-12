using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TSS.Achievements.View
{
    public class AchievementsPanelItemView : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _icon;
        [SerializeField] private float _applyFillDuration = 1f;
        [Space]
        [SerializeField] private TMP_Text _captionText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _percentText;
        [Space]
        [SerializeField] private UnityEvent _onLock;
        [SerializeField] private UnityEvent _onUnlock;
        [SerializeField] private UnityEvent _onSecretLock;
        [SerializeField] private UnityEvent _onSecretUnlock;
        [SerializeField] private UnityEvent _onLoadFinishedSuccessful;
        [SerializeField] private UnityEvent _onLoadFinishedFailure;

        private AchievementConfig _config;
        
        private void OnDisable() => DOTween.Kill(this);

        public void SetInfo(AchievementConfig config)
        {
            _config = config;
        }

        public void SetLocked()
        {
            _icon.sprite = _config.LockedIcon;
            _captionText.text = _config.LockedCaption;
            _descriptionText.text = _config.LockedDescription;
            _icon.color = _config.LockedIconColor;
            _onLock.Invoke();
        }

        public void SetUnlocked()
        {
            _icon.sprite = _config.Icon;
            _captionText.text = _config.Caption;
            _descriptionText.text = _config.Description;
            _icon.color = _config.IconColor;
            _onUnlock.Invoke();
        }

        public void SetSecretLocked()
        {
            _icon.sprite = _config.LockedIcon;
            _captionText.text = _config.LockedCaption;
            _descriptionText.text = _config.LockedDescription;
            _icon.color = _config.LockedIconColor;
            _onSecretLock.Invoke();
        } 
        
        public void SetSecretUnlocked()
        {
            _icon.sprite = _config.Icon;
            _captionText.text = _config.Caption;
            _descriptionText.text = _config.Description;
            _icon.color = _config.IconColor;
            _onSecretUnlock.Invoke();
        }

        public void SetLoadStart() => _fillImage.fillAmount = 0;
        public void SetLoadFinishedFailure() => _onLoadFinishedFailure.Invoke();
        public void SetLoadFinishedSuccess(float ratio)
        {
            _onLoadFinishedSuccessful.Invoke();
            _percentText.text = (ratio * 100).ToString("0.0") + "%";
            _fillImage.DOFillAmount(ratio, _applyFillDuration)
                .SetTarget(this)
                .Play();
        }
    }
}