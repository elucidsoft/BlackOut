using System;

namespace BlackOut
{
    public class GameTimerTickEventArgs : EventArgs
    {
        public GameTimerTickEventArgs(long seconds)
        {
            Seconds = seconds;
        }
        public long Seconds { get; private set; }
    }
}
