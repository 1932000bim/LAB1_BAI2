using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace LAB1_18520948
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;) 
        Timer timerCheck = new Timer();
        public Service1()
        {
            InitializeComponent();
        }
 
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
           
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds 
            timer.Enabled = true;
            timerCheck.Elapsed += new ElapsedEventHandler(OnElapsedTimeCheck);
            timerCheck.Interval = 3000; //number in milisecinds 
            timerCheck.Enabled = true;

        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Process note = Process.Start("Notepad++");

            System.Threading.Thread.Sleep(5000);

            note.Kill();
        }

        private void OnElapsedTimeCheck(object source, ElapsedEventArgs e)
        {
           

            if (IsRunning("Notepad++") == true)
            {
                WriteToFile("process is running");
            }
            if (IsRunning("Notepad++") == false)
            {
                WriteToFile("process stops");
            }

        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory +
           "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') +
           ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public bool IsRunning(string pName)
        {
            Process[] pname = Process.GetProcessesByName(pName);
            if (pname.Length == 0)
                return false;
            else
                return true;
        }
    } 
}
