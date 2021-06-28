using System;
using System.Drawing;

namespace POS.Core.Utilities.Extension
{
    public static class StringExtension
    {

        public static string ToKnownColourName(this string colorHex)
        {
            if (!string.IsNullOrEmpty(colorHex))
            {
                Color color = (Color)new ColorConverter().ConvertFromString(colorHex);

                if (color.IsKnownColor)
                {
                    return color.ToKnownColor().ToString();
                }
            }

            return "";
        }

        public static String ToHexValue(this System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

    }
}
