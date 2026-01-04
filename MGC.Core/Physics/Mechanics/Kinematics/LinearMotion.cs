namespace MGC.Physics.Mechanics.Kinematics
{
    /// <summary>
    /// Provides helper methods for one-dimensional (1D) linear kinematics calculations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class contains formulas for uniform motion (constant velocity) and uniformly accelerated motion
    /// (constant acceleration) along a single chosen axis.
    /// The sign of scalar values (velocity, acceleration, displacement, position) represents direction along that axis.
    /// </para>
    /// <para>
    /// All inputs and outputs use SI units:
    /// position and displacement in meters (m), time in seconds (s),
    /// velocity in meters per second (m/s), acceleration in meters per second squared (m/s²).
    /// </para>
    /// <para>
    /// Notes:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Methods in this class generally do not restrict negative values, because negative values are valid in 1D motion
    /// (e.g., motion in the opposite direction). Exceptions are thrown mainly for mathematically invalid cases such as
    /// division by zero or a negative value under a square root.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="FinalSpeedFromDisplacement(double, double, double)"/> returns a non-negative speed (magnitude),
    /// not a signed velocity, because the time-independent equation yields |v|.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="FreeFallDistance(double, double)"/> uses <see cref="PhysicConstants.StandardGravity"/> as the constant
    /// gravitational acceleration g. The caller defines the axis orientation (sign convention).
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Uniform motion: s = v * t
    /// double s = LinearMotion.Displacement(velocity: 5, time: 3);     // 15 m
    ///
    /// // Uniformly accelerated motion: v = v0 + a * t
    /// double v = LinearMotion.FinalVelocity(v0: 2, acceleration: 3, time: 4); // 14 m/s
    ///
    /// // Position with constant acceleration: x = x0 + v0*t + 0.5*a*t^2
    /// double x = LinearMotion.Position(x0: 10, initialVelocity: -2, acceleration: 1, time: 5);
    ///
    /// // Free fall distance using standard gravity:
    /// double h = LinearMotion.FreeFallDistance(time: 2); // ~19.6 m (if axis is downward)
    /// </code>
    /// </example>
    public static class LinearMotion
    {
        /// <summary>
        /// Computes displacement for uniform linear motion.
        /// </summary>
        /// <param name="velocity">Constant velocity (m/s). Can be negative to represent direction.</param>
        /// <param name="time">Time interval (s). Can be negative in a purely mathematical sense.</param>
        /// <returns>Displacement (m): s = v * t.</returns>
        public static double Displacement(double velocity, double time)
        {
            return velocity * time;
        }

        /// <summary>
        /// Computes final velocity for uniformly accelerated linear motion.
        /// </summary>
        /// <param name="v0">Initial velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Final velocity (m/s): v = v0 + a * t.</returns>
        public static double FinalVelocity(double v0, double acceleration, double time)
        {
            return v0 + acceleration * time;
        }

        /// <summary>
        /// Computes displacement for uniformly accelerated linear motion.
        /// </summary>
        /// <param name="v0">Initial velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Displacement (m): s = v0 * t + (a * t²) / 2.</returns>
        public static double Displacement(double v0, double acceleration, double time)
        {
            return v0 * time + acceleration * Math.Pow(time, 2) / 2.0;
        }

        /// <summary>
        /// Computes displacement (distance along the chosen axis) for free fall with constant gravitational acceleration.
        /// </summary>
        /// <param name="time">Time interval (s).</param>
        /// <param name="v0">Initial velocity along the axis (m/s). Default is 0.</param>
        /// <returns>Displacement (m): s = v0 * t + (g * t²) / 2.</returns>
        /// <remarks>
        /// Uses <see cref="PhysicConstants.StandardGravity"/> as g (m/s²). Sign and axis direction are defined by the caller.
        /// </remarks>
        public static double FreeFallDistance(double time, double v0 = 0)
        {
            return v0 * time + PhysicConstants.StandardGravity * Math.Pow(time, 2) / 2.0;
        }

        /// <summary>
        /// Computes time for uniform linear motion from displacement and velocity.
        /// </summary>
        /// <param name="displacement">Displacement (m).</param>
        /// <param name="velocity">Constant velocity (m/s). Must be non-zero.</param>
        /// <returns>Time (s): t = s / v.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="velocity"/> is zero.</exception>
        public static double TimeFromDisplacement(double displacement, double velocity)
        {
            if (velocity == 0)
            {
                throw new ArgumentException("Velocity must be non-zero.", nameof(velocity));
            }
            return displacement / velocity;
        }

        /// <summary>
        /// Computes constant acceleration from initial and final velocities over a time interval.
        /// </summary>
        /// <param name="v0">Initial velocity (m/s).</param>
        /// <param name="velocity">Final velocity (m/s).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Acceleration (m/s²): a = (v - v0) / t.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double AccelerationFromVelocities(double v0, double velocity, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }
            return (velocity - v0) / time;
        }

        /// <summary>
        /// Computes time needed to change velocity from v0 to v with constant acceleration.
        /// </summary>
        /// <param name="v0">Initial velocity (m/s).</param>
        /// <param name="velocity">Final velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²). Must be non-zero.</param>
        /// <returns>Time (s): t = (v - v0) / a.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="acceleration"/> is zero.</exception>
        public static double TimeFromVelocities(double v0, double velocity, double acceleration)
        {
            if (acceleration == 0)
            {
                throw new ArgumentException("Acceleration must be non-zero.", nameof(acceleration));
            }
            return (velocity - v0) / acceleration;
        }

        /// <summary>
        /// Computes the final speed (non-negative) from displacement using the time-independent kinematic equation.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²).</param>
        /// <param name="displacement">Displacement (m).</param>
        /// <returns>
        /// Final speed (m/s): |v| = sqrt(v0² + 2 * a * s).
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression under the square root is negative (no real solution).
        /// </exception>
        /// <remarks>
        /// This method returns speed (magnitude), not signed velocity. Direction must be inferred from context.
        /// </remarks>
        public static double FinalSpeedFromDisplacement(double initialVelocity, double acceleration, double displacement)
        {
            double value = Math.Pow(initialVelocity, 2) + 2.0 * acceleration * displacement;
            if (value < 0)
            {
                throw new ArgumentException("Expression under square root is negative. Check acceleration and displacement.", nameof(displacement));
            }
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Computes displacement using the time-independent kinematic equation.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity (m/s).</param>
        /// <param name="finalVelocity">Final velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²). Must be non-zero.</param>
        /// <returns>Displacement (m): s = (v² - v0²) / (2 * a).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="acceleration"/> is zero.</exception>
        public static double DisplacementFromVelocities(double initialVelocity, double finalVelocity, double acceleration)
        {
            if (acceleration == 0)
            {
                throw new ArgumentException("Acceleration must be non-zero.", nameof(acceleration));
            }
            return (Math.Pow(finalVelocity, 2) - Math.Pow(initialVelocity, 2)) / (2.0 * acceleration);
        }

        /// <summary>
        /// Computes position for uniform linear motion.
        /// </summary>
        /// <param name="x0">Initial position (m).</param>
        /// <param name="velocity">Constant velocity (m/s).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Position (m): x = x0 + v * t.</returns>
        public static double Position(double x0, double velocity, double time)
        {
            return x0 + velocity * time;
        }

        /// <summary>
        /// Computes position for uniformly accelerated linear motion.
        /// </summary>
        /// <param name="x0">Initial position (m).</param>
        /// <param name="initialVelocity">Initial velocity (m/s).</param>
        /// <param name="acceleration">Constant acceleration (m/s²).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Position (m): x = x0 + v0 * t + (a * t²) / 2.</returns>
        public static double Position(double x0, double initialVelocity, double acceleration, double time)
        {
            return x0 + initialVelocity * time + 0.5 * acceleration * Math.Pow(time, 2);
        }

        /// <summary>
        /// Computes initial velocity from positions under constant acceleration.
        /// </summary>
        /// <param name="x">Final position (m).</param>
        /// <param name="x0">Initial position (m).</param>
        /// <param name="acceleration">Constant acceleration (m/s²).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Initial velocity (m/s): v0 = (x - x0 - (a * t²)/2) / t.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double InitialVelocityFromPosition(double x, double x0, double acceleration, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }
            return (x - x0 - (acceleration * Math.Pow(time, 2)) / 2.0) / time;
        }

        /// <summary>
        /// Computes constant velocity from two positions over a time interval (uniform motion).
        /// </summary>
        /// <param name="x">Final position (m).</param>
        /// <param name="x0">Initial position (m).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Velocity (m/s): v = (x - x0) / t.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double VelocityFromPositions(double x, double x0, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }
            return (x - x0) / time;
        }
    }
}
