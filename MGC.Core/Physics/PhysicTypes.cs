using System;
namespace MGC.Physics
{
    public static class PhysicTypes
    {
        /// <summary>
        /// Specifies the temperature scale used to represent a thermodynamic temperature value.
        ///
        /// This enumeration defines the most commonly used temperature scales in physics and engineering:
        /// Kelvin, Celsius, and Fahrenheit.
        ///
        /// Important notes:
        /// - Kelvin is an absolute temperature scale used in all thermodynamic equations.
        /// - Celsius and Fahrenheit are relative scales derived from reference points
        ///   (water freezing/boiling for Celsius, historical reference points for Fahrenheit).
        /// - Internally, thermodynamic calculations should always be performed in Kelvin.
        ///
        /// Physical background:
        /// - Absolute zero corresponds to 0 K, -273.15 °C, and -459.67 °F.
        /// - Temperatures below absolute zero are physically impossible.
        ///
        /// This enumeration is typically used for:
        /// - Unit conversion
        /// - User input/output
        /// - Formatting and validation of temperature values
        /// </summary>
        public enum TemperatureType
        {
            /// <summary>
            /// Absolute thermodynamic temperature scale.
            ///
            /// Kelvin is the base unit of temperature in the International System of Units (SI).
            /// Zero Kelvin (0 K) represents absolute zero, the theoretical state in which
            /// all thermal motion of particles ceases.
            ///
            /// This scale must be used for all thermodynamic equations,
            /// including ideal gas law, entropy, and energy relations.
            /// </summary>
            Kelvin = 0,

            /// <summary>
            /// Celsius temperature scale.
            ///
            /// This scale is defined relative to the phase transitions of water:
            /// - 0 °C corresponds to the freezing point of water at standard pressure.
            /// - 100 °C corresponds to the boiling point of water at standard pressure.
            ///
            /// The Celsius scale is offset from the Kelvin scale by 273.15:
            ///     T(K) = T(°C) + 273.15
            ///
            /// Celsius values are commonly used for presentation and human interaction,
            /// but must be converted to Kelvin for thermodynamic calculations.
            /// </summary>
            Celsius = 1,

            /// <summary>
            /// Fahrenheit temperature scale.
            ///
            /// This scale is primarily used in the United States and a few other regions.
            /// It is based on historical reference points and is not directly tied to SI units.
            ///
            /// Conversion relationships:
            /// - T(°C) = (T(°F) - 32) × 5 / 9
            /// - T(K)  = (T(°F) - 32) × 5 / 9 + 273.15
            ///
            /// Like Celsius, Fahrenheit is intended for input/output purposes only
            /// and must be converted to Kelvin before performing thermodynamic calculations.
            /// </summary>
            Fahrenheit = 2
        }

        /// <summary>
        /// Specifies the pressure unit used to represent a thermodynamic pressure value.
        ///
        /// This enumeration defines commonly used pressure units in physics and engineering:
        /// Pascal (SI), Bar, Standard atmosphere, and millimeter of mercury (mmHg).
        ///
        /// Important notes:
        /// - Pascal (Pa) is the base SI unit of pressure and should be used as the internal
        ///   unit for calculations to avoid conversion errors.
        /// - Bar and atmosphere are commonly used in engineering and thermodynamics.
        /// - Millimeter of mercury (mmHg) is widely used in medicine and vacuum engineering.
        ///
        /// Reference relationships (approximate unless exact constants are used):
        /// - 1 bar = 100000 Pa
        /// - 1 atm = 101325 Pa
        /// - 1 mmHg ≈ 133.322387415 Pa (standard definition)
        ///
        /// This enumeration is typically used for:
        /// - Unit conversion
        /// - Input/output formatting
        /// - Validation and normalization of pressure values
        /// </summary>
        public enum PressureType
        {
            /// <summary>
            /// Pascal (Pa).
            ///
            /// The SI unit of pressure, defined as one Newton per square meter:
            ///     1 Pa = 1 N/m^2
            ///
            /// Pascal is the recommended internal unit for all thermodynamic and physical calculations.
            /// </summary>
            Pascal = 0,

            /// <summary>
            /// Bar (bar).
            ///
            /// A practical engineering unit of pressure:
            ///     1 bar = 100000 Pa
            ///
            /// Commonly used in engineering, meteorology, and thermodynamic problems.
            /// </summary>
            Bar = 1,

            /// <summary>
            /// Standard atmosphere (atm).
            ///
            /// Represents the standard pressure at sea level:
            ///     1 atm = 101325 Pa
            ///
            /// Frequently used for "standard conditions" and reference calculations in thermodynamics.
            /// </summary>
            Atmosphere = 2,

            /// <summary>
            /// Millimeter of mercury (mmHg).
            ///
            /// A unit based on the hydrostatic pressure of a mercury column:
            ///     1 mmHg ≈ 133.322387415 Pa
            ///
            /// Widely used in medicine (blood pressure) and vacuum engineering.
            /// </summary>
            MillimeterOfMercury = 3
        }
    }
}
