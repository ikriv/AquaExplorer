using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.Util;

namespace AquaExplorer.ViewModels
{
    class ContainerListViewModel : ListViewModelBase<AzureLocation>
    {
        private readonly IAzureService _azure;
        private readonly INavigationController _controller;
        private AzureLocation _account;

        public ContainerListViewModel(IAzureService azure, INavigationController controller)
        {
            _azure = azure;
            _controller = controller;
        }

        public ContainerListViewModel Init(AzureLocation account)
        {
            _account = account;
            return this;
        }

        public AzureLocation SelectedItem
        {
            get {  return _selectedItem; }
            set {  _selectedItem = value; RaisePropertyChanged("SelectedItem"); NavigateToSelectedItem(); }
        }
        private AzureLocation _selectedItem;

        protected override IEnumerable<AzureLocation> Load(CancellationToken token)
        {
            return 
                _azure.GetContainers(_account.Account)
                .ToList(token)
                .Select(container=>new AzureLocation(_account.Account, container))
                .ToList();
        }

        private void NavigateToSelectedItem()
        {
            if (SelectedItem == null) return;
            _controller.NavigateTo(SelectedItem);
        }
        
    }
}
