using UnityEngine;
using UnityEngine.EventSystems;

namespace Siberian25.UI.ButtonHandlers
{
    public class CloseSettingsButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        { 
            SettingsMenu.Close();
        }
    }
}