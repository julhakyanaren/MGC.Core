namespace MGC.Mathematics
{
    /// <summary>
    /// Provides methods for analyzing frequency-related characteristics
    /// of data sequences, including computation of mode, multiple modes,
    /// percentiles, and quantiles.
    /// </summary>
    /// <remarks>
    /// This static class contains utility functions for categorical and
    /// numerical data analysis. It includes:
    /// <list type="bullet">
    /// <item>
    /// <description><b>Mode</b> — the single most frequently occurring value.</description>
    /// </item>
    /// <item>
    /// <description><b>Modes</b> — all values that share the maximum frequency.</description>
    /// </item>
    /// <item>
    /// <description><b>Percentile</b> — interpolation-based percentile calculation
    /// compatible with Excel’s <c>PERCENTILE.INC</c>.</description>
    /// </item>
    /// <item>
    /// <description><b>Quantile</b> — a generalization of percentile expressed
    /// as a fraction in the range [0, 1].</description>
    /// </item>
    /// </list>
    ///
    /// All methods provide overloads for arrays and lists, and numeric operations
    /// rely on conversion to <see cref="double"/> whenever required.
    ///
    /// Input validation ensures that null or empty sequences, and out-of-range
    /// percentile or quantile values, generate meaningful exceptions.
    ///
    /// This class is part of the MGC statistical toolkit and is intended for
    /// data analysis, analytics modules, scientific computing, and
    /// game-development utilities.
    /// </remarks>
    public static class Frequency
    {
        private static void EnsureNotNullOrEmpty<T>(IReadOnlyCollection<T> values)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Sequence must not be null or empty.", nameof(values));
            }
        }

        /// <summary>
        /// Returns the mode (the most frequently occurring value) in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the input values.
        /// </typeparam>
        /// <param name="values">
        /// Array of values.
        /// </param>
        /// <returns>
        /// The value that appears most frequently in the sequence.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static T Mode<T>(T[] values) => ModeInternal(values);
        /// <summary>
        /// Returns the mode (the most frequently occurring value) in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the input values.
        /// </typeparam>
        /// <param name="values">
        /// List of values.
        /// </param>
        /// <returns>
        /// The value that appears most frequently in the sequence.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static T Mode<T>(List<T> values) => ModeInternal(values);
        private static T ModeInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);

            var frequency = new Dictionary<T, int>();
            T mode = default!;
            int maxCount = 0;
            bool hasValue = false;

            for (int i = 0; i < values.Count; i++)
            {
                T value = values[i];

                if (frequency.TryGetValue(value, out int count))
                {
                    count++;
                    frequency[value] = count;
                }
                else
                {
                    count = 1;
                    frequency[value] = 1;
                }

                if (!hasValue || count > maxCount)
                {
                    hasValue = true;
                    maxCount = count;
                    mode = value;
                }
            }

            return mode;
        }

        /// <summary>
        /// Returns all modes (all values with the highest frequency) in the array.
        /// If multiple values share the same maximum frequency, all of them are returned.
        /// </summary>
        /// <typeparam name="T">Type of the input values.</typeparam>
        /// <param name="values">Array of values.</param>
        /// <returns>
        /// An array containing all values that occur most frequently in the input sequence.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static T[] Modes<T>(T[] values) => ModesInternal(values);
        /// <summary>
        /// Returns all modes (all values with the highest frequency) in the list.
        /// If multiple values share the same maximum frequency, all of them are returned.
        /// </summary>
        /// <typeparam name="T">Type of the input values.</typeparam>
        /// <param name="values">List of values.</param>
        /// <returns>
        /// An array containing all values that occur most frequently in the input sequence.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static T[] Modes<T>(List<T> values) => ModesInternal(values);
        private static T[] ModesInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            Dictionary<T, int> frequency = new Dictionary<T, int>();
            int maxCount = 0;

            for (int i = 0; i < values.Count; i++)
            {
                T value = values[i];

                if (frequency.TryGetValue(value, out int count))
                {
                    count++;
                    frequency[value] = count;
                }
                else
                {
                    count = 1;
                    frequency[value] = 1;
                }

                if (count > maxCount)
                {
                    maxCount = count;
                }
            }
            HashSet<T> seen = new HashSet<T>();
            List<T> result = new List<T>();

            for (int i = 0; i < values.Count; i++)
            {
                T value = values[i];

                if (!seen.Contains(value) && frequency[value] == maxCount)
                {
                    seen.Add(value);
                    result.Add(value);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Calculates the p-th percentile of the numeric values in the array.
        /// Uses a linear interpolation method equivalent to Excel's PERCENTILE.INC.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <param name="percent">
        /// Percentile to compute, in the range from 0 to 100 (inclusive).
        /// For example, 50 returns the median, 90 returns the 90th percentile.
        /// </param>
        /// <returns>
        /// The p-th percentile of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="percent"/> is less than 0 or greater than 100.
        /// </exception>
        public static double Percentile<T>(T[] values, double percent) => PercentileInternal(values, percent);
        /// <summary>
        /// Calculates the p-th percentile of the numeric values in the list.
        /// Uses a linear interpolation method equivalent to Excel's PERCENTILE.INC.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <param name="percent">
        /// Percentile to compute, in the range from 0 to 100 (inclusive).
        /// For example, 50 returns the median, 90 returns the 90th percentile.
        /// </param>
        /// <returns>
        /// The p-th percentile of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="percent"/> is less than 0 or greater than 100.
        /// </exception>
        public static double Percentile<T>(List<T> values, double percent) => PercentileInternal(values, percent);
        private static double PercentileInternal<T>(IReadOnlyList<T> values, double percent)
        {
            EnsureNotNullOrEmpty(values);

            if (percent < 0.0 || percent > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(percent),"Percent must be in the range [0, 100].");
            }

            int n = values.Count;
            if (n == 1)
            {
                return Convert.ToDouble(values[0]);
            }
            double[] data = new double[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = Convert.ToDouble(values[i]);
            }

            Array.Sort(data);

            double prob = percent / 100.0;
            double index = prob * (n - 1);
            int lower = (int)Math.Floor(index);
            int upper = (int)Math.Ceiling(index);

            if (lower == upper)
            {
                return data[lower];
            }

            double fraction = index - lower;
            return data[lower] + (data[upper] - data[lower]) * fraction;
        }

        /// <summary>
        /// Calculates the q-th quantile of the numeric values in the array.
        /// Quantile is a value below which a given fraction (q) of the data falls.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <param name="q">
        /// Quantile position in the range [0.0, 1.0].
        /// For example, 0.5 returns the median, 0.9 returns the 90th percentile.
        /// </param>
        /// <returns>
        /// The q-th quantile of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="q"/> is outside the range [0.0, 1.0].
        /// </exception>
        public static double Quantile<T>(T[] values, double q) => QuantileInternal(values, q);
        /// <summary>
        /// Calculates the q-th quantile of the numeric values in the list.
        /// Quantile is a value below which a given fraction (q) of the data falls.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <param name="q">
        /// Quantile position in the range [0.0, 1.0].
        /// For example, 0.5 returns the median, 0.9 returns the 90th percentile.
        /// </param>
        /// <returns>
        /// The q-th quantile of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="q"/> is outside the range [0.0, 1.0].
        /// </exception>
        public static double Quantile<T>(List<T> values, double q) => QuantileInternal(values, q);
        private static double QuantileInternal<T>(IReadOnlyList<T> values, double q)
        {
            if (q < 0.0 || q > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(q), "Quantile must be in the range [0.0, 1.0].");
            }
            return PercentileInternal(values, q * 100f);
        }
    }
}