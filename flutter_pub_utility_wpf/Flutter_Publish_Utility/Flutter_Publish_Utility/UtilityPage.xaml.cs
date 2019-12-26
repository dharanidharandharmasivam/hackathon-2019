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
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    public partial class UtilityPage : Page
    {
        public UtilityPage()
        {
            InitializeComponent();
        }

        private void PowerShellButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"D:\src\classStruct\flutter_gauges\testbed_samples\gardient_pointer";
            string command = "flutter --version";
            RunPowerShell(directory, command);
        }

        private void RunPowerShell(string location, string command)
        {
            var Restoreprocess = new Process();
            Restoreprocess.StartInfo.FileName = "cmd.exe";
            Restoreprocess.StartInfo.WorkingDirectory = location;
            Restoreprocess.StartInfo.Arguments = "/c " + command;
            Restoreprocess.StartInfo.UseShellExecute = false;
            Restoreprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Restoreprocess.StartInfo.Verb = "runas";
            Restoreprocess.Start();
            Restoreprocess.WaitForExit(60000);
        }

        private void GenerateIPK_Click(object sender, RoutedEventArgs e)
        {

        }

    

        private void GenerateAPK_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"D:\src\classStruct\flutter_gauges\testbed_samples\gardient_pointer";
            string command = "flutter analyze";
            executePowershell(directory, command);
        }

        private void WebHost_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"D:\src\classStruct\flutter_gauges\testbed_samples\gardient_pointer";
            string command = "flutter packages get&&flutter channel beta&&flutter config --enable-web&&flutter config --enable-web&&flutter create .&&flutter run -d chrome";
            RunPowerShell(directory, command);
        }

        private void executePowershell(string location, string command)
        {
            //var Restoreprocess = new System.Diagnostics.Process();
            //Restoreprocess.StartInfo.RedirectStandardOutput = true;
            //String fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "commands.bat");
            //if (File.Exists(fileName))
            //{
            //    File.Delete(fileName);
            //}
            //StreamWriter outputFile = new StreamWriter(fileName);
            //outputFile.WriteLine(command);
            //outputFile.Close();
            //Restoreprocess.StartInfo.WorkingDirectory = location;
            //Restoreprocess.StartInfo.Arguments = @"-X";
            //Restoreprocess.StartInfo.UseShellExecute = false;
            //Restoreprocess.StartInfo.FileName = fileName;
            //Restoreprocess.Start();

            var Restoreprocess = new Process();
            Restoreprocess.StartInfo.FileName = "cmd.exe";
            Restoreprocess.StartInfo.WorkingDirectory = location;
            Restoreprocess.StartInfo.Arguments = "/c " + command;
            Restoreprocess.StartInfo.UseShellExecute = false;
            Restoreprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Restoreprocess.StartInfo.Verb = "runas";
            Restoreprocess.StartInfo.RedirectStandardOutput = true;
            Restoreprocess.Start();
            var output = Restoreprocess.StandardOutput.ReadToEnd();
           
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
                    return;
                }



                MessageBox.Show(output[index - 3] + " issues found");
            }



            Restoreprocess.WaitForExit();
            //if (File.Exists(fileName))
            //{
            //    File.Delete(fileName);
            //}



            //outputFile = new StreamWriter(fileName, true);
            //outputFile.WriteLine("flutter build apk");
            //outputFile.Close();
            //Restoreprocess.StartInfo.FileName = fileName;
            Restoreprocess.StartInfo.Arguments = "/c " + "flutter build apk";
            Restoreprocess.Start();
            output = Restoreprocess.StandardOutput.ReadToEnd();
            Restoreprocess.WaitForExit();
            if (output.Contains(@"build\app\outputs\apk\release\"))
            {
                sendAttachment(location + @"\build\app\outputs\apk\release\app-release.apk");
            }
            else
            {
                MessageBox.Show("Apk generation failed");
            }
        }



        private void sendAttachment(string attachment)
        {
            string to = "dharanidharan.dharmasivam@syncfusion.com";
            Microsoft.Office.Interop.Outlook.MailItem message = new Microsoft.Office.Interop.Outlook.Application().CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            message.To = to;
            message.Subject = "Testing";
            Microsoft.Office.Interop.Outlook.Attachment attachement = message.Attachments.Add(attachment);
            message.Body = "Test Mail";
            message.Send();
            MessageBox.Show("Message Sent to " + to);
        }

        private void PubPublish_Click(object sender, RoutedEventArgs e)
        {
            bool isWarningsError = false;
            if (!isWarningsError)
            {
                string[] deletingDirectories = { "test", "build", "images" };
                Remove_Directories(deletingDirectories, @"D:\Hackathon-2019\Dependencies\flutter-charts\flutter_charts\syncfusion_flutter_charts");
                Remove_TestScript_Reference(@"D:\Hackathon-2019\Dependencies\flutter-charts\flutter_charts\syncfusion_flutter_charts");
                //string directory = @"D:\Hackathon-2019\Dependencies\flutter-charts\flutter_charts\syncfusion_flutter_charts";
                //string command = "dartfmt -w lib";
                //RunPowerShell(directory, command);
            }

        }
        private void Remove_Directories(string[] deleteDirectories, string location)
        {
            for (int i = 0; i < deleteDirectories.Length; i++)
            {
                if (deleteDirectories[i] == "test" && Directory.Exists(location + @"\lib\src\" + deleteDirectories[i]))
                {
                    Directory.Delete(location + @"\lib\src\" + deleteDirectories[i], true);
                }
                else if (Directory.Exists(location + @"\" + deleteDirectories[i]))
                {
                    Directory.Delete(location + @"\" + deleteDirectories[i], true);
                }
            }
        }



        private void Remove_TestScript_Reference(string location)
        {
            string[] locationSeperator = new string[] { "syncfusion_flutter_" };
            string[] locationList = location.Split(locationSeperator, StringSplitOptions.None);
            string controlName = locationList[locationList.Length - 1].ToString().Trim();
            string scriptReferenceText = File.ReadAllText(location + @"\lib\" + controlName + ".dart", Encoding.UTF8);
            scriptReferenceText = scriptReferenceText.Remove(scriptReferenceText.IndexOf("// export test scripts"));
            File.WriteAllText(location + @"\lib\" + controlName + ".dart", scriptReferenceText);
        }
    }
}
