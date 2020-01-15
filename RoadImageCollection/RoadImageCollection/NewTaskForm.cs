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
    public partial class NewTaskForm : Form
    {
        public delegate void SendStringWith4(string name, string direction, string roadNum, string route);
        public event SendStringWith4 SendMainFormTitle;
        public NewTaskForm()
        {
            InitializeComponent();
        }

        private void NewTaskForm_Load(object sender, EventArgs e)
        {

        }


        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void NewTaskForm_Cancel_button(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewTaskForm_SetTask_button(object sender, EventArgs e)
        {
            if (NewTaskFormRoadName.Text == "")
            {
                MessageBox.Show("道路信息不能为空", "提示");
            }
            else
            {
                if (SendMainFormTitle != null)
                    SendMainFormTitle(NewTaskFormRoadName.Text, NewTaskFormDirectionName.Text, NewTaskFormCbb.Text, RoadChoose.Text);
                this.Close();
            }
            }

        private void NewTaskFormDirectionName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
