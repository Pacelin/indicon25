using TSS.Audio;
using UnityEngine;

namespace TSS.Achievements
{
    [CreateAssetMenu(menuName = "TSS/Achievement", fileName = "SO_Achievement")]
    public class AchievementConfig : ScriptableObject
    {
        public string Id => _id;
        
        public Sprite Icon => _icon;
        public Color IconColor => _iconColor;
        public string Caption => _caption;
        public string Description => _description;
        public int ReportsCount => _reportsCount;
        public bool IsSecret => _isSecret;

        public Sprite LockedIcon => _lockedIcon;
        public Color LockedIconColor => _lockedIconColor;
        public string LockedCaption => _lockedCaption;
        public string LockedDescription => _lockedDescription;
        
        public SoundEvent ClaimSound => _claimSound;
        public string NotificationContainer => _notificationContainer;

        [SerializeField] private string _id;
        [Header("Settings")] 
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _iconColor;
        [SerializeField] private string _caption = "Caption";
        [SerializeField] private string _description = "Description";
        [SerializeField] private int _reportsCount = 1;
        [SerializeField] private bool _isSecret;
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private Color _lockedIconColor;
        [SerializeField] private string _lockedCaption;
        [SerializeField] private string _lockedDescription;
        [Header("View")]
        [SerializeField] private SoundEvent _claimSound;
        [AchievementContainer]
        [SerializeField] private string _notificationContainer;
    }
}