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
using System.Windows.Resources;
using System.Threading;

namespace BlackOut
{
    public class GameManager
    {
        public event EventHandler<EventArgs> LevelWon;

        public int[,] level;

        private Block[,] board = new Block[5, 5];

        private static GameManager instance;

        public UIElementCollection uiElementCollection;
        public Color color;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public GameManager()
        {
            InitializeBlocks();
        }

        private void InitializeBlocks()
        {
            board = new Block[5, 5]
                { 
                     { new Block(), new Block(), new Block(), new Block(), new Block() },
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() }   
                };
        }

        public void DisplayGrid(bool isRefresh, bool isInitialize)
        {
            if (isInitialize)
            {
                uiElementCollection.Clear();
                InitializeBlocks();
            }

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Block block = board[column, row];
                    block.SetValue(Grid.RowProperty, row);
                    block.SetValue(Grid.ColumnProperty, column);
                    block.Width = 88;
                    block.Height = 88;
                    block.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    block.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);
                    block.column = column;
                    block.row = row;

                    if (level[column, row] > 0)
                    {
                        block.TurnOn();
                    }
                    else
                    {
                        block.TurnOff();
                    }

                    if (!isRefresh)
                    {
                        uiElementCollection.Add(block);
                    }
                }
            }
        }

        public bool CheckForWin()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (level[column, row] == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ResetBoard()
        {
            level = TempLevels.EmptyBoard;
            DisplayGrid(false, true);

            int delay = 0;
            TimeSpan beginTime = TimeSpan.FromSeconds(delay);
            for (int i = 0; i < 5; i++)
            {
                if (i > 0)
                {
                    if (delay == 0)
                        delay = 45;

                    beginTime = TimeSpan.FromMilliseconds(delay);
                    delay = delay += delay;
                }

                //board[0, i].FlashOnAnimation.BeginTime = beginTime;
                //board[0, i].FlashOnAnimation.Begin();

                //board[1, i].FlashOnAnimation.BeginTime = beginTime;
                //board[1, i].FlashOnAnimation.Begin();

                //board[2, i].FlashOnAnimation.BeginTime = beginTime;
                //board[2, i].FlashOnAnimation.Begin();

                //board[3, i].FlashOnAnimation.BeginTime = beginTime;
                //board[3, i].FlashOnAnimation.Begin();

                //board[4, i].FlashOnAnimation.BeginTime = beginTime;
                //board[4, i].FlashOnAnimation.Begin();
            }
        }


        internal void BlockClicked(int row, int column)
        {
            FlipBlock(column, row);

            if (row > 0)
            {
                FlipBlock(column, row - 1);
            }

            if (row < 4)
            {
                FlipBlock(column, row + 1);
            }

            if (column < 4)
            {
                FlipBlock(column + 1, row);
            }

            if (column > 0)
            {
                FlipBlock(column - 1, row);
            }

            DisplayGrid(true, false);

            Timer timer = new Timer((Object obj) =>
            {
                if (CheckForWin())
                {
                    if (LevelWon != null)
                        LevelWon(this, new EventArgs());               
                }
            }, null, 500, Timeout.Infinite);
        }

        private void FlipBlock(int column, int row)
        {
            level[column, row] = Math.Abs(level[column, row] - 1);
        }
    }
}
