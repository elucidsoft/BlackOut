using System;
using Polenter.Serialization;

namespace BlackOut
{
    public class GameSettings
    {
        private string _backgroundColor;
        private bool? _playSounds;
        private bool? _showMoveCounter;
        private bool? _showTimer;
        private bool? _showHintCount;
        private bool? _showLevel;
        private double? _soundVolume;
        private string _theme;
        private bool _themeChanged = false;

        public string BackgroundColor
        {
            get
            {
                if (_backgroundColor == null)
                {
                    return App.Current.Resources["PhoneAccentColor"].ToString();
                }

                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
            }
        }

        public string Theme
        {
            get
            {
                if (_theme == null)
                    return String.Empty;

                return _theme;
            }
            set
            {
                _themeChanged = true;
                _theme = value;
            }
        }

        [ExcludeFromSerialization]
        public bool ThemeChanged
        {
            get { return _themeChanged; }
            set { _themeChanged = false; }
        }

        public bool PlaySounds
        {
            get
            {
                if (_playSounds == null)
                    return true;

                return _playSounds.Value;
            }

            set { _playSounds = value; }
        }

        public double SoundVolume
        {
            get
            {
                if (_soundVolume == null)
                    return 100;

                return _soundVolume.Value;
            }
            set
            {
                _soundVolume = value;
            }

        }

        public bool ShowMoveCounter
        {
            get
            {
                if (_showMoveCounter == null)
                    return true;

                return _showMoveCounter.Value;
            }
            set { _showMoveCounter = value; }
        }

        public bool ShowTimer
        {
            get
            {
                if (_showTimer == null)
                    return true;

                return _showTimer.Value;
            }
            set { _showTimer = value; }
        }

        public bool ShowHintCount 
        {
            get
            {
                if (_showHintCount == null)
                    return true;

                return _showHintCount.Value;
            }
            set
            {
                _showHintCount = value;
            }
        }

        public bool ShowLevel 
        {
            get
            {
                if (_showLevel == null)
                    return true;

                return _showLevel.Value;
            }
            set
            {
                _showLevel = value;
            }
        }


    }
}
