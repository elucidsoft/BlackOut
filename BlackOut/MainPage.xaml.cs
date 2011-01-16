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

namespace BlackOut
{
    public partial class MainPage : PhoneApplicationPage
    {
        public Storyboard PhaseBackgroundAnimation = new Storyboard();
        public SolidColorBrush backgroundBrush = new SolidColorBrush();

        bool isDirty = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (App.GameManager.GameData.GameSettings.BackgroundColor == null)
            {
                App.GameManager.GameData.GameSettings.BackgroundColor = Resources["PhoneAccentColor"].ToString();
            }

            LayoutRoot.Background = new SolidColorBrush(ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor));
        }

        private void CreatePhaseBackgroundAnimation()
        {
            PhaseBackgroundAnimation = new Storyboard();
            backgroundBrush = (SolidColorBrush)LayoutRoot.Background;
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(PhaseBackgroundAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(PhaseBackgroundAnimation, backgroundBrush);

            LinearColorKeyFrame lck0 = new LinearColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(500);
            lck0.Value = ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor);

            ckf.KeyFrames.Add(lck0);
            PhaseBackgroundAnimation.Children.Add(ckf);
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
               btnContinue.Visibility = Visibility.Visible;
            }
            else
            {
               btnContinue.Visibility = Visibility.Collapsed;
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
            NavigationService.Navigate(new Uri("/ChangeColor.xaml", UriKind.Relative));
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (isDirty == true)
            {
                panItemScores.Opacity = 0;           
                CreatePhaseBackgroundAnimation();
                PhaseBackgroundAnimation.BeginTime = TimeSpan.FromMilliseconds(2000);
                PhaseBackgroundAnimation.Begin();
                PhaseBackgroundAnimation.Completed += new EventHandler(PhaseBackgroundAnimation_Completed);
            }
            else
            {
                LayoutRoot.Background = new SolidColorBrush(ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor));
            }

            AddHighScores();
            isDirty = true;
        }

        void PhaseBackgroundAnimation_Completed(object sender, EventArgs e)
        {
            panItemScores.Opacity = 1;
            PhaseBackgroundAnimation.Completed -= new EventHandler(PhaseBackgroundAnimation_Completed);
        }

        private void AddHighScores()
        {
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
    }
}