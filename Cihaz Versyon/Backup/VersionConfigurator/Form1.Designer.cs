namespace BarkodesDeviceVerison
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pBar = new System.Windows.Forms.ToolStripProgressBar();
            this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtAdr = new System.Windows.Forms.TextBox();
            this.lblAdr = new System.Windows.Forms.Label();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.btnTestConn = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtTcpPort = new System.Windows.Forms.TextBox();
            this.txtUdpPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbVersion = new System.Windows.Forms.ComboBox();
            this.btnSerVersion = new System.Windows.Forms.Button();
            this.btnGetVersion = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnListCezeri = new System.Windows.Forms.Button();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.lbDevices = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSerVersionUdp = new System.Windows.Forms.Button();
            this.btnGerVersionUdp = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pBar,
            this.LabelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 370);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(677, 24);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pBar
            // 
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(100, 18);
            // 
            // LabelStatus
            // 
            this.LabelStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(21, 19);
            this.LabelStatus.Text = "...";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtAdr);
            this.groupBox7.Controls.Add(this.lblAdr);
            this.groupBox7.Controls.Add(this.cmbDeviceType);
            this.groupBox7.Controls.Add(this.btnTestConn);
            this.groupBox7.Controls.Add(this.txtIP);
            this.groupBox7.Controls.Add(this.txtTcpPort);
            this.groupBox7.Controls.Add(this.txtUdpPort);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Location = new System.Drawing.Point(7, -2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(670, 43);
            this.groupBox7.TabIndex = 24;
            this.groupBox7.TabStop = false;
            // 
            // txtAdr
            // 
            this.txtAdr.BackColor = System.Drawing.Color.Gainsboro;
            this.txtAdr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAdr.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.txtAdr.Location = new System.Drawing.Point(432, 14);
            this.txtAdr.Name = "txtAdr";
            this.txtAdr.Size = new System.Drawing.Size(34, 20);
            this.txtAdr.TabIndex = 26;
            this.txtAdr.Text = "34";
            this.txtAdr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAdr
            // 
            this.lblAdr.AutoSize = true;
            this.lblAdr.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.lblAdr.ForeColor = System.Drawing.Color.DimGray;
            this.lblAdr.Location = new System.Drawing.Point(392, 16);
            this.lblAdr.Name = "lblAdr";
            this.lblAdr.Size = new System.Drawing.Size(34, 19);
            this.lblAdr.TabIndex = 27;
            this.lblAdr.Text = "Adr";
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbDeviceType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDeviceType.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Items.AddRange(new object[] {
            "Cezeri T&A",
            "Fsm 1453"});
            this.cmbDeviceType.Location = new System.Drawing.Point(472, 11);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(110, 27);
            this.cmbDeviceType.TabIndex = 25;
            this.cmbDeviceType.SelectedIndexChanged += new System.EventHandler(this.cmbDeviceType_SelectedIndexChanged);
            // 
            // btnTestConn
            // 
            this.btnTestConn.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnTestConn.Location = new System.Drawing.Point(588, 11);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(76, 27);
            this.btnTestConn.TabIndex = 8;
            this.btnTestConn.Text = "Test Con";
            this.btnTestConn.UseVisualStyleBackColor = true;
            this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.Gainsboro;
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIP.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.txtIP.Location = new System.Drawing.Point(26, 14);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(111, 20);
            this.txtIP.TabIndex = 2;
            this.txtIP.Text = "192.168.1.101";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTcpPort
            // 
            this.txtTcpPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtTcpPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTcpPort.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.txtTcpPort.Location = new System.Drawing.Point(211, 16);
            this.txtTcpPort.Name = "txtTcpPort";
            this.txtTcpPort.Size = new System.Drawing.Size(49, 20);
            this.txtTcpPort.TabIndex = 4;
            this.txtTcpPort.Text = "1001";
            this.txtTcpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtUdpPort
            // 
            this.txtUdpPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtUdpPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUdpPort.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.txtUdpPort.Location = new System.Drawing.Point(339, 15);
            this.txtUdpPort.Name = "txtUdpPort";
            this.txtUdpPort.Size = new System.Drawing.Size(46, 20);
            this.txtUdpPort.TabIndex = 6;
            this.txtUdpPort.Text = "1311";
            this.txtUdpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(266, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 19);
            this.label8.TabIndex = 7;
            this.label8.Text = "UdpPort";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(143, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 19);
            this.label7.TabIndex = 5;
            this.label7.Text = "TcpPort";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(6, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 19);
            this.label6.TabIndex = 3;
            this.label6.Text = "IP";
            // 
            // cmbVersion
            // 
            this.cmbVersion.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.cmbVersion.FormattingEnabled = true;
            this.cmbVersion.Items.AddRange(new object[] {
            "Undefined",
            "Basic",
            "Plus",
            "Premium",
            "Smart"});
            this.cmbVersion.Location = new System.Drawing.Point(6, 19);
            this.cmbVersion.Name = "cmbVersion";
            this.cmbVersion.Size = new System.Drawing.Size(138, 27);
            this.cmbVersion.TabIndex = 26;
            // 
            // btnSerVersion
            // 
            this.btnSerVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnSerVersion.Location = new System.Drawing.Point(6, 52);
            this.btnSerVersion.Name = "btnSerVersion";
            this.btnSerVersion.Size = new System.Drawing.Size(138, 27);
            this.btnSerVersion.TabIndex = 27;
            this.btnSerVersion.Text = "Set Version";
            this.btnSerVersion.UseVisualStyleBackColor = true;
            this.btnSerVersion.Click += new System.EventHandler(this.btnSerVersion_Click);
            // 
            // btnGetVersion
            // 
            this.btnGetVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnGetVersion.Location = new System.Drawing.Point(6, 85);
            this.btnGetVersion.Name = "btnGetVersion";
            this.btnGetVersion.Size = new System.Drawing.Size(138, 27);
            this.btnGetVersion.TabIndex = 28;
            this.btnGetVersion.Text = "Get Version";
            this.btnGetVersion.UseVisualStyleBackColor = true;
            this.btnGetVersion.Click += new System.EventHandler(this.btnGetVersion_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnListCezeri);
            this.groupBox5.Controls.Add(this.cbDevices);
            this.groupBox5.Controls.Add(this.lbDevices);
            this.groupBox5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.groupBox5.Location = new System.Drawing.Point(7, 44);
            this.groupBox5.MaximumSize = new System.Drawing.Size(150, 400);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(126, 320);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Cezeri Devices";
            // 
            // btnListCezeri
            // 
            this.btnListCezeri.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnListCezeri.Location = new System.Drawing.Point(10, 17);
            this.btnListCezeri.Name = "btnListCezeri";
            this.btnListCezeri.Size = new System.Drawing.Size(110, 27);
            this.btnListCezeri.TabIndex = 30;
            this.btnListCezeri.Text = "List Device";
            this.btnListCezeri.UseVisualStyleBackColor = true;
            this.btnListCezeri.Click += new System.EventHandler(this.btnListCezeri_Click);
            // 
            // cbDevices
            // 
            this.cbDevices.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(8, 46);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(113, 23);
            this.cbDevices.TabIndex = 4;
            // 
            // lbDevices
            // 
            this.lbDevices.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbDevices.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbDevices.ForeColor = System.Drawing.Color.Brown;
            this.lbDevices.FormattingEnabled = true;
            this.lbDevices.ItemHeight = 18;
            this.lbDevices.Location = new System.Drawing.Point(6, 73);
            this.lbDevices.Name = "lbDevices";
            this.lbDevices.Size = new System.Drawing.Size(115, 238);
            this.lbDevices.TabIndex = 3;
            this.lbDevices.SelectedIndexChanged += new System.EventHandler(this.lbDevices_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSerVersionUdp);
            this.groupBox1.Controls.Add(this.btnGerVersionUdp);
            this.groupBox1.Controls.Add(this.cmbVersion);
            this.groupBox1.Controls.Add(this.btnSerVersion);
            this.groupBox1.Controls.Add(this.btnGetVersion);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.groupBox1.Location = new System.Drawing.Point(527, 44);
            this.groupBox1.MaximumSize = new System.Drawing.Size(150, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 191);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change Version";
            // 
            // btnSerVersionUdp
            // 
            this.btnSerVersionUdp.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnSerVersionUdp.Location = new System.Drawing.Point(6, 118);
            this.btnSerVersionUdp.Name = "btnSerVersionUdp";
            this.btnSerVersionUdp.Size = new System.Drawing.Size(138, 27);
            this.btnSerVersionUdp.TabIndex = 29;
            this.btnSerVersionUdp.Text = "Set Version UDP";
            this.btnSerVersionUdp.UseVisualStyleBackColor = true;
            this.btnSerVersionUdp.Click += new System.EventHandler(this.btnSerVersionUdp_Click);
            // 
            // btnGerVersionUdp
            // 
            this.btnGerVersionUdp.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnGerVersionUdp.Location = new System.Drawing.Point(6, 151);
            this.btnGerVersionUdp.Name = "btnGerVersionUdp";
            this.btnGerVersionUdp.Size = new System.Drawing.Size(138, 27);
            this.btnGerVersionUdp.TabIndex = 30;
            this.btnGerVersionUdp.Text = "Get Version UDP";
            this.btnGerVersionUdp.UseVisualStyleBackColor = true;
            this.btnGerVersionUdp.Click += new System.EventHandler(this.btnGerVersionUdp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(677, 394);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LabelStatus;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnTestConn;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtTcpPort;
        private System.Windows.Forms.TextBox txtUdpPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.ComboBox cmbVersion;
        private System.Windows.Forms.Button btnSerVersion;
        private System.Windows.Forms.Button btnGetVersion;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnListCezeri;
        private System.Windows.Forms.ListBox lbDevices;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSerVersionUdp;
        private System.Windows.Forms.Button btnGerVersionUdp;
        private System.Windows.Forms.TextBox txtAdr;
        private System.Windows.Forms.Label lblAdr;
        private System.Windows.Forms.ToolStripProgressBar pBar;
    }
}

