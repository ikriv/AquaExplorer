using System;
using System.Windows.Input;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class SearchBoxViewModel : ViewModelBase, ISearchBox
    {
        public SearchBoxViewModel()
        {
            ClearCommand = new DelegateCommand(Clear);    
        }

        private string _searchString = String.Empty;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString"); 
            }
        }

        public void Clear()
        {
            SearchString = String.Empty;
        }

        public ICommand ClearCommand { get; private set; }
    }
}
