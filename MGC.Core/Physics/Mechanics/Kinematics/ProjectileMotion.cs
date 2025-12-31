namespace MGC.Physics.Mechanics.Kinematics
{
    /// <summary>
    /// Provides methods for calculating projectile motion parameters
    /// in a uniform gravitational field without air resistance.
    /// </summary>
    /// <remarks>
    /// Assumptions:
    /// - Air resistance is ignored
    /// - Gravitational acceleration is constant
    /// - Motion occurs in a 2D plane
    /// - Angles are measured from the horizontal axis
    /// </remarks>
    public static class ProjectileMotion
    {
        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle value in degrees.</param>
        /// <returns>Angle value in radians.</returns>
        public static double DegToRad(double degrees)
        {
            return degrees * System.Math.PI / 180.0;
        }

        /// <summary>
        /// Calculates the horizontal component of the initial velocity.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Horizontal velocity component.</returns>
        public static double HorizontalVelocity(double initialVelocity, double angleRadians)
        {
            return initialVelocity * System.Math.Cos(angleRadians);
        }

        /// <summary>
        /// Calculates the vertical component of the initial velocity.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Vertical velocity component.</returns>
        public static double VerticalVelocity(double initialVelocity, double angleRadians)
        {
            return initialVelocity * System.Math.Sin(angleRadians);
        }

        /// <summary>
        /// Calculates the total flight time of the projectile.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Total time of flight.</returns>
        public static double TimeOfFlight(double initialVelocity, double angleRadians)
        {
            return 2.0 * VerticalVelocity(initialVelocity, angleRadians)
                   / Constants.StandartGravity;
        }

        /// <summary>
        /// Calculates the maximum height reached by the projectile
        /// relative to the launch point.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Maximum height.</returns>
        public static double MaxHeight(double initialVelocity, double angleRadians)
        {
            return (System.Math.Pow(initialVelocity, 2)
                * System.Math.Pow(System.Math.Sin(angleRadians), 2))
                / (2.0 * Constants.StandartGravity);
        }

        /// <summary>
        /// Calculates the horizontal range of the projectile
        /// when landing at the same height as launch.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Horizontal flight range.</returns>
        public static double FlyRange(double initialVelocity, double angleRadians)
        {
            return (System.Math.Pow(initialVelocity, 2)
                * System.Math.Sin(2.0 * angleRadians))
                / Constants.StandartGravity;
        }

        /// <summary>
        /// Calculates the horizontal position of the projectile at a given time.
        /// </summary>
        /// <param name="time">Elapsed time.</param>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Horizontal position.</returns>
        public static double PositionX(double time, double initialVelocity, double angleRadians)
        {
            return initialVelocity * System.Math.Cos(angleRadians) * time;
        }

        /// <summary>
        /// Calculates the vertical position of the projectile at a given time.
        /// </summary>
        /// <param name="time">Elapsed time.</param>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <param name="startY">Initial vertical position.</param>
        /// <returns>Vertical position.</returns>
        public static double PositionY(double time, double initialVelocity, double angleRadians, double startY = 0)
        {
            return startY
                + initialVelocity * System.Math.Sin(angleRadians) * time
                - (Constants.StandartGravity * System.Math.Pow(time, 2) / 2.0);
        }

        /// <summary>
        /// Calculates the vertical position of the projectile as a function
        /// of the horizontal position (trajectory equation).
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <param name="coordX">Horizontal position.</param>
        /// <param name="startY">Initial vertical position.</param>
        /// <returns>Vertical position corresponding to the given horizontal coordinate.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the initial velocity is less than zero.
        /// </exception>
        public static double TrajectoryY(double initialVelocity, double angleRadians, double coordX, double startY = 0)
        {
            if (initialVelocity < 0)
            {
                throw new ArgumentException("Velocity must be greater then zero.", nameof(initialVelocity));
            }

            return startY
                + coordX * System.Math.Tan(angleRadians)
                - (Constants.StandartGravity * System.Math.Pow(coordX, 2))
                  / (2.0 * System.Math.Pow(initialVelocity, 2)
                  * System.Math.Pow(System.Math.Cos(angleRadians), 2));
        }

        /// <summary>
        /// Calculates the time required to reach the maximum height.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity magnitude.</param>
        /// <param name="angleRadians">Launch angle in radians.</param>
        /// <returns>Time to reach maximum height.</returns>
        public static double TimeToMaxHeight(double initialVelocity, double angleRadians)
        {
            return initialVelocity * System.Math.Sin(angleRadians)
                   / Constants.StandartGravity;
        }
    }
}
