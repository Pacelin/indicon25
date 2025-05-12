using DG.Tweening;
using TSS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TSS.Tweening
{
    [System.Serializable]
    [ScriptableTweenPath("Graphic/Fade", 40)]
    public class ScriptableTweenGraphic_DOFade : ScriptableTweenDurableItemBase<Graphic>
    {
        [Box("Graphic")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Graphic")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IFloatValueProvider _startValue = new FloatValueProvider(0);
        [Box("Graphic")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _endValue = new FloatValueProvider(1);
        
        protected override Tween CreateTween(Graphic obj, float duration)
        {
            var tween = obj.DOFade(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Graphic/Fade Canvas Group", 41)]
    public class ScriptableTweenCanvasGroup_DOFade : ScriptableTweenDurableItemBase<CanvasGroup>
    {
        [Box("Graphic")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Graphic")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IFloatValueProvider _startValue = new FloatValueProvider(0);
        [Box("Graphic")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _endValue = new FloatValueProvider(1);
        
        protected override Tween CreateTween(CanvasGroup obj, float duration)
        {
            var tween = obj.DOFade(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Graphic/Color", 42)]
    public class ScriptableTweenGraphic_DOColor : ScriptableTweenDurableItemBase<Graphic>
    {
        [Box("Graphic")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Graphic")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IColorValueProvider _startValue = new ColorValueProvider();
        [Box("Graphic")]
        [Order(12)]
        [SerializeReference] private IColorValueProvider _endValue = new ColorValueProvider();
        
        protected override Tween CreateTween(Graphic obj, float duration)
        {
            var tween = obj.DOColor(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Graphic/Tint", 43)]
    public class ScriptableTweenGraphic_DOTint : ScriptableTweenDurableItemBase<CanvasRenderer>
    {
        [Box("Graphic")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Graphic")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IColorValueProvider _startValue = new ColorValueProvider();
        [Box("Graphic")]
        [Order(12)]
        [SerializeReference] private IColorValueProvider _endValue = new ColorValueProvider();
        
        protected override Tween CreateTween(CanvasRenderer obj, float duration)
        {
            if (_useStartValue)
                return DOVirtual.Color(_startValue.Get(), _endValue.Get(), duration, obj.SetColor);
            return DOVirtual.Color(obj.GetColor(), _endValue.Get(), duration, obj.SetColor);
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Graphic/Fill Amount", 44)]
    public class ScriptableTweenGraphic_DOFillAmount : ScriptableTweenDurableItemBase<Image>
    {
        [Box("Graphic")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Graphic")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IFloatValueProvider _startValue = new FloatValueProvider(0);
        [Box("Graphic")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _endValue = new FloatValueProvider(1);
        
        protected override Tween CreateTween(Image obj, float duration)
        {
            if (_useStartValue)
                return obj.DOFillAmount(_endValue.Get(), duration).From(_startValue.Get());
            return obj.DOFillAmount(_endValue.Get(), duration);
        }
    }
}