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
    public class GameSettings
    {
        private string _backgroundColor;

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
    }
}
