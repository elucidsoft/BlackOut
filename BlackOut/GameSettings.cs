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
using System.Collections.Generic;

namespace BlackOut
{
    public class GameSettings
    {
        private List<int[,]> levels = new List<int[,]>();

        public GameSettings()
        {
            levels.Add(TempLevels.Level1Board);
            levels.Add(TempLevels.Level2Board);
            levels.Add(TempLevels.Level3Board);
            levels.Add(TempLevels.Level4Board);
            levels.Add(TempLevels.Level5Board);
        }

        public int[][,] Levels
        {
            get { return levels.ToArray(); }
        }

        //public static GameSettings LoadSettings()
        //{

        //}
    }
}
