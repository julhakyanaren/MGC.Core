namespace MGC.Physics.Mechanics.Statics
{
    /// <summary>
    /// Provides helper methods to verify static equilibrium conditions in classical mechanics.
    ///
    /// Statics studies bodies in rest (or moving with constant velocity) where acceleration is zero.
    /// For a rigid body to remain in static equilibrium, two independent conditions must be satisfied:
    ///
    /// 1) Translational equilibrium (no linear acceleration):
    ///    The vector sum of all external forces must be zero.
    ///    - 1D: Sum of forces along a single axis is zero.
    ///      Formula: ΣF = 0
    ///    - 2D: Sum of force components along X and Y axes is zero.
    ///      Formulas: ΣFx = 0, ΣFy = 0
    ///    - 3D: Sum of force components along X, Y and Z axes is zero.
    ///      Formulas: ΣFx = 0, ΣFy = 0, ΣFz = 0
    ///
    /// 2) Rotational equilibrium (no angular acceleration):
    ///    The algebraic sum of moments (torques) about a chosen point/axis must be zero.
    ///    Formula: ΣM = 0
    ///
    /// This class implements tolerance-based checks because floating-point calculations are not exact.
    /// Each equilibrium method evaluates the corresponding net sum and compares its absolute value
    /// against the specified tolerance:
    ///   |Σ(...)| <= tolerance
    ///
    /// Design notes:
    /// - The methods accept only scalar values and tuples to keep the core physics library independent
    ///   from Unity or any external vector types.
    /// - Forces and torques are treated as signed values. The sign convention is defined by the caller.
    /// - An empty array will produce a net sum of zero and therefore returns true. This behavior is
    ///   intentional and can be useful for validation pipelines and test scenarios.
    /// - The tolerance must be a finite number greater than zero; otherwise an exception is thrown.
    /// </summary>
    public static class StaticsEquilibrium
    {
        private static void ValidateTolerance(double tolerance)
        {
            if (double.IsNaN(tolerance) || double.IsInfinity(tolerance))
            {
                throw new ArgumentException("Tolerance must be a finite number.", nameof(tolerance));
            }

            if (tolerance <= 0.0)
            {
                throw new ArgumentException("Tolerance must be greater than zero.", nameof(tolerance));
            }
        }

        /// <summary>
        /// Checks whether a set of forces is in translational equilibrium in one dimension.
        ///
        /// Physical concept:
        /// - Translational equilibrium in 1D occurs when the algebraic sum of all forces along the axis is zero.
        ///
        /// Formula:
        /// - ΣF = 0
        ///
        /// Tolerance check:
        /// - |ΣF| <= tolerance
        /// </summary>
        /// <param name="forces">
        /// Array of signed forces along a single axis.
        /// Positive and negative values represent opposite directions according to the caller's convention.
        /// </param>
        /// <param name="tolerance">
        /// Allowed absolute error for the net force. Must be finite and greater than zero.
        /// </param>
        /// <returns>
        /// True if the net force magnitude is within tolerance; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="tolerance"/> is not finite or is less than or equal to zero.
        /// </exception>
        public static bool IsForceEquilibrium1D(double[] forces, double tolerance = 1e-9)
        {
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }

            ValidateTolerance(tolerance);

            double netForce = 0.0;

            for (int f = 0; f < forces.Length; f++)
            {
                netForce += forces[f];
            }

            return Math.Abs(netForce) <= tolerance;
        }
        /// <summary>
        /// Checks whether a set of forces is in translational equilibrium in two dimensions.
        ///
        /// Physical concept:
        /// - Translational equilibrium in 2D occurs when the sum of force components along X and Y is zero.
        ///
        /// Formulas:
        /// - ΣFx = 0
        /// - ΣFy = 0
        ///
        /// Tolerance check:
        /// - |ΣFx| <= tolerance and |ΣFy| <= tolerance
        /// </summary>
        /// <param name="forces">
        /// Array of forces represented by their components (x, y).
        /// Each tuple entry is interpreted as a single force vector in the plane.
        /// </param>
        /// <param name="tolerance">
        /// Allowed absolute error per component. Must be finite and greater than zero.
        /// </param>
        /// <returns>
        /// True if both net force components are within tolerance; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="tolerance"/> is not finite or is less than or equal to zero.
        /// </exception>
        public static bool IsForceEquilibrium2D((double x, double y)[] forces, double tolerance = 1e-9)
        {
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }

            ValidateTolerance(tolerance);

            double netForceX = 0.0;
            double netForceY = 0.0;

            for (int f = 0; f < forces.Length; f++)
            {
                netForceX += forces[f].x;
                netForceY += forces[f].y;
            }

            return
                Math.Abs(netForceX) <= tolerance &&
                Math.Abs(netForceY) <= tolerance;
        }
        /// <summary>
        /// Checks whether a set of forces is in translational equilibrium in three dimensions.
        ///
        /// Physical concept:
        /// - Translational equilibrium in 3D occurs when the sum of force components along X, Y and Z is zero.
        ///
        /// Formulas:
        /// - ΣFx = 0
        /// - ΣFy = 0
        /// - ΣFz = 0
        ///
        /// Tolerance check:
        /// - |ΣFx| <= tolerance, |ΣFy| <= tolerance, |ΣFz| <= tolerance
        /// </summary>
        /// <param name="forces">
        /// Array of forces represented by their components (x, y, z).
        /// Each tuple entry is interpreted as a single force vector in 3D space.
        /// </param>
        /// <param name="tolerance">
        /// Allowed absolute error per component. Must be finite and greater than zero.
        /// </param>
        /// <returns>
        /// True if all net force components are within tolerance; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="tolerance"/> is not finite or is less than or equal to zero.
        /// </exception>
        public static bool IsForceEquilibrium3D((double x, double y, double z)[] forces, double tolerance = 1e-9)
        {
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }

            ValidateTolerance(tolerance);

            double netForceX = 0.0;
            double netForceY = 0.0;
            double netForceZ = 0.0;

            for (int f = 0; f < forces.Length; f++)
            {
                netForceX += forces[f].x;
                netForceY += forces[f].y;
                netForceZ += forces[f].z;
            }

            return
                Math.Abs(netForceX) <= tolerance &&
                Math.Abs(netForceY) <= tolerance &&
                Math.Abs(netForceZ) <= tolerance;
        }

        /// <summary>
        /// Checks whether a set of torques (moments) is in rotational equilibrium.
        ///
        /// Physical concept:
        /// - Rotational equilibrium occurs when the algebraic sum of torques about a chosen point/axis is zero.
        /// - A torque is considered signed; the sign indicates the direction of rotation by the caller's convention.
        ///
        /// Formula:
        /// - ΣM = 0
        ///
        /// Tolerance check:
        /// - |ΣM| <= tolerance
        /// </summary>
        /// <param name="torques">
        /// Array of signed torques (moments). Positive and negative values represent opposite rotation directions.
        /// </param>
        /// <param name="tolerance">
        /// Allowed absolute error for the net torque. Must be finite and greater than zero.
        /// </param>
        /// <returns>
        /// True if the net torque magnitude is within tolerance; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="torques"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="tolerance"/> is not finite or is less than or equal to zero.
        /// </exception>
        public static bool IsMomentEquilibrium(double[] torques, double tolerance = 1e-9)
        {
            if (torques == null)
            {
                throw new ArgumentNullException(nameof(torques));
            }

            ValidateTolerance(tolerance);

            double netTorque = 0.0;

            for (int t = 0; t < torques.Length; t++)
            {
                netTorque += torques[t];
            }

            return Math.Abs(netTorque) <= tolerance;
        }

        /// <summary>
        /// Checks full static equilibrium for a planar (2D) problem using both force and moment conditions.
        ///
        /// Physical concept:
        /// - A rigid body in 2D static equilibrium must satisfy:
        ///   1) Translational equilibrium in the plane (no linear acceleration):
        ///      ΣFx = 0 and ΣFy = 0
        ///   2) Rotational equilibrium (no angular acceleration):
        ///      ΣM = 0
        ///
        /// Formulas:
        /// - ΣFx = 0
        /// - ΣFy = 0
        /// - ΣM  = 0
        ///
        /// Typical usage:
        /// - Forces are represented as (Fx, Fy).
        /// - Torques are scalar values, usually representing the moment about the axis perpendicular to the plane.
        ///
        /// Tolerance check:
        /// - |ΣFx| <= tolerance, |ΣFy| <= tolerance and |ΣM| <= tolerance
        /// </summary>
        /// <param name="forces">
        /// Array of forces represented by their components (x, y).
        /// </param>
        /// <param name="torques">
        /// Array of signed torques (moments) about the chosen reference point/axis.
        /// </param>
        /// <param name="tolerance">
        /// Allowed absolute error used for both force and moment checks. Must be finite and greater than zero.
        /// </param>
        /// <returns>
        /// True if both force equilibrium (2D) and moment equilibrium are satisfied within tolerance; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="forces"/> or <paramref name="torques"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="tolerance"/> is not finite or is less than or equal to zero.
        /// </exception>
        public static bool IsStaticEquilibrium2D((double x, double y)[] forces, double[] torques, double tolerance = 1e-9)
        {
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }

            if (torques == null)
            {
                throw new ArgumentNullException(nameof(torques));
            }

            ValidateTolerance(tolerance);

            return
                IsForceEquilibrium2D(forces, tolerance) &&
                IsMomentEquilibrium(torques, tolerance);
        }
    }
}
