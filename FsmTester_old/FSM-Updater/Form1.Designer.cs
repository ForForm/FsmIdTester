namespace FSM_Authorization
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
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbDevices = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.nudSerial = new System.Windows.Forms.NumericUpDown();
            this.btnSaveInfo = new System.Windows.Forms.Button();
            this.pgInfo = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMappingLoader = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.defaultToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsTextAddress = new System.Windows.Forms.ToolStripTextBox();
            this.tsBtnChgAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCnvType = new System.Windows.Forms.ComboBox();
            this.cmbPackLength = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnList = new System.Windows.Forms.ToolStripLabel();
            this.cmbIPs = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.textPort = new System.Windows.Forms.ToolStripTextBox();
            this.tsPing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.textAddress = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.textTimeOut = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsTestConn = new System.Windows.Forms.ToolStripButton();
            this.tsBtnBlink = new System.Windows.Forms.ToolStripButton();
            this.btnGetInfo = new System.Windows.Forms.ToolStripButton();
            this.pBar = new System.Windows.Forms.ToolStripProgressBar();
            this.rValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSerial)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbDebug
            // 
            this.rtbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDebug.BackColor = System.Drawing.Color.White;
            this.rtbDebug.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtbDebug.Location = new System.Drawing.Point(6, 6);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.Size = new System.Drawing.Size(579, 251);
            this.rtbDebug.TabIndex = 10;
            this.rtbDebug.Text = "";
            this.rtbDebug.TextChanged += new System.EventHandler(this.rtbDebug_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.lbDevices);
            this.groupBox3.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.groupBox3.Location = new System.Drawing.Point(5, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(61, 323);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Devices";
            // 
            // lbDevices
            // 
            this.lbDevices.BackColor = System.Drawing.Color.White;
            this.lbDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDevices.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbDevices.FormattingEnabled = true;
            this.lbDevices.ItemHeight = 17;
            this.lbDevices.Location = new System.Drawing.Point(3, 16);
            this.lbDevices.Name = "lbDevices";
            this.lbDevices.Size = new System.Drawing.Size(55, 304);
            this.lbDevices.TabIndex = 3;
            this.lbDevices.SelectedIndexChanged += new System.EventHandler(this.lbDevices_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(72, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(700, 307);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 15);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 286);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbDebug);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(601, 260);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DEBUG";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.nudSerial);
            this.tabPage2.Controls.Add(this.btnSaveInfo);
            this.tabPage2.Controls.Add(this.pgInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(601, 260);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DEV. INFO";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(170, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 107;
            this.label1.Text = "Seri No";
            // 
            // nudSerial
            // 
            this.nudSerial.Location = new System.Drawing.Point(222, 215);
            this.nudSerial.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSerial.Name = "nudSerial";
            this.nudSerial.Size = new System.Drawing.Size(120, 20);
            this.nudSerial.TabIndex = 106;
            this.nudSerial.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSerial.ValueChanged += new System.EventHandler(this.nudSerial_ValueChanged);
            // 
            // btnSaveInfo
            // 
            this.btnSaveInfo.Location = new System.Drawing.Point(348, 213);
            this.btnSaveInfo.Name = "btnSaveInfo";
            this.btnSaveInfo.Size = new System.Drawing.Size(75, 23);
            this.btnSaveInfo.TabIndex = 2;
            this.btnSaveInfo.Text = "Set Device";
            this.btnSaveInfo.UseVisualStyleBackColor = true;
            this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
            // 
            // pgInfo
            // 
            this.pgInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgInfo.Location = new System.Drawing.Point(3, 3);
            this.pgInfo.Name = "pgInfo";
            this.pgInfo.Size = new System.Drawing.Size(595, 254);
            this.pgInfo.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Refresh,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.toolStripSeparator5,
            this.cancelToolStripButton,
            this.toolStripSeparator,
            this.helpToolStripButton,
            this.toolStripSeparator2,
            this.tsMappingLoader,
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripSeparator4,
            this.defaultToolStripButton,
            this.toolStripSeparator6,
            this.tsTextAddress,
            this.tsBtnChgAdd,
            this.toolStripSeparator7});
            this.toolStrip1.Location = new System.Drawing.Point(0, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(52, 258);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Refresh
            // 
            this.Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Refresh.Image = ((System.Drawing.Image)(resources.GetObject("Refresh.Image")));
            this.Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(50, 20);
            this.Refresh.Text = "toolStripButton1";
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(50, 6);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(50, 20);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.ToolTipText = "Up Load // Güncelle";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(50, 6);
            // 
            // cancelToolStripButton
            // 
            this.cancelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cancelToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelToolStripButton.Image")));
            this.cancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelToolStripButton.Name = "cancelToolStripButton";
            this.cancelToolStripButton.Size = new System.Drawing.Size(50, 20);
            this.cancelToolStripButton.Text = "toolStripButton1";
            this.cancelToolStripButton.ToolTipText = "Cancel Update // Güncellemeyi İptal Et";
            this.cancelToolStripButton.Click += new System.EventHandler(this.cancelToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(50, 6);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(50, 20);
            this.helpToolStripButton.Text = "He&lp";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(50, 6);
            // 
            // tsMappingLoader
            // 
            this.tsMappingLoader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsMappingLoader.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsMappingLoader.Image = ((System.Drawing.Image)(resources.GetObject("tsMappingLoader.Image")));
            this.tsMappingLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsMappingLoader.Name = "tsMappingLoader";
            this.tsMappingLoader.Size = new System.Drawing.Size(50, 20);
            this.tsMappingLoader.Text = "MappingLoad";
            this.tsMappingLoader.ToolTipText = "MappingLoad for Cafeteria Devices";
            this.tsMappingLoader.Click += new System.EventHandler(this.tsMappingLoader_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(50, 6);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(50, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(50, 6);
            // 
            // defaultToolStripButton
            // 
            this.defaultToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.defaultToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("defaultToolStripButton.Image")));
            this.defaultToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.defaultToolStripButton.Name = "defaultToolStripButton";
            this.defaultToolStripButton.Size = new System.Drawing.Size(50, 20);
            this.defaultToolStripButton.Text = "Default Cfg / Fabrika Ayarları";
            this.defaultToolStripButton.Click += new System.EventHandler(this.defaultToolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(50, 6);
            // 
            // tsTextAddress
            // 
            this.tsTextAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tsTextAddress.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsTextAddress.Name = "tsTextAddress";
            this.tsTextAddress.Size = new System.Drawing.Size(48, 16);
            this.tsTextAddress.Text = "1";
            this.tsTextAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsBtnChgAdd
            // 
            this.tsBtnChgAdd.BackColor = System.Drawing.SystemColors.Control;
            this.tsBtnChgAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnChgAdd.Name = "tsBtnChgAdd";
            this.tsBtnChgAdd.Size = new System.Drawing.Size(50, 19);
            this.tsBtnChgAdd.Text = "Chg Ad";
            this.tsBtnChgAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsBtnChgAdd.ToolTipText = "Change IP // Ip Değiştir";
            this.tsBtnChgAdd.Click += new System.EventHandler(this.tsBtnChgAdd_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(50, 6);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.cbCnvType);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.cmbPackLength);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnFind);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtFilePath);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox3);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(713, 352);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(765, 377);
            this.toolStripContainer1.TabIndex = 16;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(608, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cnv : ";
            // 
            // cbCnvType
            // 
            this.cbCnvType.FormattingEnabled = true;
            this.cbCnvType.Items.AddRange(new object[] {
            "NewConv",
            "Tibbo",
            "Tac"});
            this.cbCnvType.Location = new System.Drawing.Point(649, 2);
            this.cbCnvType.Name = "cbCnvType";
            this.cbCnvType.Size = new System.Drawing.Size(61, 21);
            this.cbCnvType.TabIndex = 19;
            this.cbCnvType.Text = "Tac";
            this.cbCnvType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // cmbPackLength
            // 
            this.cmbPackLength.FormattingEnabled = true;
            this.cmbPackLength.Items.AddRange(new object[] {
            "32",
            "64",
            "96",
            "128",
            "160",
            "192",
            "224"});
            this.cmbPackLength.Location = new System.Drawing.Point(73, 3);
            this.cmbPackLength.Name = "cmbPackLength";
            this.cmbPackLength.Size = new System.Drawing.Size(55, 21);
            this.cmbPackLength.TabIndex = 18;
            this.cmbPackLength.Text = "224";
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnFind.Font = new System.Drawing.Font("Calibri", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFind.Location = new System.Drawing.Point(537, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnFind.Size = new System.Drawing.Size(56, 20);
            this.btnFind.TabIndex = 17;
            this.btnFind.Text = "Find";
            this.btnFind.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(133, 3);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(398, 20);
            this.txtFilePath.TabIndex = 16;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnList,
            this.cmbIPs,
            this.toolStripLabel4,
            this.textPort,
            this.tsPing,
            this.toolStripSeparator8,
            this.toolStripSeparator9,
            this.toolStripLabel3,
            this.textAddress,
            this.toolStripLabel5,
            this.textTimeOut,
            this.toolStripSeparator10,
            this.toolStripSeparator11,
            this.tsTestConn,
            this.tsBtnBlink,
            this.btnGetInfo});
            this.toolStrip2.Location = new System.Drawing.Point(3, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(762, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // btnList
            // 
            this.btnList.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnList.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnList.ForeColor = System.Drawing.Color.Black;
            this.btnList.Image = ((System.Drawing.Image)(resources.GetObject("btnList.Image")));
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(114, 22);
            this.btnList.Text = "List IP Addresses";
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // cmbIPs
            // 
            this.cmbIPs.Name = "cmbIPs";
            this.cmbIPs.Size = new System.Drawing.Size(121, 25);
            this.cmbIPs.SelectedIndexChanged += new System.EventHandler(this.cmbIPs_SelectedIndexChanged);
            this.cmbIPs.Click += new System.EventHandler(this.cmbIPs_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripLabel4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStripLabel4.ForeColor = System.Drawing.Color.Gray;
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel4.Text = "Port";
            // 
            // textPort
            // 
            this.textPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textPort.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(60, 25);
            this.textPort.Text = "1001";
            this.textPort.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsPing
            // 
            this.tsPing.BackColor = System.Drawing.Color.Transparent;
            this.tsPing.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsPing.ForeColor = System.Drawing.Color.Black;
            this.tsPing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPing.Name = "tsPing";
            this.tsPing.Size = new System.Drawing.Size(34, 22);
            this.tsPing.Text = "Ping";
            this.tsPing.Click += new System.EventHandler(this.tsPing_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripLabel3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStripLabel3.ForeColor = System.Drawing.Color.Gray;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(51, 22);
            this.toolStripLabel3.Text = "Address";
            // 
            // textAddress
            // 
            this.textAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textAddress.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(40, 25);
            this.textAddress.Text = "34";
            this.textAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripLabel5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStripLabel5.ForeColor = System.Drawing.Color.Gray;
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(53, 22);
            this.toolStripLabel5.Text = "Timeout";
            // 
            // textTimeOut
            // 
            this.textTimeOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textTimeOut.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textTimeOut.Name = "textTimeOut";
            this.textTimeOut.Size = new System.Drawing.Size(60, 25);
            this.textTimeOut.Text = "3500";
            this.textTimeOut.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textTimeOut.Click += new System.EventHandler(this.textTimeOut_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // tsTestConn
            // 
            this.tsTestConn.BackColor = System.Drawing.Color.Transparent;
            this.tsTestConn.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsTestConn.ForeColor = System.Drawing.Color.Black;
            this.tsTestConn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsTestConn.Name = "tsTestConn";
            this.tsTestConn.Size = new System.Drawing.Size(67, 22);
            this.tsTestConn.Text = "Test Conn.";
            this.tsTestConn.Click += new System.EventHandler(this.tsTestConn_Click);
            // 
            // tsBtnBlink
            // 
            this.tsBtnBlink.BackColor = System.Drawing.Color.Transparent;
            this.tsBtnBlink.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tsBtnBlink.ForeColor = System.Drawing.Color.Black;
            this.tsBtnBlink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnBlink.Name = "tsBtnBlink";
            this.tsBtnBlink.Size = new System.Drawing.Size(37, 22);
            this.tsBtnBlink.Text = "Blink";
            this.tsBtnBlink.Click += new System.EventHandler(this.tsBtnBlink_Click);
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetInfo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGetInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(52, 22);
            this.btnGetInfo.Text = "GetInfo";
            this.btnGetInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.btnGetInfo.ToolTipText = "Change IP // Ip Değiştir";
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // pBar
            // 
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(150, 20);
            // 
            // rValue
            // 
            this.rValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rValue.Name = "rValue";
            this.rValue.Size = new System.Drawing.Size(83, 21);
            this.rValue.Text = "ReturnValue";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pBar,
            this.rValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 351);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStrip1.Size = new System.Drawing.Size(765, 26);
            this.statusStrip1.TabIndex = 104;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 377);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSM Updater   --- v2.0             *.hex | *.txt Yükler ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSerial)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbDevices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton cancelToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsMappingLoader;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton defaultToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsBtnChgAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel btnList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox textPort;
        private System.Windows.Forms.ToolStripButton tsPing;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox textTimeOut;
        private System.Windows.Forms.ToolStripButton tsTestConn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox textAddress;
        private System.Windows.Forms.ToolStripTextBox tsTextAddress;
        private System.Windows.Forms.ToolStripButton tsBtnBlink;
        private System.Windows.Forms.ToolStripProgressBar pBar;
        private System.Windows.Forms.ToolStripStatusLabel rValue;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid pgInfo;
        private System.Windows.Forms.Button btnSaveInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudSerial;
        private System.Windows.Forms.ToolStripButton btnGetInfo;
        private System.Windows.Forms.ToolStripComboBox cmbIPs;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ComboBox cmbPackLength;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCnvType;
    }
}

