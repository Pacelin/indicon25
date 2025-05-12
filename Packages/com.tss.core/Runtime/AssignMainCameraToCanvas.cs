using UnityEngine;

namespace TSS.Core
{
    [DefaultExecutionOrder(-9999)]
    public class AssignMainCameraToCanvas : MonoBehaviour
    {
        private void Awake() => GetComponent<Canvas>().worldCamera = SceneCameraProvider.MainCamera;
    }
}