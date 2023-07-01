using System;
using System.Diagnostics;
using System.Xml;
using System.Threading;

namespace ProcessMonitor
{
    class ProcessMonitor
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("processes.xml");

            XmlNodeList processes = doc.GetElementsByTagName("process");

            foreach (XmlNode process in processes)
            {
                string processName = process.InnerText;

                Console.WriteLine("Monitoring process: " + processName);

                Process proc = new Process();
                proc.StartInfo.FileName = processName;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;

                try
                {
                    proc.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error starting process: " + ex.Message);
                }

                while (true)
                {
                    if (proc.HasExited)
                    {
                        Console.WriteLine("Process has stopped, attempting to restart...");
                        try
                        {
                            proc.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error restarting process: " + ex.Message);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
