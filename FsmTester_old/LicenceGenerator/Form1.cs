using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LicenceGenerator
{
    public partial class z : Form
    {
        public z()
        {
            InitializeComponent();
        }


        SimpleAES SMP;

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbAccessBlock.Text = "18";
        }

        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };
        public enum DataMode { Text, Hex }
        public enum DbgMsgType { Incoming, Outgoing, Normal, Warning, Error };

        private void Debug(DbgMsgType msgtype, string msg)
        {
            rtbDebug.Invoke(new EventHandler(delegate
            {
                rtbDebug.SelectedText = string.Empty;
                rtbDebug.SelectionFont = new Font(rtbDebug.SelectionFont, FontStyle.Regular);
                rtbDebug.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtbDebug.AppendText(msg + "\r\n");
                rtbDebug.ScrollToCaret();
                this.Refresh();
            }));
        }

        private void generatelicFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            this.Refresh();

            if (DialogResult.Yes != MessageBox.Show(this, txtFirm.Text + " Firması için " + cbFirm.Text + " Lisans Dosyası oluşturmak istiyor musunuz?", "Save License File", MessageBoxButtons.YesNo))
                return;
            SMP = new SimpleAES();

            #region Get Values
            if (hexMac.Hex0 == ""               || hexMac.Hex1 == ""            || hexMac.Hex2 == ""            || hexMac.Hex3 == ""            || hexMac.Hex4 == ""            || hexMac.Hex5 == ""           )
            { Debug(DbgMsgType.Error, "Mac Adresinde Hatalı Yada Eksik Giriş Yaptınız"); return; }
            if (hexDefaultKey.Hex0 == ""        || hexDefaultKey.Hex1 == ""     || hexDefaultKey.Hex2 == ""     || hexDefaultKey.Hex3 == ""     || hexDefaultKey.Hex4 == ""     || hexDefaultKey.Hex5 == ""    )
            { Debug(DbgMsgType.Error, "User Write Key'de Hatalı Yada Eksik Giriş Yaptınız"); return; }
            if (hexAccessWriteKey.Hex0 == ""    || hexAccessWriteKey.Hex1 == "" || hexAccessWriteKey.Hex2 == "" || hexAccessWriteKey.Hex3 == "" || hexAccessWriteKey.Hex4 == "" || hexAccessWriteKey.Hex5 == "")
            { Debug(DbgMsgType.Error, "Access Read Key'de Hatalı Yada Eksik Giriş Yaptınız"); return; }
            if (hexAccessReadKey.Hex0 == ""     || hexAccessReadKey.Hex1 == ""  || hexAccessReadKey.Hex2 == ""  || hexAccessReadKey.Hex3 == ""  || hexAccessReadKey.Hex4 == ""  || hexAccessReadKey.Hex5 == "" )
            { Debug(DbgMsgType.Error, "Access Write Key'de Hatalı Yada Eksik Giriş Yaptınız"); return; }
            #endregion

            #region Get Folder Path
            string path = System.IO.Directory.GetCurrentDirectory();
            string folderpath = path + "\\Licences";
            if (!System.IO.Directory.Exists(folderpath))
                System.IO.Directory.CreateDirectory(folderpath);


            SaveFileDialog LicFile = new SaveFileDialog();
            LicFile.Title = "Set Licence Name";
            LicFile.AddExtension = true;
            LicFile.DefaultExt = "lic";
            LicFile.Filter = "Text Document (*.lic)|*.lic";

            foreach (string Path in System.IO.Directory.GetFiles(folderpath))
            {
                File.SetAttributes(Path, FileAttributes.Normal);
            }
            if (LicFile.ShowDialog() != DialogResult.OK) return;

            string newpath = LicFile.FileName;

            foreach (string Path in System.IO.Directory.GetFiles(folderpath))
            {
                if(Path == newpath)
                File.Delete(newpath);
            }
            File.Delete(newpath);
            
            FileStream file = new FileStream(newpath, FileMode.Create);

            foreach (string Path in System.IO.Directory.GetFiles(folderpath))
            {
                File.SetAttributes(Path, FileAttributes.ReadOnly);
            }
            //File.SetAttributes(newpath, FileAttributes.ReadOnly);

            //System.Security.AccessControl.AccessControlActions sec = new System.Security.AccessControl.AccessControlActions();
            #endregion

            #region Licence Code
            string chars = "012346789";//ABCDEFGHJKLMNPQRTUVWXYZabcdefghjkmnpqrtuvwxyz";
            Random rnd = new Random();
            string LicCode = "";

            string MacString = "0";
            MacString += SMP.EncryptToString(hexMac.Hex0).Substring(0, 4);
            MacString += SMP.EncryptToString(hexMac.Hex1).Substring(0, 4);
            MacString += SMP.EncryptToString(hexMac.Hex2).Substring(0, 4);
            MacString += SMP.EncryptToString(hexMac.Hex3).Substring(0, 4);
            MacString += SMP.EncryptToString(hexMac.Hex4).Substring(0, 4);
            MacString += SMP.EncryptToString(hexMac.Hex5).Substring(0, 4);

            for (int i = 0; i < 5; i++)
            {
                LicCode += MacString.Substring(i * 5, 5);
                for (int z = 5; z < 12; z++) LicCode += chars.Substring(rnd.Next(chars.Length), 1);
                if (i != 4)
                    LicCode += "-";
            }
            #endregion

            #region GenerateLicString
            string FirmLic = "", FirmHexLic = ""; char stream =' ';
            if (cbFirm.Text == "Barkodes")
                FirmLic = "T";
            else
                FirmLic = "B";

            FirmLic += txtFirm.Text.Substring(0, 4);
            for (int i = 0; i < 5; i++)
                stream ^= (char)FirmLic[i];
            FirmLic += stream.ToString();

            foreach (char c in FirmLic)
            {
                int tmp = c;
                FirmHexLic += String.Format("{0:x2}", (uint)Convert.ToUInt32(tmp.ToString()));
            }
            #endregion

            #region GenerateKeyString

            string KeyString = Convert.ToInt32(cmbAccessBlock.Text).ToString("X2")
                               + hexDefaultKey.Hex0       + hexDefaultKey.Hex1        + hexDefaultKey.Hex2        + hexDefaultKey.Hex3     + hexDefaultKey.Hex4        + hexDefaultKey.Hex5                                                                                                                          
                               + hexAccessWriteKey.Hex0    + hexAccessWriteKey.Hex1     + hexAccessWriteKey.Hex2     + hexAccessWriteKey.Hex3  + hexAccessWriteKey.Hex4     + hexAccessWriteKey.Hex5
                               + hexAccessReadKey.Hex0 + hexAccessReadKey.Hex1 + hexAccessReadKey.Hex2 + hexAccessReadKey.Hex3 + hexAccessReadKey.Hex4 + hexAccessReadKey.Hex5 + FirmHexLic;

            KeyString = SMP.EncryptToString(KeyString);

            #endregion
            string bosluk = "                                                                                                                |";
            string lhex = (hexMac.Hex0 + "-" + hexMac.Hex1 + "-" + hexMac.Hex2 + "-" + hexMac.Hex3 + "-" + hexMac.Hex4 + "-" + hexMac.Hex5);                
            #region Write Text to File
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine("|############################################### LICENSE FILE ##################################################|");
            sw.WriteLine("|Manufacturer:" + txtUserName.Text + bosluk.Substring((14 + txtUserName.Text.Length), (bosluk.Length - (14 + txtUserName.Text.Length))));
            sw.WriteLine("|Web         :" + txtWeb.Text + bosluk.Substring((14 + txtWeb.Text.Length), (bosluk.Length - (14 + txtWeb.Text.Length))));
            sw.WriteLine("|E-Mail      :" + txtMail.Text + bosluk.Substring((14 + txtMail.Text.Length), (bosluk.Length - (14 + txtMail.Text.Length))));
            sw.WriteLine("|Telephone   :" + txtTel.Text + bosluk.Substring((14 + txtTel.Text.Length), (bosluk.Length - (14 + txtTel.Text.Length))));
            sw.WriteLine("|Fax         :" + txtFax.Text + bosluk.Substring((14 + txtFax.Text.Length), (bosluk.Length - (14 + txtFax.Text.Length))));
            sw.WriteLine("|###############################################################################################################|");
            sw.WriteLine("|License To  :" + txtFirm.Text + bosluk.Substring((14 + txtFirm.Text.Length), (bosluk.Length - (14 + txtFirm.Text.Length))));
            sw.WriteLine("|Issue Date  :" + DateTime.Now.ToString() + bosluk.Substring((14 + DateTime.Now.ToString().Length), (bosluk.Length - (14 + DateTime.Now.ToString().Length))));
            sw.WriteLine("|Issuer      :" + lhex + bosluk.Substring((14+lhex.Length), (bosluk.Length-(14+lhex.Length))));
            sw.WriteLine("|CheckSum    :" + KeyString.Substring(0,KeyString.Length/2) + bosluk.Substring((14+KeyString.Length/2), (bosluk.Length-(14+KeyString.Length/2))));
            sw.WriteLine("|             " + KeyString.Substring((KeyString.Length / 2), KeyString.Length / 2) + bosluk.Substring((14 + (KeyString.Length / 2)), (bosluk.Length - (14+(KeyString.Length/2)))));
            sw.WriteLine("|" + bosluk.Substring(1, bosluk.Length-1));
            sw.WriteLine("|License Code:" + LicCode + bosluk.Substring((14+LicCode.Length), (bosluk.Length-(14+LicCode.Length))));
            sw.WriteLine("|###############################################################################################################|");
            sw.Close();
            #endregion
            Debug(DbgMsgType.Normal, "Bilgileri Yazma Başarılı");
            pBar.Value = 100;
        }

        private void getlicFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LabelStatus.Text = ""; pBar.Value = 0; this.Refresh(); rtbDebug.Text = "";
            SMP = new SimpleAES();

            try
            {
                OpenFileDialog LicFile = new OpenFileDialog();
                LicFile.Title = "Select LIC File";
                LicFile.Filter = "License File (*.lic)|*.lic";
                LicFile.CheckFileExists = false;
                if (LicFile.ShowDialog() != DialogResult.OK) return;

                if (LicFile.FileName != "")
                {
                    string filePath = LicFile.FileName;
                    pBar.Value = 0;

                    //BinaryReader ReadStream = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    StreamReader sr = new StreamReader(filePath, Encoding.ASCII);

                    string str = "";
#region Line 1
                    //İlk satır önemli değil
                    str = sr.ReadLine();            
#endregion
#region Line 2
                    //Lisans Sahibi   *************************************************************************************************************************************
                    str = sr.ReadLine();
                    string[] strArray = str.Split(':');
                    txtUserName.Text = strArray[1];
#endregion
#region Line 3
                    //Tarih Saat      *************************************************************************************************************************************
                    str = sr.ReadLine();
                    txtWeb.Text = str;
#endregion
#region Line 4
                    //Web      *************************************************************************************************************************************
  str = sr.ReadLine();
  txtMail.Text = str;
 #endregion
#region Line 5
                    //mail      *************************************************************************************************************************************
 str = sr.ReadLine();
 txtTel.Text = str;
#endregion
#region Line 6
                    //telefon      *************************************************************************************************************************************
  str = sr.ReadLine();
  txtFax.Text = str;
#endregion
#region Line 7
                    //fax      *************************************************************************************************************************************
str = sr.ReadLine();
#endregion
#region Line 8
                    //önemli değil     *************************************************************************************************************************************
str = sr.ReadLine();
txtFirm.Text = str;
#endregion
#region Line 8
//önemli değil     *************************************************************************************************************************************
str = sr.ReadLine();
#endregion
#region Line 9
                    //Mac - Issuer    *************************************************************************************************************************************

                    str = sr.ReadLine();
                    string[] mac = str.Split(':');
                    string[] macs = mac[1].Split('-');
                    try
                    {
                    hexMac.Hex0 = macs[0]; hexMac.Hex1 = macs[1]; hexMac.Hex2 = macs[2]; hexMac.Hex3 = macs[3]; hexMac.Hex4 = macs[4]; hexMac.Hex5 = macs[5];
                    }
                    catch 
                    {
                        Debug(DbgMsgType.Error, "Mac Adresi Hatalı");
                    }
#endregion
#region Line 10
                    //Blocks & Keys   *************************************************************************************************************************************
                    try
                    {
                    string DecHex = "";
                    str = sr.ReadLine();
                    string[] key = str.Split(':');
                    string[] Key = key[1].Split(' ');
                    DecHex = Key[0];
                    str = sr.ReadLine();
                    DecHex += str.Substring(14, str.Length-(14+3));

                    string[] keys = new string[200];
                    key[1] = SMP.DecryptString(DecHex);
                    for (int i = 0; i < key[1].Length; i += 2)  keys[i / 2] = key[1].Substring(i, 2);

                    string  blck = Convert.ToUInt32(keys[0], 16).ToString(); if (cmbAccessBlock.Items.Contains(blck)) cmbAccessBlock.Text = blck;
                    
                    hexDefaultKey.Hex0      = keys[1];  hexDefaultKey.Hex1     = keys[2];    hexDefaultKey.Hex2     = keys[3];   hexDefaultKey.Hex3      = keys[4];  hexDefaultKey.Hex4      = keys[5]; hexDefaultKey.Hex5      = keys[6];
                    hexAccessWriteKey.Hex0  = keys[7];  hexAccessWriteKey.Hex1 = keys[8];    hexAccessWriteKey.Hex2 = keys[9];   hexAccessWriteKey.Hex3  = keys[10];  hexAccessWriteKey.Hex4 = keys[11]; hexAccessWriteKey.Hex5 = keys[12];
                    hexAccessReadKey.Hex0   = keys[13];  hexAccessReadKey.Hex1 = keys[14];    hexAccessReadKey.Hex2 = keys[15];   hexAccessReadKey.Hex3  = keys[16];  hexAccessReadKey.Hex4  = keys[17]; hexAccessReadKey.Hex5  = keys[18];
                    txtFirm.Text = keys[19] + keys[20] + keys[21] + keys[22] + keys[23] + keys[24];
                    }
                    catch
                    {
                        Debug(DbgMsgType.Error,  "Key Bilgileri Eksik Yada Bozulmuş");
                    }
#endregion
#region Licence Code

                    str = sr.ReadLine();
                    string[] lic = str.Split(':');
                    string licCode = "";

                    string MacString = "0";
                    MacString += SMP.EncryptToString(macs[0]).Substring(0, 4);
                    MacString += SMP.EncryptToString(macs[1]).Substring(0, 4);
                    MacString += SMP.EncryptToString(macs[2]).Substring(0, 4);
                    MacString += SMP.EncryptToString(macs[3]).Substring(0, 4);
                    MacString += SMP.EncryptToString(macs[4]).Substring(0, 4);
                    MacString += SMP.EncryptToString(macs[5]).Substring(0, 4);

                    licCode += lic[1].Substring(0, 5) + lic[1].Substring(13, 5) + lic[1].Substring(26, 5) + lic[1].Substring(39, 5) + lic[1].Substring(52, 5);

                    if (MacString != licCode) Debug(DbgMsgType.Error, "Mac Bilgileri Değiştirilmiş, yada eski lisans");

#endregion   
                    sr.Close();
                }
            }
            catch { }
            Debug(DbgMsgType.Normal, "Bilgileri Okuma Başarılı");
            pBar.Value = 100;
        }

        private void hexDefaultKey_Load(object sender, EventArgs e)
        {

        }
   }
}
