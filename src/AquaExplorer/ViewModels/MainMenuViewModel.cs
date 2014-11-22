using System.Windows.Input;
using AquaExplorer.Services;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;
        private readonly INavigationController _controller;

        public MainMenuViewModel(IDialogService dialogs, INavigationController controller)
        {
            _dialogs = dialogs;
            _controller = controller;
            AddAccountCommand = new DelegateCommand(AddAccount);
            ReloadCommand = new DelegateCommand(_controller.Reload);
        }

        public ICommand AddAccountCommand { get; private set; }

        public ICommand ReloadCommand { get; private set; }

        private void AddAccount()
        {
            _dialogs.CreateDialog<AddAccountViewModel>().Show();
        }
    }
}
