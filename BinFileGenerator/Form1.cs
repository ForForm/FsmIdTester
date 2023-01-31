using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BinFileGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   IMPORT TEXT
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        private void btnImpTxt_Click(object sender, EventArgs e)
        {
            FileStream Bfile = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.bin", FileMode.OpenOrCreate);
            FileStream Tfile = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v3.61_Mifare.txt", FileMode.OpenOrCreate);


            StreamWriter Sw = new StreamWriter(Tfile);
            BinaryReader RSr = new BinaryReader(Bfile);

            char[] cData = new char[100000];
            RSr.Read(cData, 0, 100000);
            Bfile.Close();
            RSr.Close();

            Sw.Write(cData, 0, cData.Length);
            Sw.Close();
            Tfile.Close();

        }
        #endregion

        private void btnExpBin_Click(object sender, EventArgs e)
        {
            char[] Data = new char[100000];
            char[] nData = new char[100000];
            int VetorStartAdd = 100000; int VetorEndAdd = 0; uint LineIndex = 0;


            OpenFileDialog txtfile = new OpenFileDialog();
            txtfile.Title = "Select TXT File";
            txtfile.Filter = "Binary File (*.txt)|*.txt";
            txtfile.CheckFileExists = false;
            if (txtfile.ShowDialog() != DialogResult.OK)
            {
                rValue.Text = "Cancelled"; rValue.ForeColor = Color.OrangeRed;
                return;
            }

            FileStream file = new FileStream(txtfile.FileName, FileMode.Open);
            //FileStream file = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.txt", FileMode.Open);

            StreamReader sr = new StreamReader(file);
            sr.Read(Data, 0, 100000);
            sr.Close();

            bool OnlySecond = true;
            for (int i = 0; i < 100000; i++)
            {
                //Gets Code End Character
                if (Data[i] == 'q')
                {
                    VetorEndAdd = i + 1;
                    nData[i - VetorStartAdd] = Data[i];
                    i = 1000001; continue;
                }

                //-----------------------------------------------------------------------------------------------------
                #region GetVectorStart
                //---------------------------------------------------------------------------------------------------------
                if (Data[i] == '@' && OnlySecond)
                {
                    string Vector = "";
                    Vector += Data[i + 1];
                    Vector += Data[i + 2];
                    Vector += Data[i + 3];
                    Vector += Data[i + 4];

                    LineIndex = Convert.ToUInt16(Vector, 16);
                    if (LineIndex == (uint)0x8000) continue;
                    VetorStartAdd = i;
                    OnlySecond = false;
                }
                #endregion
            }

            if (VetorStartAdd == 100000) rValue.Text = "File Error"; rValue.ForeColor = Color.Red;
            if (VetorEndAdd == 0) rValue.Text = "File Error"; rValue.ForeColor = Color.Red;

            SaveFileDialog binfile = new SaveFileDialog();
            //OpenFileDialog binfile = new OpenFileDialog();
            binfile.Title = "Select BIN File";
            binfile.Filter = "Binary File (*.bin)|*.bin";
            binfile.FileName = txtfile.SafeFileName.Substring(0,txtfile.SafeFileName.Length-4) + ".bin";
            //binfile.CheckFileExists = false;
            if (binfile.ShowDialog()!= DialogResult.OK)
            {
                rValue.Text = "Cancelled"; rValue.ForeColor = Color.OrangeRed;
                return;
            }

            //FileStream Bfile = new FileStream("E:\\Proje\\Firmware\\TT-FSM\\FsmRs485-ST - Yeni\\FsmRs485\\Debug\\FsmRs485_Yellow_v2.61_Mifare.bin", FileMode.OpenOrCreate);
            FileStream Bfile = new FileStream(binfile.FileName, FileMode.OpenOrCreate);

            BinaryWriter BSr = new BinaryWriter(Bfile);
            BSr.Write(Data, VetorStartAdd, (VetorEndAdd - VetorStartAdd));
            BSr.Close();

            rValue.Text = "Success"; rValue.ForeColor = Color.Green;


        }
    }
}
