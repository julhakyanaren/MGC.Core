using MGC.Math;
namespace MGC.Colors
{
    /// <summary>
    /// Provides utility methods for converting colors between different color spaces,
    /// including RGB, HSV, and HSL.
    /// </summary>
    /// <remarks>
    /// This class contains helper functions to:
    /// <list type="bullet">
    ///   <item><description>Extract RGB(A) components from <see cref="System.Drawing.Color"/>.</description></item>
    ///   <item><description>Convert between RGB and HSV color spaces.</description></item>
    ///   <item><description>Convert between RGB and HSL color spaces.</description></item>
    ///   <item><description>Convert between HSV and HSL using mathematical transformations.</description></item>
    /// </list>
    /// All numeric RGB inputs are expected to be in the inclusive range [0..255], while HSV and HSL
    /// components use normalized values:
    /// <list type="bullet">
    ///   <item><description><c>Hue</c>: degrees [0..360).</description></item>
    ///   <item><description><c>Saturation</c> and <c>Value</c>/<c>Lightness</c>: [0..1].</description></item>
    /// </list>
    /// Alpha channels, where applicable, are also represented in normalized form [0..1].
    /// </remarks>
    public static class ColorConvert
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to a list of components.
        /// </summary>
        /// <param name="rgb">The source color.</param>
        /// <param name="includeAlpha">
        /// If <c>true</c>, includes the alpha component (A) as the last element;
        /// otherwise only R, G, and B are included.
        /// </param>
        /// <returns>
        /// A list of components in the order R, G, B, [A], where each value is
        /// the corresponding byte component cast to <see cref="double"/>.
        /// </returns>
        public static List<double> ToCollection(System.Drawing.Color rgb, bool includeAlpha)
        {
            List<double> list = new List<double>(includeAlpha ? 4 : 3)
            {
                rgb.R,
                rgb.G,
                rgb.B
            };
            if (includeAlpha)
            {
                list.Add(rgb.A);
            }
            return list;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to an array of components.
        /// </summary>
        /// <param name="rgb">The source color.</param>
        /// <param name="includeAlpha">
        /// If <c>true</c>, returns an array of length 4 (R, G, B, A);
        /// otherwise returns an array of length 3 (R, G, B).
        /// </param>
        /// <returns>
        /// An array of components in the order R, G, B, [A], where each value is
        /// the corresponding byte component cast to <see cref="double"/>.
        /// </returns>
        public static double[] ToArray(Color rgb, bool includeAlpha)
        {
            double[] colorArray = includeAlpha ? new double[4] : new double[3];

            colorArray[0] = rgb.R;
            colorArray[1] = rgb.G;
            colorArray[2] = rgb.B;
            if (includeAlpha)
            {
                colorArray[3] = rgb.A;
            }
            return colorArray;
        }

        /// <summary>
        /// Converts an RGB color to HSV.
        /// </summary>
        /// <param name="rgb">The source RGB color.</param>
        /// <param name="hue">The resulting hue in degrees [0..360).</param>
        /// <param name="saturation">The resulting saturation in range [0..1].</param>
        /// <param name="value">The resulting value (brightness) in range [0..1].</param>
        public static void RGBToHSV(Color rgb, out double hue, out double saturation, out double value)
        {
            RGBToHSV(rgb.R, rgb.G, rgb.B, out hue, out saturation, out value);
        }

        /// <summary>
        /// Converts RGB components to HSV using normalized HSV output.
        /// </summary>
        /// <param name="red">Red component in the inclusive range [0..255].</param>
        /// <param name="green">Green component in the inclusive range [0..255].</param>
        /// <param name="blue">Blue component in the inclusive range [0..255].</param>
        /// <param name="hue">The resulting hue in degrees [0..360).</param>
        /// <param name="saturation">The resulting saturation in range [0..1].</param>
        /// <param name="value">The resulting value (brightness) in range [0..1].</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="red"/>, <paramref name="green"/> or <paramref name="blue"/>
        /// is outside the inclusive range [0..255].
        /// </exception>
        public static void RGBToHSV(double red, double green, double blue, out double hue, out double saturation, out double value)
        {
            if (red < 0d || red > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(red));
            }
            if (green < 0d || green > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(green));
            }
            if (blue < 0d || blue > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(blue));
            }
            List<double> colors = new List<double> { red, green, blue };

            double r = colors[0] / 255.0f;
            double g = colors[1] / 255.0f;
            double b = colors[2] / 255.0f;

            List<double> newColors = new List<double> { r, g, b };

            double max = Statistics.Max(newColors);
            double min = Statistics.Min(newColors);
            double diff = max - min;

            hue = 0;
            saturation = 0;
            value = 0;

            if (diff > 0)
            {
                if (max == r)
                {
                    hue = 60 * (((g - b) / diff) % 6);
                }
                else if (max == g)
                {
                    hue = 60 * (((b - r) / diff) + 2);
                }
                else
                {
                    hue = 60 * (((r - g) / diff) + 4);
                }
            }

            if (hue < 0)
            {
                hue += 360;
            }
            if (max != 0)
            {
                saturation = diff / max;
            }
            value = Statistics.Max(colors) / 255.0f;
        }

        /// <summary>
        /// Converts HSV values to an RGB <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="hue">Hue in degrees [0..360). If <see cref="double.NaN"/>, it is treated as 0.</param>
        /// <param name="saturation">Saturation in range [0..1]. Values outside are clamped.</param>
        /// <param name="value">Value (brightness) in range [0..1]. Values outside are clamped.</param>
        /// <param name="includeAlpha">
        /// If <c>true</c>, the resulting color will use <paramref name="alpha"/> as its alpha channel;
        /// otherwise the resulting color will be fully opaque.
        /// </param>
        /// <param name="alpha">
        /// Alpha in range [0..1]. Used only when <paramref name="includeAlpha"/> is <c>true</c>;
        /// values outside the range are clamped.
        /// </param>
        /// <returns>
        /// A <see cref="System.Drawing.Color"/> created from the HSV values with RGB and alpha
        /// components clamped to the byte range [0..255].
        /// </returns>
        /// <remarks>
        /// The method normalizes the input hue into [0..360),
        /// and clamps <paramref name="saturation"/> and <paramref name="value"/> into [0..1].
        /// </remarks>
        public static System.Drawing.Color HSVToRGB(double hue, double saturation, double value, bool includeAlpha = false, double alpha = 1)
        {
            if (double.IsNaN(hue))
            {
                hue = 0.0f;
            }
            hue = (hue % 360.0 + 360.0) % 360.0;
            switch (saturation)
            {
                case < 0.0f:
                    {
                        saturation = 0.0f;
                        break;
                    }
                case > 1.0f:
                    {
                        saturation = 1.0f;
                        break;
                    }
            }
            switch (value)
            {
                case < 0.0f:
                    {
                        value = 0.0f;
                        break;
                    }
                case > 1.0f:
                    {
                        value = 1.0f;
                        break;
                    }
            }
            double c = value * saturation;
            double x = c * (1.0 - System.Math.Abs((hue / 60.0) % 2.0 - 1.0));
            double m = value - c;

            double r1 = 0.0f;
            double g1 = 0.0f;
            double b1 = 0.0f;

            switch (hue)
            {
                case < 60.0f:
                    {
                        r1 = c;
                        g1 = x;
                        b1 = 0.0;
                        break;
                    }
                case < 120.0f:
                    {
                        r1 = x;
                        g1 = c;
                        b1 = 0.0;
                        break;
                    }
                case < 180.0f:
                    {
                        r1 = 0.0;
                        g1 = c;
                        b1 = x;
                        break;
                    }
                case < 240f:
                    {
                        r1 = 0.0;
                        g1 = x;
                        b1 = c;
                        break;
                    }
                case < 300.0f:
                    {
                        r1 = x;
                        g1 = 0.0;
                        b1 = c;
                        break;
                    }
                default:
                    {
                        r1 = c;
                        g1 = 0.0;
                        b1 = x;
                        break;
                    }
            }
            byte r = (byte)System.Math.Round((r1 + m) * 255.0);
            byte g = (byte)System.Math.Round((g1 + m) * 255.0);
            byte b = (byte)System.Math.Round((b1 + m) * 255.0);
            if (r > 255)
            {
                r = 255;
            }
            if (g > 255)
            {
                g = 255;
            }
            if (b > 255)
            {
                b = 255;
            }
            if (includeAlpha)
            {
                if (alpha < 0.0)
                {
                    alpha = 0.0;
                }
                if (alpha > 1.0)
                {
                    alpha = 1.0; ;
                }

                byte a = (byte)System.Math.Round(alpha * 255.0);
                if (a > 255)
                {
                    a = 255;
                }
                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Converts an RGB color to HSL using <see cref="System.Drawing.Color"/> helpers.
        /// </summary>
        /// <param name="rgb">The source color.</param>
        /// <param name="hue">The resulting hue in degrees [0..360).</param>
        /// <param name="saturation">The resulting saturation in range [0..1].</param>
        /// <param name="lightness">The resulting lightness in range [0..1].</param>
        /// <remarks>
        /// This overload relies on built-in <see cref="System.Drawing.Color.GetHue()"/>,
        /// <see cref="System.Drawing.Color.GetSaturation()"/> and
        /// <see cref="System.Drawing.Color.GetBrightness()"/> methods for conversion.
        /// </remarks>
        public static void RGBToHSL(System.Drawing.Color rgb, out double hue, out double saturation, out double lightness)
        {
            hue = rgb.GetHue();
            saturation = rgb.GetSaturation();
            lightness = rgb.GetBrightness();
        }

        /// <summary>
        /// Converts RGB components to HSL.
        /// </summary>
        /// <param name="red">Red component in the inclusive range [0..255].</param>
        /// <param name="green">Green component in the inclusive range [0..255].</param>
        /// <param name="blue">Blue component in the inclusive range [0..255].</param>
        /// <param name="hue">The resulting hue in degrees [0..360).</param>
        /// <param name="saturation">The resulting saturation in range [0..1].</param>
        /// <param name="lightness">The resulting lightness in range [0..1].</param>
        /// <exception cref="ArgumentException">
        /// Thrown if any of <paramref name="red"/>, <paramref name="green"/>, or <paramref name="blue"/> 
        /// is outside the inclusive range [0..255].
        /// </exception>
        public static void RGBToHSL(double red, double green, double blue, out double hue, out double saturation, out double lightness)
        {
            if (red < 0d || red > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(red));
            }
            if (green < 0d || green > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(green));
            }
            if (blue < 0d || blue > 255d)
            {
                throw new ArgumentException("Sequence must be beetween 0 and 255.", nameof(blue));
            }
            Color rgb = Color.FromArgb
            (
                (byte)red,
                (byte)green,
                (byte)blue
            );
            RGBToHSL(rgb, out hue, out saturation, out lightness);
        }

        /// <summary>
        /// Converts HSL values to their HSV equivalents while preserving hue.
        /// </summary>
        /// <param name="hue">Hue in degrees [0..360).</param>
        /// <param name="saturationL">HSL saturation (S_L) in range [0..1].</param>
        /// <param name="lightness">Lightness (L) in range [0..1].</param>
        /// <param name="hueOut">Output hue in degrees [0..360), equal to the input hue.</param>
        /// <param name="saturationV">Resulting HSV saturation (S_V) in range [0..1].</param>
        /// <param name="value">Resulting HSV value (V) in range [0..1].</param>
        public static void HSLToHSV(double hue, double saturationL, double lightness, out double hueOut, out double saturationV, out double value)
        {
            hueOut = hue;

            double l = lightness;
            double sl = saturationL;

            double v = l + sl * System.Math.Min(l, 1 - l);

            double sv;
            if (v == 0)
            {
                sv = 0;
            }
            else
            {
                sv = 2 * (1 - l / v);
            }
            saturationV = sv;
            value = v;
        }

        /// <summary>
        /// Converts HSV values to their HSL equivalents while preserving hue.
        /// </summary>
        /// <param name="hue">Hue in degrees [0..360).</param>
        /// <param name="saturationV">HSV saturation (S_V) in range [0..1].</param>
        /// <param name="value">HSV value (V) in range [0..1].</param>
        /// <param name="hueOut">Output hue in degrees [0..360), equal to the input hue.</param>
        /// <param name="saturationL">Resulting HSL saturation (S_L) in range [0..1].</param>
        /// <param name="lightness">Resulting HSL lightness (L) in range [0..1].</param>
        public static void HSVToHSL(double hue, double saturationV, double value, out double hueOut, out double saturationL, out double lightness)
        {
            hueOut = hue;

            double v = value;
            double sv = saturationV;

            double l = v * (1 - sv / 2);

            double sl;

            if (l == 0 || l == 1)
            {
                sl = 0;
            }
            else
            {
                sl = (v - l) / System.Math.Min(l, 1 - l);
            }
            saturationL = sl;
            lightness = l;
        }

        /// <summary>
        /// Converts HSL values to an RGB <see cref="System.Drawing.Color"/>.
        /// This method converts HSL to HSV internally and then uses <see cref="HSVToRGB(double, double, double, bool, double)"/>.
        /// </summary>
        /// <param name="hue">Hue in degrees [0..360).</param>
        /// <param name="saturation">HSL saturation (S_L) in range [0..1].</param>
        /// <param name="lightness">Lightness (L) in range [0..1].</param>
        /// <param name="includeAlpha">
        /// If <c>true</c>, the resulting color will use <paramref name="alpha"/> as its alpha channel;
        /// otherwise the resulting color will be fully opaque.
        /// </param>
        /// <param name="alpha">
        /// Alpha in range [0..1]. Used only when <paramref name="includeAlpha"/> is <c>true</c>.
        /// </param>
        /// <returns>
        /// A <see cref="System.Drawing.Color"/> created from the HSL values.
        /// </returns>
        /// <remarks>
        /// Internally, this method first converts the HSL triplet to HSV via
        /// <see cref="HSLToHSV(double, double, double, out double, out double, out double)"/>,
        /// then converts HSV to RGB using <see cref="HSVToRGB(double, double, double, bool, double)"/>.
        /// </remarks>
        public static System.Drawing.Color HSLToRGB(double hue, double saturation, double lightness, bool includeAlpha = false, double alpha = 1.0)
        {
            HSLToHSV(hue, saturation, lightness, out double h2, out double s2, out double v2);

            return HSVToRGB(h2, s2, v2, includeAlpha, alpha);
        }
    }
}
