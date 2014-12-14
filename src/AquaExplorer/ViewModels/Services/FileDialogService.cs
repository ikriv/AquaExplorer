using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AquaExplorer.ViewModels.Services
{
    class FileDialogService : IFileDialogService
    {
        public string SelectFolder(SelectFolderOptions options)
        {
            if (options == null) options = new SelectFolderOptions();
            
            var dialog = new CommonOpenFileDialog
            {
                Title = options.Title ?? "Select folder",
                IsFolderPicker = true,
                InitialDirectory = options.InitialDirectory ?? Directory.GetCurrentDirectory(),
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = true,
                ShowPlacesList = true
            };

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return null;

            return dialog.FileName;
        }
    }
}
