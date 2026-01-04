namespace MGC.Mathematics
{
    /// <summary>
    /// Provides a collection of statistical utility methods for analyzing
    /// numeric datasets, including computation of minimum, maximum, median,
    /// population variance, sample variance, and corresponding standard
    /// deviations.
    /// </summary>
    /// <remarks>
    /// This static class contains overloads for arrays and lists and supports
    /// any numeric type that can be converted to <see cref="double"/>.
    ///
    /// All methods validate input sequences and throw descriptive exceptions
    /// when the data is invalid (e.g., null, empty, or insufficient elements
    /// for sample variance).
    ///
    /// The class is intended to serve as a foundational statistical component
    /// for mathematical calculations, data analysis tools, scientific
    /// computing, and game development within the MGC library.
    /// </remarks>

    public static class Statistics
    {
        private static void EnsureNotNullOrEmpty<T>(IReadOnlyCollection<T> values)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Sequence must not be null or empty.", nameof(values));
            }
        }

        /// <summary>
        /// Returns the minimum value in the array.
        /// </summary>
        public static double Min<T>(T[] values) => MinInternal(values);
        /// <summary>
        /// Returns the minimum value in the list.
        /// </summary>
        public static double Min<T>(List<T> values) => MinInternal(values);
        private static double MinInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);

            double min = Convert.ToDouble(values[0]);

            for (int i = 1; i < values.Count; i++)
            {
                double x = Convert.ToDouble(values[i]);
                if (x < min)
                {
                    min = x;
                }
            }
            return min;
        }

        /// <summary>
        /// Returns the maximum value in the array.
        /// </summary>
        public static double Max<T>(T[] values) => MaxInternal(values);
        /// <summary>
        /// Returns the maximum value in the list.
        /// </summary>
        public static double Max<T>(List<T> values) => MaxInternal(values);
        private static double MaxInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);

            double max = Convert.ToDouble(values[0]);

            for (int i = 1; i < values.Count; i++)
            {
                double x = Convert.ToDouble(values[i]);
                if (x > max)
                {
                    max = x;
                }
            }
            return max;
        }

        /// <summary>
        /// Calculates the median of the numeric values in the array.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Median of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Median<T>(T[] values) => MedianInternal(values);
        /// <summary>
        /// Calculates the median of the numeric values in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Median of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double Median<T>(List<T> values) => MedianInternal(values);
        private static double MedianInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            double[] data = new double[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                data[i] = Convert.ToDouble(values[i]);
            }
            Array.Sort(data);

            int n = data.Length;
            int mid = n / 2;

            if (n % 2 == 1)
            {
                return data[mid];
            }
            return (data[mid - 1] + data[mid]) / 2.0;
        }

        /// <summary>
        /// Calculates the population variance of the numeric values in the array.
        /// Uses the population formula: σ² = (1 / n) * Σ (xᵢ - mean)².
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Population variance of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double VariancePopulation<T>(T[] values) => VariancePopulationInternal(values);
        /// <summary>
        /// Calculates the population variance of the numeric values in the list.
        /// Uses the population formula: σ² = (1 / n) * Σ (xᵢ - mean)².
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Population variance of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null or contains no elements.
        /// </exception>
        public static double VariancePopulation<T>(List<T> values) => VariancePopulationInternal(values);
        private static double VariancePopulationInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);

            int n = values.Count;
            var data = new double[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = Convert.ToDouble(values[i]);
            }

            double mean = Averages.Arithmetic(data);

            double sumSq = 0;
            for (int i = 0; i < n; i++)
            {
                double d = data[i] - mean;
                sumSq += d * d;
            }

            return sumSq / n;
        }

        /// <summary>
        /// Calculates the sample variance of the numeric values in the array.
        /// Uses the sample formula: s² = (1 / (n - 1)) * Σ (xᵢ - mean)².
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// Array of numeric values.
        /// </param>
        /// <returns>
        /// Sample variance of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains only one element.
        /// </exception>
        public static double VarianceSample<T>(T[] values) => VarianceSampleInternal(values);
        /// <summary>
        /// Calculates the sample variance of the numeric values in the list.
        /// Uses the sample formula: s² = (1 / (n - 1)) * Σ (xᵢ - mean)².
        /// </summary>
        /// <typeparam name="T">
        /// Numeric type of the input values. Must be convertible to <see cref="double"/>.
        /// </typeparam>
        /// <param name="values">
        /// List of numeric values.
        /// </param>
        /// <returns>
        /// Sample variance of the input values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="values"/> is null, contains no elements,
        /// or contains only one element.
        /// </exception>
        public static double VarianceSample<T>(List<T> values) => VarianceSampleInternal(values);
        private static double VarianceSampleInternal<T>(IReadOnlyList<T> values)
        {
            EnsureNotNullOrEmpty(values);
            int n = values.Count;
            if (n < 2)
            {
                throw new ArgumentException("Sample variance requires at least two values.", nameof(values));
            }
            var data = new double[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = Convert.ToDouble(values[i]);
            }
            double mean = Averages.Arithmetic(data);

            double sumSq = 0;
            for (int i = 0; i < n; i++)
            {
                double d = data[i] - mean;
                sumSq += d * d;
            }
            return sumSq / (n - 1);
        }

        /// <summary>
        /// Calculates the population standard deviation of the numeric values in the array.
        /// This is the square root of the population variance.
        /// </summary>
        public static double StdDevPopulation<T>(T[] values) => Math.Sqrt(VariancePopulation(values));
        /// <summary>
        /// Calculates the population standard deviation of the numeric values in the list.
        /// This is the square root of the population variance.
        /// </summary>
        public static double StdDevPopulation<T>(List<T> values) => Math.Sqrt(VariancePopulation(values));

        /// <summary>
        /// Calculates the sample standard deviation of the numeric values in the array.
        /// This is the square root of the sample variance.
        /// </summary>
        public static double StdDevSample<T>(T[] values) => Math.Sqrt(VarianceSample(values));
        /// <summary>
        /// Calculates the sample standard deviation of the numeric values in the list.
        /// This is the square root of the sample variance.
        /// </summary>
        public static double StdDevSample<T>(List<T> values) => Math.Sqrt(VarianceSample(values));
    }
}