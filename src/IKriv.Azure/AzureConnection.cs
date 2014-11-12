using System;
using System.Net;

namespace IKriv.Azure
{
    /// <summary>
    /// An Azure client
    /// </summary>
    public class AzureConnection
    {
        public AzureConnection()
        {
            Protocol = "https";
            BaseUrl = "blob.core.windows.net";
        }

        public string Protocol {  get; set; }
        public string BaseUrl {  get; set; }

        public Storage Storage(string name, byte[] key)
        {
            return new Storage(this, new Credentials(name, key));
        }

        internal HttpWebRequest CreateRequest(string account, string query)
        {
            string url = String.Format("{0}://{1}.{2}/{3}", Protocol, account, BaseUrl, query);
            return (HttpWebRequest)WebRequest.Create(url);
        }
    }
}
