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
using System.IO;


namespace BluetoothApplication
{
    public partial class Form1 : Form
    {
        TextWriter tw; 
        delegate void SetTextCallback(string text);

        string path;
        static string comstr;
        static int baudint;
        string fileName,sd, ss,st, sf = "", sF = "";
        ToolTip tp = new ToolTip();
        public Form1()
        {
            InitializeComponent();
            tp.SetToolTip(label4, "Created By Meet Patel../*_ ");
            // Adding ComPorts intothe comboBox and baudrate
            Comports();
            comboBox_comport.SelectedIndex = 0;
            Baudrate();
            //comstr = comboBox_comport.Text;
            //Console.WriteLine(comstr); 
            //baudint = Convert.ToInt32(ComboBox_baud.Text);
            // A1ssigning the value od com and baud
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
        }

        private void FileOption()
        {
            //throw new NotImplementedException();
            sd = DateTime.Now.ToString("dd/MM/yy");
            st = DateTime.Now.ToString("hh:mm:ss:tt");
            string[] dateF = sd.Split('/');
            string[] timef = st.Split(':');
            int i, j;
            for (i = 0, j = 0; i < timef.Length; i++, j++)
            {
                if (j < dateF.Length)
                {
                    dateF[i] = dateF[i].Trim();
                    sF = sF + dateF[i]+" ";
                }
                timef[j] = timef[j].Trim();
                sf = sf + timef[i]+" ";

            }

            string folderName = @path + "\\"+sF;
            string a = "\\";
            string b = ".txt";
            fileName = @folderName + a +  sf + b;
            sF = "";
            sf = "";
           // Console.WriteLine(folderName);
            //Console.WriteLine(fileName);
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
                //Console.Write("No");
            }
           

            if (!System.IO.File.Exists(fileName))
            {
                using (var myfile = System.IO.File.Create(fileName))
                {
                    myfile.Close();
                    
                }

            }
           /* else if (File.Exists(fileName))
            {
                TextWriter tw = new StreamWriter(fileName, true);
                tw.WriteLine("Meet Patel");
                tw.Close();
            }*/
        

    }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.DataViewer.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.DataViewer.AppendText(text);
            }
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {

                SerialPort spl = (SerialPort)sender;
               Console.WriteLine(spl.ReadLine() + "\n");
                 tw = new StreamWriter(fileName, true);
                tw.WriteLine(spl.ReadLine()+"\n");
                tw.Close();
                //File.Create(fileName).Close();
                ss = spl.ReadLine() + "\n";
                SetText(ss);
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Baudrate()
        {
            //throw new NotImplementedException();
            
            ComboBox_baud.Items.Add(9600);
            ComboBox_baud.Items.Add(19200);
            ComboBox_baud.SelectedIndex = 1;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Console.Beep();
            try {
                tw.Close();
                //System.IO.File.Create(fileName).Close();
                File.SetAttributes(fileName, FileAttributes.ReadOnly);
                serialPort1.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
           
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Select Folder to save the Text File Generated By the Programs";
            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxFolderPath.Text = folderDlg.SelectedPath;
                path = folderDlg.SelectedPath.ToString();
            }
        }

        private void textBoxFolderPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Comports()
        {
            //throw new NotImplementedException();
            
            foreach (string s in SerialPort.GetPortNames())
            {
                try {
                    comboBox_comport.Items.Add(s);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Com port is not added");
                }
                
            }
            //comboBox_comport.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comstr = comboBox_comport.Text;
            //serialPort1.PortName = comstr;
        }

        private void ComboBox_baud_SelectedIndexChanged(object sender, EventArgs e)
        {

            baudint = Convert.ToInt32(ComboBox_baud.Text);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Console.Beep();
            serialPort1.PortName = comstr;
            serialPort1.BaudRate = baudint;
            FileOption();
            try
            {
                // opening the serial port
                if (!serialPort1.IsOpen)
                    serialPort1.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

           

        }
    }
}
