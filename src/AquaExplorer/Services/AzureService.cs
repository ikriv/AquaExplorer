using System.Collections.Generic;
using AquaExplorer.BusinessObjects;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    class AzureService : IAzureService
    {
        private readonly CredentialsProtectionService _credentialsProtection;
        public AzureService(CredentialsProtectionService credentialsProtection)
        {
            _credentialsProtection = credentialsProtection;
        }

        public IEnumerable<Container> GetContainers(Account account)
        {
            var storage = GetStorage(account);
            return storage.GetContainers();
        }

        public IEnumerable<Blob> GetBlobs(Container container)
        {
            return container.GetBlobs();
        }

        public IAuthenticator GetAuthenticator(Container container)
        {
            return new Authenticator(container.Storage);
        }

        private Storage GetStorage(Account account)
        {
            var rawCredentials = _credentialsProtection.Unprotect(account.Credentials);
            var connection = new AzureConnection();
            var storage = connection.Storage(rawCredentials.Account, rawCredentials.Key);
            return storage;
        }
    }
}
