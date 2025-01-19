using Microsoft.Win32;
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
using System.IO;

namespace EkzSystemsProgramming
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		enum StatusWorking
		{
			Working,
			DontWorking
		}

		private StatusWorking _working;
		private StatusWorking Working
		{
			get
			{ 
				return _working; 
			}
			set 
			{
				switch (value)
				{
					case StatusWorking.Working:
						Title = _name + ": работает";
						_working = StatusWorking.Working;
                        StartBt.Content = "Стоп";
                        SettingBt.IsEnabled = false;
                        break;
					case StatusWorking.DontWorking:
						Title = _name + ": в ожидании";
						_working = StatusWorking.DontWorking;
                        StartBt.Content = "Страт";
                        SettingBt.IsEnabled = true;
                        break;
				}
			}
		}
		private SettingsMonitoring _settings;
		string _name;
        string _processName = "Совершенно точно не подозрительный процесс";
        public MainWindow()
		{
			InitializeComponent();
			SetSettings();
			_name = "Монитроринг рабочего времени";

            Process[] existingProcess = Process.GetProcessesByName(_processName);
			if (existingProcess.Length > 0)
			{
                Working = StatusWorking.Working;

            }
            else
                Working = StatusWorking.DontWorking;
        }
		private void SetSettings()
		{
			_settings = new SettingsMonitoring();

		}
		private void Start_Click(object sender, RoutedEventArgs e)
		{
			switch(_working)
			{
				case StatusWorking.DontWorking:
					Working = StatusWorking.Working;
					Start();
					break;
				case StatusWorking.Working:
                    Working = StatusWorking.DontWorking;
                    Stop();
					break;
			}
		}
		private void Start()
		{
			string logFilePath = "monitoring.log";
			int saveIntervalSeconds = 60;

            // Проверяем, запущен ли процесс
            Process[] existingProcess = Process.GetProcessesByName(_processName);
			MessageBox.Show("Запуск процесса мониторинга...");

			// Запуск нового процесса
			string monitoringProcessPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{_processName}.exe");
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = monitoringProcessPath,
				Arguments = $"{logFilePath} {saveIntervalSeconds}",
				CreateNoWindow = true,
				UseShellExecute = false
			};

			Process.Start(startInfo);
			Console.WriteLine("Процесс мониторинга запущен.");
		}
		private void Stop()
        {
			foreach (Process process in Process.GetProcessesByName(_processName)) 
				process.Kill();
        }

		private void Settings_Click(object sender, RoutedEventArgs e)
		{
			SettingsWindow settingsWindow = new SettingsWindow();
			settingsWindow.Show();
		}

		private void OpenFolderLogs_Click(object sender, RoutedEventArgs e)
		{

		}
	}

	public struct ForbiddenKeyCombination
	{
		public string KeyCombination;
		public bool СaseSensitive;
	}
	public class SettingsMonitoring
	{

		public string PathLogs;
		public double TimeBetweenLogging;
		public bool ProcessLogging;
		public bool KeyLogging;
		public List<string> ListNameCensurProcess;
		public List<ForbiddenKeyCombination> ListKeyboardShortcut;
		private const string RegistryPath = @"SOFTWARE\MyApp\SettingsMonitoring";

		public void SaveSettingsToRegistry()
		{
			using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath))
			{
				if (key != null)
				{
					key.SetValue(nameof(PathLogs), PathLogs);
					key.SetValue(nameof(TimeBetweenLogging), TimeBetweenLogging);
					key.SetValue(nameof(ProcessLogging), ProcessLogging);
					key.SetValue(nameof(KeyLogging), KeyLogging);

					// Сохраняем список ListNameCensurProcess как строку, разделенную запятыми
					if (ListNameCensurProcess != null)
					{
						key.SetValue(nameof(ListNameCensurProcess), string.Join(",", ListNameCensurProcess));
					}

					// Сохраняем список ListKeyboardShortcut как строку, разделенную точкой с запятой
					if (ListKeyboardShortcut != null)
					{
						var shortcuts = new List<string>();
						foreach (var shortcut in ListKeyboardShortcut)
						{
							shortcuts.Add($"{shortcut.KeyCombination}:{shortcut.СaseSensitive}");
						}
						key.SetValue(nameof(ListKeyboardShortcut), string.Join(";", shortcuts));
					}
				}
			}
		}

		public void LoadSettingsFromRegistry()
		{
			ListNameCensurProcess = new List<string>();
			ListKeyboardShortcut = new List<ForbiddenKeyCombination>();

			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
			{
				if (key != null)
				{
					PathLogs = key.GetValue(nameof(PathLogs), string.Empty).ToString();
					TimeBetweenLogging = double.TryParse(key.GetValue(nameof(TimeBetweenLogging), "0").ToString(), out double time) ? time : 0;
					ProcessLogging = bool.TryParse(key.GetValue(nameof(ProcessLogging), "false").ToString(), out bool processLogging) && processLogging;
					KeyLogging = bool.TryParse(key.GetValue(nameof(KeyLogging), "false").ToString(), out bool keyLogging) && keyLogging;

					// Загружаем список ListNameCensurProcess
					string censurProcesses = key.GetValue(nameof(ListNameCensurProcess), string.Empty).ToString();
					if (!string.IsNullOrEmpty(censurProcesses))
					{
						ListNameCensurProcess = new List<string>(censurProcesses.Split(','));
					}

					// Загружаем список ListKeyboardShortcut
					string shortcuts = key.GetValue(nameof(ListKeyboardShortcut), string.Empty).ToString();
					if (!string.IsNullOrEmpty(shortcuts))
					{
						var shortcutPairs = shortcuts.Split(';');
						foreach (var pair in shortcutPairs)
						{
							var parts = pair.Split(':');
							if (parts.Length == 2)
							{
								ListKeyboardShortcut.Add(new ForbiddenKeyCombination
								{
									KeyCombination = parts[0],
									СaseSensitive = bool.Parse(parts[1])
								});
							}
						}
					}
				}
			}
		}
	}
}