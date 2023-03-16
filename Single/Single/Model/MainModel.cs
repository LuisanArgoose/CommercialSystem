using Single.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSLib.Model;

namespace Single.Model
{
    static public class MainModel
    {
        static MainModel()
        {
            _dataBase = new DataBase();
            _views = new Views();
        }
        private static readonly DataBase _dataBase;
        public static DataBase GetDataBase()
        {
            return _dataBase;
        }
        private static readonly Views _views;
        public static Views GetViews()
        {
            return _views;
        }
    }
}
