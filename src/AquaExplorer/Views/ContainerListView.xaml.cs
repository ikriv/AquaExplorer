using System.Windows.Controls;
using System.Windows.Input;
using AquaExplorer.ViewModels;

namespace AquaExplorer.Views
{
    /// <summary>
    /// Interaction logic for ContainerListView.xaml
    /// </summary>
    public partial class ContainerListView : UserControl
    {
        public ContainerListView()
        {
            InitializeComponent();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as ContainerListViewModel;
            if (vm == null) return;
            vm.OnDoubleClick();
        }
    }
}
