using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Single.View
{
    /// <summary>
    /// Логика взаимодействия для AcceptDialog.xaml
    /// </summary>
    public partial class AcceptDialog : Window
    {
        public AcceptDialog(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }

        private void Accept(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Decline(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
