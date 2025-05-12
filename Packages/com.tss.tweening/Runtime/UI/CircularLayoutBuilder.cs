using UnityEngine;

namespace TSS.Tweening.UI
{
    public class CircularLayoutBuilder : MonoBehaviour
    {
        [SerializeField] private float _circleRadius;
        
        [ContextMenu("Build Layout")]
        private void Build()
        {
            var vector = Vector2.up;
            var step = 360f / transform.childCount;
            for (int i = 0; i < transform.childCount; i++)
            {
                ((RectTransform)transform.GetChild(i).transform).anchoredPosition = vector * _circleRadius;
                vector = Quaternion.Euler(0, 0, step) * vector;
            }
        }
    }
}