using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppForWorkWithAzureDB.Base
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        private BaseViewModel _parent;
        private string _title;

        public BaseViewModel Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                _parent = value;
                OnPropertyChanged("Parent");
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        protected BaseViewModel(BaseViewModel parent = null, string title = null)
        {
            Parent = parent;
            Title = title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public virtual void Dispose()
        {
        }
    }
}
