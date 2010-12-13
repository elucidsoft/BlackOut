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
using Microsoft.Advertising.Mobile.UI;

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
            App.GameManager.LevelLoaded += new EventHandler<LevelLoadedEventArgs>(Instance_LevelLoaded);
            App.GameManager.OnTimerChanged += new EventHandler<GameTimerTickEventArgs>(GameManager_OnTimerChanged);
            App.GameManager.OnGridBlockClicked += new EventHandler<GridBlockClickedEventArgs>(GameManager_OnGridBlockClicked);

            App.GameManager.Initialize(grid);
            App.GameManager.DisplayGrid(false);

            tbLevel.Text = App.GameManager.Level.ToString();
        }

        void GameManager_OnGridBlockClicked(object sender, GridBlockClickedEventArgs e)
        {
            tbMoves.Text = e.Moves.ToString();
        }

        void GameManager_OnTimerChanged(object sender, GameTimerTickEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                tbTime.Text = String.Format("{0:HH:mm:ss}", new DateTime(TimeSpan.FromSeconds(e.Seconds).Ticks));
            });
        }

        void Instance_LevelLoaded(object sender, LevelLoadedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                tbLevel.Text = e.Level.ToString();
            });
        }

        void Instance_LevelCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                tbMoves.Text = "0";
                tbHints.Text = "0";
                tbTime.Text = String.Format("{0:HH:mm:ss}", new DateTime(TimeSpan.FromSeconds(0).Ticks));
            });
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarBtnReset_Click(object sender, System.EventArgs e)
        {
            App.GameManager.ResetBoard(true);
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.LevelCompleted -= new EventHandler<EventArgs>(Instance_LevelCompleted);
            App.GameManager.LevelLoaded -= new EventHandler<LevelLoadedEventArgs>(Instance_LevelLoaded);
            App.GameManager.OnTimerChanged -= new EventHandler<GameTimerTickEventArgs>(GameManager_OnTimerChanged);
            grid.Children.Clear();
        }

        private void appBarBtnHint_Click(object sender, System.EventArgs e)
        {
            App.GameManager.ShowHint();
        }

        private void appBarBtnNextLevel_Click(object sender, System.EventArgs e)
        {
            int[,] board = App.GameManager.GenerateRandomBoard();
            App.GameManager.SetBoard(board);
            App.GameManager.ResetBoard(false);
        }
    }
}