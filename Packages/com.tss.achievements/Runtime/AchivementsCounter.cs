using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TSS.Achievements
{
    public class AchivementsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _format = "{0}/{1}";

        private void OnEnable()
        {
            _text.text = string.Format(_format, 
                Achievements.CompletedAchievements.Count(),
                Achievements.AllAchievements.Count);
        }
    }
}