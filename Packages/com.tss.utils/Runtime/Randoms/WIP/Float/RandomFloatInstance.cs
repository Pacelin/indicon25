namespace TSS.Utils.Randoms.Float
{
    public class RandomFloatInstance : RandomInstance<float, RandomFloatConfig>
    {
        public RandomFloatInstance(RandomFloatConfig config, int seed) : base(config, seed) { }

        public override float Take() => Config.Min + (Config.Max - Config.Min) * (float) Random.NextDouble();

        protected override void OnReset()
        {
        }
    }
}