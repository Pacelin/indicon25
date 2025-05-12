using VContainer;
using VContainer.Unity;

namespace Siberian25.Game
{
    public class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameManager>();
        }
    }
}