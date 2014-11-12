namespace AquaExplorer.Services
{
    interface IDataProtectionService
    {
        byte[] Protect(byte[] data);
        byte[] Unprotect(byte[] data);
    }
}
