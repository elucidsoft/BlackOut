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
using System.Windows.Media.Imaging;

namespace BlackOut
{
    public partial class Block : UserControl
    {
        public int row = -1;
        public int column = -1;
        public bool isBlockLit = false;

        public Storyboard TurnOnAnimation = new Storyboard();
        public Storyboard TurnOffAnimation = new Storyboard();
        public Storyboard FlashOnAnimation = new Storyboard();

        public SolidColorBrush backgroundBrush = new SolidColorBrush();

        public Color accentColor;
        public Color backgroundColor;
        public bool TestBlock = false;

        public Block()
        {
            InitializeComponent();

            accentColor = ColorConverter.Convert(App.GameManager.GameData.GameSettings.BackgroundColor).Lerp(Colors.Black, 0.15f);
            backgroundColor = accentColor.Lerp(Colors.Black, 0.65f);

            backgroundBrush = new SolidColorBrush(backgroundColor);
            backgroundBrush = (SolidColorBrush)LayoutRoot.Background;

            LayoutRoot.Background = backgroundBrush;

            ApplyTheme();

            InitializeTurnOnStoryBoardAnimation();
            InitializeTurnOffStoryBoardAnimation();
            InitializeFlashOnStoryBoardAnimation();
        }

        private void ApplyTheme()
        {
            string theme = App.GameManager.GameData.GameSettings.Theme;
           
            if (theme == "aqua")
            {
                image.Opacity = .4;
                image.Source = new BitmapImage(new Uri("themes/aqua/block_overlay.jpg", UriKind.Relative));
            }
            else if (theme == "led")
            {
                image.Source = new BitmapImage(new Uri("themes/led/block_overlay.png", UriKind.Relative));
            }
        }

        private void InitializeTurnOffStoryBoardAnimation()
        {
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(TurnOffAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(TurnOffAnimation, backgroundBrush);

            LinearColorKeyFrame lck0 = new LinearColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(300);
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
            lck0.KeyTime = TimeSpan.FromMilliseconds(300);
            lck0.Value = accentColor;

            ckf.KeyFrames.Add(lck0);
            TurnOnAnimation.Children.Add(ckf);
        }

        private void InitializeFlashOnStoryBoardAnimation()
        {
            ColorAnimationUsingKeyFrames ckf = new ColorAnimationUsingKeyFrames();

            Storyboard.SetTargetProperty(FlashOnAnimation, new PropertyPath("Color"));
            Storyboard.SetTarget(FlashOnAnimation, backgroundBrush);

            EasingColorKeyFrame lck0 = new EasingColorKeyFrame();
            lck0.KeyTime = TimeSpan.FromMilliseconds(0);
            lck0.Value = accentColor;

            EasingColorKeyFrame lck1 = new EasingColorKeyFrame();
            lck1.KeyTime = TimeSpan.FromMilliseconds(600);
            lck1.Value = backgroundColor;

            ckf.KeyFrames.Add(lck0);
            ckf.KeyFrames.Add(lck1);
            FlashOnAnimation.Children.Add(ckf);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!TestBlock)
            {
                App.GameManager.BlockClicked(row, column, !isBlockLit);
            }
            else
            {
                if (!isBlockLit)
                    TurnOn(false);
                else
                    TurnOff(false);

                App.GameManager.TestBlockClicked(row, column);
            }
        }

        public void TurnOn(bool forceOn)
        {
            HintAnimation.Stop();
            ellipse.Opacity = 0;

            if(!isBlockLit || forceOn)
                TurnOnAnimation.Begin();

            isBlockLit = true;
        }

        public void TurnOff(bool forceOff)
        {
            HintAnimation.Stop();
            ellipse.Opacity = 0;
            
            if(isBlockLit || forceOff)
                TurnOffAnimation.Begin();

            isBlockLit = false;
        }

        public void FlashOn()
        {
            Dispatcher.BeginInvoke(() =>
            {
                TurnOnAnimation.Begin();
            });
        }

        internal void ShowHint()
        {
            HintAnimation.RepeatBehavior = new RepeatBehavior(5);
            HintAnimation.AutoReverse = true;

            ShowHintAnimation.Begin();

        }

        void ShowHintAnimation_Completed(object sender, EventArgs e)
        {
            HintAnimation.Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowHintAnimation.Completed += new EventHandler(ShowHintAnimation_Completed);
            HintAnimation.Completed += new EventHandler(HintAnimation_Completed);
        }

        void HintAnimation_Completed(object sender, EventArgs e)
        {
            HideHintAnimation.Begin();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ShowHintAnimation.Completed -= new EventHandler(ShowHintAnimation_Completed);
            HintAnimation.Completed -= new EventHandler(HintAnimation_Completed);
        }

    }
}
