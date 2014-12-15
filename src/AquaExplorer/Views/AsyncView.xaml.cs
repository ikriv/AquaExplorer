using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AquaExplorer.Views
{
    /// <summary>
    /// Interaction logic for AsyncView.xaml
    /// </summary>
    [ContentProperty("Result")]
    public partial class AsyncView : UserControl
    {
        public AsyncView()
        {
            InitializeComponent();
        }


        public object Result
        {
            get { return (object)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Result.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(object), typeof(AsyncView), new PropertyMetadata(null));
    }
}
