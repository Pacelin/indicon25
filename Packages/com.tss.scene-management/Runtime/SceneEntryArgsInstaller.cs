using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace TSS.SceneManagement
{
    internal class SceneEntryArgsInstaller : IInstaller
    {
        private readonly List<Action<IContainerBuilder>> _extraInstalls = new();
        
        public void Add(Action<IContainerBuilder> extraInstalls) =>
            _extraInstalls.Add(extraInstalls);

        public void Install(IContainerBuilder builder)
        {
            foreach (Action<IContainerBuilder> extraInstall in _extraInstalls)
                extraInstall(builder);
        }
    }
}