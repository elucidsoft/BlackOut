using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BlackOut
{
    public class ResetBoardAnimationManager : IDisposable
    {
        private bool disposed = false;

        private Block[,] _board;
        private readonly GameManager _gameManager;
        private Action _action;
        private readonly SoundEffect _swooshSound = null;
        private readonly float volume = 0;

        public ResetBoardAnimationManager(GameManager gameManager, Block[,] board)
        {
            volume = Convert.ToSingle(gameManager.GameData.GameSettings.SoundVolume / 100);

            using (Stream stream = TitleContainer.OpenStream("swoosh2.wav"))
                _swooshSound = SoundEffect.FromStream(stream);

            _gameManager = gameManager;
            _board = board;
        }

        public void Begin(int startTime, int speed, Action action)
        {
            int delay = 0;
            TimeSpan beginTime = new TimeSpan();
            _action = action;

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
                FlipRowBlocks(beginTime, i);
            }
        }

        private void FlipRowBlocks(TimeSpan beginTime, int i)
        {
            _board[0, i].FlipAnimation.BeginTime = beginTime;
            _board[0, i].FlipAnimation.Begin();
            _board[1, i].FlipAnimation.BeginTime = beginTime;
            _board[1, i].FlipAnimation.Begin();
            _board[2, i].FlipAnimation.BeginTime = beginTime;
            _board[2, i].FlipAnimation.Begin();
            _board[3, i].FlipAnimation.BeginTime = beginTime;
            _board[3, i].FlipAnimation.Begin();
            _board[4, i].FlipAnimation.BeginTime = beginTime;
            _board[4, i].FlipAnimation.Begin();

            if (i == 4)
            {
                _board[4, 4].FlipAnimation.Completed += FlipAnimation_Completed;
            }
        }

        private void FlashRowBlocks(TimeSpan beginTime, int i)
        {
            _board[0, i].FlashOnAnimation.BeginTime = beginTime;
            _board[0, i].FlashOnAnimation.Begin();

            if (_gameManager.GameData.GameSettings.PlaySounds)
            {
                _swooshSound.Play(volume, Convert.ToSingle(1 - (i + 1) * .10), 0);
            }

            _board[1, i].FlashOnAnimation.BeginTime = beginTime;
            _board[1, i].FlashOnAnimation.Begin();

            _board[2, i].FlashOnAnimation.BeginTime = beginTime;
            _board[2, i].FlashOnAnimation.Begin();

            _board[3, i].FlashOnAnimation.BeginTime = beginTime;
            _board[3, i].FlashOnAnimation.Begin();

            _board[4, i].FlashOnAnimation.BeginTime = beginTime;
            _board[4, i].FlashOnAnimation.Begin();

        }

        void FlipAnimation_Completed(object sender, EventArgs e)
        {
            _action();
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
                if (_swooshSound != null)
                    _swooshSound.Dispose();
            }

            disposed = true;
        }

        ~ResetBoardAnimationManager()
        {
            Dispose(false);
        }

        #endregion

    }
}
