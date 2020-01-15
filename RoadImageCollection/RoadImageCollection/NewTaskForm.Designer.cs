namespace RoadImageCollection
{
    partial class NewTaskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.NewTaskFormCbb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewTaskFormRoadName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NewTaskFormDirectionName = new System.Windows.Forms.ComboBox();
            this.RoadChoose = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.NewTaskFormSetTaskName = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.NewTaskFormCbb);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.NewTaskFormRoadName);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.NewTaskFormDirectionName);
            this.splitContainer1.Panel1.Controls.Add(this.RoadChoose);
            this.splitContainer1.Panel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.NewTaskFormSetTaskName);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(570, 554);
            this.splitContainer1.SplitterDistance = 339;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // NewTaskFormCbb
            // 
            this.NewTaskFormCbb.FormattingEnabled = true;
            this.NewTaskFormCbb.IntegralHeight = false;
            this.NewTaskFormCbb.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.NewTaskFormCbb.Location = new System.Drawing.Point(244, 136);
            this.NewTaskFormCbb.Name = "NewTaskFormCbb";
            this.NewTaskFormCbb.Size = new System.Drawing.Size(37, 25);
            this.NewTaskFormCbb.TabIndex = 11;
            this.NewTaskFormCbb.Text = "1";
            this.NewTaskFormCbb.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(133, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "车道数量：";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "所在车道：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(133, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "方向：";
            // 
            // NewTaskFormRoadName
            // 
            this.NewTaskFormRoadName.Location = new System.Drawing.Point(244, 74);
            this.NewTaskFormRoadName.Name = "NewTaskFormRoadName";
            this.NewTaskFormRoadName.Size = new System.Drawing.Size(100, 23);
            this.NewTaskFormRoadName.TabIndex = 7;
            this.NewTaskFormRoadName.TextChanged += new System.EventHandler(this.SetRoadName);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "道路名称：";
            // 
            // NewTaskFormDirectionName
            // 
            this.NewTaskFormDirectionName.FormattingEnabled = true;
            this.NewTaskFormDirectionName.IntegralHeight = false;
            this.NewTaskFormDirectionName.Items.AddRange(new object[] {
            "上行",
            "下行"});
            this.NewTaskFormDirectionName.Location = new System.Drawing.Point(244, 246);
            this.NewTaskFormDirectionName.Name = "NewTaskFormDirectionName";
            this.NewTaskFormDirectionName.Size = new System.Drawing.Size(50, 25);
            this.NewTaskFormDirectionName.TabIndex = 5;
            this.NewTaskFormDirectionName.Text = "上行";
            this.NewTaskFormDirectionName.SelectedIndexChanged += new System.EventHandler(this.NewTaskFormDirectionName_SelectedIndexChanged);
            // 
            // RoadChoose
            // 
            this.RoadChoose.FormattingEnabled = true;
            this.RoadChoose.IntegralHeight = false;
            this.RoadChoose.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.RoadChoose.Location = new System.Drawing.Point(244, 194);
            this.RoadChoose.Name = "RoadChoose";
            this.RoadChoose.Size = new System.Drawing.Size(37, 25);
            this.RoadChoose.TabIndex = 2;
            this.RoadChoose.Text = "1";
            this.RoadChoose.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Monospac821 BT", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(390, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 43);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.NewTaskForm_Cancel_button);
            // 
            // NewTaskFormSetTaskName
            // 
            this.NewTaskFormSetTaskName.Font = new System.Drawing.Font("Monospac821 BT", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NewTaskFormSetTaskName.Location = new System.Drawing.Point(276, 18);
            this.NewTaskFormSetTaskName.Name = "NewTaskFormSetTaskName";
            this.NewTaskFormSetTaskName.Size = new System.Drawing.Size(90, 43);
            this.NewTaskFormSetTaskName.TabIndex = 0;
            this.NewTaskFormSetTaskName.Text = "创建任务";
            this.NewTaskFormSetTaskName.UseVisualStyleBackColor = true;
            this.NewTaskFormSetTaskName.Click += new System.EventHandler(this.NewTaskForm_SetTask_button);
            // 
            // NewTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 554);
            this.Controls.Add(this.splitContainer1);
            this.Name = "NewTaskForm";
            this.Text = "新建采集任务";
            this.Load += new System.EventHandler(this.NewTaskForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button NewTaskFormSetTaskName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox RoadChoose;
        private System.Windows.Forms.ComboBox NewTaskFormDirectionName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NewTaskFormRoadName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox NewTaskFormCbb;
        private System.Windows.Forms.Label label4;

    

        private void SetRoadName(object sender, System.EventArgs e)
        {
            
        }
    }
}