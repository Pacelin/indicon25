namespace TSS.Utils.Randoms.Int
{
    public class RandomIntInstance : RandomInstance<int, RandomIntConfig>
    {
        public RandomIntInstance(RandomIntConfig config, int seed) : base(config, seed) { }

        public override int Take() => Random.Next(Config.Min, Config.MaxExclusive);
        protected override void OnReset() { }
    }
}