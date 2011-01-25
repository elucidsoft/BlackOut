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
using Microsoft.Phone.Tasks;

namespace BlackOut
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void btnClearHighScores_Click(object sender, RoutedEventArgs e)
        {
            App.GameManager.GameData.Scores.Clear();
        }

        private void btnRateAndReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
            catch
            {
                MessageBox.Show("Could not open marketplace on phone...");
            }
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtSliderVolume != null)
            {
                double volume = Math.Round(sliderVolume.Value, 0);
                txtSliderVolume.Text = volume + "%";
                App.GameManager.GameData.GameSettings.SoundVolume = volume;
            }
        }

        private void tglPlaySounds_Click(object sender, RoutedEventArgs e)
        {
            sliderVolume.IsEnabled = tglPlaySounds.IsChecked.Value;
            App.GameManager.GameData.GameSettings.PlaySounds = tglPlaySounds.IsChecked.Value;
        }

        private void sliderVolume_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtSliderVolume.Text = (sliderVolume.IsEnabled) ? Math.Round(sliderVolume.Value, 0).ToString() + "%" : "Disabled";
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            tglPlaySounds.IsChecked = App.GameManager.GameData.GameSettings.PlaySounds;
            sliderVolume.IsEnabled = App.GameManager.GameData.GameSettings.PlaySounds;
            sliderVolume.Value = App.GameManager.GameData.GameSettings.SoundVolume;
            tglShowMoveCounter.IsChecked = App.GameManager.GameData.GameSettings.ShowMoveCounter;
            tglShowLevel.IsChecked = App.GameManager.GameData.GameSettings.ShowLevel;
            tglShowTimer.IsChecked = App.GameManager.GameData.GameSettings.ShowTimer;
            tglShowHints.IsChecked = App.GameManager.GameData.GameSettings.ShowHintCount;
        }

        private void tglShowMoveCounter_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.GameData.GameSettings.ShowMoveCounter = tglShowMoveCounter.IsChecked.Value;
        }

        private void tglShowTimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.GameData.GameSettings.ShowTimer = tglShowTimer.IsChecked.Value;
        }

        private void tglShowLevel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.GameData.GameSettings.ShowLevel = tglShowLevel.IsChecked.Value;
        }

        private void tglShowHints_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.GameManager.GameData.GameSettings.ShowHintCount = tglShowHints.IsChecked.Value;
        }
    }
}