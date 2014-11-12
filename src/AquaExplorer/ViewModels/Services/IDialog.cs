using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels.Services
{
    interface IDialog<T> where T : ViewModelBase
    {
        T ViewModel { get; }
        bool? DialogResult { get; }
        bool Show();
    }
}
