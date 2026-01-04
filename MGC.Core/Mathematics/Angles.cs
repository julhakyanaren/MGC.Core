namespace MGC.Mathematics
{
    /// <summary>
    /// Helper methods for working with angles:
    /// conversion, normalization, shortest differences and interpolation.
    /// </summary>
    public static class Angles
    {
        private const double FullTurnDeg = 360.0;
        private const double HalfTurnDeg = 180.0;

        /// <summary>
        /// Conversion factor from degrees to radians: π / 180.
        /// </summary>
        public const double Deg2Rad = Math.PI / 180.0;
        /// <summary>
        /// Conversion factor from radians to degrees: 180 / π.
        /// </summary>
        public const double Rad2Deg = 180.0 / Math.PI;

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>
        /// The angle converted to radians.
        /// </returns>
        public static double DegToRad(double degrees)
        {
            return degrees * Deg2Rad;
        }
        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>
        /// The angle converted to degrees.
        /// </returns>
        public static double RadToDeg(double radians)
        {
            return radians * Rad2Deg;
        }

        /// <summary>
        /// Normalizes an angle in degrees into the range [0, 360).
        /// </summary>
        /// <param name="angle">
        /// The input angle in degrees. Can be any real number, including negative values
        /// and values greater than 360.
        /// </param>
        /// <returns>
        /// An equivalent angle in the range [0, 360).
        /// </returns>
        public static double WrapDeg(double angle)
        {
            angle %= FullTurnDeg;
            if (angle < 0.0)
            {
                angle += FullTurnDeg;
            }
            return angle;
        }
        /// <summary>
        /// Normalizes an angle in radians into the range [0, 2π).
        /// </summary>
        /// <param name="angle">
        /// The input angle in radians. Can be any real number, including negative values
        /// and values exceeding one full revolution.
        /// </param>
        /// <returns>
        /// An equivalent angle in the range [0, 2π).
        /// </returns>
        public static double WrapRad(double angle)
        {
            angle %= Math.Tau;
            if (angle < 0.0)
            {
                angle += Math.Tau;
            }
            return angle;
        }

        /// <summary>
        /// Normalizes an angle in degrees into the signed range (-180, 180].
        /// </summary>
        /// <remarks>
        /// This method is useful for working with directional differences where
        /// orientation matters (e.g., "shortest rotation direction").
        /// </remarks>
        /// <param name="angle">
        /// The input angle in degrees. Can be any real number.
        /// </param>
        /// <returns>
        /// An equivalent angle in the signed range (-180, 180].
        /// Positive values represent counter-clockwise rotation,
        /// negative values represent clockwise rotation.
        /// </returns>
        public static double WrapDegSigned(double angle)
        {
            double wrapped = WrapDeg(angle);
            if (wrapped > HalfTurnDeg)
            {
                wrapped -= FullTurnDeg;
            }
            return wrapped;
        }
        /// <summary>
        /// Normalizes an angle in radians into the signed range (-π, π].
        /// </summary>
        /// <remarks>
        /// This form is ideal when computing directional angular differences,
        /// ensuring the result describes the shortest signed arc between angles.
        /// </remarks>
        /// <param name="angle">
        /// The input angle in radians. Can be any real number.
        /// </param>
        /// <returns>
        /// An equivalent angle in the signed range (-π, π].
        /// Positive values correspond to counter-clockwise rotation.
        /// </returns>
        public static double WrapRadSigned(double angle)
        {
            double wrapped = WrapRad(angle);
            if (wrapped > Math.PI)
            {
                wrapped -= Math.Tau;
            }
            return wrapped;
        }

        /// <summary>
        /// Computes the shortest signed angular difference between two angles in degrees.
        /// </summary>
        /// <remarks>
        /// The result is normalized into the range (-180, 180].
        /// A positive value means that <paramref name="to"/> is counter-clockwise
        /// relative to <paramref name="from"/>.
        /// </remarks>
        /// <param name="from">The starting angle in degrees.</param>
        /// <param name="to">The target angle in degrees.</param>
        /// <returns>
        /// The signed shortest difference <c>(to - from)</c> in the range (-180, 180].
        /// </returns>
        public static double DeltaAngleDeg(double from, double to)
        {
            double diff = to - from;
            return WrapDegSigned(diff);
        }
        /// <summary>
        /// Computes the shortest signed angular difference between two angles in radians.
        /// </summary>
        /// <remarks>
        /// The result is normalized into the range (-π, π].
        /// A positive value indicates a counter-clockwise difference.
        /// </remarks>
        /// <param name="from">The starting angle in radians.</param>
        /// <param name="to">The target angle in radians.</param>
        /// <returns>
        /// The signed shortest difference <c>(to - from)</c> in the range (-π, π].
        /// </returns>
        public static double DeltaAngleRad(double from, double to)
        {
            double diff = to - from;
            return WrapRadSigned(diff);
        }

        /// <summary>
        /// Linearly interpolates between two angles in degrees along the shortest arc.
        /// </summary>
        /// <remarks>
        /// The interpolation factor <paramref name="t"/> is not clamped.
        /// A value of 0 returns <paramref name="from"/>, and 1 returns the value
        /// reached after moving one shortest-angle difference toward <paramref name="to"/>.
        /// </remarks>
        /// <param name="from">The starting angle in degrees.</param>
        /// <param name="to">The target angle in degrees.</param>
        /// <param name="t">
        /// Interpolation factor. Typically in the range [0, 1], but values outside this
        /// range produce extrapolated angles.
        /// </param>
        /// <returns>
        /// An interpolated angle in degrees, normalized into the range [0, 360).
        /// </returns>
        public static double LerpAngleDeg(double from, double to, double t)
        {
            double delta = DeltaAngleDeg(from, to);
            double result = from + delta * t;
            return WrapDeg(result);
        }
        /// <summary>
        /// Linearly interpolates between two angles in radians along the shortest arc.
        /// </summary>
        /// <remarks>
        /// The interpolation follows the minimal-angle direction and then normalizes
        /// the result into the range [0, 2π).
        /// </remarks>
        /// <param name="from">The starting angle in radians.</param>
        /// <param name="to">The target angle in radians.</param>
        /// <param name="t">
        /// Interpolation factor. Usually within [0, 1], but not clamped.
        /// </param>
        /// <returns>
        /// An interpolated angle in radians, normalized into the range [0, 2π).
        /// </returns>
        public static double LerpAngleRad(double from, double to, double t)
        {
            double delta = DeltaAngleRad(from, to);
            double result = from + delta * t;
            return WrapRad(result);
        }

        /// <summary>
        /// Gradually moves an angle in degrees toward a target angle,
        /// limiting the maximum change by <paramref name="maxDelta"/>.
        /// </summary>
        /// <remarks>
        /// The motion follows the shortest angular direction and is based on
        /// the signed difference computed by <see cref="DeltaAngleDeg(double, double)"/>.
        /// </remarks>
        /// <param name="current">The current angle in degrees.</param>
        /// <param name="target">The target angle in degrees.</param>
        /// <param name="maxDelta">
        /// The maximum allowed change in degrees for a single step.
        /// If the remaining difference is smaller than this value, the method
        /// returns the target angle.
        /// </param>
        /// <returns>
        /// The new angle in degrees, moved toward <paramref name="target"/>
        /// by at most <paramref name="maxDelta"/> and normalized into [0, 360).
        /// </returns>
        public static double MoveTowardsAngleDeg(double current, double target, double maxDelta)
        {
            double delta = DeltaAngleDeg(current, target);

            if (maxDelta <= 0.0 || Math.Abs(delta) <= maxDelta)
            {
                return WrapDeg(target);
            }

            double step = Math.Clamp(delta, -maxDelta, maxDelta);
            double result = current + step;
            return WrapDeg(result);
        }
        /// <summary>
        /// Gradually moves an angle in radians toward a target angle,
        /// limiting the maximum change by <paramref name="maxDelta"/>.
        /// </summary>
        /// <remarks>
        /// The motion follows the shortest angular path, using
        /// <see cref="DeltaAngleRad(double, double)"/> to determine the signed difference.
        /// </remarks>
        /// <param name="current">The current angle in radians.</param>
        /// <param name="target">The target angle in radians.</param>
        /// <param name="maxDelta">
        /// The maximum allowed change in radians for a single step.
        /// If the remaining difference is smaller than this value, the method
        /// returns the target angle.
        /// </param>
        /// <returns>
        /// The new angle in radians, moved toward <paramref name="target"/>
        /// by at most <paramref name="maxDelta"/> and normalized into [0, 2π).
        /// </returns>
        public static double MoveTowardsAngleRad(double current, double target, double maxDelta)
        {
            double delta = DeltaAngleRad(current, target);

            if (maxDelta <= 0.0 || Math.Abs(delta) <= maxDelta)
            {
                return WrapRad(target);
            }

            double step = Math.Clamp(delta, -maxDelta, maxDelta);
            double result = current + step;
            return WrapRad(result);
        }

        /// <summary>
        /// Determines whether an angle in degrees lies within an angular range
        /// [<paramref name="start"/>, <paramref name="end"/>], taking into account
        /// wrap-around at 360 degrees.
        /// </summary>
        /// <param name="angle">The angle to test, in degrees.</param>
        /// <param name="start">
        /// The start of the range in degrees. The range is interpreted on a circle,
        /// so it may cross the 360°/0° boundary.
        /// </param>
        /// <param name="end">
        /// The end of the range in degrees. The range is inclusive of both
        /// <paramref name="start"/> and <paramref name="end"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="angle"/> lies within the specified range;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAngleBetweenDeg(double angle, double start, double end)
        {
            angle = WrapDeg(angle);
            start = WrapDeg(start);
            end = WrapDeg(end);

            if (start <= end)
            {
                return angle >= start && angle <= end;
            }
            return angle >= start || angle <= end;
        }
        /// <summary>
        /// Determines whether an angle in radians lies within an angular range
        /// [<paramref name="start"/>, <paramref name="end"/>], taking into account
        /// wrap-around at 2π radians.
        /// </summary>
        /// <param name="angle">The angle to test, in radians.</param>
        /// <param name="start">
        /// The start of the range in radians. The range may cross the 2π/0 boundary.
        /// </param>
        /// <param name="end">
        /// The end of the range in radians. The range is inclusive of both bounds.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="angle"/> lies within the specified circular range;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAngleBetweenRad(double angle, double start, double end)
        {
            angle = WrapRad(angle);
            start = WrapRad(start);
            end = WrapRad(end);

            if (start <= end)
            {
                return angle >= start && angle <= end;
            }
            return angle >= start || angle <= end;
        }

        /// <summary>
        /// Determines whether two angles in degrees are approximately equal,
        /// taking into account circular wrap-around.
        /// </summary>
        /// <param name="a">The first angle in degrees.</param>
        /// <param name="b">The second angle in degrees.</param>
        /// <param name="toleranceDeg">
        /// The maximum allowed absolute difference in degrees, measured along
        /// the shortest angular path.
        /// </param>
        /// <returns>
        /// <c>true</c> if the shortest difference between the two angles is less than
        /// or equal to <paramref name="toleranceDeg"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool ApproximatelyEqualDeg(double a, double b, double toleranceDeg = 0.0001)
        {
            double delta = DeltaAngleDeg(a, b);
            return Math.Abs(delta) <= toleranceDeg;
        }
        /// <summary>
        /// Determines whether two angles in radians are approximately equal,
        /// taking into account circular wrap-around.
        /// </summary>
        /// <param name="a">The first angle in radians.</param>
        /// <param name="b">The second angle in radians.</param>
        /// <param name="toleranceRad">
        /// The maximum allowed absolute difference in radians, measured along
        /// the shortest angular path.
        /// </param>
        /// <returns>
        /// <c>true</c> if the shortest difference between the two angles is less than
        /// or equal to <paramref name="toleranceRad"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool ApproximatelyEqualRad(double a, double b, double toleranceRad = 1e-6)
        {
            double delta = DeltaAngleRad(a, b);
            return Math.Abs(delta) <= toleranceRad;
        }
    }
}
