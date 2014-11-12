using System;
using System.IO;
using AquaExplorer.BusinessObjects;
using IKriv.Windows.Mvvm;

namespace AquaExplorer.ViewModels
{
    class DownloadViewModel : ViewModelBase
    {
        public DownloadViewModel(DownloadOperation operation)
        {
            Operation = operation;
            Name = Path.GetFileName(Operation.LocalPath);
            Status = "Starting";
        }

        public DownloadOperation Operation { get; private set; }

        public string Name { get; private set; }
        public string Status { get; private set; }

        public void Update()
        {
            if (Operation.IsInProgress)
            {
                Status = String.Format("{0} of {1} bytes", Operation.DownloadedSize, Operation.TotalSize);
            }
            else
            {
                if (Operation.Error != null)
                {
                    Status = "Error";
                }
                else
                {
                    Status = (Operation.IsCanceled) ? "Canceled" : "Completed";    
                }
            }

            RaisePropertyChanged("");
        }
    }
}
