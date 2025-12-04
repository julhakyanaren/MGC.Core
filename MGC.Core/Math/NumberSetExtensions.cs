namespace MGC.Math
{
    public static class NumberSetExtensions
    {

        public static bool IsInteger(this double x, double eps = 1e-9)
        {
            return NumberSets.IsInteger(x, eps);
        }
        public static bool IsNatural(this double x, bool includeZero = true, double eps = 1e-9)
        {
            return NumberSets.IsNatural(x, includeZero,  eps);
        }

        public static bool IsEven(this int number)
        {
            return NumberSets.IsEven(number);
        }
        public static bool IsOdd(this int n)
        {
            return NumberSets.IsOdd(n);
        }

        public static bool IsBetween<T>(this T value, T a, T b) where T : IComparable<T>
        {
            return NumberSets.IsBetween(value, a, b);
        }
    }
}
