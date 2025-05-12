using System;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using VContainer;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public class SceneLoadingBuilderInstructionsLoadMode : SceneLoadingBuilderBase
    {
        internal SceneLoadingBuilderInstructionsLoadMode(SceneLoadingContext context) : 
            base(context) { }
        
        public SceneLoadingBuilderInstructionsLoadMode AddInstruction(ISceneLoadingInstruction instruction)
        {
            Assert.IsNotNull(instruction);
            Context.Instructions.Add(instruction);
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadMode WithExtraInstalls(Action<IContainerBuilder> extraInstalls)
        {
            Assert.IsNotNull(extraInstalls);
            Context.ArgsInstaller.Add(extraInstalls);
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadMode Additive()
        {
            Context.LoadMode = LoadSceneMode.Additive;
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadMode Single()
        {
            Context.LoadMode = LoadSceneMode.Single;
            return this;
        }
    }
}