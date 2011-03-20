using System.Windows.Media;
using System;
using Sys = System;

namespace BlackOut
{
    public static class ColorConverter
    {
        public static Color Convert(string value)
        {
            string val = value.ToString();
            val = val.Replace("#", "");

            byte a = Sys.Convert.ToByte("ff", 16);
            byte pos = 0;
            if (val.Length == 8)
            {
                a = Sys.Convert.ToByte(val.Substring(pos, 2), 16);
                pos = 2;
            }

            byte r = Sys.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte g = Sys.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte b = Sys.Convert.ToByte(val.Substring(pos, 2), 16);

            Color col = Color.FromArgb(a, r, g, b);

            return col;
        }

        public static object ConvertBack(object value)
        {
            SolidColorBrush val = value as SolidColorBrush;
            return String.Format("#{0}{1}{2}{3}", val.Color.A, val.Color.R, val.Color.G, val.Color.B);
        }        
    }
}
