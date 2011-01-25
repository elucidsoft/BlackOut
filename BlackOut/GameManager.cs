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
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Threading;

namespace BlackOut
{
    public class GameManager : IDisposable
    {
        private bool disposed = false;

        public event EventHandler<LevelLoadedEventArgs> LevelLoaded;
        public event EventHandler<EventArgs> LevelCompleted;
        public event EventHandler<GridBlockClickedEventArgs> OnGridBlockClicked;
        public event EventHandler<GameTimerTickEventArgs> OnTimerChanged;
        public event EventHandler<EventArgs> OnHintUsed;
        public event EventHandler<EventArgs> OnResetBoardCompleted;

        private List<int> _randomBoardHashes = new List<int>();
        private List<int> _usedHints = new List<int>();
        private Block[,] _boardBlocks;

        private bool _levelTransitionInProgress = false;
        private bool _resetBoardInProgress = false;

        private Grid _grid;
        private GameData _gameData;
        private GameState _gameState;
        private Timer _timer;
        private SoundEffect _soundEffectTileOn = null;
        private SoundEffect _soundEffectTileOff = null;

        private DispatcherTimer _audioTimer;
        private LevelTransitionAnimationManager _levelTransitionAnimationManager;
        private ResetBoardAnimationManager _resetBoardAnimationManager;
        private bool _paused = false;

        #region Initialization/Constructor/Begin/Reset

        public GameManager(GameData gameData)
        {
            _gameData = gameData;
            _gameState = gameData.GameState;

            SetupAudioTimer();
            
            using (Stream stream = TitleContainer.OpenStream("tile_on.wav"))
                _soundEffectTileOff = SoundEffect.FromStream(stream);

            using (Stream stream = TitleContainer.OpenStream("tile_on.wav"))
                _soundEffectTileOn = SoundEffect.FromStream(stream);

            TimerCallback tcb = Timer_Tick;
            _timer = new Timer(tcb, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void SetupAudioTimer()
        {
            _audioTimer = new DispatcherTimer();
            _audioTimer.Interval = TimeSpan.FromMilliseconds(500);
            _audioTimer.Tick += new EventHandler((object o, EventArgs e) =>
            {
                FrameworkDispatcher.Update();
            });
        }

        public void Initialize(Grid grid)
        {
            InitializeBlocks();
            _grid = grid;
            InitializeGridBlocks();
            _audioTimer.Start();
            _timer.Change(1000, 1000);
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
            Reset();
            _gameState.Level = level;
            LoadLevel(_gameState.Level);
        }

        public void ReInitialize()
        {
            if (_grid != null)
                _grid.Children.Clear();

            InitializeBlocks();
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

        #endregion

        #region GamePlay Methods

        public void LoadLevel(int currentLevel)
        {
            int[,] levelToLoad = new int[5, 5];

            if (currentLevel > -1 && currentLevel - 1 >= _gameData.Levels.Count - 1)
            {
                currentLevel = 1;
                _gameState.Level = 1;
                levelToLoad = _gameData.Levels[0];
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

            SetupHintMaximum();
            SetupHighestLevel();

            if (LevelLoaded != null)
                LevelLoaded(this, new LevelLoadedEventArgs(currentLevel));
        }

        private void SetupHighestLevel()
        {
            if (_gameData.HighestLevel < _gameState.Level)
            {
                _gameData.HighestLevel = _gameState.Level;
            }
        }

        private void SetupHintMaximum()
        {
            if (_gameData.Difficulty > 0)
            {
                _gameState.HintMax = GetHintList().Count / _gameData.Difficulty;
            }
            else
            {
                _gameState.HintMax = 0;
            }
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

            if (_randomBoardHashes.Count > 200)
                _randomBoardHashes.Clear();

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
            } while (!(CheckEmptyBoard(board) > 3 && Solver.IsSolvable(board) && !_randomBoardHashes.Contains(board.GetHashCode())));

            _randomBoardHashes.Add(board.GetHashCode());

            Debug.WriteLine(board.GetHashCode() + " " + tries);
            return board;
        }

        public void ShowHint()
        {
            if (_resetBoardInProgress || _levelTransitionInProgress)
                return;

            List<int[]> hints = GetHintList();

            //give a more natural feel to the hints
            Random random = new Random();
            int[] hint = hints[random.Next(0, hints.Count)];
            int count = 0;
            while (_usedHints.Count != 0 && _usedHints.Contains(ArrayHelper.GetHashCode(hint)))
            {
                if (count > hints.Count)
                    return;

                hint = hints[random.Next(0, hints.Count)];
                count++;
            }

            _boardBlocks[hint[0], hint[1]].ShowHint();
            _usedHints.Add(ArrayHelper.GetHashCode(hint));

            _gameState.HintsUsed++;

            if (OnHintUsed != null)
                OnHintUsed(this, EventArgs.Empty);

        }

        private List<int[]> GetHintList()
        {
            Solver solver = new Solver(_gameState.BoardLevel);
            int[,] hintBoard = solver.GetHint();
            List<int[]> hints = new List<int[]>();

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (hintBoard[column, row] == 1)
                    {
                        //_boardBlocks[column, row].ShowHint();
                        hints.Add(new int[] { column, row });
                    }
                }
            }
            return hints;
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

        public void BeginResetBoard(bool loadLevel)
        {
            if (!_levelTransitionInProgress && _resetBoardInProgress == false)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _resetBoardInProgress = true;
                Reset();
                _grid.Dispatcher.BeginInvoke(() =>
                {
                    DisplayGrid(true, TempLevels.EmptyBoard);

                    _resetBoardAnimationManager.Begin(0, 35, () =>
                    {
                        if (loadLevel)
                        {
                            LoadLevel(_gameState.Level);
                        }
                        DisplayGrid(true);
                        _timer.Change(500, 1000); //TODO: Refactor the way the timer works here...
                        _resetBoardInProgress = false;
                    });
                });
            }
        }

        public void BeginLevelTransition()
        {
            if (!_resetBoardInProgress && _levelTransitionInProgress == false) //prevents stacking animations
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _levelTransitionInProgress = true;
                Reset();
                _grid.Dispatcher.BeginInvoke(() =>
                {
                    DisplayGrid(true, TempLevels.EmptyBoard);

                    _levelTransitionAnimationManager.Begin(50, 35, () =>
                    {
                        LoadLevel(_gameState.Level);
                        DisplayGrid(true);
                        _timer.Change(500, 1000); //TODO: Refactor the way the timer works here...
                        _levelTransitionInProgress = false;
                    });
                });
            }
        }

        private void Reset()
        {
            _usedHints.Clear();
            _gameState.Seconds = 0;
            _gameState.Moves = 0;
            _gameState.HintsUsed = 0;

            if (OnResetBoardCompleted != null)
                OnResetBoardCompleted(this, EventArgs.Empty);
        }

        public void Pause()
        {
            _paused = true;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _audioTimer.Stop();
        }

        internal void BlockClicked(int row, int column, bool blockIsOn)
        {
            if (_levelTransitionInProgress || _resetBoardInProgress)
                return;

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

            _gameState.Moves++;
            _usedHints.Clear();

            float volume = Convert.ToSingle(_gameData.GameSettings.SoundVolume / 125);

            if (_gameData.GameSettings.PlaySounds)
            {
                if (blockIsOn)
                    App.GameManager._soundEffectTileOff.Play(volume, 0, 0);
                else
                    App.GameManager._soundEffectTileOn.Play(volume, 0, 0);
            }

            if (OnGridBlockClicked != null)
                OnGridBlockClicked(this, new GridBlockClickedEventArgs(_gameState.Moves));
        }

        private void CheckIfLevelCompleted()
        {
            if (CheckForWin())
            {
                AddScore();
                _gameState.Level++;
                
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(300);
                timer.Start();
                timer.Tick += new EventHandler((object o, EventArgs e) =>
                {
                    timer.Stop();
                    BeginLevelTransition();
                    if (LevelCompleted != null)
                        LevelCompleted(this, EventArgs.Empty);
                });
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

        private void AddScore()
        {
            GameState gs = _gameState;

            if (_gameData.Scores.Count < _gameState.Level)
            {
                _gameData.Scores.Add(new Score(gs.Level, gs.Level, gs.HintsUsed, gs.Seconds));
            }
            else
            {
                for (int i = 0; i < _gameData.Scores.Count; i++)
                {
                    Score s = _gameData.Scores[i];
                    if (s.Level == Level)
                    {
                        s.Hints = gs.HintsUsed;
                        s.Level = gs.Level;
                        s.Moves = gs.Moves;
                        s.Seconds = gs.Seconds;
                    }
                }
            }
        }

        internal void TestBlockClicked(int row, int column)
        {
            if (OnGridBlockClicked != null)
                OnGridBlockClicked(this, new GridBlockClickedEventArgs());
        }

        #endregion

        #region Event Handler Methods

        private void Timer_Tick(object obj)
        {
            _gameState.Seconds++;

            if (OnTimerChanged != null)
                OnTimerChanged(this, new GameTimerTickEventArgs(_gameState.Seconds));
        }

        #endregion

        #region Properties

        public int Level
        {
            get { return _gameState.Level; }
        }

        public int Moves
        {
            get { return _gameState.Moves; }
        }

        public int HintsUsed
        {
            get { return _gameState.HintsUsed; }
        }

        public int HintsMax 
        { 
            get { return _gameState.HintMax; } 
        }

        public long Seconds
        {
            get { return _gameState.Seconds; }
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


        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (_soundEffectTileOn != null)
                    _soundEffectTileOn.Dispose();

                if (_soundEffectTileOff != null)
                    _soundEffectTileOff.Dispose();

                if (_resetBoardAnimationManager != null)
                    _resetBoardAnimationManager.Dispose();

                if (_levelTransitionAnimationManager != null)
                    _levelTransitionAnimationManager.Dispose();

                if (_timer != null)
                    _timer.Dispose();
            }

            disposed = true;
        }

        ~GameManager()
        {
            Dispose(false);
        }

        #endregion

        
    }
}
