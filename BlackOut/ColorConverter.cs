using System.Windows.Media;

namespace BlackOut
{
    public static class ColorConverter
    {
        public static Color Convert(string value)
        {
            string val = value.ToString();
            val = val.Replace("#", "");

            byte a = System.Convert.ToByte("ff", 16);
            byte pos = 0;
            if (val.Length == 8)
            {
                a = System.Convert.ToByte(val.Substring(pos, 2), 16);
                pos = 2;
            }

            byte r = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte g = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte b = System.Convert.ToByte(val.Substring(pos, 2), 16);

            Color col = Color.FromArgb(a, r, g, b);

            return col;
        }

        public static object ConvertBack(object value)
        {
            SolidColorBrush val = value as SolidColorBrush;
            return "#" + val.Color.A.ToString() + val.Color.R.ToString() + val.Color.G.ToString() + val.Color.B.ToString();
        }        
    }
}
