using Single.Core;
using Single.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Single.ViewModel
{
    public class ManagerVM : BindableBase
    {
        public ManagerVM()
        {

            TreatiesTable = MainModel.GetDataBase().SelectTreaties();
        }
        public string UserName { get => "Роль - " + MainModel.GetDataBase().GetActualUser().RoleName + ", Имя - " + MainModel.GetDataBase().GetActualUser().FullName; }

        private DataView _treatiesTable;
        public DataView TreatiesTable
        {
            get => _treatiesTable;
            set => SetProperty(ref _treatiesTable, value);
        }
        public List<string> Models 
        {
            get
            {
                List<string> list = new List<string>();
                list.Add("Все элементы");
                list.AddRange(MainModel.GetDataBase().GetModels());
                return list;
            }
        }
        private string _selectedModel;
        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                SetProperty(ref _selectedModel, value);
                Update();


            }

        }
        private void Update()
        {
            if (_selectedModel == "Все элементы")
            {
                TreatiesTable = MainModel.GetDataBase().SelectTreaties(Search);
            }
            else
            {
                TreatiesTable = MainModel.GetDataBase().SelectTreaties(Search, SelectedModel);
            }
            Errors = MainModel.GetDataBase().GetException().Message;
        }
        private string _search;
        public string Search 
        { 
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                Update();
                if (_search == "")
                    Errors = "Все записи";
            }
        }
        private DataRowView _selectedRow;
        public DataRowView SelectedRow
        {
            get => _selectedRow;
            set => SetProperty(ref _selectedRow, value);
        }
        private string _errors;
        public string Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        public DelegateCommand AddCommand
        {
            get => new DelegateCommand(() =>
            {
                bool res = MainModel.GetViews().OpenTreatiesCell();
                Update();
            });
        }
        public DelegateCommand UpdateCommand 
        { 
            get => new DelegateCommand(() =>
            {
                if(SelectedRow != null)
                {
                    bool res = MainModel.GetViews().OpenTreatiesCell(SelectedRow);
                    Update();
                }
                    
            });
        }
        public DelegateCommand DeleteCommand
        {
            get => new DelegateCommand(() =>
            {
                bool res = MainModel.GetViews().OpenAcceptDialog("Вы уверены?");
                if (res)
                {
                    MainModel.GetDataBase().DeleteTreatie(int.Parse(SelectedRow.Row.ItemArray[0].ToString()));
                    Update();
                }
            });
        }
    }
}