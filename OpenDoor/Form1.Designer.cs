namespace OpenDoor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSendAccess = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ayarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayarlarıGizleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDevType = new System.Windows.Forms.ComboBox();
            this.txtRelayTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendAccess
            // 
            resources.ApplyResources(this.btnSendAccess, "btnSendAccess");
            this.btnSendAccess.ContextMenuStrip = this.contextMenuStrip1;
            this.btnSendAccess.Name = "btnSendAccess";
            this.btnSendAccess.UseVisualStyleBackColor = true;
            this.btnSendAccess.Click += new System.EventHandler(this.btnSendAccess_Click);
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ayarlarToolStripMenuItem,
            this.ayarlarıGizleToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // ayarlarToolStripMenuItem
            // 
            resources.ApplyResources(this.ayarlarToolStripMenuItem, "ayarlarToolStripMenuItem");
            this.ayarlarToolStripMenuItem.Name = "ayarlarToolStripMenuItem";
            this.ayarlarToolStripMenuItem.Click += new System.EventHandler(this.ayarlarToolStripMenuItem_Click);
            // 
            // ayarlarıGizleToolStripMenuItem
            // 
            resources.ApplyResources(this.ayarlarıGizleToolStripMenuItem, "ayarlarıGizleToolStripMenuItem");
            this.ayarlarıGizleToolStripMenuItem.Name = "ayarlarıGizleToolStripMenuItem";
            this.ayarlarıGizleToolStripMenuItem.Click += new System.EventHandler(this.ayarlarıGizleToolStripMenuItem_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtIp
            // 
            resources.ApplyResources(this.txtIp, "txtIp");
            this.txtIp.Name = "txtIp";
            this.txtIp.TextChanged += new System.EventHandler(this.txtIp_TextChanged);
            // 
            // txtPort
            // 
            resources.ApplyResources(this.txtPort, "txtPort");
            this.txtPort.Name = "txtPort";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtAdd
            // 
            resources.ApplyResources(this.txtAdd, "txtAdd");
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.TextChanged += new System.EventHandler(this.txtAdd_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbDevType
            // 
            resources.ApplyResources(this.cmbDevType, "cmbDevType");
            this.cmbDevType.FormattingEnabled = true;
            this.cmbDevType.Items.AddRange(new object[] {
            resources.GetString("cmbDevType.Items"),
            resources.GetString("cmbDevType.Items1"),
            resources.GetString("cmbDevType.Items2")});
            this.cmbDevType.Name = "cmbDevType";
            this.cmbDevType.SelectedIndexChanged += new System.EventHandler(this.cmbDevType_SelectedIndexChanged);
            // 
            // txtRelayTime
            // 
            resources.ApplyResources(this.txtRelayTime, "txtRelayTime");
            this.txtRelayTime.Name = "txtRelayTime";
            this.txtRelayTime.TextChanged += new System.EventHandler(this.txtRelayTime_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRelayTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.cmbDevType);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSendAccess);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendAccess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ayarlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayarlarıGizleToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmbDevType;
        private System.Windows.Forms.TextBox txtRelayTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

