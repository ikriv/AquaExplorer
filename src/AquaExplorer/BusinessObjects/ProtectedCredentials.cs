using System;
using System.Xml.Serialization;

namespace AquaExplorer.BusinessObjects
{
    [XmlRoot("Account")]
    public class ProtectedCredentials
    {
        public ProtectedCredentials()
        {
        }

        public ProtectedCredentials(string account, byte[] protectedKey)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account");
            if (protectedKey == null || protectedKey.Length == 0) throw new ArgumentNullException("protectedKey");

            Account = account;
            ProtectedKey = protectedKey;
        }

        [XmlAttribute("Name")]
        public string Account { get; set;}

        [XmlAttribute("Key")]
        public byte[] ProtectedKey { get; set; }
    }
}
