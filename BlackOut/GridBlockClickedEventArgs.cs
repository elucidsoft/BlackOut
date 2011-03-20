using System;

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
