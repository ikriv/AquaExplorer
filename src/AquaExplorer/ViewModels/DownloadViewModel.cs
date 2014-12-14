using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class DownloadViewModel : ViewModelBase
    {
        private readonly IDownloadController _controller;
        private readonly IDialogService _dialogs;
        private readonly SizeAbbreviationService _abbreviationService;

        public DownloadViewModel(IDialogService dialogs, IDownloadController controller, SizeAbbreviationService abbreviationService)
        {
            _dialogs = dialogs;
            _controller = controller;
            _abbreviationService = abbreviationService;
        }

        public DownloadViewModel Init(DownloadOperation operation)
        {
            Operation = operation;
            Name = Path.GetFileName(Operation.LocalPath);
            CancelDownloadCommand = new DelegateCommand(CancelDownload);
            ShowErrorCommand = new DelegateCommand(ShowError);
            Update();
            return this;
        }

        public ICommand CancelDownloadCommand { get; private set; }
        public ICommand ShowErrorCommand { get; private set; }
        public DownloadOperation Operation { get; private set; }

        public string Name { get; private set; }
        public string Status { get; private set; }
        public bool ShowErrorLink { get; private set; }

        public void Update()
        {
            ShowErrorLink = false;

            if (Operation.IsInProgress)
            {
                if (Operation.DownloadedSize == 0)
                {
                    Status = "Starting...";
                }
                else
                {
                    Status = _abbreviationService.Abbreviate(Operation.DownloadedSize, Operation.TotalSize);    
                }
            }
            else
            {
                if (Operation.IsCanceled)
                {
                    Status = "Canceled";
                }
                else if (Operation.Error != null)
                {
                    Status = "Failed";
                    ShowErrorLink = true;
                }
                else
                {
                    Status = (Operation.IsCanceled) ? "Canceled" : "Completed";    
                }
            }

            RaisePropertyChanged("");
        }

        private void CancelDownload()
        {
            _controller.CancelDownload(Operation);
        }

        private void ShowError()
        {
            var dialog = _dialogs.CreateDialog<ExceptionViewModel>();
            var vm = dialog.ViewModel;
            vm.Title = "Download failed";
            vm.Header = String.Format("An error occured when downloading " + Name + ":");
            vm.Exception = Operation.Error;
            dialog.Show();
        }
    }
}
