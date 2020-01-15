using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoadImageCollection
{
    public partial class MainformBtnStopForm : Form
    {
        public class GlobalValue {
            public static bool flag = false;
        }
      

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Cancel(object sender, EventArgs e)
        {
            GlobalValue.flag = false;
            this.Close();
        }

        private void btn_Confirm(object sender, EventArgs e)
        {
            GlobalValue.flag = true;
            this.Close();
        }
        public MainformBtnStopForm()
        {
            InitializeComponent();
        }
    }
}
