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


namespace RoadImageCollection
{
    // gps data position class
    public class gpsPosition
    {
        public string lng;
        public string lat;
    }
    // gps data queue
    public class GpsQueue {
        int maxSize;    //栈容量
        gpsPosition[] data;  //栈数据
        int top;    //栈顶
        int bottom; //栈底
        public gpsPosition this[int index] {
            get {
                return data[index];
            }
        }
        public int getTop {
            get {
                return top;
            }
        }
        public void initializaionStact(int size) {
            data = new gpsPosition[size];
            maxSize = size;
            top = -1;
            bottom = -1;
        }
        public int getGpsQueueLength {
            get {
                return top + 1;
            }
        }
        public bool isFull() {
            if (top == maxSize - 1) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool isEmpty() {
            if (top == -1)
            {
                return true;
            }
            else {
                return false;
            }
        }
        public gpsPosition popObject()
        {
            gpsPosition temp = null;
            if (!isEmpty())
            {
                temp = data[top];
                if (top > 0) {
                    top--;
                }  
            }
            else {
                Console.WriteLine("====popObject()空栈====");
            }
            return temp;
        }
        public void pushObject(gpsPosition obj)
        {
            if (isEmpty()) {
                bottom = 0;
            }
            if (isFull()) {
                //Console.WriteLine("====pushObject()栈满====");
                List<gpsPosition> list = data.ToList();
                list.RemoveAt(bottom);
                list.Add(null);
                gpsPosition[] newData = list.ToArray();
                data = newData;
                top--;
            }
            top++;
            data[top] = obj;
        }
    }
    public partial class GpsForm : Form
    {
        SerialPort serialcom = new SerialPort();
        GpsQueue gpsQueue = new GpsQueue();
        public GpsForm()
        {
            InitializeComponent();
        }
        private void btn_OpenClick(object sender, EventArgs e)
        {
            string str1 = comboBox1.Text;
            try
            {
                if (str1 == null) {
                    MessageBox.Show("请选择一个串口", "提示");
                    return;
                }
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                serialcom.PortName = comboBox1.Text;
                serialcom.BaudRate = Convert.ToInt32(comboBox2.Text);
                serialcom.DataBits = Convert.ToInt16(comboBox3.Text);
                serialcom.Open();
                MessageBox.Show("串口打开成功！", str1);
            }
            catch (Exception er)
            {
                MessageBox.Show("Error:" + er.Message, "Error");
                return;
            }           
        }
        private void GpsForm_Load(object sender, EventArgs e)
        {
            gpsQueue.initializaionStact(10);
            comboBox2.Text = "9600";
            comboBox3.Text = "8";
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            serialcom.BaudRate = 9600;
            serialcom.StopBits = StopBits.One;
            serialcom.DataBits = 8;
            serialcom.Handshake = Handshake.None;
            serialcom.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);//注册DataReceived事件
            serialcom.Close();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] byteRead = new byte[serialcom.BytesToRead];
            serialcom.Read(byteRead, 0, byteRead.Length);
            string gpsData = null;
            string strRcv = null;
            for (int i = 0; i < byteRead.Length; i++)
            { 
                strRcv += ((char)Convert.ToInt32(byteRead[i]));
            }
            textBox1.Text += strRcv;
            gpsData = strRcv;
            string[] gpsDataArr = gpsData.Split('$');
            try {
                gpsDataArr[1].Contains(",");
            }
            catch
            {
                return;
            }
            string[] gpsArr = gpsDataArr[1].Split(',');
            if (gpsArr[0] == "GNRMC") {
                if (gpsArr[2] == "V") {
                    return;
                }
                else
                {
                    double dLon = Convert.ToDouble(gpsArr[5]);
                    dLon = dLon / 100;
                    string[] lonArr = dLon.ToString().Split('.');
                    string lo = (((Convert.ToDouble(lonArr[1]) / 60) * 10000)).ToString("#");
                    for (int b = 0; b < lonArr[1].Length; b++)
                    {
                        if (lonArr[1].Substring(b, 1) == "0")
                        {
                            lo = "0" + lo;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Double dLat = Convert.ToDouble(gpsArr[3]);
                    dLat = dLat / 100;
                    string[] latArr = dLat.ToString().Split('.');
                    string la = (((Convert.ToDouble(latArr[1]) / 60) * 10000)).ToString("#");
                    for (int a = 0; a < latArr[1].Length; a++)
                    {
                        if (latArr[1].Substring(a, 1) == "0")
                        {
                            la = "0" + la;
                        }
                        else
                        {
                            break;
                        }
                    }
                    string lon = lonArr[0].ToString() + "." + lo.Substring(0, 5);
                    string lat = latArr[0].ToString() + "." + la.Substring(0, 6);
                    gpsPosition gp = new gpsPosition();
                    gp.lat = lat;
                    gp.lng = lon;
                    gpsQueue.pushObject(gp);
                    //Console.WriteLine("{0}, {1}", gp.lng, gp.lat);
                }
            }  
        }
        public string getLngFromQueue() {
            int top = gpsQueue.getTop;
            return gpsQueue[top].lng;
        }
        public string getLatFromQueue()
        {
            int top = gpsQueue.getTop;
            return gpsQueue[top].lat;
        }
    }
}
