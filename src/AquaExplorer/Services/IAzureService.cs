using System.Collections.Generic;
using AquaExplorer.BusinessObjects;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    interface IAzureService
    {
        IEnumerable<Container> GetContainers(Account account);
        IEnumerable<Blob> GetBlobs(Container container);
        IAuthenticator GetAuthenticator(Container container);
    }
}
