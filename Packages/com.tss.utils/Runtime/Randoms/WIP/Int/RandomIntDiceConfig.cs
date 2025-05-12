using System.Collections.Generic;

namespace TSS.Utils.Randoms.Int
{
    [System.Serializable] 
    public class RandomIntDiceConfig : IRandomConfig<int>
    {
        public List<int> Dices;
        public int Offset;
        
        public IRandomInstance<int> NewInstance(int seed = 0) => new RandomIntDiceInstance(this, seed);
    }

    public class RandomIntDiceInstance : RandomInstance<int, RandomIntDiceConfig>
    {
        public RandomIntDiceInstance(RandomIntDiceConfig config, int seed) : base(config, seed) { }

        public override int Take()
        {
            var result = 0;
            foreach (var dice in Config.Dices)
                result += Random.Next(1, dice + 1);
            return result + Config.Offset;
        }

        protected override void OnReset() { }
    }
}