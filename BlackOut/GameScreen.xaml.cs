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
using Microsoft.Phone.Shell;

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
            App.GameManager.OnHintUsed += new EventHandler<EventArgs>(GameManager_OnHintUsed);
            App.GameManager.OnResetBoardCompleted += new EventHandler<EventArgs>(GameManager_OnResetBoardCompleted);

            App.GameManager.Initialize(grid);
            App.GameManager.DisplayGrid(false);

            ResetDisplay();
            UpdatePreviousAndNextButtons();
        }

        void GameManager_OnResetBoardCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ResetDisplay();
            });
        }

        private void UpdatePreviousAndNextButtons()
        {
            if (App.GameManager.GameData.GameState.Level == 1)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            }
            else if (App.GameManager.GameData.GameState.Level == App.GameManager.GameData.Levels.Count)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = false;
            }
            else
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
            }
        }

        void GameManager_OnHintUsed(object sender, EventArgs e)
        {
            tbHints.Text = App.GameManager.GameData.GameState.HintsUsed.ToString();

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
                UpdatePreviousAndNextButtons();
            });
        }

        void Instance_LevelCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ResetDisplay();
            });
        }

        private void ResetDisplay()
        {
            tbLevel.Text = App.GameManager.Level.ToString();
            tbMoves.Text = App.GameManager.Moves.ToString();
            tbHints.Text = App.GameManager.HintsUsed.ToString();
            tbTime.Text = String.Format("{0:HH:mm:ss}", new DateTime(TimeSpan.FromSeconds(App.GameManager.Seconds).Ticks));
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarBtnReset_Click(object sender, System.EventArgs e)
        {
            App.GameManager.BeginResetBoard(true);
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.LevelCompleted -= new EventHandler<EventArgs>(Instance_LevelCompleted);
            App.GameManager.LevelLoaded -= new EventHandler<LevelLoadedEventArgs>(Instance_LevelLoaded);
            App.GameManager.OnTimerChanged -= new EventHandler<GameTimerTickEventArgs>(GameManager_OnTimerChanged);
            App.GameManager.OnGridBlockClicked -= new EventHandler<GridBlockClickedEventArgs>(GameManager_OnGridBlockClicked);
            App.GameManager.OnHintUsed -= new EventHandler<EventArgs>(GameManager_OnHintUsed);
            App.GameManager.OnResetBoardCompleted -= new EventHandler<EventArgs>(GameManager_OnResetBoardCompleted);


            grid.Children.Clear();
        }

        private void appBarBtnHint_Click(object sender, System.EventArgs e)
        {
            App.GameManager.ShowHint();
        }

        private void appBarBtnNextLevel_Click(object sender, System.EventArgs e)
        {
            int index = App.GameManager.Level + 1;
            if (index < App.GameManager.GameData.Levels.Count)
            {
                ResetDisplay();
                tbLevel.Text = index.ToString();
                App.GameManager.GameData.GameState.Level = index;
                App.GameManager.BeginLevelTransition();
                UpdatePreviousAndNextButtons();
            }
        }

        private void appBarBtnPrevLevel_Click(object sender, System.EventArgs e)
        {
            int index = App.GameManager.Level - 1;
            if (index > 0)
            {
                ResetDisplay();
                tbLevel.Text = index.ToString();
                App.GameManager.GameData.GameState.Level = index;
                App.GameManager.BeginLevelTransition();
                UpdatePreviousAndNextButtons();
            }
        }

        private void ApplicationBar_StateChanged(object sender, Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }
    }
}