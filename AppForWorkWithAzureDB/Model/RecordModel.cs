using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RecordModel : INotifyPropertyChanged
    {
        private string id;
        private string serialNumber;
        private DateTime time;
        private double dose;
        private string _operator;

        public string ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        public double Dose
        {
            get
            {
                return dose;
            }

            set
            {
                dose = value;
                OnPropertyChanged();
            }
        }

        public string Operator
        {
            get
            {
                return _operator;
            }

            set
            {
                _operator = value;
                OnPropertyChanged();
            }
        }

        private void Fill()
        {

        }

        private void Save()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
