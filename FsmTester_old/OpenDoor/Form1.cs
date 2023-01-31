using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDoor.Properties;
using FSM_Authorization;

namespace OpenDoor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Settings st = new Settings();
        FSM_Authorization.ConnectionManager fsm = new FSM_Authorization.ConnectionManager();
        FSM_Authorization.ConnectionManager.Converter Cnv = ConnectionManager.Converter.Tac;
        ConfigManagerLibrary.ComMng cm = new ConfigManagerLibrary.ComMng();

        private void txtIp_TextChanged(object sender, EventArgs e)
        {
            st.Ip = txtIp.Text;
            st.Save();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            st.Port = txtPort.Text;
            st.Save();
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {
            st.Address = txtAdd.Text;
            st.Save();
        }

        private void cmbDevType_SelectedIndexChanged(object sender, EventArgs e)
        {
            st.DeviceType = cmbDevType.SelectedItem.ToString();
            st.Save();

            if (cmbDevType.Text == "Cezeri")
                txtAdd.Enabled = false;
            else
                txtAdd.Enabled = true;
        }

        private void txtRelayTime_TextChanged(object sender, EventArgs e)
        {
            st.RelayTime = txtRelayTime.Text;
            st.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width, 176);
            txtAdd.Text = st.Address;
            txtPort.Text = st.Port;
            txtIp.Text = st.Ip;
            cmbDevType.Text = st.DeviceType;
            txtRelayTime.Text = st.RelayTime;

            if (txtAdd.Text == "") txtAdd.Text = "34";
            if (txtIp.Text == "") txtIp.Text = "192.168.0.100";
            if (txtPort.Text == "") txtPort.Text = "1001";
            if (cmbDevType.SelectedIndex == -1) cmbDevType.SelectedIndex = 0;
            if (txtRelayTime.Text == "") txtRelayTime.Text = "2";

            if (cmbDevType.Text == "Cezeri")
                txtAdd.Enabled = false;
            else
                txtAdd.Enabled = true;

        }

        private void btnSendAccess_Click(object sender, EventArgs e)
        {
            this.Text = "Haberleşme Sorunu";
            try
            {
                if (cmbDevType.Text == "FSM")
                {
                    if (fsm.Access(txtIp.Text, Convert.ToInt32(txtPort.Text), Convert.ToInt32(txtAdd.Text), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtRelayTime.Text) * 10, ConnectionManager.BuzzerState.BuzzerOn, 500, Cnv) == ConnectionManager.ReturnValues.Successful)
                    {
                        this.Text = "Kapı Açıldı";
                    }
                }
                else if (cmbDevType.Text == "FSM Ekranlı")
                {
                    if (fsm.Access(txtIp.Text, Convert.ToInt32(txtPort.Text), Convert.ToInt32(txtAdd.Text), ConnectionManager.AccessType.Accept, "          ", Convert.ToInt32(txtRelayTime.Text)*10, ConnectionManager.BuzzerState.BuzzerOn, 500, Cnv) == ConnectionManager.ReturnValues.Successful)
                    {
                        this.Text = "Kapı Açıldı";
                    }
                }
                else if (cmbDevType.Text == "Cezeri")
                {
                    if (cm.SendAccess("", txtIp.Text, Convert.ToInt32(txtPort.Text), 0, ConfigManagerLibrary.ComMng.Access.Accept, Convert.ToByte(txtRelayTime.Text), " Hosgeldiniz  ", 500) == ConfigManagerLibrary.ComMng.ReturnValues.Succesfull)
                    {
                        this.Text = "Kapı Açıldı";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width,317 );
        }

        private void ayarlarıGizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width, 176);
        }






    }
}
