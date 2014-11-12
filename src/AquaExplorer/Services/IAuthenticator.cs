using System.Net;

namespace AquaExplorer.Services
{
    interface IAuthenticator
    {
        void Authenticate(HttpWebRequest request);
    }
}
