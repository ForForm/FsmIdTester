using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using rfMultiLibrary;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace FSM_Authorization
{
    public partial class Boot : Form
    {
        public Boot()
        {
            InitializeComponent();
        }

        Label[] lbl;
        Button[] btn;
        
        int length = 12;

        DeviceSettings[] Dvc;
        
        ConnectionManager fsm = new ConnectionManager();

        public class device
        {
            public string deviceIpAddress;
            public string[] deviceAddress;
        }

        public device[] deviceAddress = new device[16];
        public device[] ListDevice;
        ConnectionManager.ReturnValues Result;

        ConnectionManager.Converter ConverterType = ConnectionManager.Converter.NewConv;

        private void Boot_Load(object sender, EventArgs e)
        {
            items();
            for (int i = 0; i < 16; i++)
            {
                loadScreen(LoadScreenMode.hide, i, 0, "");
            }

            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            toolStripButton2.PerformClick();
        }

        public enum LoadScreenMode
        {
            hide = 0,
            show = 1,
            compare = 2,
            write = 3,
            read = 4,
            load = 5
        }

        GroupBox[] gBox;
        ProgressBar[] pdBar, plBar;
        Label[] lDevice, lLoad;

        void items()
        {
            gBox = new GroupBox[16] { groupBox1, groupBox2, groupBox4, groupBox5, groupBox9, groupBox8, groupBox7, groupBox6, groupBox13, groupBox12, groupBox11, groupBox10, groupBox17, groupBox16, groupBox15, groupBox14 };
            pdBar = new ProgressBar[16] { progressBar1, progressBar3, progressBar5, progressBar7, progressBar15, progressBar13, progressBar11, progressBar9, progressBar23, progressBar21, progressBar19, progressBar17, progressBar31, progressBar29, progressBar27, progressBar25 };
            plBar = new ProgressBar[16] { progressBar2, progressBar4, progressBar6, progressBar8, progressBar16, progressBar14, progressBar12, progressBar10, progressBar24, progressBar22, progressBar20, progressBar18, progressBar32, progressBar30, progressBar28, progressBar26 };
            lDevice = new Label[16] { label1, label3, label5, label7, label15, label13, label11, label9, label23, label21, label19, label17, label31, label29, label27, label25};
            lLoad = new Label[16] { label2, label4, label6, label8, label16, label14, label12, label10, label24, label22, label20, label18, label32, label30, label28, label26};
        }

        public string loadScreen(LoadScreenMode mode, int number, int fsmnumber, string IpAddress)
        {
            if (mode == LoadScreenMode.load)
            {
                plBar[number].Value += 1;
                lLoad[number].Text = ((plBar[number].Maximum / 100) * plBar[number].Value).ToString();
            }

            if (mode == LoadScreenMode.read)
                return gBox[number].Text;
            if (mode == LoadScreenMode.write)
                gBox[number].Text = IpAddress;

            if (mode == LoadScreenMode.hide)
            {
                gBox[number].Hide();
                return "null";
            }
            else
                gBox[number].Show();

            lDevice[number].Text = deviceAddress[number].deviceAddress[0].ToString() + "/" + (1).ToString() + "/" + deviceAddress[number].deviceAddress.Length.ToString();
            gBox[number].Text = IpAddress;
            pdBar[number].Value = 0;
            plBar[number].Value = 0;
            return "null";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lbDevices.SelectedIndex != -1)
            {
                if (lbAddress.Items.Count < 0)
                {
                    MessageBox.Show("Yükleme işlemi için mevcut bir TTFSM cihazı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (lbAddress.SelectedIndices.Count < 1)
                {
                    MessageBox.Show("Lütfen, yüklenecek TTFsm cihazını seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string ipAddress = lbDevices.Items[lbDevices.SelectedIndex].ToString();
                for (int x = 0; x < lbDevices.Items.Count; x++)
                {
                    string status = loadScreen(LoadScreenMode.read, x, 0, "");
                    if (status == "null")
                    {
                        deviceAddress[x] = new device();
                        deviceAddress[x].deviceIpAddress = ipAddress;// lbDevices.SelectedItems[x].ToString();
                        deviceAddress[x].deviceAddress = new string[lbAddress.SelectedIndices.Count];
                        for (int y = 0; y < lbAddress.SelectedIndices.Count; y++)
                        {
                            deviceAddress[x].deviceAddress[y] = lbAddress.SelectedItems[y].ToString();
                            loadScreen(LoadScreenMode.write, x, y, deviceAddress[x].deviceIpAddress);
                        }
                        break;
                    }
                    else if (status == ipAddress)
                    {
                        deviceAddress[x].deviceIpAddress = ipAddress;
                        deviceAddress[x].deviceAddress = new string[lbAddress.SelectedIndices.Count];
                        for (int y = 0; y < lbAddress.SelectedIndices.Count; y++)
                        {
                            deviceAddress[x].deviceAddress[y] = lbAddress.SelectedItems[y].ToString();
                            loadScreen(LoadScreenMode.write, x, y, deviceAddress[x].deviceIpAddress);
                        }
                        break;
                    }
                }
            }
            else
                MessageBox.Show("Yükleme işlemi için mevcut bir CONVERTER cihaz bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        PhysicalCommunication tactibbo = new PhysicalCommunication();
        PhysicalCommunication.ReturnValues rvalue;
        public string[] Macs, IPs, GWs, SMs, Ports, Bauds, Pars, Datas, Stops, Flows, Names, Protocols;

        int ConverterNumber = 0, FsmNumber = 0;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string[] sIP = new string[256];
            ConverterNumber = 0; FsmNumber = 0;
            lbDevices.Items.Clear();
            int deviceIndex = 0;
            try
            {               
                Macs = IPs = GWs = SMs = Ports = Bauds = Pars = Datas = Stops = Flows = Protocols = Names = null;

                if (cbConverType.Text == "TT100")
                {
                    if (tactibbo.GetLocalDevices_v2(out Macs, out IPs, out GWs, out SMs, out Ports, out Bauds, out Pars, out Datas, out Stops, out Flows, out Protocols, out Names) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                    {
                        for (int i = 0; i < Macs.Length; i++)
                        {
                            sIP[i] = IPs[i];
                        }
                        deviceIndex = Macs.Length;
                    }
                }
                else
                {
                    if (tactibbo.GetLocalDevices(out Dvc) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
                    {
                        for (int i = 0; i < Dvc.Length; i++)
                        {
                            sIP[i] = Dvc[i].d_IP;
                        }
                        deviceIndex = Dvc.Length;
                    }
                }

                string[] oIP = new string[deviceIndex];
                Array.Copy(sIP, oIP, deviceIndex);
                Array.Sort(oIP);

                for (int i = 0; i < deviceIndex; i++) //ip sıralaması
                {
                    lbDevices.Items.Add(oIP[i]);
                }

                if (lbDevices.Items.Count > 0)
                    lbDevices.SelectedIndex = 0;

                ConverterNumber += lbDevices.Items.Count;
                lbCon.Text = ConverterNumber.ToString();
                //pBar.Value = 100;
                //cmbIPs.SelectedIndex = 0;

            }
            catch
            { }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            FsmNumber = 0;
            AllFsmDecive();
           // int[] Targs; int[] Targ;
           // lbAddress.Items.Clear();
           //// rValue.Text = "Bekleyin"; pBar.Value = 0; this.Refresh();

           // fsm.GetFsmDevices(cmbIPs.Text, Convert.ToInt32(textPort.Text), out Targs, out Targ, 300, Cnv);

           // if (Targs == null) Targs = new int[0];

           // for (int i = 0; i < Targs.Length; i++)
           //     lbDevices.Items.Add(Targs[i].ToString() + Convert.ToChar(Targ[i]).ToString());

           // if (Targs.Length > 0)
           // {
           //     lbDevices.SelectedIndex = 0;
           //     GetDeviceInfo();

           //     rValue.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
           //     //pBar.Value = 100;
           // }
           // else rValue.Text = "Cihaz bulununamadı.";
        }

        Thread[] FindallFsm;
        public void AllFsmDecive()
        {
            try
            {
                if (lbDevices.Items.Count < 0) return;
                if (ListDevice != null)
                    ListDevice = null;
                ListDevice = new device[lbDevices.Items.Count];
                FindallFsm = new Thread[lbDevices.Items.Count];
                object[] objData = new object[2];
                for (int z  = 0; z < lbDevices.Items.Count; z++)
                {
                    try
                    {
                        ListDevice[z] = new device();
                        ListDevice[z].deviceIpAddress = lbDevices.Items[z].ToString();
                        objData[0] = z;
                        objData[1] = ListDevice[z].deviceIpAddress;                      
                        FindallFsm[z] = new Thread(() => ThreahFindAllFsmAddress(objData));//ThreahFindAllFsmAddress(z, ListDevice[z].deviceIpAddress));
                        FindallFsm[z].Start();
                        Thread.Sleep(20);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        void ThreahFindAllFsmAddress(object[] obj)
        {
            int[] Targs; int[] Targ;
            string sIp = (string)obj[1];
            int number = (int)obj[0];
            try
            {
                Result = fsm.GetFsmDevices(sIp, 1001, out Targs, out Targ, 1500, ConverterType);
                if (Result != ConnectionManager.ReturnValues.Successful)
                    fsm.GetFsmDevices(sIp, 1001, out Targs, out Targ, 1500, ConverterType);

                if (Targs == null)
                {
                    FindallFsm[number].Abort();
                    return;// Targs = new int[0];
                }

                ListDevice[number].deviceAddress = new string[Targs.Length];
                for (int i = 0; i < Targs.Length; i++)
                {
                    ListDevice[number].deviceAddress[i] = (Targs[i].ToString() + Convert.ToChar(Targ[i]).ToString());
                }

                if (lbDevices.Items[lbDevices.SelectedIndex].ToString() == sIp && Targs.Length != 0)
                {
                    lbAddress.Items.Clear();
                    for (int i = 0; i < Targs.Length; i++)
                        lbAddress.Items.Add(ListDevice[number].deviceAddress[i]);
                }
                FsmNumber += Targs.Length; 
                lbFsm.Text = FsmNumber.ToString();
                FindallFsm[number].Abort();
            }
            catch (Exception ex)
            {
                FindallFsm[number].Abort();
                throw;
            }

            //if (Targs.Length > 0)
            //{
            //    lbDevices.SelectedIndex = 0;
            //    GetDeviceInfo();

            //    rValue.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
            //    //pBar.Value = 100;
            //}
            //else rValue.Text = "Cihaz bulununamadı.";
        }

        void ThreahFindAllFsmAddress(int number, string ip)
        {
            try
            {
                int[] Targs; int[] Targ;
                Result = fsm.GetFsmDevices(ip, 1001, out Targs, out Targ, 1500, ConverterType);
                if(Result != ConnectionManager.ReturnValues.Successful)
                    fsm.GetFsmDevices(ip, 1001, out Targs, out Targ, 1500, ConverterType);

                if (Targs == null)
                {
                    FindallFsm[number].Abort();
                    return;// Targs = new int[0];
                }

                ListDevice[number].deviceAddress = new string[Targs.Length];
                for (int i = 0; i < Targs.Length; i++)
                {
                    ListDevice[number].deviceAddress[i] = (Targs[i].ToString() + Convert.ToChar(Targ[i]).ToString());
                }

                if (lbDevices.Items[lbDevices.SelectedIndex].ToString() == ip && Targs.Length != 0)
                {
                    lbAddress.Items.Clear();
                    for (int i = 0; i < Targs.Length; i++)
                        lbAddress.Items.Add(ListDevice[number].deviceAddress[i]);
                }

                FindallFsm[number].Abort();
            }
            catch (Exception ex)
            {
                FindallFsm[number].Abort();
                throw;
            }

            //if (Targs.Length > 0)
            //{
            //    lbDevices.SelectedIndex = 0;
            //    GetDeviceInfo();

            //    rValue.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
            //    //pBar.Value = 100;
            //}
            //else rValue.Text = "Cihaz bulununamadı.";
        }

        private void lbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDevices.Items.Count <= 0) return;
            try
            {
                for (int i = 0; i < lbDevices.Items.Count; i++)
                {
                    if (ListDevice == null)
                        continue;

                    if (lbDevices.Items[lbDevices.SelectedIndex].ToString() == ListDevice[i].deviceIpAddress)
                    {
                        lbAddress.Items.Clear();
                        if (ListDevice[i].deviceAddress != null)
                        {
                            for (int x = 0; x < ListDevice[i].deviceAddress.Length; x++)
                            {
                                lbAddress.Items.Add(ListDevice[i].deviceAddress[x]);
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        Thread[] FsmLoadData = new Thread[16];
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int z = 0; z < 16; z++)
                {
                    if (deviceAddress[z] != null)
                        break;
                    if (z == 15)
                    {
                        MessageBox.Show("Lütfen, öncelikle cihaz ekleyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                OpenFileDialog hexfile = new OpenFileDialog();
                hexfile.Title = "Select BIN File";
                hexfile.Filter = "Binary File (*.hex)|*.hex";
                hexfile.CheckFileExists = false;
                hexfile.ShowDialog();
                textBox1.Text = hexfile.FileName;

                if (hexfile.FileName == "")
                {
                    MessageBox.Show("Lütfen dosya yolunu belirtiniz.");
                    return;
                }
                
                FileStream file = new FileStream(textBox1.Text, FileMode.Open); //Dosyayı aç
                string link = textBox1.Text;
                HexCode[] hcd = GetFile(file);
                object[] objData = new object[3];
                if (hcd != null)
                {
                    for (int i = 0; i < 16; i++)
			        {
                        if (deviceAddress[i] != null)
                        {
                            objData[0] = hcd;
                            objData[1] = deviceAddress[i];
                            objData[2] = i;
                            FsmLoadData[i] = new Thread(() => BootloaderObj(objData));//Bootloader(hcd, deviceAddress[i], i));
                            FsmLoadData[i].Start();
                            Thread.Sleep(100);
                        }
                        else break;
			        }                  
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        HexCode[] GetFile(FileStream file)
        {
            #region Get File
            int deger = 224/32;
            HexCode[] hcd = new HexCode[1000];
            HexCode hc = new HexCode();
            //---------------------------------------------------------------------------------------------------------
            //FileStream file = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.txt", FileMode.Open);
            StreamReader sr = new StreamReader(file);
            string sData = ""; 
            int Count = 1000, hCount = 0, Cplus = 0; 
            string xData = "", lData = "", Adr = "", lAdr = ""; 
            bool Datafinish = true;
            int LastAddress = 0xFFE0;// tRegister = "";
       
            for (int i = 0; i < 1000; i++)
            {
                sData = sr.ReadLine();// Oku

                if (sData.Substring(0, 7) == ":208800") //Başlangıç adresini bekle
                    Count = i;

                if (Count <= i) //Başalangıç adresi geldiyse
                {
                    Adr = sData.Substring(3, 4); //önce adresi oku
                    xData = sData.Substring(9, (sData.Length - 11));// sonra datayı oku

                    if (Datafinish)//Data bitti mi
                    {
                        if (xData.Length >= 14 && (xData.Substring(0, 14).ToString() == "01000C06000004" && xData.Substring(xData.Length - 14, 14).ToString() == "000100DB044500"))
                        {
                            if (Cplus != 0)// bitis kısmında index kaldıysa önce var olanı yaz.
                            {
                                hcd[hCount++] = hc.SetData(lAdr, lData);
                            }
                            Cplus = (deger - 3); lData = ""; lAdr = Adr; Datafinish = false;
                        }
                    }

                    if (Cplus == 0)
                        lAdr = Adr;

                    byte[] bAdr = ConnectionManager.StrToByteArray(Adr);
                    int iAddress = ((bAdr[0] * 256) + bAdr[1]);
                    if (iAddress >= 0xFFE0)
                    {
                        if (Cplus > 0)
                        {
                            hcd[hCount++] = hc.SetData(lAdr, lData);
                            Cplus = 0; lAdr = ""; lData = "";
                        }

                        int kalan = iAddress - LastAddress;
                        for (int x = 0; x < kalan; x++)
                        {
                            lData += "FF";
                        }
                        lData += xData;

                        LastAddress = ((bAdr[0] * 256) + bAdr[1]) + 2;
                        if (LastAddress >= 0xFFFE)
                        {
                            hcd[hCount++] = hc.SetData("FFE0", lData);
                            Cplus = 0; lAdr = ""; lData = "";
                        }
                    }
                    else if (iAddress == 0) break;
                    else
                    {
                        lData += xData; Cplus++;
                        if (Cplus == deger)
                        {
                            hcd[hCount++] = hc.SetData(lAdr, lData);
                            Cplus = 0; lAdr = ""; lData = "";
                        }
                    }
                }
            }
            HexCode[] nhcd = new HexCode[hCount];
            for (int i = 0; i < hCount; i++)
            {
                nhcd[i] = new HexCode();
                nhcd[i].Addrress = hcd[i].Addrress;
                nhcd[i].Data = hcd[i].Data;
            }
            sr.Close();
            return nhcd;
            #endregion
        }

        private void BootloaderObj(object[] obj)
        {
            try
            {
                HexCode[] hcd = (HexCode[])obj[0];
                device dvc = (device)obj[1];
                int number = (int)obj[2];

                TcpClient client;
                int Repeat;
                bool error = false;

                #region Change To Boot
                //---------------------------------------------------------------------------------------------------------
                Repeat = 1;
                lLoad[number].Text = "Boot Mode";
                Thread.Sleep(250);

                int gAdres = 255;

                while (fsm.ChangeWorkingMode(dvc.deviceIpAddress, 1001, gAdres, ConnectionManager.WorkingModes.OfflineMode, 1500, ConverterType) != ConnectionManager.ReturnValues.Successful)
                {
                    if (Repeat++ > 3)
                    {
                        break;
                    }
                }
                #endregion

                for (int z = 0; z < dvc.deviceAddress.Length; z++)
                {
                //Again:
                    error = false;
                    int dadr = Convert.ToInt32(dvc.deviceAddress[z]);
                    pdBar[number].Maximum = dvc.deviceAddress.Length;
                    lDevice[number].Text = dadr + "/" + (z + 1).ToString() + "/" + dvc.deviceAddress.Length.ToString();

                    #region Change To Boot
                    //---------------------------------------------------------------------------------------------------------
                    Repeat = 1;
                    lLoad[number].Text = "Boot Mode";
                    Thread.Sleep(250);

                    while (fsm.SendBootRequest(dvc.deviceIpAddress, 1001, dadr, 1500, ConverterType) != ConnectionManager.ReturnValues.Successful)
                    {
                        if (Repeat++ > 3)
                        {
                            break;
                        }
                    }
                    #endregion
                    //---------------------------------------------------------------------------------------------------------
                    #region Erase Flash
                    //---------------------------------------------------------------------------------------------------------
                    Repeat = 1;
                    lLoad[number].Text = "Deleting Flash";
                    while (fsm.ESendBootBytesNew(dvc.deviceIpAddress, 1001, dadr, (byte)'E', 0, 2500, ConverterType) != ConnectionManager.ReturnValues.Successful)
                    {
                        if (Repeat++ >= 10)
                        {
                            lLoad[number].Text = "stoped";
                            Thread.Sleep(1500);
                            deviceAddress[number] = null;
                            gBox[number].Text = "null";
                            gBox[number].Hide();
                            FsmLoadData[number].Abort();

                            error = true;
                            return;
                        }
                    }
                    //if (error) continue;
                    #endregion
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    plBar[number].Maximum = hcd.Length;
                    client = new TcpClient();
                    client.Connect(dvc.deviceIpAddress, 1001);
                    if (!client.Connected)
                    {
                        return;
                    }

                    lLoad[number].Text = "Loading datas";
                    for (int i = 0; i < hcd.Length; i++)
                    {
                        //-----------------------------------------------------------------------------------------------------
                        #region Load Flash Code
                        //---------------------------------------------------------------------------------------------------------
                        Repeat = 0;
                        while ((Result = fsm.SendBootBytesNew(client, dadr, (byte)'A', hcd[i].Data, hcd[i].Addrress, Convert.ToByte(hcd[i].Data.Length), 3000, ConverterType)) != ConnectionManager.ReturnValues.Successful)
                        {
                            if (Repeat++ > 5)
                            {
                                break;//return;
                            }
                            else
                            {
                                if (!client.Connected)
                                {
                                    client = new TcpClient();
                                    client.Connect(dvc.deviceIpAddress, 1001);
                                }
                            }
                        }

                        double Dgr = ((double)100 / (double)hcd.Length) * (i + 1);
                        plBar[number].Value = (i + 1);
                        string Str = "%" + Dgr.ToString();
                        if (Str.Length == 3) Str = Str + ".0";
                        else if (Str == "%100")
                        {
                            Str = "%100 ";
                            plBar[number].Maximum = 100;
                            plBar[number].Value = 100;
                        }
                        if (Str.Length > 5)
                            lLoad[number].Text = Str.Substring(0, 5);
                        else lLoad[number].Text = Str;

                        #endregion
                    }
                    if (client.Connected)
                        client.Close();
                    pdBar[number].Value += 1;
                    //---------------------------------------------------------------------------------------------------------
                    #region End Loadind & Reboot
                    //---------------------------------------------------------------------------------------------------------  
                    #endregion
                }
                lLoad[number].Text = "Successful";
                Thread.Sleep(1500);
                deviceAddress[number] = null;
                gBox[number].Text = "null";
                gBox[number].Hide();
                lLoad[number].Text = "";
                FsmLoadData[number].Abort();
            }
            catch (Exception)
            {
            }
        }

        private void Bootloader(HexCode[] hcd, device dvc, int number)
        {
            try
            {

                TcpClient client;
                int Repeat;
                bool error = false;
                for (int z = 0; z < dvc.deviceAddress.Length; z++)
                {
                Again:
                    error = false;
                    int dadr = Convert.ToInt32(dvc.deviceAddress[z]);
                    pdBar[number].Maximum = dvc.deviceAddress.Length;
                    lDevice[number].Text = dadr + "/" + (z + 1).ToString() + "/" + dvc.deviceAddress.Length.ToString();
                    //OpenComPort();
                    //---------------------------------------------------------------------------------------------------------
                    #region Change To Boot
                    //---------------------------------------------------------------------------------------------------------
                    Repeat = 1;
                    while (fsm.SendBootRequest(dvc.deviceIpAddress, 1001, dadr, 1500, ConverterType) != ConnectionManager.ReturnValues.Successful)
                    {
                        if (Repeat++ > 3)
                        {
                            //StopUpdateThread();
                            break;
                        }
                        // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
                    }
                    #endregion

                    //if (Repeat < 6)
                    //    Debug(DbgMsgType.Warning, "BootLoader Mode (Successful) OK...\r\n", true);

                    //---------------------------------------------------------------------------------------------------------
                    #region Erase Flash
                    //---------------------------------------------------------------------------------------------------------
                    Repeat = 1;
                    //Debug(DbgMsgType.Normal, "**** FLASH ERASING ****", true);
                    //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
                    while (fsm.ESendBootBytes(dvc.deviceIpAddress, 1001, dadr, (byte)'E', 0, 1500, ConverterType) != ConnectionManager.ReturnValues.Successful)
                    {
                        if (Repeat++ >= 5)
                        {
                            //StopUpdateThread();
                            //Debug(DbgMsgType.Incoming, "Couldnt Erase, Loader Stoped", true);
                            //return;
                            error = true;
                            break;
                        }
                        // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
                    }
                    if (error) continue;
                    #endregion
                    //if (Repeat < 10)
                    //    Debug(DbgMsgType.Warning, "Flah Erased (Successful) OK...\r\n", true);
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    plBar[number].Maximum = hcd.Length;

                    //Debug(DbgMsgType.Normal, "**** BOOTLOADER STARTED ****", true);


                    client = new TcpClient();
                    client.Connect(dvc.deviceIpAddress, 1001);
                    if (!client.Connected)
                    {
                        //Debug(DbgMsgType.Error, "Load Stoped ", true);
                        //update_thread.Abort();
                        return;
                    }


                    for (int i = 0; i < hcd.Length; i++)
                    {
                        //StopUpdateThread();
                        //-----------------------------------------------------------------------------------------------------
                        #region Load Flash Code
                        //---------------------------------------------------------------------------------------------------------
                        Repeat = 0;

                        //if (hcd[i].Addrress == 0xffee) 
                        //    break;

                        while ((Result = fsm.SendBootBytes(client, dadr, (byte)'A', hcd[i].Data, hcd[i].Addrress, Convert.ToByte(hcd[i].Data.Length), 1500, ConverterType)) != ConnectionManager.ReturnValues.Successful)
                        {
                            //    StopUpdateThread();
                            if (Repeat++ > 5)
                            {
                                while ((Result = fsm.SendBootBytes(client, dadr, (byte)'R', null, 0, 0, 1500, ConverterType)) != ConnectionManager.ReturnValues.Successful)
                                {
                                    if (Repeat++ >= 2)
                                    {
                                        //StopUpdateThread();
                                        //Debug(DbgMsgType.Incoming, "Device Not Restarted", true);
                                        //return;
                                        Thread.Sleep(1000);
                                        goto Again;
     
                                    }
                                    // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
                                }
                                Thread.Sleep(1000);
                                goto Again;// break;//return;
                            }
                            else
                            {

                                if (!client.Connected)
                                {
                                    client = new TcpClient();
                                    client.Connect(dvc.deviceIpAddress, 1001);
                                }
                                //if (Repeat > 5)
                                //   Debug(DbgMsgType.Error, i.ToString() + "  " + Result.ToString(), true);
                                //Debug(DbgMsgType.Error, L_ADD.ToString() + "  " + Result.ToString(), true);
                            }
                        }
                        //if (hcd[i].Addrress == 0xfffe)
                        //{
                        //    Thread.Sleep(15);
                        //    fsm.SendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)'A', hcd[i].Data, hcd[i].Addrress, Convert.ToByte(hcd[i].Data.Length), Convert.ToInt32(textTimeOut.Text), Cnv);
                        //}
                        //Debug(DbgMsgType.Incoming, "Data Pack:   " + i.ToString() + "   ACK OK", true);//+ (char)9 + DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###")
                        double Dgr = ((double)100 / (double)hcd.Length) * (i + 1); 
                        plBar[number].Value = (i + 1);
                        string Str = "%" + Dgr.ToString();
                        //if (Str.Length == 2) Str = Str + ".00";
                        //else 
                        if (Str.Length == 3) Str = Str + ".0";
                        else if (Str == "%100")
                        {
                            Str = "%100 ";
                            plBar[number].Maximum = 100;
                            plBar[number].Value = 100;
                        }
                        if(Str.Length > 5)
                            lLoad[number].Text = Str.Substring(0, 5);
                        else lLoad[number].Text = Str;

                        #endregion
                    }

                    if (error) continue;
                    #region Device Reset
                    //---------------------------------------------------------------------------------------------------------
                    Repeat = 1;
                    //Debug(DbgMsgType.Normal, "**** Restart Device ****", true);
                    //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
                    while ((Result = fsm.SendBootBytes(client, dadr, (byte)'R', null, 0, 0, 1500, ConverterType)) != ConnectionManager.ReturnValues.Successful)
                    {
                        if (Repeat++ >= 2)
                        {
                            //StopUpdateThread();
                            //Debug(DbgMsgType.Incoming, "Device Not Restarted", true);
                            //return;
                            break;
                        }
                        // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
                    }
                    if (error) continue;
                    #endregion
                    pdBar[number].Value += 1;
                    //---------------------------------------------------------------------------------------------------------
                    #region End Loadind & Reboot
                    //---------------------------------------------------------------------------------------------------------  
                    //Debug(DbgMsgType.Warning, "Load Finished", true);
                    //Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " sn\r\n", true);
                    //Debug(DbgMsgType.Normal, "Reboot Successful ...", true);
                    //this.Refresh();
                    #endregion
                }
                Thread.Sleep(1500);
                deviceAddress[number] = null;
                gBox[number].Text = "null";
                gBox[number].Hide();
                FsmLoadData[number].Abort();
              }
            catch (Exception)
            {
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
   
        }

        private void cbConverType_Click(object sender, EventArgs e)
        {

        }

        private void cbConverType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConverType.SelectedItem == "TT100")
                ConverterType = ConnectionManager.Converter.Tac;
            else
                ConverterType = ConnectionManager.Converter.NewConv;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void defaultToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                if (deviceAddress[i] != null)
                {
                    if (FsmLoadData[i].IsAlive)
                        FsmLoadData[i].Abort();
                    Thread.Sleep(100);
                }
                else break;
            }   
        }
    }
}
