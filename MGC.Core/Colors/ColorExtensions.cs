namespace MGC.Colors
{
    /// <summary>
    /// Provides extension methods for <see cref="Color"/> based on <see cref="ColorAction"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns a new color with inverted RGB channels.
        /// </summary>
        /// <param name="color">Source color to invert.</param>
        public static Color Invert(this Color color)
        {
            return ColorAction.Invert(color);
        }

        /// <summary>
        /// Returns a new color with the red component removed (set to zero).
        /// </summary>
        /// <param name="color">Source color.</param>
        public static Color CutRed(this Color color)
        {
            return ColorAction.CutRed(color);
        }
        /// <summary>
        /// Returns a new color with the green component removed (set to zero).
        /// </summary>
        /// <param name="color">Source color.</param>
        public static Color CutGreen(this Color color)
        {
            return ColorAction.CutGreen(color);
        }
        /// <summary>
        /// Returns a new color with the blue component removed (set to zero).</summary>
        /// <param name="color">Source color.</param>
        public static Color CutBlue(this Color color)
        {
            return ColorAction.CutBlue(color);
        }
    }
}
