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
            App.GameManager.LevelCompleted += new EventHandler<EventArgs>(Instance_LevelCompleted);
            App.GameManager.LevelLoaded += new EventHandler<EventArgs>(Instance_LevelLoaded);

           App.GameManager.Initialize(grid);
           App.GameManager.DisplayGrid(false);
        }

        void Instance_LevelLoaded(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                tbLevel.Text = App.GameManager.Level.ToString();
            });
        }

        void Instance_LevelCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                tbLevel.Text = App.GameManager.Level.ToString();
            });
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarBtnReset_Click(object sender, System.EventArgs e)
        {
           App.GameManager.ResetBoard();
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
           App.GameManager.LevelCompleted -= new EventHandler<EventArgs>(Instance_LevelCompleted);
            grid.Children.Clear();
        }

        private void appBarBtnHint_Click(object sender, System.EventArgs e)
        {
           App.GameManager.ShowHint();
        }   
    }
}