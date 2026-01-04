using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGC.Physics
{
    public static class PhysicConvert
    {
        /// <summary>
        /// Converts pressure from bar to Pascal (Pa).
        ///
        /// Relationship:
        /// - 1 bar = Constants.Bar Pa
        /// </summary>
        /// <param name="pressureBar">Pressure in bar.</param>
        /// <returns>Pressure in Pascals (Pa).</returns>
        public static double BarToPascal(double pressureBar)
        {
            return pressureBar * PhysicConstants.Bar;
        }
        /// <summary>
        /// Converts pressure from Pascal (Pa) to bar.
        ///
        /// Relationship:
        /// - 1 bar = Constants.Bar Pa
        /// </summary>
        /// <param name="pressurePascal">Pressure in Pascals (Pa).</param>
        /// <returns>Pressure in bar.</returns>
        public static double PascalToBar(double pressurePascal)
        {
            return pressurePascal / PhysicConstants.Bar;
        }

        /// <summary>
        /// Converts pressure from standard atmosphere (atm) to Pascal (Pa).
        ///
        /// Relationship:
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// </summary>
        /// <param name="pressureAtm">Pressure in atmospheres (atm).</param>
        /// <returns>Pressure in Pascals (Pa).</returns>
        public static double AtmosphereToPascal(double pressureAtm)
        {
            return pressureAtm * PhysicConstants.StandardAtmosphere;
        }
        /// <summary>
        /// Converts pressure from Pascal (Pa) to standard atmosphere (atm).
        ///
        /// Relationship:
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// </summary>
        /// <param name="pressurePascal">Pressure in Pascals (Pa).</param>
        /// <returns>Pressure in atmospheres (atm).</returns>
        public static double PascalToAtmosphere(double pressurePascal)
        {
            return pressurePascal / PhysicConstants.StandardAtmosphere;
        }

        /// <summary>
        /// Converts pressure from standard atmosphere (atm) to bar.
        ///
        /// Relationship:
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// - 1 bar = Constants.Bar Pa
        /// </summary>
        /// <param name="pressureAtm">Pressure in atmospheres (atm).</param>
        /// <returns>Pressure in bar.</returns>
        public static double AtmosphereToBar(double pressureAtm)
        {
            return pressureAtm * PhysicConstants.StandardAtmosphere / PhysicConstants.Bar;
        }
        /// <summary>
        /// Converts pressure from bar to standard atmosphere (atm).
        ///
        /// Relationship:
        /// - 1 bar = Constants.Bar Pa
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// </summary>
        /// <param name="pressureBar">Pressure in bar.</param>
        /// <returns>Pressure in atmospheres (atm).</returns>
        public static double BarToAtmosphere(double pressureBar)
        {
            return pressureBar * PhysicConstants.Bar / PhysicConstants.StandardAtmosphere;
        }

        /// <summary>
        /// Converts pressure from millimeter of mercury (mmHg) to Pascal (Pa).
        ///
        /// Relationship:
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        ///
        /// Physical note:
        /// - mmHg is widely used in medicine (blood pressure) and vacuum engineering.
        /// </summary>
        /// <param name="pressureMmHg">Pressure in millimeters of mercury (mmHg).</param>
        /// <returns>Pressure in Pascals (Pa).</returns>
        public static double MillimeterOfMercuryToPascal(double pressureMmHg)
        {
            return pressureMmHg * PhysicConstants.MillimeterOfMercury;
        }
        /// <summary>
        /// Converts pressure from Pascal (Pa) to millimeter of mercury (mmHg).
        ///
        /// Relationship:
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        /// </summary>
        /// <param name="pressurePascal">Pressure in Pascals (Pa).</param>
        /// <returns>Pressure in millimeters of mercury (mmHg).</returns>
        public static double PascalToMillimeterOfMercury(double pressurePascal)
        {
            return pressurePascal / PhysicConstants.MillimeterOfMercury;
        }

        /// <summary>
        /// Converts pressure from standard atmosphere (atm) to millimeter of mercury (mmHg).
        ///
        /// Relationship:
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        ///
        /// Reference:
        /// - Approximately, 1 atm ≈ 760 mmHg.
        /// </summary>
        /// <param name="pressureAtm">Pressure in atmospheres (atm).</param>
        /// <returns>Pressure in millimeters of mercury (mmHg).</returns>
        public static double AtmosphereToMillimeterOfMercury(double pressureAtm)
        {
            return pressureAtm * PhysicConstants.StandardAtmosphere / PhysicConstants.MillimeterOfMercury;
        }
        /// <summary>
        /// Converts pressure from millimeter of mercury (mmHg) to standard atmosphere (atm).
        ///
        /// Relationship:
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        /// - 1 atm = Constants.StandardAtmosphere Pa
        /// </summary>
        /// <param name="pressureMmHg">Pressure in millimeters of mercury (mmHg).</param>
        /// <returns>Pressure in atmospheres (atm).</returns>
        public static double MillimeterOfMercuryToAtmosphere(double pressureMmHg)
        {
            return pressureMmHg * PhysicConstants.MillimeterOfMercury / PhysicConstants.StandardAtmosphere;
        }

        /// <summary>
        /// Converts pressure from bar to millimeter of mercury (mmHg).
        ///
        /// Relationship:
        /// - 1 bar = Constants.Bar Pa
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        /// </summary>
        /// <param name="pressureBar">Pressure in bar.</param>
        /// <returns>Pressure in millimeters of mercury (mmHg).</returns>
        public static double BarToMillimeterOfMercury(double pressureBar)
        {
            return pressureBar * PhysicConstants.Bar / PhysicConstants.MillimeterOfMercury;
        }
        /// <summary>
        /// Converts pressure from millimeter of mercury (mmHg) to bar.
        ///
        /// Relationship:
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        /// - 1 bar = Constants.Bar Pa
        /// </summary>
        /// <param name="pressureMmHg">Pressure in millimeters of mercury (mmHg).</param>
        /// <returns>Pressure in bar.</returns>
        public static double MillimeterOfMercuryToBar(double pressureMmHg)
        {
            return pressureMmHg * PhysicConstants.MillimeterOfMercury / PhysicConstants.Bar;
        }

        /// <summary>
        /// Converts temperature from Kelvin (K) to Celsius (°C).
        ///
        /// Relationship:
        /// - T(°C) = T(K) - 273.15
        ///
        /// Physical note:
        /// - Absolute zero corresponds to 0 K = -273.15 °C.
        /// </summary>
        /// <param name="temperatureKelvin">Temperature in Kelvin (K).</param>
        /// <returns>Temperature in Celsius (°C).</returns>
        public static double KelvinToCelsius(double temperatureKelvin)
        {
            if (temperatureKelvin < -273.15)
            {
                throw new ArgumentException("The temperature in Kelvin cannot be lower than -273.15.", nameof(temperatureKelvin));
            }
            return temperatureKelvin - 273.15;
        }
        /// <summary>
        /// Converts temperature from Celsius (°C) to Kelvin (K).
        ///
        /// Relationship:
        /// - T(K) = T(°C) + 273.15
        ///
        /// Physical note:
        /// - Temperatures below -273.15 °C are physically impossible (below absolute zero).
        /// </summary>
        /// <param name="temperatureCelsius">Temperature in Celsius (°C).</param>
        /// <returns>Temperature in Kelvin (K).</returns>
        public static double CelsiusToKelvin(double temperatureCelsius)
        {
            if (temperatureCelsius < -273.15)
            {
                throw new ArgumentException("Temperature in Celsius cannot be lower than absolute zero (-273.15 °C).",
                    nameof(temperatureCelsius));
            }

            return temperatureCelsius + 273.15;
        }

        /// <summary>
        /// Converts temperature from Fahrenheit (°F) to Celsius (°C).
        ///
        /// Relationship:
        /// - T(°C) = (T(°F) - 32) * 5 / 9
        ///
        /// This method is primarily intended for input/output conversion.
        /// </summary>
        /// <param name="temperatureFahrenheit">Temperature in Fahrenheit (°F).</param>
        /// <returns>Temperature in Celsius (°C).</returns>
        public static double FahrenheitToCelsius(double temperatureFahrenheit)
        {
            return (temperatureFahrenheit - 32.0) * 5.0 / 9.0;
        }
        /// <summary>
        /// Converts temperature from Celsius (°C) to Fahrenheit (°F).
        ///
        /// Relationship:
        /// - T(°F) = T(°C) * 9 / 5 + 32
        ///
        /// Physical note:
        /// - Temperatures below -273.15 °C are physically impossible (below absolute zero).
        /// </summary>
        /// <param name="temperatureCelsius">Temperature in Celsius (°C).</param>
        /// <returns>Temperature in Fahrenheit (°F).</returns>
        public static double CelsiusToFahrenheit(double temperatureCelsius)
        {
            if (temperatureCelsius < -273.15)
            {
                throw new ArgumentException(
                    "Temperature in Celsius cannot be lower than absolute zero (-273.15 °C).",
                    nameof(temperatureCelsius));
            }

            return temperatureCelsius * 9.0 / 5.0 + 32.0;
        }

        /// <summary>
        /// Converts temperature from Fahrenheit (°F) to Kelvin (K).
        ///
        /// Relationship:
        /// - T(K) = (T(°F) - 32) * 5 / 9 + 273.15
        ///
        /// Physical note:
        /// - Kelvin is the absolute temperature scale required for thermodynamic equations.
        /// </summary>
        /// <param name="temperatureFahrenheit">Temperature in Fahrenheit (°F).</param>
        /// <returns>Temperature in Kelvin (K).</returns>
        public static double FahrenheitToKelvin(double temperatureFahrenheit)
        {
            double temperatureKelvin = (temperatureFahrenheit - 32.0) * 5.0 / 9.0 + 273.15;

            if (temperatureKelvin < 0)
            {
                throw new ArgumentException(
                    "Temperature in Kelvin cannot be negative.",
                    nameof(temperatureFahrenheit));
            }

            return temperatureKelvin;
        }
        /// <summary>
        /// Converts temperature from Kelvin (K) to Fahrenheit (°F).
        ///
        /// Relationship:
        /// - T(°F) = (T(K) - 273.15) * 9 / 5 + 32
        ///
        /// Physical note:
        /// - Temperatures below 0 K are physically impossible (below absolute zero).
        /// </summary>
        /// <param name="temperatureKelvin">Temperature in Kelvin (K).</param>
        /// <returns>Temperature in Fahrenheit (°F).</returns>
        public static double KelvinToFahrenheit(double temperatureKelvin)
        {
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException(
                    "Temperature in Kelvin cannot be negative.",
                    nameof(temperatureKelvin));
            }

            return (temperatureKelvin - 273.15) * 9.0 / 5.0 + 32.0;
        }


    }
}
