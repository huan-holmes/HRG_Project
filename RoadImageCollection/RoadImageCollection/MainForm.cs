using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Collections;
using HalconDotNet;
using System.Drawing.Drawing2D;
using System.Timers;

namespace RoadImageCollection
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class MainForm : Form
    {
        // gpsPosition class 
        private class gpsPositionClass
        {
            public static string lng;   // longitude
            public static string lat;   // latitude
        }
        private class pictureClass
        {
            public static string picture_file;
        }
        public class timerIntervalClass
        {
            public static int mapTimer_interval = 1000;
            public static int halconTimer_interval = 1000;
        }
        public class controlColorClass
        {
            public static Color btn_StateColor;
            public static Color btn_RoadStateColor;
        }
        private class roadClass
        {
            public static string road_name; 
            public static int road_direction = 1; // car direction 0: upstream; 1: downstream
            public static int road_route;   
        }
        private class saveDataClass
        {
            public static string save_file_directory;
            public static string save_file_time;
        }
        // new form object newTaskForm、gpsForm、coordinateForm、mainFormBtnStopForm;
        NewTaskForm newTaskForm = new NewTaskForm();    // new a task
        GpsForm gpsForm = new GpsForm();    // read gpsdata from GpsQueue
        CoordinateForm coordinateForm = new CoordinateForm();   // show the trajectory of the ego_car
        //MainformBtnStopForm mainFormBtnStopForm = new MainformBtnStopForm();    // save data
        TestDataFromTxt testForm = new TestDataFromTxt();
        public delegate void SetImageValue();
        private System.Timers.Timer halcon_timer2_;    // hz of halcon show picture
        private string halcon_data_directory_ = "E:\\RoadImageCollection\\CollectionData\\roadImages\\01\\Dirc014";
        private int collection_state_ = -1;
        private bool gps_has_ = false;  // open gps or not
        private bool new_task_ = false;
        private bool save_directory_exist_ = false;  // directory exist or not
        private bool save_data_ = false;
        private string save_directory_ = "E:\\RoadImageCollection\\CollectionData"; // save directory
        // private variable halcon
        private HObject h_object_;  // halcon object
        private HTuple h_width_;    // width of halcon display
        private HTuple h_height_;   // height of halcon display
        private int count_ = 0; // index of halcon image in file
        public MainForm()
        {
            InitializeComponent();
        }
        // MainForm_Load
        private void MainForm_Load(object sender, EventArgs e)
        {
            InitWindowsForm();  // initialize the windowform of location
            InitPanel();    // initialize control of MainFormSplitContainer
            InitTimer();
            InitControlColor();
            baiduMapControl1.InitBaiduMap();
        }
        private void InitWindowsForm()
        {
            // the localzation of child windForm is the center of parent windowForm
            newTaskForm.StartPosition = FormStartPosition.CenterParent;
            gpsForm.StartPosition = FormStartPosition.CenterParent;
            coordinateForm.StartPosition = FormStartPosition.CenterParent;
            //mainFormBtnStopForm.StartPosition = FormStartPosition.CenterParent;
        }
        private void InitPanel()
        {
            InitHalcon();    // halcon control
            InitPictureBox();   // picturebox control
            InitListView(this.listView1);   // listView control
        }
        private void InitTimer()
        {
            InitHalconTimer();  // initialize halcon timer
        }
        private void InitControlColor()
        {
            controlColorClass.btn_StateColor = MainFormStartBtn.BackColor;
            controlColorClass.btn_RoadStateColor = MainFormStartBtn.BackColor;
        }
        private void InitHalcon()
        {
            h_width_ = hWindowControl1.Width;
            h_height_ = hWindowControl1.Height;
        }
        private void InitPictureBox()
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pictureBox1.ClientRectangle);
            Region region = new Region(gp);
            pictureBox1.Region = region;
            gp.Dispose();
            region.Dispose();
            pictureBox1.Load("ball1.png");
        }
        public void InitListView(ListView lv1)
        {
            lv1.View = View.Details;
            lv1.Columns.Add("", 0, HorizontalAlignment.Center);
            lv1.Columns.Add("道路名", lv1.Width / 100 * 10, HorizontalAlignment.Center);
            lv1.Columns.Add("道路编号", lv1.Width / 100 * 10, HorizontalAlignment.Center);
            lv1.Columns.Add("起始桩号", lv1.Width / 100 * 10, HorizontalAlignment.Center);
            lv1.Columns.Add("终止桩号", lv1.Width / 100 * 10, HorizontalAlignment.Center);
            lv1.Columns.Add("方向", lv1.Width / 100 * 10, HorizontalAlignment.Center);
            lv1.Columns.Add("保存时间", lv1.Width / 100 * 20, HorizontalAlignment.Center);
            lv1.Columns.Add("采集公里数", lv1.Width / 100 * 20, HorizontalAlignment.Center);
            lv1.Columns.Add("采集图片数量", lv1.Width - lv1.Width / 100 * 10 - lv1.Width / 100 * 10 - lv1.Width / 100 * 10 - lv1.Width / 100 * 10 - lv1.Width / 100 * 10 - lv1.Width / 100 * 20 - lv1.Width / 100 * 20, HorizontalAlignment.Center);
            ListViewItem item1 = new ListViewItem();      //先实例化ListViewItem这个类
            item1.Text = "1";                             //添加第1列内容
            item1.SubItems.Add("曙光路");                      //添加第2列内容
            item1.SubItems.Add("2");                      //添加第2列内容
            item1.SubItems.Add("K100+100");                      //添加第2列内容
            item1.SubItems.Add("K100+1000");                      //添加第2列内容
            item1.SubItems.Add("上行");                      //添加第3例内容
            item1.SubItems.Add("2019-12-26 10:20");                      //添加第2列内容
            item1.SubItems.Add("10");
            item1.SubItems.Add("10000");
            ListViewItem item2 = new ListViewItem();      //先实例化ListViewItem这个类
            item2.Text = "1";                             //添加第1列内容
            item2.SubItems.Add("曙光路");                      //添加第2列内容
            item2.SubItems.Add("1");                      //添加第2列内容
            item2.SubItems.Add("K250+200");                      //添加第2列内容
            item2.SubItems.Add("K300+1000");                      //添加第2列内容
            item2.SubItems.Add("上行");                      //添加第3例内容
            item2.SubItems.Add("2019-12-26 10:30");                      //添加第2列内容
            item2.SubItems.Add("50");
            item2.SubItems.Add("6000");
            lv1.Items.Add(item1);
            lv1.Items.Add(item2);
        }
        private void InitHalconTimer()
        {
            int interval = timerIntervalClass.halconTimer_interval;
            halcon_timer2_ = new System.Timers.Timer(interval);
            halcon_timer2_.AutoReset = true;
            halcon_timer2_.Enabled = false;
            halcon_timer2_.Elapsed += (o, e) => { LoadImages(); };
        }
        private void LoadImages()
        {
            if (InvokeRequired)
            {
                this.Invoke(new SetImageValue(LoadImages));
            }
            else
            {
                string path = halcon_data_directory_;
                System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] files = root.GetFiles();
                try
                {
                    ShowImage(root + "//" + files[count_].Name);
                    pictureClass.picture_file = root + "//" + files[count_];
                    gpsPositionClass.lng = testForm.getTestLngFromQueue();
                    gpsPositionClass.lat = testForm.getTestLatFromQueue();
                    baiduMapControl1.AddPositionData(gpsPositionClass.lng, gpsPositionClass.lat);
                    UpdateTlpInformation(); 
                    if (save_data_)
                    {
                        SaveCollectionData();
                    }    
                }
                catch
                {
                }
                count_++;
            }
        }
        private void UpdateTlpInformation()
        {
            MainFormRoadNameTB.Text = roadClass.road_name;
            MainFormLongitudeTB.Text = gpsPositionClass.lng;
            MainFormLatitudeTB.Text = gpsPositionClass.lat;
            MainFormCurvatureTB.Text = "0.2";
            MainFormVelTb.Text = "13km/h";
            MainFormAltTb.Text = "5.2m";
            MainFormYawTb.Text = "0.1";
            MainFormTimeTb.Text = DateTime.Now.ToString("yyy-MM-dd HH:m"); 
        }
        private void UpdateTimerState()
        {
            if (collection_state_ == 0)
            {
                baiduMapControl1.map_timer1_.Enabled = true;
                halcon_timer2_.Enabled = true;
            }
            else
            {
                baiduMapControl1.map_timer1_.Enabled = false;
                halcon_timer2_.Enabled = false;
            }
        }
        private void ShowImage(string imageRoute) {
            HTuple image_Width;
            HTuple image_Height;
            try {
                //Console.WriteLine("====showImage====");
                HOperatorSet.ReadImage(out h_object_, imageRoute);
                HOperatorSet.GetImageSize(h_object_, out image_Width, out image_Height);
                double ratioWidth = (1.0) * image_Width[0].I / h_width_;
                double ratioHeight = (1.0) * image_Height[0].I / h_height_;
                HTuple row1, column1, row2, column2;
                if (ratioWidth >= ratioHeight)
                {
                    row1 = -(1.0) * ((h_height_ * ratioWidth) - image_Height) / 2;
                    column1 = 0;
                    row2 = row1 + h_height_ * ratioWidth;
                    column2 = column1 + h_width_ * ratioWidth;
                    HOperatorSet.SetPart(hWindowControl1.HalconWindow, row1, column1, row2, column2);
                }
                HOperatorSet.DispObj(h_object_, hWindowControl1.HalconWindow);
            }
            catch { }
        }
        // update StatusTrip info while start、stop、keep
        private void UpdateTsslInfo()
        {
            if (collection_state_ == 0)
            {
                MainFormTsslStatus.Text = "正在采集...";
                double vel;
                vel = 13.0;
                MainFormStatusVelName.Text = "当前速度（km/h）：" + Convert.ToString(vel); // update vel display on StatusStrip
            }
            if (collection_state_ == 1)
            {
                MainFormTsslStatus.Text = "未开始采集";
            }
            if (collection_state_ == 2)
            {
                MainFormTsslStatus.Text = "采集暂停...";
                double vel;
                vel = 13.0;
                MainFormStatusVelName.Text = "当前速度（km/h）：" + Convert.ToString(vel); // update vel display on StatusStrip
            }
        }
        // update windows name while set new task
        public void UpdateInfoNewTaskForm(string name, string direction, string roadNum, string route)
        {
            if (direction == "上行")
            {
                roadClass.road_direction = 0;
            }
            else
            {
                roadClass.road_direction = 1;
            }
            this.Text = name + "     " + direction;
            roadClass.road_name = name;
            roadClass.road_route = Convert.ToInt32(route);
            RemoveTsb();
            SetRoadBtn(roadNum);
        }
        public void RemoveTsb()
        {
            for (int j = 0; j < 8; j++)
            {
                for (var k = 0; k < MainFormTsp.Items.Count; k++)
                {
                    if (MainFormTsp.Items[k].Name == "MainFormTsb" + Convert.ToString(j + 1))
                    {
                        MainFormTsp.Items.Remove(MainFormTsp.Items[k]);
                    }
                }
            }
        }
        public void SetRoadBtn(string roadNum)
        {
            int road_num = Convert.ToInt32(roadNum);
            for(int i = 0; i < road_num; i++)
            {
                ToolStripButton newtsb = new ToolStripButton();
                newtsb.Text = "车道" + Convert.ToString(i+1);
                if (i+1 == roadClass.road_route)
                {
                    newtsb.BackColor = Color.LightGray;
                }
                newtsb.Name = "MainFormTsb" + Convert.ToString(i + 1);
                newtsb.Size = new Size(52, 22);
                newtsb.Click += new EventHandler(RoadBtnClick);
                MainFormTsp.Items.Add(newtsb);
            }
        }
        private void RoadBtnClick(object sender, EventArgs e)
        {
            ToolStripButton tsb = (ToolStripButton)sender;
            if (tsb.BackColor == controlColorClass.btn_RoadStateColor)
            {
                tsb.BackColor = Color.LightGray;
            } else
            {
                tsb.BackColor = controlColorClass.btn_RoadStateColor;
            }
            string result = System.Text.RegularExpressions.Regex.Replace(tsb.Text, @"[^0-9]+", "");
            roadClass.road_route = Convert.ToInt32(result);
        }
        // new a windowForm
        private void MenuNewTask_Click(object sender, EventArgs e)
        {
            newTaskForm.SendMainFormTitle += new NewTaskForm.SendStringWith4(UpdateInfoNewTaskForm);
            save_directory_exist_ = false;
            newTaskForm.ShowDialog();        
            new_task_ = true;
        }
        // btn_start 
        private void btn_Start(object sender, EventArgs e)
        {
            if (!new_task_)
            {
                MessageBox.Show("请创建新的采集任务", "提示");
                return;
            }
            if (!gps_has_)
            {
                MessageBox.Show("启动Gps", "提示");
                return;
            }
            if (!new_task_ | !gps_has_)
            {
                return;
            }
            collection_state_ = 0;
            UpdateTimerState();
            UpdateTsslInfo();
            UpdateTlpInformation();
            save_data_ = true;
            if (MainFormStartBtn.BackColor == controlColorClass.btn_StateColor)
            {
                MainFormStartBtn.BackColor = Color.LightGray;
                MainFormStopBtn.BackColor = controlColorClass.btn_StateColor;
            }
                
        }
        // btn_stop
        private void btn_Stop(object sender, EventArgs e)
        {
            if (MainFormStartBtn.BackColor == controlColorClass.btn_StateColor)
            {
                if (MainFormStopBtn.BackColor == Color.LightGray)
                {
                    MainFormStopBtn.BackColor = controlColorClass.btn_StateColor;
                }
                return;
            }
            else
            {
                MainFormStopBtn.BackColor = Color.LightGray;
            }
            MainFormStartBtn.BackColor = controlColorClass.btn_StateColor;
            UpdateTimerState();
            UpdateTsslInfo();
            collection_state_ = 1;
            if (save_data_)
            {
                AddToListView(this.listView1);
                save_data_ = false;
                
            }
        }
        // btn_Keep
        private void btn_Keep(object sender, EventArgs e)
        {
            if (MainFormStartBtn.BackColor == controlColorClass.btn_StateColor)
            {
                return;
            }
            if (MainFormKeepBtn.BackColor == controlColorClass.btn_StateColor)
            {
                MainFormKeepBtn.BackColor = Color.LightGray;
                collection_state_ = 2;
                UpdateTimerState();
                UpdateTsslInfo();
            }
            else
            {
                MainFormKeepBtn.BackColor = controlColorClass.btn_StateColor;
                collection_state_ = 0;
                UpdateTimerState();
            }
        }
        private void AddToListView(ListView lv1)
        {
            ListViewItem item1 = new ListViewItem();      //先实例化ListViewItem这个类
            item1.Text = "1";                             //添加第1列内容
            item1.SubItems.Add(roadClass.road_name);                      //添加第2列内容
            item1.SubItems.Add(Convert.ToString(roadClass.road_route));                      //添加第2列内容
            item1.SubItems.Add("K100+100");                      //添加第2列内容
            item1.SubItems.Add("K100+1000");                      //添加第2列内容
            if (roadClass.road_direction == 0)
            {
                item1.SubItems.Add("上行");                      //添加第3例内容
            }
            else
            {
                item1.SubItems.Add("下行");                      //添加第3例内容
            }
            
            item1.SubItems.Add(saveDataClass.save_file_time);                      //添加第2列内容
            item1.SubItems.Add("0.2");
            item1.SubItems.Add(Convert.ToString(count_));
            lv1.Items.Add(item1);
        }
        public void SaveCollectionData() {
          if (!save_directory_exist_)
            {
                CreateDirectory();
            }
          System.IO.FileInfo fs = new System.IO.FileInfo(pictureClass.picture_file);

          string file_name = fs.Name.Split('.')[0] + "_" + gpsPositionClass.lng + "_" + gpsPositionClass.lat + ".jpg";
          fs.CopyTo(System.IO.Path.Combine(saveDataClass.save_file_directory + "\\", file_name));
        }
        public void CreateDirectory() {
            if (!System.IO.Directory.Exists(save_directory_)) {
                System.IO.Directory.CreateDirectory(save_directory_);
                
            }
            string dateTime_directory = DateTime.Now.ToString("yyy-MM-dd-HH-m");
            saveDataClass.save_file_time = DateTime.Now.ToString("yyy-MM-dd HH:m");
            string file_directory = save_directory_ + " \\" + dateTime_directory;
            if (!System.IO.Directory.Exists(file_directory)) {
                System.IO.Directory.CreateDirectory(file_directory);
            }
            string up_direction_directory = file_directory + "\\" + roadClass.road_name + "_" + "上行";
            string down_direction_directory = file_directory + "\\" + roadClass.road_name + "_" + "下行";
            if (!System.IO.Directory.Exists(up_direction_directory))
            {
                System.IO.Directory.CreateDirectory(up_direction_directory);
                System.IO.Directory.CreateDirectory(down_direction_directory);
            }
            if (roadClass.road_direction == 0)
            {
                file_directory = up_direction_directory + "\\" + Convert.ToString(roadClass.road_route);
              
                //MessageBox.Show(file_directory);
            }
            else {
                file_directory = down_direction_directory + "\\" + Convert.ToString(roadClass.road_route);
                //MessageBox.Show(file_directory);
            }
            if (!System.IO.Directory.Exists(file_directory))
            {
                System.IO.Directory.CreateDirectory(file_directory);
            }
            saveDataClass.save_file_directory = file_directory;
            save_directory_exist_ = true;
        }
       
        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
          
            
        }

        private void toolStripStatusLabel3_Click_1(object sender, EventArgs e)
        {

        }

        private void splitContainer6_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer6_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void splitContainer8_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void MainFormSplitContainer8_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void tsb_CoordinateClick(object sender, EventArgs e)
        {
            coordinateForm.ShowDialog();
        }

        private void mlt_GpsClick(object sender, EventArgs e)
        {
            gpsForm.ShowDialog();
            testForm.LoadGpsData(); // load gps from testDataFromTxt
            gps_has_ = true;
        }
    }

}
