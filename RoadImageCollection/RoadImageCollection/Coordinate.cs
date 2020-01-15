using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RoadImageCollection
{
    public partial class CoordinateForm : Form
    {
        public CoordinateForm()
        {
            InitializeComponent();
        }
       

        private void CoordinateForm_Load(object sender, EventArgs e)
        {

        }

        private void CoordinateForm_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            // start point
            int origin_x = this.Width / 100 * 20;
            int origin_y = this.Height / 100 * 80;
            int points_num = this.Width - 2 * origin_x;
            PointF[] points = new PointF[points_num];
            g.DrawLine(new Pen(Color.Black), new Point(origin_x, origin_y), new Point(this.Width - origin_x, origin_y));    // X-0.6
            g.DrawLine(new Pen(Color.Black), new Point(origin_x, origin_y), new Point(origin_x, this.Height - origin_y - this.Height / 100 * 20));  // Y-0.6
            g.DrawBeziers(new Pen(new SolidBrush(Color.Black)), new Point[] { new Point(10 + origin_x, origin_y - ((10 - 10) / 70 * this.Height / 100 * 60 + 0)), new Point(20 + origin_x, origin_y - ((50 - 10) / 70 * this.Height / 100 * 60 + 0)), new Point(40 + origin_x, origin_y - ((30 - 10) / 70 * this.Height / 100 * 60 + 0)), new Point(70 + origin_x, origin_y - ((80 - 10) / 70 * this.Height / 100 * 60 + 0)) });
            
        }
    }
}
