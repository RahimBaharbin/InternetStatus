using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;

namespace InternetCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Size = new Size(5,5);
            Location = (Point)new Size(SystemInformation.WorkingArea.Width - Size.Width , SystemInformation.WorkingArea.Height - Size.Height);
            Opacity = 0.0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
          //  timer1.Enabled = true;
        }

  

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
       

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int Flage = 0;
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    if (new Ping().Send("8.8.8.8", 3000).Status == IPStatus.Success)
                    {
                        Flage = 0;
                        BackgroundImage = Properties.Resources.internet_ok;
                    }
                    else
                    {
                        BackgroundImage = Properties.Resources.internet_nok;
                        Flage++;
                        if (Flage == 5)
                        {
                            Process p_cmd;
                            if (new Ping().Send("192.168.1.1", 3000).Status == IPStatus.Success)
                            {
                                LaunchBrowser();
                                p_cmd = new Process() { StartInfo = new ProcessStartInfo() { FileName = "cmd.exe", Arguments = "/c ping 8.8.8.8 -t" } };
                                
                            }
                            else 
                            {
                                p_cmd = Process.Start(new ProcessStartInfo("NCPA.cpl") { UseShellExecute = true });
                            }                            
                            p_cmd.Start();
                            bool _flage = true;
                            while (_flage)
                            {
                                Process[] AllProcess = Process.GetProcesses(".");
                                foreach (Process p_col in AllProcess)
                                {
                                    if (p_col.Id == p_cmd.Id)
                                    {
                                        _flage = true;
                                        Thread.Sleep(5000);
                                        break;
                                    }
                                    else
                                        _flage = false;
                                }
                            }
                            p_cmd = null;
                        }

                    }
                }
                catch
                {
                    Flage++;
                    if (Flage == 2)
                    {
                        Thread.Sleep(5000);
                        BackgroundImage = Properties.Resources.DataGridViweDefultImage_28;
                        Process p = Process.Start(new ProcessStartInfo("NCPA.cpl") { UseShellExecute = true });
                        bool flage = true;
                        while (flage)
                        {
                            Process[] AllProcess = Process.GetProcesses(".");
                            foreach (Process p_col in AllProcess)
                            {
                                if (p_col.Id == p.Id)
                                {
                                    flage = true;
                                    Thread.Sleep(5000);
                                    break;
                                }
                                else
                                    flage = false;
                            }
                        }
                        p = null;
                    }
                }
            
                // _ = Invoke((MethodInvoker)delegate ()
                //{
                //    if (Status)
                //        pictureBox1.Image = InternetCheck.Properties.Resources.ok;
                //    else
                //        pictureBox1.Image = InternetCheck.Properties.Resources.not;

                //});

            }
        }
        private Process LaunchBrowser()
        {
            string browserName = "iexplore.exe";
            using (Microsoft.Win32.RegistryKey userChoiceKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                if (userChoiceKey != null)
                {
                    string[] name = userChoiceKey.GetValueNames();
                    foreach (string item in name)
                    {
                        if (item.ToLower().Contains("progid"))
                        {
                            object progIdValue = userChoiceKey.GetValue(item);
                            if (progIdValue != null)
                            {
                                if (progIdValue.ToString().ToLower().Contains("chrome"))
                                    browserName = "chrome.exe";
                                else if (progIdValue.ToString().ToLower().Contains("firefox"))
                                    browserName = "firefox.exe";
                                else if (progIdValue.ToString().ToLower().Contains("safari"))
                                    browserName = "safari.exe";
                                else if (progIdValue.ToString().ToLower().Contains("opera"))
                                    browserName = "opera.exe";
                                else if (progIdValue.ToString().ToLower().Contains("msedge"))
                                    browserName = "msedge.exe";
                                else if (progIdValue.ToString().ToLower().Contains("yandex"))
                                    browserName = "browser.exe";
                            }
                        }
                    }

                }
            }
            try
            {
                return Process.Start(browserName, "192.168.1.1");
            }
            catch
            {
                return Process.Start("iexplore.exe", "192.168.1.1");
            }

        }
        private void topMostOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void topMostOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = false;
        }
    }
}

