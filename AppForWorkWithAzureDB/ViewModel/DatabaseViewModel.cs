using System;
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

namespace AppForWorkWithAzureDB.ViewModel
{
    public class DatabaseViewModel : BaseViewModel
    {
        //public DBFORAZUREEntities DB;
        public TestDBEntities DB;
        //private BindingList<Record> recordsList;
        //private Record selectedRecord;

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
                      //var RecordForDelete = obj as Record;
                      //if (RecordForDelete == null) return;
                      //DB.Records.Remove(RecordForDelete);
                      //DB.SaveChanges();
                      try
                      {
                          var RecordForDelete = obj as TestTable;
                          //var RecordForDelete = obj as RecordModel;
                          if (RecordForDelete == null) return;

                          //TODO 
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
        #endregion

        public DatabaseViewModel(BaseViewModel parent, string title = null):base (parent, title)
        {
            LoadRecords();
        }

        public override void Dispose()
        {
            if (DB != null) DB.Dispose();
            if (EditRecordVM != null) EditRecordVM.Dispose();
            EditRecordVM = null;
            RecordsList = null;
            SelectedRecord = null;
    }

        public void LoadRecords()
        {
            if (DB != null) DB.Dispose();

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


                ////DB = new TestDBEntities();

                ////TODO
                //RecordsList = new BindingList<RecordModel>();

                //using (var context = new TestDBEntities())
                //{
                //    context.TestTables.Load();
                //    var newRecords = context.TestTables.Local.ToBindingList();
                //    foreach (var record in newRecords)
                //    {
                //        var newitem = new RecordModel
                //        {
                //            ID = record.ID,
                //            Dose = record.Dose,
                //            Time = record.Time,
                //            Operator = record.Operator,
                //            SerialNumber = record.SerialNumber
                //        };
                //        RecordsList.Add(newitem);
                //    }
                //}
                //if (RecordsList.Count > 0) SelectedRecord = RecordsList.First();
                ////DB.TestTables.Load();
                ////RecordsList = DB.TestTables.Local.ToBindingList();
                ////if (RecordsList.Count > 0) SelectedRecord = RecordsList.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
