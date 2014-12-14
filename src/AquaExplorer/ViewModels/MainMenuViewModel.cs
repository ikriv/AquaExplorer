using System.IO;
using System.Windows.Input;
using AquaExplorer.Services;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Async;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;
        private readonly IFileDialogService _fileDialogs;
        private readonly INavigationController _controller;
        private readonly IDownloadController _downloadController;
        private readonly IConfigurationService _configService;

        public MainMenuViewModel(
            IDialogService dialogs, 
            IFileDialogService fileDialogs, 
            INavigationController controller, 
            IDownloadController downloadController,
            IConfigurationService configService)
        {
            _dialogs = dialogs;
            _fileDialogs = fileDialogs;
            _controller = controller;
            _downloadController = downloadController;
            _configService = configService;
            AddAccountCommand = new DelegateCommand(AddAccount);
            ReloadCommand = new DelegateCommand(_controller.Reload);
            SetDownloadFolderCommand = new DelegateCommand(SetDownloadFolder);
        }

        public ICommand AddAccountCommand { get; private set; }

        public ICommand ReloadCommand { get; private set; }

        public ICommand SetDownloadFolderCommand { get; private set; }

        private void AddAccount()
        {
            _dialogs.CreateDialog<AddAccountViewModel>().Show();
        }

        private void SetDownloadFolder()
        {
            var currentFolder = _downloadController.DownloadFolder;
            var newFolder =
                _fileDialogs.SelectFolder(new SelectFolderOptions
                {
                    Title = "Select Aqua Explorer download folder",
                    InitialDirectory = currentFolder
                });

            if (newFolder == null) return;
            var config = _configService.ReadConfiguration();
            config.DownloadFolder = newFolder;
            _configService.WriteConfiguration(config);
        }
    }
}
