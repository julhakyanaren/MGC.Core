using System;

namespace MGC.Physics.Mechanics.Statics
{
    /// <summary>
    /// Provides methods for computing weighted centers used in statics and basic mechanics.
    ///
    /// This class includes four closely related physical/engineering concepts:
    ///
    /// 1) Center of Mass (COM)
    ///    - The weighted average position of discrete point masses.
    ///    - Used in statics to simplify systems of masses and to find equivalent application points
    ///      for mass distribution.
    ///    - Formulas:
    ///      • 1D: x_cm = (Σ m_i * x_i) / (Σ m_i)
    ///      • 2D: (x_cm, y_cm) = (Σ m_i * (x_i, y_i)) / (Σ m_i)
    ///      • 3D: (x_cm, y_cm, z_cm) = (Σ m_i * (x_i, y_i, z_i)) / (Σ m_i)
    ///
    /// 2) Centroid (Geometric Center)
    ///    - The weighted average position of geometric parts.
    ///    - For composite objects, the centroid can be computed using part "weights"
    ///      such as length (1D), area (2D), or volume (3D).
    ///    - In uniform density cases, the centroid coincides with the center of mass.
    ///    - Formulas:
    ///      • 1D: x_c = (Σ w_i * x_i) / (Σ w_i)
    ///      • 2D: (x_c, y_c) = (Σ w_i * (x_i, y_i)) / (Σ w_i)
    ///      • 3D: (x_c, y_c, z_c) = (Σ w_i * (x_i, y_i, z_i)) / (Σ w_i)
    ///
    /// 3) Center of Gravity (COG)
    ///    - The point where the resultant gravitational force (weight) can be considered applied.
    ///    - In a uniform gravitational field, COG coincides with COM.
    ///    - When given weights directly (forces), the same weighted average formula applies:
    ///      • r_cg = (Σ W_i * r_i) / (Σ W_i)
    ///
    /// 4) Resultant Location (Center of Load) for parallel forces
    ///    - The line/point of action of the single equivalent resultant that replaces multiple
    ///      parallel forces.
    ///    - Formula (for ΣF != 0):
    ///      • r_R = (Σ F_i * r_i) / (Σ F_i)
    ///
    /// Input/validation rules:
    /// - Arrays must be non-null, non-empty, and have equal lengths.
    /// - For COM methods: masses must be non-negative, and total mass must be greater than zero.
    /// - For centroid methods: weights must be non-negative, and total weight must be greater than zero.
    /// - For COG/resultant methods using signed weights/forces: values may be signed, but the total
    ///   must be non-zero (otherwise location is undefined).
    ///
    /// Design notes:
    /// - The core library does not use any external vector types; tuple coordinates are used instead.
    /// - Exceptions are thrown early to surface invalid inputs and undefined physical situations.
    /// </summary>
    public static class CenterOfMass
    {
        /// <summary>
        /// Validates that two 1D arrays are non-null, non-empty, and have the same length.
        /// </summary>
        private static void ValidateSameLength(double[] a, double[] b, string aName, string bName)
        {
            if (a == null)
            {
                throw new ArgumentNullException(aName);
            }
            if (b == null)
            {
                throw new ArgumentNullException(bName);
            }
            if (a.Length == 0)
            {
                throw new ArgumentException("Array must not be empty.", aName);
            }
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Arrays must have the same length.", aName);
            }
        }
        /// <summary>
        /// Validates that a weights array and a 2D positions array are non-null, non-empty,
        /// and have the same length.
        /// </summary>
        private static void ValidateSameLength(double[] a, (double x, double y)[] b, string aName, string bName)
        {
            if (a == null)
            {
                throw new ArgumentNullException(aName);
            }
            if (b == null)
            {
                throw new ArgumentNullException(bName);
            }
            if (a.Length == 0)
            {
                throw new ArgumentException("Array must not be empty.", aName);
            }
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Arrays must have the same length.", aName);
            }
        }
        /// <summary>
        /// Validates that a weights array and a 3D positions array are non-null, non-empty,
        /// and have the same length.
        /// </summary>
        private static void ValidateSameLength(double[] a, (double x, double y, double z)[] b, string aName, string bName)
        {
            if (a == null)
            {
                throw new ArgumentNullException(aName);
            }
            if (b == null)
            {
                throw new ArgumentNullException(bName);
            }
            if (a.Length == 0)
            {
                throw new ArgumentException("Array must not be empty.", aName);
            }
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Arrays must have the same length.", aName);
            }
        }

        /// <summary>
        /// Computes a weighted center in 1D for non-negative weights.
        /// This helper is used for centroid-like calculations where weights represent
        /// geometric measures (length/area/volume) and must not be negative.
        ///
        /// Formula:
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// </summary>
        private static double WeightedCenter1D_NonNegativeWeights
            (double[] weights, double[] positions,
            string weightsName, string positionsName)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedSum = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                if (w < 0)
                {
                    throw new ArgumentException("Weights must be non-negative.", weightsName);
                }

                total += w;
                weightedSum += w * positions[i];
            }

            if (total == 0.0)
            {
                throw new ArgumentException("Total weight must be greater than zero. Centroid is undefined.", weightsName);
            }

            return weightedSum / total;
        }
        /// <summary>
        /// Computes a weighted center in 2D for non-negative weights.
        /// This helper is used for centroid-like calculations where weights represent
        /// geometric measures (length/area/volume) and must not be negative.
        ///
        /// Formula:
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// y = (Σ w_i * y_i) / (Σ w_i)
        /// </summary>
        private static (double x, double y) WeightedCenter2D_NonNegativeWeights
            (double[] weights, (double x, double y)[] positions,
            string weightsName, string positionsName)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedX = 0.0;
            double weightedY = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                if (w < 0)
                {
                    throw new ArgumentException("Weights must be non-negative.", weightsName);
                }

                total += w;
                weightedX += w * positions[i].x;
                weightedY += w * positions[i].y;
            }

            if (total == 0.0)
            {
                throw new ArgumentException("Total weight must be greater than zero. Centroid is undefined.", weightsName);
            }

            return (weightedX / total, weightedY / total);
        }
        /// <summary>
        /// Computes a weighted center in 3D for non-negative weights.
        /// This helper is used for centroid-like calculations where weights represent
        /// geometric measures (length/area/volume) and must not be negative.
        ///
        /// Formula:
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// y = (Σ w_i * y_i) / (Σ w_i)
        /// z = (Σ w_i * z_i) / (Σ w_i)
        /// </summary>
        private static (double x, double y, double z) WeightedCenter3D_NonNegativeWeights
            (double[] weights, (double x, double y, double z)[] positions,
            string weightsName, string positionsName)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedX = 0.0;
            double weightedY = 0.0;
            double weightedZ = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                if (w < 0)
                {
                    throw new ArgumentException("Weights must be non-negative.", weightsName);
                }

                total += w;
                weightedX += w * positions[i].x;
                weightedY += w * positions[i].y;
                weightedZ += w * positions[i].z;
            }

            if (total == 0.0)
            {
                throw new ArgumentException("Total weight must be greater than zero. Centroid is undefined.", weightsName);
            }

            return (weightedX / total, weightedY / total, weightedZ / total);
        }

        /// <summary>
        /// Computes a weighted center in 1D for signed weights.
        /// This helper is used for force/weight based calculations (COG, resultant location),
        /// where values may be signed by convention.
        ///
        /// Formula (requires Σw != 0):
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// </summary>
        private static double WeightedCenter1D_SignedWeights
            (double[] weights, double[] positions,
            string weightsName, string positionsName, string totalZeroMessage)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedSum = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                total += w;
                weightedSum += w * positions[i];
            }

            if (total == 0.0)
            {
                throw new ArgumentException(totalZeroMessage, weightsName);
            }

            return weightedSum / total;
        }

        /// <summary>
        /// Computes a weighted center in 2D for signed weights.
        /// This helper is used for force/weight based calculations (COG, resultant location),
        /// where values may be signed by convention.
        ///
        /// Formula (requires Σw != 0):
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// y = (Σ w_i * y_i) / (Σ w_i)
        /// </summary>
        private static (double x, double y) WeightedCenter2D_SignedWeights
            (double[] weights, (double x, double y)[] positions,
            string weightsName, string positionsName, string totalZeroMessage)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedX = 0.0;
            double weightedY = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                total += w;
                weightedX += w * positions[i].x;
                weightedY += w * positions[i].y;
            }

            if (total == 0.0)
            {
                throw new ArgumentException(totalZeroMessage, weightsName);
            }

            return (weightedX / total, weightedY / total);
        }

        /// <summary>
        /// Computes a weighted center in 3D for signed weights.
        /// This helper is used for force/weight based calculations (COG, resultant location),
        /// where values may be signed by convention.
        ///
        /// Formula (requires Σw != 0):
        /// x = (Σ w_i * x_i) / (Σ w_i)
        /// y = (Σ w_i * y_i) / (Σ w_i)
        /// z = (Σ w_i * z_i) / (Σ w_i)
        /// </summary>
        private static (double x, double y, double z) WeightedCenter3D_SignedWeights
            (double[] weights, (double x, double y, double z)[] positions,
            string weightsName, string positionsName, string totalZeroMessage)
        {
            ValidateSameLength(weights, positions, weightsName, positionsName);

            double total = 0.0;
            double weightedX = 0.0;
            double weightedY = 0.0;
            double weightedZ = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                double w = weights[i];

                total += w;
                weightedX += w * positions[i].x;
                weightedY += w * positions[i].y;
                weightedZ += w * positions[i].z;
            }

            if (total == 0.0)
            {
                throw new ArgumentException(totalZeroMessage, weightsName);
            }

            return (weightedX / total, weightedY / total, weightedZ / total);
        }

        /// <summary>
        /// Computes the center of mass in 1D for discrete point masses.
        ///
        /// Formula:
        /// x_cm = (Σ m_i * x_i) / (Σ m_i)
        ///
        /// Constraints:
        /// - Each mass must be non-negative.
        /// - Total mass must be greater than zero.
        /// </summary>
        /// <param name="masses">Point masses m_i (non-negative).</param>
        /// <param name="positions">Positions x_i corresponding to each mass.</param>
        /// <returns>The center of mass coordinate x_cm.</returns>
        public static double CenterOfMass1D(double[] masses, double[] positions)
        {
            ValidateSameLength(masses, positions, nameof(masses), nameof(positions));
            double massTotal = 0.0;
            double weightedSum = 0.0;

            for (int i = 0; i < masses.Length; i++)
            {
                if (masses[i] < 0)
                {
                    throw new ArgumentException("Mass must be non-negative.", nameof(masses));
                }

                massTotal += masses[i];
                weightedSum += masses[i] * positions[i];
            }

            if (massTotal == 0.0)
            {
                throw new ArgumentException("Total mass must be greater than zero. Center of mass is undefined.", nameof(masses));
            }
            return weightedSum / massTotal;
        }

        /// <summary>
        /// Computes the center of mass in 2D for discrete point masses.
        ///
        /// Formula:
        /// x_cm = (Σ m_i * x_i) / (Σ m_i)
        /// y_cm = (Σ m_i * y_i) / (Σ m_i)
        ///
        /// Constraints:
        /// - Each mass must be non-negative.
        /// - Total mass must be greater than zero.
        /// </summary>
        /// <param name="masses">Point masses m_i (non-negative).</param>
        /// <param name="positions">Positions (x_i, y_i) corresponding to each mass.</param>
        /// <returns>The center of mass coordinates (x_cm, y_cm).</returns>
        public static (double x, double y) CenterOfMass2D(double[] masses, (double x, double y)[] positions)
        {
            ValidateSameLength(masses, positions, nameof(masses), nameof(positions));
            double massTotal = 0.0;

            double weightedSumX = 0.0;
            double weightedSumY = 0.0;

            for (int i = 0; i < masses.Length; i++)
            {
                if (masses[i] < 0)
                {
                    throw new ArgumentException("Mass must be non-negative.", nameof(masses));
                }

                massTotal += masses[i];
                weightedSumX += masses[i] * positions[i].x;
                weightedSumY += masses[i] * positions[i].y;
            }

            if (massTotal == 0.0)
            {
                throw new ArgumentException("Total mass must be greater than zero. Center of mass is undefined.", nameof(masses));
            }
            return (weightedSumX / massTotal, weightedSumY / massTotal);
        }

        /// <summary>
        /// Computes the center of mass in 3D for discrete point masses.
        ///
        /// Formula:
        /// x_cm = (Σ m_i * x_i) / (Σ m_i)
        /// y_cm = (Σ m_i * y_i) / (Σ m_i)
        /// z_cm = (Σ m_i * z_i) / (Σ m_i)
        ///
        /// Constraints:
        /// - Each mass must be non-negative.
        /// - Total mass must be greater than zero.
        /// </summary>
        /// <param name="masses">Point masses m_i (non-negative).</param>
        /// <param name="positions">Positions (x_i, y_i, z_i) corresponding to each mass.</param>
        /// <returns>The center of mass coordinates (x_cm, y_cm, z_cm).</returns>
        public static (double x, double y, double z) CenterOfMass3D(double[] masses, (double x, double y, double z)[] positions)
        {
            ValidateSameLength(masses, positions, nameof(masses), nameof(positions));
            double massTotal = 0.0;

            double weightedSumX = 0.0;
            double weightedSumY = 0.0;
            double weightedSumZ = 0.0;

            for (int i = 0; i < masses.Length; i++)
            {
                if (masses[i] < 0)
                {
                    throw new ArgumentException("Mass must be non-negative.", nameof(masses));
                }

                massTotal += masses[i];
                weightedSumX += masses[i] * positions[i].x;
                weightedSumY += masses[i] * positions[i].y;
                weightedSumZ += masses[i] * positions[i].z;
            }

            if (massTotal == 0.0)
            {
                throw new ArgumentException("Total mass must be greater than zero. Center of mass is undefined.", nameof(masses));
            }
            return (weightedSumX / massTotal, weightedSumY / massTotal, weightedSumZ / massTotal);
        }

        /// <summary>
        /// Computes the centroid in 1D using non-negative weights.
        ///
        /// Typical use:
        /// - weights represent segment lengths or other non-negative measures,
        /// - centroidPositions represent the centroid/midpoint coordinate of each part.
        ///
        /// Formula:
        /// x_c = (Σ w_i * x_i) / (Σ w_i)
        ///
        /// Constraints:
        /// - Each weight must be non-negative.
        /// - Total weight must be greater than zero.
        /// </summary>
        /// <param name="weights">Non-negative weights (e.g., lengths).</param>
        /// <param name="centroidPositions">Centroid positions x_i for each part.</param>
        /// <returns>The centroid coordinate x_c.</returns>
        public static double Centroid1D(double[] weights, double[] centroidPositions)
        {
            return WeightedCenter1D_NonNegativeWeights(weights, centroidPositions, nameof(weights), nameof(centroidPositions));
        }
        /// <summary>
        /// Computes the centroid in 2D using non-negative weights.
        ///
        /// Typical use:
        /// - weights represent areas or other non-negative measures,
        /// - centroidPositions represent the centroid of each part.
        ///
        /// Formula:
        /// x_c = (Σ w_i * x_i) / (Σ w_i)
        /// y_c = (Σ w_i * y_i) / (Σ w_i)
        ///
        /// Constraints:
        /// - Each weight must be non-negative.
        /// - Total weight must be greater than zero.
        /// </summary>
        /// <param name="weights">Non-negative weights (e.g., areas).</param>
        /// <param name="centroidPositions">Centroid positions (x_i, y_i) for each part.</param>
        /// <returns>The centroid coordinates (x_c, y_c).</returns>
        public static (double x, double y) Centroid2D(double[] weights, (double x, double y)[] centroidPositions)
        {
            return WeightedCenter2D_NonNegativeWeights(weights, centroidPositions, nameof(weights), nameof(centroidPositions));
        }
        /// <summary>
        /// Computes the centroid in 3D using non-negative weights.
        ///
        /// Typical use:
        /// - weights represent volumes or other non-negative measures,
        /// - centroidPositions represent the centroid of each part.
        ///
        /// Formula:
        /// x_c = (Σ w_i * x_i) / (Σ w_i)
        /// y_c = (Σ w_i * y_i) / (Σ w_i)
        /// z_c = (Σ w_i * z_i) / (Σ w_i)
        ///
        /// Constraints:
        /// - Each weight must be non-negative.
        /// - Total weight must be greater than zero.
        /// </summary>
        /// <param name="weights">Non-negative weights (e.g., volumes).</param>
        /// <param name="centroidPositions">Centroid positions (x_i, y_i, z_i) for each part.</param>
        /// <returns>The centroid coordinates (x_c, y_c, z_c).</returns>
        public static (double x, double y, double z) Centroid3D(double[] weights, (double x, double y, double z)[] centroidPositions)
        {
            return WeightedCenter3D_NonNegativeWeights(weights, centroidPositions, nameof(weights), nameof(centroidPositions));
        }

        /// <summary>
        /// Computes the center of gravity in 1D using weights (forces).
        ///
        /// In a uniform gravitational field, the center of gravity coincides with the
        /// center of mass, but this overload allows working directly with weights.
        ///
        /// Formula (requires ΣW != 0):
        /// x_cg = (Σ W_i * x_i) / (Σ W_i)
        /// </summary>
        /// <param name="weights">Weights W_i (may be signed by convention).</param>
        /// <param name="positions">Positions x_i corresponding to each weight.</param>
        /// <returns>The center of gravity coordinate x_cg.</returns>
        public static double CenterOfGravity1D(double[] weights, double[] positions)
        {
            return WeightedCenter1D_SignedWeights(weights, positions, nameof(weights), nameof(positions), "Total weight must be non-zero. Center of gravity is undefined.");
        }
        /// <summary>
        /// Computes the center of gravity in 2D using weights (forces).
        ///
        /// Formula (requires ΣW != 0):
        /// x_cg = (Σ W_i * x_i) / (Σ W_i)
        /// y_cg = (Σ W_i * y_i) / (Σ W_i)
        /// </summary>
        /// <param name="weights">Weights W_i (may be signed by convention).</param>
        /// <param name="positions">Positions (x_i, y_i) corresponding to each weight.</param>
        /// <returns>The center of gravity coordinates (x_cg, y_cg).</returns>
        public static (double x, double y) CenterOfGravity2D(double[] weights, (double x, double y)[] positions)
        {
            return WeightedCenter2D_SignedWeights(weights, positions, nameof(weights), nameof(positions), "Total weight must be non-zero. Center of gravity is undefined.");
        }
        /// <summary>
        /// Computes the center of gravity in 3D using weights (forces).
        ///
        /// Formula (requires ΣW != 0):
        /// x_cg = (Σ W_i * x_i) / (Σ W_i)
        /// y_cg = (Σ W_i * y_i) / (Σ W_i)
        /// z_cg = (Σ W_i * z_i) / (Σ W_i)
        /// </summary>
        /// <param name="weights">Weights W_i (may be signed by convention).</param>
        /// <param name="positions">Positions (x_i, y_i, z_i) corresponding to each weight.</param>
        /// <returns>The center of gravity coordinates (x_cg, y_cg, z_cg).</returns>
        public static (double x, double y, double z) CenterOfGravity3D(double[] weights, (double x, double y, double z)[] positions)
        {
            return WeightedCenter3D_SignedWeights(weights, positions, nameof(weights), nameof(positions), "Total weight must be non-zero. Center of gravity is undefined.");
        }

        /// <summary>
        /// Computes the center of gravity in 1D from masses, assuming uniform gravity.
        /// In uniform gravity, COG = COM, therefore this method delegates to CenterOfMass1D.
        /// </summary>
        public static double CenterOfGravityFromMasses1D(double[] masses, double[] positions)
        {
            return CenterOfMass1D(masses, positions);
        }
        /// <summary>
        /// Computes the center of gravity in 2D from masses, assuming uniform gravity.
        /// In uniform gravity, COG = COM, therefore this method delegates to CenterOfMass2D.
        /// </summary>
        public static (double x, double y) CenterOfGravityFromMasses2D(double[] masses, (double x, double y)[] positions)
        {
            return CenterOfMass2D(masses, positions);
        }
        /// <summary>
        /// Computes the center of gravity in 3D from masses, assuming uniform gravity.
        /// In uniform gravity, COG = COM, therefore this method delegates to CenterOfMass3D.
        /// </summary>
        public static (double x, double y, double z) CenterOfGravityFromMasses3D(double[] masses, (double x, double y, double z)[] positions)
        {
            return CenterOfMass3D(masses, positions);
        }
        /// <summary>
        /// Computes the 1D location of the resultant (equivalent) of parallel forces.
        ///
        /// Formula (requires ΣF != 0):
        /// x_R = (Σ F_i * x_i) / (Σ F_i)
        /// </summary>
        /// <param name="forces">Forces F_i (may be signed by convention).</param>
        /// <param name="positions">Positions x_i corresponding to each force.</param>
        /// <returns>The resultant location x_R.</returns>
        public static double ResultantLocation1D(double[] forces, double[] positions)
        {
            return WeightedCenter1D_SignedWeights(forces, positions, nameof(forces), nameof(positions), "Total force must be non-zero. Resultant location is undefined.");
        }
        /// <summary>
        /// Computes the 2D location of the resultant (equivalent) of parallel forces.
        ///
        /// Formula (requires ΣF != 0):
        /// x_R = (Σ F_i * x_i) / (Σ F_i)
        /// y_R = (Σ F_i * y_i) / (Σ F_i)
        /// </summary>
        /// <param name="forces">Forces F_i (may be signed by convention).</param>
        /// <param name="positions">Positions (x_i, y_i) corresponding to each force.</param>
        /// <returns>The resultant location (x_R, y_R).</returns>
        public static (double x, double y) ResultantLocation2D(double[] forces, (double x, double y)[] positions)
        {
            return WeightedCenter2D_SignedWeights(forces, positions, nameof(forces), nameof(positions), "Total force must be non-zero. Resultant location is undefined.");
        }
        /// <summary>
        /// Computes the 3D location of the resultant (equivalent) of parallel forces.
        ///
        /// Formula (requires ΣF != 0):
        /// x_R = (Σ F_i * x_i) / (Σ F_i)
        /// y_R = (Σ F_i * y_i) / (Σ F_i)
        /// z_R = (Σ F_i * z_i) / (Σ F_i)
        /// </summary>
        /// <param name="forces">Forces F_i (may be signed by convention).</param>
        /// <param name="positions">Positions (x_i, y_i, z_i) corresponding to each force.</param>
        /// <returns>The resultant location (x_R, y_R, z_R).</returns>
        public static (double x, double y, double z) ResultantLocation3D(double[] forces, (double x, double y, double z)[] positions)
        {
            return WeightedCenter3D_SignedWeights(forces, positions, nameof(forces), nameof(positions), "Total force must be non-zero. Resultant location is undefined.");
        }
    }
}
