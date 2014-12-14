namespace AquaExplorer.ViewModels.Services
{
    class SelectFolderOptions
    {
        public string Title { get; set; }
        public string InitialDirectory { get; set; }
    }

    interface IFileDialogService
    {
        string SelectFolder(SelectFolderOptions options);
    }
}
