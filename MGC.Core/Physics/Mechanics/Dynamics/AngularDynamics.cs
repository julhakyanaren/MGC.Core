namespace MGC.Physics.Mechanics.Dynamics
{
    /// <summary>
    /// Provides methods for calculations related to inertia and rotational dynamics
    /// in classical mechanics.
    ///
    /// This class focuses on quantities that measure resistance to changes in motion:
    /// - Linear inertia (mass as the measure of inertia in translation)
    /// - Moment of inertia (measure of inertia in rotation)
    /// - Parallel axis theorem (Steiner's theorem)
    /// - Connection between torque and angular acceleration
    /// - Rotational kinetic energy
    ///
    /// The class is designed as part of the Mechanics -> Dynamics section and operates
    /// only with scalar values. It does not depend on Unity or any external vector types.
    ///
    /// All angular values must be specified in radians.
    /// </summary>
    public static class AngularDynamics
    {
        /// <summary>
        /// Returns the measure of linear inertia.
        ///
        /// In classical mechanics, mass is the measure of inertia for translational motion.
        /// </summary>
        /// <param name="mass">Body mass in kilograms (kg). Must be non-negative.</param>
        /// <returns>Linear inertia value (kg).</returns>
        /// <formula>
        /// I_linear = m
        /// </formula>
        public static double LinearInertia(double mass)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }

            return mass;
        }

        /// <summary>
        /// Calculates the moment of inertia of a point mass relative to a rotation axis.
        ///
        /// This is a fundamental model used for particles and for approximating complex bodies.
        /// </summary>
        /// <param name="mass">Point mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="radius">Distance to the rotation axis in meters (m). Must be non-negative.</param>
        /// <returns>Moment of inertia in kg*m^2.</returns>
        /// <formula>
        /// I = m * r^2
        /// </formula>
        public static double MomentOfInertiaPoint(double mass, double radius)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }

            if (radius < 0)
            {
                throw new ArgumentException("Radius must be non-negative.", nameof(radius));
            }

            return mass * radius * radius;
        }

        /// <summary>
        /// Applies the parallel axis theorem (Steiner's theorem).
        ///
        /// Used to compute the moment of inertia about an axis that is parallel
        /// to an axis through the center of mass.
        /// </summary>
        /// <param name="centralInertia">Moment of inertia about the center of mass (kg*m^2).</param>
        /// <param name="mass">Body mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="distance">Distance between the axes in meters (m). Must be non-negative.</param>
        /// <returns>Moment of inertia about the new axis in kg*m^2.</returns>
        /// <formula>
        /// I = I_cm + m * d^2
        /// </formula>
        public static double ParallelAxisTheorem(double centralInertia, double mass, double distance)
        {
            if (centralInertia < 0)
            {
                throw new ArgumentException("Central inertia must be non-negative.", nameof(centralInertia));
            }

            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }

            if (distance < 0)
            {
                throw new ArgumentException("Distance must be non-negative.", nameof(distance));
            }

            return centralInertia + (mass * distance * distance);
        }

        /// <summary>
        /// Calculates angular acceleration from torque and moment of inertia.
        ///
        /// This is the rotational analogue of Newton's second law:
        /// force causes linear acceleration, torque causes angular acceleration.
        /// </summary>
        /// <param name="torque">Applied torque in newton-meters (N*m).</param>
        /// <param name="momentOfInertia">Moment of inertia in kg*m^2. Must be non-zero.</param>
        /// <returns>Angular acceleration in rad/s^2.</returns>
        /// <formula>
        /// alpha = M / I
        /// </formula>
        public static double AngularAcceleration(double torque, double momentOfInertia)
        {
            if (momentOfInertia == 0)
            {
                throw new ArgumentException("Moment of inertia must be non-zero.", nameof(momentOfInertia));
            }

            if (momentOfInertia < 0)
            {
                throw new ArgumentException("Moment of inertia must be non-negative.", nameof(momentOfInertia));
            }

            return torque / momentOfInertia;
        }

        /// <summary>
        /// Calculates rotational kinetic energy.
        ///
        /// Rotational kinetic energy is the energy associated with rotation of a body.
        /// </summary>
        /// <param name="momentOfInertia">Moment of inertia in kg*m^2. Must be non-negative.</param>
        /// <param name="angularVelocity">Angular velocity in rad/s.</param>
        /// <returns>Rotational kinetic energy in joules (J).</returns>
        /// <formula>
        /// E = 0.5 * I * omega^2
        /// </formula>
        public static double RotationalKineticEnergy(double momentOfInertia, double angularVelocity)
        {
            if (momentOfInertia < 0)
            {
                throw new ArgumentException("Moment of inertia must be non-negative.", nameof(momentOfInertia));
            }

            return 0.5 * momentOfInertia * angularVelocity * angularVelocity;
        }
    }
}
