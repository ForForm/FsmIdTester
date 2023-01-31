using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Microsoft.VisualBasic;
using FSM.Properties;
using System.Net.Sockets;

namespace FSM_Authorization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ConnectionManager fsm = new ConnectionManager();
        PhysicalDevices PhyDev = new PhysicalDevices();
        PhysicalDevices[] Devices = null;
        ConnectionManager.ReturnValues Result;
        ConnectionManager.Converter Cnv = ConnectionManager.Converter.Tac;  
        SimpleAES SMP = new SimpleAES();
        Settings setting = new Settings();
        FsmInfo fsmInfo = new FsmInfo();
        rfMultiLibrary.DeviceSettings[] Dvc;
        string[,] nIP = new string[100, 2];
        int CnvIndex = 0;
        Boot bootForm = new Boot();
        
        public enum DbgMsgType { Incoming, Outgoing, Normal, Warning, Error };
        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.OrangeRed, Color.Red };
        public Thread update_thread;
        bool thread_stop = false;

        
        public static DialogResult InputBox(string title, string promptText, out string value)
        {
            Form form = new Form();
            Label label = new Label();
            Label label1 = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            label1.Text = "Not: Sifre uzunluğu min. 6 karakter olmalıdır.";
            textBox.PasswordChar = '*';
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(40, 18, 344, 20);
            label1.SetBounds(40, 40, 344, 10);
            buttonOk.SetBounds(228, 52, 75, 23);
            buttonCancel.SetBounds(309, 52, 75, 23);

            label.AutoSize = true; label1.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(388, 85);
            form.Controls.AddRange(new Control[] { label, textBox, label1, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            if (value.Length < 6 || value == null)
                return DialogResult.Cancel;
            return dialogResult;
        }

        #region HexToByte
        public static byte[] HexToByte(string msg)
        {
            try
            {
                msg = msg.Replace(":", "");
                msg = msg.Replace(" ", "");
                byte[] comBuffer = new byte[msg.Length / 2];
                for (int i = 0; i < msg.Length; i += 2)
                    comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
                return comBuffer;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Cnv = ConnectionManager.Converter.Tac;
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false; //başka iç parçacığına erişildi
            btnList.PerformClick();
            Refresh.PerformClick();
            txtFilePath.Text = setting.FilePath;
            nudSerial.Value = setting.LastSerial;
            bootForm.Show();
        }

       
        private void StopUpdateThread()
        {
            if (thread_stop)
            {
                Debug(DbgMsgType.Warning, "Loading Stoped By User", true);
                Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " s\r\n", true);
                rValue.Text = ""; pBar.Value = 0;
                update_thread.Suspend();
            }
        }

        DateTime bgn;
        private void Bootloader1(FileStream file)
        {
            try
            {

            TcpClient client;
            HexCode[] hcd = new HexCode[1000];
            HexCode hc = new HexCode();
            int Repeat, deger = Convert.ToInt32(cmbPackLength.Text)/32; byte ByteOfLine = 0; byte LengtOfLine = 0; string strByte = "";
            byte DataPackLength = Convert.ToByte(cmbPackLength.Text);
            thread_stop = false;
            byte[] ByteData = new byte[DataPackLength];
            rtbDebug.Text = "";
            Debug(DbgMsgType.Incoming, "**** BOOTLOADER TO ADDRESS " + textAddress.Text + "***\r\n", false);
            //---------------------------------------------------------------------------------------------------------
            #region Get File
            //---------------------------------------------------------------------------------------------------------
            //FileStream file = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.txt", FileMode.Open);
            StreamReader sr = new StreamReader(file);
            string sData = "", sHex = ""; int Count = 1000, hCount = 0, Cplus = 0; string xData = "", lData = "", Adr = "", lAdr = ""; bool Datafinish = true;

            for (int i = 0; i < 1000; i++)
            {
                sData = sr.ReadLine();// Oku

                if (sData.Substring(0, 7) == ":208800") //Başlangıç adresini bekle
                    Count = i;

                if(Count <= i) //Başalangıç adresi geldiyse
                {
                    Adr = sData.Substring(3, 4); //önce adresi oku
                    xData = sData.Substring(9, (sData.Length-11));// sonra datayı oku

                    if (Datafinish)//Data bitti mi
                    {
                        if(xData.Length >= 14 && (xData.Substring(0, 14).ToString() == "01000C06000004" && xData.Substring(xData.Length - 14, 14).ToString() == "000100DB044500"))
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

                    if (Adr == "FFE6")
                        Cplus = (deger - 1);
                    if(Adr == "FFEE")
                        Cplus = (deger - 1);
                    if (Adr == "FFF2")
                        Cplus = (deger - 1);
                    if (Adr == "FFFA")
                        Cplus = (deger - 1);
                    if (Adr == "FFFE")
                        Cplus = (deger - 1);
                    if (Adr == "0000")
                        break;
                    //yeni ekledim hex kod sürekli farklı data sıralaması çıkardığından bazen hatalı yüklemiş oluyoruz onun için yeniden düzenleme yapıldı

                    lData += xData; Cplus++;
                    if (Cplus == deger)
                    {
                        hcd[hCount++] = hc.SetData(lAdr, lData);
                        Cplus = 0; lAdr = ""; lData = "";
                    }
                }
            }
            sr.Close();
            #endregion

            int dadr = Convert.ToInt32(textAddress.Text);
            //OpenComPort();
            //---------------------------------------------------------------------------------------------------------
            #region Change To Boot
            //---------------------------------------------------------------------------------------------------------
            Repeat = 1;
            Debug(DbgMsgType.Normal, "**** CHANGE DEVICE TO BOOT MODE ****", true);
            //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
            while (fsm.SendBootRequest(cmbIPs.Text, Convert.ToInt32(textPort.Text), dadr, Convert.ToInt32(textTimeOut.Text), Cnv) != ConnectionManager.ReturnValues.Successful)
            {
                if (Repeat++ > 5)
                {
                    //StopUpdateThread();
                    Debug(DbgMsgType.Error, "Coldnt Boot or Alredy On Boot", true);
                    break;
                }
                // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
            }
            #endregion
            if (Repeat < 6)
                Debug(DbgMsgType.Warning, "BootLoader Mode (Successful) OK...\r\n", true);

            //---------------------------------------------------------------------------------------------------------
            #region Erase Flash
            //---------------------------------------------------------------------------------------------------------
            Repeat = 1;
            Debug(DbgMsgType.Normal, "**** FLASH ERASING ****", true);
            //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
            while (fsm.ESendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), dadr, (byte)'E', 0, Convert.ToInt32(textTimeOut.Text), Cnv) != ConnectionManager.ReturnValues.Successful)
            {
                if (Repeat++ >= 10)
                {
                    //StopUpdateThread();
                    Debug(DbgMsgType.Incoming, "Couldnt Erase, Loader Stoped", true);
                    //return;
                    break;
                }
                // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
            }
            #endregion
            if (Repeat < 10)
                Debug(DbgMsgType.Warning, "Flah Erased (Successful) OK...\r\n", true);
            ////////////////////////////////////////////////////////////////////////////////////////////////
            pBar.Maximum = hCount;
            bgn = DateTime.Now;
            Debug(DbgMsgType.Normal, "**** BOOTLOADER STARTED ****", true);

            
            client  = new TcpClient();
            client.Connect(cmbIPs.Text, Convert.ToInt32(textPort.Text));
            if (!client.Connected)
            {
                Debug(DbgMsgType.Error, "Load Stoped ", true);
                update_thread.Abort();
                return;
            }

            
            for (int i = 0; i < hCount; i++)
            {
                //StopUpdateThread();
                //-----------------------------------------------------------------------------------------------------
                #region Load Flash Code
                //---------------------------------------------------------------------------------------------------------
                ByteOfLine = 0;
                Repeat = 0;

                //if (hcd[i].Addrress == 0xffee) 
                //    break;

                while ((Result = fsm.SendBootBytes(client, dadr, (byte)'A', hcd[i].Data, hcd[i].Addrress, Convert.ToByte(hcd[i].Data.Length), Convert.ToInt32(textTimeOut.Text), Cnv)) != ConnectionManager.ReturnValues.Successful)
                {
                //    StopUpdateThread();
                    if (Repeat++ > 40)
                    {
                       Debug(DbgMsgType.Error, "Load Stoped ", true);
                       Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " s\r\n", true);
                       return;
                    }
                    else
                    {

                        if (!client.Connected)
                        {
                            client = new TcpClient();
                            client.Connect(cmbIPs.Text, Convert.ToInt32(textPort.Text));
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
                      Debug(DbgMsgType.Incoming, "Data Pack:   " + i.ToString() + "   ACK OK", true);//+ (char)9 + DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###")
                      double Dgr = ((double)100 / (double)hCount) * (i + 1); pBar.Value = (i+1); string Str = "%" + Dgr.ToString();
                      if (Str.Length == 3) Str = Str + ".0";
                      else if (Str == "%100") { Str = "%100 "; pBar.Maximum = 100; pBar.Value = 100;}
                      rValue.Text = Str.Substring(0,5);
                      
                #endregion
            }

            #region Device Reset
            //---------------------------------------------------------------------------------------------------------
            Repeat = 1;
            Debug(DbgMsgType.Normal, "**** Restart Device ****", true);
            //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
            while ((Result = fsm.SendBootBytes(client, Convert.ToInt32(textAddress.Text), (byte)'R', null, 0, 0, Convert.ToInt32(textTimeOut.Text), Cnv)) != ConnectionManager.ReturnValues.Successful)
            {
                if (Repeat++ >= 2)
                {
                    //StopUpdateThread();
                    Debug(DbgMsgType.Incoming, "Device Not Restarted", true);
                    //return;
                    break;
                }
                // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
            }
            #endregion

            //---------------------------------------------------------------------------------------------------------
            #region End Loadind & Reboot
            //---------------------------------------------------------------------------------------------------------  
            Debug(DbgMsgType.Warning, "Load Finished", true);
            Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " sn\r\n", true);
            Debug(DbgMsgType.Normal, "Reboot Successful ...", true);
            this.Refresh();
            #endregion

            }
            catch (Exception)
            {
            }
        }

        private void Bootloader(FileStream file)
        {
            int Repeat, VetorStartAdd = 100000; uint DataAddress = 0; byte ByteOfLine = 0; byte LengtOfLine = 0; string strByte = ""; int VectorEndAdd = 0;
            byte DataPackLength = Convert.ToByte(cmbPackLength.Text);
            thread_stop = false;
            byte[] ByteData = new byte[DataPackLength];
            rtbDebug.Text = "";
            Debug(DbgMsgType.Incoming, "**** BOOTLOADER TO ADDRESS " + textAddress.Text + "***\r\n", false);
            //---------------------------------------------------------------------------------------------------------
            #region Get File
            //---------------------------------------------------------------------------------------------------------
            //FileStream file = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.txt", FileMode.Open);
            StreamReader sr = new StreamReader(file);
            char[] Data = new char[100000];
            sr.Read(Data, 0, 100000);                                              //Veriyi oku
            for (int i = 0; i < 100000; i++) if (Data[i] == 'q') VectorEndAdd = i;
            sr.Close();
            #endregion

            //OpenComPort();
            //---------------------------------------------------------------------------------------------------------
            #region Change To Boot
            //---------------------------------------------------------------------------------------------------------
            Repeat = 1;
            Debug(DbgMsgType.Normal, "**** CHANGE DEVICE TO BOOT MODE ****", true);
            //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
            while (fsm.SendBootRequest(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv) != ConnectionManager.ReturnValues.Successful)
            {
                if (Repeat++ > 3)
                {
                    StopUpdateThread();
                    Debug(DbgMsgType.Error, "Coldnt Boot or Alredy On Boot", true);
                    break;
                }
               // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
            }
            #endregion
            if (Repeat < 4)
                Debug(DbgMsgType.Warning, "BootLoader Mode (Successful) OK...\r\n", true);

            //---------------------------------------------------------------------------------------------------------
            #region Erase Flash
            //---------------------------------------------------------------------------------------------------------
            Repeat = 1;
            Debug(DbgMsgType.Normal, "**** FLASH ERASING ****", true);
            //Debug(DbgMsgType.Normal, "[" + Repeat.ToString() + "]", true);
            while (fsm.SendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)'E', ByteData, 0, DataPackLength, Convert.ToInt32(textTimeOut.Text), Cnv) != ConnectionManager.ReturnValues.Successful)
            {
                if (Repeat++ >= 5)
                {
                    //StopUpdateThread();
                    Debug(DbgMsgType.Incoming, "Couldnt Erase, Loader Stoped", true);
                    //return;
                    break;
                }
               // Debug(DbgMsgType.Normal, " [" + Repeat.ToString() + "]", false);
            }
            #endregion
            if (Repeat < 5)
                Debug(DbgMsgType.Warning, "Flah Erased (Successful) OK...\r\n", true);
            //////////////////////////////////////////////////////////////////////////////////////////////////
            bgn = DateTime.Now;
            Debug(DbgMsgType.Normal, "**** BOOTLOADER STARTED ****", true);
            for (int i = 0; i <= 200000; i++)
            {
                StopUpdateThread();
                //-----------------------------------------------------------------------------------------------------
                #region Gets Code End Character
                if (Data[i] == 'q') { i = 1000001; continue; }
                #endregion
                //-----------------------------------------------------------------------------------------------------
                #region GetVectorStart
                //---------------------------------------------------------------------------------------------------------
                if (Data[i] == '@')
                {
                    string Vector = "";
                    Vector += Data[i + 1];
                    Vector += Data[i + 2];
                    Vector += Data[i + 3];
                    Vector += Data[i + 4];

                    DataAddress = Convert.ToUInt16(Vector, 16);//Convert.ToUInt16(Vector, 16);
                    uint L_ADD = DataAddress;
                    //this.Text = L_ADD.ToString("X");
                    if (DataAddress == (uint)0x8000) continue;    //Bootloader Kısmını Alma
                    if (DataAddress == (uint)0x8250) continue;    //Bootloader Kısmını Alma
                    VetorStartAdd = i + 7;

                    //if (LineIndex == (uint)0xfffe)   //Bootloader Kısmını Alma
                    //VetorStartAdd = i + 7;
                }
                #endregion
                //-----------------------------------------------------------------------------------------------------
                #region Load Flash Code
                //---------------------------------------------------------------------------------------------------------
                if (i >= VetorStartAdd)
                {
                    if (Data[i] != '\n' && Data[i] != '\r' && Data[i] != ' ')   //Gelen datayı ayıkla
                        strByte += Data[i];                                     //Veriyi al
                    if (Data[i] == ' ')
                    {
                        strByte = strByte.Trim();
                        ByteData[ByteOfLine] = Convert.ToByte(strByte, 16);
                        strByte = "";
                        ByteOfLine++;

                        if (Data[i + 3] == '@' || Data[i + 3] == 'q')           //Eğer Address Geliyorsa paketi tamamla
                            LengtOfLine = ByteOfLine;
                        else
                            LengtOfLine = DataPackLength;

                        if (ByteOfLine >= LengtOfLine)
                        {
                            ByteOfLine = 0;
                            Repeat = 0;
                            uint L_ADD = DataAddress;

                            while ((Result = fsm.SendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)'A', ByteData, DataAddress, LengtOfLine, Convert.ToInt32(textTimeOut.Text), Cnv)) != ConnectionManager.ReturnValues.Successful)
                            {
                                StopUpdateThread();
                                if (Repeat++ > 100)
                                {
                                    Debug(DbgMsgType.Error, "Load Stoped ", true);
                                    Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " s\r\n", true);
                                    return;
                                } 
                                else
                                {
                                    if (Repeat > 5)
                                        Debug(DbgMsgType.Error, i.ToString() + "  " + Result.ToString(), true);
                                        //Debug(DbgMsgType.Error, L_ADD.ToString() + "  " + Result.ToString(), true);
                                }
                            }
                            //Thread.Sleep(500);
                            //Debug(DbgMsgType.Incoming, Convert.ToInt32(L_ADD).ToString() + " ACK OK", true);
                            if (DataAddress == 0xfffe)
                                fsm.SendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)'A', ByteData, DataAddress, LengtOfLine, Convert.ToInt32(textTimeOut.Text), Cnv);
                           
                            Debug(DbgMsgType.Incoming, "Data Pack: " + (i - VetorStartAdd).ToString() + " ACK OK" + (char)9 + DataAddress, true);//+ (char)9 + DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###")
                            DataAddress += DataPackLength;
                            pBar.Value = 100 * (i - VetorStartAdd) / VectorEndAdd;
                            rValue.Text = "%" + pBar.Value.ToString();
                            //this.Refresh();
                        }
                    }
                }
                #endregion
            }
            //---------------------------------------------------------------------------------------------------------
            #region End Loadind & Reboot
            //---------------------------------------------------------------------------------------------------------
            Debug(DbgMsgType.Warning, "Load Finished", true);
            //Repeat = 0;
            //while (fsm.SendBootBytes(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (byte)'F', ByteData, 0xFFFFFFFF, 16, 200, Cnv) != ConnectionManager.ReturnValues.Succesfulll)
            //{
            //    if (Repeat++ > 10)
            //    {
            //        Debug(DbgMsgType.Error, "Load Stoped ", true);
            //        return;
            //    }
            //}
            Debug(DbgMsgType.Warning, DateTime.Now.Subtract(bgn).TotalSeconds.ToString("0.###") + " sn\r\n",true);
            Debug(DbgMsgType.Normal, "Reboot Successful ...", true);
            pBar.Value = 100; rValue.Text = "%100";  this.Refresh();
            #endregion
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
                    lbDevices.SelectedIndex = 0;
                    GetDeviceInfo();

                rValue.Text += "  " + Targs.Length.ToString() + "  cihaz bulundu.";
                //pBar.Value = 100;
            }
            else rValue.Text = "Cihaz bulununamadı.";

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            OpenFileDialog filePath = new OpenFileDialog();                       //Yüklenecek dosyayı seç
            if (filePath.ShowDialog() != DialogResult.OK) return;
            setting.FilePath = filePath.FileName;
            txtFilePath.Text = filePath.FileName;

            //OpenFileDialog binfile = new OpenFileDialog();
            //binfile.Title = "Select BIN File";
            //binfile.Filter = "Binary File (*.bin)|*.bin";
            //if (binfile.ShowDialog() != DialogResult.OK)
            //{
            //    Debug(DbgMsgType.Warning, "Cancelled", false);
            //    return;
            //}
            //setting.FilePath = binfile.FileName;
            //txtFilePath.Text = binfile.FileName;

            setting.Save();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //if (lbDevices.Items.Count != 0)
                //{
                    FileStream file = new FileStream(txtFilePath.Text, FileMode.Open); //Dosyayı aç
                    string link = txtFilePath.Text;
                    if (link.Substring(link.Length - 3, 3).ToString() == "hex")
                        update_thread = new Thread(() => Bootloader1(file));
                    else
                        update_thread = new Thread(() => Bootloader(file));

                    update_thread.Start();
                //}
                //else
                //   MessageBox.Show("Update için cihaz mevcut değil...");
            }
            catch (Exception ex )
            {
                MessageBox.Show("Kodun yolunu belirtiniz...");
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

        private void tsBtnChgAdd_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeDeviceAddress(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(tsTextAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful) { pBar.Value = 100; rValue.Text = Result.ToString(); textAddress.Text = tsTextAddress.Text; }
                else                                                    {   pBar.Value = 0;     rValue.Text = Result.ToString();    }
            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }
        }

        private void Debug(DbgMsgType msgtype, string msg, bool newLine)
        {
            rtbDebug.Invoke(new EventHandler(delegate
            {
                if (newLine) rtbDebug.AppendText("\r\n");
                rtbDebug.SelectedText = string.Empty;
                rtbDebug.SelectionFont = new Font(rtbDebug.SelectionFont, FontStyle.Regular);
                rtbDebug.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtbDebug.AppendText(msg);
                rtbDebug.ScrollToCaret();
            }));
        }

        private void GetDeviceInfo()
        {
            string Manufacturer, Device, Applicatin, PcbVer, FirmVer, Tester, Serial;
            DateTime PrdDate, TestDate;
            pBar.Value = 0;
            try
            {
                //Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                //    out Device,
                //    out Applicatin,
                //    out PcbVer,
                //    out PrdDate,
                //    out FirmVer,
                //    out Tester,
                //    out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);
                Result = fsm.GetDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text),
                        out Manufacturer, out Device, out Applicatin, out PcbVer, out PrdDate, out TestDate, out FirmVer,
                        out Tester, out Serial, Convert.ToInt32(textTimeOut.Text), Cnv);

                if (Result == ConnectionManager.ReturnValues.Successful)
                {
                    rtbDebug.Text = "";
                    if (Manufacturer == "BARKODES")
                    {
                        fsmInfo.SetFsmInfo(Manufacturer, Device, Applicatin, PcbVer, PrdDate, TestDate, FirmVer, Tester, Serial);
                        pgInfo.SelectedObjects = new object[] { fsmInfo };

                       // Debug(DbgMsgType.Normal, "Mnfctr:", false); Debug(DbgMsgType.Outgoing, Manufacturer, false);
                       // Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                        Debug(DbgMsgType.Normal, "Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                        Debug(DbgMsgType.Normal, " PcbV:", false); Debug(DbgMsgType.Outgoing, PcbVer, false);
                        Debug(DbgMsgType.Normal, " FrmV:", false); Debug(DbgMsgType.Outgoing, FirmVer, false);
                        Debug(DbgMsgType.Normal, " Testr:", false); Debug(DbgMsgType.Outgoing, Tester, false);
                        Debug(DbgMsgType.Normal, " PDate:", false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                        Debug(DbgMsgType.Normal, " TDate:", false); Debug(DbgMsgType.Outgoing, TestDate.ToShortDateString(), false);
                        Debug(DbgMsgType.Normal, " Serial:", false); Debug(DbgMsgType.Outgoing, Serial + "\r\n", false);
                    }
                    else
                    {
                        fsmInfo.SetFsmInfo(Manufacturer, Device, Applicatin, PcbVer, PrdDate, TestDate, FirmVer, Tester, Serial); //fsmInfo.SetFsmInfo(Device, Applicatin, PcbVer, PrdDate, FirmVer, Tester, Serial);
                        pgInfo.SelectedObjects = new object[] { fsmInfo };

                        Debug(DbgMsgType.Normal, "Name:", false); Debug(DbgMsgType.Outgoing, Device, false);
                        Debug(DbgMsgType.Normal, "  Type:", false); Debug(DbgMsgType.Outgoing, Applicatin, false);
                        Debug(DbgMsgType.Normal, "  Pcb Ver:", false); Debug(DbgMsgType.Outgoing, PcbVer.Substring(0, 2) + "." + PcbVer.Substring(2, 1), false);
                        Debug(DbgMsgType.Normal, "  Frm Ver:", false); Debug(DbgMsgType.Outgoing, FirmVer.Substring(0, 1) + "." + FirmVer.Substring(1, 2), false);
                        Debug(DbgMsgType.Normal, "  Tester:", false); Debug(DbgMsgType.Outgoing, Tester, false);
                        Debug(DbgMsgType.Normal, "  Date:", false); Debug(DbgMsgType.Outgoing, PrdDate.ToShortDateString(), false);
                        Debug(DbgMsgType.Normal, "  Serial:", false); Debug(DbgMsgType.Outgoing, Serial, false);
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

        private void lbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            textAddress.Text = lbDevices.SelectedItem.ToString();
            pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";
            GetDeviceInfo();

        }

        private void defaultToolStripButton_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                Result = fsm.ChangeWorkingMode(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), (ConnectionManager.WorkingModes)255, Convert.ToInt32(textTimeOut.Text), Cnv);
                //Result = fsm.ResetDevice(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
                if (Result == ConnectionManager.ReturnValues.Successful) { pBar.Value = 100; rValue.Text = Result.ToString(); }
                else { pBar.Value = 0; rValue.Text = Result.ToString(); }
            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            thread_stop = true;
            update_thread.Abort();
        }

        private void tsBtnBlink_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; this.Refresh();
            try
            {
                Result = fsm.Buzz(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), Convert.ToInt32(textTimeOut.Text), Cnv);
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

        private void tsMappingLoader_Click(object sender, EventArgs e)
        {
            rValue.Text = ""; pBar.Value = 0;
            try
            {
                OpenFileDialog binfile = new OpenFileDialog();
                binfile.Title = "Select LIC File";
                binfile.Filter = "License File (*.lic)|*.lic";
                binfile.CheckFileExists = false;
                binfile.ShowDialog();

                if (binfile.FileName != "")
                {
                    string filePath = binfile.FileName;
                    pBar.Value = 0;
                    Debug(DbgMsgType.Normal, filePath,true);

                    //BinaryReader ReadStream = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    StreamReader sr = new StreamReader(filePath, Encoding.ASCII);


                    string str = "";
                    for (int i = 0; i < 4; i++)
                    {
                        str = sr.ReadLine();
                        //Debug(DbgMsgType.Warning, str);
                    }
                    str = sr.ReadLine();
                    string[] checksum = str.Split(':');
                    str = sr.ReadLine();
                    string[] key = str.Split(':');
                    string[] keys = key[1].Split('-');
                    if (keys.Length != 5)
                    {
                        Debug(DbgMsgType.Error, "Lisans Dosyası hatalı...",true);
                    }
                    sr.Close();

                    checksum[1] = SMP.DecryptString(checksum[1]);
                    byte[] b = HexToByte(checksum[1]);

                    if (b.Length != 19)
                    {
                        Debug(DbgMsgType.Error, "Lisans Dosyası hatalı...",true);
                    }

                    Result = fsm.ChangeMfrKeysAndBlocks(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), b, Convert.ToInt32(textTimeOut.Text), Cnv);
                    if (Result == ConnectionManager.ReturnValues.Successful)
                    {
                        Debug(DbgMsgType.Normal, "Başarıyla yüklendi...",true);
                        pBar.Value = 100;
                    }
                    else
                        Debug(DbgMsgType.Normal, "Cihaza Yüklenemedi...",true);

                }
            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                Debug(DbgMsgType.Error, "Bilinmeyen Hata...",true);
            } 
        }

        private void nudSerial_ValueChanged(object sender, EventArgs e)
        {
            setting.LastSerial = (int)nudSerial.Value;
            setting.Save();
            fsmInfo.UpdateFsmInfo(fsmInfo.Device, fsmInfo.Tester, (int)nudSerial.Value);
            pgInfo.SelectedObjects = new object[] { fsmInfo };
        }

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            try
            {
                if (fsmInfo.PcbVer == "40R3" || fsmInfo.PcbVer == "40R2")
                    Result = fsm.ChangeDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), fsmInfo.TestDate, fsmInfo.Device, fsmInfo.Tester, fsmInfo.Serial, Convert.ToInt32(textTimeOut.Text), Cnv);
                else
                    Result = fsm.ChangeDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), fsmInfo.ProductDate, fsmInfo.Tester, fsmInfo.Serial, Convert.ToInt32(textTimeOut.Text), Cnv);
                
                if (Result == ConnectionManager.ReturnValues.Successful) { pBar.Value = 100; rValue.Text = Result.ToString(); }
                else { pBar.Value = 0; rValue.Text = Result.ToString(); }

            }
            catch (Exception ex)
            {
                pBar.Value = 0; rValue.Text = ex.ToString();
            }

            setting.Tester = fsmInfo.Tester;
            setting.Save();        
 


        }
        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            pBar.Value = 0; rValue.Text = ""; this.Refresh(); rtbDebug.Text = "";
            GetDeviceInfo();
        }


        rfMultiLibrary.PhysicalCommunication tactibbo = new rfMultiLibrary.PhysicalCommunication();
        rfMultiLibrary.PhysicalCommunication.ReturnValues rvalue;
        public string[] Macs, IPs, GWs, SMs, Ports, Bauds, Pars, Datas, Stops, Flows, Names, Protocols;

        private void btnList_Click(object sender, EventArgs e)
        {
            string[] sIP = new string[100]; nIP = new string[100, 2];
            cmbIPs.Items.Clear();
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

                if (tactibbo.GetLocalDevices(out Dvc) == rfMultiLibrary.PhysicalCommunication.ReturnValues.Succesfull)
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

                for (int i = 0; i < Macs.Length + +Dvc.Length; i++) //ip sıralaması
                {
                    cmbIPs.Items.Add(oIP[i]);
                }

                if (cmbIPs.Items.Count > 0)
                {
                    pBar.Value = 100;
                    cmbIPs.SelectedIndex = 0;
                    for (int i = 0; i < CnvIndex; i++)
                    {
                        if (cmbIPs.Text == nIP[i, 0])
                            if (nIP[i, 1] == "T") { cbCnvType.Text = "Tac"; Cnv = ConnectionManager.Converter.Tac; }
                            else { cbCnvType.Text = "NewConv"; Cnv = ConnectionManager.Converter.NewConv; }
                    }
                    rValue.Text = cmbIPs.Items.Count.ToString() + " Converter Bulundu";
                }
                else
                {
                    rValue.Text = "Converter Bulunamadı !";
                    return;
                }

                pBar.Value = 100;
                cmbIPs.SelectedIndex = 0;

            }
            catch
            { }
        }

        private void cmbIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < CnvIndex; i++)
            {
                if (cmbIPs.Text == nIP[i, 0])
                    if (nIP[i, 1] == "T") { cbCnvType.Text = "Tac"; Cnv = ConnectionManager.Converter.Tac; }
                    else { cbCnvType.Text = "NewConv"; Cnv = ConnectionManager.Converter.NewConv; }
            }
            setting.IP = cmbIPs.Text;
            setting.Save();
        }

        private void rtbDebug_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            rValue.Text = ""; pBar.Value = 0; string keys, value;
            try
            {
                if (InputBox("Keys Info", "Key:", out value) == DialogResult.OK)
                {
                    //Result = fsm.GetMfrKeysAndBlocks(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), value, out keys, Convert.ToInt32(textTimeOut.Text), Cnv);
                    //if (Result == ConnectionManager.ReturnValues.Successful)
                    //{
                    //    decimal blck = Convert.ToChar(keys.Substring(0, 1).ToString());
                    //    string Dflt = keys.Substring(1, 2);
                    //    string KeyA = keys.Substring(3, 2);
                    //    string KeyB = keys.Substring(5, 2);
                    //    Debug(DbgMsgType.Normal, "Block: " + blck + "  DfltKey: " + Dflt + "  KeyA: " + KeyA + "  KeyB: " + KeyB, true);
                    //    pBar.Value = 100;
                    //}
                    //else if(Result == ConnectionManager.ReturnValues.UndefinedError)
                    //    Debug(DbgMsgType.Normal, "Hatalı bir şifre girdiniz...", true);
                    //else
                    //    Debug(DbgMsgType.Normal, "Cihaz Cevap Vermiyor...", true);
                }
                else
                    MessageBox.Show("Lütfen doğru bir şifre giriniz.");

            }
            catch (Exception ex)
            {
                pBar.Value = 0;
                Debug(DbgMsgType.Error, "Bilinmeyen Hata...", true);
            } 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCnvType.SelectedItem.ToString() == "Tac") Cnv = ConnectionManager.Converter.Tac;
            else if (cbCnvType.SelectedItem.ToString() == "NewConv") Cnv = ConnectionManager.Converter.NewConv;
            else Cnv = ConnectionManager.Converter.Tibbo;
        }

        private void tsPing_Click(object sender, EventArgs e)
        {

        }

        private void cmbIPs_Click(object sender, EventArgs e)
        {

        }

        private void textTimeOut_Click(object sender, EventArgs e)
        {

        }

        
    }
}

