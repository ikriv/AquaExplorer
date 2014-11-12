using System.Windows;

namespace AquaExplorer.ViewModels.Services
{
    internal class WindowWrapper : IWindow
    {
        private readonly Window _window;

        public WindowWrapper(Window window)
        {
            _window = window;
        }

        public void Close()
        {
            _window.Close();
        }
    }
}
