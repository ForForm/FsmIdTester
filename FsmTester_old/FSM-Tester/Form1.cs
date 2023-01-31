using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FSM;
using System.Net;
using System.Net.Sockets;
using FSM.Properties;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;

namespace FSM_Authorization
{
    public partial class Form1 : Form
    {
        #region Definitions
        FSM_Authorization.ConnectionManager fsm = new FSM_Authorization.ConnectionManager();
        FSM_Authorization.ConnectionManager.ReturnValues Result;
        FSM_Authorization.ConnectionManager.Converter Cnv = ConnectionManager.Converter.Tac;
        Settings setting = new Settings();
        public enum DbgMsgType { Incoming, Outgoing, Normal, Warning, Error };
        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.OrangeRed, Color.Red };
        FsmConfig cfg = new FsmConfig();
        FsmInfo dvc = new FsmInfo();
        FsmClmInfo clm = new FsmClmInfo();


        public Form1()
        {
            InitializeComponent();
            Initialize();
            timer1.Interval = 3000;
        }

        void Initialize()
        {

        }
        #endregion

        private void Debug(DbgMsgType msgtype, string msg, bool newLine)
        {
            rtbDebug.Invoke(new EventHandler(delegate
            {
                rtbDebug.SelectedText = string.Empty;
                rtbDebug.SelectionFont = new Font(rtbDebug.SelectionFont, FontStyle.Regular);
                rtbDebug.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtbDebug.AppendText(msg);
                if (newLine) rtbDebug.AppendText("\r\n");
                rtbDebug.ScrollToCaret();
            }));
            this.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataGridViewRow dr1;
            //TcpClient client = new TcpClient();
            for (int i = 0; i < 10; i++)
            {
                dr1 = new DataGridViewRow();
                dgvAuth.Rows.Add(dr1);
            }

            for (int i = 0; i < 10; i++)
            {
                dgvAuth.Rows[i].Cells[0].Value = 0;
                dgvAuth.Rows[i].Cells[2].Value = 12;
                dgvAuth.Rows[i].Cells[3].Value = 00;
            }

            pgConfig.SelectedObjects = new object[] { cfg };
            pgDvcInfo.SelectedObjects = new object[] { dvc };
            pgClmConfig.SelectedObjects = new object[] { clm };

            textTimeOut.Text = setting.Timeout;
            textAddress.Text = setting.Address;

            if (textTimeOut.Text == "") textTimeOut.Text = "300";
            if (textAddress.Text == "") textAddress.Text = "34";

            btnList.PerformClick();

            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            
        }

        #region Find Converters
        rfMultiLibrary.PhysicalCommunication tactibbo = new rfMultiLibrary.PhysicalCommunication();
        rfMultiLibrary.PhysicalCommunication.ReturnValues rvalue;
        public string[] Macs, IPs, GWs, SMs, Ports, Bauds, Pars, Datas, Stops, Flows, Names, Protocols;

        private void btnList_Click(object sender, EventArgs e)
        {
            cmbIPs.Items.Clear();
            Macs = IPs = GWs = SMs = Ports = Bauds = Pars = Datas = Stops = Flows = Protocols = Names = null;
            try
            {
                //if (tactibbo.GetLocalDevices(out Macs, out IPs, out GWs, out SMs, out Ports, out Bauds, out Pars, out Datas, out Stops, out Flows, out Names) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                //{
                //    for (int i = 0; i < Macs.Length; i++)
                //    {
                //        cmbIPs.Items.Add(IPs[i]);
                //    }

                //}
                if (tactibbo.GetLocalDevices_v2(out Macs, out IPs, out GWs, out SMs, out Ports, out Bauds, out Pars, out Datas, out Stops, out Flows, out Protocols, out Names) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                {
                    for (int i = 0; i < Macs.Length; i++)
                    {
                        cmbIPs.Items.Add(IPs[i]);
                    }
                }

                if (cmbIPs.Items.Count > 0)
                    rValue.Text = cmbIPs.Items.Count.ToString() + " Converter Bulundu";
                else
                {
                    rValue.Text = "Converter Bulunamadı !";
                    return;
                }

                for (int i = 0; i < Macs.Length; i++)
                {
                    if (IPs[i] == setting.IP)
                    {
                        cmbIPs.SelectedIndex = i;
                        pBar.Value = 100;
                        return;
                    }
                }
                pBar.Value = 100;
                cmbIPs.SelectedIndex = 0;

            }
            catch
            { }
        }

        private void cmbIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            setting.IP = cmbIPs.Text;
            setting.Save();
        }
        #endregion

        #region Configuration One By One
        private void btnChgRelayNC_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeRelayContact(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.Contact.NC, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnChgRelayNO_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeRelayContact(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.Contact.NO, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnPassBackOn_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangePassBackState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.PassBackState.PassBackOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnPassBackOff_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangePassBackState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.PassBackState.PassBackOff, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnAsciiInfoOn_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeOnlineInfoAsciiState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AsciiState.AsciiOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }

        }

        private void btnAsciiInfoOff_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeOnlineInfoAsciiState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AsciiState.AsciiOff, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnNoLog_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeOnlineInfoLogState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.Logging.NoLogs, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeOnlineInfoLogState(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.Logging.SaveLogs, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        #endregion

        #region SmartRelay
        TcpClient client;
        public Thread Thread_Inp;
        bool Listen = false;

        public delegate void DebugMessage(string Message);
        public event DebugMessage onDebugMessage;

        private void Debug(string msg)
        {
            listBox1.Invoke(new EventHandler(delegate
            {
                listBox1.Items.Add(msg);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }));
        }

        void ListenForInp(object obj)
        {
            ConnectionManager.ReturnValues rv;
            int Log = 0, Address = 0;
            client = new TcpClient();
            if (fsm.PingAndPortTest(cmbIPs.Text, 1001, client) != ConnectionManager.ReturnValues.Successful)
                return;

            int LineNum = 0;
            Listen = true;
            while (Listen)
            {
                rv = fsm.ListenSmarRelay(out Address, out Log, client, 1000, Cnv);
                if (rv == ConnectionManager.ReturnValues.Successful)
                {
                    LineNum++;
                    string InpText = LineNum.ToString() + "  Address = " + Address.ToString() + "   ";
                    if (Log == 0) InpText += "Closed";
                    if (Log == 1) InpText += "Opened";
                    Debug(InpText);
                    Thread.Sleep(1);
                }

            }
            client.Close();

        }

        private void btnSetAddress_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.ChangeDeviceAddress(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(txtNewAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    textAddress.Text = txtNewAddress.Text;
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnLsiten_Click(object sender, EventArgs e)
        {
            Thread_Inp = new Thread(this.ListenForInp);
            Thread_Inp.Start();
            foreach (Control c in tabs.TabPages[2].Controls)
            {
                c.Enabled = false;
            }
            btnStop.Enabled = true;
            listBox1.Enabled = true;


        }

        private void textAddress_TextChanged(object sender, EventArgs e)
        {
            txtAddress.Text = textAddress.Text;
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            textAddress.Text = txtAddress.Text;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Listen = false;
            Thread_Inp.Abort();
            client.Close();
            foreach (Control c in tabs.TabPages[2].Controls)
            {
                c.Enabled = true;
            }
        }

        private void btnAccs1_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            string Address = (sender as Button).Name.Substring(7, 2);
            try
            {
                byte Result = fsm.SmartRelayAccess(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(Address), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtAcsTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result != 0)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }

        }

        private void btnFindBroken_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.SetSmartRelayRedLeds(cmbIPs.Text, Convert.ToInt32(textPort.Text), 0xFF, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        #endregion

        private void btnAccess_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtAcsTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnTestConn_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.DeviceTestConnection(cmbIPs.Text, Convert.ToInt32(textPort.Text), 0xFF, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void tsPing_Click(object sender, EventArgs e)
        {
            ConnectionManager conection;
            pBar.Value = 0; this.Refresh();
            try
            {
                conection = new ConnectionManager();
                TcpClient client = new TcpClient();
                Result = conection.PingAndPortTest(cmbIPs.Text, 1001, client);
                client.Close();

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void tsTestConn_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.DeviceTestConnection(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            int[] Targs; int[] Targ;
            lbDevices.Items.Clear();
            rValue.Text = "Bekleyin"; pBar.Value = 0; this.Refresh();

            Result = fsm.GetFsmDevices(cmbIPs.Text, Convert.ToInt32(textPort.Text), out Targs, out Targ, 300, Cnv);

            if (Targs == null) Targs = new int[0];

            for (int i = 0; i < Targs.Length; i++)
                lbDevices.Items.Add(Targs[i].ToString() + Convert.ToChar(Targ[i]).ToString());

            if (Targs.Length > 0)
            {
                if(Targ[0] == 82)
                    GetSmartDeviceInfo();
                else
                    getConfigToolStripMenuItem.PerformClick();
                //getDvcInfoToolStripMenuItem.PerformClick();
                rValue.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
                pBar.Value = 100;
            }
            else rValue.Text = "Cihaz bulununamadı.";

            //if (Result == ConnectionManager.ReturnValues.Succesfull) pBar.Value = 100;
            //else pBar.Value = 0;

        }
        FsmInfo fsmInfo = new FsmInfo();
        private void GetDeviceInfo()
        {
            string Device;
            string Applicatin;
            string PcbVer;
            DateTime PrdDate;
            string FirmVer;
            string Tester;
            string Serial;

            pBar.Value = 0;
            try
            {
                Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    out Device,
                    out Applicatin,
                    out PcbVer,
                    out PrdDate,
                    out FirmVer,
                    out Tester,
                    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);


                fsmInfo.SetFsmInfo(Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);
                pgDvcInfo.SelectedObjects = new object[] { fsmInfo };

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rtbDebug.Text = "";
                    Debug(DbgMsgType.Normal, "Name:",       false); Debug(DbgMsgType.Outgoing, Device, false);
                    Debug(DbgMsgType.Normal, "   Type:",    false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                    Debug(DbgMsgType.Normal, "   Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer.Substring(0, 2) + "." + PcbVer.Substring(2, 1), false);
                    Debug(DbgMsgType.Normal, "   Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer.Substring(0, 1) + "." + FirmVer.Substring(1, 2), false);
                    Debug(DbgMsgType.Normal, "   Tester:",  false); Debug(DbgMsgType.Outgoing, Tester, false);
                    Debug(DbgMsgType.Normal, "   Date:",    false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                    Debug(DbgMsgType.Normal, "   Serial:",  false); Debug(DbgMsgType.Outgoing, Serial + "\r\n", false);
                    pBar.Value = 100; rValue.Text = Result.ToString();
                }
                else { pBar.Value = 0; rValue.Text = Result.ToString(); }
            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }
        }

        private void GetSmartDeviceInfo()
        {
            byte[] SmrtCfg = new byte[7];
            string Device;
            string Applicatin;
            string PcbVer;
            DateTime PrdDate;
            string FirmVer;
            string Tester;
            string Serial;

            pBar.Value = 0;
            try
            {
                Result = fsm.GetSmartDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    out SmrtCfg,
                    out Device,
                    out Applicatin,
                    out PcbVer,
                    out PrdDate,
                    out FirmVer,
                    out Tester,
                    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);


                fsmInfo.SetFsmCfg(SmrtCfg, Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);
                pgConfig.SelectedObjects = new object[] { fsmInfo };
                pgDvcInfo.SelectedObjects = new object[] { fsmInfo };

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rtbDebug.Text = "";
                    Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                    Debug(DbgMsgType.Normal, "   Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                    Debug(DbgMsgType.Normal, "   Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer.Substring(0, 2) + "." + PcbVer.Substring(2, 1), false);
                    Debug(DbgMsgType.Normal, "   Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer.Substring(0, 1) + "." + FirmVer.Substring(1, 2), false);
                    Debug(DbgMsgType.Normal, "   Tester:", false); Debug(DbgMsgType.Outgoing, Tester, false);
                    Debug(DbgMsgType.Normal, "   Date:", false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                    Debug(DbgMsgType.Normal, "   Serial:", false); Debug(DbgMsgType.Outgoing, Serial, false);
                    pBar.Value = 100; rValue.Text = Result.ToString();
                }
                else { pBar.Value = 0; rValue.Text = Result.ToString(); }
            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }
        }

        private void lbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DvcAdr;
            DvcAdr = lbDevices.SelectedItem.ToString();
            if (DvcAdr.Substring(1, 1) == "R")
            {
                textAddress.Text = DvcAdr.Substring(0, 1).ToString();
                GetSmartDeviceInfo();
            }
            else if (DvcAdr.Substring(2, 1) == "R")
            {
                textAddress.Text = DvcAdr.Substring(0, 2).ToString();
                GetSmartDeviceInfo();
            }
            else
            {
                if (DvcAdr.Length == 4)
                {
                    if (DvcAdr.Substring(3, 1) == "R")
                    {
                        textAddress.Text = DvcAdr.Substring(0, 3).ToString();
                        GetSmartDeviceInfo();
                    }
                    else
                    {
                        textAddress.Text = DvcAdr.ToString();
                        pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";
                        getConfigToolStripMenuItem.PerformClick();
                        GetDeviceInfo();
                    }
                }
                else
                {
                    textAddress.Text = DvcAdr.ToString();
                    pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";
                    getConfigToolStripMenuItem.PerformClick();
                    GetDeviceInfo();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            string IpAddress;
            IpAddress = cmbIPs.Text;

            try
            {
                Result = fsm.ChangeConfigParameters(cmbIPs.Text,
                                                    Convert.ToInt32(textPort.Text),
                                                    Convert.ToInt32(textAddress.Text),
                                                    cfg.GetFsmConfig(),// IpAddress,
                                                    Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }


            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        
        private void getConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            FsmConfig cfgi = null;

            try
            {

                Result = fsm.GetConfigParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),out cfgi,Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                         cfg = cfgi;
                        pgConfig.SelectedObjects = new object[] { cfg };
                        pBar.Value = 100;
                        rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
                getDvcInfo();
            }
            catch (Exception ex)
            {

            }
        }

        private void chgBaudrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionManager.BaudRate BaudR = ConnectionManager.BaudRate._115200;
            pBar.Value = 0;
            try
            {
                string Baud = "";
                switch (Baud)
                {
                    case "_115200": BaudR = ConnectionManager.BaudRate._115200; break;
                    case "_57600": BaudR = ConnectionManager.BaudRate._57600; break;
                    case "_34800": BaudR = ConnectionManager.BaudRate._38400; break;
                    case "_19200": BaudR = ConnectionManager.BaudRate._19200; break;
                    case "_9600": BaudR = ConnectionManager.BaudRate._9600; break;
                    default: break;
                }
                Result = fsm.ChangeBaudRate(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), BaudR, Convert.ToInt32(textTimeOut.Text), Cnv);


                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
            byte[] Log;
            int begin = Convert.ToInt32(txtBegin.Text);
            int end = Convert.ToInt32(txtEnd.Text);
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.AccessDirection AcsDir;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (end > LogIndex)
                        end = LogIndex;
                    else
                    {
                        Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    }
                }
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,                
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result != ConnectionManager.ReturnValues.Successful)
                {
                     Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                     return;
                }
                if (end != 0)
                {
                    for (int i = begin; i < end; i++)
                    {
                        Result = fsm.GetLastLogData(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out strLogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (Result == ConnectionManager.ReturnValues.Successful)
                        {
                            Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + LogID.ToString() + "    " + "Time: " + strLogTime + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "FBack: " + FBack.ToString(), true);
                            //i++;
                        }
                        else
                        {
                            Debug(DbgMsgType.Outgoing, i.ToString() + " " + (char)9 + Result.ToString(), true);

                        }
                    }
                }
                else
                    Debug(DbgMsgType.Outgoing, " Kayıt YOK !", true);

                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        public byte stri = 0;
        public void ListenOnline()
        {
            UInt64 ID; int Address; int OfflineLogCount; byte[] temp; int i = 0;
            ConnectionManager.AccessType acc;
            AgainClient:
            TcpClient client = new TcpClient();
            fsm.PingAndPortTest(cmbIPs.Text, Convert.ToInt32(textPort.Text), client);
            if (ckAcc.Checked)
                acc = ConnectionManager.AccessType.Accept;
            else
                acc = ConnectionManager.AccessType.Deny;
            while (true)
            {
                if (stri != 1) { client.Close(); i = 0; return; }
                if (!client.Connected) { client.Close(); goto AgainClient; }

                Result = fsm.ListenOnlineRequest(client, Convert.ToInt32(textAddress.Text),
                                out ID, out Address, acc, ConnectionManager.BuzzerState.BuzzerOn,
                                10, out OfflineLogCount, Convert.ToInt32(textTimeOut.Text), Cnv, out temp);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    i++;
                    Debug(DbgMsgType.Outgoing, i.ToString() + "  OfLogCnt: " + OfflineLogCount.ToString() + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Case: " + Result.ToString(), true);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                            //if (stri != 1) { client.Close(); i = 0; return; }
                            //if (!client.Connected) { client.Close(); goto AgainClient; }

                            //if (fsm.ListenOnlineRequest(out ID, out Address, out OfflineLogCount, client, Convert.ToInt32(textTimeOut.Text), Cnv, out temp) == ConnectionManager.ReturnValues.Successful)
                            //{
                            //    Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), acc, 10, ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                            //     if (Result == ConnectionManager.ReturnValues.Successful)
                            //     {
                            //                i++;
                            //              Debug(DbgMsgType.Outgoing, i.ToString() + "  OfLogCnt: " + OfflineLogCount.ToString() + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Case: " + Result.ToString(), true);
                            //              pBar.Value = 100;
                            //              rValue.Text = Result.ToString();
                            //     }            
                            //}
            }            
        }

        private void btnSavePers_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                Result = fsm.RecordAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), PersonName , Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnErsPer_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.EraseAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(txtID.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }


        private void btnSendAccess_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            string PersonName;
            PersonName = txtAccsName.Text;

            try
            {
                Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, PersonName, 30, ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void getIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                            out PerIndex,
                                                            out LogIndex,
                                                            out BlackIndex,
                                                            out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    rtbDebug.Text = "Log Indx: " + LogIndex.ToString() + "     Pers Indx: " + PerIndex.ToString() + "     Black Lst Indx: " + BlackIndex.ToString() + "     Ersade Per Indx: " + ErasedPerIndex.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnGetTimeTables_Click(object sender, EventArgs e)
        {
            byte[] AccessRules = null; pBar.Value = 0;
            try
            {
                Result = fsm.GetAuthRulesPerHour(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                 out AccessRules,
                                                 Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rValue.Text = Result.ToString();
                    pBar.Value = 100;

                    for (int i = 0; i < 10; i++)    //HOUR
                    {
                        dgvAuth.Rows[i].Cells[2].Value = AccessRules[i];
                    }

                    for (int i = 0; i < 10; i++)  //MIN
                    {
                        dgvAuth.Rows[i].Cells[3].Value = AccessRules[i + 10];
                    }

                    for (int i = 0; i < 10; i++)   //AUTH
                    {
                        dgvAuth.Rows[i].Cells[0].Value = AccessRules[i + 20];
                        switch (AccessRules[i + 20])
                        {
                            case 0: dgvAuth.Rows[i].Cells[1].Value = "Unlimited"; break;
                            case 1: dgvAuth.Rows[i].Cells[1].Value = "OnlyAuth"; break;
                            case 2: dgvAuth.Rows[i].Cells[1].Value = "FullAuth"; break;
                        }
                    }
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSetTimeTables_Click(object sender, EventArgs e)
        {
            byte[] AccessRules = new byte[30]; pBar.Value = 0;
            try
            {


                for (int i = 0; i < 10; i++)
                    AccessRules[i] = Convert.ToByte(dgvAuth.Rows[i].Cells[2].Value);    //HOUR
                for (int i = 0; i < 10; i++)
                    AccessRules[i + 10] = Convert.ToByte(dgvAuth.Rows[i].Cells[3].Value);    //MIN
                for (int i = 0; i < 10; i++)
                    AccessRules[i + 20] = Convert.ToByte(dgvAuth.Rows[i].Cells[0].Value);    //AUTH 

                Result = fsm.ChangeAuthRulesPerHour(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), AccessRules, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }


        private void btnChgAddress_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.ChangeDeviceAddress(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(tsTextAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    textAddress.Text = tsTextAddress.Text;
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void defaultToolStripButton_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;

            if (DialogResult.Yes != MessageBox.Show(this, "[ " + txtAddress.Text + " ]   Tüm Veriler Silinecektir, Eminmisiniz?", "Fabrika Ayarı", MessageBoxButtons.YesNo))
                return;

            try
            {
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (ConnectionManager.WorkingModes)0xFF, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void textAddress_TextChanged_1(object sender, EventArgs e)
        {
            setting.Address = textAddress.Text;
            setting.Save();
        }

        private void textTimeOut_TextChanged(object sender, EventArgs e)
        {
            setting.Timeout = textTimeOut.Text;
            setting.Save();
        }

        private void eraseLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;

            if (DialogResult.Yes != MessageBox.Show(this, "[ " + txtAddress.Text + " ]   Tüm Geçiş Bilgileri Silinecektir, Eminmisiniz?", "Silme", MessageBoxButtons.YesNo))
                return;
            
            try
            {
                Result = fsm.EraseAllLogData(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }


        private void findPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbDebug.Text = "";
            pBar.Value = 0;
            UInt32 PersonIndex = 0;
            try
            {
                Result = fsm.FindPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(txtPeronID.Text), out PersonIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "Index = " + PersonIndex.ToString(),false);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else if (Result == ConnectionManager.ReturnValues.PersonNotFound)
                {
                    Debug(DbgMsgType.Outgoing, Result.ToString(), false);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {
            //DateTime Time = new DateTime(); pBar.Value = 0;
            //try
            //{
            //    Result = fsm.GetDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out Time, Convert.ToInt32(textTimeOut.Text), Cnv);
            //    if (Result == ConnectionManager.ReturnValues.Succesfull)
            //    {
            //        pBar.Value = 100;
            //        rValue.Text = Result.ToString();
            //        Debug(DbgMsgType.Outgoing, "Current Time" + Time.ToString(), true);

            //    }
            //    else
            //    {
            //        pBar.Value = 0;
            //        rValue.Text = Result.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    pBar.Value = 0;
            //    rValue.Text = ex.ToString();
            //}
        }

        private void eraseTimeTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.EraseAuthRulesPerHour(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void eraseAllPeopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;

            if (DialogResult.Yes != MessageBox.Show(this, "[ " + txtAddress.Text + " ]   Tüm Personel Bilgileri Silinecektir, Eminmisiniz?", "Silme", MessageBoxButtons.YesNo))
                return;

            try
            {
                Result = fsm.EraseAllPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnGetRezPeople_Click(object sender, EventArgs e)
        {
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;            byte[] Packet; uint PersonID;
            pBar.Value = 0;   rtbDebug.Text = "";
            
            try
            {
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result != ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }


                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out PerIndex, out LogIndex, out BlackIndex, out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    int StrIndex = Convert.ToInt32(txtFrom.Text);
                    int EndIndex = Convert.ToInt32(txtTo.Text);
                    if (EndIndex > PerIndex) EndIndex = PerIndex-1;
                    //--------------------------------------------------------------------------------
                    for (int i = StrIndex; i <= EndIndex; i++)
                    {
                        string PerName ;
                        DateTime StartDate, EndDate;
                        Result = fsm.GetPersonID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Packet, out PersonID, out PerName, out StartDate, out EndDate, Convert.ToInt32(textTimeOut.Text), Cnv);
                        Debug(DbgMsgType.Outgoing, i.ToString() + ":    ST: " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "    ID:  " + PersonID.ToString() + "    PN: " + PerName + "    ET: " + EndDate.ToString("yyyy-MM-dd HH:mm:ss"), true);
                    }
                    //--------------------------------------------------------------------------------
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }


                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ResetDevice(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnSetTime_Click_1(object sender, EventArgs e)
        {
            DateTime Time = new DateTime();
            //Time = DateTime.Now;
            Time = dtCurrentTime.Value;
            pBar.Value = 0;
            try
            {
                //dtCurrentTime.Value.AddMinutes(Convert.ToInt32(txtMin.Text));
                //dtCurrentTime.Value.AddHours(Convert.ToInt32(txtHour.Text));
                //Time = dtCurrentTime.Value;

                Result = fsm.ChangeDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Time, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    Debug(DbgMsgType.Outgoing, "Change Time: " + Time.ToString(), true);

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnGetTime_Click(object sender, EventArgs e)
        {
            DateTime Time = new DateTime(); pBar.Value = 0;
            try
            {
                
               // Result = fsm.GetDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out Time, Convert.ToInt32(textTimeOut.Text), Cnv);
                Result = fsm.GetDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out Time, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    Debug(DbgMsgType.Outgoing, "Current Time: " + Time.ToString(), true);

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnSendAccess_Click_1(object sender, EventArgs e)
        {
            try
            {
                Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, 30, ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void ChgDvcName_Click(object sender, EventArgs e)
        {
            string DeviceName;
            DeviceName = DvcName.Text;
            try
            {

                Result = fsm.ChangeDeviceName(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), DeviceName, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }

        }

        private void DvcName_Click(object sender, EventArgs e)
        {
            DvcName.MaxLength = 15;
        }

        private void txtAccsName_Click(object sender, EventArgs e)
        {
            txtAccsName.MaxLength = 10;
        }

        private void txtPerName_Click(object sender, EventArgs e)
        {
            txtPerName.MaxLength = 10;
        }

        private void cmbIPs_Click(object sender, EventArgs e)
        {

        }

        private void pgConfig_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void RezerSave_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            string PersonName;
            DateTime PersonClockEn = new DateTime();
            DateTime PersonClockEx = new DateTime();
            PersonClockEn = EntryDateTime.Value;
            PersonClockEx = ExitDateTime.Value;
            PersonName = PrsnNm.Text;
            //string.Format("{0:yyMMddHHmm}", PersonClock);
            //PersonClock = EntryDateTime.Text + ExitDateTime.Text;

            try
            {
                Result = fsm.RecordAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PrsnID.Text), PersonName, PersonClockEn, PersonClockEx, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnGetPeople_Click_1(object sender, EventArgs e)
        {
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex; byte[] Packet; uint PersonID;
            pBar.Value = 0; rtbDebug.Text = "";

            try
            {
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result != ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }


                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out PerIndex, out LogIndex, out BlackIndex, out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    int StrIndex = Convert.ToInt32(txtFrom.Text);
                    int EndIndex = Convert.ToInt32(txtTo.Text);
                    if (EndIndex > PerIndex) EndIndex = PerIndex - 1;
                    //--------------------------------------------------------------------------------
                    for (int i = StrIndex; i <= EndIndex; i++)
                    {
                       Result = fsm.GetPersonID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Packet, out PersonID, Convert.ToInt32(textTimeOut.Text), Cnv);
                        Debug(DbgMsgType.Outgoing, i.ToString() + "    ID:  " + PersonID.ToString(), true);
                    }
                    //--------------------------------------------------------------------------------
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }


                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        void Kromla(Bitmap img)
        {
            BitmapData bmdo = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            Bitmap bm = new Bitmap(img.Width, img.Height, PixelFormat.Format1bppIndexed);
            BitmapData bmdn = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            DateTime dt = DateTime.Now;
            int x, y;
            for (y = 0; y < img.Height; y++)
            {
                for (x = 0; x < img.Width; x++)
                {
                    int index = y * bmdo.Stride + (x * 4);
                    float oran = ((float)(((float)trackBar1.Value) / ((float)trackBar1.Maximum)));
                    if (Color.FromArgb(Marshal.ReadByte(bmdo.Scan0, index + 2), Marshal.ReadByte(bmdo.Scan0, index + 1), Marshal.ReadByte(bmdo.Scan0, index)).GetBrightness() > oran)
                        this.SetIndexedPixel(x, y, bmdn, true); //set it if its bright.
                }
            }
            bm.UnlockBits(bmdn);
            img.UnlockBits(bmdo);
            this.pictureBox2.BackgroundImage = bm;
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        protected void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
        {
            int index = y * bmd.Stride + (x >> 3);
            byte p = Marshal.ReadByte(bmd.Scan0, index);
            byte mask = (byte)(0x80 >> (x & 0x7));
            if (pixel)
                p |= mask;
            else
                p &= (byte)(mask ^ 0xff);
            Marshal.WriteByte(bmd.Scan0, index, p);
        }

        private void btnYukle_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (pictureBox2.BackgroundImage != null)
            {
                //ComMng cm = new ComMng();
               // Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.WorkingModes.ServiceMode, Convert.ToInt32(textTimeOut.Text), Cnv);
				 
                //if (Result == ConnectionManager.ReturnValues.Succesfull)
                 //  {
                    Image pic = pictureBox2.BackgroundImage;
                    CLogoManager clm = new CLogoManager(pic);
                    var stream = clm.BuildLogoGetReverseBytes();

                    for (int x = 0; x < 3; x++)
                    {
                        int y=0;
                        if (x == 1) y = 19;
                        if (x == 2) y = 39;
                   
                    if (stream.Count == 640)
                    {
                            for (int i = 0; i < stream.Count; i += 32)
                            //for (int i = 640; i > 0; i -= 32)
                            {
                                Application.DoEvents();
                                progressBar1.Value = (y+i / 32);
                                //this.Text = (i / 32).ToString();
                                 Result = fsm.SaveLogoImage(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)(i / 32), stream.GetRange(i, 32).ToArray(), Convert.ToInt32(textTimeOut.Text), Cnv);
                                if (Result == ConnectionManager.ReturnValues.Successful)
                                {
                                    return;
                                }
                            }
                    }
                   }
                    Result = fsm.RefleshScreen(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        return;
                    }  
                //}

            }
        }

        private void btnKaydet_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.BackgroundImage != null)
            {
                sfd.FileName = "";
                sfd.ShowDialog();
                if (sfd.FileName != "")
                {
                    // BMP Dosyası|*.BMP;*.DIB;*.RLE
                    // JPEG Dosyası|*.JPG;*.JPEG;*.JPE;*.JFIF
                    // GIF Dosyası|*.GIF
                    // TIFF Dosyası|*.TIF;*.TIFF
                    // PNG Dosyası|*.PNG
                    Bitmap bmp = new Bitmap(pictureBox2.BackgroundImage);
                    string fname = sfd.FileName;
                    if (fname.EndsWith(".BMP") || fname.EndsWith(".DIB") || fname.EndsWith(".RLE"))
                        bmp.Save(sfd.FileName, ImageFormat.Bmp);
                    else if (fname.EndsWith(".GIF"))
                        bmp.Save(sfd.FileName, ImageFormat.Gif);
                    else if (fname.EndsWith(".JPG") || fname.EndsWith(".JPEG") || fname.EndsWith(".JPE") || fname.EndsWith(".JFIF"))
                        bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                    else if (fname.EndsWith(".PNG"))
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    else if (fname.EndsWith(".TIF") || fname.EndsWith(".TIFF"))
                        bmp.Save(sfd.FileName, ImageFormat.Tiff);
                    else
                        bmp.Save(sfd.FileName);
                }
            }
        }

        private void LogoDegistir_Load(object sender, EventArgs e)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string filter = "";
            foreach (var item in codecs)
            {
                if (filter != "") filter += ";";
                filter += item.FilenameExtension;
            }
            filter = "Resim Dosyaları|" + filter;
            ofd.Filter = filter;

            string filter2 = "";
            foreach (var item in codecs)
            {
                if (filter2 != "") filter2 += "|";
                filter2 += item.FormatDescription + " Dosyası|" + item.FilenameExtension;
            }
            sfd.Filter = filter2;
        }

        private void btnGozat_Click(object sender, EventArgs e)
        {
            ofd.FileName = "";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                Bitmap img = new Bitmap(128, 40);
                Graphics grafik = Graphics.FromImage(img);
                if (rbBeyaz.Checked)
                    grafik.Clear(Color.White);
               // else if (rbSiyah.Checked)
                 //   grafik.Clear(Color.Black);
                Image imaj = Image.FromFile(ofd.FileName);
                imaj = resizeImage(imaj, new Size(128, 40));
                grafik.DrawImage(imaj, (img.Width - imaj.Width) / 2, 0);
                if (img.PixelFormat != PixelFormat.Format32bppPArgb)
                {
                    Bitmap temp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb);
                    Graphics g = Graphics.FromImage(temp);
                    g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    img.Dispose();
                    g.Dispose();
                    img = temp;
                }
                this.pictureBox1.BackgroundImage = (Image)img.Clone();
                //this.Text = this.pictureBox1.BackgroundImage.Size.ToString();
                Kromla((Bitmap)img.Clone());
                grafik.Dispose();
                imaj.Dispose();
                img.Dispose();
                grafik = null;
                imaj = null;
                grafik = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            if (pictureBox1.BackgroundImage != null)
            {
                Bitmap bmp = new Bitmap(pictureBox1.BackgroundImage);
                Kromla(bmp);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.ChangeDeviceAddress(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(DvcAdr.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    //textAddress.Text = txtNewAddress.Text;
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        int PCount,StopCnt;
        private void RecordPersonIDName()
        {
            long id;
            PCount = Convert.ToInt32(PerCount.Text);
            string PersonName;
            PersonName = PerNameTxt.Text;

            for (int i = StopCnt; i < PCount; i++)
            {
                id = Convert.ToUInt32(PerID.Text) + 1;
                PerID.Text = Convert.ToString(id);
                PerNameTxt.Text = PerNameTxt.Text.Substring(0,5) + Convert.ToString(i);

                Result = fsm.RecordAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PerID.Text), PersonName, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                 {
                     Bar2.Value = i;
                     label6.Text = PerCount.Text + " / " + Convert.ToString(i+1);//Result.ToString();
                     label10.Text = Result.ToString();
                 }
                 else
                 {
                     //label6.Text = PerCount.Text + " / " + Convert.ToString(i);
                     Bar2.Value = 0;
                     label10.Text = Result.ToString();
                 }
                Thread.Sleep(10);
            }
            Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.OfflineMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

            if (Result == ConnectionManager.ReturnValues.Successful)
            {
                label10.Text = "Stop";
                StopCnt = PCount;
            }
        }

        private void RecordPersonID()
        {
            long id;
            PCount = Convert.ToInt32(PerCount.Text);

            for (int i = StopCnt; i < PCount; i++)
            {
                id = Convert.ToUInt32(PerID.Text) + 1;
                PerID.Text = Convert.ToString(id);

                Result = fsm.RecordAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PerID.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Bar2.Value = i;
                    label6.Text = PerCount.Text + " / " + Convert.ToString(i + 1);
                    label10.Text = Result.ToString();
                        
                }
                else
                {
                    //label6.Text = PerCount.Text + " / " + Convert.ToString(i);
                    i = PCount;
                    Bar2.Value = Convert.ToInt32(label6.Text.Substring(PerCount.Text.Length + 3, (label6.Text.Length - (PerCount.Text.Length + 3))));
                    label10.Text = Result.ToString();
                }
                Thread.Sleep(10);
            }
            Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.OfflineMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

            if (Result == ConnectionManager.ReturnValues.Successful)
            {
                label10.Text = "Stop";
                StopCnt = PCount;
            }
        }

        private void RecordMain()
        {
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;

            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread threadID = new Thread(new ThreadStart(RecordPersonID));
            Thread threadName = new Thread(new ThreadStart(RecordPersonIDName));
            fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                                ConnectionManager.WorkingModes.ServiceMode,
                                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
            if (ClearID.Checked == true)
            {
                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);

                if (PerIndex != 0 && Result == ConnectionManager.ReturnValues.Successful)
                {
                    fsm.EraseAllPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful) { Bar2.Value = PCount; label10.Text = "Siliniyor.."; }

                    for (int i = 0; i < 10000; i++)
                    {
                        Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                    out PerIndex,
                                                                    out LogIndex,
                                                                    out BlackIndex,
                                                                    out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (PerIndex == 0 && Result == ConnectionManager.ReturnValues.Successful)
                        {
                            i = 10000;
                            if (PerNameTxt.Text == "Name") threadID.Start();
                            else threadName.Start();
                        }
                    }
                }
                else
                {
                    if (PerNameTxt.Text == "Name") threadID.Start();
                    else threadName.Start();
                }
            }
            else
            {
                if (PerNameTxt.Text == "Name") threadID.Start();
                else threadName.Start();
            }
        }

        private void PerIDSave_Click(object sender, EventArgs e)
        {
            
            Bar2.Value = 0;
            StopCnt = 0;
            string PersonName; 
            PersonName = txtPerName.Text;
            PCount = Convert.ToInt32(PerCount.Text);
            Bar2.Maximum = PCount;
            PerID.Text = "1110000000";
            try
            {
                System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
                Thread threadID = new Thread(new ThreadStart(RecordPersonID));
                Thread threadName = new Thread(new ThreadStart(RecordPersonIDName));
                Thread threadMain = new Thread(new ThreadStart(RecordMain));

                threadMain.Start();
                //fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                                                    ConnectionManager.WorkingModes.ServiceMode,
                //                                                                    Convert.ToInt32(textTimeOut.Text), Cnv);
                //if (ClearID.Checked == true)
                //{
                //    Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                                    out PerIndex,
                //                                                    out LogIndex,
                //                                                    out BlackIndex,
                //                                                    out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                   
                //        if (PerIndex != 0 && Result == ConnectionManager.ReturnValues.Succesfull)
                //        {
                //            fsm.EraseAllPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                //            for (int i = 0; i < 10000; i++)
                //            {
                //                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                                            out PerIndex,
                //                                                            out LogIndex,
                //                                                            out BlackIndex,
                //                                                            out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                //                if (PerIndex == 0 && Result == ConnectionManager.ReturnValues.Succesfull)
                //                {
                //                    i = 10000;
                //                    if (PerNameTxt.Text == "Name") threadID.Start();
                //                    else threadName.Start();
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (PerNameTxt.Text == "Name") threadID.Start();
                //            else threadName.Start();
                //        }
                //}
                //else
                //{
                //    if (PerNameTxt.Text == "Name") threadID.Start();
                //    else threadName.Start();
                //}
               
                //for (int i = 0; i < PCount; i++)
                //{
                //    id = Convert.ToUInt32(PerID.Text)+1;
                //    PerID.Text = Convert.ToString(id);

                //    Result = fsm.RecordAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PerID.Text), PersonName, Convert.ToInt32(textTimeOut.Text), Cnv);
                //    Bar2.Maximum = PCount;
                //    Bar2.Value = i;
                //    label6.Text = Convert.ToString(i);//Result.ToString();

                //   /* if (Result == ConnectionManager.ReturnValues.Succesfull)
                //    {
                //        Bar2.Maximum = PCount;
                //        Bar2.Value = i;
                //        label6.Text = Convert.ToString(i);//Result.ToString();

                //    }
                //    else
                //    {
                //        Bar2.Value = 0;
                //        label6.Text = Result.ToString();
                //    }*/
                //}
                //fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                ConnectionManager.WorkingModes.OfflineMode,
                //                                Convert.ToInt32(textTimeOut.Text), Cnv);
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void DvcAdr_TextChanged(object sender, EventArgs e)
        {

        }

        private void PerNameTxt_TextChanged(object sender, EventArgs e)
        {
            PerNameTxt.MaxLength = 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PCount = Convert.ToInt32(label6.Text.Substring(PerCount.Text.Length + 3, (label6.Text.Length - (PerCount.Text.Length + 3))));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopCnt = Bar2.Value;
            PCount = Convert.ToInt32(PerCount.Text);
            Bar2.Maximum = PCount;
            try
            {
                System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
                Thread threadID = new Thread(new ThreadStart(RecordPersonID));
                Thread threadName = new Thread(new ThreadStart(RecordPersonIDName));

                fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (PerNameTxt.Text == "Name") threadID.Start();
                else threadName.Start();

            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void GetVersion_Click(object sender, EventArgs e)
        {
            //byte version = 0;
            pBar.Value = 0;
            ConnectionManager.DeviceVersion version;
            Result = fsm.GetDeviceVersionFsm(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out version, Convert.ToInt32(textTimeOut.Text), Cnv);
            if (Result == ConnectionManager.ReturnValues.Successful)
            {
                pBar.Value = 100;
                rValue.Text = Result.ToString();
                rtbDebug.Text = version.ToString();
            }

        }

        private void findPersonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            byte[] Packet; uint PersonID; pBar.Value = 0;   rtbDebug.Text = "";
            
            try
            {
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result != ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }
                String PerName; int  PersonIndex=0, ChgInnx=0;
               DateTime StartDate, EndDate;
               for (int y = 0; y < 20; )
               {
                   Result = fsm.GetFindPersonID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), ChgInnx, out  PersonIndex, out Packet, out PersonID, out PerName, out StartDate, out EndDate, Convert.ToInt32(textTimeOut.Text), Cnv);
                   if (Result == ConnectionManager.ReturnValues.Successful)
                   {
                       Debug(DbgMsgType.Outgoing,(y+1).ToString() + "   Idx: " + PersonIndex.ToString() + ":   ST: " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "   ID:  " + PersonID.ToString() + "   PN: " + PerName + "   ET: " + EndDate.ToString("yyyy-MM-dd HH:mm:ss"), true);
                       y++; 
                       ChgInnx = PersonIndex+1;
                       pBar.Value = 100;
                       //rValue.Text = Result.ToString();
                       rValue.Text = y.ToString() + " Adet Farklı Rezervasyon Bulundu...";
                   }
                   else
                   {
                       if (y == 0)
                       {
                           pBar.Value = 0;
                           rValue.Text = "Rezervasyon Bulunamadı...";
                       }
                       y = 20;
                       
                   }
               }


                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
         }

        private void getDvcInfo()
        {            
            string Device;
            string Applicatin;
            string PcbVer;
            DateTime PrdDate;
            string FirmVer;
            string Tester;
            string Serial;

            pBar.Value = 0;
            try
            {
                Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    out Device,
                    out Applicatin,
                    out PcbVer,
                    out PrdDate,
                    out FirmVer,
                    out Tester,
                    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);


                fsmInfo.SetFsmInfo(Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pgDvcInfo.SelectedObjects = new object[] { fsmInfo };
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void setDvcInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), fsmInfo.ProductDate, fsmInfo.Tester, fsmInfo.Serial, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful) { pBar.Value = 100; rValue.Text = Result.ToString(); }
                else { pBar.Value = 0; rValue.Text = Result.ToString(); }

            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Image pic = pictureBox2.BackgroundImage;
            CLogoManager clm = new CLogoManager(pic);
            var stream = clm.BuildLogoGetReverseBytes();
            byte[] Buffer = new byte[32];
            string yaz = "";
            textBox1.Clear();
                if (stream.Count == 640)
                {
                    for (int i = 0; i < stream.Count; i += 32)
                    //for (int i = 640; i > 0; i -= 32)
                    {
                        Application.DoEvents();
                        progressBar1.Value = ( i / 32);
                        //stream.GetRange(i, 32).ToArray();
                        //Array.Copy(stream.GetRange(i, 32).ToArray(), 0, Buffer, 0, 32);
                        Buffer = stream.GetRange(i, 32).ToArray();

                            for (int j = 0; j < 32; j++)
                            {
                                yaz = yaz + "," + Buffer[j];
                            }
                            textBox1.Text = yaz + "/r/n";

                        //this.Text = (i / 32).ToString();
                       /* Result = fsm.SaveLogoImage(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)(i / 32), stream.GetRange(i, 32).ToArray(), Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (Result == ConnectionManager.ReturnValues.Succesfull)
                        {
                            return;
                        }*/
                    }
                }
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            DateTime PersonClockEn = new DateTime();
            DateTime PersonClockEx = new DateTime();
            PersonClockEn = EntryDateTime.Value;
            PersonClockEx = ExitDateTime.Value;
            try
            {
                Result = fsm.EraseAPerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PrsnID.Text), PersonClockEn, PersonClockEx, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        int Count = 0; int Count1 = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true; string IpAddress;
            IpAddress = cmbIPs.Text;
           // getConfigToolStripMenuItem.PerformClick();

            //toolStripMenuItem1.PerformClick();/*  timer1.Enabled = true;
            pBar.Value = 0; this.Refresh(); 
            lblSuccesfull.Text = "";
            
                try
                {
                   // Result = fsm.DeviceTestConnection(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);

                   /* Result = fsm.ChangeConfigParameters(cmbIPs.Text,
                                                           Convert.ToInt32(textPort.Text),
                                                           Convert.ToInt32(textAddress.Text),
                                                           cfg.GetFsmConfig(), IpAddress,
                                                           Convert.ToInt32(textTimeOut.Text), Cnv); */
                    Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, 30, ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Count++;
                        pBar.Value = 100;
                        rValue.Text = Result.ToString();
                        lblSuccesfull.Text = Count.ToString() + "/" + Count1.ToString();
                    }
                    else
                    {
                        Count1++;
                        pBar.Value = 0;
                        rValue.Text = Result.ToString();
                        lblSuccesfull.Text = Count.ToString() + "/" + Count1.ToString();
                    }
                    
                }
                catch (Exception ex)
                {
                    pBar.Value = 0;
                    rValue.Text = ex.ToString();
               }

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button4.PerformClick();
            //toolStripMenuItem1.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
           Count = 0; Count1 = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Result = fsm.ConverterReset(cmbIPs.Text);//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    lblSuccesfull.Text = Count.ToString() + "/" + Count1.ToString();
                }
                else
                {
                    Count1++;
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                    lblSuccesfull.Text = Count.ToString() + "/" + Count1.ToString();
                }

            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void LogIndx_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }
       
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbDevices.Text != "")
                {
                    stri++;
                    Thread threadOnline = new Thread(new ThreadStart(ListenOnline));
                    if (stri == 1)
                    {
                        button7.Text = "Online Stop";
                        threadOnline.Start();//ListenOnline();
                    }
                    else
                    {
                        stri = 0;
                        button7.Text = "Online Start";
                        threadOnline.Suspend();
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rtbDebug.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            string IpAddress;
            IpAddress = cmbIPs.Text;

            try
            {
                Result = fsm.ChangeClimaConfig(cmbIPs.Text,
                                                    Convert.ToInt32(textPort.Text),
                                                    Convert.ToInt32(textAddress.Text),
                                                    clm.GetClmConfig(),// IpAddress,
                                                    Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }


            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            FsmClmInfo cfgi = null;

            try
            {

                Result = fsm.GetClimaConfig(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out cfgi, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    clm = cfgi;
                    pgClmConfig.SelectedObjects = new object[] { clm };
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

     }
    }

