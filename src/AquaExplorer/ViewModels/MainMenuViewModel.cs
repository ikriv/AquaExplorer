using System.Windows.Input;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;

        public MainMenuViewModel(IDialogService dialogs)
        {
            _dialogs = dialogs;
            AddAccountCommand = new DelegateCommand(AddAccount);
        }

        public ICommand AddAccountCommand { get; private set; }

        private void AddAccount()
        {
            _dialogs.CreateDialog<AddAccountViewModel>().Show();
        }
    }
}
