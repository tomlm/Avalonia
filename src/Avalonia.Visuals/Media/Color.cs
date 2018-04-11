// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace Avalonia.Media
{
    /// <summary>
    /// An ARGB color.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Gets or sets the Alpha component of the color.
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Gets or sets the Red component of the color.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Gets or sets the Green component of the color.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets or sets the Blue component of the color.
        /// </summary>
        public byte B { get; }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from alpha, red, green and blue components.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <returns>The color.</returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from red, green and blue components.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <returns>The color.</returns>
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color(0xff, r, g, b);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from an integer.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <returns>The color.</returns>
        public static Color FromUInt32(uint value)
        {
            return new Color(
                (byte)((value >> 24) & 0xff),
                (byte)((value >> 16) & 0xff),
                (byte)((value >> 8) & 0xff),
                (byte)(value & 0xff)
            );
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/> value.</param>
        /// <param name="normalized">True if the color vector is normalized floats, false if it's bytes (0-255)</param>
        /// <returns>The color.</returns>
        public static Color FromVector4(Vector4 value, bool normalized)
        {
            if (normalized)
            {
                return new Color(
                    (byte)(value.W * 255),
                    (byte)(value.X * 255),
                    (byte)(value.Y * 255),
                    (byte)(value.Z * 255)
                );
            } else {
                return new Color(
                    (byte)(value.W),
                    (byte)(value.X),
                    (byte)(value.Y),
                    (byte)(value.Z)
                );
            }

        }
        /// <summary>
        /// Parses a color string.
        /// </summary>
        /// <param name="s">The color string.</param>
        /// <returns>The <see cref="Color"/>.</returns>
        public static Color Parse(string s)
        {
            if (s[0] == '#')
            {
                var or = 0u;

                if (s.Length == 7)
                {
                    or = 0xff000000;
                }
                else if (s.Length != 9)
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }

                return FromUInt32(uint.Parse(s.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture) | or);
            }
            else
            {
                var upper = s.ToUpperInvariant();
                var member = typeof(Colors).GetTypeInfo().DeclaredProperties
                    .FirstOrDefault(x => x.Name.ToUpperInvariant() == upper);

                if (member != null)
                {
                    return (Color)member.GetValue(null);
                }
                else
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }
            }
        }

        /// <summary>
        /// Returns the string representation of the color.
        /// </summary>
        /// <returns>
        /// The string representation of the color.
        /// </returns>
        public override string ToString()
        {
            uint rgb = ((uint)A << 24) | ((uint)R << 16) | ((uint)G << 8) | (uint)B;
            return $"#{rgb:x8}";
        }

        /// <summary>
        /// Returns the integer representation of the color.
        /// </summary>
        /// <returns>
        /// The integer representation of the color.
        /// </returns>
        public uint ToUint32()
        {
            return ((uint)A << 24) | ((uint)R << 16) | ((uint)G << 8) | (uint)B;
        }
    }
}
