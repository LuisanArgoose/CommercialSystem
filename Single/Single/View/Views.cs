using Single.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Single.View
{
    public class Views
    {
        public Views() { }
        private SingIn _singIn;
        public SingIn SingIn
        {
            get => _singIn;
            set
            {
                if(_singIn == null) _singIn = value;
            }
        }
        public void SetSingIn(SingIn singIn)
        {
            if (_singIn == null) _singIn = singIn;
        }
        public void CloseSingIn()
        {
            try
            {
                _singIn.Close();
            }
            catch { }
        }
        private Admin _admin;
        public void OpenAdmin()
        {
            _admin = new Admin();
            _admin.Show();
        }
        public void CloseAdmin()
        {
            try
            {
                _admin.Close();
            }
            catch { }
        }
        private Manager _manager;
        public void OpenManager()
        {
            _manager = new Manager();
            _manager.Show();
        }
        public void CloseManager()
        {
            try
            {
                _manager.Close();
            }
            catch { }
        }
        private TreatiesCell _treatiesCell;
        public bool OpenTreatiesCell()
        {
            _treatiesCell = new TreatiesCell();
            var res = _treatiesCell.ShowDialog() ?? false;
            return res;
        }
        public bool OpenTreatiesCell(DataRowView r)
        {
            _treatiesCell = new TreatiesCell();
            _treatiesCell.DataContext = new TreatiesCellVM(r);
            var res = _treatiesCell.ShowDialog() ?? false;
            return res;
        }
        public void CloseTreatiesCell()
        {
            try
            {
                _treatiesCell.Close();
            }
            catch { }
        }
        public bool OpenAcceptDialog(string m)
        {
            AcceptDialog a = new AcceptDialog(m);
            return a.ShowDialog() ?? false;
        }
    }
}
