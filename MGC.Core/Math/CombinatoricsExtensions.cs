namespace MGC.Math
{
    public static class CombinatoricsExtensions
    {
        /// <summary>
        /// Computes the factorial of the integer value.
        /// </summary>
        /// <remarks>
        /// Equivalent to calling <see cref="Combinatorics.Factorial(int)"/>.
        /// Valid range: <c>0 ≤ number ≤ 20</c>.
        /// </remarks>
        /// <param name="number">The integer whose factorial is computed.</param>
        /// <returns>The factorial <c>number!</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="number"/> is outside the allowed range.
        /// </exception>
        public static long Factorial(this int number)
        {
            return Combinatorics.Factorial(number);
        }

        /// <summary>
        /// Computes the double factorial of the integer value.
        /// </summary>
        /// <remarks>
        /// Equivalent to calling <see cref="Combinatorics.DoubleFactorial(int)"/>.
        /// Valid range: <c>0 ≤ number ≤ 33</c>.
        /// </remarks>
        /// <param name="number">The integer whose double factorial is computed.</param>
        /// <returns>The value of <c>number!!</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="number"/> is outside the allowed range.
        /// </exception>
        public static long DoubleFactorial(this int number)
        {
            return Combinatorics.DoubleFactorial(number);
        }

        /// <summary>
        /// Computes the number of k-permutations of the integer value
        /// interpreted as <c>n</c> in the expression <c>P(n, k)</c>.
        /// </summary>
        /// <remarks>
        /// Equivalent to calling <see cref="Combinatorics.Permutation(int, int)"/>.
        /// Valid for values where <c>0 ≤ k ≤ n ≤ 20</c>.
        /// </remarks>
        /// <param name="n">The size of the original set.</param>
        /// <param name="k">The number of ordered elements to choose.</param>
        /// <returns>The number of k-permutations of n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="n"/> or <paramref name="k"/> is outside the allowed range.
        /// </exception>
        public static long Permutation(this int n, int k)
        {
            return Combinatorics.Permutation(n, k);
        }

        /// <summary>
        /// Computes the binomial coefficient C(n, k), also known as “n choose k”,
        /// where the integer value is interpreted as <c>n</c>.
        /// </summary>
        /// <remarks>
        /// Equivalent to calling <see cref="Combinatorics.Combination(int, int)"/>.
        /// Valid for values where <c>0 ≤ k ≤ n ≤ 20</c>.
        /// </remarks>
        /// <param name="n">The size of the original set.</param>
        /// <param name="k">The number of elements chosen from the set.</param>
        /// <returns>The binomial coefficient C(n, k).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="n"/> or <paramref name="k"/> is outside the allowed range.
        /// </exception>
        public static long Combination(this int n, int k)
        {
            return Combinatorics.Combination(n, k);
        }
    }
}
