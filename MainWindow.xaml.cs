using System;
using System.Collections.Generic;
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
						Start();
                        Title = _name + ": работает";
                        _working = StatusWorking.Working;
                        break;
                    case StatusWorking.DontWorking:
						Stop();
                        Title = _name + ": в ожидании";
                        _working = StatusWorking.DontWorking;
                        break;
                }
            }
		}

		string _name;
		public MainWindow()
		{
			InitializeComponent();
            _name = "Монитроринг рабочего времени";
            Working = StatusWorking.DontWorking;

		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			switch(_working)
			{
				case StatusWorking.DontWorking:
					_startBt.Content = "Стоп";
                    Working = StatusWorking.Working;
					break;
				case StatusWorking.Working:
					_startBt.Content = "Страт";
                    Working = StatusWorking.DontWorking;
					break;
			}
		}
		private void Start()
		{

		}
		private void Stop()
		{

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
}