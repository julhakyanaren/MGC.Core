namespace MGC.UI
{
    public static class ControlLayout
    {
        /// <summary>
        /// Places the target control relative to the reference control
        /// according to the specified 3x3 alignment.
        /// </summary>
        /// <param name="target">
        /// The control that will be moved and aligned.
        /// </param>
        /// <param name="reference">
        /// The reference control used as a basis for alignment.
        /// </param>
        /// <param name="alignment">
        /// The desired alignment of the target relative to the reference
        /// (e.g., center, top-left, bottom-right).
        /// </param>
        /// <param name="offsetX">
        /// Optional horizontal offset in pixels. Positive values move the
        /// target control to the right.
        /// </param>
        /// <param name="offsetY">
        /// Optional vertical offset in pixels. Positive values move the
        /// target control downward.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="target"/> or <paramref name="reference"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method assumes that both controls share the same parent
        /// (i.e., belong to the same container). If they have different
        /// parents, coordinate conversion (e.g., <see cref="Control.PointToScreen(System.Drawing.Point)"/>
        /// and <see cref="Control.PointToClient(System.Drawing.Point)"/>) must be handled separately.
        /// </remarks>
        public static void PlaceRelativeTo(this Control target, Control reference, ControlAlignment alignment, int offsetX = 0, int offsetY = 0)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (reference is null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            int refLeft = reference.Left;
            int refTop = reference.Top;
            int refWidth = reference.Width;
            int refHeight = reference.Height;

            int newLeft;
            int newTop;

            switch (alignment)
            {
                case ControlAlignment.TopLeft:
                    {
                        newLeft = refLeft;
                        newTop = refTop;
                        break;
                    }
                case ControlAlignment.TopCenter:
                    {
                        newLeft = refLeft + (refWidth - target.Width) / 2;
                        newTop = refTop;
                        break;
                    }
                case ControlAlignment.TopRight:
                    {
                        newLeft = refLeft + refWidth - target.Width;
                        newTop = refTop;
                        break;
                    }
                case ControlAlignment.MiddleLeft:
                    {
                        newLeft = refLeft;
                        newTop = refTop + (refHeight - target.Height) / 2;
                        break;
                    }
                case ControlAlignment.Center:
                    {
                        newLeft = refLeft + (refWidth - target.Width) / 2;
                        newTop = refTop + (refHeight - target.Height) / 2;
                        break;
                    }
                case ControlAlignment.MiddleRight:
                    {
                        newLeft = refLeft + refWidth - target.Width;
                        newTop = refTop + (refHeight - target.Height) / 2;
                        break;
                    }
                case ControlAlignment.BottomLeft:
                    {
                        newLeft = refLeft;
                        newTop = refTop + refHeight - target.Height;
                        break;
                    }
                case ControlAlignment.BottomCenter:
                    {
                        newLeft = refLeft + (refWidth - target.Width) / 2;
                        newTop = refTop + refHeight - target.Height;
                        break;
                    }
                case ControlAlignment.BottomRight:
                    {
                        newLeft = refLeft + refWidth - target.Width;
                        newTop = refTop + refHeight - target.Height;
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "Unknown alignment value.");
                    }
            }

            target.Left = newLeft + offsetX;
            target.Top = newTop + offsetY;
        }
        /// <summary>
        /// Places the control inside its parent container according to the
        /// specified 3x3 alignment.
        /// </summary>
        /// <param name="target">
        /// The control that will be positioned within its parent container.
        /// </param>
        /// <param name="alignment">
        /// The desired alignment within the parent (e.g., center, top-center).
        /// </param>
        /// <param name="offsetX">
        /// Optional horizontal offset in pixels relative to the aligned position.
        /// </param>
        /// <param name="offsetY">
        /// Optional vertical offset in pixels relative to the aligned position.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="target"/> is <c>null</c> or
        /// if <paramref name="target"/> has no parent.
        /// </exception>
        public static void PlaceInParent(this Control target, ControlAlignment alignment, int offsetX = 0, int offsetY = 0)
        {
            ArgumentNullException.ThrowIfNull(target);
            if (target.Parent is null)
            {
                throw new ArgumentNullException(nameof(target.Parent), "The control has no parent container.");
            }
            Control parent = target.Parent;
            target.PlaceRelativeTo(parent, alignment, offsetX, offsetY);
        }

        public static void SetColor(this Control target, Color back, Color fore)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            target.BackColor = back;
            target.ForeColor = fore;
        }

        public static void RevertColor(this Control target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            Color[] colors = { target.BackColor, target.ForeColor };
            target.BackColor = colors[1];
            target.ForeColor = colors[0];
        }
    }
}
