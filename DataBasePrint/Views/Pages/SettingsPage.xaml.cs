using DataBasePrint.ViewModels;
using NPOI.SS.Formula.Functions;
using NPOI.Util.Collections;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace DataBasePrint.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        
        private  Configuration _config;
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }
        
        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            // _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int lastNumberOfSpaces = Properties.Settings.Default.NumberOfSpace;
            SpacenumBox.Value = lastNumberOfSpaces;
            if ((string.IsNullOrEmpty(Properties.Settings.Default.IPPADDRS)))
            {
                Properties.Settings.Default.IPPADDRS="127.0.0.1";
                Properties.Settings.Default.Save();
            }

            if ((string.IsNullOrEmpty(Properties.Settings.Default.IPPPORT)))
            {
                Properties.Settings.Default.IPPPORT = "2000";
                Properties.Settings.Default.Save();
            }

            txtIPAddress.Text = Properties.Settings.Default.IPPADDRS;
            txtPort.Text = Properties.Settings.Default.IPPPORT;

            btnSave.Click += btnSave_Click;
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.IPPADDRS = txtIPAddress.Text;
            Properties.Settings.Default.IPPPORT = txtPort.Text;
            Properties.Settings.Default.Save();

            

            MessageBox.Show("Nastavenia boli uložené!");
        }

        private void SpacenumBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numberOfSpaces = (int)(SpacenumBox.Value);
            Properties.Settings.Default.NumberOfSpace = numberOfSpaces;
            Properties.Settings.Default.Save();
        }
        private void SpacenumBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int numberOfSpaces = (int)(SpacenumBox.Value);
            Properties.Settings.Default.NumberOfSpace = numberOfSpaces;
            Properties.Settings.Default.Save();
        }
    }
}