using System;
using System.Drawing;

namespace POSSystem.WPF.UI.Service
{
    public class ColorService : IColorService
    {
        public Color GetColor(string colorName)
        {
            Color c = Color.FromName(colorName);
            if (c.IsKnownColor) return c;
            byte[] rgb = GenerateRandomColor();
            c = Color.FromArgb(1, rgb[0], rgb[1], rgb[2]);
            return c;
        }

        public string GetColorHex(Color color)
        {
            string hex = "";
            try
            {
                hex = string.Format("#{0}{1}{2}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
            }
            catch
            {

                hex = "#288db8";
            }
            return hex;
        }

        private byte[] GenerateRandomColor()
        {
            Random rand = new Random();
            byte[] colorRGB = new byte[3];
            colorRGB[0] = (byte)rand.Next(0, 255);
            colorRGB[1] = (byte)rand.Next(0, 255);
            colorRGB[2] = (byte)rand.Next(0, 255);
            return colorRGB;
        }
    }
}
