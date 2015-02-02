using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;
using Microsoft.Practices.Unity;

namespace AquaExplorer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly INavigationController _controller;

        public MainViewModel(INavigationController controller)
        {
            _controller = controller;
            _controller.LocationChanged += ()=> RaisePropertyChanged("CurrentPage");
            _controller.NavigateTo(AzureLocation.Root);
        }

        [Dependency]
        public LocationBarViewModel LocationBar { get; set; }

        [Dependency]
        public DownloadBarViewModel DownloadBar { get; set; }

        [Dependency]
        public MainMenuViewModel MainMenu { get; set; }

        [Dependency]
        public ISearchBox SearchBox { get; set; }

        public ViewModelBase CurrentPage
        {
            get { return _controller.Page as ViewModelBase; }
        }
    }
}
