using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flutter_Publish_Utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PowerShellButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"D:\src\flutterSB_MR\flutter_examples\flutter_examples";
            string command = "flutter --version";
            RunPowerShell(directory, command);
        }

        private void RunPowerShell(string location, string command)
        {
            var Restoreprocess = new System.Diagnostics.Process();
            Restoreprocess.StartInfo.FileName = "cmd.exe";
            Restoreprocess.StartInfo.WorkingDirectory = location;
            Restoreprocess.StartInfo.Arguments = "/c " + command;
            Restoreprocess.StartInfo.UseShellExecute = false;
            Restoreprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Restoreprocess.StartInfo.Verb = "runas";
            Restoreprocess.Start();
            Restoreprocess.WaitForExit();
        }

        private void PubPublish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateAPK_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WebHost_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
