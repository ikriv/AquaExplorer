using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AquaExplorer.Services;

namespace AquaExplorer.Controls
{
    class TextBlockWithHighlight : TextBlock, INotifyPropertyChanged
    {
        public string HighlightedText
        {
            get { return (string)GetValue(HighlightedTextProperty); }
            set { SetValue(HighlightedTextProperty, value); }
        }

        public static readonly DependencyProperty HighlightedTextProperty =
            DependencyProperty.Register("HighlightedText", typeof(string),
                typeof(TextBlockWithHighlight), 
                new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));


        public Brush HighlightedForeground
        {
            get { return (Brush)GetValue(HighlightedForegroundProperty); }
            set { SetValue(HighlightedForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightedForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightedForegroundProperty =
            DependencyProperty.Register("HighlightedForeground", typeof(Brush), 
            typeof(TextBlockWithHighlight), 
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender, ApplyHighlighting));

        public Brush HighlightedBackground
        {
            get { return (Brush)GetValue(HighlightedBackgroundProperty); }
            set { SetValue(HighlightedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightedBackground.  This enables animation, styling, binding, etc...
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

            Inlines.Clear();

            bool noHighlight = String.IsNullOrEmpty(highlighted);
            int index = noHighlight ? -1 : SubstringSearch.IndexOf(text, highlighted);

            HasMatch = noHighlight || index >= 0;

            if (index < 0) // highlighted text empty or not found
            {
                Inlines.Add(text);
                return;
            }

            if (index > 0) // there is some text before the highlighted part
            {
                Inlines.Add(text.Substring(0, index));
            }

            // add the highlighted part
            Inlines.Add(
                new Run(text.Substring(index, highlighted.Length))
                {
                    Background = HighlightedBackground,
                    Foreground = HighlightedForeground
                });

            int afterHighlightIdx = index + highlighted.Length; //move index to the end of the highlighted part

            if (afterHighlightIdx < text.Length) // there is some text after highlight
            {
                Inlines.Add(text.Substring(afterHighlightIdx));
            }
        }
    }
}
