namespace BinFileGenerator
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
            this.btnImpTxt = new System.Windows.Forms.Button();
            this.btnExpBin = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.rValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImpTxt
            // 
            this.btnImpTxt.Location = new System.Drawing.Point(93, 12);
            this.btnImpTxt.Name = "btnImpTxt";
            this.btnImpTxt.Size = new System.Drawing.Size(75, 23);
            this.btnImpTxt.TabIndex = 14;
            this.btnImpTxt.Text = "Import .txt";
            this.btnImpTxt.UseVisualStyleBackColor = true;
            this.btnImpTxt.Visible = false;
            this.btnImpTxt.Click += new System.EventHandler(this.btnImpTxt_Click);
            // 
            // btnExpBin
            // 
            this.btnExpBin.Location = new System.Drawing.Point(12, 12);
            this.btnExpBin.Name = "btnExpBin";
            this.btnExpBin.Size = new System.Drawing.Size(75, 23);
            this.btnExpBin.TabIndex = 13;
            this.btnExpBin.Text = "Export .bin";
            this.btnExpBin.UseVisualStyleBackColor = true;
            this.btnExpBin.Click += new System.EventHandler(this.btnExpBin_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 54);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStrip1.Size = new System.Drawing.Size(228, 22);
            this.statusStrip1.TabIndex = 105;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // rValue
            // 
            this.rValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rValue.Name = "rValue";
            this.rValue.Size = new System.Drawing.Size(84, 17);
            this.rValue.Text = "ReturnValue";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 76);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnImpTxt);
            this.Controls.Add(this.btnExpBin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = ".bin File Generator";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImpTxt;
        private System.Windows.Forms.Button btnExpBin;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel rValue;
    }
}

