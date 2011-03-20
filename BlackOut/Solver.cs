
namespace BlackOut
{
    public class Solver
    {
        int[,] _currentBoard;

        static readonly int[,] _hintBoard = new int[5, 5] {
                                {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0},
			                    {0, 0, 0, 0, 0}
                             };

        static readonly int[, ] _hintMatrix = {
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
        static readonly int[] _n1 = { 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1 };
        static readonly int[] _n2 = { 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1 };


        public Solver(int[,] board)
        {
            _currentBoard = board;
        }

        private static void CopyArray(int[] src, int[] dest)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }
        }

        private static void AddToArray(int[] src, int[] v)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = (src[i] + v[i]) % 2;
            }
        }

        private static int Weight(int[] v)
        {
            int t = 0;
            for (int i = 0; i < v.Length; i++)
            {
                t = t + v[i];
            }
            return (t);
        }

        public int[,] GetHint()
        {
            int[] currentState = new int[23];
            int[] hintVector = new int[25];
            int[] best = new int[25];
            int[] next = new int[25];

            for (int i = 0; i < 25; i++)
            {
                hintVector[i] = 0;
            }
            for (int i = 0; i < 23; i++)
            {
                if (_currentBoard[i % 5,i / 5] == 0)
                {
                    currentState[i] = 0;
                }
                else
                {
                    currentState[i] = 1;
                }
            }
            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    hintVector[i] =
                      (hintVector[i] + currentState[j] * _hintMatrix[i, j]) % 2;
                }
            }

            // Now we have a working hint vector, but we test h+n1, h+n2 
            // and h+n1+n2 to see which has the lowest weight, giving a
            // shortest solution.

            CopyArray(hintVector, best);
            CopyArray(hintVector, next);
            AddToArray(next, _n1);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }
            CopyArray(hintVector, next);
            AddToArray(next, _n2);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }
            CopyArray(hintVector, next);
            AddToArray(next, _n1);
            AddToArray(next, _n2);
            if (Weight(next) < Weight(best))
            {
                CopyArray(next, best);
            }

            for (int i = 0; i < 25; i++)
            {
                if (best[i] == 1) { _hintBoard[i % 5, i / 5] = 1; }
                else { _hintBoard[i % 5, i / 5] = 0; }
            }

            return _hintBoard;
        }

        public static bool IsSolvable(int[,] current_board)
        {
            int[] currentState = new int[25];
            int dotprod = 0;

            for (int i = 0; i < 25; i++)
            {
                if (current_board[i % 5, i / 5] == 1)
                {
                    currentState[i] = 0;
                }
                else
                {
                    currentState[i] = 1;
                }
            }
            for (int i = 0; i < 25; i++)
            {
                dotprod = (dotprod + currentState[i] * _n1[i]) % 2;
            }

            if (dotprod != 0) { return (false); }

            dotprod = 0;
            for (int i = 0; i < 25; i++)
            {
                dotprod = (dotprod + currentState[i] * _n2[i]) % 2;
            }

            if (dotprod != 0) { return (false); }
            else { return (true); }
        }

    }
}
