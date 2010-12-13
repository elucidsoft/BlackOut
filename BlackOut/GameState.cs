using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackOut
{
    public class GameState
    {
        int[,] _boardLevel = new int[5, 5];
        public int Level { get; set; }
        public long Seconds { get; set; }
        public int Moves { get; set; }
        public int HintsUsed { get; set; }
        public int[,] BoardLevel 
        {
            get
            {
                return _boardLevel;
            }
            set
            {
                _boardLevel = value;
            }
        }
                                    
    }
}
