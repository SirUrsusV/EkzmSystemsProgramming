using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Совершенно_точно_не_подозрительный_процесс
{
    class Program
    {
        static void Main(string[] args)
        {
            // Десериализация настроек из аргументов
            var settings = new MonitoringSettings
            {
                LogFilePath = args[0],
                SaveIntervalSeconds = int.Parse(args[1])
            };
            StartMonitoring(settings);
        }

        static void StartMonitoring(MonitoringSettings settings)
        {
            while (true)
            {
                try
                {
                    var processes = Process.GetProcesses();
                    using (StreamWriter writer = new StreamWriter(settings.LogFilePath, true))
                    {
                        writer.WriteLine($"[{DateTime.Now}] Активные процессы:");
                        foreach (var process in processes)
                        {
                            writer.WriteLine($"- {process.ProcessName}");
                        }
                    }
                    Thread.Sleep(settings.SaveIntervalSeconds * 1000);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(settings.LogFilePath, $"Ошибка: {ex.Message}\n");
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
