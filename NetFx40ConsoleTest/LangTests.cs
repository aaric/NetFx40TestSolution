using System;

namespace NetFx40ConsoleTest
{
    public static class LangTests
    {
        private static int ToInt(this float value)
        {
            return (int)value;
        }

        private static void SumToN(int m, ref int n)
        {
            n = m + n;
        }

        public static void Main1(string[] args)
        {
            float fVal = 1.5f;
            Console.WriteLine("ToInt -> " + fVal.ToInt());

            int m = 1, n = 2;
            SumToN(m, ref n);
            Console.WriteLine("SumToN -> " + n);
        }
    }
}