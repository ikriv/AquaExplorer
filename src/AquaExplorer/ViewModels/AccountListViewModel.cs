using System;
using System.Collections.ObjectModel;
using System.Linq;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class AccountListViewModel : ViewModelBase, INavigationTarget
    {
        private readonly INavigationController _controller;

        public AccountListViewModel(IConfigurationService configService, INavigationController controller)
        {
            _controller = controller;

            var config = configService.ReadConfiguration(); // make async!

            Accounts = 
                new ObservableCollection<Account>(
                    config.Accounts
                        .Select(c=>new Account { Credentials = c })
                        .OrderBy(a=>a.Credentials.Account));
        }
        public ObservableCollection<Account> Accounts {  get; private set; }

        public Account SelectedAccount
        {
            get {  return _selectedAccount; }
            set {  _selectedAccount = value; RaisePropertyChanged("SelectedAccount"); }
        }
        private Account _selectedAccount;

        public void BeginLoad()
        {
            if (LoadCompleted != null) LoadCompleted(this);
        }

        public void BeginStopLoading()
        {
            if (LoadCompleted != null) LoadCompleted(this);
        }

        public void OnDoubleCLick()
        {
            NavigateToSelectedAccount();
        }

        public event Action<INavigationTarget> LoadCompleted;

        private void NavigateToSelectedAccount()
        {
            if (SelectedAccount == null) return;
            var location = new AzureLocation(SelectedAccount, null);
            _controller.NavigateTo(location);
        }
    }
}
