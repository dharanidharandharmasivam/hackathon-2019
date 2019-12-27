using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for APKGeneration.xaml
    /// </summary>
    public partial class APKGeneration : Page
    {
        MailWindow window;
        public APKGeneration()
        {
            InitializeComponent();
        }
        private void GenerateAPK_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"D:\projects\funnel3test\flutter-charts\flutter_charts\flutter_charts_testbed";
            string command = "flutter analyze";
            executePowershell(directory, command, (sender as Button).Content == "Generate App Bundle");
        }



        private void executePowershell(string location, string command, bool isAppBundle)
        {
            var Restoreprocess = new Process();
            Restoreprocess.StartInfo.FileName = "cmd.exe";
            Restoreprocess.StartInfo.WorkingDirectory = location;
            Restoreprocess.StartInfo.Arguments = "/c " + command;
            Restoreprocess.StartInfo.UseShellExecute = false;
            Restoreprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Restoreprocess.StartInfo.Verb = "runas";
            Restoreprocess.StartInfo.RedirectStandardOutput = true;
            Restoreprocess.Start();
            Restoreprocess.WaitForExit();
            var output = Restoreprocess.StandardOutput.ReadToEnd();
            String fileName = System.IO.Path.Combine(location, "log.txt");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter outputFile = new StreamWriter(fileName);
            outputFile.Close();
            File.WriteAllText(fileName, output);
            if (output.Contains("No issues found!"))
            {
                MessageBox.Show("Flutter Analyzed successfully");
            }
            else
            {
                int index = output.IndexOf("issues found");
                if (index == -1)
                {
                    MessageBox.Show("issues found");
                    File.WriteAllText(fileName, output);
                    return;
                }



                File.WriteAllText(fileName, output);
                MessageBox.Show(output[index - 3] + " issues found");
            }


            if (!isAppBundle)
            {
                Restoreprocess.StartInfo.Arguments = "/c " + "flutter build apk";
                Restoreprocess.Start();
                output = Restoreprocess.StandardOutput.ReadToEnd();
                Restoreprocess.WaitForExit();
                if (output.Contains(@"build\app\outputs\apk\release\"))
                {
                    MessageBox.Show("Apk generated successfully");
                    sendAttachment(location + @"\build\app\outputs\apk\release\app-release.apk");
                }
                else
                {
                    File.WriteAllText(fileName, output);
                    MessageBox.Show("Apk generation failed");
                }
            }
            else
            {
                Restoreprocess.StartInfo.Arguments = "/c " + "flutter build appbundle";
                Restoreprocess.Start();
                output = Restoreprocess.StandardOutput.ReadToEnd();
                Restoreprocess.WaitForExit();
               
            }
        }



        private void sendAttachment(string attachment)
        {
            window = new MailWindow();
            string to = "";
            window.ShowDialog();
            to = window.Email;
            if (to == "")
            {
                return;
            }
            Microsoft.Office.Interop.Outlook.MailItem message = new Microsoft.Office.Interop.Outlook.Application().CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            message.To = to;
            message.Subject = "Testing";
            Microsoft.Office.Interop.Outlook.Attachment attachement = message.Attachments.Add(attachment);
            message.Body = "Test Mail";
            message.Send();
            MessageBox.Show("Message Sent to " + to);
        }
    }
}
