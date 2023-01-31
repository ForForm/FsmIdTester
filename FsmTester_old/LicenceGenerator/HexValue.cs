using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LicenceGenerator
{
    public partial class HexValue : UserControl
    {
        public HexValue()
        {
            InitializeComponent();
        }

        public enum ReturnValues
        {
            Success,
            WrongHexChar,
            TwoCharsNeeded,
            NoEntry
        }

        #region Properties

        public string Caption
        {
            get { return lblName.Text; } 
            set { lblName.Text = value; }
        }

        public string Hex0
        {
            get 
            {
                if (CheckHexValue(txtHex0.Text) != ReturnValues.Success)    return "";
                else  return txtHex0.Text;
            }
            set{txtHex0.Text = value;}
        }

        public string Hex1
        {
            get
            {
                if (CheckHexValue(txtHex1.Text) != ReturnValues.Success) return "";
                else return txtHex1.Text;
            }
            set { txtHex1.Text = value; }
        }

        public string Hex2
        {
            get
            {
                if (CheckHexValue(txtHex2.Text) != ReturnValues.Success) return "";
                else return txtHex2.Text;
            }
            set { txtHex2.Text = value; }
        }

        public string Hex3
        {
            get
            {
                if (CheckHexValue(txtHex3.Text) != ReturnValues.Success) return "";
                else return txtHex3.Text;
            }
            set { txtHex3.Text = value; }
        }

        public string Hex4
        {
            get
            {
                if (CheckHexValue(txtHex4.Text) != ReturnValues.Success) return "";
                else return txtHex4.Text;
            }
            set { txtHex4.Text = value; }
        }

        public string Hex5
        {
            get
            {
                if (CheckHexValue(txtHex5.Text) != ReturnValues.Success) return "";
                else return txtHex5.Text;
            }
            set { txtHex5.Text = value; }
        }
        #endregion

        string HexChars = "0123456789ABCDEF";
        ReturnValues CheckHexValue(string HexVal)
        {
            if (HexVal.Length < 2 || HexVal.Length > 2) return ReturnValues.TwoCharsNeeded;

            char[] HexArray = HexVal.ToCharArray();
            if (HexChars.Contains(HexArray[0]) && HexChars.Contains(HexArray[1]))
                return ReturnValues.Success;
            else
                return ReturnValues.WrongHexChar;
        }

        private void txtHex0_TextChanged(object sender, EventArgs e)
        {
            string HexText = (sender as TextBox).Text;
            HexText = HexText.Trim();
            if (HexText.Length < 2)
            {
                return;
            }
            if (CheckHexValue(HexText) != ReturnValues.Success)
            {
                (sender as TextBox).BackColor = Color.Orange;
            }
            else
                (sender as TextBox).BackColor = Color.White;
        }

        private void txtHex0_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key0 = e.KeyData;
            Keys key1 = e.KeyCode;

            if (e.KeyData == Keys.Space ||  e.KeyData == Keys.Right)
            {
                this.SelectNextControl((TextBox)sender, true, true, true, true);

            }
        }

        private void txtHex0_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;
        }




    }
}
