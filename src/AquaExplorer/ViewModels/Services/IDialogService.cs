using System.Windows;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels.Services
{
    interface IDialogService
    {
        MessageBoxResult Show(string text, string caption, MessageBoxButton buttons, MessageBoxImage icon);
        IDialog<T> CreateDialog<T>() where T : ViewModelBase;
    }
}
