using System.Linq;
using UnityEngine;

namespace TSS.Achievements
{
    public class AchievementsCondition : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activeWhenAchievedAll;
        [SerializeField] private GameObject[] _activeWhenNotAchievedAll;

        private void OnEnable()
        {
            var allAchieved = Achievements.CompletedAchievements.Count() == Achievements.AllAchievements.Count;

            foreach (var activate in _activeWhenAchievedAll)
                activate.SetActive(allAchieved);
            foreach (var deactivate in _activeWhenNotAchievedAll)
                deactivate.SetActive(!allAchieved);
        }
    }
}