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
    public static class TempLevels
    {
        public static int[,] EmptyBoard = new int[5, 5]
                                    {
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0}
                                    };

        public static int[,] Level1Board = new int[5, 5]
                                    {
                                      {0, 1, 0, 0, 0},
                                      {1, 1, 1, 0, 0},
                                      {0, 1, 0, 1, 0},
                                      {0, 0, 1, 1, 1},
                                      {0, 0, 0, 1, 0}
                                    };


        public static int[,] Level2Board = new int[5, 5]
                                    {
                                      {1, 0, 0, 0, 1},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {1, 0, 0, 0, 1}
                                    };


        public static int[,] Level3Board = new int[5, 5]
                                    {
                                      {0, 0, 1, 0, 0},
                                      {0, 0, 1, 0, 0},
                                      {1, 1, 0, 1, 1},
                                      {0, 0, 1, 0, 0},
                                      {0, 0, 1, 0, 0}
                                    };


        public static int[,] Level4Board = new int[5, 5]
                                    {
                                      {1, 0, 0, 0, 0},
                                      {0, 1, 0, 0, 0},
                                      {0, 0, 1, 0, 0},
                                      {0, 0, 0, 1, 0},
                                      {0, 0, 0, 0, 1}
                                    };


        public static int[,] Level5Board = new int[5, 5]
                                    {
                                      {1, 1, 0, 1, 1},
                                      {1, 1, 0, 1, 1},
                                      {1, 1, 0, 1, 1},
                                      {1, 1, 0, 1, 1},
                                      {1, 1, 0, 1, 1}
                                    };

    }
}
