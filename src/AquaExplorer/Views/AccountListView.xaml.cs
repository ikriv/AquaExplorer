using System.Windows.Controls;
using System.Windows.Input;
using AquaExplorer.ViewModels;

namespace AquaExplorer.Views
{
    /// <summary>
    /// Interaction logic for AccountListView.xaml
    /// </summary>
    public partial class AccountListView : UserControl
    {
        public AccountListView()
        {
            InitializeComponent();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as AccountListViewModel;
            if (vm == null) return;
            vm.OnDoubleCLick();
        }
    }
}
