using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;

namespace AquaExplorer.ViewModels
{
    class AccountListViewModel : ListViewModelBase<Account>
    {
        private readonly INavigationController _controller;
        private readonly IConfigurationService _configService;

        public AccountListViewModel(IConfigurationService configService, INavigationController controller)
        {
            _controller = controller;
            _configService = configService;
        }

        public Account SelectedAccount
        {
            get {  return _selectedAccount; }
            set {  _selectedAccount = value; RaisePropertyChanged("SelectedAccount"); }
        }
        private Account _selectedAccount;

        public void OnDoubleCLick()
        {
            NavigateToSelectedAccount();
        }

        private void NavigateToSelectedAccount()
        {
            if (SelectedAccount == null) return;
            var location = new AzureLocation(SelectedAccount, null);
            _controller.NavigateTo(location);
        }

        protected override IEnumerable<Account> Load(CancellationToken token)
        {
            var config = _configService.ReadConfiguration(); // make async!

            return config.Accounts
                .Select(c => new Account {Credentials = c})
                .OrderBy(a => a.Credentials.Account)
                .ToArray();
        }
    }
}
