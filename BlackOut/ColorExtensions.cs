using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Xna = Microsoft.Xna.Framework;

namespace BlackOut
{
    public static class ColorExtensions
    {
        public static Color Lerp(this Color color, Color to, float amount)
        {
            float startAlpha = color.A;
            float startRed = color.R;
            float startGreen = color.G;
            float startBlue = color.B;

            float endAlpha = to.A;
            float endRed = to.R;
            float endGreen = to.G;
            float endBlue = to.B;

            byte a = (byte)Xna.MathHelper.Lerp(startAlpha, endAlpha, amount);
            byte r = (byte)Xna.MathHelper.Lerp(startRed, endRed, amount);
            byte g = (byte)Xna.MathHelper.Lerp(startGreen, endGreen, amount);
            byte b = (byte)Xna.MathHelper.Lerp(startBlue, endBlue, amount);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
