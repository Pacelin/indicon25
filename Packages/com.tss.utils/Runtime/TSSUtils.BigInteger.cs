using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace TSS.Utils
{
    public static partial class TSSUtils
    {
        private static readonly string[] _letters = new[] { 
            "K", "m", "M", "b", "B", "t", "T", "q", "Q", "s", "S", "o", "O", "n", "N", "d", "D", 
            "uD", "dD", "tD", "TD", "qD", "QD", "sD", "SD", "oD", "OD", "nD", "ND", "V" };
        private static readonly BigInteger _maxBig = BigInteger.Parse(string.Concat(Enumerable.Repeat("999", 30)));

        public static int MaxBigIntDimensions = _letters.Length + 1;

        public static string GetBigIntLetter(int dimension) => dimension == 0 ? "" : _letters[dimension - 1];

        public static BigInteger ApplyExpo(this SerializedBigInteger serializedInteger, double power, int level) =>
            ((BigInteger)serializedInteger).ApplyExpo(power, level);
        public static BigInteger ApplyExpo(this BigInteger integer, double power, int level)
        {
            for (int i = 0; i < level; i++)
                integer = integer * (BigInteger)(Math.Exp(power) * 100000) / 100000;
            return integer;
        }

        public static bool ValidForPower(this BigInteger integer, double power)
        {
            var testInteger = integer * (BigInteger)(Math.Exp(power) * 100000) / 100000;
            return testInteger != integer;
        }
        public static int FindMaxExpoLevel(this BigInteger integer, int maxDimensions, double power)
        {
            int result = 0;
            int triesCount = 0;
            double minPower = 0;
            BigInteger testInteger = integer;
            BigInteger lastInteger = integer;
            while (lastInteger == testInteger)
            {
                minPower += power;
                testInteger *= (BigInteger)(Math.Exp(minPower) * 100000);
                testInteger /= 100000;
                triesCount++;
                if (triesCount > 100)
                    return int.MaxValue;
            }
            while (true)
            {
                integer *= (BigInteger)(Math.Exp(minPower) * 100000);
                integer /= 100000;
                if (integer > _maxBig)
                    return result;
                result += triesCount;
                if (result == int.MaxValue)
                    return int.MaxValue;
            }
        }
        
        public static string ToStringWithLetter(this SerializedBigInteger serializedInteger, int decimalDigits = 1,
            string numberFormat = null, string letterFormat = null) =>
            ((BigInteger)serializedInteger).ToStringWithLetter(decimalDigits);
        public static string ToStringWithLetter(this BigInteger integer, int decimalDigits = 1,
            string numberFormat = null, string letterFormat = null)
        {
            var str = integer.ToString();
            var dimension = (str.Length - 1) / 3;
            if (dimension == 0)
                return str;

            var builder = new StringBuilder();
            var dimensionSize = (str.Length - 1) % 3 + 1;
            for (int i = 0; i < dimensionSize; i++)
                builder.Append(str[i]);
            if (decimalDigits > 0)
            {
                bool dotAdded = false;
                var decimalBuilder = new StringBuilder();
                for (int i = 0; i < decimalDigits; i++)
                {
                    var ch = str[dimensionSize + i];
                    decimalBuilder.Append(str[dimensionSize + i]);
                    if (ch != '0')
                    {
                        if (!dotAdded)
                        {
                            dotAdded = true;
                            builder.Append('.');
                        }

                        builder.Append(decimalBuilder);
                        decimalBuilder.Clear();
                    }
                }
            }

            var number = numberFormat == null ? builder.ToString() : string.Format(numberFormat, builder);
            var letter = letterFormat == null ? _letters[dimension - 1] : string.Format(letterFormat, _letters[dimension - 1]);
            return number + letter;
        }

        public static int[] BrokeIntoParts(this BigInteger integer, int elementsCount)
        {
            elementsCount = Mathf.Min(elementsCount, MaxBigIntDimensions);
            var result = new List<int>();
            while (integer > 0)
            {
                result.Add((int) (integer % 1000));
                integer /= 1000;
            }
            
            while (result.Count < elementsCount)
                result.Add(0);

            result.Reverse();
            return result.ToArray();
        }
        public static string ConnectIntoBigInt(this int[] parts)
        {
            if (parts == null || parts.Length == 0)
                return "0";
            var builder = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
                builder.Append(parts[i].ToString("000"));
            return builder.ToString();
        }

        public static BigInteger ApplyExpo(this SerializedBigInteger serializedInteger, BigExpoLevelStep[] steps, int level) =>
            ((BigInteger)serializedInteger).ApplyExpo(steps, level);
        public static BigInteger ApplyExpo(this BigInteger integer, BigExpoLevelStep[] steps, int level)
        {
            int currentStep = 0;
            BigInteger testInteger = integer - 10;
            for (int i = 0; i <= level; i++)
            {
                if (currentStep < steps.Length - 1 &&
                    i >= steps[currentStep + 1].Level)
                    currentStep++;
                if (i > 0)
                    integer = integer * (BigInteger)(steps[currentStep].Increment * 100000) / 100000;
                if (testInteger == integer)
                    return integer;
                if (integer > _maxBig)
                    return testInteger;
                testInteger = integer;
            }
            return integer;
        }
        
        public static bool TryFindMaxLevel(this BigInteger integer, BigExpoLevelStep[] steps, out int maxLevel)
        {
            int currentStep = 0;
            maxLevel = 0;
            BigInteger testInteger = integer;
            while (true)
            {
                if (currentStep < steps.Length - 1 &&
                    maxLevel >= steps[currentStep + 1].Level)
                    currentStep++;
                double power = steps[currentStep].Increment;
                integer = integer * (BigInteger)(power * 100000) / 100000;
                if (testInteger == integer)
                    return false;
                if (integer > _maxBig)
                    return true;
                maxLevel++;
                testInteger = integer;
            }
        }
    }
}
