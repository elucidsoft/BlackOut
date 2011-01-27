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
using System.Threading;
using Microsoft.Phone.Tasks;
using System.Windows.Threading;

namespace BlackOut
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Storyboard _phaseBackgroundAnimation = new Storyboard();
        private SolidColorBrush _backgroundBrush = new SolidColorBrush();
        bool isDirty = false;

        #region Constructor/Load

        public MainPage()
        {
            InitializeComponent();

            if (App.GameManager.GameData.GameSettings.BackgroundColor == null)
            {
                App.GameManager.GameData.GameSettings.BackgroundColor = Resources["PhoneAccentColor"].ToString();
            }

            LoadDifficulty();
            ShowContinue();
            LayoutRoot.Background = new SolidColorBrush(ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (isDirty == true)
            {
                if (((SolidColorBrush)LayoutRoot.Background).Color.ToString() != App.GameManager.GameData.GameSettings.BackgroundColor)
                {
                    panItemScores.Opacity = 0;
                    panItemAbout.Opacity = 0;
                    CreatePhaseBackgroundAnimation();
                    _phaseBackgroundAnimation.BeginTime = TimeSpan.FromMilliseconds(750);
                    _phaseBackgroundAnimation.Begin();
                    _phaseBackgroundAnimation.Completed += new EventHandler(PhaseBackgroundAnimation_Completed);
                }
            }
            else
            {
                ApplicationBar.IsVisible = true;
                LayoutRoot.Background = new SolidColorBrush(ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor));
            }

            AddHighScores();
            isDirty = true;
            LoadDifficulty();

            ShowContinue();
            ApplicationBar.IsVisible = true;
        }

        private void ShowContinue()
        {
            if (IsExistingGame())
            {
                tbDifficulty.Text = App.GameManager.GameData.DifficultyString();
                grdContinue.Visibility = Visibility.Visible;
            }
            else
            {
                grdContinue.Visibility = Visibility.Collapsed;
            }
        }

        private static bool IsExistingGame()
        {
            return App.GameManager.GameData.GameState.Seconds > 0 || App.GameManager.GameData.GameState.Level > 1 || App.GameManager.GameData.HighestLevel > 1 || App.GameManager.GameData.GameState.Moves > 0;
        }

        #endregion

        private void CreatePhaseBackgroundAnimation()
        {
            _phaseBackgroundAnimation = new Storyboard();
            _backgroundBrush = (SolidColorBrush)LayoutRoot.Background;
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(_phaseBackgroundAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(_phaseBackgroundAnimation, _backgroundBrush);

            LinearColorKeyFrame lck0 = new LinearColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(500);
            lck0.Value = ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor);

            ckf.KeyFrames.Add(lck0);
            _phaseBackgroundAnimation.Children.Add(ckf);
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            App.GameManager.ReInitialize();
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void btnContinue_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsExistingGame())
            {
                if (MessageBox.Show("You have an existing game, starting a new one will clear the progress of the previous!", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    App.GameManager.GameData.HighestLevel = 1;
                    App.GameManager.GameData.Scores.Clear();
                }
                else
                {
                    return;
                }
            }

            SetDifficulty();
            App.GameManager.ReInitialize();
            GameData.LoadPreBuiltLevels(App.GameManager.GameData);
            //GameData.LoadCustomLevels(App.GameManager.GameData);
            App.GameManager.Start(1);
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));

        }

        private void SetDifficulty()
        {
            if (lpDifficulty.SelectedIndex == 0)
            {
                App.GameManager.GameData.Difficulty = 1;
            }
            else if (lpDifficulty.SelectedIndex == 1)
            {
                App.GameManager.GameData.Difficulty = 2;
            }
            else
            {
                App.GameManager.GameData.Difficulty = 0;
            }
        }



        private void btnDesignBoard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.ReInitialize();
            App.GameManager.Start(-1);
            NavigationService.Navigate(new Uri("/CreateLevel.xaml", UriKind.Relative));
        }

        void PhaseBackgroundAnimation_Completed(object sender, EventArgs e)
        {
            panItemScores.Opacity = 1;
            panItemAbout.Opacity = 1;
            
            _phaseBackgroundAnimation.Completed -= new EventHandler(PhaseBackgroundAnimation_Completed);
        }

        private void AddHighScores()
        {
            if (App.GameManager.GameData.Scores.Count > 0)
            {
                gridNoScores.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridNoScores.Visibility = Visibility.Visible;
            }

            lstScores.Items.Clear();
            for (int i = 0; i < App.GameManager.GameData.Scores.Count; i++)
            {
                Score s = App.GameManager.GameData.Scores[i];
                ListBoxItem lbi = new ListBoxItem();
                lbi.Style = (Style)Resources["ListBoxItemStyle1"];
                lbi.DataContext = s;
                lbi.ApplyTemplate();
                lstScores.Items.Add(lbi);
            }
        }

        private void LoadDifficulty()
        {
            int difficulty = App.GameManager.GameData.Difficulty;

            if (difficulty == 0)
            {
                lpDifficulty.SelectedIndex = 2;
            }
            else if (difficulty == 1)
            {
                lpDifficulty.SelectedIndex = 0;
            }
            else if (difficulty == 2)
            {
                lpDifficulty.SelectedIndex = 1;
            }
        }

        private void lnkSupportEmail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask emailTask = new EmailComposeTask();
                emailTask.To = "support@elucidsoft.com";
                emailTask.Subject = "BlackOut Support";
                emailTask.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open the email application on the phone...");
            }
        }

        private void lnkSupportSite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                WebBrowserTask task = new WebBrowserTask();
                task.URL = "http://getsatisfaction.com/elucidsoft";
                task.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open the browser application on the phone...");
            }
        }

        private void lnkSite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                WebBrowserTask task = new WebBrowserTask();
                task.URL = "http://www.elucidsoft.com/";
                task.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open the browser application on the phone...");
            }
        }

        private void btnOtherGames_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                MarketplaceSearchTask task = new MarketplaceSearchTask();
                task.ContentType = MarketplaceContentType.Applications;
                task.SearchTerms = "Elucidsoft LLC";
                task.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open the marketplace on the phone...");
            }
        }

        private void panorama_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            panorama.DefaultItem = panorama.Items[App.MainMenuSelectedIndex];
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.MainMenuSelectedIndex == 1)
            {
                e.Cancel = true;
                NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
            }
        }

        private void appBarBtnTheme_Click(object sender, System.EventArgs e)
        {
            ApplicationBar.IsVisible = false;
            NavigationService.Navigate(new Uri("/ChangeColor.xaml", UriKind.Relative));
        }

        private void appBarBtnSettings_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void appBarBtnHowTo_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HowTo.xaml", UriKind.Relative));
        }
    }
}