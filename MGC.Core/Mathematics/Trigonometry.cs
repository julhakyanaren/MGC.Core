namespace MGC.Core.Mathematics
{
    using System;

    /// <summary>
    /// Provides mathematical functions for trigonometric operations
    /// that are not directly available in <see cref="Math"/>.
    /// </summary>
    /// <remarks>
    /// This class complements the standard <see cref="Math"/> trigonometric API
    /// by implementing additional reciprocal trigonometric functions:
    /// <list type="bullet">
    /// <item><description>cotangent (<c>cot</c>)</description></item>
    /// <item><description>secant (<c>sec</c>)</description></item>
    /// <item><description>cosecant (<c>csc</c>)</description></item>
    /// </list>
    /// <para>
    /// All angles passed to the methods of this class must be specified in radians,
    /// in accordance with the conventions used by <see cref="Math"/>.
    /// </para>
    /// <para>
    /// Each method explicitly checks the domain of definition of the corresponding
    /// trigonometric function and throws an <see cref="ArgumentException"/> when
    /// the function is mathematically undefined.
    /// </para>
    /// <para>
    /// The class is designed as a lightweight, deterministic extension to the core
    /// mathematics module and does not duplicate functionality already provided
    /// by <see cref="Math"/>.
    /// </para>
    /// </remarks>
    public static class Trigonometry
    {
        /// <summary>
        /// Computes the cotangent of an angle.
        /// </summary>
        /// <param name="a">
        /// Angle in radians.
        /// </param>
        /// <returns>
        /// The cotangent of the specified angle, defined as cos(a) / sin(a).
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when sin(a) equals zero, since the cotangent is undefined at this angle
        /// (a = k·π, where k is an integer).
        /// </exception>
        /// <remarks>
        /// This method uses the mathematical definition:
        /// cot(a) = cos(a) / sin(a).
        /// <para>
        /// The angle must be specified in radians, as required by <see cref="Math"/> trigonometric functions.
        /// </para>
        /// </remarks>
        public static double Cot(double a)
        {
            double sin = Math.Sin(a);
            if (sin == 0.0)
            {
                throw new ArgumentException(
                    "Cotangent is undefined when sin(angle) = 0.",
                    nameof(a));
            }

            return Math.Cos(a) / sin;
        }

        /// <summary>
        /// Computes the secant of an angle.
        /// </summary>
        /// <param name="a">
        /// Angle in radians.
        /// </param>
        /// <returns>
        /// The secant of the specified angle, defined as 1 / cos(a).
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when cos(a) equals zero, since the secant is undefined at this angle
        /// (a = π/2 + k·π, where k is an integer).
        /// </exception>
        /// <remarks>
        /// This method uses the mathematical definition:
        /// sec(a) = 1 / cos(a).
        /// <para>
        /// The angle must be specified in radians, as required by <see cref="Math"/> trigonometric functions.
        /// </para>
        /// </remarks>
        public static double Sec(double a)
        {
            double cos = Math.Cos(a);

            if (cos == 0.0)
            {
                throw new ArgumentException("Secant is undefined when cos(angle) = 0.", nameof(a));
            }

            return 1.0 / cos;
        }

        /// <summary>
        /// Computes the cosecant of an angle.
        /// </summary>
        /// <param name="a">
        /// Angle in radians.
        /// </param>
        /// <returns>
        /// The cosecant of the specified angle, defined as 1 / sin(a).
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when sin(a) equals zero, since the cosecant is undefined at this angle
        /// (a = k·π, where k is an integer).
        /// </exception>
        /// <remarks>
        /// This method uses the mathematical definition:
        /// csc(a) = 1 / sin(a).
        /// <para>
        /// The angle must be specified in radians, as required by <see cref="Math"/> trigonometric functions.
        /// </para>
        /// </remarks>
        public static double Csc(double a)
        {
            double sin = Math.Sin(a);

            if (sin == 0.0)
            {
                throw new ArgumentException("Cosecant is undefined when sin(angle) = 0.", nameof(a));
            }

            return 1.0 / sin;
        }
    }
}
