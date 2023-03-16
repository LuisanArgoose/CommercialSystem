using Single.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSLib.Tables;

namespace Single.ViewModel
{
    public class TreatiesVM : Treaties
    {
        public List<string> ClientsFullNamesList { get => MainModel.GetDataBase().GetClientsFullNames(); }
        public List<string> ModelsList { get => MainModel.GetDataBase().GetModels(); }
    }
}
