using System;
using System.Collections.ObjectModel;
using AquaExplorer.BusinessObjects;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    interface IDownloadController
    {
        ObservableCollection<DownloadOperation> Downloads { get; }
        event Action<DownloadOperation> DownloadProgress;
        string DownloadFolder { get; set; }

        DownloadOperation BeginDownload(Blob blob);
        void CancelDownload(DownloadOperation operation);
        void RemoveDownload(DownloadOperation operation);
    }
}
