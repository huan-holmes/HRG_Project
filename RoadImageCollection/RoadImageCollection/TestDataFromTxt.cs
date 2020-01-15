using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace RoadImageCollection
{
    
    
    public partial class TestDataFromTxt : Form
    {
        private int count_ = 0;
        private System.Timers.Timer timer3;
        GpsQueue gpsTestQueue = new GpsQueue();
        public List<String[]> gpsData_list_ = new List<String[]>();
        public string test_lng;
        public string test_lat;
        public TestDataFromTxt()
        {
            InitializeComponent();
            timer_Set();
            gpsTestQueue.initializaionStact(10);

        }
        public void LoadGpsData()
        {
            
            string path = "E:\\data\\3.txt";
            System.IO.StreamReader file_reader = new System.IO.StreamReader(path);
            string strLine;
            while ((strLine = file_reader.ReadLine()) != null)
            {
               // Console.WriteLine(strLine);
                
                if (strLine != null && strLine.Length > 0)
                {
                    if (strLine.Split('$')[1].Split(',')[0] == "GNRMC")
                    {
                        
                        gpsData_list_.Add(strLine.Split('$')[1].Split(','));
                    }
                   
                    
                }
                strLine = file_reader.ReadLine();
            }
           
        }
   
        public void timer_Set()
        {
            Console.WriteLine("====timer_Set()====");
            int interval = 1000;
            timer3 = new System.Timers.Timer(interval);
            timer3.AutoReset = true;
            timer3.Enabled = true;
            timer3.Elapsed += new System.Timers.ElapsedEventHandler(GpsDataStack);
        }
        public void GpsDataStack(object sender, ElapsedEventArgs e)
        {
            if (gpsData_list_.Count == 0)
            {
                return;
            }
            if (gpsData_list_.Count < count_)
            {
                timer3.Enabled = false;
                return;
            }
            var data = gpsData_list_[count_];
            if (data[2] == "V")
            {

                count_++;
                return;
            }
            else
            {
                double dLon = Convert.ToDouble(data[5]);
                dLon = dLon / 100.0;
                string[] lonArr = dLon.ToString().Split('.');
                string lo = (((Convert.ToDouble(lonArr[1]) / 60.0) * 10000)).ToString("#");
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
                Double dLat = Convert.ToDouble(data[3]);
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
                string lon = lonArr[0].ToString() + "." + lo.Substring(0, 6);
                string lat = latArr[0].ToString() + "." + la.Substring(0, 6);
                test_lng = lon;
                test_lat = lat;
                gpsPosition gp = new gpsPosition();
                gp.lng = lon;
                gp.lat = lat;
                gpsTestQueue.pushObject(gp);
                int top = gpsTestQueue.getTop;
                //Console.WriteLine("gp: {0}, gpsLat: {1}", lon, lat);
                //Console.WriteLine("gpsLng: {0}, gpsLat: {1}", gpsTestStack[top].lng, gpsTestStack[top].lat);

            }
            count_++;
        }
        public string getTestLngFromQueue()
        {
            int top = gpsTestQueue.getTop;
            return gpsTestQueue[top].lng;
        }       
        public string getTestLatFromQueue()
        {
            int top = gpsTestQueue.getTop;
            return gpsTestQueue[top].lat;
        }
    }
}
