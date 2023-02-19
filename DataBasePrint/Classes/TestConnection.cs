using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NPOI.SS.Formula.Functions;
using NPOI.Util.Collections;
using System.Configuration;
using System.ComponentModel;

namespace DataBasePrint
{
    public class TestConnection
    {
        string ipAddres;
        int ipportt;
       public bool isPrinterOnline;

        public event PropertyChangedEventHandler PropertyChanged;
        public TestConnection()
        {
            Properties.Settings.Default.PropertyChanged += (sender, e) =>
            {
                // Ak sa zmení nastavenie IPAddress, aktualizuj premennú ipAddress
                if (e.PropertyName == "IPPADDRS")
                {
                    ipAddres = Properties.Settings.Default.IPPADDRS;
                }
                // Ak sa zmení nastavenie PortNumber, aktualizuj premennú ipport
                else if (e.PropertyName == "IPPPORT")
                {
                    ipportt = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
                }
                
            };
            // Načítajte počiatočné hodnoty nastavení do premenných
            ipAddres = Properties.Settings.Default.IPPADDRS;
            ipportt = Convert.ToInt32(Properties.Settings.Default.IPPPORT);
            isPrinterOnline = false;
        }

        public void TestPrinter()
        {
            using (TcpClient printiclient = new TcpClient())
            {
                try
                {
                    printiclient.Connect(IPAddress.Parse(ipAddres), ipportt);
                    printiclient.ReceiveTimeout = 2000; // nastavenie časového limitu na očakávanie odpovede
                    using (NetworkStream printistream = printiclient.GetStream())
                    {
                        byte[] requestDatafirst = new byte[] { 0xB1, 0x00, 0x01, 0X01, 0xB1 };
                        printistream.Write(requestDatafirst, 0, requestDatafirst.Length);

                        byte[] response1 = new byte[1];
                        printistream.Read(response1, 0, response1.Length);
                        if (response1[0] != 0x06)
                        {
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
                            throw new Exception("Neočakávaná odpoveď");
                        }
                        // Prijatie dát podľa modelu E4h Identification
                        byte[] response12 = new byte[19];
                        printistream.Read(response12, 0, response12.Length);

                        // Odoslanie potvrdenia o prijatí 0x06
                        printistream.Write(new byte[] { 0x06 }, 0, 1);

                        // ak všetko prebehlo úspešne, tlačiareň je ON-LINE
                        MessageBox.Show("Tlačiareň je ON-LINE");
                        isPrinterOnline = true;
                       // sendPrintSignalbtn.IsEnabled = true;
                        //SetButtonEnabledStatus();


                    }
                }
                catch (SocketException ex)
                {
                    // ak došlo k chybe pripojenia, tlačiareň nie je ON-LINE
                    MessageBox.Show("Je tlačiareň ON-LINE? Je IP správne nastavená?");
                    isPrinterOnline = false;
                    

                }
            }
        }
    }
}
