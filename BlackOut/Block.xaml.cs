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

namespace BlackOut
{
    public partial class Block : UserControl
    {
        public int row = -1;
        public int column = -1;
        public bool isBlockLit = false;

        Storyboard TurnOnAnimation = new Storyboard();
        Storyboard TurnOffAnimation = new Storyboard();

        SolidColorBrush backgroundBrush = new SolidColorBrush();

        Color accentColor;
        Color backgroundColor;

        public Block()
        {
            InitializeComponent();

            accentColor = ((Color)Resources["PhoneAccentColor"]).Lerp(Colors.Black, 0.15f);
            backgroundColor = accentColor.Lerp(Colors.Black, 0.65f);

            backgroundBrush = new SolidColorBrush(backgroundColor);
            backgroundBrush = (SolidColorBrush)LayoutRoot.Background;

            LayoutRoot.Background = backgroundBrush;

            InitializeTurnOnStoryBoardAnimation();
            InitializeTurnOffStoryBoardAnimation();
        }

        private void InitializeTurnOffStoryBoardAnimation()
        {
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(TurnOffAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(TurnOffAnimation, backgroundBrush);

            LinearColorKeyFrame lck0 = new LinearColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(200);
            lck0.Value = backgroundColor;

            ckf.KeyFrames.Add(lck0);
            TurnOffAnimation.Children.Add(ckf);
        }

        private void InitializeTurnOnStoryBoardAnimation()
        {
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(TurnOnAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(TurnOnAnimation, backgroundBrush);

            LinearColorKeyFrame lck0 = new LinearColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(200);
            lck0.Value = accentColor;

            ckf.KeyFrames.Add(lck0);
            TurnOnAnimation.Children.Add(ckf);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.BlockClicked(row, column);
        }

        public void TurnOn()
        {
            if(!isBlockLit)
            {
                isBlockLit = true;
                TurnOnAnimation.Begin();
            }
        }

        public void TurnOff()
        {
            isBlockLit = false;
            TurnOffAnimation.Begin();
        }

        public void FlashOn()
        {
            Dispatcher.BeginInvoke(() =>
            {
                //TurnOnAnimation.Begin();
            });
        }

    }
}
