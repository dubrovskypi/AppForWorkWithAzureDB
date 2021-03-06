﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForWorkWithAzureDB.Base;
using Model;
using System.Data.Entity;
using System.ComponentModel;
using System.Windows;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.Timers;
using System.Windows.Threading;

namespace AppForWorkWithAzureDB.ViewModel
{
    public class DatabaseViewModel : BaseViewModel
    {
        //public DBFORAZUREEntities DB;
        public TestDBEntities DB;
        //private BindingList<Record> recordsList;
        //private Record selectedRecord;

        private Timer _timerToAdd;
        //private BackgroundWorker _worker = new BackgroundWorker();

        private BindingList<TestTable> recordsList;
        private TestTable selectedRecord;

        //private BindingList<RecordModel> recordsList;
        //private RecordModel selectedRecord;

        private EditRecordViewModel editRecordVM;

        public TestTable SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                if (EditRecordVM != null)
                {
                    EditRecordVM.Dispose();
                    EditRecordVM = null;
                }
                selectedRecord = value;
                OnPropertyChanged("SelectedRecord");
            }
        }

        public BindingList<TestTable> RecordsList
        {
            get { return recordsList; }
            set
            {
                SelectedRecord = null;
                recordsList = value;
                OnPropertyChanged("RecordsList");
            }
        }

        //public RecordModel SelectedRecord
        //{
        //    get { return selectedRecord; }
        //    set
        //    {
        //        if (editRecordVM != null) editRecordVM.Dispose();
        //        editRecordVM = null;
        //        selectedRecord = value;
        //        OnPropertyChanged("SelectedRecord");
        //    }
        //}

        //public BindingList<RecordModel> RecordsList
        //{
        //    get { return recordsList; }
        //    set
        //    {
        //        SelectedRecord = null;
        //        recordsList = value;
        //        OnPropertyChanged("RecordsList");
        //    }
        //}

        public EditRecordViewModel EditRecordVM
        {
            get
            {
                return editRecordVM;
            }
            set
            {
                if (editRecordVM != null)
                {
                    editRecordVM.Dispose();
                    editRecordVM = null;
                }
                editRecordVM = value;
                OnPropertyChanged("EditRecordVM");
            }
        }
        #region Comands
        private DelegateCommand updateCommand;
        public DelegateCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                    (updateCommand = new DelegateCommand(() =>
                    {
                        LoadRecords();
                        //_worker.RunWorkerAsync();
                        if (editRecordVM != null)
                        {
                            editRecordVM.Dispose();
                            editRecordVM = null;
                        }

                    }
                    ));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      //вариант1
                      //var RecordForDelete = obj as Record;
                      //if (RecordForDelete == null) return;
                      //DB.Records.Remove(RecordForDelete);
                      //DB.SaveChanges();
                      try
                      {
                          var RecordForDelete = obj as TestTable;
                          //var RecordForDelete = obj as RecordModel;
                          if (RecordForDelete == null) return;
                          DB.TestTables.Remove(RecordForDelete);
                          DB.SaveChanges();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                      }

                  },
                 (obj) => obj != null));
            }
        }

        private DelegateCommand addRecordCommand;
        public DelegateCommand AddRecordCommand
        {
            get
            {
                return addRecordCommand ??
                    (addRecordCommand = new DelegateCommand(() =>
                    {
                        EditRecordVM = new EditRecordViewModel(this, "New record");
                    }));
            }
        }

        private RelayCommand editRecordCommand;
        public RelayCommand EditRecordCommand
        {
            get
            {
                return editRecordCommand ??
                    (editRecordCommand = new RelayCommand(obj =>
                    {
                        //вариант1
                        //var recordForEdit = obj as Record;
                        //if (recordForEdit == null) return;
                        //EditRecordVM = new EditRecordViewModel(this, "Edit record", recordForEdit);

                        var recordForEdit = obj as TestTable;
                        //var recordForEdit = obj as RecordModel;
                        if (recordForEdit == null) return;
                        EditRecordVM = new EditRecordViewModel(this, "Edit record", recordForEdit);
                    },
                    obj => obj != null));
            }
        }

        private DelegateCommand startTimerCommand;
        public DelegateCommand StartTimerCommand
        {
            get
            {
                return startTimerCommand ??
                    (startTimerCommand = new DelegateCommand(() =>
                    {
                        if (_timerToAdd != null)
                        {
                            if (_timerToAdd.Enabled) _timerToAdd.Stop();
                            else _timerToAdd.Start();
                        }

                    }));
            }
        }
        #endregion

        public DatabaseViewModel(BaseViewModel parent, string title = null):base (parent, title)
        {
            LoadRecords();
            
            _timerToAdd = new Timer(10000) { AutoReset = true };
            _timerToAdd.Elapsed += delegate
            {
                try
                {
                    var record = new TestTable
                    {
                        ID = Guid.NewGuid().ToString(),
                        Dose = 99,
                        Operator = "operator",
                        SerialNumber = "serial",
                        Time = DateTime.Now
                    };
                    using (var context = new TestDBEntities("name=TestDBEntities"))
                    {
                        context.TestTables.Add(record);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            };
            //_timerToAdd.Start();

            //_worker.DoWork += _WorkerDoWork;
            //_worker.RunWorkerCompleted += _WorkerRunWorkerCompleted;
            //_worker.RunWorkerAsync();
        }

        //private void _WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //private void _WorkerDoWork(object sender, DoWorkEventArgs e)
        //{
        //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action (() => 
        //    {
        //        try
        //        {
        //            var record = new TestTable
        //            {
        //                ID = Guid.NewGuid().ToString(),
        //                Dose = 99,
        //                Operator = "operator",
        //                SerialNumber = "serial",
        //                Time = DateTime.Now
        //            };
        //            DB.TestTables.Add(record);
        //            DB.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }
        //    ));
        //}

        public override void Dispose()
        {
            if (DB != null) DB.Dispose();

            _timerToAdd.Stop();
            _timerToAdd.Dispose();

            if (EditRecordVM != null) EditRecordVM.Dispose();
            EditRecordVM = null;
            RecordsList = null;
            SelectedRecord = null;
    }

        public void LoadRecords()
        {
            if (DB != null) DB.Dispose();

            //вариант1
            //DB = new DBFORAZUREEntities();
            //DB.Records.Load();
            //RecordsList = DB.Records.Local.ToBindingList();
            //if (RecordsList.Count > 0) SelectedRecord = RecordsList.First();
            try
            {
                //user id=AxisG;password=POLIapplehouse93;
                var connectionstring = ConfigurationManager.ConnectionStrings["TestDBEntitiesWithoutPwd"].ConnectionString;
                var entityStringBuilder = new EntityConnectionStringBuilder(connectionstring);
                var factory = DbProviderFactories.GetFactory(entityStringBuilder.Provider);
                var providerBuilder = factory.CreateConnectionStringBuilder();

                providerBuilder.ConnectionString = entityStringBuilder.ProviderConnectionString;
                //providerBuilder.Add("user id", "AxisG");
                //providerBuilder.Add("password", "POLIapplehouse93");
                providerBuilder.Add("user id", "TestUser");
                providerBuilder.Add("password", "TestPassword_1");
                entityStringBuilder.ProviderConnectionString = providerBuilder.ToString();

                DB = new TestDBEntities(entityStringBuilder.ToString());
                //DB = new TestDBEntities();
                DB.TestTables.Load();
                RecordsList = DB.TestTables.Local.ToBindingList();
                if (RecordsList.Count > 0) SelectedRecord = RecordsList.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddRecords()
        {

        }
    }
}
