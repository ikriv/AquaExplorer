using System.Windows.Controls;
using System.Windows.Input;
using AquaExplorer.ViewModels;

namespace AquaExplorer.Views
{
    /// <summary>
    /// Interaction logic for BlobListView.xaml
    /// </summary>
    public partial class BlobListView : UserControl
    {
        public BlobListView()
        {
            InitializeComponent();
        }

        private void OnBlobListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as BlobListViewModel;
            if (vm == null) return;
            vm.DownloadCommand.Execute(null);
        }
    }
}
