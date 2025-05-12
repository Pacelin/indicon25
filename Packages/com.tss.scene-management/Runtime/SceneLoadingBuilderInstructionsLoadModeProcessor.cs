using System;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using VContainer;

namespace TSS.SceneManagement
{
    [PublicAPI]
    public class SceneLoadingBuilderInstructionsLoadModeProcessor : SceneLoadingBuilderBase
    {
        internal SceneLoadingBuilderInstructionsLoadModeProcessor(SceneLoadingContext context) : 
            base(context) { }
        
        public SceneLoadingBuilderInstructionsLoadMode WithProcessor(ISceneLoadingProcessor processor)
        {
            Assert.IsNotNull(processor);
            Context.Processor = processor;
            return new SceneLoadingBuilderInstructionsLoadMode(Context);
        }
        
        public SceneLoadingBuilderInstructionsLoadModeProcessor AddInstruction(ISceneLoadingInstruction instruction)
        {
            Assert.IsNotNull(instruction);
            Context.Instructions.Add(instruction);
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadModeProcessor WithExtraInstalls(Action<IContainerBuilder> extraInstalls)
        {
            Assert.IsNotNull(extraInstalls);
            Context.ArgsInstaller.Add(extraInstalls);
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadModeProcessor Additive()
        {
            Context.LoadMode = LoadSceneMode.Additive;
            return this;
        }

        public SceneLoadingBuilderInstructionsLoadModeProcessor Single()
        {
            Context.LoadMode = LoadSceneMode.Single;
            return this;
        }
    }
}