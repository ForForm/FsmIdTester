using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace LicenceGenerator
{
    public class Blocks_Keys
    {


        private string _Mac;
        private string _MonRdKey;
        private string _MonWrKey;
        private string _UserRdKey;
        private string _UserWrKey;
        private string _AccessRdKey;
        private string _AccessWrKey;

        #region Cezeri Blocks
        [DescriptionAttribute("MoneyBlock"),
        CategoryAttribute("Blocks")]
        public int MoneyBlock { get; set; }

        [DescriptionAttribute("User Block"),
        CategoryAttribute("Blocks")]
        public int UserBlock { get; set; }

        [DescriptionAttribute("AccessBlock"),
        CategoryAttribute("Blocks")]
        public int AccessBlock { get; set; }
        #endregion

        #region Cezeri Keys
        [DescriptionAttribute("Money Read Key"),
        CategoryAttribute("Keys")]
        public string MonRdKey { get { return _MonRdKey; } set { _MonRdKey = SplitText(value, 2); } }

        [DescriptionAttribute("Money Write Key"),
        CategoryAttribute("Keys")]
        public string MonWrKey { get { return _MonWrKey; } set { _MonWrKey = SplitText(value, 2); } }

        [DescriptionAttribute("User Read Key"),
        CategoryAttribute("Keys")]
        public string UserRdKey { get { return _UserRdKey; } set { _UserRdKey = SplitText(value, 2); } }

        [DescriptionAttribute("User Write Key"),
        CategoryAttribute("Keys")]
        public string UserWrKey { get { return _UserWrKey; } set { _UserWrKey = SplitText(value, 2); } }

        [DescriptionAttribute("Access Read Key"),
        CategoryAttribute("Keys")]
        public string AccessRdKey { get { return _AccessRdKey; } set { _AccessRdKey = SplitText(value, 2); } }

        [DescriptionAttribute("Access Write Key"),
        CategoryAttribute("Keys")]
        public string AccessWrKey { get { return _AccessWrKey; } set { _AccessWrKey = SplitText(value, 2); } }

        #endregion

        #region User

        [DescriptionAttribute("User"),
        CategoryAttribute("User")]
        public string User { get; set; }

        [DescriptionAttribute("Date"),
        CategoryAttribute("User")]
        public string DateTime { get; set; }


        [DescriptionAttribute("Mac"),
        CategoryAttribute("User")]
        public string Mac { get { return _Mac; } set { _Mac = SplitText(value,2); } }

        #endregion

        string HexChars = "012346789ABCDEF";

        string SplitText(string txt, int splitCnt)
        {
            //string NewTxt = "";
            //if (txt.Length > 12) txt = txt.Substring(0, 12);

            //for (int i = 0; i < txt.Length; i++)
            //{
            //    NewTxt += txt.Substring(i, 1);
            //    if (i % splitCnt == 0) NewTxt += "-";

            //}
            //return NewTxt;
            return txt;
        }

        public enum BlockNumbers  
        {
            Block_1 = 1,
            Block_2 = 2,
            Block_3 = 3,
            Block_4,
            Block_5,
            Block_6,
            Block_7,
            Block_8,
            Block_9,
            Block_10,
            Block_11,
            Block_12,
            Block_13,
            Block_14,
            Block_15,
            Block_16

        };
    }


}
