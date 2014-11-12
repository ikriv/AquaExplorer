using System.Collections.Generic;
using System.Windows.Input;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class LocationBarViewModel : ViewModelBase
    {
        private readonly INavigationController _controller;

        public LocationBarViewModel(INavigationController controller)
        {
            _controller = controller;
            _controller.LocationChanged += OnLocationChanged; 
            NavigateCommand = new DelegateCommand<AzureLocation>(_controller.NavigateTo);
            OnLocationChanged();
        }

        public IEnumerable<AzureLocation> Locations
        {
            get { return _locations; }
            set { _locations = value; RaisePropertyChanged("Locations"); }
        }
        private IEnumerable<AzureLocation> _locations;

        public ICommand NavigateCommand { get; private set; }

        private void OnLocationChanged()
        {
            var locationList = new List<AzureLocation>();
            for (var location = _controller.Location; location != null; location = location.GetParent())
            {
                locationList.Add(location);
            }

            locationList.Reverse(); // root comes first
            Locations = locationList;
        }
    }
}
