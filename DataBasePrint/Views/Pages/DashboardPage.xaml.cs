using Wpf.Ui.Common.Interfaces;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Linq;
using System.IO.Packaging;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using System.Threading.Tasks;
//using System.Media;
using System.Windows.Media;
using System.Globalization;
using Wpf.Ui.Interop.WinDef;
using System.ComponentModel;
using System.Net;
using NPOI.OpenXmlFormats.Wordprocessing;


namespace DataBasePrint.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public static string intervalOp;
        public static string rowCount;
        public static string filePath2;
        public static int sheetIndex;
        public static string fontSize;
        public static string medzpred;
        public static string medzza;
        public string expan;
        public static string ipAddress;
        public static int ipport;
        public static string ip;
        public static int port;
        public string speedSetVar;
        public static string tacho;
        public string pocetopak;
        string input;
        public bool isPrinterOnline;
        private bool _isPrinterOnline;
        int spacenumber;


        private DataGridRow _selectedRow = null;
        private DataGridRow _previousSelectedRow;

        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }
       



        private readonly DataProManager _profileManagerdatapro = new DataProManager(@"databaseprofiles.xml");
        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            




        Properties.Settings.Default.PropertyChanged += (sender, e) =>
            {
                // Ak sa zmení nastavenie IPAddress, aktualizuj premennú ipAddress
                if (e.PropertyName == "IPPADDRS")
                {
                    ipAddress = Properties.Settings.Default.IPPADDRS;
                }
                // Ak sa zmení nastavenie PortNumber, aktualizuj premennú ipport
                else if (e.PropertyName == "IPPPORT")
                {
                    ipport = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
                }
                else if (e.PropertyName == "NumberOfSpace")
                {
                    spacenumber = (Properties.Settings.Default.NumberOfSpace);
                }
            };

            // Načítajte počiatočné hodnoty nastavení do premenných
            ipAddress = Properties.Settings.Default.IPPADDRS;
            ipport = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
            spacenumber= (Properties.Settings.Default.NumberOfSpace);
            //testovanie stavu, či je tlačiareň on-line a úprava výstupu nekonzistetnej odpoovede

            

            _profileManagerdatapro.LoadProfilesdatapro();
            foreach (Profiledatapro profiledatapro in _profileManagerdatapro.Profilesdatapro)
            {
                ProfileSetCombodatapro.Items.Add(profiledatapro.Namedatapro);
            }
            ProfileSetCombodatapro.SelectedIndex = 0;
            _profileManagerdatapro.LoadProfilesdatapro();
            foreach (Profiledatapro profiledatapro in _profileManagerdatapro.Profilesdatapro)
            {
                saveProfiledatapro.Items.Add(profiledatapro.Namedatapro);
            }
            // dáta pre combobox veľkosť písma
            int[] items = new int[] { 5, 7, 9, 11, 16, 24 };
            foreach (int item in items)
            {
                comboBox1.Items.Add(item);
            }
           // comboBox1.SelectedIndex = 0;

            // dáta pre combobox zvýraznenie
            int[] zvyraz = new int[] { 1, 2, 3, 4 };
            foreach (int item in zvyraz)
            {
                zvyrazCombo.Items.Add(item);
            }
          //  zvyrazCombo.SelectedIndex = 0;
            //comboboxy parametrov
            jobHorizontalDirectionComboBox.Items.Add("Normálny");
            jobHorizontalDirectionComboBox.Items.Add("Invertovať");
            //jobHorizontalDirectionComboBox.SelectedIndex = 0;

            charactersHorizontalDirectionComboBox.Items.Add("Normálny");
            charactersHorizontalDirectionComboBox.Items.Add("Invertovať");
           // charactersHorizontalDirectionComboBox.SelectedIndex = 0;

            charactersVerticalDirectionComboBox.Items.Add("Normálny");
            charactersVerticalDirectionComboBox.Items.Add("Invertovať");
           // charactersVerticalDirectionComboBox.SelectedIndex = 0;

            tachoModeComboBox.Items.Add("Konštantná rýchlosť");
            tachoModeComboBox.Items.Add("Tacho");
          //  tachoModeComboBox.SelectedIndex = 0;

            printingModeComboBox.Items.Add("Objekt");
            printingModeComboBox.Items.Add("Opakovanie");
          //  printingModeComboBox.SelectedIndex = 0;

            unitTypeComboBox.Items.Add("mm");
            unitTypeComboBox.Items.Add("H-frame");
          //  unitTypeComboBox.SelectedIndex = 0;


        }

       
              
     

        private void OpenExcel_Click(object sender, RoutedEventArgs e)
        { 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            if (openFileDialog.ShowDialog() == true)
            {

                string filePath = openFileDialog.FileName;
                fileBox.Text = Path.GetFileName(filePath);

                try
                {
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        IWorkbook workbook;
                        try
                        {
                            workbook = new XSSFWorkbook(stream);
                        }
                        catch (Exception)
                        {
                            stream.Position = 0;
                            try
                            {
                                workbook = new HSSFWorkbook(stream);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Nesprávny formát súboru.");
                                return;
                            }
                        }

                        List<string> sheetNames = new List<string>();
                        for (int i = 0; i < workbook.NumberOfSheets; i++)
                        {
                            sheetNames.Add(workbook.GetSheetAt(i).SheetName);
                        }
                        ComboBox.ItemsSource = sheetNames;
                        workbook.Close();
                        ComboBox.SelectionChanged += (s, args) => ComboBox_SelectionChanged(s, args, filePath);

                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("CHYBA!: " + ex.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Súbor je otvorený v inom programe.");
                }
            }
        }
        private void ComboBox_SelectionChanged(object s, SelectionChangedEventArgs args, string filePath)
        {

            int selectedIndex = ComboBox.SelectedIndex;
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    try
                    {
                        workbook = new XSSFWorkbook(stream);
                    }
                    catch (Exception)
                    {
                        stream.Position = 0;
                        try
                        {
                            workbook = new HSSFWorkbook(stream);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Nesprávny formát súboru.");
                            return;
                        }
                    }

                    if (selectedIndex >= 0 && selectedIndex < workbook.NumberOfSheets)
                    {
                        filePath2 = filePath;
                        ISheet sheet = workbook.GetSheetAt(selectedIndex);
                        int totalRows = sheet.LastRowNum;
                        if (totalRows > 0)
                        {
                            IRow headerRow = sheet.GetRow(0);
                            int totalColumns = headerRow.LastCellNum;
                            if (totalColumns > 0)
                            {
                                DataTable dataTable = new DataTable();
                                for (int i = 0; i < totalColumns; i++)
                                {
                                    dataTable.Columns.Add(i.ToString());
                                }
                                for (int i = 0; i <= totalRows; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row == null) continue;
                                    DataRow dataRow = dataTable.NewRow();
                                    for (int j = 0; j < totalColumns; j++)
                                    {
                                        ICell cell = row.GetCell(j);
                                        if (cell == null) continue;
                                        dataRow[j] = cell.ToString();
                                    }
                                    dataTable.Rows.Add(dataRow);
                                    sheetIndex = selectedIndex;
                                }

                                DataGrid.ItemsSource = dataTable.DefaultView;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CHYBA!: " + ex.Message);
            }
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {

        }




        

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dataGrid = sender as DataGrid;
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            if (dataRowView != null)
            {
                string result = "";
                int rowIndex = dataGrid.Items.IndexOf(dataRowView);
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);

                // zmena farby okraja pre predchádzajúci vybraný riadok (ak existuje)
                if (_selectedRow != null)
                {
                    
                    _selectedRow.BorderBrush = Brushes.Transparent;
                    _selectedRow.BorderThickness = new Thickness(0);
                 
                }

                // uloženie odkazu na nový vybraný riadok
                _selectedRow = selectedRow;

                // zafarbenie nového vybraného riadku
                _selectedRow.BorderBrush = Brushes.ForestGreen;
                _selectedRow.BorderThickness = new Thickness(0, 2, 0, 2);


              
                // uloženie odkazu na nový vybraný riadok
                _previousSelectedRow = selectedRow;
                               
               

                //  MessageBox.Show("Vybraný riadok má index: " + rowIndex);
                using (FileStream stream = File.Open(filePath2, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    if (filePath2.EndsWith(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(stream);
                    }
                    else if (filePath2.EndsWith(".xls"))
                    {
                        workbook = new HSSFWorkbook(stream);
                    }
                    else
                    {
                        throw new Exception("Nepodporovaný formát súboru.");
                    }
                    string multipliedString = new string(' ', spacenumber);
                    result = "";
                    IRow row = workbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                    if (chkColumn1.IsChecked == true)
                    {
                        if (row.GetCell(0) != null)
                        {
                            result += row.GetCell(0).ToString();
                        }
                    }
                    if (chkColumn2.IsChecked == true)
                    {
                        if (row.GetCell(1) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(1).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(1).ToString();
                            }
                        }
                    }
                    if (chkColumn3.IsChecked == true)
                    {
                        if (row.GetCell(2) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(2).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(2).ToString();
                            }
                        }
                    }
                    if (chkColumn4.IsChecked == true)
                    {
                        if (row.GetCell(3) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(3).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(3).ToString();
                            }
                        }
                    }
                    if (chkColumn5.IsChecked == true)
                    {
                        if (row.GetCell(4) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(4).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(4).ToString();
                            }
                        }
                    }
                    if (chkColumn6.IsChecked == true)
                    {
                        if (row.GetCell(5) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(5).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(5).ToString();
                            }
                        }
                    }
                    if (chkColumn7.IsChecked == true)
                    {
                        if (row.GetCell(6) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(6).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(6).ToString();
                            }
                        }
                    }
                    if (chkColumn8.IsChecked == true)
                    {
                        if (row.GetCell(7) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(7).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(7).ToString();
                            }
                        }

                    }
                    if (chkColumn9.IsChecked == true)
                    {
                        if (row.GetCell(8) != null)
                        {
                            if (result.Length > 0)
                            {
                                string cellValue = row.GetCell(8).ToString().Trim();
                                result += multipliedString + cellValue;
                            }
                            else
                            {
                                result += row.GetCell(8).ToString();
                            }
                        }
                    }

                }
                txtResult.Text = RemoveDiacritics(result);
          }

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            {
                switch (comboBox1.SelectedItem)
                {
                    case 5:
                        fontSize = "011A";
                        break;
                    case 7:
                        fontSize = "011B";
                        break;
                    case 9:
                        fontSize = "011C";
                        break;
                    case 11:
                        fontSize = "011D";
                        break;
                    case 16:
                        fontSize = "011E";
                        break;
                    case 24:
                        fontSize = "011F";
                        break;
                }
            }
        }

        private void zvyrazCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                int selectedValue = Convert.ToInt32(zvyrazCombo.SelectedItem);
                switch (selectedValue)
                {
                    case 1:
                        expan = "01";
                        break;
                    case 2:
                        expan = "02";
                        break;
                    case 3:
                        expan = "03";
                        break;
                    case 4:
                        expan = "04";
                        break;
                }
            }
        }

        private void tachoModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tachoModeComboBox.SelectedItem.ToString() == "Tacho")
            {
                constSpeedSet.IsEnabled = false;
                delenietacha.IsEnabled = true;
            }
            else if (tachoModeComboBox.SelectedItem.ToString() == "Konštantná rýchlosť")
            {
                constSpeedSet.IsEnabled = true;
                delenietacha.IsEnabled = false;
            }
        }
        private void printingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (printingModeComboBox.SelectedItem.ToString() == "Objekt")
            {
                intarvalOP.IsEnabled = false;
                pocetOP.IsEnabled = false;
            }
            else if (printingModeComboBox.SelectedItem.ToString() == "Opakovanie")
            {
                intarvalOP.IsEnabled = true;
                pocetOP.IsEnabled = true;
            }
        }

        private void IntarvalOP_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox intarvalOP = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(intarvalOP.Text))
            {
                intarvalOP.Text = "2";
            }

            if (!int.TryParse(intarvalOP.Text, out number3))
            {
                number3 = 2;
            }
            else if (number3 < 2)
            {
                number3 = 2;
            }
            else if (number3 > 6363)
            {
                number3 = 6363;
            }

            intarvalOP.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            intervalOp = string.Format("{0:X4}", number3);
        }

        private void medzerapred_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox medzerapred = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(medzerapred.Text))
            {
                medzerapred.Text = "3";
            }

            if (!int.TryParse(medzerapred.Text, out number3))
            {
                number3 = 3;
            }
            else if (number3 < 3)
            {
                number3 = 3;
            }
            else if (number3 > 6363)
            {
                number3 = 6363;
            }

            medzerapred.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            medzpred = string.Format("{0:X4}", number3);
        }
        private void medzeraza_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox medzeraza = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(medzeraza.Text))
            {
                medzeraza.Text = "3";
            }

            if (!int.TryParse(medzeraza.Text, out number3))
            {
                number3 = 3;
            }
            else if (number3 < 3)
            {
                number3 = 3;
            }
            else if (number3 > 6363)
            {
                number3 = 6363;
            }

            medzeraza.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            medzza = string.Format("{0:X4}", number3);
        }
        private void delenietacha_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox delenietacha = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(delenietacha.Text))
            {
                delenietacha.Text = "7";
            }

            if (!int.TryParse(delenietacha.Text, out number3))
            {
                number3 = 1;
            }
            else if (number3 < 1)
            {
                number3 = 1;
            }
            else if (number3 > 63)
            {
                number3 = 63;
            }

            delenietacha.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            tacho = string.Format("{0:X2}", number3);
        }

        private void pocetOP_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox pocetOP = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(pocetOP.Text))
            {
                pocetOP.Text = "0";
            }

            if (!int.TryParse(pocetOP.Text, out number3))
            {
                number3 = 0;
            }
            else if (number3 < 0)
            {
                number3 = 0;
            }
            else if (number3 > 255)
            {
                number3 = 255;
            }

            pocetOP.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            pocetopak = string.Format("{0:X2}", number3);
        }

        private void constSpeedSet_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox constSpeedSet = sender as TextBox;
            int number3;

            if (string.IsNullOrEmpty(constSpeedSet.Text))
            {
                constSpeedSet.Text = "100";
            }

            if (!int.TryParse(constSpeedSet.Text, out number3))
            {
                number3 = 1;
            }
            else if (number3 < 1)
            {
                number3 = 1;
            }
            else if (number3 > 5745)
            {
                number3 = 5745;
            }

            constSpeedSet.Text = number3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(number3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            speedSetVar = string.Format("{0:X4}", number3);
        }
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string jobHorizontalDirection = jobHorizontalDirectionComboBox.SelectedItem.ToString();
            string charactersHorizontalDirection = charactersHorizontalDirectionComboBox.SelectedItem.ToString();
            string charactersVerticalDirection = charactersVerticalDirectionComboBox.SelectedItem.ToString();
            string tachoMode = tachoModeComboBox.SelectedItem.ToString();
            string printingMode = printingModeComboBox.SelectedItem.ToString();
            string unitType = unitTypeComboBox.SelectedItem.ToString();

            // Konvertovanie  zvolených parametrov na binárne hodnoty
            int jobHorizontalDirectionValue = jobHorizontalDirection == "Normálny" ? 0 : 1;
            int charactersHorizontalDirectionValue = charactersHorizontalDirection == "Normálny" ? 0 : 1;
            int charactersVerticalDirectionValue = charactersVerticalDirection == "Normálny" ? 0 : 1;
            int tachoModeValue = tachoMode == "Konštantná rýchlosť" ? 0 : 1;
            int res1 = 0;
            int printingModeValue = printingMode == "Objekt" ? 0 : 1;
            int unitTypeValue = unitType == "mm" ? 0 : 1;
            int res2 = 0;

            // Konverzia binárnych hodnôt na int
            int binaryRepresentation = jobHorizontalDirectionValue << 7 | charactersHorizontalDirectionValue << 6 | charactersVerticalDirectionValue << 5 | tachoModeValue << 4 | res1 << 3 | printingModeValue << 2 | unitTypeValue << 1 | res2;
            // Konverzia int na hexadecimálnu hodnotu
            string hexJobparam = binaryRepresentation.ToString("X2");

            //odstránenie diakritiky
            string textWithDiacritics = txtResult.Text;
            string textWithoutDiacritics = RemoveDiacritics(textWithDiacritics);
            

            //vstupný reťazec
            if (string.IsNullOrEmpty(textWithoutDiacritics))
            {
                input = "   ";
            }
            else
            {
                input = textWithoutDiacritics;
            }

            //ip a port aj s konverziou
            ip = Properties.Settings.Default.IPPADDRS;
            port = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
            byte[] data = Encoding.UTF8.GetBytes(input);
            // Konverzia byte array na hexadecimalne hodnoty
            string hex = BitConverter.ToString(data).Replace("-", string.Empty);

            //staticke data
            const string typ = "10";
            const string dlzka = "0012";
            //velkost pisma
            string fontsize = fontSize;

            const string algonum = "0000";
            const string yref = "0001";
            const string reserved = "00";
            // zvyraznenie
            string expansion = expan;

            const string generic = "00000000";
            const string endjob = "0D";
            //delimiter line
            const string delimit1 = "0A";
            //parametere riadku 
            const string lineparamlen = "0004";
            const string linecount = "01";
            const string lineparam = "09";
            //nastavenie opakovania, medzier z predu-zo zadu, dtop, delenie tacha
            string medzeraza = medzza;
            string medzerapred = medzpred;
            string delenietacha = tacho;
            const string dtopfilter = "0a";
            string pocetopakovani = pocetopak;
            //konstatna rzchlost mm/s
            string speed = speedSetVar;
            // opakovanie v mm
            string distance = intervalOp;
            // sucet parametrov spravy
            string sumjobparameters = hexJobparam;
            // MessageBox.Show(" (Binary representation: " + binaryRepresentation + ", Hexadecimal representation: " + sumjobparameters + ")");
            //dlzka parametrov
            const string jobparamlen = "0012";
            const string jobparamnum = "00";
            const string jobparamtype = "01";
            const string numparam = "0004";

            // identifikátor (ef znamená poslanie kompletnej spravy do buffra
            const string efHex = "ef";

            hex = hex + typ;
            hex = hex + dlzka;
            hex = hex + fontsize;
            hex = hex + algonum;
            hex = hex + yref;
            hex = hex + reserved;
            hex = hex + expansion;
            hex = hex + generic;
            hex = hex + dlzka;
            hex = hex + typ;
            hex = hex + endjob;



            hex = typ + hex;
            hex = dlzka + hex;
            hex = generic + hex;
            hex = expansion + hex;
            hex = reserved + hex;
            hex = yref + hex;
            hex = algonum + hex;
            hex = fontsize + hex;
            hex = dlzka + hex;
            hex = typ + hex;
            hex = delimit1 + hex;

            //neidentifikované
            const string nonidentity = "0000060005";
            hex = nonidentity + hex;

            hex = delimit1 + hex;

            hex = lineparamlen + hex;
            hex = linecount + hex;
            hex = lineparam + hex;


            //line y koordinaty
            const string coordinatesy = "080000080000001F";
            hex = coordinatesy + hex;

            hex = algonum + hex;

            // parametre

            hex = speed + hex;
            hex = distance + hex;
            hex = medzeraza + hex;
            hex = medzerapred + hex;
            hex = delenietacha + hex;
            hex = dtopfilter + hex;
            hex = pocetopakovani + hex;
            hex = sumjobparameters + hex;
            hex = jobparamlen + hex;
            hex = jobparamnum + hex;
            hex = jobparamtype + hex;
            hex = numparam + hex;



            //dlzka celková
            int byteCount = hex.Length / 2;
            hex = byteCount.ToString("X4") + hex;



            hex = efHex + hex;

            // konverzia hex stringu na bytové pole
            byte[] hexData = new byte[hex.Length / 2];
            for (int i = 0; i < hexData.Length; i++)
            {
                hexData[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            
            //výpočet kontrolného súčtu
            static byte CalculateChecksum(byte[] data)
            {
                byte result = 0;
                foreach (byte b in data)
                {
                    result ^= b;
                }
                return result;
            }

            byte checksum = CalculateChecksum(hexData);

            hex = hex + checksum.ToString("X2");
           

            SendData(hex, ip, port);
        }
        private void SendData(string hex, string ip, int port)
        {
            // konverzia hex stringu na bytové pole
            byte[] hexstream = new byte[hex.Length / 2];
            for (int i = 0; i < hexstream.Length; i++)
            {
                hexstream[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            try
            {
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        // odoslanie bytového poľa dát na zariadenie
                        stream.Write(hexstream, 0, hexstream.Length);
                        byte[] response = new byte[5];

                        // vytvorenie asynchrónnej úlohy na čítanie odpovede od zariadenia
                        Task<int> task = stream.ReadAsync(response, 0, response.Length);

                        // čakanie na odpoveď od zariadenia s časovým limitom 2 sekundy
                        if (task.Wait(2000))
                        {
                            // ak príde odpoveď v časovom limite, skontrolujte, či je odpoveď 06h
                            if (response[0] == 0x06)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    if (_previousSelectedRow != null)
                                    {
                                        _previousSelectedRow.Foreground = Brushes.GreenYellow;
                                    }
                                });

                                stream.Close();
                                client.Close();
                                MessageBox.Show("Dáta boli úspešne odoslané.");
                            }
                        }
                        else
                        {
                            stream.Close();
                            client.Close();
                            // ak sa nevráti odpoveď v časovom limite, zobrazte chybovú hlášku
                            MessageBox.Show("Časový limit vypršal. Nepodarilo sa prijatie odpovede od zariadenia.");
                        }
                        stream.Close();
                        client.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala chyba. Je tlačiareň on-line?.\n" + ex.Message);
                
            }
        }

        private void sendPrintSignalbtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (TcpClient printiclient = new TcpClient(ipAddress, ipport))
                {
                    using (NetworkStream printistream = printiclient.GetStream())
                    {
                        byte[] requestDatafirst = new byte[] { 0xB1, 0x00, 0x01, 0X01, 0xB1 };
                       printistream.Write(requestDatafirst, 0, requestDatafirst.Length);

                        byte[] response1 = new byte[1];
                        printistream.Read(response1, 0, response1.Length);
                        if (response1[0] != 0x06)
                        {
                            printiclient.Close();
                            throw new Exception("Neočakávaná odpoveď");
                        }
                        byte[] response2 = new byte[86];
                        printistream.Read(response2, 0, response2.Length);
                       printistream.Write(new byte[] { 0x06 }, 0, 1);

                        byte[] requestDatasecond = new byte[] { 0x34, 0x00, 0x00, 0x34 };
                        printistream.Write(requestDatasecond, 0, requestDatasecond.Length);

                        byte[] response3 = new byte[1];
                        printistream.Read(response3, 0, response3.Length);
                        if (response3[0] != 0x06)
                        {
                            printiclient.Close();
                            throw new Exception("Neočakávaná odpoveď");
                        }
                        byte[] response4 = new byte[29];
                        printistream.Read(response4, 0, response4.Length);
                        printistream.Write(new byte[] { 0x06 }, 0, 1);

                        byte[] requestDatathird = new byte[] { 0xE6, 0x00, 0x00, 0xE6 };
                        printistream.Write(requestDatathird, 0, requestDatathird.Length);

                        byte[] response5 = new byte[1];
                        printistream.Read(response5, 0, response5.Length);
                        if (response5[0] != 0x06)
                        {
                            printiclient.Close();
                            throw new Exception("Neočakávaná odpoveď");
                        }
                        
                        // Odoslanie požiadavky 0xA6 0x00 0X00 0XA6
                        byte[] requestData = new byte[] { 0xA6, 0x00, 0x00, 0xA6 };
                        printistream.Write(requestData, 0, requestData.Length);

                        // Očakávanie odpovede 0x06
                        byte[] response11 = new byte[1];
                        printistream.Read(response11, 0, response11.Length);
                        if (response11[0] != 0x06)
                        {
                            printiclient.Close();
                            throw new Exception("Neočakávaná odpoveď");
                        }

                        // Prijatie dát podľa modelu E4h Identification
                        byte[] response12 = new byte[19];
                        printistream.Read(response12, 0, response12.Length);

                        // Odoslanie potvrdenia o prijatí 0x06
                        printistream.Write(new byte[] { 0x06 }, 0, 1); 

                        // Odoslanie požiadavky 0x32, 0x00, 0x01, 0x01, 0x32
                        byte[] printiData = new byte[] { 0x32, 0x00, 0x01, 0x01, 0x32 };
                        printistream.Write(printiData, 0, printiData.Length);

                        // Očakávanie odpovede 0x06 0x00 0x01 0xYY 0xYY
                        byte[] response = new byte[6];
                        printistream.Read(response, 0, response.Length);
                        printistream.Write(new byte[] { 0x06 }, 0, 1);
                        if (response[0] != 0x06)
                        {
                            printiclient.Close();
                            throw new Exception("Neočakávaná odpoveď");
                        }
                        if (response[1] == 0x32 && response[2] == 0x00 && response[3] == 0x01 && response[4] == 0x02 && response[5] == 0x31)
                        {
                            printiclient.Close();
                            MessageBox.Show("Tlačiareň vypnutá");
                        }
                        else if (response[1] == 0x32 && response[2] == 0x00 && response[3] == 0x01 && response[4] == 0x00 && response[5] == 0x33)
                        {
                            printiclient.Close();
                            MessageBox.Show("Tlač pozastavená");
                        }
                        else if (response[1] == 0x32 && response[2] == 0x00 && response[3] == 0x01 && response[4] == 0x01 && response[5] == 0x32)
                        {
                            printistream.Write(new byte[] { 0x94, 0x00, 0x00, 0x94 }, 0, 4);
                            printiclient.Close();
                            MessageBox.Show("Príkaz odoslaný");
                        }
                    }
                    printiclient.Close();
                    
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show("Nastala chyba. Je tlačiareň on-line?\n" + ex.Message);
                
            }
            
        }

        private void FillControlsWithProfileDatadatapro(Profiledatapro profiledatapro)
        {
            comboBox1.SelectedIndex = profiledatapro.FontSizeProdatapro;
            zvyrazCombo.SelectedIndex = profiledatapro.ExpanseProdatapro;
            tachoModeComboBox.SelectedIndex = profiledatapro.SpeedmodeProdatapro;
            printingModeComboBox.SelectedIndex = profiledatapro.PrintmodeProdatapro;
            unitTypeComboBox.SelectedIndex = profiledatapro.UnitsProdatapro;
            charactersVerticalDirectionComboBox.SelectedIndex = profiledatapro.CharVdirectProdatapro;
            charactersHorizontalDirectionComboBox.SelectedIndex = profiledatapro.CharHdirectProdatapro;
            jobHorizontalDirectionComboBox.SelectedIndex = profiledatapro.JobdirectionProdatapro;

            delenietacha.Text = profiledatapro.DelenietachaProdatapro.ToString();
            ValidateAndConvertdelenietacha(delenietacha);
            constSpeedSet.Text = profiledatapro.PruductSpeedProdatapro.ToString();
            ValidateAndConvertConstSpeedSet(constSpeedSet);
            pocetOP.Text = profiledatapro.PoceopProdatapro.ToString();
            ValidateAndConvertpocetOP(pocetOP);
            medzerapred.Text = profiledatapro.MedzerapredProdatapro.ToString();
            ValidateAndConvertmedzerapred(medzerapred);

            medzeraza.Text = profiledatapro.MedzerazaProdatapro.ToString();
            ValidateAndConvertmedzeraza(medzeraza);

            intarvalOP.Text = profiledatapro.IntervapopProdatapro.ToString();
            ValidateAndConvertIntarvalOP(intarvalOP);
        }
        private void ProfileSetCombodatapro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedProfiledataproIndex = ProfileSetCombodatapro.SelectedIndex;
            if (selectedProfiledataproIndex >= 0)
            {
                Profiledatapro selectedProfiledatapro = _profileManagerdatapro.Profilesdatapro[selectedProfiledataproIndex];
                FillControlsWithProfileDatadatapro(selectedProfiledatapro);
            }
        }

        private void btnSaveProdatapro_Click(object sender, RoutedEventArgs e)
        {

            string profileNamedatapro = saveProfiledatapro.Text;
            int selectedProfileIndex = saveProfiledatapro.SelectedIndex;
            // Update existing profile
            Profiledatapro selectedProfiledatapro = _profileManagerdatapro.Profilesdatapro[selectedProfileIndex];
            selectedProfiledatapro.Namedatapro = profileNamedatapro;
          //  MessageBox.Show("Profile name: " + selectedProfiledatapro.Namedatapro);
            selectedProfiledatapro.FontSizeProdatapro = comboBox1.SelectedIndex;
           // MessageBox.Show("Profile font size: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.MedzerapredProdatapro = zvyrazCombo.SelectedIndex;
           // MessageBox.Show("Profile margin before: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.SpeedmodeProdatapro = tachoModeComboBox.SelectedIndex;
           // MessageBox.Show("Profile speed mode: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.PrintmodeProdatapro = printingModeComboBox.SelectedIndex;
           // MessageBox.Show("Profile print mode: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.UnitsProdatapro = unitTypeComboBox.SelectedIndex;
           // MessageBox.Show("Profile units: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.CharVdirectProdatapro = charactersVerticalDirectionComboBox.SelectedIndex;
           // MessageBox.Show("Profile vertical character direction: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.CharHdirectProdatapro = charactersHorizontalDirectionComboBox.SelectedIndex;
           // MessageBox.Show("Profile horizontal character direction: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.JobdirectionProdatapro = jobHorizontalDirectionComboBox.SelectedIndex;
          //  MessageBox.Show("Profile job direction: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.DelenietachaProdatapro = Convert.ToInt32(delenietacha.Text);
          //  MessageBox.Show("Profile delimiter: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.PruductSpeedProdatapro = Convert.ToInt32(constSpeedSet.Text);
          //  MessageBox.Show("Profile product speed: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.MedzerapredProdatapro = Convert.ToInt32(medzerapred.Text);
          //  MessageBox.Show("Profile margin before: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.MedzerazaProdatapro = Convert.ToInt32(medzeraza.Text);
           // MessageBox.Show("Profile margin after: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.PoceopProdatapro = Convert.ToInt32(pocetOP.Text);
           // MessageBox.Show("Profile number of operations: " + selectedProfiledatapro.FontSizeProdatapro);
            selectedProfiledatapro.IntervapopProdatapro = Convert.ToInt32(intarvalOP.Text);
           // MessageBox.Show("Profile interval of operations: " + selectedProfiledatapro.FontSizeProdatapro);

            _profileManagerdatapro.SaveProfilesdatapro();

            MessageBox.Show("Dáta boli uložené do: " + profileNamedatapro);

        }

        private void saveProfiledatapro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int selectedIndex = saveProfiledatapro.SelectedIndex;
            if (selectedIndex == 0)
            {
                btnSaveProdatapro.IsEnabled = false;
            }
            else
            {
                btnSaveProdatapro.IsEnabled = true;
            }

        }
        private void ValidateAndConvertConstSpeedSet(TextBox constSpeedSet)
        {
            int adtnumber3;
            if (string.IsNullOrEmpty(constSpeedSet.Text))
            {
                constSpeedSet.Text = "100";
            }

            if (!int.TryParse(constSpeedSet.Text, out adtnumber3))
            {
                adtnumber3 = 2;
            }
            else if (adtnumber3 < 2)
            {
                adtnumber3 = 2;
            }
            else if (adtnumber3 > 5745)
            {
                adtnumber3 = 5745;
            }

            constSpeedSet.Text = adtnumber3.ToString();

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            speedSetVar = string.Format("{0:X4}", adtnumber3);
        }

        private void ValidateAndConvertpocetOP(TextBox pocetOP)
        {
            int adtnumber3;

            if (string.IsNullOrEmpty(pocetOP.Text))
            {
                pocetOP.Text = "0";
            }

            if (!int.TryParse(pocetOP.Text, out adtnumber3))
            {
                adtnumber3 = 0;
            }
            else if (adtnumber3 < 0)
            {
                adtnumber3 = 0;
            }
            else if (adtnumber3 > 255)
            {
                adtnumber3 = 255;
            }

            pocetOP.Text = adtnumber3.ToString();

            byte[] bytescon3 = BitConverter.GetBytes(adtnumber3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            pocetopak = string.Format("{0:X2}", adtnumber3);
        }
        private void ValidateAndConvertdelenietacha(TextBox delenietacha)
        {

            int adtnumber3;

            if (string.IsNullOrEmpty(delenietacha.Text))
            {
                delenietacha.Text = "7";
            }

            if (!int.TryParse(delenietacha.Text, out adtnumber3))
            {
                adtnumber3 = 1;
            }
            else if (adtnumber3 < 1)
            {
                adtnumber3 = 1;
            }
            else if (adtnumber3 > 63)
            {
                adtnumber3 = 63;
            }

            delenietacha.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            tacho = string.Format("{0:X2}", adtnumber3);
        }
        private void ValidateAndConvertmedzeraza(TextBox medzeraza)
        {

            int adtnumber3;

            if (string.IsNullOrEmpty(medzeraza.Text))
            {
                medzeraza.Text = "3";
            }

            if (!int.TryParse(medzeraza.Text, out adtnumber3))
            {
                adtnumber3 = 3;
            }
            else if (adtnumber3 < 3)
            {
                adtnumber3 = 3;
            }
            else if (adtnumber3 > 6363)
            {
                adtnumber3 = 6363;
            }

            medzeraza.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            medzza = string.Format("{0:X4}", adtnumber3);
        }
        private void ValidateAndConvertmedzerapred(TextBox medzerapred)
        {
            int adtnumber3;

            if (string.IsNullOrEmpty(medzerapred.Text))
            {
                medzerapred.Text = "3";
            }

            if (!int.TryParse(medzerapred.Text, out adtnumber3))
            {
                adtnumber3 = 3;
            }
            else if (adtnumber3 < 3)
            {
                adtnumber3 = 3;
            }
            else if (adtnumber3 > 6363)
            {
                adtnumber3 = 6363;
            }

            medzerapred.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            medzpred = string.Format("{0:X4}", adtnumber3);
        }
        private void ValidateAndConvertIntarvalOP(TextBox intarvalOP)
        {

            int adtnumber3;

            if (string.IsNullOrEmpty(intarvalOP.Text))
            {
                intarvalOP.Text = "2";
            }

            if (!int.TryParse(intarvalOP.Text, out adtnumber3))
            {
                adtnumber3 = 2;
            }
            else if (adtnumber3 < 2)
            {
                adtnumber3 = 2;
            }
            else if (adtnumber3 > 6363)
            {
                adtnumber3 = 6363;
            }

            intarvalOP.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            intervalOp = string.Format("{0:X4}", adtnumber3);
        }

        public static string RemoveDiacritics(string text)
        {
            string normalizedText = text.Normalize(NormalizationForm.FormKD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalizedText)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        private void sendPrintSignalbtn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}