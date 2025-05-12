using DG.Tweening;
using TSS.Utils;
using UnityEngine;

namespace TSS.Tweening
{
    [System.Serializable]
    [ScriptableTweenPath("SpriteRenderer/Fade", 60)]
    public class ScriptableTweenSpriteRenderer_DOFade : ScriptableTweenDurableItemBase<SpriteRenderer>
    {
        [Box("Sprite Renderer")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Sprite Renderer")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IFloatValueProvider _startValue = new FloatValueProvider(0);
        [Box("Sprite Renderer")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _endValue = new FloatValueProvider(1);
        
        protected override Tween CreateTween(SpriteRenderer obj, float duration)
        {
            var tween = obj.DOFade(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("SpriteRenderer/Color", 61)]
    public class ScriptableTweenSpriteRenderer_DOColor : ScriptableTweenDurableItemBase<SpriteRenderer>
    {
        [Box("Sprite Renderer")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Sprite Renderer")]
        [ShowIf(nameof(_useStartValue))]
        [Order(11)]
        [SerializeReference] private IColorValueProvider _startValue = new ColorValueProvider();
        [Box("Sprite Renderer")]
        [Order(12)]
        [SerializeReference] private IColorValueProvider _endValue = new ColorValueProvider();
        
        protected override Tween CreateTween(SpriteRenderer obj, float duration)
        {
            var tween = obj.DOColor(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
}