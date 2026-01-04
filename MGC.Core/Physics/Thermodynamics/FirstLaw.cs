namespace MGC.Physics.Thermodynamics
{
    /// <summary>
    /// Provides utility methods based on the First Law of Thermodynamics
    /// for energy balance calculations in closed thermodynamic systems.
    ///
    /// The First Law of Thermodynamics expresses the conservation of energy
    /// for a system in the form:
    ///     ΔU = Q − W
    ///
    /// where:
    /// - ΔU is the change in internal energy of the system,
    /// - Q is the heat transferred to the system,
    /// - W is the work performed by the system on its surroundings.
    ///
    /// This class operates on scalar energy values and does not depend on
    /// any specific substance model, equation of state, or thermodynamic process.
    ///
    /// Sign convention used throughout this class:
    /// - Q > 0 : heat is supplied to the system
    /// - W > 0 : work is performed by the system
    ///
    /// All values are assumed to be expressed in consistent energy units
    /// (typically Joules).
    /// </summary>
    public static class FirstLaw
    {
        /// <summary>
        /// Calculates the change in internal energy of a system
        /// from the supplied heat and the work performed by the system.
        ///
        /// Formula:
        ///     ΔU = Q − W
        /// </summary>
        /// <param name="heat">
        /// Heat transferred to the system.
        /// Positive if heat is supplied to the system.
        /// </param>
        /// <param name="work">
        /// Work performed by the system on its surroundings.
        /// Positive if work is done by the system.
        /// </param>
        /// <returns>
        /// Change in internal energy of the system.
        /// </returns>
        public static double InternalEnergyChange(double heat, double work)
        {
            return heat - work;
        }
        /// <summary>
        /// Calculates the amount of heat transferred to a system
        /// from a known change in internal energy and performed work.
        ///
        /// Formula:
        ///     Q = ΔU + W
        /// </summary>
        /// <param name="internalEnergyChange">
        /// Change in internal energy of the system.
        /// </param>
        /// <param name="work">
        /// Work performed by the system on its surroundings.
        /// </param>
        /// <returns>
        /// Heat transferred to the system.
        /// </returns>
        public static double HeatFromEnergyBalance(double internalEnergyChange, double work)
        {
            return internalEnergyChange + work;
        }
        /// <summary>
        /// Calculates the work performed by a system
        /// from a known heat transfer and change in internal energy.
        ///
        /// Formula:
        ///     W = Q − ΔU
        /// </summary>
        /// <param name="internalEnergyChange">
        /// Change in internal energy of the system.
        /// </param>
        /// <param name="heat">
        /// Heat transferred to the system.
        /// </param>
        /// <returns>
        /// Work performed by the system on its surroundings.
        /// </returns>
        public static double WorkFromEnergyBalance(double internalEnergyChange, double heat)
        {
            return heat - internalEnergyChange;
        }

        /// <summary>
        /// Computes the energy balance residual for a closed system
        /// using absolute values of internal energy.
        ///
        /// The method evaluates the difference between:
        /// - the change in internal energy of the system, and
        /// - the net energy transfer by heat and work.
        ///
        /// Formula:
        ///     (U₂ − U₁) − (Q − W)
        /// </summary>
        /// <param name="initialInternalEnergy">
        /// Initial internal energy of the system.
        /// </param>
        /// <param name="finalInternalEnergy">
        /// Final internal energy of the system.
        /// </param>
        /// <param name="heat">
        /// Heat transferred to the system.
        /// </param>
        /// <param name="work">
        /// Work performed by the system on its surroundings.
        /// </param>
        /// <returns>
        /// Energy balance residual.
        /// A value of zero indicates perfect energy balance.
        /// </returns>
        public static double ClosedSystemEnergyBalance(
            double initialInternalEnergy, double finalInternalEnergy,
            double heat, double work)
        {
            double balanceFirst = finalInternalEnergy - initialInternalEnergy;
            double balanceSecond = heat - work;

            return balanceFirst - balanceSecond;
        }
        /// <summary>
        /// Determines whether the energy balance of a closed system
        /// satisfies the First Law of Thermodynamics within a given tolerance.
        /// </summary>
        /// <param name="initialInternalEnergy">
        /// Initial internal energy of the system.
        /// </param>
        /// <param name="finalInternalEnergy">
        /// Final internal energy of the system.
        /// </param>
        /// <param name="heat">
        /// Heat transferred to the system.
        /// </param>
        /// <param name="work">
        /// Work performed by the system on its surroundings.
        /// </param>
        /// <param name="tolerance">
        /// Numerical tolerance used to account for floating-point inaccuracies.
        /// </param>
        /// <returns>
        /// True if the energy balance residual is within the specified tolerance;
        /// otherwise, false.
        /// </returns>
        public static bool IsEnergyBalanced(
            double initialInternalEnergy, double finalInternalEnergy,
            double heat, double work,
            double tolerance = 1e-9)
        {
            return Math.Abs(ClosedSystemEnergyBalance(
                initialInternalEnergy, finalInternalEnergy,
                heat, work)) <= tolerance;
        }
    }
}
