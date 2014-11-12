using System;
using System.Windows;
using System.Windows.Input;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.ViewModels.Services;
using IKriv.Azure;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class AddAccountViewModel : ViewModelBase
    {
        private readonly IConfigurationService _config;
        private readonly CredentialsProtectionService _cred;
        private readonly IDialogService _dialogs;
        private readonly IWindow _window;

        public AddAccountViewModel(
                                  IConfigurationService config, 
                                  CredentialsProtectionService cred,
                                  IDialogService dialogs,
                                  IWindow window)
        {
            _config = config;
            _cred = cred;
            _dialogs = dialogs;
            _window = window;
            AddAndCloseCommand = new DelegateCommand(AddAndClose);
        }

        public ICommand AddAndCloseCommand { get; private set; }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; RaisePropertyChanged("AccountName"); }
        }
        private string _accountName;

        public string AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; RaisePropertyChanged("AccountKey"); }
        }
        private string _accountKey;

        public string Title { get { return "Add Account"; } }

        private void AddAndClose()
        {
            var account = GetAccount();
            if (account == null) return;
            var config = _config.ReadConfiguration();
            config.Accounts.Add(account.Credentials);

            try
            {
                _config.WriteConfiguration(config);
            }
            catch (Exception ex)
            {
                _dialogs.Show("Error saving configuration: " + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _window.Close();
        }

        private byte[] DecipherKey(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                _dialogs.Show("Please enter account key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            try
            {
                return Convert.FromBase64String(key);
            }
            catch
            {
                _dialogs.Show("Account key is not a valid Base64 string", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private Account GetAccount()
        {
            var key = DecipherKey(AccountKey);
            if (key == null) return null;

            if (String.IsNullOrEmpty(AccountName))
            {
                _dialogs.Show("Please enter account name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return new Account
            {
                Credentials = _cred.Protect(new Credentials(AccountName, key))
            };
        }
    }
}
