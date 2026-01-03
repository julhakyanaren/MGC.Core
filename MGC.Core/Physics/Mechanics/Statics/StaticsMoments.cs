using System;

namespace MGC.Physics.Mechanics.Statics
{
    /// <summary>
    /// Provides methods related to moments (torques) and lever equilibrium in classical statics.
    ///
    /// This class focuses on rotational statics concepts where the body is in equilibrium
    /// (no angular acceleration). Core ideas implemented here:
    ///
    /// 1) Lever arm (moment arm)
    ///    - The lever arm is the perpendicular distance from the pivot (axis) to the line of action of a force.
    ///    - It can be represented as:
    ///      • Magnitude (non-negative)
    ///      • Signed value (algebraic, depends on chosen sign convention)
    ///
    /// 2) Torque (moment of a force)
    ///    - Torque measures the rotational effect of a force about a point/axis.
    ///    - Implemented as:
    ///      • Magnitude (non-negative)
    ///      • Signed value (algebraic)
    ///
    /// 3) Rotational equilibrium
    ///    - A rigid body is in rotational equilibrium when the algebraic sum of torques is zero:
    ///      ΣM = 0
    ///    - Includes strict and tolerance-based checks.
    ///
    /// 4) Lever law (two-force balance)
    ///    - A special equilibrium case:
    ///      F1*l1 = F2*l2
    ///    - Supports strict and tolerance-based checks and solving for an unknown force.
    ///
    /// Design notes:
    /// - Operates on scalars only; no Unity or external vector types are used.
    /// - Radius (distance to axis) must be non-negative.
    /// - Signed results follow the caller's sign convention (direction of rotation).
    /// - Tolerance overloads are recommended for floating-point calculations.
    /// </summary>
    public static class StaticsMoments
    {
        private static void ValidateNonNegative(double value, string paramName, string message)
        {
            if (value < 0.0)
            {
                throw new ArgumentException(message, paramName);
            }
        }
        private static void ValidateToleranceNonNegative(double tolerance)
        {
            if (double.IsNaN(tolerance) || double.IsInfinity(tolerance))
            {
                throw new ArgumentException("Tolerance must be a finite number.", nameof(tolerance));
            }

            if (tolerance < 0.0)
            {
                throw new ArgumentException("Tolerance must be non-negative.", nameof(tolerance));
            }
        }

        /// <summary>
        /// Calculates the magnitude of the lever arm (moment arm).
        ///
        /// Physical concept:
        /// - Lever arm is the perpendicular distance from the pivot to the force line of action.
        ///
        /// Formula:
        /// - |l| = |r * sin(theta)|
        /// </summary>
        /// <param name="radius">Distance from the axis of rotation (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between the radius vector and the force direction (rad).</param>
        /// <returns>Lever arm magnitude (m), always non-negative.</returns>
        public static double LeverArm(double radius, double angleRadians)
        {
            ValidateNonNegative(radius, nameof(radius), "Radius must be non-negative.");

            double arm = radius * System.Math.Sin(angleRadians);

            return System.Math.Abs(arm);
        }
        /// <summary>
        /// Calculates the signed (algebraic) lever arm.
        ///
        /// Physical concept:
        /// - Signed lever arm encodes direction based on the chosen sign convention.
        ///
        /// Formula:
        /// - l = r * sin(theta)
        /// </summary>
        /// <param name="radius">Distance from the axis of rotation (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between the radius vector and the force direction (rad).</param>
        /// <returns>Signed lever arm (m).</returns>
        public static double SignedLeverArm(double radius, double angleRadians)
        {
            ValidateNonNegative(radius, nameof(radius), "Radius must be non-negative.");

            return radius * System.Math.Sin(angleRadians);
        }

        /// <summary>
        /// Calculates the torque magnitude.
        ///
        /// Physical concept:
        /// - Torque magnitude equals force magnitude multiplied by lever arm magnitude.
        ///
        /// Formula:
        /// - |M| = |F| * |l| = F * |r * sin(theta)|
        /// </summary>
        /// <param name="force">Force magnitude (N).</param>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between force and radius (rad).</param>
        /// <returns>Torque magnitude (N*m), always non-negative if force is non-negative.</returns>
        public static double Torque(double force, double radius, double angleRadians)
        {
            return force * LeverArm(radius, angleRadians);
        }
        /// <summary>
        /// Calculates the signed (algebraic) torque.
        ///
        /// Physical concept:
        /// - Signed torque indicates direction of rotation based on the caller's convention.
        ///
        /// Formula:
        /// - M = F * r * sin(theta)
        /// </summary>
        /// <param name="force">Force magnitude (N).</param>
        /// <param name="radius">Distance from the axis (m). Must be non-negative.</param>
        /// <param name="angleRadians">Angle between force and radius (rad).</param>
        /// <returns>Signed torque (N*m).</returns>
        public static double SignedTorque(double force, double radius, double angleRadians)
        {
            return force * SignedLeverArm(radius, angleRadians);
        }
        /// <summary>
        /// Calculates torque from a known lever arm.
        ///
        /// Physical concept:
        /// - If the lever arm is already known as a perpendicular distance, torque is:
        ///
        /// Formula:
        /// - M = F * l
        /// </summary>
        /// <param name="force">Force magnitude (N).</param>
        /// <param name="leverArm">Lever arm length (m).</param>
        /// <returns>Torque (N*m).</returns>
        public static double TorqueFromLever(double force, double leverArm)
        {
            return force * leverArm;
        }

        /// <summary>
        /// Determines whether a lever is in equilibrium under two forces (strict comparison).
        ///
        /// Physical concept:
        /// - Lever equilibrium is a special case of rotational equilibrium about the pivot:
        ///   M1 = M2
        ///
        /// Formula:
        /// - F1*l1 = F2*l2
        /// </summary>
        /// <param name="force1">Magnitude of the first force (N).</param>
        /// <param name="leverArm1">Lever arm of the first force (m).</param>
        /// <param name="force2">Magnitude of the second force (N).</param>
        /// <param name="leverArm2">Lever arm of the second force (m).</param>
        /// <returns>True if in equilibrium; otherwise false.</returns>
        public static bool IsLeverInEquilibrium(double force1, double leverArm1, double force2, double leverArm2)
        {
            return force1 * leverArm1 == force2 * leverArm2;
        }
        /// <summary>
        /// Determines whether a lever is in equilibrium under two forces using a tolerance.
        ///
        /// Physical concept:
        /// - Compares the difference of torques produced by both forces.
        ///
        /// Formula:
        /// - |F1*l1 - F2*l2| <= tolerance
        /// </summary>
        /// <param name="force1">Magnitude of the first force (N).</param>
        /// <param name="leverArm1">Lever arm of the first force (m).</param>
        /// <param name="force2">Magnitude of the second force (N).</param>
        /// <param name="leverArm2">Lever arm of the second force (m).</param>
        /// <param name="tolerance">Allowed absolute error for comparison. Must be non-negative.</param>
        /// <returns>True if in equilibrium within tolerance; otherwise false.</returns>
        public static bool IsLeverInEquilibrium(double force1, double leverArm1, double force2, double leverArm2, double tolerance)
        {
            ValidateToleranceNonNegative(tolerance);

            double diff = (force1 * leverArm1) - (force2 * leverArm2);

            return System.Math.Abs(diff) <= tolerance;
        }

        /// <summary>
        /// Determines whether a rigid body is in rotational equilibrium (strict comparison).
        ///
        /// Physical concept:
        /// - Rotational equilibrium occurs when the algebraic sum of torques is zero.
        ///
        /// Formula:
        /// - ΣM = 0
        /// </summary>
        /// <param name="torques">
        /// Array of signed torque values (N*m).
        /// Positive and negative values represent opposite directions of rotation.
        /// </param>
        /// <returns>True if the sum of torques is exactly zero; otherwise false.</returns>
        public static bool IsRotationalEquilibrium(double[] torques)
        {
            if (torques == null)
            {
                throw new ArgumentNullException(nameof(torques));
            }

            double sum = 0.0;

            for (int i = 0; i < torques.Length; i++)
            {
                sum += torques[i];
            }

            return sum == 0.0;
        }
        /// <summary>
        /// Determines whether a rigid body is in rotational equilibrium using a tolerance.
        ///
        /// Physical concept:
        /// - Rotational equilibrium occurs when the algebraic sum of torques is approximately zero.
        ///
        /// Formula:
        /// - |ΣM| <= tolerance
        /// </summary>
        /// <param name="torques">
        /// Array of signed torque values (N*m).
        /// Positive and negative values represent opposite directions of rotation.
        /// </param>
        /// <param name="tolerance">Allowed absolute error for comparison. Must be non-negative.</param>
        /// <returns>True if the sum of torques is within tolerance of zero; otherwise false.</returns>
        public static bool IsRotationalEquilibrium(double[] torques, double tolerance)
        {
            if (torques == null)
            {
                throw new ArgumentNullException(nameof(torques));
            }

            ValidateToleranceNonNegative(tolerance);

            double sum = 0.0;

            for (int i = 0; i < torques.Length; i++)
            {
                sum += torques[i];
            }

            return System.Math.Abs(sum) <= tolerance;
        }

        /// <summary>
        /// Calculates the force required to balance a lever using the lever law.
        ///
        /// Physical concept:
        /// - For two forces acting on a lever in equilibrium about a pivot:
        ///   F1*l1 = F2*l2
        ///
        /// Solving for the unknown force:
        /// - F_unknown = (F_known * l_known) / l_unknown
        ///
        /// Physical note about unknownLeverArm == 0:
        /// - If the lever arm is zero, the force produces no torque about the pivot.
        ///   Equilibrium is impossible for any non-zero opposing torque.
        /// </summary>
        /// <param name="knownForce">Known force magnitude (N).</param>
        /// <param name="knownLeverArm">Lever arm of the known force (m).</param>
        /// <param name="unknownLeverArm">Lever arm of the unknown force (m). Must be non-negative.</param>
        /// <returns>Required force magnitude (N) to satisfy the lever law.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="unknownLeverArm"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="unknownLeverArm"/> is zero because equilibrium is impossible.
        /// </exception>
        public static double ForceFromLeverLaw(double knownForce, double knownLeverArm, double unknownLeverArm)
        {
            ValidateNonNegative(unknownLeverArm, nameof(unknownLeverArm), "Lever arm must be non-negative.");

            if (unknownLeverArm == 0.0)
            {
                throw new InvalidOperationException(
                    "Lever arm equal to zero means no torque can be produced. Equilibrium is impossible.");
            }

            return (knownForce * knownLeverArm) / unknownLeverArm;
        }
    }
}
