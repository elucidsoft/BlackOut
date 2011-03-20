using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Device.Location;
using Google.AdMob.Ads.WindowsPhone7;

namespace BlackOut
{
    public partial class GameScreen : PhoneApplicationPage
    {
        bool isDirty = false;
        bool adShown = false;

        GeoCoordinateWatcher gcw;

        public GameScreen()
        {
            InitializeComponent();

            App.GameManager.Initialize(grid);
            App.GameManager.DisplayGrid(false);
            isDirty = true;

            ShowLabels();
            SetupAds();
        }

        private void ShowLabels()
        {
            grdMovesLabel.Visibility = (App.GameManager.GameData.GameSettings.ShowMoveCounter == false) ? Visibility.Collapsed : Visibility.Visible;
            grdHintsLabel.Visibility = (App.GameManager.GameData.GameSettings.ShowHintCount == false) ? Visibility.Collapsed : Visibility.Visible;
            grdLevelLabel.Visibility = (App.GameManager.GameData.GameSettings.ShowLevel == false) ? Visibility.Collapsed : Visibility.Visible;
            grdTimeLabel.Visibility = (App.GameManager.GameData.GameSettings.ShowTimer == false) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.LevelCompleted += Instance_LevelCompleted;
            App.GameManager.LevelLoaded += Instance_LevelLoaded;
            App.GameManager.OnTimerChanged += GameManager_OnTimerChanged;
            App.GameManager.OnGridBlockClicked += GameManager_OnGridBlockClicked;
            App.GameManager.OnHintUsed += GameManager_OnHintUsed;
            App.GameManager.OnResetBoardCompleted += GameManager_OnResetBoardCompleted;

            App.MainMenuSelectedIndex = 0;
            if (isDirty == false)
            {
                App.GameManager.Initialize(grid);
                App.GameManager.DisplayGrid(false);
            }

            ResetDisplay();
            tbHints.Text = String.Format("{0}/{1}", App.GameManager.HintsUsed, App.GameManager.HintsMax);
            UpdatePreviousAndNextButtons();
            isDirty = false;

            if (adShown == true)
            {
                adShown = false;
                App.GameManager.Resume();
            }
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
            int currentLevel = App.GameManager.GameData.GameState.Level;
            int highestLevel = App.GameManager.GameData.HighestLevel;
            int hintMax = App.GameManager.GameData.GameState.HintMax;

            ApplicationBarIconButton abiBtnHint = ((ApplicationBarIconButton)ApplicationBar.Buttons[3]);
            ApplicationBarIconButton abiBtnPrev = ((ApplicationBarIconButton)ApplicationBar.Buttons[0]);
            ApplicationBarIconButton abiBtnNext = ((ApplicationBarIconButton)ApplicationBar.Buttons[1]);

            abiBtnHint.IsEnabled = false;
            abiBtnPrev.IsEnabled = false;
            abiBtnNext.IsEnabled = false;

            if (hintMax > 0 && App.GameManager.GameData.GameSettings.ShowHintCount == true)
            {
                abiBtnHint.IsEnabled = true;
            }

            if (currentLevel == 1 && highestLevel > 1)
            {
                abiBtnNext.IsEnabled = true;
            }

            if (currentLevel > 1)
            {
                abiBtnPrev.IsEnabled = true;
            }

            if (currentLevel < highestLevel)
            {
                abiBtnNext.IsEnabled = true;
            }

            if (App.GameManager.HintsUsed >= App.GameManager.HintsMax)
            {
                abiBtnHint.IsEnabled = false;
            }
        }

        void GameManager_OnHintUsed(object sender, EventArgs e)
        {
            tbHints.Text = String.Format("{0}/{1}", App.GameManager.HintsUsed, App.GameManager.HintsMax);
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
                tbHints.Text = String.Format("{0}/{1}", App.GameManager.HintsUsed, App.GameManager.HintsMax);
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
            tbHints.Foreground = new SolidColorBrush(Colors.White);
            tbLevel.Text = App.GameManager.Level.ToString();
            tbMoves.Text = App.GameManager.Moves.ToString();
            tbHints.Text = String.Format("{0}/{1}", "0", "-");
            tbTime.Text = String.Format("{0:HH:mm:ss}", new DateTime(TimeSpan.FromSeconds(App.GameManager.Seconds).Ticks));
        }

        private void appBarBtnReset_Click(object sender, EventArgs e)
        {
            App.GameManager.BeginResetBoard(true);
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.LevelCompleted -= Instance_LevelCompleted;
            App.GameManager.LevelLoaded -= Instance_LevelLoaded;
            App.GameManager.OnTimerChanged -= GameManager_OnTimerChanged;
            App.GameManager.OnGridBlockClicked -= GameManager_OnGridBlockClicked;
            App.GameManager.OnHintUsed -= GameManager_OnHintUsed;
            App.GameManager.OnResetBoardCompleted -= GameManager_OnResetBoardCompleted;


            grid.Children.Clear();
        }

        private void appBarBtnHint_Click(object sender, EventArgs e)
        {
            App.GameManager.ShowHint();
            if (App.GameManager.HintsUsed >= App.GameManager.HintsMax)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;
            }

            if (((App.GameManager.HintsUsed) / 3) == (App.GameManager.HintsMax / 3))
            {
                tbHints.Foreground = new SolidColorBrush(Colors.Yellow);
            }

            if (((App.GameManager.HintsUsed) / 2) == (App.GameManager.HintsMax / 2))
            {
                tbHints.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void appBarBtnNextLevel_Click(object sender, EventArgs e)
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

        private void appBarBtnPrevLevel_Click(object sender, EventArgs e)
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

        private void appBarMnuMainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarMnuHighScores_Click(object sender, EventArgs e)
        {
            App.MainMenuSelectedIndex = 1;
            NavigationService.GoBack();
        }

         private void EnableDisableAppBars(bool state)
        {
            ApplicationBarIconButton abiBtnPrev = ((ApplicationBarIconButton)ApplicationBar.Buttons[0]);
            ApplicationBarIconButton abiBtnNext = ((ApplicationBarIconButton)ApplicationBar.Buttons[1]);
            ApplicationBarIconButton abiBtnReset = ((ApplicationBarIconButton)ApplicationBar.Buttons[2]);
            ApplicationBarIconButton abiBtnHint = ((ApplicationBarIconButton)ApplicationBar.Buttons[3]);
            ApplicationBarMenuItem abmItmMainMenu = ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]);
            ApplicationBarMenuItem abmItmHighScores = ((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]);

            abmItmMainMenu.IsEnabled = state;
            abmItmHighScores.IsEnabled = state;
            abiBtnReset.IsEnabled = state;
            abiBtnHint.IsEnabled = state;
            abiBtnPrev.IsEnabled = state;
            abiBtnNext.IsEnabled = state;
        }

        #region Ad Setup

        private void SetupAds()
        {
           
        }

        private void SetAdGeoLocation()
        {
            gcw = new GeoCoordinateWatcher();

            gcw.PositionChanged += gcw_PositionChanged;
            gcw.Start();
        }

        void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                adControl.GpsLocation = null;
            }
            else
            {
                adControl.GpsLocation = new GpsLocation()
                {
                    Accuracy = e.Position.Location.HorizontalAccuracy,
                    Latitude = e.Position.Location.Latitude,
                    Longitude = e.Position.Location.Longitude
                };
            }
        } 
        
        private void adControl_AdPresentingScreen(object sender, RoutedEventArgs e)
        {
            adShown = true;
            App.GameManager.Pause();
        }

        #endregion

    }
}