using Single.Core;
using Single.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSLib.Core;

namespace Single.ViewModel
{
    public class AdminVM : BindableBase
    {
        public AdminVM()
        {
            Tables = MainModel.GetDataBase().GetTablesName();
            Errors = "";

        }
        public string UserName { get => "Роль - " + MainModel.GetDataBase().GetActualUser().RoleName + ", Имя - " + MainModel.GetDataBase().GetActualUser().FullName; }
        private List<string> _tables;
        public List<string> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }
        private string _selectedTable;
        public string SelectedTable
        {
            get => _selectedTable;
            set
            {
                SetProperty(ref _selectedTable, value);
                _search = "";
                if (SelectedTable != null)
                    Table = MainModel.GetDataBase().SelectTable(SelectedTable, Search);
                else
                    Table = null;
            }
        }
        private string _search;
        public string Search
        {
            get => _search ?? "";
            set
            {
                _search = value;
                Table = MainModel.GetDataBase().SelectTable(SelectedTable, Search);
                
                Errors = MainModel.GetDataBase().GetException().Message;
                if (_search == "")
                    Errors = "Все записи";
            }
        }
        private DataView _table;
        public DataView Table
        {
            get => _table;
            set
            {
                SetProperty(ref _table, value);
                Errors = "";
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(SelectedTable == null) { return; }
                    MainModel.GetDataBase().SaveChanges(Table.Table);
                    MyException ex = MainModel.GetDataBase().GetException();
                    
                    if (ex.Code == 304)
                    {
                        Table = MainModel.GetDataBase().SelectTable(SelectedTable, Search);
                    }
                    Errors = ex.Message;



                });
            }
        }
        public DelegateCommand ReloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedTable == null) { return; }
                    Table = MainModel.GetDataBase().SelectTable(SelectedTable, Search);
                });
            }
        }
        private string _errors;
        public string Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
    }
}
