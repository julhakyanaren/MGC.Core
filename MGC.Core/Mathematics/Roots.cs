namespace MGC.Mathematics
{
    using Physics.Thermodynamics;
    public static class Roots
    {
        /// <summary>
        /// Computes the n-th root of a real number with validation for real-valued solutions.
        /// </summary>
        /// <param name="x">The radicand (the value under the root sign).</param>
        /// <param name="n">The degree of the root.</param>
        /// <returns>
        /// The real <paramref name="n"/>-th root of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="n"/> is zero or if a real solution does not exist
        /// (for example, even root of a negative value).
        /// </exception>
        public static double SafeRoot(double x, double n)
        {
            if (n == 0.0)
            {
                throw new ArgumentException("The root degree cannot be zero.", nameof(n));
            }
            if (x < 0)
            {
                if (!NumberSets.IsInteger(n))
                {
                    throw new ArgumentException("For negative radicand, the root degree must be an integer.", nameof(n));
                }

                int k = (int)Math.Round(n);
                if (NumberSets.IsEven(k))
                {
                    throw new ArgumentException("There is no real solution for even-degree roots of negative numbers.", nameof(x));
                }
                double absRoot = Math.Pow(-x, 1.0 / k);
                return -absRoot;
            }
            return Math.Pow(x, 1.0 / n);
        }

        /// <summary>
        /// Computes the <paramref name="n"/>-th root of a real number.
        /// </summary>
        /// <param name="x">
        /// The radicand (the value under the root sign).
        /// </param>
        /// <param name="n">
        /// The degree of the root.
        /// Must not be zero.
        /// Even degrees for negative <paramref name="x"/> produce <see cref="double.NaN"/>.
        /// </param>
        /// <returns>
        /// The real <paramref name="n"/>-th root of <paramref name="x"/> when it exists.
        /// If <paramref name="x"/> is negative and <paramref name="n"/> is even,
        /// the method returns <see cref="double.NaN"/> because there is no real solution.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="n"/> is zero, since a root of degree zero is undefined.
        /// </exception>
        public static double Root(double x, int n)
        {
            if (n == 0)
            {
                throw new ArgumentException("The root degree cannot be zero.", nameof(n));
            }
            if (x < 0)
            {
                if ((n & 1) == 0)
                {
                    return double.NaN;
                }
                return -Math.Pow(-x, 1.0 / n);
            }
            return Math.Pow(x, 1.0 / n);
        }

        /// <summary>
        /// Attempts to compute the <paramref name="n"/>-th root of a real number using
        /// the Newton–Raphson iterative method.
        /// </summary>
        /// <param name="x">
        /// The radicand (the value under the root sign).
        /// </param>
        /// <param name="n">
        /// The degree of the root. Must be a positive integer.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the computed approximation of the real
        /// <paramref name="n"/>-th root of <paramref name="x"/> if the operation succeeded;
        /// otherwise, contains <see cref="double.NaN"/>.
        /// </param>
        /// <param name="tolerance">
        /// The desired precision of the result. Iterations stop when
        /// the change between successive approximations is less than this value.
        /// Must be greater than zero.
        /// </param>
        /// <param name="maxIterations">
        /// The maximum number of iterations to perform. Must be greater than zero.
        /// </param>
        /// <returns>
        /// <c>true</c> if the n-th root could be computed as a real number;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool TryNewtonRoot(double x, int n, out double result, double tolerance = 1e-10, int maxIterations = 100)
        {
            result = double.NaN;
            if (n <= 0)
            {
                return false;
            }
            if (tolerance <= 0.0)
            {
                return false;
            }
            if (maxIterations <= 0)
            {
                return false;
            }
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                return false;
            }

            if (x == 0.0)
            {
                result = 0.0;
                return true;
            }

            if (n == 1)
            {
                result = x;
                return true;
            }

            bool isNegative = x < 0.0;
            if (isNegative && (n & 1) == 0)
            {
                return false;
            }
            double absX = isNegative ? -x : x;
            double y = absX >= 1.0 ? absX : 1.0;
            for (int i = 0; i < maxIterations; i++)
            {
                double yPow = Math.Pow(y, n - 1);
                if (yPow == 0.0)
                {
                    y = 0.0;
                    break;
                }
                double yNext = ((n - 1) * y + absX / yPow) / n;

                if (Math.Abs(yNext - y) <= tolerance)
                {
                    y = yNext;
                    break;
                }

                y = yNext;
            }

            result = isNegative ? -y : y;
            return true;
        }

        /// <summary>
        /// Computes the <paramref name="n"/>-th root of a real number using
        /// the Newton–Raphson iterative method.
        /// </summary>
        /// <param name="x">
        /// The radicand (the value under the root sign).
        /// </param>
        /// <param name="n">
        /// The degree of the root. Must be a positive integer.
        /// </param>
        /// <param name="tolerance">
        /// The desired precision of the result. Iterations stop when
        /// the change between successive approximations is less than this value.
        /// Must be greater than zero.
        /// </param>
        /// <param name="maxIterations">
        /// The maximum number of iterations to perform. Must be greater than zero.
        /// </param>
        /// <returns>
        /// An approximation of the real <paramref name="n"/>-th root of <paramref name="x"/>.
        /// For negative <paramref name="x"/> and odd <paramref name="n"/>, the negative real root is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="n"/> is not positive, or if
        /// <paramref name="tolerance"/> is not positive, or if
        /// <paramref name="maxIterations"/> is not positive.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="x"/> is negative and <paramref name="n"/> is even,
        /// because there is no real n-th root in that case.
        /// </exception>
        public static double NewtonRoot(double x, int n, double tolerance = 1e-10, int maxIterations = 100)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "The root degree must be positive.");
            }
            if (tolerance <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "Tolerance must be greater than zero.");
            }
            if (maxIterations <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxIterations), "Max iterations must be greater than zero.");
            }
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                throw new ArgumentException("The radicand must be a finite value.", nameof(x));
            }
            if (x == 0.0)
            {
                return 0.0;
            }
            if (n == 1)
            {
                return x;
            }

            bool isNegative = x < 0.0;

            if (isNegative && (n & 1) == 0)
            {
                throw new ArgumentException("Even root of a negative value has no real solution.", nameof(x));
            }
            double absX = 0;
            if (isNegative)
            {
                absX = -x;
            }
            else
            {
                absX = x;
            }

            double y = 0;
            if (absX >= 1.0)
            {
                y = absX;
            }
            else
            {
                y = 1.0;
            }

            for (int i = 0; i < maxIterations; i++)
            {
                double yPow = Math.Pow(y, n - 1);
                if (yPow == 0.0)
                {
                    y = 0.0;
                    break;
                }
                double yNext = ((n - 1) * y + absX / yPow) / n;

                if (Math.Abs(yNext - y) <= tolerance)
                {
                    y = yNext;
                    break;
                }

                y = yNext;
            }
            if (isNegative)
            {
                return -y;
            }
            else
            {
                return y;
            }
        }
    }
}
