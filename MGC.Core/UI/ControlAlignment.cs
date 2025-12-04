namespace MGC.UI
{
    /// <summary>
    /// Defines a 3x3 alignment grid used to position one control
    /// relative to another or within its parent container.
    /// </summary>
    public enum ControlAlignment
    {
        /// <summary>
        /// Top-left corner.
        /// </summary>
        TopLeft = 1,
        /// <summary>
        /// Top edge, horizontally centered.
        /// </summary>
        TopCenter = 2,
        /// <summary>
        /// Top-right corner.
        /// </summary>
        TopRight = 3,
        /// <summary>
        /// Left edge, vertically centered.
        /// </summary>
        MiddleLeft = 4,
        /// <summary>
        /// Exact center (both horizontally and vertically).
        /// </summary>
        Center = 5,
        /// <summary>
        /// Right edge, vertically centered.
        /// </summary>
        MiddleRight = 6,
        /// <summary>
        /// Bottom-left corner.
        /// </summary>
        BottomLeft = 7,
        /// <summary>
        /// Bottom edge, horizontally centered.
        /// </summary>
        BottomCenter = 8,
        /// <summary>
        /// Bottom-right corner.
        /// </summary>
        BottomRight = 9
    }
}
