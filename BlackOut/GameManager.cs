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
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        public event EventHandler<EventArgs> LevelLoaded;
        public event EventHandler<EventArgs> LevelCompleted;

        private Block[,] _boardBlocks;
        public int[,] _boardLevel = new int[5, 5]
                                    {
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0}
                                    };

        private UIElementCollection _gridBlockDisplayCollection;
        private GameSettings _gameSettings = new GameSettings();
        private BoardAnimationManager _boardAnimationManager;

        private int _level = 0;
        private int _usedHints = 0;

        public int Level
        {
            get { return _level; }
        }

        public int UsedHints
        {
            get { return _usedHints; }
        }

        public int[,] ActiveBoardLevel 
        {
            get { return _boardLevel; } 
        }

        public GameManager()
        {
            InitializeBlocks();
        }

        public void Initialize(UIElementCollection uiElementCollection)
        {
            _gridBlockDisplayCollection = uiElementCollection;
            LoadLevel(_level);
        }

        public void Start(int level)
        {
            _level = level;
        }

        public void InitializeBlocks()
        {
            _boardBlocks = new Block[5, 5]
                { 
                     { new Block(), new Block(), new Block(), new Block(), new Block() },
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() },   
                     { new Block(), new Block(), new Block(), new Block(), new Block() }   
                };
            _boardAnimationManager = new BoardAnimationManager(this, _boardBlocks);
        }

        public void LoadLevel(int currentLevel)
        {
            if (currentLevel - 1 >= _gameSettings.Levels.Length)
            {
                currentLevel = 1;
                this._level = 1;
            }

            int[,] levelToLoad = _gameSettings.Levels[currentLevel - 1];
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _boardLevel[column, row] = levelToLoad[column, row];
                }
            }

            if (LevelLoaded != null)
                LevelLoaded(this, new EventArgs());
        }

        public void DisplayGrid(bool isRefresh)
        {
            DisplayGrid(isRefresh, _boardLevel);
        }

        public void DisplayGrid(bool isRefresh, int[,] level)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Block block = _boardBlocks[column, row];
                    block.SetValue(Grid.RowProperty, row);
                    block.SetValue(Grid.ColumnProperty, column);
                    block.Width = 88;
                    block.Height = 88;
                    block.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    block.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);
                    block.column = column;
                    block.row = row;

                    RotateTransform rotateTransform = new RotateTransform();
                    rotateTransform.Angle = 0;

                    block.RenderTransform = rotateTransform;

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
                        _gridBlockDisplayCollection.Add(block);
                    }
                }
            }
        }

        public void ShowHint()
        {
            Solver solver = new Solver(_boardLevel);
            solver.GetHint();
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (solver.hint_board[column, row] == 1)
                    {
                        _boardBlocks[column, row].ShowHint();
                        return;
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
                    if (_boardLevel[column, row] == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ResetBoard()
        {
            _gridBlockDisplayCollection.Dispatcher.BeginInvoke(() =>
               {
                   DisplayGrid(true, TempLevels.EmptyBoard);

                   _boardAnimationManager.RotateBoardBlocks(0);
                   _boardAnimationManager.FlashDown(0, 35);
               });
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

            DisplayGrid(true);
            CheckIfLevelCompleted();
        }

        private void CheckIfLevelCompleted()
        {
            if (CheckForWin())
            {
                _level++; 
                Timer timer = null;
                timer = new Timer((object a) =>
                {
                    ResetBoard();
                    if (LevelCompleted != null)
                        LevelCompleted(this, new EventArgs());

                    timer.Dispose();
                }, null, TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void FlipBlock(int column, int row)
        {
            _boardLevel[column, row] = Math.Abs(_boardLevel[column, row] - 1);
        }
    }
}
