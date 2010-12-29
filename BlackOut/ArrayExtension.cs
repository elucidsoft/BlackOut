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

namespace BlackOut
{
    public static class ArrayHelper
    {
        public static void CopyMultiTo(int[] src, int[] dest)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }
        }

        public static int GetHashCode(int[,] src)
        {
            int hashCode = src.Length;
            for (int i = 0; i <= src.GetUpperBound(0); i++)
            {
                for (int x = 0; x <= src.GetUpperBound(1); x++)
                {
                    hashCode = unchecked(hashCode * 314159 + src[i, x]);
                }
            }
            return hashCode;
        }

        public static int GetHashCode(int[] src)
        {
            int hashCode = src.Length;
            for (int i = 0; i < src.Length; i++)
            {
                hashCode = unchecked(hashCode * 314159 + src[i]);
            }
            return hashCode;
        }
    }
}
