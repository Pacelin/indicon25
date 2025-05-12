using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace TSS.Utils.Randoms
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max) =>
            min + (max - min) * (float) random.NextDouble();

        public static int GetAccumlatedItemIndex(this IList<float> items, float accumulatedValue)
        {
            for (int i = 0; i < items.Count; i++)
            {
                accumulatedValue -= items[i];
                if (accumulatedValue <= 0)
                    return i;
            }

            return items.Count - 1;
        }

        public static T GetAccumulatedItem<T>(this IList<T> items, Func<T, float> weightFunc,
            float accumulatedValue)
        {
            foreach (var item in items)
            {
                accumulatedValue -= weightFunc(item);
                if (accumulatedValue <= 0)
                    return item;
            }

            return items.LastOrDefault();
        }
    }
}