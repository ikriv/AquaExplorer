using System;

namespace AquaExplorer.BusinessObjects
{
    class DownloadOperation
    {
        public DownloadOperation(Uri uri, string localPath, long size, DateTime startTime)
        {
            Uri = uri;
            LocalPath = localPath;
            TotalSize = size;
            StartTime = startTime;
            IsInProgress = true;
        }

        public Uri Uri { get; private set; }
        public string LocalPath { get; private set; }
        public long TotalSize { get; private set; }
        public DateTime StartTime { get; private set; }

        public long DownloadedSize { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsCanceled { get; set; }
        public Exception Error { get; set; }
    }
}
