using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AquaExplorer.BusinessObjects;
using AquaExplorer.Services;
using AquaExplorer.Util;
using AquaExplorer.ViewModels.Services;
using Microsoft.Practices.Unity;

namespace AquaExplorer.ViewModels
{
    class ContainerListViewModel : ListViewModelBase<AzureLocation>
    {
        private readonly IAzureService _azure;
        private readonly INavigationController _controller;
        private AzureLocation _account;

        [Dependency]
        public ISearchBox SearchBox { get; set; }


        public ContainerListViewModel(IAzureService azure, INavigationController controller)
        {
            _azure = azure;
            _controller = controller;
        }

        public ContainerListViewModel Init(AzureLocation account)
        {
            _account = account;
            SearchBox.Clear();
            return this;
        }

        public AzureLocation SelectedItem
        {
            get {  return _selectedItem; }
            set {  _selectedItem = value; RaisePropertyChanged("SelectedItem"); }
        }
        private AzureLocation _selectedItem;

        public void OnDoubleClick()
        {
            NavigateToSelectedItem();
        }

        protected override IEnumerable<AzureLocation> Load(CancellationToken token)
        {
            return 
                _azure.GetContainers(_account.Account)
                .ToList(token)
                .Select(container=>new AzureLocation(_account.Account, container))
                .OrderBy(location=>location.ContainerName)
                .ToList();
        }

        private void NavigateToSelectedItem()
        {
            if (SelectedItem == null) return;
            _controller.NavigateTo(SelectedItem);
        }
        
    }
}
