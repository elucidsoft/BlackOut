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
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.level = TempLevels.Level1Board;
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.level = TempLevels.Level2Board;
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.level = TempLevels.Level3Board;
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void button1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.level = TempLevels.Level4Board;
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));
        }

        private void button1_Copy3_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.level = TempLevels.Level5Board;
            NavigationService.Navigate(new Uri("/GameScreen.xaml", UriKind.Relative));            
        }
    }
}