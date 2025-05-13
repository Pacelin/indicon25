using System.Globalization;

namespace TSS.Utils
{
    public static partial class TSSUtils
    {
        private static readonly string[] _romanThousands = {"", "M", "MM", "MMM"};
        private static readonly string[] _romanHundreds = {"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"};
        private static readonly string[] _romanTens = {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"};
        private static readonly string[] _romanOnes = {"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"};
        
        private static readonly NumberFormatInfo NumberFormatSeparateSpaces = new();

        public static string ToStringWithGroupSpacing(this int value)
        {
            NumberFormatSeparateSpaces.NumberDecimalDigits = 0;
            NumberFormatSeparateSpaces.NumberGroupSeparator = " ";
            return value.ToString(NumberFormatSeparateSpaces);
        }
        
        public static string ToStringRoman(int arabic)
        {
            if (arabic >= 4000)
            {
                int thou = arabic / 1000;
                arabic %= 1000;
                return "(" + ToStringRoman(thou) + ")" + ToStringRoman(arabic);
            }

            string result = "";

            int num;
            num = arabic / 1000;
            result += _romanThousands[num];
            arabic %= 1000;

            num = arabic / 100;
            result += _romanHundreds[num];
            arabic %= 100;

            num = arabic / 10;
            result += _romanTens[num];
            arabic %= 10;

            result += _romanOnes[arabic];

            return result;
        }
    }
}
