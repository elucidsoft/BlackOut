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
    public class BoardAnimationManager
    {
        private Block[,] _board;
        private GameManager _gameManager;

        public BoardAnimationManager(GameManager gameManager, Block[,] board)
        {
            _gameManager = gameManager;
            _board = board;
        }

        public void RotateBoardBlocks(int startTime)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _board[column, row].TurnLeft.BeginTime = TimeSpan.FromMilliseconds(startTime);
                    _board[column, row].FlashOnAnimation.Begin();
                    _board[column, row].FlashOnAnimation.BeginTime = TimeSpan.FromMilliseconds(startTime);
                    _board[column, row].TurnLeft.Begin();
                }
            }
        }

        private int FlashLeftRight(int startTime, int speed)
        {
            int delay = 0;
            TimeSpan beginTime;
            for (int i = 0; i < 5; i++)
            {
                beginTime = TimeSpan.FromMilliseconds(startTime) + TimeSpan.FromMilliseconds(delay);
                if (i > 0)
                {
                    if (delay == 0)
                        delay = speed;

                    delay = delay += delay;
                }

                FlashColumnsBlocks(beginTime, i);
            }

            return delay;
        }

        public int FlashDown(int startTime, int speed)
        {
            int delay = 0;
            TimeSpan beginTime = new TimeSpan();
            for (int i = 0; i < 5; i++)
            {
                beginTime = TimeSpan.FromMilliseconds(startTime) + TimeSpan.FromMilliseconds(delay);
                if (i > 0)
                {
                    if (delay == 0)
                        delay = speed;

                    delay = delay += delay;
                }

                FlashRowBlocks(beginTime, i);
            }

            return delay;
        }

        private void FlashRowBlocks(TimeSpan beginTime, int i)
        {
            _board[0, i].FlashOnAnimation.BeginTime = beginTime;
            _board[0, i].FlashOnAnimation.Begin();

            _board[1, i].FlashOnAnimation.BeginTime = beginTime;
            _board[1, i].FlashOnAnimation.Begin();

            _board[2, i].FlashOnAnimation.BeginTime = beginTime;
            _board[2, i].FlashOnAnimation.Begin();

            _board[3, i].FlashOnAnimation.BeginTime = beginTime;
            _board[3, i].FlashOnAnimation.Begin();

            _board[4, i].FlashOnAnimation.BeginTime = beginTime;
            _board[4, i].FlashOnAnimation.Begin();

            if (i == 4)
            {
                _board[4, 4].FlashOnAnimation.Completed += new EventHandler(FlashOnRowsAnimation_Completed);
            }
        }

        private void FlashColumnsBlocks(TimeSpan beginTime, int i)
        {
            _board[i, 0].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 0].FlashOnAnimation.Begin();

            _board[i, 1].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 1].FlashOnAnimation.Begin();

            _board[i, 2].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 2].FlashOnAnimation.Begin();

            _board[i, 3].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 3].FlashOnAnimation.Begin();

            _board[i, 4].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 4].FlashOnAnimation.Begin();

            if (i == 4)
            {
                _board[4, 4].FlashOnAnimation.Completed += new EventHandler(FlashOnColumnsAnimation_Completed);
            }
        }

        void FlashOnColumnsAnimation_Completed(object sender, EventArgs e)
        {
            _board[4, 4].FlashOnAnimation.Completed -= new EventHandler(FlashOnColumnsAnimation_Completed);
            _gameManager.LoadLevel(_gameManager.Level);
            _gameManager.DisplayGrid(true, _gameManager.ActiveBoardLevel);//
        }

        void FlashOnRowsAnimation_Completed(object sender, EventArgs e)
        {
            FlashLeftRight(0, 35);
            _board[4, 4].FlashOnAnimation.Completed -= new EventHandler(FlashOnRowsAnimation_Completed);
        }
    }
}
