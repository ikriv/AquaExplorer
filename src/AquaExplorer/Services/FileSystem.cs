using System;
using System.IO;

namespace AquaExplorer.Services
{
    class FileSystem : IFileSystem
    {
        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        public string GetMyDocumentsFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string GetProgramDataFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }
    }
}
