using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TSS.Core
{
    public class MonoScope : LifetimeScope
    {
        [SerializeField] private MonoInstaller[] _installers;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var installer in _installers)
                installer.Install(builder);
            Install(builder);
        }
        
        protected virtual void Install(IContainerBuilder builder) { }
    }
}