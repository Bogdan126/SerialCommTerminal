using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;


namespace SerialCommTerminal
{
    public partial class Form1 : Form
    {
        public string ReceiveData;
        public int pom=0;

        public Form1()
        {
            InitializeComponent();
            OdczytPortow();

            Port.DtrEnable = true;
            Port.RtsEnable = true;
        }

        void OdczytPortow()
        {
            String[] ports = SerialPort.GetPortNames();
            ReadPorts.Items.AddRange(ports);
        }

        private void OpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                if (ReadPorts.Text == "")
                {
                }
                else if (BaudRate.Text == "")
                {
                }
                else
                {
                    Port.PortName = ReadPorts.Text; //Najpierw przypisujemy nazwę portu, który wybraliśmy z listy
                    Port.BaudRate = Convert.ToInt32(BaudRate.Text); //Przypisujemy szybkość transmisji
                    Port.Parity = Parity.None;
                    Port.StopBits = StopBits.One;
                    Port.DataBits = 8;
                    ClosePort.Enabled = true;
                    OpenPort.Enabled = false;
                    string pathfile = @"D:\Users\Bogdan\temp";
                    string filename = "new2.txt";
                    File.Delete(pathfile + filename);

                    Port.Open();
                    Port.DtrEnable = true;
                    Port.RtsEnable = true;
                    Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
        //Wyłączenie portu po naciśnięciu przycisku
        private void ClosePort_Click(object sender, EventArgs e)
        {
            
            Port.Close();
            ClosePort.Enabled = false;
            OpenPort.Enabled = true;
            string pathfile = @"D:\Users\Bogdan\temp";
            string filename = "new2.txt";
            File.Delete(pathfile + filename);
        }

        //Funkcja do odbierania danych i przesyłania ich do pliku tekstowego
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            string indata = sp.ReadExisting();
            ReceiveData += indata + Environment.NewLine;
            string pathfile = @"D:\Users\Bogdan\tempnew2.txt";
            string filename = "new2.txt";
            System.IO.File.WriteAllText(pathfile + filename, ReceiveData);
            var lineCount = File.ReadLines(pathfile + filename).Count();
            string[] data = new string[lineCount];
            data = File.ReadLines(pathfile+filename).ToArray();
            double[] datad = new double[lineCount];
            if (Port.IsOpen)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (Double.TryParse(data[i], out double temp))
                        datad[i] = temp;
                }
                Scope.Channels[0].Data.SetYData(datad);
                Array.Clear(datad, 0, datad.Length);
                
            }
        }
    }
}