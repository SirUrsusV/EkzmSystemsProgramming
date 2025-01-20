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
using System.Windows.Shapes;

namespace EkzSystemsProgramming
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        SettingsMonitoring _settingsMonitoring;
        public SettingsWindow(SettingsMonitoring settingsMonitoring)
        {
            InitializeComponent();
            _settingsMonitoring = settingsMonitoring;

            LogPathTBox.Text = _settingsMonitoring.PathLogs;
            TimeSaveTBox.Text = _settingsMonitoring.TimeBetweenLogging.ToString();
            LogProcessCBox.IsChecked = _settingsMonitoring.ProcessLogging;
            LogKeyCBox.IsChecked = _settingsMonitoring.KeyLogging;

            ConfirmBt.IsEnabled = false;
        }
        private void ChangeValue()
        {
            ConfirmBt.IsEnabled = true;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmBt.IsEnabled = false;
            try
            {
                _settingsMonitoring.PathLogs = LogPathTBox.Text;
                _settingsMonitoring.TimeBetweenLogging = double.Parse(TimeSaveTBox.Text);
                _settingsMonitoring.ProcessLogging = (bool)LogProcessCBox.IsChecked;
                _settingsMonitoring.KeyLogging = (bool)LogKeyCBox.IsChecked;

                _settingsMonitoring.SaveSettingsToRegistry();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void LogPathTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue();
        }

        private void LogProcessCBox_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue();
        }
    }
}
