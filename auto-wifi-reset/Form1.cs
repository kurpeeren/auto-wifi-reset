using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace auto_wifi_reset
{



    public partial class Form1 : Form
    {
        Thread thread1;
        public Form1()
        {
            InitializeComponent();
            thread1 = new Thread(taskloop);
            CheckForIllegalCrossThreadCalls =false;
            if (!WifiManager.IsUserAdministrator())
            {
                label1.Text = "Program is not running as administrator.";
            }
        }

        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {

            if(thread1.ThreadState == System.Threading.ThreadState.Running)
            { thread1.Abort(); }
            else {
                thread1 = new Thread(taskloop); 
                thread1.Start(); }



        }
        bool tick=true;
        Thread thread2;
        public void taskloop()
        {
            while (true)
            {

                textBox1.Text = WifiManager.GetDefaultGateway();
                if (textBox2.Text != textBox1.Text)
                {
                    WifiManager.DisableWifi();
                    while (WifiManager.IsWifiEnabled())
                    { Thread.Sleep(1000);
                        panel1.BackColor = tick ? Color.LightGreen : Color.Orange;
                        tick = !tick;
                    }
                }


                bool wifiEnabled = WifiManager.IsWifiEnabled();

                if (wifiEnabled)
                {
                    label1.Text = "WiFi is still enabled.";
                }
                else
                {
                    label1.Text = "WiFi is disabled.";
                    WifiManager.EnableWifi();
                    while(!WifiManager.IsWifiEnabled())
                    {
                        Thread.Sleep(1000);
                        panel1.BackColor = tick ? Color.LightGreen : Color.Orange;
                        tick = !tick;
                    }
                    textBox1.Text = WifiManager.GetDefaultGateway();
                }
                panel1.BackColor = tick ? Color.LightGreen : Color.Green;
                tick = !tick;
                Thread.Sleep(1000);


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WifiManager.EnableWifi();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WifiManager.DisableWifi();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = WifiManager.GetDefaultGateway();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread1.ThreadState != System.Threading.ThreadState.Aborted)
            { thread1.Abort(); }
        }

    }



    public class WifiManager
    {
        public static bool IsUserAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string GetDefaultGateway()
        {
            string defaultGateway = null;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation gateway in ni.GetIPProperties().GatewayAddresses)
                    {
                        if (gateway.Address != null)
                        {
                            defaultGateway = gateway.Address.ToString();
                            break;
                        }
                    }
                }
                if (defaultGateway != null)
                    break;
            }

            return defaultGateway;
        }

        private static void ExecuteCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process p = Process.Start(psi);
            if (p != null)
            {
                p.StandardInput.WriteLine(command);
                p.StandardInput.Flush();
                p.StandardInput.Close();
                p.WaitForExit();
            }
        }

        public static void DisableWifi()
        {
            ExecuteCommand("netsh interface set interface Wi-Fi admin=disable");
        }

        public static void EnableWifi()
        {
            ExecuteCommand("netsh interface set interface Wi-Fi admin=enable");
        }

        public static bool IsWifiEnabled()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId='Wi-Fi'");
            ManagementObjectCollection adapters = searcher.Get();

            foreach (ManagementObject adapter in adapters)
            {
                bool enabled = (bool)adapter["NetEnabled"];
                return enabled;
            }

            return false;
        }

    }




    
}
