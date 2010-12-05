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
            int i;
            for (i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }
        }
    }
}
