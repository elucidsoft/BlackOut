using System;

namespace BlackOut
{
    public class LevelLoadedEventArgs : EventArgs   
    {
        public int Level { get; set; }
        public LevelLoadedEventArgs(int level)
        {
            Level = level;
        }
    }
}
