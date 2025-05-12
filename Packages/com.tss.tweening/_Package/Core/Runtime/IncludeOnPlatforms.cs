using System;
using System.Linq;
using UnityEngine;

namespace TSS.Core
{
    public class IncludeOnPlatforms : MonoBehaviour
    {
        private enum ExcludeBehaviour
        {
            [InspectorName("Destroy Game Object")]
            Destroy,
            [InspectorName("Disable Game Object")]
            Disable,
        }
        
        [SerializeField]
        private RuntimePlatform[] _includePlatforms;
        
        [SerializeField]
        private ExcludeBehaviour _excludeBehaviour = ExcludeBehaviour.Disable;
        
        private void Awake()
        {
            if (_includePlatforms.Contains(Application.platform))
                return;

            switch (_excludeBehaviour)
            {
                case ExcludeBehaviour.Destroy:
                    Destroy(gameObject);
                    break;
                case ExcludeBehaviour.Disable:
                    gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}