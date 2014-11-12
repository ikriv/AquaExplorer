using System.IO;

namespace AquaExplorer.Services
{
    interface IFileSystem
    {
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        bool FileExists(string path);
        Stream OpenRead(string path);
        Stream OpenWrite(string path);
        string GetMyDocumentsFolder();
        string GetProgramDataFolder();
    }
}
