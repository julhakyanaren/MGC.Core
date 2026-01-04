using MGC.Mathematics;

namespace MGC.Core.Math.Extensions
{
    public static class AnglesExtensions
    {
        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.DegToRad(double)"/>.
        /// </remarks>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>
        /// The angle converted to radians.
        /// </returns>
        public static double DegToRad(this double degrees)
        {
            return Angles.DegToRad(degrees);
        }
        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.RadToDeg(double)"/>.
        /// </remarks>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>
        /// The angle converted to degrees.
        /// </returns>
        public static double RadToDeg(this double radians)
        {
            return Angles.RadToDeg(radians);
        }

        /// <summary>
        /// Normalizes an angle in degrees into the range [0, 360).
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.WrapDeg(double)"/>.
        /// </remarks>
        /// <param name="angle">The input angle in degrees.</param>
        /// <returns>
        /// An equivalent angle in the range [0, 360).
        /// </returns>
        public static double WrapDeg(this double angle)
        {
            return Angles.WrapDeg(angle);
        }
        /// <summary>
        /// Normalizes an angle in radians into the range [0, 2π).
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.WrapRad(double)"/>.
        /// </remarks>
        /// <param name="angle">The input angle in radians.</param>
        /// <returns>
        /// An equivalent angle in the range [0, 2π).
        /// </returns>
        public static double WrapRad(this double angle)
        {
            return Angles.WrapRad(angle);
        }

        /// <summary>
        /// Normalizes an angle in degrees into the signed range (-180, 180].
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.WrapDegSigned(double)"/>.
        /// </remarks>
        /// <param name="angle">The input angle in degrees.</param>
        /// <returns>
        /// An equivalent angle in the signed range (-180, 180].
        /// </returns>
        public static double WrapDegSigned(this double angle)
        {
            return Angles.WrapDegSigned(angle);
        }
        /// <summary>
        /// Normalizes an angle in radians into the signed range (-π, π].
        /// </summary>
        /// <remarks>
        /// This method is equivalent to <see cref="Angles.WrapRadSigned(double)"/>.
        /// </remarks>
        /// <param name="angle">The input angle in radians.</param>
        /// <returns>
        /// An equivalent angle in the signed range (-π, π].
        /// </returns>
        public static double WrapRadSigned(this double angle)
        {
            return Angles.WrapRadSigned(angle);
        }
    }
}
