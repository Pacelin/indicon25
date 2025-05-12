using mixpanel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Siberian25.UI.ButtonHandlers
{
    public class MixpanelButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string _event;
        
        public void OnPointerClick(PointerEventData eventData) => Mixpanel.Track(_event);
    }
}