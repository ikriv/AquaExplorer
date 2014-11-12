﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AquaExplorer.Services;
using AquaExplorer.Util;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    abstract class ListViewModelBase<T> : ViewModelBase, INavigationTarget
    {
        private Task _loadingTask;
        private CancellationTokenSource _tokenSource;
        private bool _loadCompleted;
        private readonly object _sync = new object();

        // to be implemented in the derived class
        protected abstract IEnumerable<T> Load(CancellationToken token);

        public IEnumerable<T> Items
        {
            get { return _items; }
            set {  _items = value; RaisePropertyChanged("Items"); }
        }
        private IEnumerable<T> _items;

        public Exception Error
        {
            get { return _error; }
            set { _error = value; RaisePropertyChanged("Error"); }
        }
        private Exception _error;

        public void BeginLoad()
        {
            if (_loadingTask != null) throw new ApplicationException("Already loading"); /*!*/

            _tokenSource = new CancellationTokenSource();

            _loadingTask = Task.Factory.StartNew(()=>Load(_tokenSource.Token))
                .OnSuccess(items => Items = items)
                .OnError(ex => Error = ex)
                .ContinueWith(t => OnLoadCompleted());
        }

        public void BeginStopLoading()
        {
            // we assume that BeginLoad() has been called

            bool isCompleted;

            lock (_sync)
            {
                isCompleted = _loadCompleted;
                if (!isCompleted) _tokenSource.Cancel();
            }
            
            if (isCompleted) OnLoadCompleted();
        }

        private void OnLoadCompleted()
        {
            lock (_sync) _loadCompleted = true;
            if (LoadCompleted != null) LoadCompleted(this);
        }

        public event Action<INavigationTarget> LoadCompleted;
    }
}