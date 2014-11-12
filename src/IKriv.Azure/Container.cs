using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Linq;

namespace IKriv.Azure
{
    public class Container
    {
        internal Container(Storage storage)
        {
            if (storage == null) throw new ArgumentNullException("storage");
            Storage = storage;
        }

        public Storage Storage { get; private set; }
        public Credentials Credentials { get {  return Storage.Credentials; } }
        public AzureConnection Connection { get {  return Storage.Connection; } }

        public string Name { get; internal set; }
        public DateTime LastModified {  get; internal set; }

        public IEnumerable<Blob> GetBlobs()
        {
            return Batch.Fold(GetBlobs);
        }

        private IEnumerable<Blob> GetBlobs(Batch.NextMarker marker)
        {
            return AzureException.Catch( "reading list of blobs for container " + Name, ()=>
            {
                var query = HttpUtility.UrlEncode(Name) + "?restype=container&comp=list";
                if (!String.IsNullOrEmpty(marker.Value)) query += "&marker=" + HttpUtility.UrlEncode(marker.Value);

                var request = Connection.CreateRequest(Credentials.Account, query);
                Authenticator.Authenticate(request, Credentials);

                // ReSharper disable once AssignNullToNotNullAttribute
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var text = reader.ReadToEnd();
                    System.Diagnostics.Trace.WriteLine(text);

                    var doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(text);
                        if (doc.DocumentElement == null) return Enumerable.Empty<Blob>();
                        var rootName = doc.DocumentElement.Name;
                        if (rootName != "EnumerationResults") throw new AzureException(text, null);

                        var markerNode = doc.SelectSingleNode("/EnumerationResults/NextMarker");
                        if (markerNode != null) marker.Value = markerNode.InnerText;

                        var nodes = doc.SelectNodes("/EnumerationResults/Blobs/Blob");
                        if (nodes == null) return Enumerable.Empty<Blob>();
                        var result = nodes.Cast<XmlNode>().Select(BlobFromNode).Where(b=>b!=null);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(String.Format("Server returned invalid XML. {0}. {1}", ex.Message, text), ex);
                    }
                    
                }
            });
        }

        private Blob BlobFromNode(XmlNode node)
        {
            if (node == null) return null;

            var name = node.GetNodeValue("Name");
            if (name == null) return null; // name is mandatory; Blob without name is invalid

            return new Blob(this) 
            { 
                Name = name, 
                Url = node.GetNodeValue("Url"),
                LastModified = node.GetNodeDate("Properties/Last-Modified", new DateTime()),
                ContentLength = node.GetNodeLong("Properties/Content-Length", -1),
                ContentType = node.GetNodeValue("Properties/Content-Type")
            };
        }

    }
}
