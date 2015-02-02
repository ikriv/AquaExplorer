using System;
using AquaExplorer.ViewModels.Services;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class SearchBoxViewModel : ViewModelBase, ISearchBox
    {
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
    }
}
