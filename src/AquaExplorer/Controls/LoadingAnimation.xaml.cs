using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AquaExplorer.Controls
{
    /// <summary>
    /// Interaction logic for LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingAnimation : UserControl
    {
        private readonly Storyboard _loadingStoryBoard = new Storyboard();

        public LoadingAnimation()
        {
            InitializeComponent();

            Resources.Add("LoadingStoryBoard", _loadingStoryBoard);

            int circleCount = rp1.Children.Count;

            for (int i = 0; i < circleCount; i++)
            {
                FrameworkElement e = rp1.Children[i] as FrameworkElement;

                e.Opacity = .2;

                Storyboard storyBoard = new Storyboard();
                storyBoard.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                storyBoard.BeginTime = TimeSpan.FromMilliseconds(i * (1000 / circleCount));
                storyBoard.RepeatBehavior = RepeatBehavior.Forever;

                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 1;
                animation.To = .2;

                Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
                Storyboard.SetTargetName(animation, e.Name);

                storyBoard.Children.Add(animation);

                _loadingStoryBoard.Children.Add(storyBoard);
            }

            IsVisibleChanged += (sender, args)=>OnIsVisibleChanged();
            OnIsVisibleChanged();
        }

        private void OnIsVisibleChanged()
        {
            if (IsVisible)
                _loadingStoryBoard.Begin();

            else
                _loadingStoryBoard.Stop();
        }
    }
}
