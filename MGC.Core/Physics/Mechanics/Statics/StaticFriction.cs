namespace MGC.Physics.Mechanics.Statics
{
    /// <summary>
    /// Provides utility methods for calculations related to static friction (friction at rest)
    /// and the condition for the onset of slipping in classical statics.
    ///
    /// Static friction is a contact force that prevents relative motion between two surfaces.
    /// Its magnitude is self-adjusting: it can take any value from 0 up to a maximum limit,
    /// depending on the applied tangential (shear) force.
    ///
    /// Key relationships:
    /// - Maximum static friction (threshold before slip):
    ///     F_s,max = mu_s * N
    ///
    /// - No slip condition:
    ///     <![CDATA[|F_t| <= mu_s * N]]>
    ///
    /// - Slip condition:
    ///     <![CDATA[|F_t| > mu_s * N]]>
    ///
    /// Sign convention used in this class:
    /// - appliedTangentialForce may be signed to represent direction along a chosen tangent axis.
    /// - StaticFrictionForce returns a signed value that always opposes the applied tangential force
    ///   (i.e., it acts in the opposite direction along the tangent axis).
    ///
    /// Units:
    /// - Forces are in Newtons (N).
    /// - Coefficient of static friction (muStatic) is dimensionless.
    ///
    /// Validation rules:
    /// - normalForce must be non-negative for contact problems (N >= 0).
    /// - muStatic must be non-negative (muStatic >= 0).
    /// - For RequiredMuStaticToPreventSlip, normalForce must be greater than zero (N > 0),
    ///   because the required coefficient is undefined at N = 0.
    /// </summary>
    public static class StaticFriction
    {
        private static void ValidateMuStatic(double muStatic)
        {
            if (muStatic < 0.0)
            {
                throw new ArgumentException(
                    "Static friction coefficient (muStatic) must be non-negative.",
                    nameof(muStatic));
            }
        }
        private static void ValidateNormalForceNonNegative(double normalForce)
        {
            if (normalForce < 0.0)
            {
                throw new ArgumentException(
                    "Normal force must be non-negative.",
                    nameof(normalForce));
            }
        }
        private static void ValidateNormalForcePositive(double normalForce)
        {
            if (normalForce <= 0.0)
            {
                throw new ArgumentException(
                    "Normal force must be greater than zero.",
                    nameof(normalForce));
            }
        }

        /// <summary>
        /// Computes the maximum possible static friction force magnitude before slipping begins.
        ///
        /// Physical meaning:
        /// - This is the threshold value of static friction. If the magnitude of the applied tangential
        ///   force exceeds this limit, slipping will occur.
        ///
        /// Formula:
        /// - F_s,max = mu_s * N
        /// </summary>
        /// <param name="normalForce">Normal reaction force N (must be non-negative).</param>
        /// <param name="muStatic">Coefficient of static friction mu_s (must be non-negative).</param>
        /// <returns>The maximum static friction force magnitude (>= 0).</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="normalForce"/> is negative or <paramref name="muStatic"/> is negative.
        /// </exception>
        public static double MaxStaticFriction(double normalForce, double muStatic)
        {
            ValidateNormalForceNonNegative(normalForce);
            ValidateMuStatic(muStatic);

            return normalForce * muStatic;
        }

        /// <summary>
        /// Determines whether slipping occurs for a given applied tangential force.
        ///
        /// Physical meaning:
        /// - Compares the magnitude of the applied tangential force against the maximum static friction.
        /// - If the applied force magnitude exceeds the limit, slipping begins.
        ///
        /// Condition:
        /// - Slip occurs if: <![CDATA[|F_t| > mu_s * N]]>
        /// </summary>
        /// <param name="appliedTangentialForce">
        /// Applied tangential (shear) force F_t along a chosen tangent axis (may be signed).
        /// </param>
        /// <param name="normalForce">Normal reaction force N (must be non-negative).</param>
        /// <param name="muStatic">Coefficient of static friction mu_s (must be non-negative).</param>
        /// <returns>
        /// True if slipping occurs; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="normalForce"/> is negative or <paramref name="muStatic"/> is negative.
        /// </exception>
        public static bool IsSlipOccurring(double appliedTangentialForce, double normalForce, double muStatic)
        {
            ValidateNormalForceNonNegative(normalForce);
            ValidateMuStatic(muStatic);

            return System.Math.Abs(appliedTangentialForce) > normalForce * muStatic;
        }

        /// <summary>
        /// Computes the minimum coefficient of static friction required to prevent slipping
        /// for a given applied tangential force and normal force.
        ///
        /// Physical meaning:
        /// - This is the smallest mu_s such that the no-slip condition holds:
        ///     <![CDATA[|F_t| <= mu_s * N]]>
        ///
        /// Formula:
        /// - mu_s,min = |F_t| / N
        /// </summary>
        /// <param name="appliedTangentialForce">
        /// Applied tangential (shear) force F_t along a chosen tangent axis (may be signed).
        /// </param>
        /// <param name="normalForce">
        /// Normal reaction force N (must be greater than zero).
        /// </param>
        /// <returns>The minimum required coefficient of static friction (>= 0).</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="normalForce"/> is less than or equal to zero.
        /// </exception>
        public static double RequiredMuStaticToPreventSlip(double appliedTangentialForce, double normalForce)
        {
            ValidateNormalForcePositive(normalForce);

            return System.Math.Abs(appliedTangentialForce / normalForce);
        }

        /// <summary>
        /// Computes the static friction force along the tangent axis that acts to oppose
        /// the applied tangential force, limited by the maximum static friction threshold.
        ///
        /// Physical meaning:
        /// - Static friction opposes impending motion and adjusts its magnitude as needed,
        ///   up to the maximum value mu_s * N.
        ///
        /// Returned sign:
        /// - The returned value is signed and always opposes <paramref name="appliedTangentialForce"/>.
        ///
        /// Magnitude rule:
        /// - |F_s| = min(|F_t|, mu_s * N)
        ///
        /// Direction rule:
        /// - F_s has opposite sign to F_t
        /// </summary>
        /// <param name="appliedTangentialForce">
        /// Applied tangential (shear) force F_t along a chosen tangent axis (may be signed).
        /// </param>
        /// <param name="normalForce">Normal reaction force N (must be non-negative).</param>
        /// <param name="muStatic">Coefficient of static friction mu_s (must be non-negative).</param>
        /// <returns>
        /// Signed static friction force along the tangent axis that opposes the applied force.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="normalForce"/> is negative or <paramref name="muStatic"/> is negative.
        /// </exception>
        public static double StaticFrictionForce(double appliedTangentialForce, double normalForce,double muStatic)
        {
            ValidateNormalForceNonNegative(normalForce);
            ValidateMuStatic(muStatic);

            double maxFriction = normalForce * muStatic;
            double requiredMagnitude = System.Math.Abs(appliedTangentialForce);
            double actualMagnitude = System.Math.Min(requiredMagnitude, maxFriction);

            double sign = System.Math.Sign(appliedTangentialForce);
            return -sign * actualMagnitude;
        }

        /// <summary>
        /// Determines whether the given coefficient of static friction is sufficient to prevent slipping
        /// under the specified tangential and normal forces.
        ///
        /// Physical meaning:
        /// - Checks the no-slip condition:
        ///     <![CDATA[|F_t| <= mu_s * N]]>
        /// </summary>
        /// <param name="appliedTangentialForce">
        /// Applied tangential (shear) force F_t along a chosen tangent axis (may be signed).
        /// </param>
        /// <param name="normalForce">Normal reaction force N (must be non-negative).</param>
        /// <param name="muStatic">Coefficient of static friction mu_s (must be non-negative).</param>
        /// <returns>
        /// True if slipping can be prevented; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="normalForce"/> is negative or <paramref name="muStatic"/> is negative.
        /// </exception>
        public static bool CanPreventSlip(double appliedTangentialForce, double normalForce, double muStatic)
        {
            ValidateNormalForceNonNegative(normalForce);
            ValidateMuStatic(muStatic);

            return System.Math.Abs(appliedTangentialForce) <= muStatic * normalForce;
        }
    }
}
