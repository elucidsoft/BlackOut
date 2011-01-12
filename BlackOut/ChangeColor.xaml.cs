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
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)((ListBoxItem)lstColor.SelectedItem).Foreground;
            App.GameManager.GameData.GameSettings.BackgroundColor = color.Color.ToString();

            NavigationService.GoBack();
        }
    }
}