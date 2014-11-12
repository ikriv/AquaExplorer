using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace IKriv.Azure
{
    /// <summary>
    /// Azure Blob storage account
    /// </summary>
    public class Storage
    {
        internal Storage(AzureConnection connection, Credentials credentials)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (credentials == null) throw new ArgumentNullException("credentials");
            Connection = connection;
            Credentials = credentials;
        }

        public AzureConnection Connection {  get; private set; }
        public Credentials Credentials { get; private set; }

        public IEnumerable<Container> GetContainers()
        {
            return Batch.Fold(GetContainers);
        }

        public void Authenticate(HttpWebRequest request)
        {
            Authenticator.Authenticate(request, Credentials);
        }

        private IEnumerable<Container> GetContainers(Batch.NextMarker marker)
        {
            return AzureException.Catch( "reading list of containers for account " + Credentials.Account, ()=>
            {
                var query = "?comp=list";
                if (!String.IsNullOrEmpty(marker.Value)) query += "&marker=" + HttpUtility.UrlEncode(marker.Value);

                var request = Connection.CreateRequest(Credentials.Account, query);
                Authenticator.Authenticate(request, Credentials);

                // ReSharper disable once AssignNullToNotNullAttribute
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var text = reader.ReadToEnd();
                    var doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(text);
                        if (doc.DocumentElement == null) return Enumerable.Empty<Container>();

                        var rootName = doc.DocumentElement.Name;
                        if (rootName != "EnumerationResults") throw new AzureException(text, null);

                        var markerValue = doc.GetNodeValue("/EnumerationResults/NextMarker");
                        if (markerValue != null) marker.Value = markerValue;

                        var nodes = doc.SelectNodes("/EnumerationResults/Containers/Container");
                        if (nodes == null) return Enumerable.Empty<Container>();

                        var result = nodes.Cast<XmlNode>().Select(ContainerFromNode).Where(c=>c!=null);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(String.Format("Server returned invalid XML. {0}. {1}",  ex.Message, text), ex);
                    }
                }
            });
        }

        private Container ContainerFromNode(XmlNode node)
        {
            if (node == null) return null;

            var name = node.GetNodeValue("Name");
            if (name == null) return null; // name is mandatory; Container without name is invalid

            var date = node.GetNodeDate("Properties/Last-Modified", new DateTime());

            return new Container(this) { Name = name, LastModified = date };
        }
    }
}
