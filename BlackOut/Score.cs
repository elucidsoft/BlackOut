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
    public class Score
    {
        public Score() { }

        public Score(int moves, int level, int hints, long seconds)
        {
            this.Moves = moves;
            this.Level = level;
            this.Hints = hints;
            this.Seconds = seconds;
        }

        public int Moves { get; set; }

        public int Level { get; set; }

        public int Hints { get; set; }

        public long Seconds { get; set; }

        public string DisplayTime
        {
            get
            {
                return String.Format("{0:HH:mm:ss}", new DateTime(TimeSpan.FromSeconds(Seconds).Ticks));
            }
        }
    }
}
