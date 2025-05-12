using Cysharp.Threading.Tasks;
using TMPro;
using TSS.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TSS.Achievements.View
{
    public class AchievementNotificationView : MonoBehaviour
    {
        public string ViewId => _viewId;
    
        [SerializeField] private string _viewId;
        [SerializeField] private TMP_Text _captionText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _iconImage;
        [Space]
        [SerializeField] private ScriptableTween _claimTween;

        public UniTask Claim(AchievementConfig config)
        {
            _captionText.text = config.Caption;
            _descriptionText.text = config.Description;
            _iconImage.sprite = config.Icon;
            config.ClaimSound.PlayOneShot();
            _claimTween.Play();
            return _claimTween.WaitWhilePlay();
        }
    }
}