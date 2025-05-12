using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TSS.Utils;
using UnityEngine;

namespace TSS.Tweening
{
    [System.Serializable]
    [ScriptableTweenPath("Transform/Move")]
    public class ScriptableTweenTranform_DOMove : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Transform")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IVector3ValueProvider _startValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IVector3ValueProvider _endValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(14)]
        [SerializeField] private bool _local;

        protected override Tween CreateTween(Transform obj, float duration)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> tween;
            if (_local)
                tween = obj.DOLocalMove(_endValue.Get(), duration);
            else 
                tween = obj.DOMove(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());

            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Rotate", 1)]
    public class ScriptableTweenTranform_DORotate : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Transform")]
        [Order(11)]
        [ShowIf(nameof(_useStartValue))]
        [SerializeReference] private IVector3ValueProvider _startValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IVector3ValueProvider _endValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(13)]
        [SerializeField] private RotateMode _rotateMode = RotateMode.FastBeyond360;
        [Box("Transform")]
        [Order(14)]
        [SerializeField] private bool _local;

        protected override Tween CreateTween(Transform obj, float duration)
        {
            TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore;
            if (_local)
                tweenerCore = obj.DOLocalRotate(_endValue.Get(), duration, _rotateMode);
            else
                tweenerCore = obj.DORotate(_endValue.Get(), duration, _rotateMode);
            if (_useStartValue)
                tweenerCore.From(_startValue.Get());
            return tweenerCore;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Scale", 2)]
    public class ScriptableTweenTranform_DOScale : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Transform")]
        [ShowIf(nameof(_useStartValue))]
        [Order(11)]
        [SerializeReference] private IFloatValueProvider _startValue = new FloatValueProvider(0);
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _endValue = new FloatValueProvider(1);

        protected override Tween CreateTween(Transform obj, float duration)
        {
            var tween = obj.DOScale(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    [System.Serializable]
    [ScriptableTweenPath("Transform/Scale Vector", 3)]
    public class ScriptableTweenTranform_DOScaleVector : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeField] private bool _useStartValue;
        [Box("Transform")]
        [ShowIf(nameof(_useStartValue))]
        [Order(11)]
        [SerializeReference] private IVector3ValueProvider _startValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IVector3ValueProvider _endValue = new Vector3ValueProvider();

        protected override Tween CreateTween(Transform obj, float duration)
        {
            var tween = obj.DOScale(_endValue.Get(), duration);
            if (_useStartValue)
                tween.From(_startValue.Get());
            return tween;
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Jump", 4)]
    public class ScriptableTweenTranform_DOJump : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeReference] private IVector3ValueProvider _endValue = new Vector3ValueProvider();
        [Box("Transform")]
        [Order(11)]
        [SerializeReference] private IFloatValueProvider _jumpForce = new FloatValueProvider(1);
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IIntValueProvider _jumpsCount = new IntValueProvider(1);
        [Box("Transform")]
        [Order(13)]
        [SerializeField] private bool _local;

        protected override Tween CreateTween(Transform obj, float duration)
        {
            if (_local)
                return obj.DOLocalJump(_endValue.Get(), _jumpForce.Get(), _jumpsCount.Get(), duration);
            return obj.DOJump(_endValue.Get(), _jumpForce.Get(), _jumpsCount.Get(), duration);
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Shake Position", 5)]
    public class ScriptableTweenTranform_DOShakePosition : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeReference] private IVector3ValueProvider _strength = new Vector3ValueProvider(Vector3.one);
        [Box("Transform")]
        [Order(11)]
        [SerializeReference] private IIntValueProvider _vibrato = new IntValueProvider(10);
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _randomness = new FloatValueProvider(90);
        [Box("Transform")]
        [Order(13)]
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
        [Box("Transform")]
        [Order(14)]
        [SerializeField] private bool _fadeOut = true;
        
        protected override Tween CreateTween(Transform obj, float duration)
        {
            return obj.DOShakePosition(duration, _strength.Get(), _vibrato.Get(), _randomness.Get(),
                false, _fadeOut, _randomnessMode);
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Shake Rotation", 6)]
    public class ScriptableTweenTranform_DOShakeRotation : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeReference] private IVector3ValueProvider _strength = new Vector3ValueProvider(Vector3.one);
        [Box("Transform")]
        [Order(11)]
        [SerializeReference] private IIntValueProvider _vibrato = new IntValueProvider(10);
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _randomness = new FloatValueProvider(90);
        [Box("Transform")]
        [Order(13)]
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
        [Box("Transform")]
        [Order(14)]
        [SerializeField] private bool _fadeOut = true;
        
        protected override Tween CreateTween(Transform obj, float duration)
        {
            return obj.DOShakeRotation(duration, _strength.Get(), _vibrato.Get(), _randomness.Get(),
                _fadeOut, _randomnessMode);
        }
    }
    
    [System.Serializable]
    [ScriptableTweenPath("Transform/Shake Scale", 7)]
    public class ScriptableTweenTranform_DOShakeScale : ScriptableTweenDurableItemBase<Transform>
    {
        [Box("Transform")]
        [Order(10)]
        [SerializeReference] private IVector3ValueProvider _strength = new Vector3ValueProvider(Vector3.one);
        [Box("Transform")]
        [Order(11)]
        [SerializeReference] private IIntValueProvider _vibrato = new IntValueProvider(10);
        [Box("Transform")]
        [Order(12)]
        [SerializeReference] private IFloatValueProvider _randomness = new FloatValueProvider(90);
        [Box("Transform")]
        [Order(13)]
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
        [Box("Transform")]
        [Order(14)]
        [SerializeField] private bool _fadeOut = true;
        
        protected override Tween CreateTween(Transform obj, float duration)
        {
            return obj.DOShakeScale(duration, _strength.Get(), _vibrato.Get(), _randomness.Get(),
                _fadeOut, _randomnessMode);
        }
    }
}