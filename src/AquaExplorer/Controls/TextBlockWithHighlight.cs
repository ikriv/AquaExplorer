using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using AquaExplorer.Util;

namespace AquaExplorer.Controls
{
    [ContentProperty("Text")]
    public class TextBlockWithHighlight : Control, INotifyPropertyChanged
    {
        private readonly TextBlock _textBlock;

        public TextBlockWithHighlight()
        {
            // alignment defaults
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            _textBlock = new TextBlock();

            AddVisualChild(_textBlock);
        }

        public string HighlightedText
        {
            get { return (string)GetValue(HighlightedTextProperty); }
            set { SetValue(HighlightedTextProperty, value); }
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockWithHighlight), 
                new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));


        public static readonly DependencyProperty HighlightedTextProperty =
            DependencyProperty.Register("HighlightedText", typeof(string),
                typeof(TextBlockWithHighlight), 
                new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));


        public Brush HighlightedForeground
        {
            get { return (Brush)GetValue(HighlightedForegroundProperty); }
            set { SetValue(HighlightedForegroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedForegroundProperty =
            DependencyProperty.Register("HighlightedForeground", typeof(Brush), 
            typeof(TextBlockWithHighlight), 
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));

        public Brush HighlightedBackground
        {
            get { return (Brush)GetValue(HighlightedBackgroundProperty); }
            set { SetValue(HighlightedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedBackgroundProperty =
            DependencyProperty.Register("HighlightedBackground", typeof(Brush), 
            typeof(TextBlockWithHighlight), 
            new FrameworkPropertyMetadata(Brushes.Blue, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));

        private bool _hasMatch = true;
        public bool HasMatch
        {
            get { return _hasMatch; }
            set
            {
                if (_hasMatch != value)
                {
                    _hasMatch = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("HasMatch"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0) return _textBlock;
            return null;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _textBlock.Measure(constraint);
            return _textBlock.DesiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _textBlock.Arrange(new Rect(new Point(0,0), arrangeBounds));
            return _textBlock.DesiredSize;
        }


        private static void ApplyHighlighting(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as TextBlockWithHighlight;
            if (tb == null) return;
            tb.ApplyHighlighting();
        }

        private void ApplyHighlighting()
        {
            string highlighted = HighlightedText;
            string text = Text;

            _textBlock.Inlines.Clear();

            var segments = StringHighlighter.GetSegments(text, highlighted);
            foreach (var segment in segments)
            {
                var str = text.Substring(segment.Start, segment.End - segment.Start);
                var run = new Run(str);

                if (segment.IsHighlighted)
                {
                    run.Background = HighlightedBackground;
                    run.Foreground = HighlightedForeground;
                }

                _textBlock.Inlines.Add(run);
            }
        }
    }
}
