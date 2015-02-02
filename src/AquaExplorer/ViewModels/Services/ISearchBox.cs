using System;
using System.ComponentModel;

namespace AquaExplorer.ViewModels.Services
{
    interface ISearchBox : INotifyPropertyChanged
    {
        string SearchString { get; set; }
        void Clear();
    }
}
