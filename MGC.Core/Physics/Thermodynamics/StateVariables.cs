using System;

namespace MGC.Physics.Thermodynamics
{
    /// <summary>
    /// Provides utility methods for working with thermodynamic state variables.
    ///
    /// This class focuses on core macroscopic state quantities that describe the instantaneous
    /// condition of a thermodynamic system, independent of the path used to reach the state.
    ///
    /// Included state properties and related helpers:
    /// - Density (rho): mass per unit volume
    /// - Specific volume (v): volume per unit mass (inverse of density for positive values)
    /// - Conversions between (mass, volume, density, specific volume)
    /// - Generic helpers for converting between total and specific (per-mass) quantities
    ///
    /// Notes:
    /// - This class intentionally does not implement any equation of state (e.g., ideal gas law).
    ///   Those relations are provided by the IdealGasLaw class.
    /// - Unit conversions (Pa/bar/atm/mmHg and K/°C/°F) are provided by a dedicated conversions class.
    /// </summary>
    public static class StateVariables
    {
        /// <summary>
        /// Calculates density:
        ///     rho = m / V
        ///
        /// Units:
        /// - mass: kg
        /// - volume: m^3
        /// - result density: kg/m^3
        ///
        /// Physical meaning:
        /// - Density represents mass per unit volume.
        /// - It is a fundamental state property used throughout thermodynamics and fluid mechanics.
        /// </summary>
        /// <param name="mass">Mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="volume">Volume in cubic meters (m^3). Must be greater than zero.</param>
        /// <returns>Density in kg/m^3.</returns>
        public static double Density(double mass, double volume)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }

            return mass / volume;
        }

        /// <summary>
        /// Calculates specific volume:
        ///     v = V / m
        ///
        /// Units:
        /// - volume: m^3
        /// - mass: kg
        /// - result specific volume: m^3/kg
        ///
        /// Physical meaning:
        /// - Specific volume is volume per unit mass.
        /// - For positive mass it is the reciprocal of density: v = 1 / rho.
        /// </summary>
        /// <param name="mass">Mass in kilograms (kg). Must be greater than zero.</param>
        /// <param name="volume">Volume in cubic meters (m^3). Must be non-negative.</param>
        /// <returns>Specific volume in m^3/kg.</returns>
        public static double SpecificVolume(double mass, double volume)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            if (volume < 0)
            {
                throw new ArgumentException("Volume must be non-negative.", nameof(volume));
            }

            return volume / mass;
        }

        /// <summary>
        /// Calculates density from specific volume:
        ///     rho = 1 / v
        ///
        /// Units:
        /// - specificVolume: m^3/kg
        /// - result density: kg/m^3
        /// </summary>
        /// <param name="specificVolume">Specific volume v in m^3/kg. Must be greater than zero.</param>
        /// <returns>Density rho in kg/m^3.</returns>
        public static double DensityFromSpecificVolume(double specificVolume)
        {
            if (specificVolume <= 0)
            {
                throw new ArgumentException("Specific volume must be greater than zero.", nameof(specificVolume));
            }

            return 1.0 / specificVolume;
        }

        /// <summary>
        /// Calculates specific volume from density:
        ///     v = 1 / rho
        ///
        /// Units:
        /// - density: kg/m^3
        /// - result specific volume: m^3/kg
        /// </summary>
        /// <param name="density">Density rho in kg/m^3. Must be greater than zero.</param>
        /// <returns>Specific volume v in m^3/kg.</returns>
        public static double SpecificVolumeFromDensity(double density)
        {
            if (density <= 0)
            {
                throw new ArgumentException("Density must be greater than zero.", nameof(density));
            }

            return 1.0 / density;
        }

        /// <summary>
        /// Calculates mass from density and volume:
        ///     m = rho * V
        ///
        /// Units:
        /// - density: kg/m^3
        /// - volume: m^3
        /// - result mass: kg
        /// </summary>
        /// <param name="density">Density rho in kg/m^3. Must be non-negative.</param>
        /// <param name="volume">Volume in m^3. Must be non-negative.</param>
        /// <returns>Mass in kilograms (kg).</returns>
        public static double MassFromDensity(double density, double volume)
        {
            if (density < 0)
            {
                throw new ArgumentException("Density must be non-negative.", nameof(density));
            }
            if (volume < 0)
            {
                throw new ArgumentException("Volume must be non-negative.", nameof(volume));
            }

            return density * volume;
        }

        /// <summary>
        /// Calculates volume from mass and density:
        ///     V = m / rho
        ///
        /// Units:
        /// - mass: kg
        /// - density: kg/m^3
        /// - result volume: m^3
        /// </summary>
        /// <param name="mass">Mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="density">Density rho in kg/m^3. Must be greater than zero.</param>
        /// <returns>Volume in cubic meters (m^3).</returns>
        public static double VolumeFromDensity(double mass, double density)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (density <= 0)
            {
                throw new ArgumentException("Density must be greater than zero.", nameof(density));
            }

            return mass / density;
        }

        /// <summary>
        /// Calculates volume from mass and specific volume:
        ///     V = v * m
        ///
        /// Units:
        /// - mass: kg
        /// - specificVolume: m^3/kg
        /// - result volume: m^3
        /// </summary>
        /// <param name="mass">Mass in kilograms (kg). Must be non-negative.</param>
        /// <param name="specificVolume">Specific volume v in m^3/kg. Must be non-negative.</param>
        /// <returns>Volume in cubic meters (m^3).</returns>
        public static double VolumeFromSpecificVolume(double mass, double specificVolume)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (specificVolume < 0)
            {
                throw new ArgumentException("Specific volume must be non-negative.", nameof(specificVolume));
            }

            return specificVolume * mass;
        }

        /// <summary>
        /// Calculates mass from volume and specific volume:
        ///     m = V / v
        ///
        /// Units:
        /// - volume: m^3
        /// - specificVolume: m^3/kg
        /// - result mass: kg
        /// </summary>
        /// <param name="volume">Volume in cubic meters (m^3). Must be non-negative.</param>
        /// <param name="specificVolume">Specific volume v in m^3/kg. Must be greater than zero.</param>
        /// <returns>Mass in kilograms (kg).</returns>
        public static double MassFromSpecificVolume(double volume, double specificVolume)
        {
            if (volume < 0)
            {
                throw new ArgumentException("Volume must be non-negative.", nameof(volume));
            }
            if (specificVolume <= 0)
            {
                throw new ArgumentException("Specific volume must be greater than zero.", nameof(specificVolume));
            }

            return volume / specificVolume;
        }

        /// <summary>
        /// Converts a total extensive quantity to a specific (per-mass) quantity:
        ///     specific = total / m
        ///
        /// Examples:
        /// - Specific internal energy: u = U / m
        /// - Specific enthalpy:        h = H / m
        /// - Specific entropy:         s = S / m
        ///
        /// Units depend on the given quantity:
        /// - If total is Joules, result is J/kg.
        /// - If total is J/K, result is J/(kg·K).
        /// </summary>
        /// <param name="totalValue">Total (extensive) value.</param>
        /// <param name="mass">Mass in kilograms (kg). Must be greater than zero.</param>
        /// <returns>Specific (per-mass) value.</returns>
        public static double SpecificValue(double totalValue, double mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }

            return totalValue / mass;
        }

        /// <summary>
        /// Converts a specific (per-mass) quantity to a total extensive quantity:
        ///     total = specific * m
        ///
        /// Examples:
        /// - Total internal energy: U = u * m
        /// - Total enthalpy:       H = h * m
        /// - Total entropy:        S = s * m
        /// </summary>
        /// <param name="specificValue">Specific (per-mass) value.</param>
        /// <param name="mass">Mass in kilograms (kg). Must be non-negative.</param>
        /// <returns>Total (extensive) value.</returns>
        public static double TotalFromSpecific(double specificValue, double mass)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }

            return specificValue * mass;
        }
    }
}
