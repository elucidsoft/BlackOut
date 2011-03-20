using System;

namespace BlackOut
{
    public class Score
    {
        private int _moves;

        public Score() { }

        public Score(int moves, int level, int hints, long seconds)
        {
            this.Moves = moves;
            this.Level = level;
            this.Hints = hints;
            this.Seconds = seconds;
        }

        public int Moves 
        {
            get { return _moves + 1; } //this is like this because original release had a bug where moves was zero based
            set { _moves = value; }
        }

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
