namespace MGC.Math
{
    /// <summary>
    /// Provides helper methods for determining the membership of numeric values
    /// in basic mathematical sets such as integers, natural numbers, and
    /// for checking numeric properties like parity.
    /// </summary>
    public static class NumberSets
    {
        /// <summary>
        /// Determines whether a floating-point value is an integer within the given tolerance.
        /// </summary>
        /// <param name="x">The value to test.</param>
        /// <param name="eps">
        /// The tolerance used when comparing the value to its rounded counterpart.
        /// This accounts for floating-point precision errors.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="x"/> is finite and differs from its rounded
        /// value by no more than <paramref name="eps"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(double x, double eps = 1e-9)
        {
            if (!double.IsFinite(x))
            {
                return false;
            }

            return System.Math.Abs(x - System.Math.Round(x)) <= eps;
        }
        /// <summary>
        /// Determines whether a floating-point value is a natural number.
        /// Natural numbers are positive integers; optionally zero is included.
        /// </summary>
        /// <param name="x">The value to test.</param>
        /// <param name="includeZero">
        /// If <c>true</c>, zero is considered a natural number.
        /// If <c>false</c>, the natural numbers start at one.
        /// </param>
        /// <param name="eps">
        /// The tolerance used for integer comparison. See <see cref="IsInteger(double, double)"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="x"/> is finite, integer (within tolerance),
        /// and greater than or equal to zero or one depending on <paramref name="includeZero"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNatural(double x, bool includeZero = true, double eps = 1e-9)
        {
            if (!double.IsFinite(x))
            {
                return false;
            }
            if (!IsInteger(x, eps))
            {
                return false;
            }
            if (includeZero)
            {
                return x >= 0;
            }
            else
            {
                return x >= 1;
            }
        }

        /// <summary>
        /// Determines whether an integer number is even.
        /// </summary>
        /// <param name="number">The integer value to test.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="number"/> is divisible by 2; otherwise <c>false</c>.
        /// </returns>
        public static bool IsEven(int number)
        {
            return System.Math.Abs(number) % 2 == 0;
        }
        /// <summary>
        /// Determines whether an integer number is odd.
        /// </summary>
        /// <param name="n">The integer value to test.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="n"/> is not divisible by 2; otherwise <c>false</c>.
        /// </returns>
        public static bool IsOdd(int n)
        {
            return System.Math.Abs(n) % 2 != 0;
        }

        public static bool IsBetween<T>(T value, T a, T b) where T : IComparable<T>
        {
            if (a.CompareTo(b) == 0)
            {
                return false;
            }
            T min = a;
            T max = b;

            if (a.CompareTo(b) > 0)
            {
                min = b;
                max = a;
            }
            if (value.CompareTo(min) > 0 && value.CompareTo(max) < 0)
            {
                return true;
            }

            return false;
        }

    }
}
