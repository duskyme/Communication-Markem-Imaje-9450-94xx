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
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
//using Wpf.Ui.Controls;
//using Wpf.Ui.Common;



namespace DataBasePrint.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataPage : INavigableView<ViewModels.DataViewModel>
    {
        public static string adtintervalOp;
        public static string adtRowCount;
        public static string adtFilePath2;
        public static int adtSheetIndex;
        public static string adtfontSize;
        public static string adtmedzpred;
        public static string adtmedzza;
        public string adtexpan;
        public static string adtIpAddress;
        public static int adtIpport;
        public static string adtip;
        public static int adtport;
        public string adtspeedSetVar;
        public static string adttacho;
        public string adtpocetopak;
        string adtinput;
        public ViewModels.DataViewModel ViewModel

        {
            get;

        }
        private readonly ProfileManager _profileManager = new ProfileManager(@"profiles.xml");
        public DataPage(ViewModels.DataViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            adtIpAddress = Properties.Settings.Default.IPPADDRS;
            adtIpport = Convert.ToInt32(Properties.Settings.Default.IPPPORT);


            _profileManager.LoadProfiles();
            foreach (Profile profile in _profileManager.Profiles)
            {
                ProfileSetCombo.Items.Add(profile.Name);
            }
            ProfileSetCombo.SelectedIndex = 0;
            _profileManager.LoadProfiles();
            foreach (Profile profile in _profileManager.Profiles)
            {
                saveProfile.Items.Add(profile.Name);
            }



            

            // dáta pre combobox veľkosť písma
            int[] items = new int[] { 5, 7, 9, 11, 16, 24 };
            foreach (int item in items)
            {
                adtcomboBox1.Items.Add(item);
            }
          //  adtcomboBox1.SelectedIndex = 0;

            // dáta pre combobox zvýraznenie
            int[] zvyraz = new int[] { 1, 2, 3, 4 };
            foreach (int item in zvyraz)
            {
                adtzvyrazCombo.Items.Add(item);
            }
           // adtzvyrazCombo.SelectedIndex = 0;
            //comboboxy parametrov
            adtjobHorizontalDirectionComboBox.Items.Add("Normálny");
            adtjobHorizontalDirectionComboBox.Items.Add("Invertovať");
           // adtjobHorizontalDirectionComboBox.SelectedIndex = 0;

            adtcharactersHorizontalDirectionComboBox.Items.Add("Normálny");
            adtcharactersHorizontalDirectionComboBox.Items.Add("Invertovať");
           // adtcharactersHorizontalDirectionComboBox.SelectedIndex = 0;

            adtcharactersVerticalDirectionComboBox.Items.Add("Normálny");
            adtcharactersVerticalDirectionComboBox.Items.Add("Invertovať");
           // adtcharactersVerticalDirectionComboBox.SelectedIndex = 0;

            adttachoModeComboBox.Items.Add("Konštantná rýchlosť");
            adttachoModeComboBox.Items.Add("Tacho");
           // adttachoModeComboBox.SelectedIndex = 0;

            adtprintingModeComboBox.Items.Add("Objekt");
            adtprintingModeComboBox.Items.Add("Opakovanie");
          //  adtprintingModeComboBox.SelectedIndex = 0;

            adtunitTypeComboBox.Items.Add("mm");
            adtunitTypeComboBox.Items.Add("H-frame");
           // adtunitTypeComboBox.SelectedIndex = 0;
        }

        private void adtComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            {
                switch (adtcomboBox1.SelectedItem)
                {
                    case 5:
                        adtfontSize = "011A";
                        break;
                    case 7:
                        adtfontSize = "011B";
                        break;
                    case 9:
                        adtfontSize = "011C";
                        break;
                    case 11:
                        adtfontSize = "011D";
                        break;
                    case 16:
                        adtfontSize = "011E";
                        break;
                    case 24:
                        adtfontSize = "011F";
                        break;
                }
            }
        }

        private void adtzvyrazCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                int selectedValue = Convert.ToInt32(adtzvyrazCombo.SelectedItem);
                switch (selectedValue)
                {
                    case 1:
                        adtexpan = "01";
                        break;
                    case 2:
                        adtexpan = "02";
                        break;
                    case 3:
                        adtexpan = "03";
                        break;
                    case 4:
                        adtexpan = "04";
                        break;
                }
            }
        }

        private void adttachoModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (adttachoModeComboBox.SelectedItem.ToString() == "Tacho")
            {
                adtconstSpeedSet.IsEnabled = false;
                adtdelenietacha.IsEnabled = true;
            }
            else if (adttachoModeComboBox.SelectedItem.ToString() == "Konštantná rýchlosť")
            {
                adtconstSpeedSet.IsEnabled = true;
                adtdelenietacha.IsEnabled = false;
            }
        }

        private void adtprintingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (adtprintingModeComboBox.SelectedItem.ToString() == "Objekt")
            {
                adtintarvalOP.IsEnabled = false;
                adtpocetOP.IsEnabled = false;
            }
            else if (adtprintingModeComboBox.SelectedItem.ToString() == "Opakovanie")
            {
                adtintarvalOP.IsEnabled = true;
                adtpocetOP.IsEnabled = true;
            }
        }

        private void adtIntarvalOP_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtintarvalOP = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtintarvalOP.Text))
            {
                adtintarvalOP.Text = "2";
            }

            if (!int.TryParse(adtintarvalOP.Text, out adtnumber3))
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

            adtintarvalOP.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtintervalOp = string.Format("{0:X4}", adtnumber3);
        }
        private void adtmedzerapred_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtmedzerapred = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtmedzerapred.Text))
            {
                adtmedzerapred.Text = "3";
            }

            if (!int.TryParse(adtmedzerapred.Text, out adtnumber3))
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

            adtmedzerapred.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtmedzpred = string.Format("{0:X4}", adtnumber3);
        }
        private void adtmedzeraza_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtmedzeraza = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtmedzeraza.Text))
            {
                adtmedzeraza.Text = "3";
            }

            if (!int.TryParse(adtmedzeraza.Text, out adtnumber3))
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

            adtmedzeraza.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtmedzza = string.Format("{0:X4}", adtnumber3);
        }
        private void adtdelenietacha_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtdelenietacha = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtdelenietacha.Text))
            {
                adtdelenietacha.Text = "7";
            }

            if (!int.TryParse(adtdelenietacha.Text, out adtnumber3))
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

            adtdelenietacha.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adttacho = string.Format("{0:X2}", adtnumber3);
        }

        private void adtpocetOP_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtpocetOP = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtpocetOP.Text))
            {
                adtpocetOP.Text = "0";
            }

            if (!int.TryParse(adtpocetOP.Text, out adtnumber3))
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

            adtpocetOP.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] bytescon3 = BitConverter.GetBytes(adtnumber3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            adtpocetopak = string.Format("{0:X2}", adtnumber3);
        }

        private void adtconstSpeedSet_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox adtconstSpeedSet = sender as TextBox;
            int adtnumber3;

            if (string.IsNullOrEmpty(adtconstSpeedSet.Text))
            {
                adtconstSpeedSet.Text = "100";
            }

            if (!int.TryParse(adtconstSpeedSet.Text, out adtnumber3))
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

            adtconstSpeedSet.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtspeedSetVar = string.Format("{0:X4}", adtnumber3);
        }
        private void adtsendBtn_Click(object sender, RoutedEventArgs e)
        {
            string adtjobHorizontalDirection = adtjobHorizontalDirectionComboBox.SelectedItem.ToString();
            string adtcharactersHorizontalDirection = adtcharactersHorizontalDirectionComboBox.SelectedItem.ToString();
            string adtcharactersVerticalDirection = adtcharactersVerticalDirectionComboBox.SelectedItem.ToString();
            string adttachoMode = adttachoModeComboBox.SelectedItem.ToString();
            string adtprintingMode = adtprintingModeComboBox.SelectedItem.ToString();
            string adtunitType = adtunitTypeComboBox.SelectedItem.ToString();

            // Konvertovanie  zvolených parametrov na binárne hodnoty
            int adtjobHorizontalDirectionValue = adtjobHorizontalDirection == "Normálny" ? 0 : 1;
            int adtcharactersHorizontalDirectionValue = adtcharactersHorizontalDirection == "Normálny" ? 0 : 1;
            int adtcharactersVerticalDirectionValue = adtcharactersVerticalDirection == "Normálny" ? 0 : 1;
            int adttachoModeValue = adttachoMode == "Konštantná rýchlosť" ? 0 : 1;
            int adtres1 = 0;
            int adtprintingModeValue = adtprintingMode == "Objekt" ? 0 : 1;
            int adtunitTypeValue = adtunitType == "mm" ? 0 : 1;
            int adtres2 = 0;

            // Konverzia binárnych hodnôt na int
            int adtbinaryRepresentation = adtjobHorizontalDirectionValue << 7 | adtcharactersHorizontalDirectionValue << 6 | adtcharactersVerticalDirectionValue << 5 | adttachoModeValue << 4 | adtres1 << 3 | adtprintingModeValue << 2 | adtunitTypeValue << 1 | adtres2;
            // Konverzia int na hexadecimálnu hodnotu
            string adthexJobparam = adtbinaryRepresentation.ToString("X2");

            //vstupný reťazec
           // string adtinput = adtinputBox.Text;
            if (string.IsNullOrEmpty(adtinputBox.Text))
            {
                adtinput = "   ";
            }
            else
            {
                adtinput = adtinputBox.Text;
            }
            //ip a port aj s konverziou
            adtip = Properties.Settings.Default.IPPADDRS;
            adtport = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
            byte[] adtdata = Encoding.UTF8.GetBytes(adtinput);
            // Konverzia byte array na hexadecimalne hodnoty
            string adthex = BitConverter.ToString(adtdata).Replace("-", string.Empty);

            //staticke data
            const string adttyp = "10";
            const string adtdlzka = "0012";
            //velkost pisma
            string adtfontsize = adtfontSize;

            const string adtalgonum = "0000";
            const string adtyref = "0001";
            const string adtreserved = "00";
            // zvyraznenie
            string adtexpansion = adtexpan;

            const string adtgeneric = "00000000";
            const string adtendjob = "0D";
            //delimiter line
            const string adtdelimit1 = "0A";
            //parametere riadku 
            const string adtlineparamlen = "0004";
            const string adtlinecount = "01";
            const string adtlineparam = "09";
            //nastavenie opakovania, medzier z predu-zo zadu, dtop, delenie tacha
            string adtmedzeraza = adtmedzza;
            string adtmedzerapred = adtmedzpred;
            string adtdelenietacha = adttacho;
            const string adtdtopfilter = "0a";
            string adtpocetopakovani = adtpocetopak;
            //konstatna rzchlost mm/s
            string adtspeed = adtspeedSetVar;
            // opakovanie v mm
            string adtdistance = adtintervalOp;
            // sucet parametrov spravy
            string adtsumjobparameters = adthexJobparam;
            // MessageBox.Show(" (Binary representation: " + binaryRepresentation + ", Hexadecimal representation: " + sumjobparameters + ")");
            //dlzka parametrov
            const string adtjobparamlen = "0012";
            const string adtjobparamnum = "00";
            const string adtjobparamtype = "01";
            const string adtnumparam = "0004";

            // identifikátor (ef znamená poslanie kompletnej spravy do buffra
            const string adtefHex = "ef";

            adthex = adthex + adttyp;
            adthex = adthex + adtdlzka;
            adthex = adthex + adtfontsize;
            adthex = adthex + adtalgonum;
            adthex = adthex + adtyref;
            adthex = adthex + adtreserved;
            adthex = adthex + adtexpansion;
            adthex = adthex + adtgeneric;
            adthex = adthex + adtdlzka;
            adthex = adthex + adttyp;
            adthex = adthex + adtendjob;



            adthex = adttyp + adthex;
            adthex = adtdlzka + adthex;
            adthex = adtgeneric + adthex;
            adthex = adtexpansion + adthex;
            adthex = adtreserved + adthex;
            adthex = adtyref + adthex;
            adthex = adtalgonum + adthex;
            adthex = adtfontsize + adthex;
            adthex = adtdlzka + adthex;
            adthex = adttyp + adthex;
            adthex = adtdelimit1 + adthex;

            //neidentifikované
            const string adtnonidentity = "0000060005";
            adthex = adtnonidentity + adthex;

            adthex = adtdelimit1 + adthex;

            adthex = adtlineparamlen + adthex;
            adthex = adtlinecount + adthex;
            adthex = adtlineparam + adthex;


            //line y koordinaty
            const string adtcoordinatesy = "080000080000001F";
            adthex = adtcoordinatesy + adthex;

            adthex = adtalgonum + adthex;

            // parametre

            adthex = adtspeed + adthex;
            adthex = adtdistance + adthex;
            adthex = adtmedzeraza + adthex;
            adthex = adtmedzerapred + adthex;
            adthex = adtdelenietacha + adthex;
            adthex = adtdtopfilter + adthex;
            adthex = adtpocetopakovani + adthex;
            adthex = adtsumjobparameters + adthex;
            adthex = adtjobparamlen + adthex;
            adthex = adtjobparamnum + adthex;
            adthex = adtjobparamtype + adthex;
            adthex = adtnumparam + adthex;



            //dlzka celková
            int adtbyteCount = adthex.Length / 2;
            adthex = adtbyteCount.ToString("X4") + adthex;



            adthex = adtefHex + adthex;

            // konverzia hex stringu na bytové pole
            byte[] adthexData = new byte[adthex.Length / 2];
            for (int i = 0; i < adthexData.Length; i++)
            {
                adthexData[i] = Convert.ToByte(adthex.Substring(i * 2, 2), 16);
            }

            //výpočet kontrolného súčtu
            static byte adtCalculateChecksum(byte[] adtdata)
            {
                byte adtresult = 0;
                foreach (byte adtb in adtdata)
                {
                    adtresult ^= adtb;
                }
                return adtresult;
            }

            byte adtchecksum = adtCalculateChecksum(adthexData);

            adthex = adthex + adtchecksum.ToString("X2");


            adtSendData(adthex, adtip, adtport);
        }

        private void adtSendData(string adthex, string adtip, int adtport)
        {
            // konverzia hex stringu na bytové pole
            byte[] adthexstream = new byte[adthex.Length / 2];
            for (int i = 0; i < adthexstream.Length; i++)
            {
                adthexstream[i] = Convert.ToByte(adthex.Substring(i * 2, 2), 16);
            }
            try
            {
                using (TcpClient adtclient = new TcpClient(adtip, adtport))
                {
                    using (NetworkStream adtstream = adtclient.GetStream())
                    {
                        // odoslanie bytového poľa dát na zariadenie
                        adtstream.Write(adthexstream, 0, adthexstream.Length);
                        byte[] response = new byte[5];

                        // vytvorenie asynchrónnej úlohy na čítanie odpovede od zariadenia
                        Task<int> task = adtstream.ReadAsync(response, 0, response.Length);

                        // čakanie na odpoveď od zariadenia s časovým limitom 2 sekundy
                        if (task.Wait(2000))
                        {
                            // ak príde odpoveď v časovom limite, skontrolujte, či je odpoveď 06h
                            if (response[0] == 0x06)
                            {
                                adtstream.Close();
                                adtclient.Close();
                                MessageBox.Show("Dáta boli úspešne odoslané.");
                            }
                        }
                        else
                        {
                            adtstream.Close();
                            adtclient.Close();
                            // ak sa nevráti odpoveď v časovom limite, zobrazte chybovú hlášku
                            MessageBox.Show("Časový limit vypršal. Nepodarilo sa prijatie odpovede od zariadenia.");
                        }
                        adtstream.Close();
                        adtclient.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala chyba. Je tlačiareň on-line?.\n" + ex.Message);
            }
        }
        private void adtsendPrintSignalbtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (TcpClient printiclient = new TcpClient(adtIpAddress, adtIpport))
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
        private void FillControlsWithProfileData(Profile profile)
        {
            adtcomboBox1.SelectedIndex = profile.FontSizePro;
            adtzvyrazCombo.SelectedIndex = profile.ExpansePro;
            adttachoModeComboBox.SelectedIndex = profile.SpeedmodePro;
            adtprintingModeComboBox.SelectedIndex = profile.PrintmodePro;
            adtunitTypeComboBox.SelectedIndex = profile.UnitsPro;
            adtcharactersVerticalDirectionComboBox.SelectedIndex = profile.CharVdirectPro;
            adtcharactersHorizontalDirectionComboBox.SelectedIndex = profile.CharHdirectPro;
            adtjobHorizontalDirectionComboBox.SelectedIndex = profile.JobdirectionPro;

            adtdelenietacha.Text = profile.DelenietachaPro.ToString();
            ValidateAndConvertAdtdelenietacha(adtdelenietacha);
            adtconstSpeedSet.Text = profile.PruductSpeedPro.ToString();
            ValidateAndConvertAdtConstSpeedSet(adtconstSpeedSet);
            adtpocetOP.Text = profile.PoceopPro.ToString();
            ValidateAndConvertAdtpocetOP(adtpocetOP);
            adtmedzerapred.Text = profile.MedzerapredPro.ToString();
            ValidateAndConvertAdtmedzerapred(adtmedzerapred);

            adtmedzeraza.Text = profile.MedzerazaPro.ToString();
            ValidateAndConvertAdtmedzeraza(adtmedzeraza);

            adtintarvalOP.Text = profile.IntervapopPro.ToString();
            ValidateAndConvertAdtIntarvalOP(adtintarvalOP);
        }
        private void ProfileSetCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedProfileIndex = ProfileSetCombo.SelectedIndex;
            if (selectedProfileIndex >= 0)
            {
                Profile selectedProfile = _profileManager.Profiles[selectedProfileIndex];
                FillControlsWithProfileData(selectedProfile);
            }
        }
        

        private void btnSavePro_Click(object sender, RoutedEventArgs e)
        {

            string profileName = saveProfile.Text;
            int selectedProfileIndex = saveProfile.SelectedIndex;
            // Update existing profile
            Profile selectedProfile = _profileManager.Profiles[selectedProfileIndex];
            selectedProfile.Name = profileName;
       //     MessageBox.Show("Profile name: " + selectedProfile.Name);
            selectedProfile.FontSizePro = adtcomboBox1.SelectedIndex;
       //     MessageBox.Show("Profile font size: " + selectedProfile.FontSizePro);
            selectedProfile.MedzerapredPro = adtzvyrazCombo.SelectedIndex;
       //     MessageBox.Show("Profile margin before: " + selectedProfile.MedzerapredPro);
            selectedProfile.SpeedmodePro = adttachoModeComboBox.SelectedIndex;
       //     MessageBox.Show("Profile speed mode: " + selectedProfile.SpeedmodePro);
            selectedProfile.PrintmodePro = adtprintingModeComboBox.SelectedIndex;
       //     MessageBox.Show("Profile print mode: " + selectedProfile.PrintmodePro);
            selectedProfile.UnitsPro = adtunitTypeComboBox.SelectedIndex;
       //     MessageBox.Show("Profile units: " + selectedProfile.UnitsPro);
            selectedProfile.CharVdirectPro = adtcharactersVerticalDirectionComboBox.SelectedIndex;
       //     MessageBox.Show("Profile vertical character direction: " + selectedProfile.CharVdirectPro);
            selectedProfile.CharHdirectPro = adtcharactersHorizontalDirectionComboBox.SelectedIndex;
       //     MessageBox.Show("Profile horizontal character direction: " + selectedProfile.CharHdirectPro);
            selectedProfile.JobdirectionPro = adtjobHorizontalDirectionComboBox.SelectedIndex;
       //     MessageBox.Show("Profile job direction: " + selectedProfile.JobdirectionPro);
            selectedProfile.DelenietachaPro = Convert.ToInt32(adtdelenietacha.Text);
       //     MessageBox.Show("Profile delimiter: " + selectedProfile.DelenietachaPro);
            selectedProfile.PruductSpeedPro = Convert.ToInt32(adtconstSpeedSet.Text);
      //     MessageBox.Show("Profile product speed: " + selectedProfile.PruductSpeedPro);
            selectedProfile.MedzerapredPro = Convert.ToInt32(adtmedzerapred.Text);
        //    MessageBox.Show("Profile margin before: " + selectedProfile.MedzerapredPro);
            selectedProfile.MedzerazaPro = Convert.ToInt32(adtmedzeraza.Text);
         //   MessageBox.Show("Profile margin after: " + selectedProfile.MedzerazaPro);
            selectedProfile.PoceopPro = Convert.ToInt32(adtpocetOP.Text);
         //   MessageBox.Show("Profile number of operations: " + selectedProfile.PoceopPro);
            selectedProfile.IntervapopPro = Convert.ToInt32(adtintarvalOP.Text);
         //   MessageBox.Show("Profile interval of operations: " + selectedProfile.IntervapopPro);

            _profileManager.SaveProfiles();
            MessageBox.Show("Dáta boli uložené do: " + profileName);


        }

        private void saveProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                int selectedIndex = saveProfile.SelectedIndex;
                if (selectedIndex == 0)
                {
                    btnSavePro.IsEnabled = false;
                }
                else
                {
                    btnSavePro.IsEnabled = true;
                }
            
        }

        private void ValidateAndConvertAdtConstSpeedSet(TextBox adtconstSpeedSet)
        {
            int adtnumber3;
            if (string.IsNullOrEmpty(adtconstSpeedSet.Text))
            {
                adtconstSpeedSet.Text = "100";
            }

            if (!int.TryParse(adtconstSpeedSet.Text, out adtnumber3))
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

            adtconstSpeedSet.Text = adtnumber3.ToString();

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtspeedSetVar = string.Format("{0:X4}", adtnumber3);
        }

        private void ValidateAndConvertAdtpocetOP(TextBox adtpocetOP)
        {
            int adtnumber3;

            if (string.IsNullOrEmpty(adtpocetOP.Text))
            {
                adtpocetOP.Text = "0";
            }

            if (!int.TryParse(adtpocetOP.Text, out adtnumber3))
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

            adtpocetOP.Text = adtnumber3.ToString();

            byte[] bytescon3 = BitConverter.GetBytes(adtnumber3);
            string hexmedzp3 = BitConverter.ToString(bytescon3).Replace("-", "");

            adtpocetopak = string.Format("{0:X2}", adtnumber3);
        }
        private void ValidateAndConvertAdtdelenietacha(TextBox adtdelenietacha)
        {
            
            int adtnumber3;

            if (string.IsNullOrEmpty(adtdelenietacha.Text))
            {
                adtdelenietacha.Text = "7";
            }

            if (!int.TryParse(adtdelenietacha.Text, out adtnumber3))
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

            adtdelenietacha.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adttacho = string.Format("{0:X2}", adtnumber3);
        }
        private void ValidateAndConvertAdtmedzeraza(TextBox adtmedzeraza)
        {
            
            int adtnumber3;

            if (string.IsNullOrEmpty(adtmedzeraza.Text))
            {
                adtmedzeraza.Text = "3";
            }

            if (!int.TryParse(adtmedzeraza.Text, out adtnumber3))
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

            adtmedzeraza.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtmedzza = string.Format("{0:X4}", adtnumber3);
        }
        private void ValidateAndConvertAdtmedzerapred(TextBox adtmedzerapred)
        {
            int adtnumber3;

            if (string.IsNullOrEmpty(adtmedzerapred.Text))
            {
                adtmedzerapred.Text = "3";
            }

            if (!int.TryParse(adtmedzerapred.Text, out adtnumber3))
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

            adtmedzerapred.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtmedzpred = string.Format("{0:X4}", adtnumber3);
        }
        private void ValidateAndConvertAdtIntarvalOP(TextBox adtintarvalOP)
        {
            
            int adtnumber3;

            if (string.IsNullOrEmpty(adtintarvalOP.Text))
            {
                adtintarvalOP.Text = "2";
            }

            if (!int.TryParse(adtintarvalOP.Text, out adtnumber3))
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

            adtintarvalOP.Text = adtnumber3.ToString();//.PadLeft(4, '0');

            byte[] adtbytescon3 = BitConverter.GetBytes(adtnumber3);
            string adthexmedzp3 = BitConverter.ToString(adtbytescon3).Replace("-", "");

            adtintervalOp = string.Format("{0:X4}", adtnumber3);
        }




    }

}
