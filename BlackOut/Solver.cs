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
using System.Diagnostics;

namespace BlackOut
{
    public class Solver
    {
        int[,] current_board;

        static readonly int [,] goal_board = new int[5, 5] {
                                {1, 1, 1, 1, 1},
			                    {1, 1, 1, 1, 1},
			                    {1, 1, 1, 1, 1},
			                    {1, 1, 1, 1, 1},
			                    {1, 1, 1, 1, 1}
                             };

        static readonly int[,] hint_board = new int[5, 5] {
                                {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0}
                             };

        static readonly int[, ] hint_matrix = {
                                { 0,1,1,1,0,0,0,1,0,1,0,0,0,1,1,0,0,0,0,1,0,0,0 },
                                { 1,1,0,1,1,0,1,0,0,0,0,0,1,1,1,0,0,0,1,0,0,0,0 },
                                { 1,0,1,1,1,1,0,1,1,0,0,0,1,1,0,1,1,1,1,1,0,1,0 },
                                { 1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1 },
                                { 0,1,1,0,1,1,0,0,0,0,1,0,1,0,1,0,0,1,0,1,1,1,0 },
                                { 0,0,1,0,1,0,1,1,0,1,0,0,1,0,0,0,0,0,1,1,0,0,0 },
                                { 0,1,0,1,0,1,1,0,1,1,0,0,0,1,0,1,1,1,0,0,0,1,0 },
                                { 1,0,1,0,0,1,0,1,1,0,0,0,0,0,1,1,0,1,0,1,1,0,1 },
                                { 0,0,1,0,0,0,1,1,1,0,1,0,0,1,1,1,0,0,1,0,0,1,1 },
                                { 1,0,0,0,0,1,1,0,0,0,1,0,1,0,1,0,1,1,0,1,0,0,1 },
                                { 0,0,0,0,1,0,0,0,1,1,0,0,1,0,1,1,1,1,1,0,0,1,0 },
                                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1 },
                                { 0,1,1,0,1,1,0,0,0,1,1,0,1,1,0,0,0,1,0,0,1,1,0 },
                                { 1,1,1,0,0,0,1,0,1,0,0,0,1,1,1,0,0,0,1,0,0,0,0 },
                                { 1,1,0,0,1,0,0,1,1,1,1,0,0,1,1,1,1,0,1,0,1,0,0 },
                                { 0,0,1,0,0,0,1,1,1,0,1,0,0,0,1,1,0,1,0,1,1,0,1 },
                                { 0,0,1,1,0,0,1,0,0,1,1,1,0,0,1,0,1,1,1,0,0,0,1 },
                                { 0,0,1,0,1,0,1,1,0,1,1,0,1,0,0,1,1,0,1,1,1,0,0 },
                                { 0,1,1,0,0,1,0,0,1,0,1,0,0,1,1,0,1,1,1,0,0,0,1 },
                                { 1,0,1,0,1,1,0,1,0,1,0,0,0,0,0,1,0,1,0,1,1,0,1 },
                                { 0,0,0,1,1,0,0,1,0,0,0,1,1,0,1,1,0,1,0,1,1,1,0 },
                                { 0,0,1,1,1,0,1,0,1,0,1,1,1,0,0,0,0,0,0,0,1,1,1 },
                                { 0,0,0,1,0,0,0,1,1,1,0,1,0,0,0,1,1,0,1,1,0,1,0 }
                              };
        static readonly int[] n1 = { 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1 };
        static readonly int[] n2 = { 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1 };


        public Solver(int[,] board)
        {
            this.current_board = board;
        }

        private void CopyArray(int[] src, int[] dest)
        {
            int i;
            for (i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }
        }

        private void AddToArray(int[] src, int[] v)
        {
            int i;
            for (i = 0; i < src.Length; i++)
            {
                src[i] = (src[i] + v[i]) % 2;
            }
        }

        private int Weight(int[] v)
        {
            int i, t = 0;
            for (i = 0; i < v.Length; i++)
            {
                t = t + v[i];
            }
            return (t);
        }

        public int[,] GetHint()
        {
            int[] current_state = new int[23];
            int[] hint_vector = new int[25];
            int[] best = new int[25];
            int[] next = new int[25];
            int i, j;

            for (i = 0; i < 25; i++)
            {
                hint_vector[i] = 0;
            }
            for (i = 0; i < 23; i++)
            {
                if (current_board[i % 5,i / 5] == 0)
                {
                    current_state[i] = 0;
                }
                else
                {
                    current_state[i] = 1;
                }
            }
            for (i = 0; i < 23; i++)
            {
                for (j = 0; j < 23; j++)
                {
                    hint_vector[i] =
                      (hint_vector[i] + current_state[j] * hint_matrix[i, j]) % 2;
                }
            }

            // Now we have a working hint vector, but we test h+n1, h+n2 
            // and h+n1+n2 to see which has the lowest weight, giving a
            // shortest solution.

            CopyArray(hint_vector, best);
            CopyArray(hint_vector, next);
            AddToArray(next, n1);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }
            CopyArray(hint_vector, next);
            AddToArray(next, n2);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }
            CopyArray(hint_vector, next);
            AddToArray(next, n1);
            AddToArray(next, n2);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }

            for (i = 0; i < 25; i++)
            {
                if (best[i] == 1) { hint_board[i % 5, i / 5] = 1; }
                else { hint_board[i % 5, i / 5] = 0; }
            }

            return hint_board;
        }

        public static bool IsSolvable(int[,] current_board)
        {
            int[] current_state = new int[25];
            int i = 0; 
            int dotprod = 0;

            for (i = 0; i < 25; i++)
            {
                if (current_board[i % 5, i / 5] == 1)
                {
                    current_state[i] = 0;
                }
                else
                {
                    current_state[i] = 1;
                }
            }

            dotprod = 0;
            for (i = 0; i < 25; i++)
            {
                dotprod = (dotprod + current_state[i] * n1[i]) % 2;
            }

            if (dotprod != 0) { return (false); }

            dotprod = 0;
            for (i = 0; i < 25; i++)
            {
                dotprod = (dotprod + current_state[i] * n2[i]) % 2;
            }

            if (dotprod != 0) { return (false); }
            else { return (true); }
        }

    }
}
