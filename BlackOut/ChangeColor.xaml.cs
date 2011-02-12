using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BlackOut
{
    public partial class ChangeColor : PhoneApplicationPage
    {
        public ChangeColor()
        {
            InitializeComponent();

            LoadTheme();
        }

        private void LoadTheme()
        {
            string theme = App.GameManager.GameData.GameSettings.Theme;
            if (theme == "aqua")
            {
                lpTheme.SelectedIndex = 0;
            }
            else if (theme == "led")
            {
                lpTheme.SelectedIndex = 2;
            }
            else
            {
                lpTheme.SelectedIndex = 1;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)((ListBoxItem)lstColor.SelectedItem).Foreground;
            App.GameManager.GameData.GameSettings.BackgroundColor = color.Color.ToString();

            NavigationService.GoBack();

            string theme = String.Empty;

            if (lpTheme.SelectedIndex == 0)
            {
                theme = "aqua";
            }
            else if (lpTheme.SelectedIndex == 2)
            {
                theme = "led";
            }

            App.GameManager.GameData.GameSettings.Theme = theme;
        }

        private void lpTheme_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadTheme();
        }
    }
}