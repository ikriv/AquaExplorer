using System;
using System.ComponentModel;
using System.Net;

namespace AquaExplorer.Services
{
    class WebClientImpl : IWebClient
    {
        private readonly AzureWebClient _webClient = new AzureWebClient();
        private bool _isInUse;

        public WebClientImpl()
        {
            _webClient.DownloadFileCompleted += (_, args) =>
                {
                    if (DownloadCompleted != null) DownloadCompleted(this, args);
                };

            _webClient.DownloadProgressChanged += (_, args) =>
                {
                    if (DownloadProgress != null) DownloadProgress(this, args);
                };
        }

        public void Authenticate(IAuthenticator authenticator)
        {
            _webClient.Authenticator = authenticator;
        }

        public void BeginDownload(Uri uri, string localPath)
        {
            if (_isInUse) throw new InvalidOperationException("This web client is already in use");
            _isInUse = true;
            _webClient.DownloadFileAsync(uri, localPath);
        }

        public void CancelDownload()
        {
            _webClient.CancelAsync();
        }

        public event AsyncCompletedEventHandler DownloadCompleted;
        public event DownloadProgressChangedEventHandler DownloadProgress;

        private class AzureWebClient : WebClient
        {
            public IAuthenticator Authenticator { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                var httpRequest = request as HttpWebRequest;
                if (httpRequest != null && Authenticator != null)
                {
                    Authenticator.Authenticate(httpRequest);
                }
                return request;
            }
        }
    }
}
