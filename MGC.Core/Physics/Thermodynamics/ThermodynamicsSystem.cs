using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGC.Physics.Thermodynamics
{
    /// <summary>
    /// Provides core thermodynamics definitions and helper utilities for working with
    /// thermodynamic systems and idealized processes.
    ///
    /// This class is intended to be a lightweight "foundation" for the Thermodynamics module.
    /// It defines:
    /// - System types (open / closed / isolated) based on whether mass and/or energy can cross the boundary.
    /// - Process types (isothermal / isobaric / isochoric / adiabatic) based on the constrained property.
    /// - Utility checks that can be used by other classes (First Law, Heat, Work, Entropy) to validate
    ///   assumptions and select appropriate equations.
    ///
    /// Notes:
    /// - "Energy exchange" here means any energy transfer across the boundary (heat and/or work).
    /// - An isolated system exchanges neither mass nor energy with its surroundings.
    /// - Process constraints are checked approximately using a tolerance, because in practical usage
    ///   values are often floating-point and measured/derived.
    /// </summary>
    public static class ThermodynamicsSystem
    {
        /// <summary>
        /// Describes how a thermodynamic system can exchange mass and energy with its surroundings.
        /// </summary>
        public enum SystemType
        {
            /// <summary>
            /// Open system: mass and energy can cross the boundary.
            /// Examples: turbines, compressors, nozzles, open tanks with inflow/outflow.
            /// </summary>
            Open = 0,

            /// <summary>
            /// Closed system: no mass crosses the boundary, but energy (heat/work) can.
            /// Examples: sealed piston-cylinder (no leakage), rigid sealed container with heating.
            /// </summary>
            Closed = 1,
            
            /// <summary>
            /// Isolated system: neither mass nor energy crosses the boundary.
            /// This is an idealization used in many theoretical problems.
            /// </summary>
            Isolated = 2
        }

        /// <summary>
        /// Describes an idealized thermodynamic process defined by a constrained property.
        /// </summary>
        public enum ProcessType
        {
            /// <summary>
            /// Isothermal process: temperature is constant (T = const).
            /// </summary>
            Isothermal = 0,

            /// <summary>
            /// Isobaric process: pressure is constant (p = const).
            /// </summary>
            Isobaric = 1,

            /// <summary>
            /// Isochoric process: volume is constant (V = const).
            /// </summary>
            Isochoric = 2,

            /// <summary>
            /// Adiabatic process: no heat transfer (Q = 0).
            /// Note: this method checks only p/V/T constraints; Q is not part of the signature.
            /// For adiabatic validation in computations, use energy/heat balance in the First Law.
            /// </summary>
            Adiabatic = 3
        }

        /// <summary>
        /// Returns true if the specified system type allows mass exchange across the system boundary.
        /// </summary>
        /// <param name="type">Thermodynamic system type.</param>
        public static bool IsMassExchangeAllowed(SystemType type)
        {
            if (type == SystemType.Open)
            {
                return true;
            }
            if (type == SystemType.Closed)
            {
                return false;
            }
            if (type == SystemType.Isolated)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Unknown SystemType value.");
        }
        /// <summary>
        /// Returns true if the specified system type allows energy exchange (heat and/or work)
        /// across the system boundary.
        /// </summary>
        /// <param name="type">Thermodynamic system type.</param>
        public static bool IsEnergyExchangeAllowed(SystemType type)
        {
            if (type == SystemType.Open)
            {
                return true;
            }
            if (type == SystemType.Closed)
            {
                return true;
            }
            if (type == SystemType.Isolated)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Unknown SystemType value.");
        }
        /// <summary>
        /// Returns true if the specified system type allows heat exchange across the system boundary.
        /// </summary>
        /// <param name="type">Thermodynamic system type.</param>
        public static bool IsHeatExchangeAllowed(SystemType type)
        {
            if (type == SystemType.Open)
            {
                return true;
            }
            if (type == SystemType.Closed)
            {
                return true;
            }
            if (type == SystemType.Isolated)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Unknown SystemType value.");
        }
        /// <summary>
        /// Returns true if the specified system type allows work exchange across the system boundary.
        /// </summary>
        /// <param name="type">Thermodynamic system type.</param>
        public static bool IsWorkExchangeAllowed(SystemType type)
        {
            if (type == SystemType.Open)
            {
                return true;
            }
            if (type == SystemType.Closed)
            {
                return true;
            }
            if (type == SystemType.Isolated)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Unknown SystemType value.");
        }
        /// <summary>
        /// Checks whether an idealized process constraint is satisfied approximately between two states.
        ///
        /// This is a lightweight validation helper used to confirm that inputs match an intended
        /// process type:
        /// - Isothermal: T1 ≈ T2
        /// - Isobaric:   p1 ≈ p2
        /// - Isochoric:  V1 ≈ V2
        /// - Adiabatic:  no direct p/V/T constraint is checked here (returns true).
        ///
        /// Important:
        /// - For Adiabatic processes, the defining condition is Q = 0 (no heat transfer), which is not
        ///   represented in this method signature. Use the First Law and heat/work computations to validate it.
        /// </summary>
        /// <param name="type">Idealized process type.</param>
        /// <param name="p1">Pressure in state 1.</param>
        /// <param name="p2">Pressure in state 2.</param>
        /// <param name="v1">Volume in state 1.</param>
        /// <param name="v2">Volume in state 2.</param>
        /// <param name="t1">Temperature in state 1.</param>
        /// <param name="t2">Temperature in state 2.</param>
        /// <param name="tolerance">Absolute tolerance used for approximate equality checks (must be non-negative).</param>
        public static bool IsProcessConstraintSatisfied(ProcessType type,
            double p1, double p2,
            double v1, double v2,
            double t1, double t2,
            double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("Tolerance must be non-negative.", nameof(tolerance));
            }

            if (type == ProcessType.Isothermal)
            {
                return ApproximatelyEqual(t1, t2, tolerance);
            }
            if (type == ProcessType.Isobaric)
            {
                return ApproximatelyEqual(p1, p2, tolerance);
            }
            if (type == ProcessType.Isochoric)
            {
                return ApproximatelyEqual(v1, v2, tolerance);
            }
            if (type == ProcessType.Adiabatic)
            {
                return true;
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Unknown ProcessType value.");
        }
        /// <summary>
        /// Checks if the system can be considered in approximate equilibrium based on rates of change.
        ///
        /// A common practical criterion for "quasi-equilibrium" is that state variables change slowly:
        /// |dT/dt|, |dp/dt|, |dV/dt| are all below a small tolerance.
        ///
        /// This is useful when you have derivative-like values from simulation or sampling.
        /// </summary>
        /// <param name="dTdt">Rate of temperature change (dT/dt).</param>
        /// <param name="dpdt">Rate of pressure change (dp/dt).</param>
        /// <param name="dVdt">Rate of volume change (dV/dt).</param>
        /// <param name="tolerance">Non-negative threshold for each absolute rate.</param>
        public static bool IsEquilibriumByRates(double dTdt, double dpdt, double dVdt, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("Tolerance must be non-negative.", nameof(tolerance));
            }

            if (global::System.Math.Abs(dTdt) > tolerance)
            {
                return false;
            }
            if (global::System.Math.Abs(dpdt) > tolerance)
            {
                return false;
            }
            if (global::System.Math.Abs(dVdt) > tolerance)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Checks if the system can be considered in approximate equilibrium based on a spread of measurements.
        ///
        /// This method is intended for scenarios where you have multiple temperature samples from different
        /// points in a system. A simple equilibrium criterion is:
        /// max(temperatures) - min(temperatures) <= tolerance
        ///
        /// If the spread is small, the system can be treated as approximately isothermal internally.
        /// </summary>
        /// <param name="temperatures">Array of temperature samples.</param>
        /// <param name="tolerance">Non-negative allowed spread (max - min).</param>
        public static bool IsEquilibriumBySpread(double[] temperatures, double tolerance)
        {
            if (temperatures == null)
            {
                throw new ArgumentNullException(nameof(temperatures));
            }
            if (temperatures.Length == 0)
            {
                throw new ArgumentException("Temperatures array must not be empty.", nameof(temperatures));
            }
            if (tolerance < 0)
            {
                throw new ArgumentException("Tolerance must be non-negative.", nameof(tolerance));
            }

            double min = temperatures[0];
            double max = temperatures[0];

            for (int i = 1; i < temperatures.Length; i++)
            {
                double t = temperatures[i];

                if (t < min)
                {
                    min = t;
                }
                if (t > max)
                {
                    max = t;
                }
            }

            return (max - min) <= tolerance;
        }

        private static bool ApproximatelyEqual(double a, double b, double tolerance)
        {
            return global::System.Math.Abs(a - b) <= tolerance;
        }
    }
}
