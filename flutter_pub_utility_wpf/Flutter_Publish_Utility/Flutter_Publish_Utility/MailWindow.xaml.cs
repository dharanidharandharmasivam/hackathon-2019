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

namespace Flutter_Publish_Utility
{
    /// <summary>
    /// Interaction logic for MailWindow.xaml
    /// </summary>
    public partial class MailWindow : Window
    {
        public String Email = "";
        public MailWindow()
        {
            InitializeComponent();
            Email = "";
        }



        private void Answer_TextChanged(object sender, TextChangedEventArgs e)
        {
            Email = (sender as TextBox).Text;
        }



        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
