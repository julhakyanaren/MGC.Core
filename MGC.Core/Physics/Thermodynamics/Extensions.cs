using System;

namespace MGC.Physics.Thermodynamics
{
    public static class ThermodynamicsExtensions
    {
        /// <summary>
        /// Converts a temperature value from a specified temperature scale to Kelvin (K).
        ///
        /// Kelvin is the absolute thermodynamic temperature scale and is required for
        /// most thermodynamic equations (ideal gas law, entropy relations, etc.).
        ///
        /// Conversion rules:
        /// - From Celsius: K = °C + 273.15
        /// - From Fahrenheit: K = (°F - 32) * 5/9 + 273.15
        /// - From Kelvin: unchanged
        ///
        /// Physical remark:
        /// - Temperatures below 0 K are physically impossible and will be rejected by the conversion logic.
        /// </summary>
        /// <param name="temperature">Input temperature value.</param>
        /// <param name="fromType">Temperature unit/scale of the input value.</param>
        /// <returns>Temperature converted to Kelvin (K).</returns>
        public static double ToKelvin(this double temperature, PhysicTypes.TemperatureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.TemperatureType.Celsius:
                    {
                        return PhysicConvert.CelsiusToKelvin(temperature);
                    }
                case PhysicTypes.TemperatureType.Fahrenheit:
                    {
                        return PhysicConvert.FahrenheitToKelvin(temperature);
                    }
                case PhysicTypes.TemperatureType.Kelvin:
                    {
                        return temperature;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown TemperatureType value.");
                    }
            }
        }
        /// <summary>
        /// Converts a temperature value from a specified temperature scale to Celsius (°C).
        ///
        /// Celsius is commonly used for display and user input. For thermodynamic equations,
        /// Kelvin is recommended as the internal unit.
        ///
        /// Conversion rules:
        /// - From Kelvin: °C = K - 273.15
        /// - From Fahrenheit: °C = (°F - 32) * 5/9
        /// - From Celsius: unchanged
        ///
        /// Physical remark:
        /// - Values below absolute zero are physically impossible; any conversion producing such
        ///   a value is rejected by the underlying conversion method.
        /// </summary>
        /// <param name="temperature">Input temperature value.</param>
        /// <param name="fromType">Temperature unit/scale of the input value.</param>
        /// <returns>Temperature converted to Celsius (°C).</returns>
        public static double ToCelsius(this double temperature, PhysicTypes.TemperatureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.TemperatureType.Celsius:
                    {
                        return temperature;
                    }
                case PhysicTypes.TemperatureType.Fahrenheit:
                    {
                        return PhysicConvert.FahrenheitToCelsius(temperature);
                    }
                case PhysicTypes.TemperatureType.Kelvin:
                    {
                        return PhysicConvert.KelvinToCelsius(temperature);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown TemperatureType value.");
                    }
            }
        }
        /// <summary>
        /// Converts a temperature value from a specified temperature scale to Fahrenheit (°F).
        ///
        /// Fahrenheit is primarily used in the United States and a few other regions.
        /// For thermodynamic equations, Kelvin is recommended as the internal unit.
        ///
        /// Conversion rules:
        /// - From Celsius: °F = °C * 9/5 + 32
        /// - From Kelvin: °F = (K - 273.15) * 9/5 + 32
        /// - From Fahrenheit: unchanged
        ///
        /// Physical remark:
        /// - Temperatures below absolute zero are physically impossible; conversions producing invalid
        ///   values are rejected by the underlying conversion logic.
        /// </summary>
        /// <param name="temperature">Input temperature value.</param>
        /// <param name="fromType">Temperature unit/scale of the input value.</param>
        /// <returns>Temperature converted to Fahrenheit (°F).</returns>
        public static double ToFahrenheit(this double temperature, PhysicTypes.TemperatureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.TemperatureType.Celsius:
                    {
                        return PhysicConvert.CelsiusToFahrenheit(temperature);
                    }
                case PhysicTypes.TemperatureType.Fahrenheit:
                    {
                        return temperature;
                    }
                case PhysicTypes.TemperatureType.Kelvin:
                    {
                        return PhysicConvert.KelvinToFahrenheit(temperature);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown TemperatureType value.");
                    }
            }
        }

        /// <summary>
        /// Converts a pressure value from a specified pressure unit to Pascal (Pa).
        ///
        /// Pascal is the SI base unit of pressure and is recommended as the internal unit
        /// for all physical and thermodynamic calculations.
        ///
        /// Conversion rules (via <see cref="StateVariables"/>):
        /// - From bar: Pa = bar * Constants.Bar
        /// - From atm: Pa = atm * Constants.StandardAtmosphere
        /// - From mmHg: Pa = mmHg * Constants.MillimeterOfMercury
        /// - From Pa: unchanged
        ///
        /// Design remark:
        /// - Negative values are allowed because pressure may be represented as gauge pressure
        ///   (relative to a reference). If absolute pressure is required, validate it in the calling code.
        /// </summary>
        /// <param name="pressure">Input pressure value.</param>
        /// <param name="fromType">Pressure unit of the input value.</param>
        /// <returns>Pressure converted to Pascals (Pa).</returns>
        public static double ToPascal(this double pressure, PhysicTypes.PressureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.PressureType.Pascal:
                    {
                        return pressure;
                    }
                case PhysicTypes.PressureType.Bar:
                    {
                        return PhysicConvert.BarToPascal(pressure);
                    }
                case PhysicTypes.PressureType.MillimeterOfMercury:
                    {
                        return PhysicConvert.MillimeterOfMercuryToPascal(pressure);
                    }
                case PhysicTypes.PressureType.Atmosphere:
                    {
                        return PhysicConvert.AtmosphereToPascal(pressure);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown PressureType value.");
                    }
            }
        }
        /// <summary>
        /// Converts a pressure value from a specified pressure unit to bar.
        ///
        /// Bar is widely used in engineering practice:
        /// - 1 bar = 100000 Pa
        ///
        /// Conversion rules (via <see cref="StateVariables"/>):
        /// - From Pa: bar = Pa / Constants.Bar
        /// - From atm: bar = atm * Constants.StandardAtmosphere / Constants.Bar
        /// - From mmHg: bar = mmHg * Constants.MillimeterOfMercury / Constants.Bar
        /// - From bar: unchanged
        ///
        /// Design remark:
        /// - Negative values are allowed (gauge pressure). Validate in the calling code if needed.
        /// </summary>
        /// <param name="pressure">Input pressure value.</param>
        /// <param name="fromType">Pressure unit of the input value.</param>
        /// <returns>Pressure converted to bar.</returns>
        public static double ToBar(this double pressure, PhysicTypes.PressureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.PressureType.Pascal:
                    {
                        return PhysicConvert.PascalToBar(pressure);
                    }
                case PhysicTypes.PressureType.Bar:
                    {
                        return pressure;
                    }
                case PhysicTypes.PressureType.MillimeterOfMercury:
                    {
                        return PhysicConvert.MillimeterOfMercuryToBar(pressure);
                    }
                case PhysicTypes.PressureType.Atmosphere:
                    {
                        return PhysicConvert.AtmosphereToBar(pressure);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown PressureType value.");
                    }
            }
        }
        /// <summary>
        /// Converts a pressure value from a specified pressure unit to millimeter of mercury (mmHg).
        ///
        /// Millimeter of mercury is commonly used in medicine and vacuum engineering.
        /// Standard definition:
        /// - 1 mmHg = Constants.MillimeterOfMercury Pa
        ///
        /// Conversion rules (via <see cref="StateVariables"/>):
        /// - From Pa: mmHg = Pa / Constants.MillimeterOfMercury
        /// - From bar: mmHg = bar * Constants.Bar / Constants.MillimeterOfMercury
        /// - From atm: mmHg = atm * Constants.StandardAtmosphere / Constants.MillimeterOfMercury
        /// - From mmHg: unchanged
        ///
        /// Design remark:
        /// - Negative values are allowed (gauge pressure). Validate in the calling code if needed.
        /// </summary>
        /// <param name="pressure">Input pressure value.</param>
        /// <param name="fromType">Pressure unit of the input value.</param>
        /// <returns>Pressure converted to millimeters of mercury (mmHg).</returns>
        public static double ToMillimeterOfMercury(this double pressure, PhysicTypes.PressureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.PressureType.Pascal:
                    {
                        return PhysicConvert.PascalToMillimeterOfMercury(pressure);
                    }
                case PhysicTypes.PressureType.Bar:
                    {
                        return PhysicConvert.BarToMillimeterOfMercury(pressure);
                    }
                case PhysicTypes.PressureType.MillimeterOfMercury:
                    {
                        return pressure;

                    }
                case PhysicTypes.PressureType.Atmosphere:
                    {
                        return PhysicConvert.AtmosphereToMillimeterOfMercury(pressure);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown PressureType value.");
                    }
            }
        }
        /// <summary>
        /// Converts a pressure value from a specified pressure unit to standard atmosphere (atm).
        ///
        /// Standard atmosphere is frequently used as a reference pressure:
        /// - 1 atm = Constants.StandardAtmosphere Pa
        ///
        /// Conversion rules (via <see cref="StateVariables"/>):
        /// - From Pa: atm = Pa / Constants.StandardAtmosphere
        /// - From bar: atm = bar * Constants.Bar / Constants.StandardAtmosphere
        /// - From mmHg: atm = mmHg * Constants.MillimeterOfMercury / Constants.StandardAtmosphere
        /// - From atm: unchanged
        ///
        /// Design remark:
        /// - Negative values are allowed (gauge pressure). Validate in the calling code if needed.
        /// </summary>
        /// <param name="pressure">Input pressure value.</param>
        /// <param name="fromType">Pressure unit of the input value.</param>
        /// <returns>Pressure converted to atmospheres (atm).</returns>
        public static double ToAtmosphere(this double pressure, PhysicTypes.PressureType fromType)
        {
            switch (fromType)
            {
                case PhysicTypes.PressureType.Pascal:
                    {
                        return PhysicConvert.PascalToAtmosphere(pressure);
                    }
                case PhysicTypes.PressureType.Bar:
                    {
                        return PhysicConvert.BarToAtmosphere(pressure);
                    }
                case PhysicTypes.PressureType.MillimeterOfMercury:
                    {
                        return PhysicConvert.MillimeterOfMercuryToAtmosphere(pressure);
                    }
                case PhysicTypes.PressureType.Atmosphere:
                    {
                        return pressure;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromType), "Unknown PressureType value.");
                    }
            }
        }
    }
}
