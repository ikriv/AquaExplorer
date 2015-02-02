using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.Util;
using AquaExplorer.ViewModels.Services;
using IKriv.Azure;
using IKriv.Windows.Mvvm;
using Microsoft.Practices.Unity;

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
            SearchBox.Clear();

            this.OnPropertyChanged("Items", () => RaisePropertyChanged("FilteredItems"));
            SearchBox.OnPropertyChanged("SearchString", ()=>RaisePropertyChanged("FilteredItems"));

            return this;
        }

        [Dependency]
        public ISearchBox SearchBox { get; set; }

        public Blob SelectedBlob
        {
            get { return _selectedBlob; }
            set { _selectedBlob = value; RaisePropertyChanged("SelectedBlob"); }
        }
        private Blob _selectedBlob;

        public IEnumerable<Blob> FilteredItems
        {
            get
            {
                if (Items == null) return null;
                if (SearchBox == null) return Items;
                if (String.IsNullOrEmpty(SearchBox.SearchString)) return Items;

                return Items.Where(blob => SubstringSearch.HasMatch(blob.Name, SearchBox.SearchString));
            }
        }

        public ICommand DownloadCommand { get; private set; }

        protected override IEnumerable<Blob> Load(CancellationToken token)
        {
            return _azure.GetBlobs(_location.Container).OrderBy(blob=>blob.Name).ToList();
        }

        private void DownloadBlob()
        {
            if (_selectedBlob == null) return;
            _downloadController.BeginDownload(_selectedBlob);
        }
    }
}
