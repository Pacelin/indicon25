using System.Collections.Generic;
using UnityEngine;

namespace TSS.Utils
{
    public static partial class TSSUtils
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int index = Random.Range(0, count + 1);
                (list[index], list[count]) = (list[count], list[index]);
            }

            return list;
        }

        public static T[] GetShuffled<T>(this IReadOnlyList<T> list)
        {
            var result = new T[list.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = list[i];
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int index = Random.Range(0, count + 1);
                (result[index], result[count]) = (result[count], result[index]);
            }

            return result;
        }
    }
}
