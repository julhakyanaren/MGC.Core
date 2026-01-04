using System;

namespace MGC.Physics.Mechanics.Dynamics
{
    /// <summary>
    /// Provides methods for calculations related to linear Moment, impulse,
    /// and angular Moment in classical dynamics.
    ///
    /// This class focuses on "moments" in the dynamics sense:
    /// - Linear Moment of a body and total Moment of a system
    /// - Impulse of a force as a Moment change measure
    /// - Angular Moment expressed via linear Moment and lever arm geometry
    ///
    /// Key physical concepts:
    ///
    /// 1) Linear Moment
    /// - Measures quantity of motion:
    ///   p = m * v
    ///
    /// 2) Total Moment of a system
    /// - Vector (or scalar) sum of individual momenta:
    ///   P = Σ p_i
    ///
    /// 3) Impulse
    /// - For a constant force applied during dt:
    ///   J = F * dt
    /// - In general, impulse corresponds to Moment change:
    ///   J = Δp
    ///
    /// 4) Angular Moment (via linear Moment and lever arm)
    /// - Magnitude:
    ///   |L| = p * |r * sin(theta)|
    /// - Signed:
    ///   L = p * r * sin(theta)
    ///
    /// Design notes:
    /// - Uses scalar values and tuple-based vectors (no Unity or external vector types).
    /// - All angles are in radians.
    /// - Mass equal to zero is allowed (useful for theoretical cases, graphs, and edge tests).
    /// - Negative mass is not allowed.
    /// </summary>
    public static class MomentDynamics
    {
        private static void ValidateMassNonNegative(double mass)
        {
            if (mass < 0.0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
        }
        private static void ValidateRadiusNonNegative(double radius)
        {
            if (radius < 0.0)
            {
                throw new ArgumentException("Radius must be non-negative.", nameof(radius));
            }
        }

        /// <summary>
        /// Calculates the linear Moment of a body (1D).
        ///
        /// Formula:
        /// - p = m * v
        /// </summary>
        /// <param name="mass">Body mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="velocity">Linear velocity in meters per second (m/s).</param>
        /// <returns>Linear Moment in kg*m/s.</returns>
        public static double LinearMoment(double mass, double velocity)
        {
            ValidateMassNonNegative(mass);

            return mass * velocity;
        }
        /// <summary>
        /// Calculates the linear Moment vector in 2D.
        ///
        /// Formulas:
        /// - px = m * vx
        /// - py = m * vy
        /// </summary>
        /// <param name="mass">Body mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="velocityX">Velocity component along the X axis (m/s).</param>
        /// <param name="velocityY">Velocity component along the Y axis (m/s).</param>
        /// <returns>Moment components (px, py) in kg*m/s.</returns>
        public static (double x, double y) LinearMoment2D(double mass, double velocityX, double velocityY)
        {
            ValidateMassNonNegative(mass);

            return (mass * velocityX, mass * velocityY);
        }
        /// <summary>
        /// Calculates the linear Moment vector in 3D.
        ///
        /// Formulas:
        /// - px = m * vx
        /// - py = m * vy
        /// - pz = m * vz
        /// </summary>
        /// <param name="mass">Body mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="velocityX">Velocity component along the X axis (m/s).</param>
        /// <param name="velocityY">Velocity component along the Y axis (m/s).</param>
        /// <param name="velocityZ">Velocity component along the Z axis (m/s).</param>
        /// <returns>Moment components (px, py, pz) in kg*m/s.</returns>
        public static (double x, double y, double z) LinearMoment3D(double mass, double velocityX, double velocityY, double velocityZ)
        {
            ValidateMassNonNegative(mass);

            return (mass * velocityX, mass * velocityY, mass * velocityZ);
        }

        /// <summary>
        /// Calculates the total linear Moment of a system (1D) from individual momenta.
        ///
        /// Formula:
        /// - P = p1 + p2 + p3 + ...
        /// </summary>
        /// <param name="momenta">Array of linear momenta (kg*m/s).</param>
        /// <returns>Total linear Moment of the system (kg*m/s).</returns>
        public static double TotalMoment(params double[] momenta)
        {
            if (momenta == null)
            {
                throw new ArgumentNullException(nameof(momenta));
            }

            double total = 0.0;

            for (int i = 0; i < momenta.Length; i++)
            {
                total += momenta[i];
            }

            return total;
        }
        /// <summary>
        /// Calculates the total linear Moment of a system (1D) using masses and velocities.
        ///
        /// Formula:
        /// - P = m1*v1 + m2*v2 + m3*v3 + ...
        /// </summary>
        /// <param name="masses">Array of body masses (kg). Each mass must be non-negative.</param>
        /// <param name="velocities">Array of corresponding velocities (m/s).</param>
        /// <returns>Total linear Moment of the system (kg*m/s).</returns>
        public static double TotalMoment(double[] masses, double[] velocities)
        {
            if (masses == null)
            {
                throw new ArgumentNullException(nameof(masses));
            }

            if (velocities == null)
            {
                throw new ArgumentNullException(nameof(velocities));
            }

            if (masses.Length != velocities.Length)
            {
                throw new ArgumentException("Masses and velocities must have the same length.");
            }

            double total = 0.0;

            for (int i = 0; i < masses.Length; i++)
            {
                total += LinearMoment(masses[i], velocities[i]);
            }

            return total;
        }
        /// <summary>
        /// Calculates the total linear Moment of a system in 2D from individual Moment vectors.
        ///
        /// Formulas:
        /// - Px = Σ px
        /// - Py = Σ py
        /// </summary>
        /// <param name="momenta">Array of Moment vectors represented as (px, py).</param>
        /// <returns>Total Moment components (Px, Py) in kg*m/s.</returns>
        public static (double x, double y) TotalMoment2D((double x, double y)[] momenta)
        {
            if (momenta == null)
            {
                throw new ArgumentNullException(nameof(momenta));
            }

            double totalX = 0.0;
            double totalY = 0.0;

            for (int i = 0; i < momenta.Length; i++)
            {
                totalX += momenta[i].x;
                totalY += momenta[i].y;
            }

            return (totalX, totalY);
        }
        /// <summary>
        /// Calculates the total linear Moment of a system in 2D using masses and velocity vectors.
        ///
        /// Formulas:
        /// - Px = Σ (mi * vix)
        /// - Py = Σ (mi * viy)
        /// </summary>
        /// <param name="masses">Array of body masses (kg). Each mass must be non-negative.</param>
        /// <param name="velocities">Array of velocity vectors represented as (vx, vy).</param>
        /// <returns>Total Moment components (Px, Py) in kg*m/s.</returns>
        public static (double x, double y) TotalMoment2D(double[] masses, (double x, double y)[] velocities)
        {
            if (masses == null)
            {
                throw new ArgumentNullException(nameof(masses));
            }

            if (velocities == null)
            {
                throw new ArgumentNullException(nameof(velocities));
            }

            if (masses.Length != velocities.Length)
            {
                throw new ArgumentException("Masses and velocities must have the same length.");
            }

            double totalX = 0.0;
            double totalY = 0.0;

            for (int i = 0; i < masses.Length; i++)
            {
                ValidateMassNonNegative(masses[i]);

                totalX += masses[i] * velocities[i].x;
                totalY += masses[i] * velocities[i].y;
            }

            return (totalX, totalY);
        }
        /// <summary>
        /// Calculates the total linear Moment of a system in 3D from individual Moment vectors.
        ///
        /// Formulas:
        /// - Px = Σ px
        /// - Py = Σ py
        /// - Pz = Σ pz
        /// </summary>
        /// <param name="momenta">Array of Moment vectors represented as (px, py, pz).</param>
        /// <returns>Total Moment components (Px, Py, Pz) in kg*m/s.</returns>
        public static (double x, double y, double z) TotalMoment3D((double x, double y, double z)[] momenta)
        {
            if (momenta == null)
            {
                throw new ArgumentNullException(nameof(momenta));
            }

            double totalX = 0.0;
            double totalY = 0.0;
            double totalZ = 0.0;

            for (int i = 0; i < momenta.Length; i++)
            {
                totalX += momenta[i].x;
                totalY += momenta[i].y;
                totalZ += momenta[i].z;
            }

            return (totalX, totalY, totalZ);
        }

        /// <summary>
        /// Calculates the impulse of a force (constant-force model).
        ///
        /// Physical concept:
        /// - Impulse is the effect of a force applied during a time interval.
        /// - It corresponds to Moment change in many problems:
        ///   J = Δp
        ///
        /// Formula:
        /// - J = F * dt
        /// </summary>
        /// <param name="force">Force magnitude in newtons (N).</param>
        /// <param name="deltaTime">Time interval in seconds (s). Must be non-negative.</param>
        /// <returns>Impulse in N*s.</returns>
        public static double Impulse(double force, double deltaTime)
        {
            if (deltaTime < 0.0)
            {
                throw new ArgumentException("Delta time must be non-negative.", nameof(deltaTime));
            }

            return force * deltaTime;
        }

        /// <summary>
        /// Calculates the magnitude of angular Moment using radius and linear Moment.
        ///
        /// Physical concept:
        /// - Angular Moment magnitude can be computed from linear Moment and lever arm geometry.
        ///
        /// Formula:
        /// - |L| = p * |r * sin(theta)|
        /// </summary>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="linearMoment">Linear Moment (kg*m/s).</param>
        /// <param name="angleRadians">Angle between Moment direction and radius (rad).</param>
        /// <returns>Angular Moment magnitude (kg*m^2/s).</returns>
        public static double AngularMoment(double radius, double linearMoment, double angleRadians)
        {
            ValidateRadiusNonNegative(radius);

            double arm = Math.Abs(radius * Math.Sin(angleRadians));

            return linearMoment * arm;
        }
        /// <summary>
        /// Calculates the magnitude of angular Moment using mass and speed.
        ///
        /// Formulas:
        /// - p = m * v
        /// - |L| = p * |r * sin(theta)|
        /// </summary>
        /// <param name="mass">Body mass (kg). Must be non-negative.</param>
        /// <param name="velocity">Speed (m/s).</param>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between velocity direction and radius (rad).</param>
        /// <returns>Angular Moment magnitude (kg*m^2/s).</returns>
        public static double AngularMoment(double mass, double velocity, double radius, double angleRadians)
        {
            return AngularMoment(radius, LinearMoment(mass, velocity), angleRadians);
        }
        /// <summary>
        /// Calculates the signed angular Moment using radius and linear Moment.
        ///
        /// Physical concept:
        /// - Signed angular Moment indicates direction based on the caller's convention.
        ///
        /// Formula:
        /// - L = p * r * sin(theta)
        /// </summary>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="linearMoment">Linear Moment (kg*m/s).</param>
        /// <param name="angleRadians">Angle between Moment direction and radius (rad).</param>
        /// <returns>Signed angular Moment (kg*m^2/s).</returns>
        public static double SignedAngularMoment(double radius, double linearMoment, double angleRadians)
        {
            ValidateRadiusNonNegative(radius);

            return linearMoment * (radius * Math.Sin(angleRadians));
        }
        /// <summary>
        /// Calculates the signed angular Moment using mass and speed.
        ///
        /// Formulas:
        /// - p = m * v
        /// - L = p * r * sin(theta)
        /// </summary>
        /// <param name="mass">Body mass (kg). Must be non-negative.</param>
        /// <param name="velocity">Speed (m/s).</param>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between velocity direction and radius (rad).</param>
        /// <returns>Signed angular Moment (kg*m^2/s).</returns>
        public static double SignedAngularMoment(double mass, double velocity, double radius, double angleRadians)
        {
            return SignedAngularMoment(radius, LinearMoment(mass, velocity), angleRadians);
        }
    }
}
