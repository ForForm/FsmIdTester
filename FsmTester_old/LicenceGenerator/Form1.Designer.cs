namespace LicenceGenerator
{
    partial class z
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
            System.Windows.Forms.GroupBox Keys;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(z));
            this.hexAccessReadKey = new LicenceGenerator.HexValue();
            this.hexAccessWriteKey = new LicenceGenerator.HexValue();
            this.hexDefaultKey = new LicenceGenerator.HexValue();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtWeb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.hexMac = new LicenceGenerator.HexValue();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFirm = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtFirm = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbAccessBlock = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.generatelicFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getlicFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pBar = new System.Windows.Forms.ToolStripProgressBar();
            this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            Keys = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            Keys.SuspendLayout();
            groupBox1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Keys
            // 
            Keys.Controls.Add(this.hexAccessReadKey);
            Keys.Controls.Add(this.hexAccessWriteKey);
            Keys.Controls.Add(this.hexDefaultKey);
            Keys.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            Keys.Location = new System.Drawing.Point(6, 30);
            Keys.Name = "Keys";
            Keys.Size = new System.Drawing.Size(423, 112);
            Keys.TabIndex = 4;
            Keys.TabStop = false;
            Keys.Text = "Keys";
            // 
            // hexAccessReadKey
            // 
            this.hexAccessReadKey.AutoScroll = true;
            this.hexAccessReadKey.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hexAccessReadKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexAccessReadKey.Caption = "Access Read Key";
            this.hexAccessReadKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hexAccessReadKey.Hex0 = "44";
            this.hexAccessReadKey.Hex1 = "61";
            this.hexAccessReadKey.Hex2 = "68";
            this.hexAccessReadKey.Hex3 = "69";
            this.hexAccessReadKey.Hex4 = "6C";
            this.hexAccessReadKey.Hex5 = "69";
            this.hexAccessReadKey.Location = new System.Drawing.Point(10, 81);
            this.hexAccessReadKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hexAccessReadKey.MaximumSize = new System.Drawing.Size(401, 22);
            this.hexAccessReadKey.MinimumSize = new System.Drawing.Size(401, 22);
            this.hexAccessReadKey.Name = "hexAccessReadKey";
            this.hexAccessReadKey.Size = new System.Drawing.Size(401, 22);
            this.hexAccessReadKey.TabIndex = 13;
            // 
            // hexAccessWriteKey
            // 
            this.hexAccessWriteKey.AutoScroll = true;
            this.hexAccessWriteKey.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hexAccessWriteKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexAccessWriteKey.Caption = "Access Write Key";
            this.hexAccessWriteKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hexAccessWriteKey.Hex0 = "53";
            this.hexAccessWriteKey.Hex1 = "74";
            this.hexAccessWriteKey.Hex2 = "41";
            this.hexAccessWriteKey.Hex3 = "50";
            this.hexAccessWriteKey.Hex4 = "72";
            this.hexAccessWriteKey.Hex5 = "6D";
            this.hexAccessWriteKey.Location = new System.Drawing.Point(10, 56);
            this.hexAccessWriteKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hexAccessWriteKey.MaximumSize = new System.Drawing.Size(401, 22);
            this.hexAccessWriteKey.MinimumSize = new System.Drawing.Size(401, 22);
            this.hexAccessWriteKey.Name = "hexAccessWriteKey";
            this.hexAccessWriteKey.Size = new System.Drawing.Size(401, 22);
            this.hexAccessWriteKey.TabIndex = 12;
            // 
            // hexDefaultKey
            // 
            this.hexDefaultKey.AutoScroll = true;
            this.hexDefaultKey.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hexDefaultKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexDefaultKey.Caption = "Default Key";
            this.hexDefaultKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hexDefaultKey.Hex0 = "FF";
            this.hexDefaultKey.Hex1 = "FF";
            this.hexDefaultKey.Hex2 = "FF";
            this.hexDefaultKey.Hex3 = "FF";
            this.hexDefaultKey.Hex4 = "FF";
            this.hexDefaultKey.Hex5 = "FF";
            this.hexDefaultKey.Location = new System.Drawing.Point(10, 30);
            this.hexDefaultKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hexDefaultKey.MaximumSize = new System.Drawing.Size(401, 22);
            this.hexDefaultKey.MinimumSize = new System.Drawing.Size(401, 22);
            this.hexDefaultKey.Name = "hexDefaultKey";
            this.hexDefaultKey.Size = new System.Drawing.Size(401, 22);
            this.hexDefaultKey.TabIndex = 11;
            this.hexDefaultKey.Load += new System.EventHandler(this.hexDefaultKey_Load);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.panel8);
            groupBox1.Controls.Add(this.panel7);
            groupBox1.Controls.Add(this.panel6);
            groupBox1.Controls.Add(this.panel4);
            groupBox1.Controls.Add(this.hexMac);
            groupBox1.Controls.Add(this.panel1);
            groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            groupBox1.Location = new System.Drawing.Point(6, 148);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(423, 176);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "User";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.txtFax);
            this.panel8.Controls.Add(this.label15);
            this.panel8.Controls.Add(this.label16);
            this.panel8.Location = new System.Drawing.Point(10, 146);
            this.panel8.MaximumSize = new System.Drawing.Size(401, 22);
            this.panel8.MinimumSize = new System.Drawing.Size(401, 22);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(401, 22);
            this.panel8.TabIndex = 31;
            // 
            // txtFax
            // 
            this.txtFax.BackColor = System.Drawing.Color.White;
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFax.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFax.Location = new System.Drawing.Point(72, 2);
            this.txtFax.MaxLength = 200;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(324, 18);
            this.txtFax.TabIndex = 26;
            this.txtFax.Text = "+90 (216) 459 04 84";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(3, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 17);
            this.label15.TabIndex = 24;
            this.label15.Text = "FAX";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(55, 2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(12, 17);
            this.label16.TabIndex = 25;
            this.label16.Text = ":";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.txtTel);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Location = new System.Drawing.Point(10, 122);
            this.panel7.MaximumSize = new System.Drawing.Size(401, 22);
            this.panel7.MinimumSize = new System.Drawing.Size(401, 22);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(401, 22);
            this.panel7.TabIndex = 30;
            // 
            // txtTel
            // 
            this.txtTel.BackColor = System.Drawing.Color.White;
            this.txtTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTel.Location = new System.Drawing.Point(72, 2);
            this.txtTel.MaxLength = 200;
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(324, 18);
            this.txtTel.TabIndex = 26;
            this.txtTel.Text = "+90 (216) 442 20 62";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 17);
            this.label13.TabIndex = 24;
            this.label13.Text = "TEL";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(55, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(12, 17);
            this.label14.TabIndex = 25;
            this.label14.Text = ":";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.txtMail);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Location = new System.Drawing.Point(10, 98);
            this.panel6.MaximumSize = new System.Drawing.Size(401, 22);
            this.panel6.MinimumSize = new System.Drawing.Size(401, 22);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(401, 22);
            this.panel6.TabIndex = 29;
            // 
            // txtMail
            // 
            this.txtMail.BackColor = System.Drawing.Color.White;
            this.txtMail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMail.Location = new System.Drawing.Point(72, 2);
            this.txtMail.MaxLength = 200;
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(324, 18);
            this.txtMail.TabIndex = 26;
            this.txtMail.Text = "info@barkodes.com.tr";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(3, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "MAIL";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(55, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 17);
            this.label12.TabIndex = 25;
            this.label12.Text = ":";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.txtWeb);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(10, 74);
            this.panel4.MaximumSize = new System.Drawing.Size(401, 22);
            this.panel4.MinimumSize = new System.Drawing.Size(401, 22);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(401, 22);
            this.panel4.TabIndex = 28;
            // 
            // txtWeb
            // 
            this.txtWeb.BackColor = System.Drawing.Color.White;
            this.txtWeb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWeb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeb.Location = new System.Drawing.Point(72, 2);
            this.txtWeb.MaxLength = 200;
            this.txtWeb.Name = "txtWeb";
            this.txtWeb.Size = new System.Drawing.Size(324, 18);
            this.txtWeb.TabIndex = 26;
            this.txtWeb.Text = "www.barkodes.com.tr";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "WEB";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(55, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 17);
            this.label10.TabIndex = 25;
            this.label10.Text = ":";
            // 
            // hexMac
            // 
            this.hexMac.AutoScroll = true;
            this.hexMac.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hexMac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexMac.Caption = "MAC Address";
            this.hexMac.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hexMac.Hex0 = "FF";
            this.hexMac.Hex1 = "FF";
            this.hexMac.Hex2 = "FF";
            this.hexMac.Hex3 = "FF";
            this.hexMac.Hex4 = "FF";
            this.hexMac.Hex5 = "FF";
            this.hexMac.Location = new System.Drawing.Point(10, 26);
            this.hexMac.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hexMac.MaximumSize = new System.Drawing.Size(401, 22);
            this.hexMac.MinimumSize = new System.Drawing.Size(401, 22);
            this.hexMac.Name = "hexMac";
            this.hexMac.Size = new System.Drawing.Size(401, 22);
            this.hexMac.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(10, 50);
            this.panel1.MaximumSize = new System.Drawing.Size(401, 22);
            this.panel1.MinimumSize = new System.Drawing.Size(401, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(401, 22);
            this.panel1.TabIndex = 27;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.White;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(72, 2);
            this.txtUserName.MaxLength = 200;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(324, 18);
            this.txtUserName.TabIndex = 26;
            this.txtUserName.Text = "Barkodes Bilgi. Sist. Bilgi İlet. Yaz. ve Tic. Ltd. Şti.";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(46, 17);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "NAME";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(55, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = ":";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.label6);
            groupBox2.Controls.Add(this.panel2);
            groupBox2.Controls.Add(this.panel3);
            groupBox2.Controls.Add(this.panel5);
            groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            groupBox2.Location = new System.Drawing.Point(6, 330);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(423, 127);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "License Info";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(175, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 21);
            this.label6.TabIndex = 31;
            this.label6.Text = "Get Lic";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cbFirm);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(179, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(232, 34);
            this.panel2.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Lic Type";
            // 
            // cbFirm
            // 
            this.cbFirm.DropDownHeight = 110;
            this.cbFirm.FormattingEnabled = true;
            this.cbFirm.IntegralHeight = false;
            this.cbFirm.Items.AddRange(new object[] {
            "Barkodes",
            "Bayiler"});
            this.cbFirm.Location = new System.Drawing.Point(85, 2);
            this.cbFirm.Name = "cbFirm";
            this.cbFirm.Size = new System.Drawing.Size(142, 29);
            this.cbFirm.TabIndex = 6;
            this.cbFirm.Text = "Barkodes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(67, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 17);
            this.label3.TabIndex = 25;
            this.label3.Text = ":";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.txtFirm);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(10, 68);
            this.panel3.MaximumSize = new System.Drawing.Size(400, 50);
            this.panel3.MinimumSize = new System.Drawing.Size(200, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(400, 29);
            this.panel3.TabIndex = 28;
            // 
            // txtFirm
            // 
            this.txtFirm.BackColor = System.Drawing.Color.White;
            this.txtFirm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFirm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirm.Location = new System.Drawing.Point(58, 3);
            this.txtFirm.MaxLength = 200;
            this.txtFirm.MinimumSize = new System.Drawing.Size(0, 10);
            this.txtFirm.Multiline = true;
            this.txtFirm.Name = "txtFirm";
            this.txtFirm.Size = new System.Drawing.Size(337, 21);
            this.txtFirm.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 17);
            this.label4.TabIndex = 24;
            this.label4.Text = "Firm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(40, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 17);
            this.label5.TabIndex = 25;
            this.label5.Text = ":";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.cmbAccessBlock);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(10, 28);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(124, 34);
            this.panel5.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(3, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 24;
            this.label8.Text = "Access";
            // 
            // cmbAccessBlock
            // 
            this.cmbAccessBlock.DropDownHeight = 110;
            this.cmbAccessBlock.FormattingEnabled = true;
            this.cmbAccessBlock.IntegralHeight = false;
            this.cmbAccessBlock.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "5",
            "6",
            "8",
            "9",
            "10",
            "12",
            "13",
            "14",
            "16",
            "17",
            "18",
            "20",
            "21",
            "22",
            "24",
            "25",
            "26",
            "28",
            "29",
            "30",
            "32",
            "33",
            "34",
            "36",
            "37",
            "38",
            "40",
            "41",
            "42",
            "44",
            "45",
            "46",
            "48",
            "49",
            "50",
            "52",
            "53",
            "54",
            "56",
            "57",
            "58",
            "60",
            "61",
            "62"});
            this.cmbAccessBlock.Location = new System.Drawing.Point(72, 2);
            this.cmbAccessBlock.Name = "cmbAccessBlock";
            this.cmbAccessBlock.Size = new System.Drawing.Size(44, 29);
            this.cmbAccessBlock.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(55, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 17);
            this.label9.TabIndex = 25;
            this.label9.Text = ":";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generatelicFileToolStripMenuItem,
            this.getlicFileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(435, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // generatelicFileToolStripMenuItem
            // 
            this.generatelicFileToolStripMenuItem.Name = "generatelicFileToolStripMenuItem";
            this.generatelicFileToolStripMenuItem.Size = new System.Drawing.Size(123, 21);
            this.generatelicFileToolStripMenuItem.Text = "Generate .lic File";
            this.generatelicFileToolStripMenuItem.Click += new System.EventHandler(this.generatelicFileToolStripMenuItem_Click);
            // 
            // getlicFileToolStripMenuItem
            // 
            this.getlicFileToolStripMenuItem.Name = "getlicFileToolStripMenuItem";
            this.getlicFileToolStripMenuItem.Size = new System.Drawing.Size(89, 21);
            this.getlicFileToolStripMenuItem.Text = "Get .lic File";
            this.getlicFileToolStripMenuItem.Click += new System.EventHandler(this.getlicFileToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pBar,
            this.LabelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 499);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(435, 27);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pBar
            // 
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(133, 21);
            // 
            // LabelStatus
            // 
            this.LabelStatus.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(53, 22);
            this.LabelStatus.Text = "Started";
            // 
            // rtbDebug
            // 
            this.rtbDebug.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtbDebug.Location = new System.Drawing.Point(6, 463);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.Size = new System.Drawing.Size(423, 32);
            this.rtbDebug.TabIndex = 9;
            this.rtbDebug.Text = "";
            // 
            // z
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.ClientSize = new System.Drawing.Size(435, 526);
            this.Controls.Add(this.rtbDebug);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(Keys);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "z";
            this.Text = "TT-FSM Licence Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            Keys.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem generatelicFileToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbAccessBlock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar pBar;
        private System.Windows.Forms.ToolStripStatusLabel LabelStatus;
        private HexValue hexDefaultKey;
        private HexValue hexMac;
        private System.Windows.Forms.ToolStripMenuItem getlicFileToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtbDebug;
        private HexValue hexAccessReadKey;
        private HexValue hexAccessWriteKey;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtFirm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtWeb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
    }
}

