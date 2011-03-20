using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace BlackOut
{
    public class LevelTransitionAnimationManager : IDisposable
    {
        private bool disposed = false;

        private Block[,] _board;
        private readonly GameManager _gameManager;
        private Action _action;
        private SoundEffect _swooshSound1 = null;
        private SoundEffect _swooshSound2 = null;
        private float volume = 0;

        public LevelTransitionAnimationManager(GameManager gameManager, Block[,] board)
        {
            volume = Convert.ToSingle(gameManager.GameData.GameSettings.SoundVolume / 100);

            using (Stream stream = TitleContainer.OpenStream("swoosh1.wav"))
                _swooshSound1 = SoundEffect.FromStream(stream);

            using (Stream stream = TitleContainer.OpenStream("swoosh2.wav"))
                _swooshSound2 = SoundEffect.FromStream(stream);

            _gameManager = gameManager;
            _board = board;
        }

        public void RotateBoardBlocks(int startTime)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    _board[column, row].FlashOnAnimation.Begin();
                    _board[column, row].FlashOnAnimation.BeginTime = TimeSpan.FromMilliseconds(startTime);

                    _board[column, row].TurnLeftAnimation.BeginTime = TimeSpan.FromMilliseconds(startTime + 250);
                    _board[column, row].TurnLeftAnimation.Begin();
                }

                if (_gameManager.GameData.GameSettings.PlaySounds)
                {
                    _swooshSound2.Play(1.00f, Convert.ToSingle(1 - (row + 1) * .10), 0);
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

        public void Begin(int startTime, int speed, Action action)
        {
            _action = action;
            RotateBoardBlocks(0);
            StartFlashRowBlocks(startTime, speed);
        }

        private int StartFlashRowBlocks(int startTime, int speed)
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

            if (_gameManager.GameData.GameSettings.PlaySounds)
            {
                _swooshSound1.Play(volume, Convert.ToSingle(1 - (i + 1) * .10), 0);
            }

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
                _board[4, 4].FlashOnAnimation.Completed += FlashOnRowsAnimation_Completed;
            }
        }

        private void FlashColumnsBlocks(TimeSpan beginTime, int i)
        {
            _board[i, 0].FlashOnAnimation.BeginTime = beginTime;
            _board[i, 0].FlashOnAnimation.Begin();

            if (_gameManager.GameData.GameSettings.PlaySounds)
            {
                _swooshSound1.Play(volume, Convert.ToSingle(1 - (i + 1) * .10), 0);
            }

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
                _board[4, 4].FlashOnAnimation.Completed += FlashOnColumnsAnimation_Completed;
            }
        }

        void FlashOnColumnsAnimation_Completed(object sender, EventArgs e)
        {
            _board[4, 4].FlashOnAnimation.Completed -= FlashOnColumnsAnimation_Completed;
            _action();
        }

        void FlashOnRowsAnimation_Completed(object sender, EventArgs e)
        {
            FlashLeftRight(0, 35);
            _board[4, 4].FlashOnAnimation.Completed -= FlashOnRowsAnimation_Completed;
        }

        
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
                if (_swooshSound1 != null)
                    _swooshSound1.Dispose();

                if (_swooshSound2 != null)
                    _swooshSound2.Dispose();
            }

            disposed = true;
        }

        ~LevelTransitionAnimationManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
