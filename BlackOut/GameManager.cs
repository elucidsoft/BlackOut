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
using System.Diagnostics;
using System.Collections.Generic;

namespace BlackOut
{
    public class GameManager
    {
        public void Reset()
        {
            if(_grid != null)
                _grid.Children.Clear();

            InitializeBlocks();
        }

        public event EventHandler<EventArgs> LevelLoaded;
        public event EventHandler<EventArgs> LevelCompleted;
        public event EventHandler<EventArgs> OnGridBlockClicked;

        private List<int> randomBoards = new List<int>();

        private Block[,] _boardBlocks;

        private Grid _grid;
        private GameData _gameData;
        private GameState _gameState;

        private LevelTransitionAnimationManager _levelTransitionAnimationManager;
        private ResetBoardAnimationManager _resetBoardAnimationManager;

        private int _usedHints = 0;

        public GameManager(GameData gameData)
        {
            _gameData = gameData;
            _gameState = gameData.GameState;
            InitializeBlocks();
        }

        public void Initialize(Grid grid)
        {
            _grid = grid;
            InitializeGridBlocks();
        }

        private void InitializeGridBlocks()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _grid.Children.Add(_boardBlocks[column, row]);
                }
            }
        }

        public void Start(int level)
        {
            _gameState.Level = level;
            LoadLevel(_gameState.Level);
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
            _levelTransitionAnimationManager = new LevelTransitionAnimationManager(this, _boardBlocks);
            _resetBoardAnimationManager = new ResetBoardAnimationManager(this, _boardBlocks);
        }

        public int[,] GetBoard()
        {
            int[,] board = new int[5, 5];

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    board[column, row] = _boardBlocks[column, row].isBlockLit ? 1 : 0;
                }
            }

            return board;
        }

        public void LoadLevel(int currentLevel)
        {
            int[,] levelToLoad = new int[5,5];

            if (currentLevel > -1 && currentLevel - 1 >= _gameData.Levels.Length)
            {
                currentLevel = 1;
                _gameState.Level = 1;
            }
            else if (currentLevel == -1)
            {
                _gameState.Level = -1;
                levelToLoad = TempLevels.EmptyBoard;
                return;
            }
            else
            {
                levelToLoad = _gameData.Levels[currentLevel - 1];
            }

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _gameState.BoardLevel[column, row] = levelToLoad[column, row];
                }
            }

            if (LevelLoaded != null)
                LevelLoaded(this, EventArgs.Empty);
        }

        public void DisplayGrid(bool isRefresh)
        {
            DisplayGrid(isRefresh, _gameState.BoardLevel);
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

                    if (_gameState.Level == -1)
                        block.TestBlock = true;

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
                }
            }
        }

        public int[,] GenerateRandomBoard()
        {
            int[,] board;
            Random numberOfBlocks = new Random();
            Random onOff1 = new Random();
            Random onOff2 = new Random();

            if (randomBoards.Count > 200)
                randomBoards.Clear();

            int tries = 0;
            do
            {
                board = new int[5, 5];
                for (int i = 0; i < numberOfBlocks.Next(0, 25); i++)
                {
                    board[i % 5, i / 5] = onOff1.Next(0, 2);
                }

                for (int i = 0; i < numberOfBlocks.Next(0, 25); i++)
                {
                    board[i / 5, i % 5] = onOff2.Next(0, 2);
                }

                for (int i = 24; i > numberOfBlocks.Next(0, 25); i--)
                {
                    board[i / 5, i % 5] = onOff2.Next(0, 2);
                }

                tries++;

                if (tries > 20)
                    break;
            } while (!(CheckEmptyBoard(board) > 3 && Solver.IsSolvable(board) && !randomBoards.Contains(board.GetHashCode())));

            randomBoards.Add(board.GetHashCode());
            
            Debug.WriteLine(board.GetHashCode() + " " + tries);
            return board;
        }

        public void ShowHint()
        {
            Solver solver = new Solver(_gameState.BoardLevel);
            int[,] hintBoard = solver.GetHint();
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (hintBoard[column, row] == 1)
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
                    if (_gameState.BoardLevel[column, row] == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ResetBoard(bool loadLevel)
        {
            _grid.Dispatcher.BeginInvoke(() =>
               {
                   DisplayGrid(true, TempLevels.EmptyBoard);

                   _resetBoardAnimationManager.Begin(0, () =>
                   {
                       if (loadLevel)
                       {
                           LoadLevel(_gameState.Level);
                       }
                       DisplayGrid(true);
                   });
               });
        }

        private void LevelTransition()
        {
            _grid.Dispatcher.BeginInvoke(() =>
            {
                DisplayGrid(true, TempLevels.EmptyBoard);
                _levelTransitionAnimationManager.Begin(0, 35, () =>
                {
                    LoadLevel(_gameState.Level);
                    DisplayGrid(true);
                });
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
                _gameState.Level++;
                Timer timer = null;
                timer = new Timer((object a) =>
                {
                    LevelTransition();
                    if (LevelCompleted != null)
                        LevelCompleted(this, EventArgs.Empty);

                    timer.Dispose();
                }, null, TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void FlipBlock(int column, int row)
        {
            _gameState.BoardLevel[column, row] = Math.Abs(_gameState.BoardLevel[column, row] - 1);
        }

        public int CheckEmptyBoard(int[,] board)
        {
            int total = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    total += board[column, row];
                }
            }
            return total;
        }

        public int Level
        {
            get { return _gameState.Level; }
        }

        public int UsedHints
        {
            get { return _usedHints; }
        }

        public int[,] ActiveBoardLevel
        {
            get { return _gameState.BoardLevel; }
        }

        public GameData GameData
        {
            get { return _gameData; }
        }

        public int[,] BoardLevel
        {
            get { return _gameState.BoardLevel; }
        }

        public void SetBoard(int[,] boardLevel)
        {
            _gameState.BoardLevel = boardLevel;
        }

        internal void TestBlockClicked(int row, int column)
        {
            if (OnGridBlockClicked != null)
                OnGridBlockClicked(this, EventArgs.Empty);
        }
    }
}
