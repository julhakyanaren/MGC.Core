using System;

namespace MGC.Physics.Thermodynamics
{
    /// <summary>
    /// Provides methods for calculations based on the ideal gas equation of state.
    ///
    /// Supported forms:
    /// - Mass-based form:
    ///     P * V = m * R_specific * T
    ///
    /// - Molar-based form:
    ///     P * V = n * R * T
    ///     where R = Constants.GasConstant
    ///
    /// This class contains only equation-of-state relations and helpers derived from them.
    /// Unit conversions are intentionally not included here.
    /// </summary>
    public static class IdealGasLaw
    {
        /// <summary>
        /// Calculates pressure of an ideal gas using the mass-based form of the ideal gas equation:
        ///     P * V = m * R_specific * T
        ///
        /// Derived formula:
        ///     P = (m * R_specific * T) / V
        ///
        /// Units:
        /// - mass: kg
        /// - specificGasConstant (R_specific): J/(kg·K)
        /// - temperatureKelvin: K
        /// - volume: m^3
        /// - result pressure: Pa
        /// </summary>
        public static double PressureFromMass(
            double mass, double specificGasConstant,
            double temperatureKelvin, double volume)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be non-negative.", nameof(temperatureKelvin));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }

            return mass * specificGasConstant * temperatureKelvin / volume;
        }
        /// <summary>
        /// Calculates pressure of an ideal gas using the molar form of the ideal gas equation:
        ///     P * V = n * R * T
        ///
        /// Derived formula:
        ///     P = (n * R * T) / V
        ///
        /// Units:
        /// - amountOfSubstance: mol
        /// - temperatureKelvin: K
        /// - volume: m^3
        /// - result pressure: Pa
        /// </summary>
        public static double PressureFromMoles(double amountOfSubstance, double temperatureKelvin, double volume)
        {
            if (amountOfSubstance < 0)
            {
                throw new ArgumentException("Amount of substance must be non-negative.", nameof(amountOfSubstance));
            }
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be non-negative.", nameof(temperatureKelvin));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }

            return amountOfSubstance * PhysicConstants.GasConstant * temperatureKelvin / volume;
        }

        /// <summary>
        /// Calculates absolute temperature of an ideal gas using the mass-based form:
        ///     P * V = m * R_specific * T
        ///
        /// Derived formula:
        ///     T = (P * V) / (m * R_specific)
        ///
        /// Units:
        /// - pressure: Pa (absolute pressure)
        /// - volume: m^3
        /// - mass: kg
        /// - specificGasConstant: J/(kg·K)
        /// - result temperature: K
        /// </summary>
        public static double TemperatureFromMass(double pressure, double volume, double mass, double specificGasConstant)
        {
            if (pressure <= 0)
            {
                throw new ArgumentException("Pressure must be greater than zero.", nameof(pressure));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }

            return pressure * volume / (mass * specificGasConstant);
        }
        /// <summary>
        /// Calculates absolute temperature of an ideal gas using the molar form:
        ///     P * V = n * R * T
        ///
        /// Derived formula:
        ///     T = (P * V) / (n * R)
        ///
        /// Units:
        /// - pressure: Pa (absolute pressure)
        /// - volume: m^3
        /// - amountOfSubstance: mol
        /// - R: J/(mol·K) (Constants.GasConstant)
        /// - result temperature: K
        /// </summary>
        public static double TemperatureFromMoles(double pressure, double volume, double amountOfSubstance)
        {
            if (pressure <= 0)
            {
                throw new ArgumentException("Pressure must be greater than zero.", nameof(pressure));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }
            if (amountOfSubstance <= 0)
            {
                throw new ArgumentException("Amount of substance must be greater than zero.", nameof(amountOfSubstance));
            }

            return pressure * volume / (amountOfSubstance * PhysicConstants.GasConstant);
        }

        /// <summary>
        /// Calculates volume of an ideal gas using the mass-based form:
        ///     P * V = m * R_specific * T
        ///
        /// Derived formula:
        ///     V = (m * R_specific * T) / P
        ///
        /// Units:
        /// - mass: kg
        /// - specificGasConstant: J/(kg·K)
        /// - temperatureKelvin: K
        /// - pressure: Pa (absolute pressure)
        /// - result volume: m^3
        /// </summary>
        public static double VolumeFromMass(double mass, double specificGasConstant, double temperatureKelvin, double pressure)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be non-negative.", nameof(temperatureKelvin));
            }
            if (pressure <= 0)
            {
                throw new ArgumentException("Pressure must be greater than zero.", nameof(pressure));
            }

            return mass * specificGasConstant * temperatureKelvin / pressure;
        }
        /// <summary>
        /// Calculates volume of an ideal gas using the molar form:
        ///     P * V = n * R * T
        ///
        /// Derived formula:
        ///     V = (n * R * T) / P
        ///
        /// Units:
        /// - amountOfSubstance: mol
        /// - temperatureKelvin: K
        /// - pressure: Pa (absolute pressure)
        /// - result volume: m^3
        /// </summary>
        public static double VolumeFromMoles(double amountOfSubstance, double temperatureKelvin, double pressure)
        {
            if (amountOfSubstance <= 0)
            {
                throw new ArgumentException("Amount of substance must be greater than zero.", nameof(amountOfSubstance));
            }
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be non-negative.", nameof(temperatureKelvin));
            }
            if (pressure <= 0)
            {
                throw new ArgumentException("Pressure must be greater than zero.", nameof(pressure));
            }

            return amountOfSubstance * PhysicConstants.GasConstant * temperatureKelvin / pressure;
        }

        /// <summary>
        /// Calculates amount of substance (moles) from an ideal gas state:
        ///     P * V = n * R * T
        ///
        /// Derived formula:
        ///     n = (P * V) / (R * T)
        ///
        /// Units:
        /// - pressure: Pa
        /// - volume: m^3
        /// - temperatureKelvin: K
        /// - result: mol
        /// </summary>
        public static double MolesFromState(double pressure, double volume, double temperatureKelvin)
        {
            if (pressure < 0)
            {
                throw new ArgumentException("Pressure must be non-negative.", nameof(pressure));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }
            if (temperatureKelvin <= 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be greater than zero.", nameof(temperatureKelvin));
            }

            return pressure * volume / (PhysicConstants.GasConstant * temperatureKelvin);
        }
        /// <summary>
        /// Calculates gas mass from an ideal gas state using the mass-based form:
        ///     P * V = m * R_specific * T
        ///
        /// Derived formula:
        ///     m = (P * V) / (R_specific * T)
        ///
        /// Units:
        /// - pressure: Pa
        /// - volume: m^3
        /// - specificGasConstant: J/(kg·K)
        /// - temperatureKelvin: K
        /// - result: kg
        /// </summary>
        public static double MassFromState(double pressure, double volume, double specificGasConstant, double temperatureKelvin)
        {
            if (pressure < 0)
            {
                throw new ArgumentException("Pressure must be non-negative.", nameof(pressure));
            }
            if (volume <= 0)
            {
                throw new ArgumentException("Volume must be greater than zero.", nameof(volume));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }
            if (temperatureKelvin <= 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be greater than zero.", nameof(temperatureKelvin));
            }

            return pressure * volume / (specificGasConstant * temperatureKelvin);
        }

        /// <summary>
        /// Calculates amount of substance in moles from gas mass and molar mass:
        ///     n = m / M
        ///
        /// Units:
        /// - mass: kg
        /// - molarMass: kg/mol
        /// - result: mol
        /// </summary>
        public static double MolesFromMass(double mass, double molarMass)
        {
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non-negative.", nameof(mass));
            }
            if (molarMass <= 0)
            {
                throw new ArgumentException("Molar mass must be greater than zero.", nameof(molarMass));
            }

            return mass / molarMass;
        }
        /// <summary>
        /// Calculates gas mass from amount of substance in moles and molar mass:
        ///     m = n * M
        ///
        /// Units:
        /// - amountOfSubstance: mol
        /// - molarMass: kg/mol
        /// - result: kg
        /// </summary>
        public static double MassFromMoles(double amountOfSubstance, double molarMass)
        {
            if (amountOfSubstance < 0)
            {
                throw new ArgumentException("Amount of substance must be non-negative.", nameof(amountOfSubstance));
            }
            if (molarMass <= 0)
            {
                throw new ArgumentException("Molar mass must be greater than zero.", nameof(molarMass));
            }

            return amountOfSubstance * molarMass;
        }

        /// <summary>
        /// Calculates specific gas constant from molar mass:
        ///     R_specific = R / M
        ///
        /// Units:
        /// - molarMass: kg/mol
        /// - result: J/(kg·K)
        /// </summary>
        public static double SpecificGasConstantFromMolarMass(double molarMass)
        {
            if (molarMass <= 0)
            {
                throw new ArgumentException("Molar mass must be greater than zero.", nameof(molarMass));
            }

            return PhysicConstants.GasConstant / molarMass;
        }

        /// <summary>
        /// Calculates density of an ideal gas from state variables:
        ///     P = rho * R_specific * T
        ///
        /// Derived formula:
        ///     rho = P / (R_specific * T)
        ///
        /// Units:
        /// - pressure: Pa
        /// - specificGasConstant: J/(kg·K)
        /// - temperatureKelvin: K
        /// - result density: kg/m^3
        /// </summary>
        public static double DensityFromState(double pressure, double specificGasConstant, double temperatureKelvin)
        {
            if (pressure < 0)
            {
                throw new ArgumentException("Pressure must be non-negative.", nameof(pressure));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }
            if (temperatureKelvin <= 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be greater than zero.", nameof(temperatureKelvin));
            }

            return pressure / (specificGasConstant * temperatureKelvin);
        }

        /// <summary>
        /// Calculates pressure of an ideal gas from density:
        ///     P = rho * R_specific * T
        ///
        /// Units:
        /// - density: kg/m^3
        /// - specificGasConstant: J/(kg·K)
        /// - temperatureKelvin: K
        /// - result pressure: Pa
        /// </summary>
        public static double PressureFromDensity(double density, double specificGasConstant, double temperatureKelvin)
        {
            if (density < 0)
            {
                throw new ArgumentException("Density must be non-negative.", nameof(density));
            }
            if (specificGasConstant <= 0)
            {
                throw new ArgumentException("Specific gas constant must be greater than zero.", nameof(specificGasConstant));
            }
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvins must be non-negative.", nameof(temperatureKelvin));
            }

            return density * specificGasConstant * temperatureKelvin;
        }
    }
}
