namespace MGC.Math
{
    /// <summary>
    /// Provides a collection of statistical average calculation methods,
    /// including arithmetic, geometric, harmonic, quadratic (RMS),
    /// and weighted means.
    /// </summary>
    /// <remarks>
    /// This static class contains multiple overloads for working with arrays
    /// and lists of numeric values of any type that can be converted to
    /// <see cref="double"/>.  
    /// 
    /// All methods perform input validation and throw descriptive exceptions
    /// when encountering invalid data such as:
    /// <list type="bullet">
    /// <item>
    /// <description>null or empty collections</description>
    /// </item>
    /// <item>
    /// <description>non-positive values where mathematically prohibited 
    /// (e.g., geometric mean)</description>
    /// </item>
    /// <item>
    /// <description>zero weights in weighted mean calculations</description>
    /// </item>
    /// </list>
    /// 
    /// The class is designed to provide reliable, reusable statistical tools
    /// for mathematical, analytical, scientific and game-development scenarios.
    /// </remarks>
    public static class Averages
    {
        private static void EnsureNotNullOrEmpty<T>(IReadOnlyCollection<T> values)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Cannot compute mean for empty set.", nameof(values));
            }
        }

        /// <summary>
        /// Calculates the arithmetic mean (average) of the numeric values in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Arithmetic<T>(T[] values) => ArithmeticInternal(values);
        /// <summary>
        /// Calculates the arithmetic mean (average) of the numeric values in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Arithmetic<T>(List<T> values) => ArithmeticInternal(values);
        private static double ArithmeticInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            double sum = 0;
            for (int i = 0; i < values.Count; i++)
            {
                sum += Convert.ToDouble(values[i]);
            }
            return sum / values.Count;
        }

        /// <summary>
        /// Calculates the geometric mean of the numeric values in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Geometric mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains values that are less than or equal to zero.
        /// </exception>
        public static double Geometric<T>(T[] values) => GeometricInternal(values);
        /// <summary>
        /// Calculates the geometric mean of the numeric values in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Geometric mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains values that are less than or equal to zero.
        /// </exception>
        public static double Geometric<T>(List<T> values) => GeometricInternal(values);
        private static double GeometricInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            double logSum = 0;
            for (int i = 0; i < values.Count; i++)
            {
                double x = Convert.ToDouble(values[i]);

                if (x <= 0)
                {
                    throw new ArgumentException("All values must be > 0 for geometric mean.", nameof(values));
                }
                logSum += System.Math.Log(x);
            }
            return System.Math.Exp(logSum / values.Count);
        }

        /// <summary>
        /// Calculates the harmonic mean of the numeric values in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Harmonic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains values equal to zero.
        /// </exception>
        public static double Harmonic<T>(T[] values) => HarmonicInternal(values);
        /// <summary>
        /// Calculates the harmonic mean of the numeric values in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Harmonic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains values equal to zero.
        /// </exception>
        public static double Harmonic<T>(List<T> values) => HarmonicInternal(values);
        private static double HarmonicInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            double invSum = 0;
            for (int i = 0; i < values.Count; i++)
            {
                double x = Convert.ToDouble(values[i]);

                if (x == 0)
                {
                    throw new ArgumentException("Values must not be 0 for harmonic mean.", nameof(values));
                }
                invSum += 1.0 / x;
            }
            return values.Count / invSum;
        }

        /// <summary>
        /// Calculates the quadratic mean (root mean square, RMS) of the numeric values in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Quadratic mean (RMS) of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Quadratic<T>(T[] values) => QuadraticInternal(values);
        /// <summary>
        /// Calculates the quadratic mean (root mean square, RMS) of the numeric values in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Quadratic mean (RMS) of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Quadratic<T>(List<T> values) => QuadraticInternal(values);
        private static double QuadraticInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            double sumSquares = 0;
            for (int i = 0; i < values.Count; i++)
            {
                double x = Convert.ToDouble(values[i]);
                sumSquares += x * x;
            }
            return System.Math.Sqrt(sumSquares / values.Count);
        }

        /// <summary>
        /// Calculates the weighted arithmetic mean of the numeric values in the array
        /// using the corresponding weights.
        /// </summary>
        /// <typeparam name="TValue">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <typeparam name="TWeight">
        /// Numeric type of the weights. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <param name="weights">
        /// Array of weights corresponding to <paramref name="values"/>.
        /// </param>
        /// <returns>
        /// Weighted arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="values"/> or <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is empty, if the arrays have different lengths,
        /// or if the sum of all weights is equal to zero.
        /// </exception>
        public static double WeightedArithmetic<TValue, TWeight>(
            TValue[] values,
            TWeight[] weights) =>
            WeightedArithmeticInternal(values, weights);

        /// <summary>
        /// Calculates the weighted arithmetic mean of the numeric values in the list
        /// using the corresponding weights.
        /// </summary>
        /// <typeparam name="TValue">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <typeparam name="TWeight">
        /// Numeric type of the weights. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <param name="weights">
        /// List of weights corresponding to <paramref name="values"/>.
        /// </param>
        /// <returns>
        /// Weighted arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="values"/> or <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is empty, if the lists have different lengths,
        /// or if the sum of all weights is equal to zero.
        /// </exception>
        public static double WeightedArithmetic<TValue, TWeight>
            (List<TValue> values, List<TWeight> weights) => WeightedArithmeticInternal(values, weights);

        /// <summary>
        /// Calculates the weighted arithmetic mean of the numeric values in the list
        /// using the corresponding weights provided as an array.
        /// </summary>
        /// <typeparam name="TValue">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <typeparam name="TWeight">
        /// Numeric type of the weights. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <param name="weights">
        /// Array of weights corresponding to <paramref name="values"/>.
        /// </param>
        /// <returns>
        /// Weighted arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="values"/> or <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is empty, if the collection lengths differ,
        /// or if the sum of all weights is equal to zero.
        /// </exception>
        public static double WeightedArithmetic<TValue, TWeight>
            (List<TValue> values, TWeight[] weights) => WeightedArithmeticInternal(values, weights);

        /// <summary>
        /// Calculates the weighted arithmetic mean of the numeric values in the array
        /// using the corresponding weights provided as a list.
        /// </summary>
        /// <typeparam name="TValue">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <typeparam name="TWeight">
        /// Numeric type of the weights. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <param name="weights">
        /// List of weights corresponding to <paramref name="values"/>.
        /// </param>
        /// <returns>
        /// Weighted arithmetic mean of the input values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="values"/> or <paramref name="weights"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is empty, if the collection lengths differ,
        /// or if the sum of all weights is equal to zero.
        /// </exception>
        public static double WeightedArithmetic<TValue, TWeight>
            (TValue[] values, List<TWeight> weights) => WeightedArithmeticInternal(values, weights);

        private static double WeightedArithmeticInternal<TValue, TWeight>
            (IReadOnlyList<TValue> values, IReadOnlyList<TWeight> weights)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }
            if (values.Count == 0)
            {
                throw new ArgumentException("Cannot compute mean for empty set.", nameof(values));
            }
            if (values.Count != weights.Count)
            {
                throw new ArgumentException("Values and weights must have the same length.");
            }

            double weightedSum = 0;
            double weightSum = 0;

            for (int i = 0; i < values.Count; i++)
            {
                double v = Convert.ToDouble(values[i]);
                double w = Convert.ToDouble(weights[i]);

                weightedSum += v * w;
                weightSum += w;
            }

            if (weightSum == 0)
            {
                throw new ArgumentException("Sum of weights must not be 0.", nameof(weights));
            }
            return weightedSum / weightSum;
        }
    }
}
