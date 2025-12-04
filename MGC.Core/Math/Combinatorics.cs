namespace MGC.Math
{
    public static class Combinatorics
    {
        /// <summary>
        /// Computes a generalized factorial-like product using a specified decrement step.
        /// </summary>
        /// <remarks>
        /// This method performs the multiplication:
        /// <c>number * (number - step) * (number - 2 * step) * ...</c>
        /// until the value becomes less than or equal to zero.
        /// <para/>
        /// It is used as a shared internal implementation for both
        /// <see cref="Factorial(int)"/> (step = 1) and
        /// <see cref="DoubleFactorial(int)"/> (step = 2).
        /// </remarks>
        /// <param name="number">The starting number of the sequence.</param>
        /// <param name="step">
        /// The decrement step between sequence elements (usually 1 or 2).
        /// </param>
        /// <returns>
        /// The computed product. If <paramref name="number"/> is 1 or less,
        /// the result is <c>1</c>.
        /// </returns>
        private static long FactorialInternal(int number, int step)
        {
            long result = 1;
            do
            {
                result *= number;
                number -= step;
            }
            while (number > 0);
            return result;
        }

        /// <summary>
        /// Computes the factorial of a non-negative integer <c>n</c> using the definition:
        /// <c>n! = n × (n−1) × (n−2) × ... × 1</c>.
        /// </summary>
        /// <remarks>
        /// This method supports values only in the range <c>0 ≤ n ≤ 20</c>,
        /// because <c>21!</c> exceeds the maximum value representable by
        /// <see cref="long"/>.
        /// </remarks>
        /// <param name="number">The number whose factorial is computed.</param>
        /// <returns>
        /// The factorial value <c>n!</c>.  
        /// Returns <c>1</c> if <paramref name="number"/> is <c>0</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="number"/> is negative or greater than 20.
        /// </exception>
        public static long Factorial(int number)
        {
            if (number < 0 || number > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Value must be between 0 and 20.");
            }
            if (number == 0)
            {
                return 1;
            }
            return FactorialInternal(number, 1);
        }

        /// <summary>
        /// Computes the double factorial <c>n!!</c>, defined as:
        /// <para/>
        /// • For even n:  <c>n!! = n × (n−2) × (n−4) × ... × 2</c>  
        /// • For odd n:   <c>n!! = n × (n−2) × (n−4) × ... × 1</c>  
        /// <para/>
        /// With special cases:  
        /// <c>0!! = 1</c> and <c>1!! = 1</c>.
        /// </summary>
        /// <remarks>
        /// The valid range is <c>0 ≤ n ≤ 33</c>, because <c>34!!</c> exceeds the limit of
        /// <see cref="long"/>.  
        /// In contrast to ordinary factorial, double factorial grows slower, allowing a wider range.
        /// </remarks>
        /// <param name="number">The number whose double factorial is computed.</param>
        /// <returns>The value of <c>n!!</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="number"/> is negative or greater than 33.
        /// </exception>
        public static long DoubleFactorial(int number)
        {
            if (number < 0 || number > 33)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Value must be between 0 and 33.");
            }
            if (number == 0 || number == 1)
            {
                return 1;
            }
            return FactorialInternal(number, 2);
        }

        /// <summary>
        /// Computes the number of k-permutations of n distinct elements:
        /// ordered selections without repetition.
        /// </summary>
        /// <remarks>
        /// Defined only for values where <c>0 ≤ k ≤ n ≤ 20</c>.
        /// Uses the identity:
        /// <para/>
        /// <c>P(n, k) = n! / (n - k)!</c>.
        /// <para/>
        /// Special cases:
        /// <list type="bullet">
        ///   <item><description><c>P(n, 0) = 1</c></description></item>
        ///   <item><description><c>P(n, n) = n!</c></description></item>
        /// </list>
        /// </remarks>
        /// <param name="n">The size of the original set.</param>
        /// <param name="k">The number of ordered elements to choose.</param>
        /// <returns>The number of k-permutations of n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="n"/> is outside the range 0..20,
        /// or when <paramref name="k"/> is outside the range 0..n.
        /// </exception>
        public static long Permutation(int n, int k)
        {
            if (n < 0 || n > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Value must be between 0 and 20.");
            }
            if (k < 0 || k > n)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "Value of k must be between 0 and n (inclusive).");
            }
            if (k == 0)
            {
                return 1;
            }
            if (k == n)
            {
                return n.Factorial();
            }
            return n.Factorial() / (n - k).Factorial();
        }

        /// <summary>
        /// Computes the binomial coefficient C(n, k), also known as “n choose k”:
        /// the number of ways to choose k elements from n without regard to order.
        /// </summary>
        /// <remarks>
        /// Valid only for <c>0 ≤ k ≤ n ≤ 20</c>.
        /// Uses the identity:
        /// <para/>
        /// <c>C(n, k) = P(n, k) / k!</c>.
        /// <para/>
        /// Special cases:
        /// <list type="bullet">
        ///   <item><description><c>C(n, 0) = 1</c></description></item>
        ///   <item><description><c>C(n, n) = 1</c></description></item>
        /// </list>
        /// </remarks>
        /// <param name="n">The size of the original set.</param>
        /// <param name="k">The number of elements chosen from the set.</param>
        /// <returns>The binomial coefficient C(n, k).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="n"/> is outside the range 0..20,
        /// or when <paramref name="k"/> is outside the range 0..n.
        /// </exception>
        public static long Combination(int n, int k)
        {
            if (n < 0 || n > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Value must be between 0 and 20.");
            }
            if (k < 0 || k > n)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "Value of k must be between 0 and n (inclusive).");
            }
            if (k == 0 || k == n)
            {
                return 1;
            }
            return n.Permutation(k) / k.Factorial();
        }
    }
}
