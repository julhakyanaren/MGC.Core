using System;

namespace MGC.Physics.Mechanics.Statics
{
    /// <summary>
    /// Provides practical 1D statics utilities for common beam problems:
    /// - Support reactions for one or two supports under point loads and uniform distributed loads (UDL).
    /// - Converting a uniform distributed load segment into an equivalent point load.
    /// - Internal force resultants at a section: axial normal force N(x), shear force Q(x), and bending moment M(x).
    /// - Building shear and moment diagrams on a user-provided set of x-coordinates.
    ///
    /// Coordinate model:
    /// - The beam axis is along x.
    /// - All positions (supports, loads, diagram points) use the same units (m, cm, etc.).
    ///
    /// Sign convention (recommended and used by these methods):
    /// - Point loads: positive values act downward.
    /// - UDL intensity: positive values act downward.
    /// - Support reactions returned by this class: positive values act upward.
    ///
    /// Section evaluation convention:
    /// - For ShearForceAtX and BendingMomentAtX, a load/support is included only if its position is strictly less than x (pos &lt; x).
    ///   This gives the left-limit value at discontinuities (classic diagram convention).
    /// </summary>
    public static class SupportReactions
    {
        /// <summary>
        /// Computes a single unknown vertical support reaction in 1D using force equilibrium.
        ///
        /// Physical meaning:
        /// - When there is exactly one unknown vertical reaction and all applied loads are vertical,
        ///   the reaction magnitude equals the algebraic sum of applied loads.
        ///
        /// Formula (with the class sign convention):
        /// - ΣFy = 0  =>  R = ΣFi
        ///
        /// Where:
        /// - loads[i] are point loads (positive downward),
        /// - returned R is positive upward.
        /// </summary>
        /// <param name="loads">Array of vertical point loads (positive values act downward).</param>
        /// <returns>Single support reaction (positive upward).</returns>
        public static double SingleSupportReaction1D(double[] loads)
        {
            if (loads == null)
            {
                throw new ArgumentNullException(nameof(loads));
            }

            double netLoad = 0.0;
            for (int l = 0; l < loads.Length; l++)
            {
                netLoad += loads[l];
            }

            return netLoad;
        } 
        /// <summary>
        /// Computes reactions of a simply supported beam with two vertical supports in 1D under point loads.
        ///
        /// Physical meaning:
        /// - Solves reactions (RA, RB) using equilibrium equations:
        ///   ΣFy = 0 and ΣMA = 0.
        ///
        /// Formulas (with the class sign convention):
        /// - ΣFy = 0  =>  RA + RB = ΣFi
        /// - ΣMA = 0  =>  RB * (xB - xA) = Σ( Fi * (xi - xA) )
        /// - RA = ΣFi - RB
        ///
        /// Where:
        /// - Fi are point loads (positive downward),
        /// - RA and RB are reactions (positive upward).
        /// </summary>
        /// <param name="loads">Array of point load magnitudes (positive downward).</param>
        /// <param name="positions">Array of x-positions for each load.</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <returns>Tuple (reactionA, reactionB), each positive upward.</returns>
        public static (double reactionA, double reactionB) TwoSupportReactions1D(
            double[] loads, double[] positions,
            double supportA, double supportB)
        {
            if (loads == null)
            {
                throw new ArgumentNullException(nameof(loads));
            }

            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            if (loads.Length != positions.Length)
            {
                throw new ArgumentException("Loads and Positions must have the same length.");
            }

            if (supportA == supportB)
            {
                throw new ArgumentException("Support positions must be different (supportA != supportB).");
            }

            double netLoad = SingleSupportReaction1D(loads);

            double momentAboutA = 0.0;
            for (int m = 0; m < loads.Length; m++)
            {
                momentAboutA += loads[m] * (positions[m] - supportA);
            }

            double span = supportB - supportA;
            double reactionB = momentAboutA / span;

            double reactionA = netLoad - reactionB;

            return (reactionA, reactionB);
        }
        /// <summary>
        /// Converts a uniform distributed load (UDL) segment into an equivalent point load.
        ///
        /// Physical meaning:
        /// - A uniform load q acting on [start, end] is replaced by a single force equal to the area under q(x),
        ///   applied at the centroid of the segment.
        ///
        /// Formulas:
        /// - F = q * (end - start)
        /// - xF = (start + end) / 2
        ///
        /// Note:
        /// - Returned force keeps the same sign as intensity (positive downward if intensity is positive).
        /// </summary>
        /// <param name="intensity">UDL intensity q (positive downward), units: N/m.</param>
        /// <param name="start">Start x of the UDL segment.</param>
        /// <param name="end">End x of the UDL segment.</param>
        /// <returns>Tuple (force, position) of the equivalent point load.</returns>
        public static (double force, double position) DistributedLoadToPointLoadUniform(
            double intensity,
            double start,
            double end)
        {
            if (end < start)
            {
                throw new ArgumentException("End must be greater than or equal to Start.");
            }

            double length = end - start;
            double force = intensity * length;
            double position = (start + end) * 0.5;

            return (force, position);
        }
        /// <summary>
        /// Computes reactions of a simply supported beam with two vertical supports in 1D
        /// under a combination of point loads and uniform distributed loads (UDL segments).
        ///
        /// Physical meaning:
        /// - Each UDL segment is replaced by an equivalent point load (same total force and centroid position),
        ///   then reactions are found using ΣFy = 0 and ΣMA = 0.
        ///
        /// Formulas:
        /// - For each UDL (q on [a,b]):
        ///   F = q(b-a), xF = (a+b)/2
        /// - ΣFy = 0  =>  RA + RB = ΣF(all point loads and UDL equivalents)
        /// - ΣMA = 0  =>  RB * (xB - xA) = Σ( Fk * (xk - xA) )
        /// - RA = ΣF - RB
        /// </summary>
        /// <param name="pointLoads">Array of point load magnitudes (positive downward).</param>
        /// <param name="pointPositions">Array of x-positions for each point load.</param>
        /// <param name="udlSegments">Array of UDL segments (intensity, start, end).</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <returns>Tuple (reactionA, reactionB), each positive upward.</returns>
        public static (double reactionA, double reactionB) SupportReactionsWithUDL1D(
            double[] pointLoads,
            double[] pointPositions,
            (double intensity, double start, double end)[] udlSegments,
            double supportA,
            double supportB)
        {
            if (pointLoads == null)
            {
                throw new ArgumentNullException(nameof(pointLoads));
            }

            if (pointPositions == null)
            {
                throw new ArgumentNullException(nameof(pointPositions));
            }

            if (udlSegments == null)
            {
                throw new ArgumentNullException(nameof(udlSegments));
            }

            if (pointLoads.Length != pointPositions.Length)
            {
                throw new ArgumentException("PointLoads and PointPositions must have the same length.");
            }

            if (supportA == supportB)
            {
                throw new ArgumentException("Support positions must be different (supportA != supportB).");
            }

            double sumLoads = 0.0;
            double momentAboutA = 0.0;

            for (int i = 0; i < pointLoads.Length; i++)
            {
                double load = pointLoads[i];
                double x = pointPositions[i];

                sumLoads += load;
                momentAboutA += load * (x - supportA);
            }

            for (int j = 0; j < udlSegments.Length; j++)
            {
                double intensity = udlSegments[j].intensity;
                double start = udlSegments[j].start;
                double end = udlSegments[j].end;

                if (end < start)
                {
                    throw new ArgumentException("UDL segment End must be greater than or equal to Start.");
                }

                double length = end - start;
                double eqForce = intensity * length;
                double eqPosition = (start + end) * 0.5;

                sumLoads += eqForce;
                momentAboutA += eqForce * (eqPosition - supportA);
            }

            double span = supportB - supportA;
            double reactionB = momentAboutA / span;

            double reactionA = sumLoads - reactionB;

            return (reactionA, reactionB);
        }

        /// <summary>
        /// Computes axial normal force N(x) at a section x for a 1D axial loading model.
        ///
        /// Physical meaning:
        /// - N(x) is the internal axial resultant that equilibrates the axial forces acting on the left part of the body.
        ///
        /// Recommended sign convention:
        /// - reactionAxial is the axial reaction acting in +X on the left part (positive to +X),
        /// - axialLoads[i] are applied axial forces (positive to +X),
        /// - returned N(x) follows: N(x) = reactionAxial - Σ(axialLoads to the left of x).
        ///
        /// Formula:
        /// - N(x) = Rax - Σ( F_i ), for all i where x_i &lt; x.
        /// </summary>
        /// <param name="x">Section position along the axis.</param>
        /// <param name="reactionAxial">Axial reaction (recommended: positive to +X).</param>
        /// <param name="axialLoads">Array of axial point loads (recommended: positive to +X).</param>
        /// <param name="axialPositions">Array of x-positions for each axial load.</param>
        /// <returns>Axial normal force N(x) with the chosen sign convention.</returns>
        public static double NormalForceAtX(
            double x,
            double reactionAxial,
            double[] axialLoads,
            double[] axialPositions)
        {
            if (axialLoads == null)
            {
                throw new ArgumentNullException(nameof(axialLoads));
            }

            if (axialPositions == null)
            {
                throw new ArgumentNullException(nameof(axialPositions));
            }

            if (axialLoads.Length != axialPositions.Length)
            {
                throw new ArgumentException("Axial loads and axial positions must have the same length.");
            }

            double sumLeft = 0.0;

            for (int i = 0; i < axialLoads.Length; i++)
            {
                if (axialPositions[i] < x)
                {
                    sumLeft += axialLoads[i];
                }
            }

            return reactionAxial - sumLeft;
        }
        /// <summary>
        /// Computes shear force Q(x) at a section x for a beam under vertical point loads and UDL segments.
        ///
        /// Physical meaning:
        /// - Q(x) is the internal transverse resultant that equilibrates the vertical forces on the left side of the section.
        ///
        /// Sign convention used here:
        /// - Point loads and UDL intensity are positive downward.
        /// - Reactions are positive upward.
        /// - Q(x) is computed from the left part of the beam:
        ///   Q(x) = (upward reactions to the left) - (downward loads to the left).
        ///
        /// Section convention:
        /// - A force/support is included only if its position is strictly less than x (pos &lt; x),
        ///   producing the left-limit value at discontinuities.
        ///
        /// UDL resultant to the left of x for a segment [a,b]:
        /// - if x ≤ a: 0
        /// - if a &lt; x &lt; b: q(x-a)
        /// - if x ≥ b: q(b-a)
        /// </summary>
        /// <param name="x">Section position along the beam.</param>
        /// <param name="reactionA">Reaction at support A (positive upward).</param>
        /// <param name="reactionB">Reaction at support B (positive upward).</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <param name="pointLoads">Array of point loads (positive downward).</param>
        /// <param name="pointPositions">Array of x-positions for each point load.</param>
        /// <param name="udlSegments">Array of UDL segments (intensity positive downward).</param>
        /// <returns>Shear force Q(x) at the section (positive upward on the left part).</returns>
        public static double ShearForceAtX(
            double x,
            double reactionA,
            double reactionB,
            double supportA,
            double supportB,
            double[] pointLoads,
            double[] pointPositions,
            (double intensity, double start, double end)[] udlSegments)
        {
            if (pointLoads == null)
            {
                throw new ArgumentNullException(nameof(pointLoads));
            }

            if (pointPositions == null)
            {
                throw new ArgumentNullException(nameof(pointPositions));
            }

            if (udlSegments == null)
            {
                throw new ArgumentNullException(nameof(udlSegments));
            }

            if (pointLoads.Length != pointPositions.Length)
            {
                throw new ArgumentException("PointLoads and PointPositions must have the same length.");
            }

            double shear = 0.0;

            if (supportA < x)
            {
                shear += reactionA;
            }

            if (supportB < x)
            {
                shear += reactionB;
            }

            for (int i = 0; i < pointLoads.Length; i++)
            {
                if (pointPositions[i] < x)
                {
                    shear -= pointLoads[i];
                }
            }

            for (int j = 0; j < udlSegments.Length; j++)
            {
                double intensity = udlSegments[j].intensity;
                double start = udlSegments[j].start;
                double end = udlSegments[j].end;

                if (end < start)
                {
                    throw new ArgumentException("UDL segment End must be greater than or equal to Start.");
                }

                shear -= UdlShearContributionAtX(x, intensity, start, end);
            }

            return shear;
        }
        /// <summary>
        /// Computes bending moment M(x) at a section x for a beam under vertical point loads and UDL segments.
        ///
        /// Physical meaning:
        /// - M(x) is the internal bending resultant that equilibrates moments of external forces acting on the left part.
        ///
        /// Sign convention used here:
        /// - Point loads and UDL intensity are positive downward.
        /// - Reactions are positive upward.
        /// - Positive M(x) corresponds to a sagging moment (classic "smile" shape).
        ///
        /// Section convention:
        /// - A force/support is included only if its position is strictly less than x (pos &lt; x),
        ///   producing the left-limit value at discontinuities.
        ///
        /// UDL moment contribution about the section x for a segment [a,b] (uniform q):
        /// - if x ≤ a: 0
        /// - if a &lt; x &lt; b: q(x-a)^2 / 2
        /// - if x ≥ b: (q(b-a)) * (x - (a+b)/2)
        /// </summary>
        /// <param name="x">Section position along the beam.</param>
        /// <param name="reactionA">Reaction at support A (positive upward).</param>
        /// <param name="reactionB">Reaction at support B (positive upward).</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <param name="pointLoads">Array of point loads (positive downward).</param>
        /// <param name="pointPositions">Array of x-positions for each point load.</param>
        /// <param name="udlSegments">Array of UDL segments (intensity positive downward).</param>
        /// <returns>Bending moment M(x) at the section (sagging positive).</returns>
        public static double BendingMomentAtX(
            double x, double reactionA, double reactionB,
            double supportA, double supportB,
            double[] pointLoads, double[] pointPositions,
            (double intensity, double start, double end)[] udlSegments)
        {
            if (pointLoads == null)
            {
                throw new ArgumentNullException(nameof(pointLoads));
            }

            if (pointPositions == null)
            {
                throw new ArgumentNullException(nameof(pointPositions));
            }

            if (udlSegments == null)
            {
                throw new ArgumentNullException(nameof(udlSegments));
            }

            if (pointLoads.Length != pointPositions.Length)
            {
                throw new ArgumentException("PointLoads and PointPositions must have the same length.");
            }

            double moment = 0.0;

            if (supportA < x)
            {
                moment += reactionA * (x - supportA);
            }

            if (supportB < x)
            {
                moment += reactionB * (x - supportB);
            }

            for (int i = 0; i < pointLoads.Length; i++)
            {
                if (pointPositions[i] < x)
                {
                    moment -= pointLoads[i] * (x - pointPositions[i]);
                }
            }

            for (int j = 0; j < udlSegments.Length; j++)
            {
                double intensity = udlSegments[j].intensity;
                double start = udlSegments[j].start;
                double end = udlSegments[j].end;

                if (end < start)
                {
                    throw new ArgumentException("UDL segment End must be greater than or equal to Start.");
                }

                moment -= UdlMomentContributionAtX(x, intensity, start, end);
            }

            return moment;
        }

        /// <summary>
        /// Builds a shear force diagram Q(x) for a user-provided set of x-coordinates.
        ///
        /// Physical meaning:
        /// - Returns Q values evaluated at each x in the input array.
        /// - Uses the same sign convention and left-limit rule as ShearForceAtX.
        ///
        /// Notes:
        /// - The method does not sort xs; it evaluates in the provided order.
        /// - If xs is empty, an empty array is returned.
        /// </summary>
        /// <param name="xs">Array of x-coordinates where the diagram is evaluated.</param>
        /// <param name="reactionA">Reaction at support A (positive upward).</param>
        /// <param name="reactionB">Reaction at support B (positive upward).</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <param name="pointLoads">Array of point loads (positive downward).</param>
        /// <param name="pointPositions">Array of x-positions for each point load.</param>
        /// <param name="udlSegments">Array of UDL segments (intensity positive downward).</param>
        /// <returns>Array of shear force values Q at each x in xs.</returns>
        public static double[] ShearDiagram(
            double[] xs,
            double reactionA,
            double reactionB,
            double supportA,
            double supportB,
            double[] pointLoads,
            double[] pointPositions,
            (double intensity, double start, double end)[] udlSegments)
        {
            if (xs == null)
            {
                throw new ArgumentNullException(nameof(xs));
            }

            double[] result = new double[xs.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                result[i] = ShearForceAtX(
                    xs[i],
                    reactionA,
                    reactionB,
                    supportA,
                    supportB,
                    pointLoads,
                    pointPositions,
                    udlSegments);
            }

            return result;
        }
        /// <summary>
        /// Builds a bending moment diagram M(x) for a user-provided set of x-coordinates.
        ///
        /// Physical meaning:
        /// - Returns M values evaluated at each x in the input array.
        /// - Uses the same sign convention and left-limit rule as BendingMomentAtX.
        ///
        /// Notes:
        /// - The method does not sort xs; it evaluates in the provided order.
        /// - If xs is empty, an empty array is returned.
        /// </summary>
        /// <param name="xs">Array of x-coordinates where the diagram is evaluated.</param>
        /// <param name="reactionA">Reaction at support A (positive upward).</param>
        /// <param name="reactionB">Reaction at support B (positive upward).</param>
        /// <param name="supportA">x-position of support A.</param>
        /// <param name="supportB">x-position of support B.</param>
        /// <param name="pointLoads">Array of point loads (positive downward).</param>
        /// <param name="pointPositions">Array of x-positions for each point load.</param>
        /// <param name="udlSegments">Array of UDL segments (intensity positive downward).</param>
        /// <returns>Array of bending moment values M at each x in xs.</returns>
        public static double[] MomentDiagram(
            double[] xs,
            double reactionA,
            double reactionB,
            double supportA,
            double supportB,
            double[] pointLoads,
            double[] pointPositions,
            (double intensity, double start, double end)[] udlSegments)
        {
            if (xs == null)
            {
                throw new ArgumentNullException(nameof(xs));
            }

            double[] result = new double[xs.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                result[i] = BendingMomentAtX(
                    xs[i],
                    reactionA,
                    reactionB,
                    supportA,
                    supportB,
                    pointLoads,
                    pointPositions,
                    udlSegments);
            }

            return result;
        }

        private static double UdlShearContributionAtX(double x, double intensity, double start, double end)
        {
            if (x <= start)
            {
                return 0.0;
            }

            if (x >= end)
            {
                return intensity * (end - start);
            }

            return intensity * (x - start);
        }
        private static double UdlMomentContributionAtX(double x, double intensity, double start, double end)
        {
            if (x <= start)
            {
                return 0.0;
            }

            if (x >= end)
            {
                double length = end - start;
                double force = intensity * length;
                double centroid = (start + end) * 0.5;

                return force * (x - centroid);
            }

            double partial = x - start;
            return intensity * partial * partial * 0.5;
        }
    }
}
