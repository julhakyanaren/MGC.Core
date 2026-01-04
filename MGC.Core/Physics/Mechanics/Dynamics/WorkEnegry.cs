namespace MGC.Physics.Mechanics.WorkEnergy
{
    /// <summary>
    /// Provides methods related to mechanical work, kinetic energy,
    /// potential energy, and the work-energy theorem.
    ///
    /// This class belongs to classical (Newtonian) mechanics and
    /// operates under non-relativistic assumptions.
    ///
    /// Covered concepts:
    /// - Mechanical work as a scalar quantity
    /// - Work-energy theorem
    /// - Gravitational work and potential energy
    /// - Frictional work (kinetic and static cases)
    ///
    /// The class is designed for a physics core library and does not
    /// depend on any external engines or frameworks.
    ///
    /// Units:
    /// - Mass: kilograms (kg)
    /// - Distance / Height: meters (m)
    /// - Force: newtons (N)
    /// - Energy / Work: joules (J)
    /// - Gravity: meters per second squared (m/s^2)
    ///
    /// Sign conventions:
    /// - Positive work increases kinetic energy
    /// - Negative work decreases kinetic energy
    /// - Gravitational work is negative for upward motion
    /// - Friction always performs negative work
    /// </summary>
    public static class WorkEnergy
    {
        /// <summary>
        /// Calculates the work done by the gravitational force
        /// based on a change in height.
        ///
        /// Formula:
        /// W = -m * g * Δh
        ///
        /// A positive deltaHeight (upward motion) produces
        /// negative work.
        /// A negative deltaHeight (downward motion) produces
        /// positive work.
        /// </summary>
        public static double WorkOfGravityFromHeightChange(
            double mass,
            double deltaHeight,
            double g = PhysicConstants.StandardGravity)
        {
            if (mass < 0)
            {
                throw new ArgumentException(
                    "Mass must be non-negative.",
                    nameof(mass));
            }

            if (g < 0)
            {
                throw new ArgumentException(
                    "Gravity must be non-negative.",
                    nameof(g));
            }

            return -mass * g * deltaHeight;
        }

        /// <summary>
        /// Calculates the work done by friction when the magnitude
        /// of the friction force is known.
        ///
        /// Formula:
        /// W = -F * s
        /// </summary>
        public static double WorkOfFrictionFromForce(
            double frictionForceMagnitude,
            double distance)
        {
            if (frictionForceMagnitude < 0)
            {
                throw new ArgumentException(
                    "Friction force magnitude must be non-negative.",
                    nameof(frictionForceMagnitude));
            }

            if (distance < 0)
            {
                throw new ArgumentException(
                    "Distance must be non-negative.",
                    nameof(distance));
            }

            return -frictionForceMagnitude * distance;
        }

        /// <summary>
        /// Calculates the work done by kinetic friction using
        /// the friction coefficient and normal force.
        ///
        /// Formula:
        /// W = -μ * N * s
        /// </summary>
        public static double WorkOfKineticFriction(
            double mu,
            double normalForce,
            double distance)
        {
            if (mu < 0)
            {
                throw new ArgumentException(
                    "Friction coefficient must be non-negative.",
                    nameof(mu));
            }

            if (normalForce < 0)
            {
                throw new ArgumentException(
                    "Normal force must be non-negative.",
                    nameof(normalForce));
            }

            if (distance < 0)
            {
                throw new ArgumentException(
                    "Distance must be non-negative.",
                    nameof(distance));
            }

            return -(mu * normalForce) * distance;
        }

        /// <summary>
        /// Calculates the work done by kinetic friction on a
        /// horizontal surface.
        ///
        /// Normal force is assumed as:
        /// N = m * g
        ///
        /// Formula:
        /// W = -μ * m * g * s
        /// </summary>
        public static double WorkOfKineticFrictionHorizontal(
            double mu,
            double mass,
            double distance,
            double g = PhysicConstants.StandardGravity)
        {
            if (mu < 0)
            {
                throw new ArgumentException(
                    "Friction coefficient must be non-negative.",
                    nameof(mu));
            }

            if (mass < 0)
            {
                throw new ArgumentException(
                    "Mass must be non-negative.",
                    nameof(mass));
            }

            if (distance < 0)
            {
                throw new ArgumentException(
                    "Distance must be non-negative.",
                    nameof(distance));
            }

            if (g < 0)
            {
                throw new ArgumentException(
                    "Gravity must be non-negative.",
                    nameof(g));
            }

            return -(mu * mass * g) * distance;
        }

        /// <summary>
        /// Calculates the maximum static friction force before
        /// motion begins.
        ///
        /// Formula:
        /// F_s,max = μ_s * N
        /// </summary>
        public static double MaxStaticFrictionForce(
            double muStatic,
            double normalForce)
        {
            if (muStatic < 0)
            {
                throw new ArgumentException(
                    "Friction coefficient must be non-negative.",
                    nameof(muStatic));
            }

            if (normalForce < 0)
            {
                throw new ArgumentException(
                    "Normal force must be non-negative.",
                    nameof(normalForce));
            }

            return muStatic * normalForce;
        }

        /// <summary>
        /// Calculates the work done by kinetic friction on an
        /// inclined plane.
        ///
        /// Normal force:
        /// N = m * g * cos(alpha)
        ///
        /// Formula:
        /// W = -μ * m * g * cos(alpha) * s
        /// </summary>
        public static double WorkOfKineticFrictionIncline(
            double mu, double mass, double distance, 
            double inclineAngleRadians,
            double g = PhysicConstants.StandardGravity)
        {
            if (mu < 0)
            {
                throw new ArgumentException(
                    "Friction coefficient must be non-negative.",
                    nameof(mu));
            }

            if (mass < 0)
            {
                throw new ArgumentException(
                    "Mass must be non-negative.",
                    nameof(mass));
            }

            if (distance < 0)
            {
                throw new ArgumentException(
                    "Distance must be non-negative.",
                    nameof(distance));
            }

            if (g < 0)
            {
                throw new ArgumentException(
                    "Gravity must be non-negative.",
                    nameof(g));
            }

            double normalForce =
                mass * g * Math.Cos(inclineAngleRadians);

            return -(mu * normalForce) * distance;
        }
    }
}
