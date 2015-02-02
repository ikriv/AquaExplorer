using System;
using System.ComponentModel;

namespace AquaExplorer.Util
{
    static class NotifyPropertyChangedExtensions
    {
        public static void OnPropertyChanged(this INotifyPropertyChanged vm, string name, Action action)
        {
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == name) action();
            };
        }
    }
}
