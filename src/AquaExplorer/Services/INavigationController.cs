using System;
using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Services
{
    interface INavigationController
    {
        AzureLocation Location { get; }
        event Action LocationChanged;

        INavigationTarget Page { get; }

        void NavigateTo(AzureLocation location);
        void StopLoading();
        void Reload();

        bool IsLoading { get; }
        event Action IsLoadingChanged;
        
    }
}
