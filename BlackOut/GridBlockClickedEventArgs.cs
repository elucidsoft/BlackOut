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
    public class GridBlockClickedEventArgs : EventArgs
    {
        public int Moves { get; set; }
        public GridBlockClickedEventArgs(int moves)
        {
            Moves = moves;
        }

        public GridBlockClickedEventArgs()
        {

        }
    }
}
