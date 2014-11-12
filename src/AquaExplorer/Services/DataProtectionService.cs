using System.Security.Cryptography;

namespace AquaExplorer.Services
{
    class DataProtectionService : IDataProtectionService
    {
        // this is NOT an encryption key!
        private static readonly byte[] Salt = {95, 48, 223, 11, 39, 7, 126, 82, 12 };

        public byte[] Protect(byte[] data)
        {
            return ProtectedData.Protect(data, Salt, DataProtectionScope.CurrentUser);
        }

        public byte[] Unprotect(byte[] data)
        {
            return ProtectedData.Unprotect(data, Salt, DataProtectionScope.CurrentUser);
        }

    }
}
