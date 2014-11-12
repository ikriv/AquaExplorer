using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Services
{
    interface INavigationTargetBuilder
    {
        INavigationTarget CreateNavigationTarget(AzureLocation location);
    }
}
