using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AquaExplorer.BusinessObjects;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    internal class DownloadController : IDownloadController
    {
        private readonly ITimeService _time;
        private readonly IScheduler _scheduler;
        private readonly Func<IWebClient> _webClientFactory;
        private readonly IAzureService _azure;
        private readonly IFileSystem _fileSystem;

        public DownloadController(ITimeService time, 
                                  IScheduler scheduler, 
                                  Func<IWebClient> webClientFactory, 
                                  IAzureService azure,
                                  IFileSystem fileSystem,
                                  IConfigurationService configService
                                 )
        {
            _time = time;
            _scheduler = scheduler;
            _webClientFactory = webClientFactory;
            _azure = azure;
            _fileSystem = fileSystem;

            Downloads = new ObservableCollection<DownloadOperation>();
            configService.ConfigurationChanged += ApplyConfiguration;
            ApplyConfiguration(configService.ReadConfiguration());
        }

        public ObservableCollection<DownloadOperation> Downloads { get; private set; }

        public event Action<DownloadOperation> DownloadProgress;

        public string DownloadFolder { get; set; }

        private class ActiveDownload
        {
            public DownloadOperation Operation { get; set; }
            public IWebClient WebClient { get; set; }
            public event Action<ActiveDownload> Progress;

            public void HookupEvents(IScheduler scheduler)
            {
                WebClient.DownloadCompleted += (_, args) => 
                    scheduler.ExecuteOnUiThread(() =>
                    {
                        Operation.IsInProgress = false;
                        Operation.IsCanceled = args.Cancelled;
                        Operation.Error = args.Error;
                        if (Progress != null) Progress(this);
                    });

                WebClient.DownloadProgress += (_, args) =>
                    scheduler.ExecuteOnUiThread(()=>
                    {
                        Operation.DownloadedSize = args.BytesReceived;
                        if (Progress != null) Progress(this);
                    });
            }
        }

        private readonly List<ActiveDownload> _activeDownloads = new List<ActiveDownload>(); 

        public DownloadOperation BeginDownload(Blob blob)
        {
            // if download folder does not exist, create it
            if (!_fileSystem.DirectoryExists(DownloadFolder))
            {
                _fileSystem.CreateDirectory(DownloadFolder);
            }

            // if download folder still does not exist, bail
            if (!_fileSystem.DirectoryExists(DownloadFolder))
            {
                throw new ApplicationException("Download folder does not exist: " + DownloadFolder);
            }

            var currentTime = _time.GetUtcNow();
            var uri = new Uri(blob.Url);
            var path = Path.Combine(DownloadFolder, GetFileName(uri));
            var operation = new DownloadOperation(uri, path, blob.ContentLength, currentTime);
            var webClient = _webClientFactory();
            var download = new ActiveDownload {Operation = operation, WebClient = webClient};
            Downloads.Add(operation);
            _activeDownloads.Add(download);
            download.HookupEvents(_scheduler);
            download.Progress += OnDownloadProgress;
            webClient.Authenticate(_azure.GetAuthenticator(blob.Container));
            webClient.BeginDownload(uri, path);
            return operation;
        }

        public void CancelDownload(DownloadOperation operation)
        {
            if (!_scheduler.IsUiThread()) throw new InvalidOperationException("CancelDownload() must be called on UI thread");

            var download = _activeDownloads.FirstOrDefault(d => d.Operation == operation);
            if (download == null) return;
            if (!download.Operation.IsInProgress) return;
            download.WebClient.CancelDownload();
        }

        public void RemoveDownload(DownloadOperation operation)
        {
            if (!_scheduler.IsUiThread()) throw new InvalidOperationException("RemoveDownload() must be called on UI thread");

            CancelDownload(operation);
            Downloads.Remove(operation);
        }

        private void OnDownloadProgress(ActiveDownload download)
        {
            if (!_scheduler.IsUiThread())
            {
                throw new ApplicationException("OnDownloadProgress() unexpectedly called on a worker thread");
            }

            if (!download.Operation.IsInProgress)
            {
                _activeDownloads.Remove(download);
            }

            if (DownloadProgress != null) DownloadProgress(download.Operation);
        }

        private static string GetFileName(Uri uri)
        {
            string result = "";

            int nSegments = uri.Segments.Length;
            if (nSegments > 0) result = uri.Segments[nSegments - 1];

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(c, '_');
            }

            if (result == "") result = "root";

            return result;
        }

        private void ApplyConfiguration(Configuration config)
        {
            DownloadFolder = config.DownloadFolder ?? GetDefaultDownloadFolder();
        }

        private string GetDefaultDownloadFolder()
        {
            var myDocuments = _fileSystem.GetMyDocumentsFolder();
            return Path.Combine(myDocuments, "Azure");
        }
    }
}
