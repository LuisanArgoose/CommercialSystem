using Single.Core;
using Single.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSLib.Tables;

namespace Single.ViewModel
{
    public class TreatiesCellVM : BindableBase
    {
        public TreatiesCellVM(DataRowView r)
        {
            TreatieVM = new TreatiesVM()
            {
                Id = int.Parse(r.Row.ItemArray[0].ToString()),
                Number = int.Parse(r.Row.ItemArray[1].ToString()),
                BuyDate = DateTime.Parse(r.Row.ItemArray[2].ToString()),
                ClientFullName = r.Row.ItemArray[3].ToString(),
                ModelName = r.Row.ItemArray[4].ToString(),
                UserFullName = r.Row.ItemArray[5].ToString()
            };
        }
        public TreatiesCellVM()
        {
            TreatieVM = new TreatiesVM()
            {
                BuyDate = DateTime.Now,
                UserFullName = MainModel.GetDataBase().GetActualUser().FullName
            };
            
        }
        private TreatiesVM _treatieVM;
        public TreatiesVM TreatieVM
        {
            get => _treatieVM;
            set => SetProperty(ref _treatieVM, value);
        }
        public DelegateCommand SaveCommand { get => new DelegateCommand(() =>
            {
                MainModel.GetDataBase().InsertOrUpdateTreatie(TreatieVM);
                Errors = MainModel.GetDataBase().GetException().Message;
                if (MainModel.GetDataBase().GetException().Code == 305)
                {
                    MainModel.GetViews().CloseTreatiesCell();

                }
            });
        }
        private string _errors;
        public string Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        
    }
}
