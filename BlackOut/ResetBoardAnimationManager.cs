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
    public class ResetBoardAnimationManager
    {
        private Block[,] _board;
        private GameManager _gameManager;
        Action _action;

        public ResetBoardAnimationManager(GameManager gameManager, Block[,] board)
        {
            _gameManager = gameManager;
            _board = board;
        }

        public void Begin(int startTime, Action action)
        {
            _action = action;
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _board[column, row].FlashOnAnimation.Begin();
                    _board[column, row].FlashOnAnimation.BeginTime = TimeSpan.FromMilliseconds(startTime);

                    _board[column, row].FlipAnimation.BeginTime = TimeSpan.FromMilliseconds(startTime);
                    _board[column, row].FlipAnimation.Begin();

                    if (column == 4 && row == 4)
                    {
                        _board[4, 4].FlipAnimation.Completed += new EventHandler(FlipAnimation_Completed);
                    }
                }
            }
        }

        void FlipAnimation_Completed(object sender, EventArgs e)
        {
            _action();
        }
    }
}
