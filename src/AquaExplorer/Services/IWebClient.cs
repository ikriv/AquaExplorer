using System;
using System.ComponentModel;
using System.Net;

namespace AquaExplorer.Services
{
    interface IWebClient
    {
        void Authenticate(IAuthenticator authenticator);
        void BeginDownload(Uri uri, string localPath);
        void CancelDownload();
        event AsyncCompletedEventHandler DownloadCompleted;
        event DownloadProgressChangedEventHandler DownloadProgress;
    }
}
