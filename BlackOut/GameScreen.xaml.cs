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
using System.Windows.Threading;
using System.Device.Location;
using System.Diagnostics;

namespace BlackOut
{
    public partial class GameScreen : PhoneApplicationPage
    {
        bool isDirty = false;
        bool adOverlayOpened = false;
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
            App.GameManager.LevelCompleted += new EventHandler<EventArgs>(Instance_LevelCompleted);
            App.GameManager.LevelLoaded += new EventHandler<LevelLoadedEventArgs>(Instance_LevelLoaded);
            App.GameManager.OnTimerChanged += new EventHandler<GameTimerTickEventArgs>(GameManager_OnTimerChanged);
            App.GameManager.OnGridBlockClicked += new EventHandler<GridBlockClickedEventArgs>(GameManager_OnGridBlockClicked);
            App.GameManager.OnHintUsed += new EventHandler<EventArgs>(GameManager_OnHintUsed);
            App.GameManager.OnResetBoardCompleted += new EventHandler<EventArgs>(GameManager_OnResetBoardCompleted);

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
            int totalLevels = App.GameManager.GameData.Levels.Count;
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

        private void appBarMnuMainMenu_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarMnuHighScores_Click(object sender, System.EventArgs e)
        {
            App.MainMenuSelectedIndex = 1;
            NavigationService.GoBack();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (adOverlayOpened)
            {
                EnableDisableAppBars(true);
                UpdatePreviousAndNextButtons();
                App.GameManager.Resume();
                adOverlayOpened = false;
            }
            else
            {
                App.GameManager.Pause();
            }
        }

        private void adControl_MMOverlayOpened(object sender, EventArgs e)
        {
            adOverlayOpened = true;
            EnableDisableAppBars(false);
            App.GameManager.Pause();
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

        private void adControl_AdEngaged(object sender, EventArgs e)
        {
            App.GameManager.Pause();
        }

        private void adControl_AdDisengaged(object sender, EventArgs e)
        {
            App.GameManager.Resume();
        }

        #region Ad Setup

        private void SetupAds()
        {
            adGrid.Children.Clear();
            AdControl adControl = new AdControl();
            adControl.Name = "adControl";
            adControl.ApplicationId = "7a6e48b6-2793-4796-9323-162bdbbf364a";
            adControl.AdUnitId = "10012784";
            adControl.RotationEnabled = true;
            adControl.AdControlError += new EventHandler<ErrorEventArgs>(adControl_AdControlError);

            SetAdGeoLocation();
            adGrid.Children.Add(adControl);

            DispatcherTimer checkAdTimer = new DispatcherTimer();
            checkAdTimer.Interval = TimeSpan.FromSeconds(30);
            checkAdTimer.Start();
            checkAdTimer.Tick += ((object sender, EventArgs e) =>
            {
                if (adControl.Visibility == Visibility.Collapsed)
                {
                    AddAdDuplex();
                    checkAdTimer.Stop();
                }
            });
        }

        private void SetAdGeoLocation()
        {
            gcw = new GeoCoordinateWatcher();

            gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            gcw.Start();
        }

        void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            AdControl adControl = adGrid.FindName("adControl") as AdControl;
            if (adControl != null)
            {
                adControl.Location = new Location(e.Position.Location.Latitude, e.Position.Location.Longitude);
            }
            gcw.Stop();
        }

        void adControl_AdControlError(object sender, ErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                AddAdDuplex();
            });
        }

        private void AddAdDuplex()
        {
            adGrid.Children.Clear();
            AdDuplex.AdControl adDuplex = new AdDuplex.AdControl();
            adDuplex.AppId = "1191";
            adDuplex.IsTest = false;
            adDuplex.VerticalAlignment = VerticalAlignment.Bottom;

            adGrid.Children.Add(adDuplex);
        }

        #endregion
    }
}