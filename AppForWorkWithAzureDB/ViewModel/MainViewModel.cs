using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForWorkWithAzureDB.Base;
using AppForWorkWithAzureDB.ViewModel;

namespace AppForWorkWithAzureDB.ViewModel
{
    public class MainViewModel: BaseViewModel
    {

        private BaseViewModel currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel != null)
                {
                    currentViewModel.Dispose();
                    currentViewModel = null;
                }
                currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        //private DelegateCommand clearCommand;
        //public DelegateCommand ClearCommand
        //{
        //    get
        //    {
        //        return clearCommand ??
        //            (clearCommand = new DelegateCommand(() =>
        //            {
        //                if (CurrentViewModel != null)
        //                {
        //                    CurrentViewModel.Dispose();
        //                    CurrentViewModel = null;
        //                } 
        //            }));
        //    }
        //}

        public MainViewModel() : base(null)
        {
            CurrentViewModel = new DatabaseViewModel(this, "DBView");
        }

        public override void Dispose()
        {
            CurrentViewModel.Dispose();
            CurrentViewModel = null;
        }
    }
}
