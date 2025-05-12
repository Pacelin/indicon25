using MessagePipe;
using R3;
using TSS.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TSS.Core
{
    internal class RuntimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c =>
            {
                var serviceProvider = c.AsServiceProvider();
                GlobalMessagePipe.SetProvider(serviceProvider);
                ObservableSystem.RegisterServiceProvider(serviceProvider);
                ObservableSystem.RegisterUnhandledExceptionHandler(Debug.LogException);
                ObservableSystem.DefaultFrameProvider = UnityFrameProvider.Update;
                ObservableSystem.DefaultTimeProvider = UnityTimeProvider.Update;
            });

            builder.RegisterEntryPoint<RuntimeEntryPoint>();
            var types = TSSUtils.Types()
                .NonAbstractClass()
                .DerivedFrom<IRuntimeLoader>();
            foreach (var type in types.Get())
                builder.Register(typeof(IRuntimeLoader), type, Lifetime.Singleton);
        }
    }
}