using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using IKriv.Azure;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class BlobListViewModel : ListViewModelBase<Blob>
    {
        private readonly IAzureService _azure;
        private readonly IDownloadController _downloadController;
        private AzureLocation _location;

        public BlobListViewModel(IAzureService azure, IDownloadController downloadController)
        {
            _azure = azure;
            _downloadController = downloadController;
            DownloadCommand = new DelegateCommand(DownloadBlob);
        }

        public BlobListViewModel Init(AzureLocation location)
        {
            _location = location;
            return this;
        }

        public Blob SelectedBlob
        {
            get { return _selectedBlob; }
            set { _selectedBlob = value; RaisePropertyChanged("SelectedBlob"); }
        }
        private Blob _selectedBlob;

        public ICommand DownloadCommand { get; private set; }

        protected override IEnumerable<Blob> Load(CancellationToken token)
        {
            return _azure.GetBlobs(_location.Container);
        }

        private void DownloadBlob()
        {
            if (_selectedBlob == null) return;
            _downloadController.BeginDownload(_selectedBlob);
        }


    }
}
