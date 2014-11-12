using System;

namespace IKriv.Azure
{
    /// <summary>
    /// Azure storage credentials
    /// </summary>
    public class Credentials
    {
        public Credentials(string account, byte[] key)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account");
            if (key == null || key.Length == 0) throw new ArgumentNullException("key");

            Account = account;
            Key = key;
        }

        public string Account { get; private set;}
        public byte[] Key { get; private set; }
    }
}
