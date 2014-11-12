using System;
using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Services
{
    class NavigationController : INavigationController
    {
        private readonly INavigationTargetBuilder _targetBuilder;
        private AzureLocation _location;
        private bool _isLoading;
        private object _sync = new object();

        public NavigationController(INavigationTargetBuilder targetBuilder)
        {
            _targetBuilder = targetBuilder;
        }

        public AzureLocation Location
        {
            get { return _location;  }
            private set { _location = value; if (LocationChanged != null) LocationChanged(); }
        }

        public event Action LocationChanged;

        public INavigationTarget Page { get; private set; }

        public void NavigateTo(AzureLocation location)
        {
            lock (_sync)
            {
                if (Page != null)
                {
                    Page.LoadCompleted -= OnPageLoadCompleted;
                    Page.BeginStopLoading();
                }
                var newPage = _targetBuilder.CreateNavigationTarget(location);
                newPage.LoadCompleted += OnPageLoadCompleted;
                
                Page = newPage;
                Location = location;
                IsLoading = true;

                Page.BeginLoad();
            }
        }

        private void OnPageLoadCompleted(INavigationTarget page)
        {
            lock (_sync)
            {
                if (page != Page) return;
                IsLoading = false;
            }
        }

        public void StopLoading()
        {
            lock (_sync)
            {
                if (Page == null) return;
                Page.BeginStopLoading();
            }
        }

        public void Reload()
        {
            NavigateTo(Location); // creates new page
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            private set 
            {  
                if (_isLoading == value) return;
                _isLoading = value; 
                if (IsLoadingChanged != null) IsLoadingChanged(); 
            }
        }

        public event Action IsLoadingChanged;
    }
}
