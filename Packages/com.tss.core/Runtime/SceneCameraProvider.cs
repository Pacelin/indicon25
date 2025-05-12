using UnityEngine;

namespace TSS.Core
{
    [DefaultExecutionOrder(-10000)]
    public class SceneCameraProvider : MonoBehaviour
    {
        public static Camera MainCamera => _sceneCamera;
        private static Camera _sceneCamera;

        private void Awake() => _sceneCamera = GetComponent<Camera>();
    }
}