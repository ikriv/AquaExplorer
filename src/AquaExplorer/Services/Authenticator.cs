using System.Net;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    class Authenticator : IAuthenticator
    {
        private readonly Storage _storage;

        public Authenticator(Storage storage)
        {
            _storage = storage;
        }

        public void Authenticate(HttpWebRequest request)
        {
            _storage.Authenticate(request);
        }
    }
}
