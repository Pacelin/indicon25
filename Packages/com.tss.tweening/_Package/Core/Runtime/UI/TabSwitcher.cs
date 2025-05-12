using UnityEngine;

namespace TSS.Core.UI
{
    public class TabSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tabs;

        public void SetActiveTab(GameObject newTab)
        {
            DisableTabs();
            newTab.SetActive(true);
        }

        public void DisableTabs()
        {
            foreach (GameObject tab in _tabs)
                tab.SetActive(false);
        }
    }
}
