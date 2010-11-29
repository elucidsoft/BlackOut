using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackOut;
using System.Diagnostics;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSolver()
        {
            int[,] level1Hint = new int[5, 5];

            Solver solver = new Solver(TempLevels.Level1Board);
            level1Hint = solver.GetHint();

            PrintBoard(TempLevels.Level1Board);

            Debug.WriteLine("----------------------------");
            PrintBoard(level1Hint);
        }

        private static void PrintBoard(int[,] board)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Debug.Write(" ");
                    if (board[column, row] == 1)
                        Debug.Write("1");
                    else
                        Debug.Write("0");
                }
                Debug.WriteLine("");
            }
        }
    }
}
