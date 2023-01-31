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
using FSM_Authorization;

namespace FSM_Authorization_2v82
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
        int CnvIndex = 0;
        string[,] nIP = new string[100, 2];

        public Form1()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-tr");
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
            // pgDvcInfo.SelectedObjects = new object[] { dvc };

            //textTimeOut.Text = setting.Timeout;
            textAddress.Text = setting.Address;

            if (textTimeOut.Text == "") textTimeOut.Text = "300";
            if (textAddress.Text == "") textAddress.Text = "34";

            btnList.PerformClick();

            tcbCommVersion.SelectedIndex = 0;
            fsm.CommVersion = false;

            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;

            cb_baudrate.DataSource = Enum.GetValues(typeof(FSM_Authorization.ConnectionManager.BaudRate)); 

        }

        #region Find Converters
        rfMultiLibrary.PhysicalCommunication tactibbo = new rfMultiLibrary.PhysicalCommunication();
        rfMultiLibrary.PhysicalCommunication.ReturnValues rvalue;
        public string[] Macs, IPs, GWs, SMs, Ports, Bauds, Pars, Datas, Stops, Flows, Names, Protocols;

        private void btnList_Click(object sender, EventArgs e)
        {
            rfMultiLibrary.DeviceSettings[] Dvc; CnvIndex = 0;
            cmbIPs.Items.Clear(); nIP = new string[100, 2]; string[] sIP = new string[100];
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
                        nIP[CnvIndex, 0] = IPs[i];
                        nIP[CnvIndex, 1] = "T"; //converter tipine göre converter secme
                        sIP[CnvIndex] = IPs[i];
                        CnvIndex++;
                    }
                }
                rfMultiLibrary.PhysicalCommunication.ReturnValues Getlcldvc;
                Getlcldvc = tactibbo.GetLocalDevices(out Dvc);
                if (Getlcldvc == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                {
                    for (int i = 0; i < Dvc.Length; i++)
                    {
                        nIP[CnvIndex, 0] = Dvc[i].d_IP;
                        nIP[CnvIndex, 1] = "C"; //converter tipine göre converter secme
                        sIP[CnvIndex] = Dvc[i].d_IP;
                        CnvIndex++;
                    }
                }
                string[] oIP = new string[Macs.Length + Dvc.Length];
                Array.Copy(sIP, oIP, (Macs.Length + Dvc.Length));
                Array.Sort(oIP);
                //string[] oIP = new string[Macs.Length];
                //Array.Copy(sIP, oIP, (Macs.Length));
                //Array.Sort(oIP);

                for (int i = 0; i < Macs.Length + +Dvc.Length; i++) //ip sıralaması
                {
                    cmbIPs.Items.Add(oIP[i]);
                    //if (oIP[i] == setting.IP)
                    //{
                    //    cmbIPs.SelectedIndex = i;                  
                    //    return;
                    //}
                }

                if (cmbIPs.Items.Count > 0)
                {
                    rValue.Text = cmbIPs.Items.Count.ToString() + " Converter Bulundu";
                    pBar.Value = 100;
                    cmbIPs.SelectedIndex = 0;
                    for (int i = 0; i < CnvIndex; i++)
                    {
                        if (cmbIPs.Text == nIP[i, 0])
                            if (nIP[i, 1] == "T") { DoorSt.Text = "T"; Cnv = ConnectionManager.Converter.Tac; }
                            else { DoorSt.Text = "C"; Cnv = ConnectionManager.Converter.NewConv; }
                    }
                }
                else
                {
                    rValue.Text = "Converter Bulunamadı !";
                    return;
                }

                //pBar.Value = 100;
                //cmbIPs.SelectedIndex = 0;
                //if (CnvType[0] == 'T') { DoorSt.Text = "T"; Cnv = ConnectionManager.Converter.Tac; }
                //else { DoorSt.Text = "C"; Cnv = ConnectionManager.Converter.Cezeri; }

            }
            catch (Exception ex)
            {
                Debug(DbgMsgType.Outgoing, ex.ToString(), true);
            }
        }

        private void cmbIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < CnvIndex; i++)
            {
                if (cmbIPs.Text == nIP[i, 0])
                    if (nIP[i, 1] == "T") { DoorSt.Text = "T"; Cnv = ConnectionManager.Converter.Tac; }
                    else { DoorSt.Text = "C"; Cnv = ConnectionManager.Converter.NewConv; }
            }
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
            if (stri == 1) MessageBox.Show("Önce Online Kapatmanız Gerekiyor !");
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.SendAccess(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtAcsTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            int[] Targs; int[] Targ;
            lbDevices.Items.Clear();
            rValue.Text = "Bekleyin"; pBar.Value = 0; this.Refresh();

            Result = fsm.GetFsmDevices(cmbIPs.Text, Convert.ToInt32(textPort.Text), out Targs, out Targ, Convert.ToInt32(textTimeOut.Text), Cnv);

            if (Targs == null) Targs = new int[0];

            for (int i = 0; i < Targs.Length; i++)
                lbDevices.Items.Add(Targs[i].ToString() + Convert.ToChar(Targ[i]).ToString());

            if (Targs.Length > 0)
            {
                if (Targ[0] == 82)
                    GetSmartDeviceInfo();
                else
                {
                    GetDeviceInfo();
                    getConfigToolStripMenuItem.PerformClick();
                }
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
            string Manufacturer, Device, Applicatin, PcbVer, FirmVer, Tester, Serial;
            DateTime PrdDate, TestDate;

            pBar.Value = 0;
            try
            {
                Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    out Manufacturer,
                    out Device,
                    out Applicatin,
                    out PcbVer,
                    out PrdDate,
                    out TestDate,
                    out FirmVer,
                    out Tester,
                    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);

                //pgDvcInfo.SelectedObjects = new object[] { fsmInfo };

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rtbDebug.Text = "";
                    if (Manufacturer == "BARKODES")
                    {
                        int firmversion;

                        try
                        {
                            string fr = FirmVer.Substring(3, 1);
                            if(Convert.ToChar(fr) >= 'A')
                                firmversion = Convert.ToInt32(FirmVer.Replace(".", "").Replace(fr, "0"));
                            else firmversion = Convert.ToInt32(FirmVer.Replace(".", ""));
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        
                        if (firmversion >= 500)
                        {
                            tcbCommVersion.SelectedIndex = 1;
                            fsm.CommVersion = true;
                        }
                        else
                        {
                            tcbCommVersion.SelectedIndex = 0;
                            fsm.CommVersion = false;
                        }

                        fsmInfo.SetFsmInfo(Manufacturer, Device, Applicatin, PcbVer, PrdDate, TestDate, FirmVer, Tester, Serial);
                        //Debug(DbgMsgType.Normal, "Manufactr:", false); Debug(DbgMsgType.Outgoing, Manufacturer, false);
                        Debug(DbgMsgType.Normal, "Name: " + Device + " Type: " + Applicatin + " Pcb Ver: " + PcbVer + " Frm Ver: " + FirmVer + " Tester: " + Tester + " TDate: " + TestDate.ToShortDateString() + " Serial: " + Serial + "\r\n", false);
                        ////Debug(DbgMsgType.Normal, "Manufactr:", false); Debug(DbgMsgType.Outgoing, Manufacturer, false);
                        //Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                        //Debug(DbgMsgType.Normal, "   Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                        //Debug(DbgMsgType.Normal, "   Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer, false);
                        //Debug(DbgMsgType.Normal, "   Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer, false);
                        //Debug(DbgMsgType.Normal, "   Tester:", false); Debug(DbgMsgType.Outgoing, Tester, false);
                        ////Debug(DbgMsgType.Normal, "   PDate:", false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                        //Debug(DbgMsgType.Normal, "   TDate:", false); Debug(DbgMsgType.Outgoing, TestDate.ToShortDateString(), false);
                        //Debug(DbgMsgType.Normal, "   Serial:", false); Debug(DbgMsgType.Outgoing, Serial + "\r\n", false);
                    }
                    else
                    {
                        fsm.CommVersion = false;
                        tcbCommVersion.SelectedIndex = 0;
                        fsmInfo.SetFsmInfo(Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);
                        PcbVer = PcbVer.Substring(0, 2) + "R" + PcbVer.Substring(2, 1);
                        FirmVer = FirmVer.Substring(0, 1) + "." + FirmVer.Substring(1, 2);
                        Debug(DbgMsgType.Normal, "Name: " + Device + " Type: " + Applicatin + " Pcb Ver: " + PcbVer + " Frm Ver: " + FirmVer + " Tester: " + Tester + " TDate: " + TestDate.ToShortDateString() + " Serial: " + Serial + "\r\n", false);
                        //Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                        //Debug(DbgMsgType.Normal, "   Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                        //Debug(DbgMsgType.Normal, "   Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer.Substring(0, 2) + "R" + PcbVer.Substring(2, 1), false);
                        //Debug(DbgMsgType.Normal, "   Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer.Substring(0, 1) + "." + FirmVer.Substring(1, 2), false);
                        //Debug(DbgMsgType.Normal, "   Tester:", false); Debug(DbgMsgType.Outgoing, Tester, false);
                        //Debug(DbgMsgType.Normal, "   Date:", false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                        //Debug(DbgMsgType.Normal, "   Serial:", false); Debug(DbgMsgType.Outgoing, Serial + "\r\n", false);
                    }
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
                //pgDvcInfo.SelectedObjects = new object[] { fsmInfo };

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rtbDebug.Text = "";
                    tcbCommVersion.SelectedIndex = 0;
                    fsm.CommVersion = false;
                    Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                    Debug(DbgMsgType.Normal, "   Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                    Debug(DbgMsgType.Normal, "   Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer, false);
                    Debug(DbgMsgType.Normal, "   Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer, false);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            try
            {
                string DvcAdr;
                DvcAdr = lbDevices.SelectedItem.ToString();
                if (DvcAdr.Substring(1, 1) == "R" || DvcAdr.Substring(1, 1) == " ")
                {
                    if (DvcAdr.Substring(1, 1) == "R")
                    {
                        textAddress.Text = DvcAdr.Substring(0, 1).ToString();
                        GetSmartDeviceInfo();
                    }
                    else
                    {
                        textAddress.Text = DvcAdr.ToString();
                        pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";
                        GetDeviceInfo();
                        getConfigToolStripMenuItem.PerformClick();
                    }
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
                            GetDeviceInfo();
                            getConfigToolStripMenuItem.PerformClick();                         
                        }
                    }
                    else
                    {
                        textAddress.Text = DvcAdr.ToString();
                        pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";                      
                        GetDeviceInfo();
                        getConfigToolStripMenuItem.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string IpAddress;
            IpAddress = cmbIPs.Text;

            try
            {
                string type = lbDevices.SelectedItem.ToString();

                if (type.Substring(type.Length - 1) == "R")
                {
                    Result = fsm.ChangeSmartRelayConfigParameters(cmbIPs.Text,
                                                        Convert.ToInt32(textPort.Text),
                                                        Convert.ToInt32(textAddress.Text),
                                                        fsmInfo.GetSmartRelayCfg(),
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
                else
                {
                    string sfw = fsmInfo.FirmVer.Replace(".", "");
                    uint fw = Convert.ToUInt32(sfw);
                    if (fw >= 500) //v5.00
                    {
                        Result = fsm.ChangeConfigParameters(cmbIPs.Text,
                                Convert.ToInt32(textPort.Text),
                                Convert.ToInt32(textAddress.Text),
                                cfg.GetFsmConfig_v5(),// IpAddress,
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
                    else
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            FsmConfig cfgi = null;

            try
            {
                string sfw = fsmInfo.FirmVer.Replace(".", "");
                uint fw = Convert.ToUInt32(sfw);
                if (fw >= 500) //v5.00
                {
                    Result = fsm.GetConfigParameters_v5(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out cfgi, Convert.ToInt32(textTimeOut.Text), Cnv);
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
                }
                else
                {
                    Result = fsm.GetConfigParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out cfgi, Convert.ToInt32(textTimeOut.Text), Cnv);
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
                }
                //getDvcInfo();
            }
            catch (Exception ex)
            {

            }
        }

        private void chgBaudrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            ConnectionManager.BaudRate BaudR = ConnectionManager.BaudRate._115200;
            pBar.Value = 0;
            try
            {
                string Baud = "";
                switch (tsBaudrateTxt.Text)
                {
                    case "115200": BaudR = ConnectionManager.BaudRate._115200; break;
                    case "57600": BaudR = ConnectionManager.BaudRate._57600; break;
                    case "34800": BaudR = ConnectionManager.BaudRate._38400; break;
                    case "19200": BaudR = ConnectionManager.BaudRate._19200; break;
                    case "9600": BaudR = ConnectionManager.BaudRate._9600; break;
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
            byte[] Log;
            int begin = Convert.ToInt32(txtBegin.Text), x = 0;
            int end = Convert.ToInt32(txtEnd.Text);
            UInt64 LogID;
            DateTime LogTime;
            //string strLogTime;
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
                //Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                ConnectionManager.WorkingModes.ServiceMode,                
                //                                Convert.ToInt32(textTimeOut.Text), Cnv);

                //if (Result != ConnectionManager.ReturnValues.Successful)
                //{
                //     Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                //     return;
                //}
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (int i = begin; i < end; i++)
                {
                Again:
                    //Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out strLogTime, out AcsDir, Convert.ToInt32(textTimeOut.Text), Cnv);
                    Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + (char)9 + "FdBack:" + FBack.ToString(), true);
                        //i++;
                    }
                    else
                    {
                        x++;
                        Debug(DbgMsgType.Outgoing, (char)9 + " " + x.ToString() + " " + Result.ToString(), true);
                        if (x >= 30) { Debug(DbgMsgType.Normal, (char)9 + " index : " + i.ToString(), true); break; }
                        goto Again;
                    }
                    //Thread.Sleep(100);
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
                //Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //            ConnectionManager.WorkingModes.OfflineMode,
                //            Convert.ToInt32(textTimeOut.Text), Cnv);

            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }
        public byte stri = 0, testPort = 0;
        public byte EmergencyFlag = 0;
        public void ListenOnline(string cIP)
        {
            byte grpDoor0, grpDoor1;
            UInt64 ID; int Address; int OfflineLogCount; byte[] temp; int i = 0;
            ConnectionManager.AccessType acc;
            ConnectionManager.DoorStatus Door;
            ConnectionManager.EmergencySts Emergency;
            ConnectionManager.CardStatus cardStatus;
        AgainClient:
            TcpClient client = new TcpClient();
            fsm.PingAndPortTest(cIP, Convert.ToInt32(textPort.Text), client);

            while (true)
            {
                if (stri != 1) { client.Close(); i = 0; return; }
                if (!client.Connected)
                {
                    client.Close();
                    goto AgainClient;
                }

                if (checkBox2.Checked)
                {
                    acc = ConnectionManager.AccessType.Accept;
                    checkBox2.Text = "Accept";
                }
                else
                {
                    acc = ConnectionManager.AccessType.Deny;
                    checkBox2.Text = "Deny";
                }

                if (EmergencyFlag == 1)
                {
                    fsm.EmergencyExit_TCP(client, cmbIPs.Text, Convert.ToInt32(textPort.Text));
                    EmergencyFlag = 0;
                }
                else if (EmergencyFlag == 2)
                {
                    fsm.EmergencyExitClose_TCP(client, cmbIPs.Text, Convert.ToInt32(textPort.Text));
                    EmergencyFlag = 0;
                }

                if (chkOldElev.Checked)
                {
                    ChkBoxControl(out grpDoor0, out grpDoor1);
                    Result = fsm.ListenOnlineRequest(client, Convert.ToInt32(textAddress.Text),
                                    out ID, out Address, acc, ConnectionManager.BuzzerState.BuzzerOn, grpDoor0, grpDoor1,
                                    Convert.ToInt32(txtRelayTime.Text), out OfflineLogCount, Convert.ToInt32(textTimeOut.Text), Cnv, out temp);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        i++;
                        Debug(DbgMsgType.Outgoing, i.ToString() + "  OfLogCnt: " + OfflineLogCount.ToString() + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Case: " + Result.ToString(), true);
                        pBar.Value = 100;
                        rValue.Text = Result.ToString();
                    }
                }
                else if (chkNewElev.Checked)
                {
                    byte[] gates;
                    ChkBoxControl(out gates, 0);
                    Result = fsm.ListenOnlineRequest(client, Convert.ToInt32(textAddress.Text),
                                    out ID, out Address, acc, ConnectionManager.BuzzerState.BuzzerOn, gates,
                                    Convert.ToInt32(txtRelayTime.Text), out OfflineLogCount, Convert.ToInt32(textTimeOut.Text), Cnv, out temp);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        i++;
                        Debug(DbgMsgType.Outgoing, i.ToString() + "  OfLogCnt: " + OfflineLogCount.ToString() + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Case: " + Result.ToString(), true);
                        pBar.Value = 100;
                        rValue.Text = Result.ToString();
                    }
                }
                else
                {
                    //Result = fsm.ListenOnlineRequest(client, Convert.ToInt32(textAddress.Text),
                    //                out ID, out Address, acc, ConnectionManager.BuzzerState.BuzzerOn,
                    //                10, out OfflineLogCount, Convert.ToInt32(textTimeOut.Text), Cnv, out temp);
                    
                    Result = fsm.ListenOnlineRequest(out ID, out Address, out OfflineLogCount, out Emergency, out Door, out cardStatus, client, Convert.ToInt32(textTimeOut.Text), Cnv, out temp);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        i++;
                        if (ID != 0 && cardStatus == ConnectionManager.CardStatus.Undefine) // i.ToString() + 
                            Debug(DbgMsgType.Outgoing, " Zaman: " + DateTime.Now.ToLongTimeString() + " IP: " + cIP + "   " + "OfLogCnt: " + OfflineLogCount.ToString() + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Case: " + Result.ToString(), true);
                        else if (ID != 0 && cardStatus != ConnectionManager.CardStatus.Undefine) // i.ToString() + 
                            Debug(DbgMsgType.Outgoing, " Zaman: " + DateTime.Now.ToLongTimeString() + " IP: " + cIP + "   " + "Adr: " + Address + "   " + "ID: " + ID + "   " + "Card: " + cardStatus + "   " + "Case: " + Result.ToString(), true);
                        else if (ID == 0 && Emergency == ConnectionManager.EmergencySts.NotActive)
                            Debug(DbgMsgType.Outgoing, "Adr: " + Address + "   " + "DoorSts: " + Door.ToString() + "   " + "Case: " + Result.ToString(), true);
                        else
                            Debug(DbgMsgType.Outgoing, "Adr: " + Address + "   " + "EmergencySts: " + Emergency.ToString() + "   " + "Case: " + Result.ToString(), true);

                        if (cardStatus == ConnectionManager.CardStatus.Undefine)
                        {
                            

                            Result = fsm.Access(client, Address, acc, Convert.ToInt32(txtRelayTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);                           
                        }
                        else
                        {
                            String aa = txtRelayTime.Text; //10
                            String bb = textTimeOut.Text; //1000
                            Result = fsm.CardResponse(client, Address, cardStatus, Convert.ToInt32(textTimeOut.Text), Cnv);
                        }

                        if (Result == ConnectionManager.ReturnValues.Successful)
                        {
                            pBar.Value = 100;
                            rValue.Text = Result.ToString();
                        }
                    }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                //Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                ConnectionManager.WorkingModes.ServiceMode,
                //                                Convert.ToInt32(textTimeOut.Text), Cnv);

                //if (Result != ConnectionManager.ReturnValues.Successful)
                //{
                //    Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                //    return;
                //}

                Result = fsm.RecordAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), PersonName, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, txtID.Text + (char)9 + "Added", true);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    pBar.Value = 0;
                    rValue.Text = Result.ToString();
                }

                //Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //            ConnectionManager.WorkingModes.OfflineMode,
                //            Convert.ToInt32(textTimeOut.Text), Cnv);
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void btnErsPer_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            try
            {
                Result = fsm.EraseAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, txtID.Text + (char)9 + "Erased", true);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    Debug(DbgMsgType.Outgoing, txtID.Text + (char)9 + "Not Found", true);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            long LogIndex, PerIndex, BlackIndex, ErasedPerIndex, EventIndex, Spare1Log, Spare2Log;
            pBar.Value = 0; this.Refresh();
            try
            {
                //Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //                                            out PerIndex,
                //                                            out LogIndex,
                //                                            out BlackIndex,
                //                                            out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    rtbDebug.Text = "Log Indx: " + LogIndex.ToString() + "     Pers Indx: " + PerIndex.ToString() + "     Black Lst Indx: " + BlackIndex.ToString() + "     Ersade Per Indx: " + ErasedPerIndex.ToString() +
                        "     EventIndex: " + EventIndex.ToString() + "     Sp1Log: " + Spare1Log.ToString() + "     Sp2Log: " + Spare2Log.ToString();
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            rtbDebug.Text = "";
            pBar.Value = 0;
            UInt32 PersonIndex = 0;
            try
            {
                Result = fsm.FindPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtPeronID.Text), out PersonIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "Index = " + PersonIndex.ToString(), false);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
                        string PerName;
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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

        private void btnSendAccess_Click_1(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                Result = fsm.SendAccess(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtRelayTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex; byte[] Packet; UInt64 PersonID = 0; UInt32 nPersonID = 0;
            pBar.Value = 0; rtbDebug.Text = "";

            int StrIndex = Convert.ToInt32(txtFrom.Text);
            int EndIndex = Convert.ToInt32(txtTo.Text);

            try
            {
                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text),
                                                    Convert.ToInt32(textAddress.Text),
                                                    out PerIndex,
                                                    out LogIndex,
                                                    out BlackIndex,
                                                    out ErasedPerIndex,
                                                    Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (EndIndex > PerIndex)
                        EndIndex = PerIndex - 1;
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


                //Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out PerIndex, out LogIndex, out BlackIndex, out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                //if (Result == ConnectionManager.ReturnValues.Successful)
                //{
                //    int StrIndex = Convert.ToInt32(txtFrom.Text);
                //    int EndIndex = Convert.ToInt32(txtTo.Text);
                //    if (EndIndex > PerIndex) EndIndex = PerIndex - 1;
                //--------------------------------------------------------------------------------
                bool firmversion = false;
                for (int i = StrIndex; i <= EndIndex; i++)
                {
                    ConnectionManager.PersonState state = ConnectionManager.PersonState.Active;
                    if (!firmversion)
                     Result = fsm.GetPersonCardID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out PersonID, out state, Convert.ToInt32(textTimeOut.Text), Cnv);

                    if (Result == ConnectionManager.ReturnValues.PersonIndexOvf || firmversion)
                    {
                        Result = fsm.GetPersonID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Packet, out nPersonID, Convert.ToInt32(textTimeOut.Text), Cnv);
                        firmversion = true;
                    }

                    if (firmversion)
                    {
                        string StrLodID = nPersonID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "    ID:  " + bosluk + nPersonID.ToString(), true);
                    }else
                    {
                        string StrLodID = PersonID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "    ID:  " + bosluk + PersonID.ToString() + "\tState: " + state.ToString(), true);
                    }
                }
                //--------------------------------------------------------------------------------
                //    pBar.Value = 100;
                //    rValue.Text = Result.ToString();
                //}
                //else
                //{
                //    pBar.Value = 0;
                //    rValue.Text = Result.ToString();
                //}


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
                    int y = 0;
                    if (x == 1) y = 19;
                    if (x == 2) y = 39;

                    if (stream.Count == 640)
                    {
                        for (int i = 0; i < stream.Count; i += 32)
                        //for (int i = 640; i > 0; i -= 32)
                        {
                            Application.DoEvents();
                            progressBar1.Value = (y + i / 32);
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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

        int PCount, StopCnt;
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
                PerNameTxt.Text = PerNameTxt.Text.Substring(0, 5) + Convert.ToString(i);

                Result = fsm.RecordAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PerID.Text), PersonName, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Bar2.Value = i;
                    label6.Text = PerCount.Text + " / " + Convert.ToString(i + 1);//Result.ToString();
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

                Result = fsm.RecordAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt32(PerID.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
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
            threadID.Abort();
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

        Thread threadID;
        private void PerIDSave_Click(object sender, EventArgs e)
        {

            Bar2.Value = 0;
            StopCnt = 0;
            string PersonName;
            PersonName = txtPerName.Text;
            PCount = Convert.ToInt32(PerCount.Text);
            Bar2.Maximum = PCount;

            try
            {
                System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
                threadID = new Thread(new ThreadStart(RecordPersonID));
                threadID.Start();
                //Thread threadName = new Thread(new ThreadStart(RecordPersonIDName));
                //Thread threadMain = new Thread(new ThreadStart(RecordMain));

                //threadMain.Start();
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            byte[] Packet; uint PersonID; pBar.Value = 0; rtbDebug.Text = "";

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
                String PerName; int PersonIndex = 0, ChgInnx = 0;
                DateTime StartDate, EndDate;
                for (int y = 0; y < 20; )
                {
                    Result = fsm.GetFindPersonID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), ChgInnx, out  PersonIndex, out Packet, out PersonID, out PerName, out StartDate, out EndDate, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, (y + 1).ToString() + "   Idx: " + PersonIndex.ToString() + ":   ST: " + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "   ID:  " + PersonID.ToString() + "   PN: " + PerName + "   ET: " + EndDate.ToString("yyyy-MM-dd HH:mm:ss"), true);
                        y++;
                        ChgInnx = PersonIndex + 1;
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            string Manufacturer, Device, Applicatin, PcbVer, FirmVer, Tester, Serial;
            DateTime PrdDate, TestDate;

            pBar.Value = 0;
            try
            {
                Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    out Manufacturer,
                    out Device,
                    out Applicatin,
                    out PcbVer,
                    out PrdDate,
                    out TestDate,
                    out FirmVer,
                    out Tester,
                    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);


                fsmInfo.SetFsmInfo(Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    //pgDvcInfo.SelectedObjects = new object[] { fsmInfo };
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            Image pic = pictureBox2.BackgroundImage;
            CLogoManager clm = new CLogoManager(pic);
            var stream = clm.BuildLogoGetReverseBytes();
            byte[] Buffer = new byte[32];
            string yaz = "";
            if (stream.Count == 640)
            {
                for (int i = 0; i < stream.Count; i += 32)
                //for (int i = 640; i > 0; i -= 32)
                {
                    Application.DoEvents();
                    progressBar1.Value = (i / 32);
                    //stream.GetRange(i, 32).ToArray();
                    //Array.Copy(stream.GetRange(i, 32).ToArray(), 0, Buffer, 0, 32);
                    Buffer = stream.GetRange(i, 32).ToArray();

                    for (int j = 0; j < 32; j++)
                    {
                        yaz = yaz + "," + Buffer[j];
                    }
                    Debug(DbgMsgType.Outgoing, yaz.ToString(), true);
                    yaz = "";


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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            timer1.Enabled = true; string IpAddress;
            IpAddress = cmbIPs.Text;
            // getConfigToolStripMenuItem.PerformClick();

            //toolStripMenuItem1.PerformClick();/*  timer1.Enabled = true;
            pBar.Value = 0; this.Refresh();

            try
            {
                // Result = fsm.DeviceTestConnection(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);

                /* Result = fsm.ChangeConfigParameters(cmbIPs.Text,
                                                        Convert.ToInt32(textPort.Text),
                                                        Convert.ToInt32(textAddress.Text),
                                                        cfg.GetFsmConfig(), IpAddress,
                                                        Convert.ToInt32(textTimeOut.Text), Cnv); */
                Result = fsm.SendAccess(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, 30, ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    Count1++;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Count = 0; Count1 = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                Result = fsm.ConverterReset(cmbIPs.Text);//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    Count1++;
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

        private void LogIndx_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }

        private void btnAddAuthor_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0; byte grpDoor0, grpDoor1;
            try
            {
                if (ckbMethod.Checked)
                {
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.WorkingModes.ServiceMode,
                                Convert.ToInt32(textTimeOut.Text), Cnv);

                    if (Result != ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true);
                        return;
                    }

                    ChkBoxControl(out grpDoor0, out grpDoor1);
                    Result = fsm.RecordAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtIDL.Text), grpDoor0, grpDoor1, Convert.ToInt32(textTimeOut.Text), Cnv);
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


                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.WorkingModes.OfflineMode,
                                Convert.ToInt32(textTimeOut.Text), Cnv);
                }
                else
                {
                    byte[] gates;
                    ChkBoxControl(out gates, 0);
                    Result = fsm.RecordAPersonNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtIDL.Text), 10, gates, Convert.ToInt32(textTimeOut.Text), Cnv);
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

            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        public void ChkBoxControl(out byte door0, out byte door1)
        {
            door0 = 0; door1 = 0; byte cnt = 0;

            if (cbBox8.Checked) cnt = (byte)(cnt + 128);
            if (cbBox7.Checked) cnt = (byte)(cnt + 64);
            if (cbBox6.Checked) cnt = (byte)(cnt + 32);
            if (cbBox5.Checked) cnt = (byte)(cnt + 16);
            if (cbBox4.Checked) cnt = (byte)(cnt + 8);
            if (cbBox3.Checked) cnt = (byte)(cnt + 4);
            if (cbBox2.Checked) cnt = (byte)(cnt + 2);
            if (cbBox1.Checked) cnt = (byte)(cnt + 1);
            door0 = cnt;
            cnt = 0;
            if (cbBox0.Checked) cnt = (byte)(cnt + 128);
            if (cbBoxF.Checked) cnt = (byte)(cnt + 64);
            if (cbBoxE.Checked) cnt = (byte)(cnt + 32);
            if (cbBoxD.Checked) cnt = (byte)(cnt + 16);
            if (cbBoxC.Checked) cnt = (byte)(cnt + 8);
            if (cbBoxB.Checked) cnt = (byte)(cnt + 4);
            if (cbBoxA.Checked) cnt = (byte)(cnt + 2);
            if (cbBox9.Checked) cnt = (byte)(cnt + 1);
            door1 = cnt;
        }

        public void ChkBoxControl(out byte[] gates, byte type)
        {
            gates = null; //byte cnt = 0;
            gates = new byte[16];

            CheckBox[] cBox = new CheckBox[128] {
                cbBox1, cbBox2, cbBox3, cbBox4, cbBox5, cbBox6, cbBox7, cbBox8,
                cbBox9, cbBoxA, cbBoxB, cbBoxC, cbBoxD, cbBoxE, cbBoxF, cbBox0,
                checkBox5, checkBox6, checkBox7, checkBox9, checkBox10,checkBox12,checkBox14,checkBox16,
                checkBox18,checkBox20,checkBox19,checkBox17,checkBox8,checkBox15,checkBox13,checkBox11,
                checkBox21,checkBox22,checkBox23,checkBox25,checkBox26,checkBox28,checkBox30,checkBox32,
                checkBox34,checkBox36,checkBox35,checkBox33,checkBox24,checkBox31,checkBox29,checkBox27,
                checkBox37,checkBox38,checkBox39,checkBox41,checkBox42,checkBox44,checkBox46,checkBox48,
                checkBox50,checkBox52,checkBox51,checkBox49,checkBox40,checkBox47,checkBox45,checkBox43,
                checkBox53,checkBox54,checkBox55,checkBox57,checkBox58,checkBox60,checkBox62,checkBox64,
                checkBox66,checkBox68,checkBox67,checkBox65,checkBox56,checkBox63,checkBox61,checkBox59,
                checkBox69,checkBox70,checkBox71,checkBox73,checkBox74,checkBox76,checkBox78,checkBox80,
                checkBox82,checkBox84,checkBox83,checkBox81,checkBox72,checkBox79,checkBox77,checkBox75,
                checkBox85,checkBox86,checkBox87,checkBox89,checkBox90,checkBox92,checkBox94,checkBox96,
                checkBox98,checkBox100,checkBox99,checkBox97,checkBox88,checkBox95,checkBox93,checkBox91,
                checkBox101,checkBox102,checkBox103,checkBox105,checkBox106,checkBox108,checkBox110,checkBox112,
                checkBox114,checkBox116,checkBox115,checkBox113,checkBox104,checkBox111,checkBox109,checkBox107
            };

            if (type == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    byte gate = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        if (cBox[(i * 8) + x].Checked)
                        {
                            byte g = (byte)(1 << x);
                            gate |= g;
                        }
                    }
                    gates[i] = gate;
                }
            }
            else if(type == 1)
            {
                for (int i = 0; i < 128; i++)
                    cBox[i].CheckState = CheckState.Checked;
            }
            else if(type == 2)
            {
                for (int i = 0; i < 128; i++)
                    cBox[i].CheckState = CheckState.Unchecked;
            }

            //if (cbBox8.Checked) cnt = (byte)(cnt + 128);
            //if (cbBox7.Checked) cnt = (byte)(cnt + 64);
            //if (cbBox6.Checked) cnt = (byte)(cnt + 32);
            //if (cbBox5.Checked) cnt = (byte)(cnt + 16);
            //if (cbBox4.Checked) cnt = (byte)(cnt + 8);
            //if (cbBox3.Checked) cnt = (byte)(cnt + 4);
            //if (cbBox2.Checked) cnt = (byte)(cnt + 2);
            //if (cbBox1.Checked) cnt = (byte)(cnt + 1);
            //gates[0] = cnt;
            //cnt = 0;
            //if (cbBox0.Checked) cnt = (byte)(cnt + 128);
            //if (cbBoxF.Checked) cnt = (byte)(cnt + 64);
            //if (cbBoxE.Checked) cnt = (byte)(cnt + 32);
            //if (cbBoxD.Checked) cnt = (byte)(cnt + 16);
            //if (cbBoxC.Checked) cnt = (byte)(cnt + 8);
            //if (cbBoxB.Checked) cnt = (byte)(cnt + 4);
            //if (cbBoxA.Checked) cnt = (byte)(cnt + 2);
            //if (cbBox9.Checked) cnt = (byte)(cnt + 1);
            //gates[1] = cnt;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte[] gate;
            ChkBoxControl(out gate, 2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte[] gate;
            ChkBoxControl(out gate, 1);
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sendAccessLiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0; byte grpDoor0, grpDoor1;
            try
            {
                ChkBoxControl(out grpDoor0, out grpDoor1);

                Result = fsm.Access(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, 30, ConnectionManager.BuzzerState.BuzzerOn, grpDoor0, grpDoor1, Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void saatAyarlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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

        private void saatGetirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Logo_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {

        }

        private void getLogModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
            byte[] Log;
            int begin = Convert.ToInt32(txtBegin.Text), x = 0;
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

                //------------------------------------------------------------------------------------------------------------------
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                ConnectionManager.WorkingModes.ServiceMode,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "****** Cihaz SERVİS Moduna Alındı *******", true);
                }
                else
                {
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
                    Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true); return;
                }
                //------------------------------------------------------------------------------------------------------------------
                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (int i = begin; i < end; )
                {
                    Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "Fback: " + FBack.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString() + "\t" +
                        fsm.timebuf[2] + "/" + fsm.timebuf[1] + "/" + (fsm.timebuf[0] + 2000) + "  " + fsm.timebuf[3] + ":" + fsm.timebuf[4] + ":" + fsm.timebuf[5], true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);

                //------------------------------------------------------------------------------------------------------------------
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                            ConnectionManager.WorkingModes.OfflineMode,
                            Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "****** Cihaz OFFLINE Moduna Alındı *******", true);
                }
                //------------------------------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                Result = fsm.ConverterReset(cmbIPs.Text, Convert.ToInt32(textPort.Text));//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    Count1++;
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

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                Result = fsm.EmergencyExit_TCP(cmbIPs.Text, Convert.ToInt32(textPort.Text));//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    Count1++;
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

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Result = fsm.EmergencyExitClose_TCP(cmbIPs.Text, Convert.ToInt32(textPort.Text));//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Count++;
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                }
                else
                {
                    Count1++;
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

        public bool DoorFlag = true;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (stri == 1)
                {
                    if (DoorFlag) { EmergencyFlag = 1; DoorFlag = false; }
                    else { EmergencyFlag = 2; DoorFlag = true; }
                }
                else
                {
                    //if (DoorFlag) { DoorSt.Text = "C"; DoorFlag = false; Cnv = ConnectionManager.Converter.Cezeri; }
                    //else { DoorSt.Text = "T";  DoorFlag = true; Cnv = ConnectionManager.Converter.Tac; }

                    //if (DoorFlag)
                    //{
                    //    DoorFlag = false;
                    //    Result = fsm.EmergencyExit_TCP(cmbIPs.Text, Convert.ToInt32(textPort.Text));//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                    //    if (Result == ConnectionManager.ReturnValues.Successful)
                    //    {
                    //        pBar.Value = 100;
                    //        rValue.Text = Result.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    DoorFlag = true;
                    //    Result = fsm.EmergencyExitClose_TCP(cmbIPs.Text, Convert.ToInt32(textPort.Text));//, Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                    //    if (Result == ConnectionManager.ReturnValues.Successful)
                    //    {
                    //        pBar.Value = 100;
                    //        rValue.Text = Result.ToString();
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void getTimeUdpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            DateTime Time = new DateTime(); pBar.Value = 0;
            try
            {

                // Result = fsm.GetDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out Time, Convert.ToInt32(textTimeOut.Text), Cnv);
                Result = fsm.GetDateTimeUdp(cmbIPs.Text, 1311, Convert.ToInt32(textAddress.Text), out Time, Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void toolStripComboBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //if (lbDevices.Text != "")
                //{
                testPort++;
                Thread[] threadOnline = new Thread[1];

                threadOnline[0] = new Thread(() => TestPort1());
                //threadOnline[1] = new Thread(() => TestPort2());
                //threadOnline[2] = new Thread(() => TestPort3());
                //threadOnline[3] = new Thread(() => TestPort4());

                if (testPort == 1)
                {
                    button8.Text = "Con. Port Test -> Stop";
                    threadOnline[0].Start();//ListenOnline();
                    //threadOnline[1].Start();
                    //threadOnline[2].Start();
                    //threadOnline[3].Start();
                }
                else
                {
                    testPort = 0;
                    button8.Text = "Con. Port Test -> Start";
                    threadOnline[0].Suspend();
                    //threadOnline[1].Suspend();
                    //threadOnline[2].Suspend();
                    //threadOnline[3].Suspend();
                }

            }
            catch (Exception ex)
            { }
        }

        private void TestPort1()
        {
            while (true)
            {
                int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
                pBar.Value = 0; this.Refresh();
                if (testPort != 1) break;
                Debug(DbgMsgType.Warning, DateTime.Now.ToString() + "\r\n", false);
                try
                {
                    Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Normal, " --> Log Indx: " + LogIndex.ToString() + "     Pers Indx: " + PerIndex.ToString() + "     Black Lst Indx: " + BlackIndex.ToString() + "     Ersade Per Indx: " + ErasedPerIndex.ToString() + "\r\n", false);
                    }
                    else
                    {
                        Debug(DbgMsgType.Warning, " --> Index: Failed\r\n", false);
                    }
                }
                catch (Exception ex)
                {
                    pBar.Value = 0;
                    rValue.Text = ex.ToString();
                }
                Thread.Sleep(500);

                try
                {
                    Result = fsm.SendAccess(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), ConnectionManager.AccessType.Accept, Convert.ToInt32(txtRelayTime.Text), ConnectionManager.BuzzerState.BuzzerOn, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Normal, " --> Access: Successfuul\r\n", false);
                    }
                    else
                    {
                        Debug(DbgMsgType.Warning, " --> Access: Failed\r\n", false);
                    }
                }

                catch (Exception ex)
                {
                    pBar.Value = 0;
                    rValue.Text = ex.ToString();
                }
                Thread.Sleep(500);

                if (TestLocator(cmbIPs.Text, 1932))
                {
                    Debug(DbgMsgType.Normal, " --> Locator: Successfuul\r\n", false);
                }
                else
                {
                    Debug(DbgMsgType.Warning, " --> Locator: Failed\r\n", false);
                }
                Thread.Sleep(500);

                if (DeviceFactory(cmbIPs.Text, 1311))
                {
                    Debug(DbgMsgType.Normal, " --> UDP: Successfuul\r\n", false);
                }
                else
                {
                    Debug(DbgMsgType.Warning, " --> UDP: Failed\r\n", false);
                }
                Thread.Sleep(500);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private bool TestLocator(string oldIP, int Port)
        {
            byte[] LocatorData = new byte[4];
            LocatorData[0] = (byte)72;
            LocatorData[1] = (byte)LocatorData.Length;
            LocatorData[2] = (byte)'T';
            LocatorData[3] = (byte)((0 - LocatorData[0] - LocatorData[1] - LocatorData[2]) & 0xff);

            //IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, Port); //Warning
            //UdpClient ucl = new UdpClient(Port);

            //ucl.Client.ReceiveTimeout = 2000;//1000
            //ucl.EnableBroadcast = true;
            //ucl.Send(LocatorData, LocatorData.Length, ipend);
            //Thread.Sleep(5);

            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(oldIP), Port); //Warning
            UdpClient ucl = new UdpClient(Port);
            ucl.Client.ReceiveTimeout = 2000;//1000
            //ucl.EnableBroadcast = true;
            ucl.Send(LocatorData, LocatorData.Length, ipend);
            Thread.Sleep(5);
            try
            {
                while (true)
                {
                    byte[] bfr = ucl.Receive(ref ipend);
                    if ((byte)bfr[0] == (byte)'T' && (byte)bfr[1] == (byte)'E' && (byte)bfr[2] == (byte)'S' && (byte)bfr[3] == (byte)'T')
                    {
                        ucl.Close();
                        return true;
                    }
                    else
                    {
                        ucl.Close();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ucl.Close();
                return false;
            }
        }

        public bool DeviceFactory(string oldIP, int UdpPort)
        {
            try
            {
                byte[] SndBuffer = new byte[7]; byte[] RcvBuf; int RcvLen;

                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)2;
                SndBuffer[2] = (byte)0x40;
                SndBuffer[3] = 0x22; // Reserve//0
                SndBuffer[SndBuffer.Length - 2] = (byte)0xAA;
                SndBuffer[SndBuffer.Length - 1] = (byte)0xBB;

                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(oldIP), UdpPort);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = 2000;
                client.Client.SendTimeout = 2000;
                client.Connect(ipend);
                if (client.Client.Connected)
                {
                    client.Send(SndBuffer, SndBuffer.Length);
                    try
                    {
                        byte[] buf = new byte[client.Client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            Thread[] threadOnline = new Thread[1];

            threadOnline[0] = new Thread(() => getlog());

            button8.Text = "Con. Port Test -> Stop";
            threadOnline[0].Start();//ListenOnline();

        }

        private void getlog()
        {

            while (true)
            {
                pBar.Value = 0;
                int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
                byte[] Log;
                int begin = Convert.ToInt32(txtBegin.Text), x = 0;
                int end = Convert.ToInt32(txtEnd.Text);
                UInt64 LogID;
                DateTime LogTime;
                string strLogTime;
                ConnectionManager.FeedBackControl FBack;
                ConnectionManager.AccessDirection AcsDir;
                rtbDebug.Text = "";
                Thread.Sleep(1000);
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

                    //------------------------------------------------------------------------------------------------------------------
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                    ConnectionManager.WorkingModes.ServiceMode,
                                                    Convert.ToInt32(textTimeOut.Text), Cnv);

                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, "****** Cihaz SERVİS Moduna Alındı *******", true);
                    }
                    else { Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true); return; }
                    //------------------------------------------------------------------------------------------------------------------

                    Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                    for (int i = begin; i < end; i++)
                    {
                        Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (Result == ConnectionManager.ReturnValues.Successful)
                        {
                            x = 0;
                            string StrLodID = LogID.ToString();
                            string bosluk = "";

                            if (StrLodID.Length <= 10)
                                bosluk = "_______";
                            else
                                bosluk = "";

                            Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "Fback: " + FBack.ToString(), true);
                        }
                    }
                    Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);

                    //------------------------------------------------------------------------------------------------------------------
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.WorkingModes.OnlineAuthorMode,
                                Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, "****** Cihaz ONLINE Moduna Alındı *******", true);
                    }
                    //------------------------------------------------------------------------------------------------------------------
                }
                catch (Exception ex)
                {
                    pBar.Value = 0;
                    rValue.Text = ex.ToString();
                }
            }
        }

        private void getDeviceInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDeviceInfo();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Result = fsm.LiftAllOpen(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                            ConnectionManager.LiftKeys.AllOpen,
                                            Convert.ToInt32(textTimeOut.Text), Cnv);
            if (Result == ConnectionManager.ReturnValues.Successful)
            {
                Debug(DbgMsgType.Outgoing, "Bütün röleler aktif", true);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Result = fsm.LiftAllOpen(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.LiftKeys.AllClose,
                                Convert.ToInt32(textTimeOut.Text), Cnv);
            if (Result == ConnectionManager.ReturnValues.Successful)
            {
                Debug(DbgMsgType.Outgoing, "Bütün röleler pasif", true);
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            timer2.Interval = (Convert.ToInt32(txtTimer.Text) * 1000);
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            getLogModeToolStripMenuItem.PerformClick();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            timer3.Interval = (Convert.ToInt32(textBox1.Text) * 1000);
            timer3.Enabled = true;
        }

        bool CancelLog = false;
        public void GetLogs()
        {
            pBar.Value = 0;
            int LogIndex, PerIndex, BlackIndex, ErasedPerIndex;
            byte[] Log;
            int begin = 0, x = 0;
            int end = 1000;
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.AccessDirection AcsDir;
            rtbDebug.Text = "";
            CancelLog = false;

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

                //------------------------------------------------------------------------------------------------------------------
                if (cbServis.Checked)
                {
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                    ConnectionManager.WorkingModes.ServiceMode,
                                                    Convert.ToInt32(textTimeOut.Text), Cnv);

                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, "****** Cihaz SERVİS Moduna Alındı *******", true);
                    }
                    else
                    {
                        Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.WorkingModes.OfflineMode,
                                Convert.ToInt32(textTimeOut.Text), Cnv);
                        Debug(DbgMsgType.Outgoing, "Mod Değiştirilemedi " + (char)9 + Result.ToString(), true); return;
                    }
                }

                //------------------------------------------------------------------------------------------------------------------
                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (int i = begin; i < end; )
                {
                    if (CancelLog)
                        return;

                    Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "Fback: " + FBack.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString(), true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 20)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);

                fsm.EraseAllLogData(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);

                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (int i = 0; i < 20; )
                {
                    if (CancelLog)
                        return;

                    Result = fsm.GetLastLogDataNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out LogID, out LogTime, out AcsDir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "Fback: " + FBack.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString(), true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 20)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);

                if (cbServis.Checked)
                {
                    Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                ConnectionManager.WorkingModes.OfflineMode,
                                Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Outgoing, "****** Cihaz OFFLINE Moduna Alındı *******", true);
                    }
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void testLogÇekmeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            CancelLog = true;
            timer3.Enabled = false;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            GetLogs();
        }

        private void getSpareLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            byte[] Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            bool part = false;
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.AccessDirection access;
            rtbDebug.Text = "";
            
            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (Spare1Log < 10000)
                    {
                        part = false;
                        if (end > Spare1Log)
                            end = Spare1Log;
                        else
                        {
                            Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                        }
                    }
                    else
                    {
                        part = true;
                        if (end > Spare2Log)
                            end = Spare2Log;
                        else
                        {
                            Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                        }
                    }
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end; )
                {
                    Result = fsm.GetAccessSpareLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, part, out Log, out LogID, out LogTime, out access, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        //Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + access.ToString() + "\tFback: " + FBack.ToString(), true);
                        rtbDebug.AppendText("\r\n" + i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + access.ToString() + "\tFback: " + FBack.ToString());
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString(), true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void getEventLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            DateTime EventTime;
            ConnectionManager.Events Event;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,                                                             
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (end > EventIndex)
                        end = EventIndex;
                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end; )
                {
                    Result = fsm.GetEventLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Event, out EventTime, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        Debug(DbgMsgType.Outgoing, i.ToString() + "\t" + "EventCode: " + Event.ToString() + (char)9 + "Time: " + EventTime.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString(), true);
                        i++;
                        if (stoplog++ >= 10)
                            break;
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void getAccessLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            byte[] Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.DenyWithReason reason;
            ConnectionManager.Direction dir;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                out PerIndex,
                                                out LogIndex,
                                                out EventIndex,
                                                out BlackIndex,
                                                out ErasedPerIndex,
                                                out Spare1Log, out Spare2Log,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (end > LogIndex)
                        end = LogIndex;
                    else
                    {
                        Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    }
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end; )
                {
                    Result = fsm.GetAccessLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (int)i, out Log, out LogID, out LogTime, out reason, out dir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        //Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "DvcTyp: " + AcsDir.ToString() + "    " + "Fback: " + FBack.ToString(), true);
                        rtbDebug.AppendText(i.ToString() + "\tID: " + bosluk + LogID.ToString() + "\tTime: " + LogTime.ToString() + "\tAccess: " + reason.ToString() + "\tDir: " + dir.ToString() + "\tFback: " + FBack.ToString() + "\r\n");
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString() + "\t" +
                        fsm.timebuf[2] + "/" + fsm.timebuf[1] + "/" + (fsm.timebuf[0] + 2000) + "  " + fsm.timebuf[3] + ":" + fsm.timebuf[4] + ":" + fsm.timebuf[5], true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void eraseAllSpareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;

            if (DialogResult.Yes != MessageBox.Show(this, "[ " + txtAddress.Text + " ]   Tüm Geçiş Bilgileri Silinecektir, Eminmisiniz?", "Silme", MessageBoxButtons.YesNo))
                return;

            try
            {
                Result = fsm.EraseAllAccessSpareLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void eraseAllEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;

            if (DialogResult.Yes != MessageBox.Show(this, "[ " + txtAddress.Text + " ]   Tüm Geçiş Bilgileri Silinecektir, Eminmisiniz?", "Silme", MessageBoxButtons.YesNo))
                return;

            try
            {
                Result = fsm.EraseAllEventLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
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

        Thread TestComm;
        private void button17_Click(object sender, EventArgs e)
        {
            TestComm = new Thread(() => commtest());
            TestComm.Start();
        }

        private void commtest()
        {
            long LogIndex, PerIndex, BlackIndex, ErasedPerIndex, EventIndex, Spare1Log, Spare2Log;
            pBar.Value = 0; this.Refresh();
            try
            {
                UInt32 i = 0, Noasnwer = 0;
                while (true)
                {
                    Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                    out PerIndex,
                                                                    out LogIndex,
                                                                    out BlackIndex,
                                                                    out ErasedPerIndex,
                                                                    out EventIndex,
                                                                    out Spare1Log, out Spare2Log,
                                                                    Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        pBar.Value = 100;
                        //rValue.Text = Result.ToString();
                        rtbDebug.AppendText((++i).ToString() + "\tLog: " + LogIndex.ToString() + "\tPers: " + PerIndex.ToString() + "\tBlack: " + BlackIndex.ToString() + "\tErsPer: " + ErasedPerIndex.ToString() +
                            "\tEvt: " + EventIndex.ToString() + "\tSp1: " + Spare1Log.ToString() + "\tSp2: " + Spare2Log.ToString() + "\r\n");
                    }
                    else
                    {
                        pBar.Value = 0;
                        rValue.Text = (++Noasnwer).ToString();//Result.ToString();
                        rtbDebug.AppendText(Result.ToString() + "\r\n");
                    }
                    Thread.Sleep(Convert.ToInt32(textBox2.Text));
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
                TestComm.Abort();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            TestComm.Abort();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                Result = fsm.SaveStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToByte(txtGC.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    int begin = dtBegin.Value.Hour * 60 + dtBegin.Value.Minute;
                    int end = dtEnd.Value.Hour * 60 + dtEnd.Value.Minute;
                    CheckBox[] chb = new CheckBox[7] { cb_Sun, cb_Mon, cb_Tue, cb_Wed, cb_Thu, cb_Fri, cb_Sat };
                    for (byte i = 0; i < 7; i++)
                    {
                        if (chb[i].Checked)
                            break;
                        if (i == 6)
                        {
                            Debug(DbgMsgType.Normal, "Lütfen gün seçiniz.", true);
                            return;
                        }
                    }

                    for (byte i = 0; i < 7; i++)
                    {
                        if (chb[i].Checked)
                        {
                            Result = fsm.SaveTimeToStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                Convert.ToByte(txtGC.Text), i, begin, end, Convert.ToInt32(textTimeOut.Text), Cnv);
                            if (Result == ConnectionManager.ReturnValues.Successful)
                            {
                                Debug(DbgMsgType.Normal, "Code: " + txtGC.Text +
                                                         "\tDay: " + i.ToString() +
                                                         "\tBgn:" + dtBegin.Value.ToShortTimeString() +
                                                         "\tEnd:" + dtEnd.Value.ToShortTimeString(), true);
                                pBar.Value = 100;
                                rValue.Text = Result.ToString();

                            }
                            else
                            {
                                pBar.Value = 0;
                                rValue.Text = Result.ToString();
                            }
                        }
                    }

                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "İşlem başarısız", true);
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void saveStaffPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                Result = fsm.SavePerson(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToUInt64(txtID.Text), Convert.ToByte(tsStaffGroup.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Outgoing, "CardID: " + txtID.Text + "\tGroup: " + tsStaffGroup.Text + "\tAdded", true);
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

        private void button20_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            byte[] staffGroup;

            try
            {
                Result = fsm.GetStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out staffGroup, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    string sg = "StaffGroups: ";
                    for (int i = 0; i < staffGroup.Length; i++)
                    {
                        if (staffGroup[i] != 0 && staffGroup[i] != 0xFF)
                        {
                            sg += staffGroup[i].ToString();
                            if (i != staffGroup.Length - 1)
                                sg += ", ";
                        }
                    }

                    Debug(DbgMsgType.Normal, sg, true);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "İşlem başarısız", true);
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                byte sGroup = Convert.ToByte(txtGC.Text);
                if (sGroup >= 255 && sGroup == 0)
                {
                    Debug(DbgMsgType.Normal, "Lütfen grup kodunu 1 - 254 arasında giriniz.", true);
                    return;
                }

                CheckBox[] chb = new CheckBox[7] { cb_Sun, cb_Mon, cb_Tue, cb_Wed, cb_Thu, cb_Fri, cb_Sat };
                for (byte i = 0; i < 7; i++)
                {
                    if (chb[i].Checked)
                        break;
                    if (i == 6)
                    {
                        Debug(DbgMsgType.Normal, "Lütfen gün seçiniz.", true);
                        return;
                    }
                }

                int[] begin;
                int[] finish;

                for (byte i = 0; i < 7; i++)
                {
                    if (chb[i].Checked)
                    {

                        Result = fsm.GetTimeFromStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), sGroup, (byte)i, out begin, out finish, Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (Result == ConnectionManager.ReturnValues.Successful)
                        {
                            string tm = "-------------------> " + chb[i].Text + " <-------------------\r\n";
                            for (int x = 0; x < begin.Length; x++)
                            {
                                tm += "BeginTime: " + (begin[x] / 60) + ":" + (begin[x] % 60) + "\tFinishTime: " + (finish[x] / 60) + ":" + (finish[x] % 60) + "\r\n";
                            }

                            Debug(DbgMsgType.Normal, tm, true);
                            pBar.Value = 100;
                            rValue.Text = Result.ToString();
                        }
                    }
                    //else
                    //{
                    //    string tm = "-------------------> " + chb[i].Text + " <-------------------\r\n";
                    //    Debug(DbgMsgType.Normal, tm, true);

                    //}
                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.tacteknoloji.com");
            System.Diagnostics.Process.Start("https://www.barkodes.com.tr");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                //if (lbDevices.Text != "")
                //{
                stri++;
                Thread[] threadOnline = new Thread[cmbIPs.Items.Count];

                // = new Thread(new ThreadStart(ListenOnline(cmbIPs.Text)));
                for (int i = 0; i < cmbIPs.Items.Count; i++)
                {
                    string IPAdr = cmbIPs.Items[i].ToString();
                    threadOnline[i] = new Thread(() => ListenOnline(IPAdr));

                    if (stri == 1)
                    {
                        button23.Text = "OStop";
                        threadOnline[i].Start();//ListenOnline();
                    }
                    else
                    {
                        stri = 0;
                        button23.Text = "OStart";
                        threadOnline[i].Suspend();
                    }
                }

                //}
            }
            catch (Exception ex)
            { }
        }

        public string[] GetLocalIp()
        {
            IPAddress[] localip = Dns.GetHostAddresses(Dns.GetHostName());
            string[] lip = new string[localip.Length / 2];
            int i = 0;

            foreach (IPAddress address in localip)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    lip[i++] = address.ToString();
                }
            }
            return lip;
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            string[] lip = GetLocalIp();
            comboBox1.Items.Clear();
            for (int i = 0; i < lip.Length; i++)
            {
                comboBox1.Items.Add(lip[i]);
            }
            if (lip.Length > 0)
                comboBox1.Text = lip[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Thread OnlineList;
        private void button35_Click(object sender, EventArgs e)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                OnlineList = new Thread(() => ServerOpen(comboBox1.Text, Convert.ToInt32(textBox9.Text)));
                OnlineList.Start();
            }
        }

        TcpListener serverSocket;
        TcpClient clientSocket;
        bool slStart;

        void ServerOpen(string serverip, int port)
        {
            serverSocket = new TcpListener(IPAddress.Parse(serverip), port);//IPAddress.Parse(serverip), 
            clientSocket = default(TcpClient);
            int counter = 0;
            slStart = true;

            serverSocket.Start();

            try
            {
                while (slStart)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    string[] sip = clientSocket.Client.RemoteEndPoint.ToString().Split(':');
                    richTextBox1.AppendText(sip[0] + "\r\n");
                    label54.Text = " + " + counter.ToString();
                    startClient(clientSocket, counter);
                }

                serverSocket.Stop();
                clientSocket.Close();
                OnlineList.Abort();
            }
            catch (Exception)
            {
                serverSocket.Stop();
                clientSocket.Close();
                OnlineList.Abort();
                throw;
            }

        }

        private void getSpareLogV402ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            byte[] Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            bool part = false;
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.DenyWithReason reason;
            ConnectionManager.Direction dir;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (Spare1Log < 10000)
                    {
                        part = false;
                        if (end > Spare1Log)
                            end = Spare1Log;
                        else
                        {
                            Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                        }
                    }
                    else
                    {
                        part = true;
                        if (end > Spare2Log)
                            end = Spare2Log;
                        else
                        {
                            Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                        }
                    }
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end;)
                {
                    Result = fsm.GetAccessSpareLogNew(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, part, out Log, out LogID, out LogTime, out reason, out dir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + reason.ToString() + "\tDir: " + dir.ToString() + "\tFback: " + FBack.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, i.ToString() + "  " + (char)9 + " nolu log çekilemedi. Hata : " + Result.ToString(), true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void getPersonElevatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            long LogIndex, PerIndex, BlackIndex, ErasedPerIndex, EventIndex, Spare1Log, Spare2Log;
            UInt64 personID;
            byte shift;
            byte[] gates;
            pBar.Value = 0; rtbDebug.Text = "";

            long StrIndex = Convert.ToInt32(txtFrom.Text);
            long EndIndex = Convert.ToInt32(txtTo.Text);

            
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (EndIndex > PerIndex)
                        EndIndex = PerIndex - 1;
                    else
                    {
                        Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    }
                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }


                for (long i = StrIndex; i <= EndIndex; i++)
                {
                    ConnectionManager.PersonState state;
                    Result = fsm.GetPersonCardID(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (int)i, out personID, out state, out shift, out gates, Convert.ToInt32(textTimeOut.Text), Cnv);

                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        string StrLodID = personID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        Debug(DbgMsgType.Outgoing, i.ToString() + "    ID:  " + bosluk + personID.ToString() + "\tState: " + state.ToString(), true);
                    }
                    else
                        Debug(DbgMsgType.Outgoing, i.ToString() + "\t" + Result.ToString(), true);
                }

            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void ckbMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbMethod.Checked)
                label40.Text = "Old";
            else
                label40.Text = "New";
        }

        private void chkOldElev_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOldElev.Checked)
                chkNewElev.CheckState = CheckState.Unchecked;
        }

        private void chkNewElev_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewElev.Checked)
                chkOldElev.CheckState = CheckState.Unchecked;
        }

        private void button24_Click(object sender, EventArgs e)
        {

            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            try
            {
                int adr = Convert.ToInt32(cbElevAdr.Text);
                ConnectionManager.Contact contact;
                if (cbElevContact.SelectedItem.ToString() == "NO")
                    contact = ConnectionManager.Contact.NO;
                else contact = ConnectionManager.Contact.NC;

                 Result = fsm.SaveElevatorConfig(cmbIPs.Text, Convert.ToInt32(textPort.Text), adr, contact, Convert.ToInt32(textTimeOut.Text), Cnv);
                pBar.Value = 100;
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        Thread threadCalib;
        TcpClient clientcalib = null;
        bool calibflag = false;
        private void button26_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;

            try
            {
                if (!calibflag)
                {
                    clientcalib = new TcpClient();
                    ConnectionManager.ReturnValues Result = fsm.PingAndPortTest(cmbIPs.Text, Convert.ToInt32(textPort.Text), clientcalib);
                    if (Result != ConnectionManager.ReturnValues.Successful)
                    {
                        clientcalib.Close();
                        return;
                    }

                    calibflag = true;
                    button26.BackColor = Color.GreenYellow;
                    threadCalib = new Thread(() => CalibrationTestProcess(clientcalib));
                    threadCalib.Start();
                }
                else calibflag = false;
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        bool CalibrationStatus = false;
        public void CalibrationTestProcess(TcpClient client)
        {
            try
            {
                if (client.Connected)
                {
                    while (true)
                    {
                        if (!calibflag || !client.Connected)
                        {
                            if (client.Connected)
                                client.Close();
                            button26.BackColor = Color.Red;
                            threadCalib.Abort();
                        }

                        string cal;
                        Result = fsm.CalibreTest(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), client, out cal, Convert.ToInt32(textTimeOut.Text), Cnv);
                        if (Result == ConnectionManager.ReturnValues.Successful)
                        {

                            Debug(DbgMsgType.Normal, cal, true);
                            if (cal.Substring(0, 33) == "MSP430 Clock Calibration Value - ")//Calibration: ")
                            {
                                string cval = cal.Substring(33, 3);
                                int val = Convert.ToInt32(cval);// + 5;
                                Result = fsm.SetCalibration(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), client, (byte)val, Convert.ToInt32(textTimeOut.Text), Cnv);
                                if (Result == ConnectionManager.ReturnValues.Successful)
                                {

                                    Debug(DbgMsgType.Normal, "Set Calibration: " + val, true);
                                    button26.BackColor = Color.Green;
                                    threadCalib.Abort();

                                }

                            }
                            else if (cal.Substring(0, 38) == "MSP430 Clock Calibration Value : OK - ")//Calibration: ")
                            {
                                Debug(DbgMsgType.Normal, cal, true);
                                button26.BackColor = Color.Green;
                                threadCalib.Abort();
                            }

                            pBar.Value = 100;
                            rValue.Text = Result.ToString();

                        }
                        else
                        {
                            Debug(DbgMsgType.Outgoing, ">> Taranıyor <<", true);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void getEventLogV400ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            DateTime EventTime;
            ConnectionManager.Events_v4 Event;
            rtbDebug.Text = "";
            byte[] Log;

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (end > EventIndex)
                        end = EventIndex;
                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    return;
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end;)
                {
                    Result = fsm.GetEventLog_old(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, out Log, out Event, out EventTime, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {

                        rtbDebug.AppendText(String.Format("\r\nIndex:   {0,-6}   EventCode: {1,-16}   EventTime: {2,-15}", i.ToString(), Event.ToString(), EventTime.ToString()));
                       
                        if (EventTime.Year > DateTime.Now.Year)
                            rtbDebug.AppendText("\r\nRawData: " + BitConverter.ToString(Log));

                        x = 0;
                        //Debug(DbgMsgType.Outgoing, i.ToString() + "\t" + "EventCode: " + Event.ToString() + (char)9 + "Time: " + EventTime.ToString(), true);
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, String.Format("Index: {0,-6}   Error Status: {1,-10}", i.ToString(), Result.ToString()), true);
                        Debug(DbgMsgType.Error, "RawData: " + BitConverter.ToString(Log), true);
                        i++;
                        if (stoplog++ >= 10)
                            break;
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }


    private void getSpareLogRawV400ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            byte[] Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            bool part = false;
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.AccessDirection access;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                                out PerIndex,
                                                                out LogIndex,
                                                                out BlackIndex,
                                                                out ErasedPerIndex,
                                                                out EventIndex,
                                                                out Spare1Log, out Spare2Log,
                                                                Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (begin < 10000)
                    {
                        part = false;
                        if (end > Spare1Log)
                            end = Spare1Log;
                    }
                    else
                    {
                        part = true;
                        begin = (begin - 10000);
                        end = (end - 10000);
                        if (end > Spare2Log)
                            end = Spare2Log;
                    }
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end;)
                {
                    Result = fsm.GetAccessSpareLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), i, part, out Log, out LogID, out LogTime, out access, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        //Debug(DbgMsgType.Outgoing, i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + access.ToString() + "\tFback: " + FBack.ToString(), true);
                        rtbDebug.AppendText(String.Format("\r\nIndex:   {0,-6}   Cardid: {1,-16}   LogTime: {2,-15}   Access: {3,-10}   Fback: {4,-10}", i.ToString(), bosluk + LogID.ToString(), LogTime.ToString(), access.ToString(), FBack.ToString()));
                        //rtbDebug.AppendText("\r\n" + i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + access.ToString() + "\tFback: " + FBack.ToString());
                        if (LogTime.Year > DateTime.Now.Year)
                            rtbDebug.AppendText("\r\nRawData: " + BitConverter.ToString(Log));
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, String.Format("Index: {0,-6}   Error Status: {1,-10}", i.ToString(), Result.ToString()), true);
                        Debug(DbgMsgType.Error, "RawData: " + BitConverter.ToString(Log), true);
                        //if (test++ >= 1)
                        //{
                        //    test = 0;
                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        int terminalBegin = 0, terminalFinish = 0, terminalTotal = 0;

        private void rt_terminalBeginData_TextChanged(object sender, EventArgs e)
        {
            lb_terminalBeginSize.Text = (terminalBegin = rt_terminalBeginData.TextLength / 2).ToString();
            lb_terminalTotalSize.Text = (terminalTotal = 20 - (terminalBegin + terminalFinish)).ToString();
        }

        private void rt_terminalFinishData_TextChanged(object sender, EventArgs e)
        {
            lb_terminalFinishSize.Text = (terminalFinish = rt_terminalFinishData.TextLength / 2).ToString();
            lb_terminalTotalSize.Text = (terminalTotal = 20 - (terminalBegin + terminalFinish)).ToString();
        }

        private void bt_terminalSave_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                byte[] data = new byte[22];
                data[0] = (byte)terminalBegin;
                data[1] = (byte)terminalFinish;

                if(((rt_terminalBeginData.TextLength % 2) != 0) || ((rt_terminalFinishData.TextLength % 2) != 0))
                {
                    MessageBox.Show("Eksik veya fazla değer girdiniz", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] tdata = Conversion.HexToByte(rt_terminalBeginData.Text);
                Array.Copy(tdata, 0, data, 2, terminalBegin);

                tdata = Conversion.HexToByte(rt_terminalFinishData.Text);
                Array.Copy(tdata, 0, data, 2 + terminalBegin, terminalFinish);

                Result = fsm.SendTerminalData(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), data, Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void bt_terminalRead_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                byte[] data;

                Result = fsm.GetTerminalData(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), out data, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {

                    byte[] bdata = new byte[data[0]];
                    byte[] fdata = new byte[data[1]];
                    Array.Copy(data, 2, bdata, 0, bdata.Length);
                    Array.Copy(data, 2 + bdata.Length, fdata, 0, fdata.Length);

                    rt_terminalBeginData.Text = Conversion.Byte2Hex(bdata);
                    rt_terminalFinishData.Text = Conversion.Byte2Hex(fdata);

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

        private void bt_Baudrate_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            try
            {
                Result = fsm.ChangeBaudRate(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (ConnectionManager.BaudRate)cb_baudrate.SelectedItem, Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void rt_terminalBeginData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((terminalBegin + terminalFinish) == 20)
            {
                e.Handled = true;
                return;
            }

            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57)
            {
                e.Handled = false;
            }
            else if (((int)e.KeyChar >= 66 && (int)e.KeyChar <= 70) || ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 102))
            {
                e.Handled = false;
            }
            else if ((int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            //lb_terminalBeginSize.Text = (terminalBegin = rt_terminalBeginData.TextLength / 2).ToString();
            //lb_terminalTotalSize.Text = (terminalTotal = 20 - (terminalBegin + terminalFinish)).ToString();
        }

        private void rt_terminalFinishData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((terminalBegin + terminalFinish) == 20)
            {
                e.Handled = true;
                return;
            }

            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57)
            {
                e.Handled = false;
            }
            else if (((int)e.KeyChar >= 66 && (int)e.KeyChar <= 70) || ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 102))
            {
                e.Handled = false;
            }
            else if ((int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            //lb_terminalFinishSize.Text = (terminalFinish = rt_terminalFinishData.TextLength / 2).ToString();
            //lb_terminalTotalSize.Text = (terminalTotal = 20 - (terminalBegin + terminalFinish)).ToString();
        }

        public void startClient(TcpClient inClientSocket, int clineNo)
        {
            Thread ctThread = new Thread(() => OnlineProcess(inClientSocket, clineNo));
            ctThread.Start();
            Thread.Sleep(10);
        }

        int StartCount=0;
        private void OnlineProcess(TcpClient inClientSocket, int clineNo)
        {
            int requestCount = 0;
            byte[] bytesFrom;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = inClientSocket.GetStream();
                    byte[] Parse;
                    ConnectionManager.AccessType Access;
                    int RelayTime = (Convert.ToInt32(textBox3.Text) / 100);

                    if (checkBox3.Checked)
                    {
                        Access = ConnectionManager.AccessType.Accept;
                    }
                    else
                    {
                        Access = ConnectionManager.AccessType.Deny;
                    }

                    if (checkBox4.Checked && StartCount == 1)
                    {
                        Parse = fsm.ReadAccessData(3, 255, Access, RelayTime, ConnectionManager.BuzzerState.BuzzerOn, ConnectionManager.LogsProcess.StartSendingLog);
                        networkStream.Write(Parse, 0, Parse.Length);
                        StartCount = 0;
                    }
                    else if (StartCount == 2)
                    {
                        Parse = fsm.ReadAccessData(3, 255, Access, RelayTime, ConnectionManager.BuzzerState.BuzzerOn, ConnectionManager.LogsProcess.StopSendingLog);
                        networkStream.Write(Parse, 0, Parse.Length);
                        StartCount = 0;
                    }

                    if (networkStream.DataAvailable)
                    {
                        bytesFrom = new byte[65536];
                        string cip;
                        string[] sip = inClientSocket.Client.RemoteEndPoint.ToString().Split(':');
                        cip = sip[0]; 
                        int rsize = networkStream.Read(bytesFrom, 0, (int)inClientSocket.ReceiveBufferSize);
                        object[] datas;

                        if(DataParse(bytesFrom, out datas))
                        {
                            if (datas.Length > 0)
                            {
                                byte process = Convert.ToByte(datas[0]);
                                int address = Convert.ToInt32(datas[1]);

                                switch (process)
                                {
                                    case 0:
                                        {                                           
                                            Debug(DbgMsgType.Incoming, "--> IP: " + cip + "\tAddress: " + address + "\tID: " + datas[2] + "\tLog: " + datas[3], true);
                                            Parse = fsm.ReadAccessData(process, address, Access, RelayTime, ConnectionManager.BuzzerState.BuzzerOn, ConnectionManager.LogsProcess.AckLog);
                                            networkStream.Write(Parse, 0, Parse.Length);
                                        } break;
                                    case 1:
                                        {
                                            Debug(DbgMsgType.Incoming, "--> IP: " + cip + "\tAddress: " + address + "\tGateState: " + datas[2], true);
                                        } break;
                                    case 2: 
                                        {
                                            Debug(DbgMsgType.Incoming, "--> IP: " + cip + "\tAddress: " + address + "\tFireAlarm: " + datas[2], true);
                                        } break;
                                    case 3: 
                                        {
                                            if (Convert.ToUInt64(datas[2]) != 0)
                                            {
                                                Debug(DbgMsgType.Outgoing, "--> IP: " + cip + "\tAddress: " + datas[1] + "\tID: " + datas[2] + "\tTime: " + datas[3] + "\tReason: " + datas[4] + "\tDirection: " + datas[5] + "\tFback: " + datas[6], true);
                                                Parse = fsm.ReadAccessData(process, address, Access, RelayTime, ConnectionManager.BuzzerState.BuzzerOn, ConnectionManager.LogsProcess.AckLog);
                                                networkStream.Write(Parse, 0, Parse.Length);
                                            }
                                            else Debug(DbgMsgType.Error, "<--HATALI KAYIT--> IP: " + cip + "\tAddress: " + address + "\tCommand: " + datas[2], true);
                                        } break;
                                    case 4: 
                                        {
                                            Debug(DbgMsgType.Warning, "<--ACK--> IP: " + cip + "\tAddress: " + address + "\tCommand: " + datas[2], true);
                                        } break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    serverSocket.Stop();
                    clientSocket.Close();
                    break;
                }
            }
        }

       
        private bool DataParse(byte[] rcvData, out object[] data)
        {
            data = null;
            for (int i = 0; i < rcvData.Length; i++)
            {
                if (rcvData[i] == (byte)154)
                {
                    int rid = 0, crc=0, len = rcvData[i - 1];
                    for (int x = 0; x < len-1; x++)
                    {
                        crc ^= rcvData[(i - 1) + x];
                    }
                    crc = 255 - crc;
                    if (crc == rcvData[(i-1) + (len - 1)])
                    {
                        rid = (i-1);
                        switch (rcvData[i + 3])
                        {
                            case (72):
                                {
                                    switch(rcvData[i + 4])
                                    {
                                        case 74:   
                                                {
                                                    if (len >= 18)
                                                    {

                                                        data = new object[4];
                                                        data[0] = 0;
                                                        data[1] = rcvData[rid + 2];
                                                        ulong ID = 0;
                                                        ID = rcvData[rid + 6];
                                                        ID = (ID << 8) | (rcvData[rid + 7]);
                                                        ID = (ID << 8) | (rcvData[rid + 8]);
                                                        ID = (ID << 8) | (rcvData[rid + 9]);
                                                        ID = (ID << 8) | (rcvData[rid + 10]);
                                                        ID = (ID << 8) | (rcvData[rid + 11]);
                                                        ID = (ID << 8) | (rcvData[rid + 12]);
                                                        data[2] = ID;
                                                        int OfflineLogCount;
                                                        OfflineLogCount = rcvData[rid + 16];
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 15]);
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 14]);
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 13]);
                                                        data[3] = OfflineLogCount;
                                                        return true;
                                                    }
                                                    else if (len < 18)
                                                    {
                                                        data = new object[4];
                                                        data[0] = 0;
                                                        data[1] = rcvData[rid + 2];
                                                        ulong ID = 0;
                                                        ID = rcvData[rid + 6];
                                                        ID = (ID << 8) | (rcvData[rid + 7]);
                                                        ID = (ID << 8) | (rcvData[rid + 8]);
                                                        ID = (ID << 8) | (rcvData[rid + 9]);
                                                        data[2] = ID;
                                                        int OfflineLogCount;
                                                        OfflineLogCount = rcvData[rid + 13];
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 12]);
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 11]);
                                                        OfflineLogCount = (OfflineLogCount << 8) | (rcvData[rid + 10]);
                                                        data[3] = OfflineLogCount;
                                                        return true;
                                                    }
                                                }break;
                                        case 146: 
                                                {
                                                    if (len > 7)
                                                    {
                                                        data = new object[3];
                                                        data[0] = 1;
                                                        data[1] = rcvData[rid + 2];
                                                        data[2] = (ConnectionManager.DoorStatus)rcvData[rid + 6];
                                                        return true;
                                                    }
                                                }break;
                                        case 148:
                                                {
                                                    if (len > 7)
                                                    {
                                                        data = new object[3];
                                                        data[0] = 2;
                                                        data[1] = rcvData[rid + 2];
                                                        data[2] = (ConnectionManager.EmergencySts)rcvData[rid + 6];
                                                        return true;
                                                    } 

                                                }break;
                                    }
                                }break;

                            case (162 + 1):
                                {
                                    if (len >= 15)
                                    {
                                        data = new object[7];
                                        data[0] = 3;
                                        data[1] = rcvData[rid + 2];

                                        if (rcvData[rid + 20] == 100)
                                        {
                                            data[2] = 0;
                                            break;
                                        }

                                        ulong LogID = 0;
                                        LogID = ((LogID << 8) | rcvData[rid + 12]);
                                        LogID = ((LogID << 8) | rcvData[rid + 11]);
                                        LogID = ((LogID << 8) | rcvData[rid + 10]);
                                        LogID = ((LogID << 8) | rcvData[rid + 9]);
                                        LogID = ((LogID << 8) | rcvData[rid + 8]);
                                        LogID = ((LogID << 8) | rcvData[rid + 7]);
                                        LogID = ((LogID << 8) | rcvData[rid + 6]);
                                        data[2] = LogID;

                                        DateTime time = new DateTime(2000 + rcvData[rid + 13], rcvData[rid + 14], rcvData[rid + 15], rcvData[rid + 16], rcvData[rid + 17], rcvData[rid + 18]);
                                        data[3] = time;

                                        int Rsn = rcvData[rid + 19] & 0xF0;
                                        int dir = rcvData[rid + 19] & 0x0F;

                                        data[4] = (ConnectionManager.DenyWithReason)Enum.Parse(typeof(ConnectionManager.DenyWithReason), Rsn.ToString());
                                        data[5] = (ConnectionManager.Direction)Enum.Parse(typeof(ConnectionManager.Direction), dir.ToString());
                                        data[6] = (ConnectionManager.FeedBackControl)Enum.Parse(typeof(ConnectionManager.FeedBackControl), rcvData[rid + 20].ToString());
                                        return true;
                                    }
                                    else if (len == 7)
                                    {
                                        data = new object[4];
                                        data[0] = 4;
                                        data[1] = rcvData[rid + 2];
                                        data[2] = (ConnectionManager.Commands)(rcvData[rid + 4] - 1);
                                        if (rcvData[rid + 5] == 0)
                                            data[3] = ConnectionManager.ReturnValues.Successful;
                                        else data[3] = ConnectionManager.ReturnValues.Failed;
                                        return true;
                                    }
                                }break;
                               default:
                                   {
                                    if (len == 7)
                                    {
                                        data = new object[4];
                                        data[0] = 4;
                                        data[1] = rcvData[rid + 2];
                                        data[2] = (ConnectionManager.Commands)(rcvData[rid + 4] - 1);
                                        if (rcvData[rid + 5] == 0)
                                            data[3] = ConnectionManager.ReturnValues.Successful;
                                        else data[3] = ConnectionManager.ReturnValues.Failed;
                                        return true;
                                    }
                                } break;
                        }                        
                    }
                    return false;
                }
            }
            return false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                checkBox3.Text = "Accept";
            else checkBox3.Text = "Deny";
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                StartCount = 1;
                checkBox4.Text = "SendToStopLog";
            }
            else
            {
                StartCount = 2;
                checkBox4.Text = "SendToStartLog";
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                slStart = false;
                richTextBox1.Clear();
                serverSocket.Stop();
                clientSocket.Close();
                OnlineList.Abort();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                Result = fsm.EraseStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    Convert.ToByte(txtGC.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Normal, "Code: " + txtGC.Text + " Erased", true);
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

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;
            string PersonName;
            PersonName = txtPerName.Text;

            try
            {
                Result = fsm.EraseAllStaffGroup(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                    Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    Debug(DbgMsgType.Normal, "All StaffGroup Erased", true);
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

        private void button28_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            pBar.Value = 0;

            try
            {
                ConnectionManager.ClockDir dir;
                if (radioButton2.Checked)
                    dir = ConnectionManager.ClockDir.Forth;
                else
                    dir = ConnectionManager.ClockDir.Back;
                Result = fsm.SaveWinSumTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), dateTimePicker2.Value, Convert.ToInt32(textBox4.Text), dir, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    int begin = dtBegin.Value.Hour * 60 + dtBegin.Value.Minute;
                    int end = dtEnd.Value.Hour * 60 + dtEnd.Value.Minute;

                    Debug(DbgMsgType.Normal, "Time: " + dateTimePicker2.Value.ToString() +
                                                "\tHour: " + textBox4.Text +
                                                "\tDir:" + dir.ToString(), true);
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();

                }
                else
                {
                    Debug(DbgMsgType.Outgoing, "İşlem başarısız", true);
                }
            }

            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void tcbCommVersion_Click(object sender, EventArgs e)
        {
            if (tcbCommVersion.SelectedIndex == 1)
            {
                fsm.CommVersion = true;
            }
            else
            {
                fsm.CommVersion = false;
            }
        }

        private void getAccessLogV400ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }

            pBar.Value = 0;
            long LogIndex, PerIndex, EventIndex, BlackIndex, ErasedPerIndex, Spare1Log, Spare2Log;
            byte[] Log;
            long begin = Convert.ToInt32(txtBegin.Text), x = 0;
            long end = Convert.ToInt32(txtEnd.Text);
            UInt64 LogID;
            DateTime LogTime;
            string strLogTime;
            ConnectionManager.FeedBackControl FBack;
            ConnectionManager.DenyWithReason reason;
            ConnectionManager.AccessDirection dir;
            rtbDebug.Text = "";

            try
            {

                Result = fsm.GetDatabaseParameters(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                                                out PerIndex,
                                                out LogIndex,
                                                out EventIndex,
                                                out BlackIndex,
                                                out ErasedPerIndex,
                                                out Spare1Log, out Spare2Log,
                                                Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    if (end > LogIndex)
                        end = LogIndex;
                    else
                    {
                        Debug(DbgMsgType.Outgoing, "Index Çekilemedi " + (char)9 + Result.ToString(), true);
                    }
                }

                int test = 0, stoplog = 0;
                Debug(DbgMsgType.Outgoing, "Start Time --> " + DateTime.Now.ToString(), true);
                for (long i = begin; i < end; )
                {
                    Result = fsm.GetAccessLog(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (int)i, out Log, out LogID, out LogTime, out dir, out FBack, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        x = 0;
                        string StrLodID = LogID.ToString();
                        string bosluk = "";

                        if (StrLodID.Length <= 10)
                            bosluk = "_______";
                        else
                            bosluk = "";

                        rtbDebug.AppendText(String.Format("\r\nIndex:   {0,-6}   Cardid: {1,-16}   LogTime: {2,-15}   Access: {3,-10}   Fback: {4,-10}", i.ToString(), bosluk + LogID.ToString(), LogTime.ToString(), dir.ToString(), FBack.ToString()));
                        //rtbDebug.AppendText("\r\n" + i.ToString() + "  " + (char)9 + "ID: " + bosluk + LogID.ToString() + (char)9 + "Time: " + LogTime.ToString() + "    " + "\tAccess: " + access.ToString() + "\tFback: " + FBack.ToString());
                        if (LogTime.Year > DateTime.Now.Year)
                            rtbDebug.AppendText("\r\nRawData: " + BitConverter.ToString(Log));
                        i++;
                    }
                    else
                    {
                        Debug(DbgMsgType.Error, String.Format("Index: {0,-6}   Error Status: {1,-10}", i.ToString(), Result.ToString()), true);
                        Debug(DbgMsgType.Error, "RawData: " + BitConverter.ToString(Log), true);

                        i++;
                        if (stoplog++ >= 10)
                            break;
                        //}
                    }
                }
                Debug(DbgMsgType.Outgoing, "End Time --> " + DateTime.Now.ToString(), true);
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                rValue.Text = ex.ToString();
            }
        }

        private void autoTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stri == 1) { MessageBox.Show("Önce Online Kapatmanız Gerekiyor !"); return; }
            DateTime Time = new DateTime();
            //Time = DateTime.Now;
            Time = dtCurrentTime.Value;
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeDateTime(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), DateTime.Now, Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    pBar.Value = 100;
                    rValue.Text = Result.ToString();
                    Debug(DbgMsgType.Outgoing, "Change Time: " + DateTime.Now.ToString(), true);

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
    }
}

