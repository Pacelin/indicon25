using TSS.Audio;
using UnityEngine;

namespace Siberian25.Audio
{
    public class SceneOST : MonoBehaviour
    {
        public SoundEvent.Instance Instance => _instance;
        
        [SerializeField] private SoundEvent _event;

        private SoundEvent.Instance _instance;
        private void OnEnable()
        {
            _instance = _event.CreateInstance();
            _instance.Start();
        }

        private void OnDisable()
        {
            _instance.Stop(true);
            _instance.Release();
            _instance = null;
        }
    }
}