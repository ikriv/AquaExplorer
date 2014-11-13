using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class DownloadBarViewModel : ViewModelBase
    {
        private readonly IDownloadController _controller;
        private readonly Func<DownloadViewModel> _createDownloadVm;

        public DownloadBarViewModel(IDownloadController controller, Func<DownloadViewModel> createDownloadVm)
        {
            _controller = controller;
            _createDownloadVm = createDownloadVm;
            Downloads = new ObservableCollection<DownloadViewModel>();

            _controller.Downloads.CollectionChanged += (sender, args) => OnDownloadsChanged();
            _controller.DownloadProgress += OnDownloadProgress;
            OnDownloadsChanged();
        }

        private void OnDownloadsChanged()
        {
            Downloads =
                _controller.Downloads
                    .OrderByDescending(d => d.StartTime)
                    .Select(d => _createDownloadVm().Init(_controller, d))
                    .ToList();

            _downloadsDictionary = Downloads.ToDictionary(d => d.Operation);
        }

        private void OnDownloadProgress(DownloadOperation operation)
        {
            DownloadViewModel vm;
            if (!_downloadsDictionary.TryGetValue(operation, out vm)) return;
            vm.Update();
        }

        public IEnumerable<DownloadViewModel> Downloads
        {
            get { return _downloads; }
            set { _downloads = value; RaisePropertyChanged("Downloads"); }
        }
        private IEnumerable<DownloadViewModel> _downloads;

        private Dictionary<DownloadOperation, DownloadViewModel> _downloadsDictionary;
    }
}
