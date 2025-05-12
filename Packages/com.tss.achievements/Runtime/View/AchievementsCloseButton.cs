using UnityEngine;
using UnityEngine.EventSystems;

namespace TSS.Achievements.View
{
    public class AchievementsCloseButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData) => Achievements.HidePanel();
    }
}