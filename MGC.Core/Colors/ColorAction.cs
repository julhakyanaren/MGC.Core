namespace MGC.Colors
{
    /// <summary>
    /// Provides utility methods for manipulating and combining colors,
    /// including inversion, channel removal, additive blending, partial mixing,
    /// and component-wise subtraction.
    /// </summary>
    /// <remarks>
    /// This class contains helper functions to:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Invert RGB components while preserving alpha using
    ///       <see cref="Invert(Color)"/> and <see cref="Invert(byte, byte, byte, byte)"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Remove individual color channels with
    ///       <see cref="CutRed(Color)"/>, <see cref="CutGreen(Color)"/> and <see cref="CutBlue(Color)"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Perform additive blending of multiple colors using
    ///       <see cref="Add(System.Collections.Generic.List{Color}, float)"/>,
    ///       <see cref="Add(Color[], float)"/> and <see cref="Add(Color, Color, float)"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply partial additive mixing of two colors with a mixing coefficient
    ///       via <see cref="AdditiveMix(Color, Color, float, float)"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Subtract one color from another component-wise using
    ///       <see cref="Subtract(Color, Color, float)"/>.
    ///     </description>
    ///   </item>
    /// </list>
    /// All operations clamp resulting RGB components to the valid byte range [0..255].
    /// The <c>alpha</c> parameters are treated as normalized multipliers in the range [0..1]
    /// and are converted to the 0–255 alpha channel in the resulting color.
    /// </remarks>
    public static class ColorAction
    {
        /// <summary>
        /// Inverts the RGB components of the given color while preserving its alpha channel.
        /// </summary>
        /// <param name="inputColor">Source color to invert.</param>
        /// <returns>A new color with inverted RGB values.</returns>
        public static Color Invert(Color inputColor)
        {
            return Invert(inputColor.R, inputColor.G, inputColor.B, inputColor.A);
        }
        /// <summary>
        /// Inverts the provided RGB components while preserving the given alpha value.
        /// </summary>
        /// <param name="red">Red component of the color (0–255).</param>
        /// <param name="green">Green component of the color (0–255).</param>
        /// <param name="blue">Blue component of the color (0–255).</param>
        /// <param name="alpha">Alpha component of the color (0–255).</param>
        /// <returns>A new inverted color.</returns>
        public static Color Invert(byte red, byte green, byte blue, byte alpha)
        {
            return Color.FromArgb(alpha, 255 - red, 255 - green, 255 - blue);
        }

        /// <summary>
        /// Returns a new color with the red channel removed (set to zero).
        /// </summary>
        /// <param name="color">Original color.</param>
        /// <returns>Color with the red component set to zero.</returns>
        public static Color CutRed(Color color)
        {
            return Color.FromArgb(color.A, 0, color.G, color.B);
        }
        /// <summary>
        /// Returns a new color with the green channel removed (set to zero).
        /// </summary>
        /// <param name="color">Original color.</param>
        /// <returns>Color with the green component set to zero.</returns>
        public static Color CutGreen(Color color)
        {
            return Color.FromArgb(color.A, color.R, 0, color.B);
        }
        /// <summary>
        /// Returns a new color with the blue channel removed (set to zero).
        /// </summary>
        /// <param name="color">Original color.</param>
        /// <returns>Color with the blue component set to zero.</returns>
        public static Color CutBlue(Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, 0);
        }

        /// <summary>
        /// Performs additive color blending for all colors in the provided list.
        /// The resulting color components are clamped to the 0–255 range.
        /// </summary>
        /// <param name="colors">A list of colors to be added.</param>
        /// <param name="alpha">Alpha multiplier for the resulting color (0–1).</param>
        /// <returns>A new color resulting from additive blending.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        public static Color Add(List<Color> colors, float alpha = 1)
        {
            if (colors is null)
            {
                throw new ArgumentNullException(nameof(colors));
            }
            return Add(colors.ToArray(), alpha);
        }
        /// <summary>
        /// Performs additive blending of two colors by internally calling <see cref="Add(Color[], float)"/>.
        /// </summary>
        /// <param name="firstColor">First color.</param>
        /// <param name="secondColor">Second color.</param>
        /// <param name="alpha">Alpha multiplier for the result (0–1).</param>
        /// <returns>The result of adding the two colors.</returns>
        public static Color Add(Color firstColor, Color secondColor, float alpha = 1)
        {
            Color[] colors = { firstColor, secondColor };
            return Add(colors, alpha);
        }
        /// <summary>
        /// Performs additive color blending for all colors in the provided array.
        /// The resulting RGB values are clamped to the valid byte range 0–255.
        /// </summary>
        /// <param name="colors">Array of colors to add.</param>
        /// <param name="alpha">Alpha multiplier for the output color (0–1).</param>
        /// <returns>The resulting color after additive blending.</returns>
        /// <exception cref="ArgumentNullException">Thrown when colors is null.</exception>
        /// <exception cref="ArgumentException">Thrown when colors array is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when alpha is outside the range 0–1.</exception>
        public static Color Add(Color[] colors, float alpha = 1)
        {
            if (colors is null)
            {
                throw new ArgumentNullException(nameof(colors));
            }
            if (colors.Length == 0)
            {
                throw new ArgumentException("Colors array must not be empty.", nameof(colors));
            }
            if (alpha < 0f || alpha > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), "Value must be between 0 and 1.");
            }
            int mixR = 0;
            int mixG = 0;
            int mixB = 0;
            for (int c = 0; c < colors.Length; c++)
            {
                mixR += colors[c].R;
                mixG += colors[c].G;
                mixB += colors[c].B;
            }
            mixR = Math.Clamp(mixR, 0, 255);
            mixG = Math.Clamp(mixG, 0, 255);
            mixB = Math.Clamp(mixB, 0, 255);

            return Color.FromArgb(Math.Clamp(Convert.ToInt32(alpha * 255), 0, 255), mixR, mixG, mixB);
        }

        /// <summary>
        /// Performs additive mixing between two colors using a mixing coefficient.
        /// The second color is scaled by <paramref name="mixCoeff"/> before being added.
        /// </summary>
        /// <param name="firstColor">Base color.</param>
        /// <param name="secondColor">Color to be partially added.</param>
        /// <param name="mixCoeff">Coefficient in the range 0–1 specifying how much of the second color is added.</param>
        /// <param name="alpha">Alpha multiplier for the result (0–1).</param>
        /// <returns>A new color created by additive partial blending.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when mixCoeff or alpha are out of range.</exception>
        public static Color AdditiveMix(Color firstColor, Color secondColor, float mixCoeff, float alpha = 1)
        {
            if (mixCoeff < 0f || mixCoeff > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(mixCoeff), "Value must be between 0 and 1.");
            }
            if (alpha < 0f || alpha > 1f)
            {
                throw new ArgumentException("Value must be between 0 and 1.", nameof(alpha));
            }

            int r = Math.Clamp((int)(firstColor.R + secondColor.R * mixCoeff), 0, 255);
            int g = Math.Clamp((int)(firstColor.G + secondColor.G * mixCoeff), 0, 255);
            int b = Math.Clamp((int)(firstColor.B + secondColor.B * mixCoeff), 0, 255);

            return Color.FromArgb(Math.Clamp(Convert.ToInt32(alpha * 255), 0, 255), r, g, b);
        }

        /// <summary>
        /// Subtracts one color from another component-wise, with results clamped to the 0–255 range.
        /// </summary>
        /// <param name="mainColor">The base color.</param>
        /// <param name="subtractableColor">The color to subtract from the base color.</param>
        /// <param name="alpha">Alpha multiplier for the resulting color (0–1).</param>
        /// <returns>A new color created by component-wise subtraction.</returns>
        /// <exception cref="ArgumentException">Thrown if alpha is outside the range 0–1.</exception>
        public static Color Subtract(Color mainColor, Color subtractableColor, float alpha = 1)
        {
            if (alpha < 0f || alpha > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), "Value must be between 0 and 1.");
            }
            int red = Math.Clamp(mainColor.R - subtractableColor.R, 0, 255);
            int green = Math.Clamp(mainColor.G - subtractableColor.G, 0, 255);
            int blue = Math.Clamp(mainColor.B - subtractableColor.B, 0, 255);
            return Color.FromArgb(Math.Clamp(Convert.ToInt32(alpha * 255), 0, 255), red, green, blue);
        }
    }
}
