using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BarkodesDeviceVerison.Properties;
using BarkodesDeviceVerison;
using rfMultiLibrary;
using System.Threading;

namespace BarkodesDeviceVerison
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setting = new Settings();
            cmbDeviceType.Text = setting.DeviceType;
            btnListCezeri.PerformClick();
            Thread.Sleep(100);
            if (cmbDeviceType.Text == "Fsm 1453")
                button1.PerformClick();

            HaberlesmeKodu.DataSource = Enum.GetValues(typeof(BarkodesDeviceVerison.CommunicationCode.ManufacturerCode));
        }
        BarkodesDeviceVerison.ComMng.Converter Cnv = ComMng.Converter.Tac;
        ComMng.ReturnValues Result;
        Device[] dvc;
        List<Device> Devices;
        ComMng cm = new ComMng();
        Settings setting = new Settings();

        private void btnTestConn_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            if (cmbDeviceType.Text == "Cezeri T&A")
            {
                DateTime dt;


                Result = cm.GetDateTime("FF-FF-FF-FF-FF-FF",
                                        txtIP.Text,
                                        Convert.ToInt32(txtTcpPort.Text),
                                        Convert.ToInt32(txtUdpPort.Text),
                                        out dt,
                                        2000);
                if (Result == ComMng.ReturnValues.Succesfull)
                {
                    pBar.Value = 100;
                    LabelStatus.Text = Result.ToString();
                }

            }
            else if (cmbDeviceType.Text == "Fsm 1453")
            {
                    Result = cm.DeviceTestConnection(txtIP.Text, Convert.ToInt32(txtTcpPort.Text), Convert.ToInt32(txtAdr.Text), 500, Cnv);
                    if (Result == ComMng.ReturnValues.Succesfull)
                    {
                        pBar.Value = 100;
                        LabelStatus.Text = Result.ToString();
                    }
            }
        }

        public class IdStarIpAscending : IComparer<Device>
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(Device x, Device y)
            {
                //return string.Compare(y.NetParams.IP.Split('.')[3], x.NetParams.IP.Split('.')[3]);
                int strA = Convert.ToInt32(y.NetParams.IP.Split('.')[3]);
                int strB = Convert.ToInt32(x.NetParams.IP.Split('.')[3]);

                if (strA > strB) return -1;
                else return 1;
            }
        }

        string[,] nIP = new string[100, 2];
        int CnvIndex = 0;
        private void btnListCezeri_Click(object sender, EventArgs e)
        {
            dvc = null;
            cbDevices.Items.Clear();
            lbDevices.Items.Clear();
            if (cmbDeviceType.Text == "Cezeri T&A")
            {

                cm.GetLocalDevices(out dvc);

                if (dvc == null) return;

                IdStarIpAscending cmp = new IdStarIpAscending();
                Array.Sort(dvc, cmp);
                Devices = new List<Device>();

                LabelStatus.Text = " Cihaz bulunamadı.";

                if ((dvc != null) & (dvc.Length > 0))
                {
                    for (int i = 0; i < dvc.Length; i++)
                    {

                        bool DvcReply = false;
                        cbDevices.Items.Add(dvc[i].NetParams.IP);
                        cbDevices.Text = dvc[0].NetParams.IP.ToString();

                        for (int x = 0; x < i; x++)
                        {
                            if (dvc[i].NetParams.Mac == dvc[x].NetParams.Mac && dvc[i].NetParams.IP == dvc[x].NetParams.IP)
                                DvcReply = true;
                        }

                        if (DvcReply) continue;


                        if (dvc[i].MfgParams.Device.ToString() == "112")
                        {
                            lbDevices.Items.Add(dvc[i].NetParams.IP);
                            Devices.Add(dvc[i]);
                        }

                        lbDevices.Items.Add(dvc[i].NetParams.IP);
                        Devices.Add(dvc[i]);
                    }
                    if (lbDevices.Items.Count > 0)
                        lbDevices.SelectedIndex = 0;

                    LabelStatus.Text = lbDevices.Items.Count.ToString() + " Cihaz bulundu.";
                }
            }
            else if (cmbDeviceType.Text == "Fsm 1453")
            {
                cbDevices.Items.Clear();
                rfMultiLibrary.DeviceSettings[] Dvc; CnvIndex = 0;
                nIP = new string[100, 2]; string[] sIP = new string[100];
                Macs = IPs = GWs = SMs = Ports = Bauds = Pars = Datas = Stops = Flows = Protocols = Names = null;
                try
                {
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
                        cbDevices.Items.Add(oIP[i]);
                    }

                    if (cbDevices.Items.Count > 0)
                    {
                        LabelStatus.Text = cbDevices.Items.Count.ToString() + " Converter Bulundu";
                        pBar.Value = 100;
                        cbDevices.SelectedIndex = 0;
                        for (int i = 0; i < CnvIndex; i++)
                        {
                            if (cbDevices.Text == nIP[i, 0])
                                if (nIP[i, 1] == "T") { Cnv = ComMng.Converter.Tac; }
                                else {Cnv = ComMng.Converter.NewCon; }
                        }
                    }
                    else
                    {
                        LabelStatus.Text = "Converter Bulunamadı !";
                        return;
                    }

                    //pBar.Value = 100;
                    //cmbIPs.SelectedIndex = 0;
                    //if (CnvType[0] == 'T') { DoorSt.Text = "T"; Cnv = ConnectionManager.Converter.Tac; }
                    //else { DoorSt.Text = "C"; Cnv = ConnectionManager.Converter.Cezeri; }

                }
                catch (Exception ex)
                {
                    //Debug(DbgMsgType.Outgoing, ex.ToString(), true);
                }
                //Macs = IPs = GWs = SMs = Ports = Bauds = Pars = Datas = Stops = Flows = Protocols = Names = null;
                //try
                //{
                //    //if (tactibbo.GetLocalDevices(out Macs, out IPs, out GWs, out SMs, out Ports, out Bauds, out Pars, out Datas, out Stops, out Flows, out Names) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfulll)
                //    //{
                //    //    for (int i = 0; i < Macs.Length; i++)
                //    //    {
                //    //        cmbIPs.Items.Add(IPs[i]);
                //    //    }

                //    //}
                //    if (tactibbo.GetLocalDevices_v2(out Macs, out IPs, out GWs, out SMs, out Ports, out Bauds, out Pars, out Datas, out Stops, out Flows, out Protocols, out Names) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                //    {
                //        Array.Sort(IPs);
                //        for (int i = 0; i < Macs.Length; i++)
                //        {
                //            cbDevices.Items.Add(IPs[i]);
                //        }
                //    }

                //    if (cbDevices.Items.Count > 0)
                //        LabelStatus.Text = cbDevices.Items.Count.ToString() + " Converter Bulundu";
                //    else
                //    {
                //        LabelStatus.Text = "Converter Bulunamadı !";
                //        return;
                //    }

                //    for (int i = 0; i < Macs.Length; i++)
                //    {
                //        if (IPs[i] == setting.IP)
                //        {
                //            cbDevices.SelectedIndex = i;
                //            pBar.Value = 100;
                //            return;
                //        }
                //    }
                //    pBar.Value = 100;
                //    cbDevices.SelectedIndex = 0;

                //}
                //catch
                //{ }
            }
        }

        private void lbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
          if (cmbDeviceType.Text == "Cezeri T&A")
                    txtIP.Text = lbDevices.SelectedItem.ToString();
          else if (cmbDeviceType.Text == "Fsm 1453")
              txtAdr.Text = lbDevices.SelectedItem.ToString();
        }

        private void cmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setting.DeviceType = cmbDeviceType.Text;
            setting.Save();
        }

        private void btnGetVersion_Click(object sender, EventArgs e)
        {
            byte version = 0;
            pBar.Value = 0;
            if (cmbDeviceType.Text == "Cezeri T&A")
            {
                Result = cm.GetDeviceVersion("FF-FF-FF-FF-FF-FF",
                                            txtIP.Text,
                                            Convert.ToInt32(txtTcpPort.Text),
                                            Convert.ToInt32(txtUdpPort.Text),
                                            out version,
                                            500);
                if (Result == ComMng.ReturnValues.Succesfull)
                {
                    pBar.Value = 100;
                    LabelStatus.Text = Result.ToString();
                }


            }
            else if (cmbDeviceType.Text == "Fsm 1453")
            {
                Result = cm.GetDeviceVersionFsm(txtIP.Text, Convert.ToInt32(txtTcpPort.Text), Convert.ToInt32(txtAdr.Text), out version, 500, Cnv);
                if (Result == ComMng.ReturnValues.Succesfull)
                {
                    pBar.Value = 100;
                    LabelStatus.Text = Result.ToString();
                }
            }


            switch (version)
            {
                case 1: cmbVersion.SelectedIndex = 1; break;
                case 2: cmbVersion.SelectedIndex = 2; break;
                case 3: cmbVersion.SelectedIndex = 3; break;
                case 4: cmbVersion.SelectedIndex = 4; break;
                default: cmbVersion.SelectedIndex = 0; break;
            }
            
        }

        private void btnSerVersion_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            if (cmbDeviceType.Text == "Cezeri T&A")
            {
                byte version = (byte)cmbVersion.SelectedIndex;

                Result = cm.SetDeviceVersion("FF-FF-FF-FF-FF-FF",
                                            txtIP.Text,
                                            Convert.ToInt32(txtTcpPort.Text),
                                            Convert.ToInt32(txtUdpPort.Text),
                                            version,
                                            500);


                if (Result == ComMng.ReturnValues.Succesfull)
                {
                    pBar.Value = 100;
                    LabelStatus.Text = Result.ToString();
                }

            }
            else if (cmbDeviceType.Text == "Fsm 1453")
            {
                if (cmbVersion.Text == "FullAccess")
                    cmbVersion.Text = cmbVersion.Items[3].ToString();

                byte version = (byte)cmbVersion.SelectedIndex;
                Result = cm.SetDeviceVersionFsm(txtIP.Text, Convert.ToInt32(txtTcpPort.Text), Convert.ToInt32(txtAdr.Text), version, (CommunicationCode.ManufacturerCode)HaberlesmeKodu.SelectedItem, 2000, Cnv);

                if (Result ==ComMng.ReturnValues.Succesfull)
                {
                    pBar.Value = 100;
                    LabelStatus.Text = Result.ToString();
                }
            }

        }

        private void btnSerVersionUdp_Click(object sender, EventArgs e)
        {
            byte version = (byte)cmbVersion.SelectedIndex;
            if (cmbDeviceType.Text == "Cezeri T&A")
            {
                Result = cm.SetDeviceVersionUDP("FF-FF-FF-FF-FF-FF",
                                            txtIP.Text,
                                            Convert.ToInt32(txtTcpPort.Text),
                                            Convert.ToInt32(txtUdpPort.Text),
                                            version,
                                            500);


                LabelStatus.Text = Result.ToString();

            }
        }

        private void btnGerVersionUdp_Click(object sender, EventArgs e)
        {
            byte version = 0;
            if (cmbDeviceType.Text == "Cezeri T&A")
            {
                Result = cm.GetDeviceVersionUDP("FF-FF-FF-FF-FF-FF",
                                            txtIP.Text,
                                            Convert.ToInt32(txtTcpPort.Text),
                                            Convert.ToInt32(txtUdpPort.Text),
                                            out version,
                                            500);

                switch (version)
                {
                    case 1: cmbVersion.SelectedIndex = 1; break;
                    case 2: cmbVersion.SelectedIndex = 2; break;
                    case 3: cmbVersion.SelectedIndex = 3; break;
                    default: cmbVersion.SelectedIndex = 0; break;
                }
                LabelStatus.Text = Result.ToString();

            }
        }
        rfMultiLibrary.PhysicalCommunication tactibbo = new rfMultiLibrary.PhysicalCommunication();
        public string[] Macs, IPs, GWs, SMs, Ports, Bauds, Pars, Datas, Stops, Flows, Names, Protocols;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int[] Targs;
                lbDevices.Items.Clear();
                LabelStatus.Text = "Bekleyin"; this.Refresh();

                Result = cm.GetFsmDevices(txtIP.Text, Convert.ToInt32(txtTcpPort.Text), out Targs, 500, Cnv);

                if (Targs == null) Targs = new int[0];

                for (int i = 0; i < Targs.Length; i++)
                    lbDevices.Items.Add(Targs[i].ToString());

                if (Targs.Length > 0)
                {
                    //getConfigToolStripMenuItem.PerformClick();
                    LabelStatus.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
                }
                else LabelStatus.Text = "Cihaz bulununamadı.";
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void cbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIP.Text = cbDevices.Text;
        }








    }
}
