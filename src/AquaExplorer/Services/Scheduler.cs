using System;
using System.Threading;
using System.Windows.Threading;

namespace AquaExplorer.Services
{
    class Scheduler : IScheduler
    {
        private readonly Dispatcher _dispatcher;

        public Scheduler()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void ExecuteOnUiThread(Action action)
        {
            if (IsUiThread())
            {
                action();
            }
            else
            {
                _dispatcher.BeginInvoke(action);    
            }
        }

        public void ExecuteOnWorkerThread(Action action)
        {
            ThreadPool.QueueUserWorkItem(_=>action());
        }

        public bool IsUiThread()
        {
            return _dispatcher.CheckAccess();
        }
    }
}
