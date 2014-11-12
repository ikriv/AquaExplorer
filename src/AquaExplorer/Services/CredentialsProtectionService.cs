using AquaExplorer.BusinessObjects;
using IKriv.Azure;

namespace AquaExplorer.Services
{
    class CredentialsProtectionService
    {
        private readonly IDataProtectionService _dataProtectionService;

        public CredentialsProtectionService(IDataProtectionService dataProtectionService)
        {
            _dataProtectionService = dataProtectionService;
        }

        public ProtectedCredentials Protect(Credentials c)
        {
            return new ProtectedCredentials(c.Account, _dataProtectionService.Protect(c.Key));
        }

        public Credentials Unprotect(ProtectedCredentials c)
        {
            return new Credentials(c.Account, _dataProtectionService.Unprotect(c.ProtectedKey));
        }
    }
}
