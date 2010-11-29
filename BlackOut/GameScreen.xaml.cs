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
    public partial class GameScreen : PhoneApplicationPage
    {
        public GameScreen()
        {
            InitializeComponent();
            
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.LevelWon += new EventHandler<EventArgs>(Instance_LevelWon);
            GameManager.Instance.uiElementCollection = grid.Children;
            GameManager.Instance.color = (Color)Resources["PhoneAccentColor"];
            GameManager.Instance.DisplayGrid(false, true);
        }

        void Instance_LevelWon(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Woo! You Won!!");
                NavigationService.GoBack();
            });
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarBtnReset_Click(object sender, System.EventArgs e)
        {
            GameManager.Instance.ResetBoard();
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.LevelWon -= new EventHandler<EventArgs>(Instance_LevelWon);

        }   
    }
}