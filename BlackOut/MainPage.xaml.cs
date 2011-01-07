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
using Microsoft.Phone.Notification;

namespace BlackOut
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            App.GameManager.ReInitialize();
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void btnContinue_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.GameManager.GameData.GameState.Seconds > 0)
            {
               // btnContinue.Visibility = Visibility.Visible;
            }
            else
            {
               // btnContinue.Visibility = Visibility.Collapsed;
            }
        }

        private void btnNewGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.ReInitialize();
            GameData.LoadPreBuiltLevels(App.GameManager.GameData);
            //GameData.LoadCustomLevels(App.GameManager.GameData);
            App.GameManager.Start(1);
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void btnChangeColor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void btnDesignBoard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.ReInitialize();
            App.GameManager.Start(-1);
            NavigationService.Navigate(new Uri("/CreateLevel.xaml", UriKind.Relative));
        }

        private void btnSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}