using UnityEngine;

namespace TSS.Core.UI
{
    [CreateAssetMenu(menuName = "TSS/UI/Cursor Data", fileName = "cursor_data")]
    public class SoftwareCursorData : ScriptableObject
    {
        [SerializeField]
        private Texture2D _cursor;
        
        [SerializeField]
        private Vector2 _hotspot;

        public Texture2D Cursor => _cursor;
        public Vector2 Hotspot => _hotspot;
    }
}