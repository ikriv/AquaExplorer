using System;

namespace AquaExplorer.Services
{
    interface INavigationTarget
    {
        void BeginLoad();
        void BeginStopLoading();
        event Action<INavigationTarget> LoadCompleted;
    }
}
