using System;

namespace AquaExplorer.Services
{
    interface IScheduler
    {
        void ExecuteOnUiThread(Action action);
        void ExecuteOnWorkerThread(Action action);
        bool IsUiThread();
    }
}
