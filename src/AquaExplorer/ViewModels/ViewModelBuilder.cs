using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using Microsoft.Practices.Unity;

namespace AquaExplorer.ViewModels
{
    class ViewModelBuilder : INavigationTargetBuilder
    {
        private readonly IUnityContainer _container;

        public ViewModelBuilder(IUnityContainer container)
        {
            _container = container;
        }

        public INavigationTarget CreateNavigationTarget(AzureLocation location)
        {
            if (location.Container != null)
            {
                return _container.Resolve<BlobListViewModel>().Init(location);
            }

            if (location.Account != null)
            { 
                return _container.Resolve<ContainerListViewModel>().Init(location);
            }

            return _container.Resolve<AccountListViewModel>();
        }
    }
}
