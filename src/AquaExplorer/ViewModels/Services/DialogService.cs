using System.Windows;
using AquaExplorer.Views;
using IKriv.Windows.Mvvm;
using Microsoft.Practices.Unity;

namespace AquaExplorer.ViewModels.Services
{
    class DialogService : IDialogService
    {
        private readonly IUnityContainer _container;

        private Window OwnerWindow { get; set; }
        
        public DialogService(IUnityContainer container, [OptionalDependency] Window window)
        {
            OwnerWindow = window;
            _container = container;
        }

        public MessageBoxResult Show(string text, string caption, MessageBoxButton buttons, MessageBoxImage icon)
        {
            if (OwnerWindow != null)
            {
                return MessageBox.Show(OwnerWindow, text, caption, buttons, icon);
            }
            else
            {
                return MessageBox.Show(text, caption, buttons, icon);
            }
        }

        public IDialog<T> CreateDialog<T>() where T : ViewModelBase
        {
            // use child container in case displayed viewmodel wants to use its own DialogService
            var childContainer = _container.CreateChildContainer();
            var window = new DialogWindow {Owner = OwnerWindow};
            childContainer.RegisterInstance<Window>(window);

            var dialog = childContainer.Resolve<Dialog<T>>();
            return dialog;
        }


        // ReSharper disable once ClassNeverInstantiated.Local
        // Class is instantiated via Unity container Resolve()
        private class Dialog<T> : IDialog<T> where T : ViewModelBase
        {
            private readonly Window _window;

            public Dialog(T viewModel, Window window)
            {
                ViewModel = viewModel;
                _window = window;
                _window.DataContext = viewModel;
            }

            public T ViewModel { get; private set; }

            public bool? DialogResult { get; private set; }

            public bool Show()
            {
                DialogResult = _window.ShowDialog();
                return DialogResult == true;
            }
        }
    }
}
