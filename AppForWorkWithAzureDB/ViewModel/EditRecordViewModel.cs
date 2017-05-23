using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppForWorkWithAzureDB.Base;
using Model;
using System.Data.Entity;

namespace AppForWorkWithAzureDB.ViewModel
{
    public class EditRecordViewModel : BaseViewModel
    {
        //private Record currentRecord;
        //public Record CurrentRecord
        //{
        //    get
        //    {
        //        return currentRecord;
        //    }

        //    set
        //    {
        //        currentRecord = value;
        //        OnPropertyChanged("CurrentRecord");
        //    }
        //}
        private TestTable currentRecord;
        public TestTable CurrentRecord
        {
            get
            {
                return currentRecord;
            }

            set
            {
                currentRecord = value;
                OnPropertyChanged("CurrentRecord");
            }
        }

        #region Comands
        private DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                    (cancelCommand = new DelegateCommand(() =>
                    {
                        var DBVM = Parent as DatabaseViewModel;
                        if (DBVM == null) return;
                        DBVM.EditRecordVM = null;
                    }));
            }
        }

        private RelayCommand applyCommand;
        public RelayCommand ApplyCommand
        {
            get
            {
                return applyCommand ??
                    (applyCommand = new RelayCommand(obj =>
                    {
                        //var record = obj as Record;
                        var record = obj as TestTable;
                        if (record == null) return;
                        var DBVM = Parent as DatabaseViewModel;
                        if (DBVM == null) return;
                        //try
                        //{
                        //    if (record.Id == null)
                        //    {
                        //        Random random = new Random();
                        //        record.Id = Guid.NewGuid().ToString();
                        //        record.Time = DateTime.Now.ToString();
                        //        DBVM.DB.Records.Add(record);
                        //    }
                        //    else
                        //    {
                        //        Record recordForEdit = DBVM.DB.Records.Find(record.Id);
                        //        if (recordForEdit != null)
                        //        {
                        //            recordForEdit.Dose = record.Dose;
                        //            recordForEdit.Time = DateTime.Now.ToString();
                        //            recordForEdit.Operator = record.Operator;
                        //            recordForEdit.SerialNumber = record.SerialNumber;
                        //        }

                        //    }
                        //    DBVM.DB.SaveChanges();
                        //    DBVM.LoadRecords();
                        //    DBVM.EditRecordVM = null;
                        //}
                        try
                        {
                            if (string.IsNullOrWhiteSpace(record.ID))
                            {
                                record.ID = Guid.NewGuid().ToString();
                                //record.Time = DateTime.Now;
                                DBVM.DB.TestTables.Add(record);
                            }
                            else
                            {
                                TestTable recordForEdit = DBVM.DB.TestTables.Find(record.ID);
                                if (recordForEdit != null)
                                {
                                    recordForEdit.Dose = record.Dose;
                                    recordForEdit.Time = DateTime.Now;
                                    recordForEdit.Operator = record.Operator;
                                    recordForEdit.SerialNumber = record.SerialNumber;
                                }

                            }
                            DBVM.DB.SaveChanges();
                            DBVM.LoadRecords();
                            DBVM.EditRecordVM = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }, 
                    ApplyCanExecute
                    ));
            }
        }

        bool ApplyCanExecute(object param)
        {
            if (param == null) return false;
            //var record = param as Record;
            var record = param as TestTable;
            if (record == null) return false;
            if (record.Dose < 0 ||
                string.IsNullOrWhiteSpace(record.Operator) ||
                string.IsNullOrWhiteSpace(record.SerialNumber)) return false;
            return true;
        }
        #endregion

        public EditRecordViewModel(BaseViewModel parent, string title) : base(parent, title)
        {
            //CurrentRecord = new Record();
            CurrentRecord = new TestTable { Time = DateTime.Now};
        }
        //public EditRecordViewModel(BaseViewModel parent, string title, Record record) : base(parent, title)
        //{
        //    CurrentRecord = record;
        //}
        public EditRecordViewModel(BaseViewModel parent, string title, TestTable record) : base(parent, title)
        {
            CurrentRecord = record;
        }


        public override void Dispose()
        {
            CurrentRecord = null;
        }

    }
}
