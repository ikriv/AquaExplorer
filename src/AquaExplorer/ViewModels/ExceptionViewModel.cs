using System;
using System.Windows.Input;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class ExceptionViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public object Header { get; set; }
        public Exception Exception { get; set; }
        public ICommand CloseCommand { get; private set; }

        public ExceptionViewModel(IWindow window)
        {
            CloseCommand = new DelegateCommand(window.Close);
        }
    }
}
