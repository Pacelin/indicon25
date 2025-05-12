using JetBrains.Annotations;
using UnityEngine;

namespace TSS.Tweening
{
    [PublicAPI]
    public abstract class ScriptableTweenEventHandler : MonoBehaviour
    {
        public abstract void OnTrigger();
    }
}