using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
//using Cryption;

namespace FSM_Authorization
{
    #region UpperLevelInterface

    public class FsmConfig
    {

        [DescriptionAttribute("Direction"),
        CategoryAttribute("Fsm Config")]
        public ConnectionManager.Direction Direction { get; set; }

        [DescriptionAttribute("WorkingMode"),
        CategoryAttribute("Fsm Config")]
        public ConnectionManager.WorkingModes WorkMode { get; set; }

        [DescriptionAttribute("RelayTime"),
        CategoryAttribute("Fsm HW Config")]
        public byte Relaytime { get; set; }

        [DescriptionAttribute("OnlineTimeout"),
        CategoryAttribute("Fsm Config")]
        public byte OnlineTmout { get; set; }

        [DescriptionAttribute("DeviceAddress"),
        CategoryAttribute("Fsm HW Config")]
        public byte DeviceAddress { get; set; }

        [DescriptionAttribute("OpticalInputState"),
        CategoryAttribute("Fsm Optical Inputs")]
        public ConnectionManager.OpticalInput OpticInp { get; set; }

        [DescriptionAttribute("OpticalInputState"),
        CategoryAttribute("Fsm Optical Inputs")]
        public ConnectionManager.OpticalInputs FirstOptic { get; set; }

        [DescriptionAttribute("OpticalInputState"),
        CategoryAttribute("Fsm Optical Inputs")]
        public ConnectionManager.OpticalInputs SecondOptic { get; set; }

        [DescriptionAttribute("Güvenlik anahtarı"),
        CategoryAttribute("Fsm Optical Inputs")]
        public bool SecuritySwitch { get; set; }

        [DescriptionAttribute("AuthorizationRule"),
        CategoryAttribute("Fsm Config")]
        public ConnectionManager.Authorization AuthRule { get; set; }

        [DescriptionAttribute("ContactState"),
        CategoryAttribute("Fsm HW Config")]
        public ConnectionManager.Contact ContactSt { get; set; }

        [DescriptionAttribute("PassBackState"),
        CategoryAttribute("Fsm Mifare Config")]
        public ConnectionManager.PassBackState PassBackSt { get; set; }

        [DescriptionAttribute("AsciiState"),
        CategoryAttribute("Fsm X Config")]
        public ConnectionManager.AsciiState AsciiSt { get; set; }

        [DescriptionAttribute("SmartRelay"),
        CategoryAttribute("SmartRly Cntrl")]
        public ConnectionManager.SmrtRelayState SRlyCtrl { get; set; }

        [DescriptionAttribute("LoggingState"),
        CategoryAttribute("Fsm X Config")]
        public ConnectionManager.Logging LoggingSt { get; set; }

        [DescriptionAttribute("KeyControl"),
        CategoryAttribute("Fsm Mifare Config")]
        public ConnectionManager.KeyCtrl KeyCtrl { get; set; }

        [DescriptionAttribute("SeqReadState"),
        CategoryAttribute("Fsm Seq. Read")]
        public ConnectionManager.SequentialState SeqReadSt { get; set; }

        [DescriptionAttribute("SeqTimeOut"),
        CategoryAttribute("Fsm Seq. Read")]
        public int SeqTimeout { get; set; }

        [DescriptionAttribute("Reserved 1"),
        CategoryAttribute("Reserved")]
        public byte Reserved1 { get; set; }

        [DescriptionAttribute("Reserved 2"),
        CategoryAttribute("Reserved")]
        public byte Reserved2 { get; set; }

        [DescriptionAttribute("Reserved 3"),
        CategoryAttribute("Reserved")]
        public byte Reserved3 { get; set; }

        [DescriptionAttribute("Reserved 4"),
        CategoryAttribute("Reserved")]
        public byte Reserved4 { get; set; }

        public FsmConfig()
        {

        }

        public void SetFsmConfig(byte[] CfgParams)
        {
            this.Direction = (ConnectionManager.Direction)CfgParams[0];
            this.WorkMode = (ConnectionManager.WorkingModes)CfgParams[1];
            this.Relaytime = (byte)CfgParams[2];
            this.OnlineTmout = (byte)CfgParams[3];
            this.DeviceAddress = (byte)CfgParams[4];
            this.OpticInp = (ConnectionManager.OpticalInput)CfgParams[5];
            this.FirstOptic = ConnectionManager.OpticalInputs.Passive;
            this.SecondOptic = ConnectionManager.OpticalInputs.Passive;
            this.SecuritySwitch = false;
            this.AuthRule = (ConnectionManager.Authorization)CfgParams[6];
            this.ContactSt = (ConnectionManager.Contact)CfgParams[7];
            this.PassBackSt = (ConnectionManager.PassBackState)CfgParams[9];
            this.SRlyCtrl = (ConnectionManager.SmrtRelayState)CfgParams[20];
            this.AsciiSt = (ConnectionManager.AsciiState)CfgParams[10];
            this.LoggingSt = (ConnectionManager.Logging)CfgParams[11];
            this.KeyCtrl = (ConnectionManager.KeyCtrl)CfgParams[12];
            this.SeqReadSt = (ConnectionManager.SequentialState)CfgParams[13];
            this.SeqTimeout = (int)(CfgParams[15] * 256 + CfgParams[14]);

            this.Reserved1 = CfgParams[16];
            this.Reserved2 = CfgParams[17];
            this.Reserved3 = CfgParams[18];
            this.Reserved4 = CfgParams[19];
        }

        public void SetFsmConfig_v5(byte[] CfgParams)
        {
            this.Direction = (ConnectionManager.Direction)CfgParams[0];
            this.WorkMode = (ConnectionManager.WorkingModes)CfgParams[1];
            this.Relaytime = (byte)CfgParams[2];
            this.OnlineTmout = (byte)CfgParams[3];
            this.DeviceAddress = (byte)CfgParams[4];
            this.OpticInp = ConnectionManager.OpticalInput.Passive;
            this.FirstOptic = (ConnectionManager.OpticalInputs)(CfgParams[5] & 7);
            this.SecondOptic = (ConnectionManager.OpticalInputs)((CfgParams[5] >> 3) & 7);
            this.SecuritySwitch = Convert.ToBoolean((CfgParams[5] >> 6) & 1);
            this.AuthRule = (ConnectionManager.Authorization)CfgParams[6];
            this.ContactSt = (ConnectionManager.Contact)CfgParams[7];
            this.PassBackSt = (ConnectionManager.PassBackState)CfgParams[9];
            this.SRlyCtrl = (ConnectionManager.SmrtRelayState)CfgParams[20];
            this.AsciiSt = (ConnectionManager.AsciiState)CfgParams[10];
            this.LoggingSt = (ConnectionManager.Logging)CfgParams[11];
            this.KeyCtrl = (ConnectionManager.KeyCtrl)CfgParams[12];
            this.SeqReadSt = (ConnectionManager.SequentialState)CfgParams[13];
            this.SeqTimeout = (int)(CfgParams[15] * 256 + CfgParams[14]);

            this.Reserved1 = CfgParams[16];
            this.Reserved2 = CfgParams[17];
            this.Reserved3 = CfgParams[18];
            this.Reserved4 = CfgParams[19];
        }

        public byte[] GetFsmConfig_v5()
        {
            byte[] CfgParams = new byte[21];

            CfgParams[0] = (byte)Direction;
            CfgParams[1] = (byte)WorkMode;
            CfgParams[2] = Relaytime;
            CfgParams[3] = OnlineTmout;
            CfgParams[4] = DeviceAddress;
            byte optic = (byte)FirstOptic;
            optic |= (byte)((byte)SecondOptic << 3);
            optic |= (byte)(Convert.ToByte(SecuritySwitch) << 6);
            CfgParams[5] = optic;
            CfgParams[6] = (byte)AuthRule;
            CfgParams[7] = (byte)ContactSt;
            CfgParams[8] = (byte)0;                //BaudRate
            CfgParams[9] = (byte)PassBackSt;
            CfgParams[10] = (byte)AsciiSt;
            CfgParams[11] = (byte)LoggingSt;
            CfgParams[12] = (byte)KeyCtrl;
            CfgParams[13] = (byte)SeqReadSt;
            CfgParams[14] = (byte)(SeqTimeout % 256);
            CfgParams[15] = (byte)(SeqTimeout / 256);

            CfgParams[16] = (byte)0;                //Reserved
            CfgParams[17] = (byte)0;                //Reserved
            CfgParams[18] = (byte)0;                //Reserved
            CfgParams[19] = (byte)0;                //Reserved
            CfgParams[20] = (byte)SRlyCtrl;                //Reserved

            return CfgParams;
        }

        public byte[] GetFsmConfig()
        {
            byte[] CfgParams = new byte[21];

            CfgParams[0] = (byte)Direction;
            CfgParams[1] = (byte)WorkMode;
            CfgParams[2] = Relaytime;
            CfgParams[3] = OnlineTmout;
            CfgParams[4] = DeviceAddress;
            CfgParams[5] = (byte)OpticInp;
            CfgParams[6] = (byte)AuthRule;
            CfgParams[7] = (byte)ContactSt;
            CfgParams[8] = (byte)0;                //BaudRate
            CfgParams[9] = (byte)PassBackSt;
            CfgParams[10] = (byte)AsciiSt;
            CfgParams[11] = (byte)LoggingSt;
            CfgParams[12] = (byte)KeyCtrl;
            CfgParams[13] = (byte)SeqReadSt;
            CfgParams[14] = (byte)(SeqTimeout % 256);
            CfgParams[15] = (byte)(SeqTimeout / 256);

            CfgParams[16] = (byte)0;                //Reserved
            CfgParams[17] = (byte)0;                //Reserved
            CfgParams[18] = (byte)0;                //Reserved
            CfgParams[19] = (byte)0;                //Reserved
            CfgParams[20] = (byte)SRlyCtrl;                //Reserved

            return CfgParams;
        }
    }


    public class FsmInfo
    {
        [DescriptionAttribute("RelayTime"),
        CategoryAttribute("Relay Config")]
        public byte Relaytime { get; set; }

        [DescriptionAttribute("OnlineTimeout"),
        CategoryAttribute("Relay Config")]
        public byte OnlineTmout { get; set; }

        [DescriptionAttribute("DeviceAddress"),
        CategoryAttribute("Relay Config")]
        public byte DeviceAddress { get; set; }

        [DescriptionAttribute("OpticalInputState"),
        CategoryAttribute("Relay Config")]
        public ConnectionManager.OpticalInput OpticInp { get; set; }

        [DescriptionAttribute("ContactState"),
        CategoryAttribute("Relay Config")]
        public ConnectionManager.Contact ContactSt { get; set; }

        [ReadOnly(true)]
        [DescriptionAttribute("Manufacturer"),
        CategoryAttribute("FsmInfo")]
        public string Manufacturer { get; set; }

        [DescriptionAttribute("Device"),
        CategoryAttribute("FsmInfo")]
        public string Device { get; set; }

        [DescriptionAttribute("Application"),
        CategoryAttribute("FsmInfo")]
        public string Application { get; set; }

        [DescriptionAttribute("PcbVersion"),
        CategoryAttribute("FsmInfo")]
        public string PcbVer { get; set; }

        [DescriptionAttribute("ProductionDate"),
        CategoryAttribute("FsmInfo")]
        public DateTime ProductDate { get; set; }

        [ReadOnly(true)]
        [DescriptionAttribute("TestDate"),
        CategoryAttribute("FsmInfo")]
        public DateTime TestDate { get; set; }

        [DescriptionAttribute("FirmVerion"),
        CategoryAttribute("FsmInfo")]
        public string FirmVer { get; set; }

        [DescriptionAttribute("TesterName"),
        CategoryAttribute("FsmInfo")]
        public string Tester { get; set; }

        [DescriptionAttribute("Serial No"),
        CategoryAttribute("FsmInfo")]
        public string Serial { get; set; }



        public FsmInfo()
        {

        }

        public void SetFsmInfo(string manufacturer, string device, string application, string pcbVersion, DateTime productDate, DateTime testDate, string firmwareVersion, string tester, string serial)
        {
            this.Manufacturer = manufacturer;
            this.Device = device;
            this.Application = application;
            this.PcbVer = pcbVersion;
            this.FirmVer = firmwareVersion;
            this.Tester = tester;
            this.ProductDate = productDate;
            this.TestDate = testDate;
            this.Serial = serial;
        }

        public void SetFsmInfo(string device, string application, string pcbVersion, DateTime productDate, string firmwareVersion, string tester, string serial)
        {

            this.Device = device;
            this.Application = application;
            this.PcbVer = pcbVersion.Substring(0, 2) + "." + pcbVersion.Substring(2, 1);
            this.FirmVer = firmwareVersion.Substring(0, 1) + "." + firmwareVersion.Substring(1, 2);
            this.Tester = tester;
            this.ProductDate = productDate;
            this.Serial = serial;
        }

        public void SetFsmCfg(byte[] CfgParameter, string device, string application, string pcbVersion, DateTime productDate, string firmwareVersion, string tester, string serial)
        {
            FsmConfig fsm = new FsmConfig();

            this.Relaytime = (byte)CfgParameter[1];
            this.OnlineTmout = (byte)CfgParameter[2];
            this.DeviceAddress = (byte)CfgParameter[3];
            this.OpticInp = (ConnectionManager.OpticalInput)CfgParameter[5];
            this.ContactSt = (ConnectionManager.Contact)CfgParameter[6];

            this.Device = device;
            this.Application = application;
            if (pcbVersion.Substring(1, 1) != ".")
                this.PcbVer = pcbVersion.Substring(0, 2) + "." + pcbVersion.Substring(2, 1);
            else this.PcbVer = pcbVersion;
            if (firmwareVersion.Substring(1, 1) != ".")
                this.FirmVer = firmwareVersion.Substring(0, 1) + "." + firmwareVersion.Substring(1, 2);
            else this.FirmVer = firmwareVersion;
            this.Tester = tester;
            this.ProductDate = productDate;
            this.Serial = serial;
        }

        public byte[] GetSmartRelayCfg()
        {
            byte[] fsm = new byte[5];

            fsm[0] = this.Relaytime;
            fsm[1] = this.OnlineTmout;
            fsm[2] = this.DeviceAddress;
            fsm[3] = (byte)this.OpticInp;
            fsm[4] = (byte)this.ContactSt;
            
            return fsm;
        }

        public void UpdateFsmInfo(DateTime productDate, string tester, int serial)
        {
            this.Tester = tester;
            this.ProductDate = productDate.Date;
            this.Serial = serial.ToString();
        }

        //public void GetFsmInfo(out string device, out string application,out string pcbVersion,out DateTime productDate,out string firmwareVersion, out string tester, out string serial)
        //{
        //    device            =  this.Device;         
        //    application       =  this.Application;    
        //    pcbVersion        =  this.PcbVer;            
        //    firmwareVersion   =  this.FirmVer;       
        //    tester            =  this.Tester;
        //    productDate       =  this.ProductDate;   
        //    serial            =  this.Serial;         
        //}
    }

    public static class Conversion
    {
        #region IntToByte

        public static byte[] IntToByte(Int64 value, int size)
        {
            try
            {
                byte[] buffer = new byte[size];
                for (int i = 0; i < size; i++)
                    buffer[i] = (byte)(value >> (i * 8));

                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] IntToByteEndian(Int64 value, int size)
        {
            try
            {
                byte[] buffer = new byte[size];
                for (int i = 0; i < size; i++)
                    buffer[(size - 1) - i] = (byte)(value >> (i * 8));

                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string IntToBinnaryString(ulong value, int bitsize)
        {
            try
            {
                string bin = "";
                for (int i = bitsize; i > 0; i--)
                    bin += ((value >> i) & 1).ToString();

                return bin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region ByteToInt

        public static long ByteToInt(byte[] value, int begin, int size)
        {
            try
            {
                long buffer = 0;

                for (int i = 0; i < size; i++)
                    buffer = (buffer << 8) | value[(begin + (size - 1)) - i];

                return buffer;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region HexToByte

        public static byte[] HexToByte(string msg)
        {
            try
            {
                msg = msg.Replace(":", "");
                msg = msg.Replace(" ", "");
                msg = msg.Replace("-", "");
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

        #region StringToByte

        public static byte[] StringToByte(string msg)
        {
            try
            {
                byte[] rt = System.Text.Encoding.ASCII.GetBytes(msg);
                return rt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region ByteToHex

        public static string ByteToHex(byte[] comByte)
        {
            try
            {
                StringBuilder builder = new StringBuilder(comByte.Length * 3);
                foreach (byte data in comByte)
                    builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string Byte2Hex(byte[] comByte)
        {
            try
            {
                StringBuilder builder = new StringBuilder(comByte.Length * 2);
                foreach (byte data in comByte)
                    builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(2));
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string ByteToHexArray_1(byte[] comByte)
        {
            try
            {
                string hex = "";
                StringBuilder builder = new StringBuilder(comByte.Length * 2);
                for (int i = 0; i < comByte.Length; i++)//foreach (byte data in comByte)
                {
                    hex = "0x" + Convert.ToString(comByte[i], 16).PadLeft(2, '0') + ", ";
                    if ((i > 0) && ((i+1) % 16 == 0))
                        hex += "\n";
                    builder.Append(hex);
                }
                return builder.ToString().ToUpper().Replace("X", "x");
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string shortToHexArray(short[] comByte)
        {
            try
            {
                string hex = "";
                StringBuilder builder = new StringBuilder(comByte.Length * 4);
                for (int i = 0; i < comByte.Length; i++)//foreach (byte data in comByte)
                {
                    hex = "0x" + Convert.ToString(comByte[i], 16).PadLeft(4, '0') + ", ";
                    if ((i > 0) && ((i + 1) % 16 == 0))
                        hex += "\n";
                    builder.Append(hex);
                }
                return builder.ToString().ToUpper().Replace("X", "x");
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string ByteToHexArray_2(byte[] comByte)
        {
            try
            {
                string hex = "";
                StringBuilder builder = new StringBuilder(comByte.Length * 2);
                for (int i = 0; i < comByte.Length; i++)//foreach (byte data in comByte)
                {
                    hex = Convert.ToString(comByte[i], 16).PadLeft(2, '0') + " ";
                    builder.Append(hex);
                }
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string Byte2HexArray(byte[] comByte)
        {
            try
            {
                string hex = "";
                StringBuilder builder = new StringBuilder(comByte.Length * 2);
                for (int i = 0; i < comByte.Length; i += 2)//foreach (byte data in comByte)
                {
                    hex = "0x" + Convert.ToString(comByte[i], 16).PadLeft(2, '0') + Convert.ToString(comByte[i + 1], 16).PadLeft(2, '0').PadRight(3, ',');
                    builder.Append(hex);
                }
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string StringByte2Hex(byte[] comByte, int begin, int length, bool space)
        {
            try
            {
                int Length = length * 2;
                if (space) Length = length * 3;

                StringBuilder builder = new StringBuilder(Length);
                for (int i = begin; i < begin + length; i++)//foreach (byte data in comByte)
                {
                    if (space)
                        builder.Append(Convert.ToString(comByte[i], 16).PadLeft(2, '0').PadRight(3, ' '));
                    else
                        builder.Append(Convert.ToString(comByte[i], 16).PadLeft(2, '0').PadRight(2));
                }
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }


    public class ConnectionManager
    {
        string _DLLVersion = "fsm485_v1.0.1";
        public string DLLVersion { get { return _DLLVersion; } set { _DLLVersion = value; } }

        private byte[] _ManufacturerCode;

        public ConnectionManager()
        {

        }

        public ReturnValues getManufacturerCode(out byte[] code)
        {
            code = null;
            if (_ManufacturerCode != null)
            {
                code = new byte[8];
                Array.Copy(_ManufacturerCode, 0, code, 0, 8);
                return ReturnValues.Successful;
            }
            return ReturnValues.NoLicanceFile;
        }


        #region DataStructer
        /*
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<Data0....DataN>-<CRC.High>-<CRC.Low>
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<SubCommand>-<CRC.High>-<CRC.Low>
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<Value0...ValueN>-<CRC.High>-<CRC.Low>
        */
        public enum DataStruct
        {
            PCToReader = 153,
            ReaderTOPC = 154
        }

        #endregion

        #region Commands

        public enum Commands
        {
            RstDvc = 2,
            RstConverter = 4,
            ChgMode = 6,

            ChgBaudRate = 16,
            ChgDateTime = 18,
            ChgInterval = 20,

            SetRly = 22,
            ClrRly = 24,

            SetBeep = 26,
            ClrBeep = 28,

            RcdPerson = 30,
            ErsPerson = 32,
            GetLog = 34,
            ErsAllPerson = 36,
            ErsAllLog = 88,

            GetLstID = 46,
            GetLstKeys = 48,
            GetPerson = 38,
            GetBlackPerson = 40,
            ErsBlackPerson = 42,
            ErsBlackIndex = 44,

            GetDateTime = 50,

            AcsAccept = 56,
            AcsDeny = 58,
            AcsWait = 60,
            AcsLock = 62,

            ChgAcsRules = 82,
            GetAcsRules = 84,

            ChgConfig = 64,
            GetConfig = 66,

            ChgDirection = 68,
            ChgAuthorization = 70,
            ChgRelayContact = 54,
            ChgPassBack = 116,
            ChgisAscii = 118,
            ChgRelayDT = 122,
            ChgLoging = 124,

            RequestPending = 72,
            RequestID = 74,
            RequestKey = 76,
            EventInfoAccess = 78,

            GetIndexData = 80,

            Buzzing = 92,

            ChgLedStatus = 94,
            Ping = 96,

            AcsCmd = 98,

            GetDvcInfo = 100,

            ChgDvcAdr = 102,

            WriteBlock = 104,
            ReadBlock = 106,
            WriteSector = 108,
            ReadSector = 110,

            RcdPeople = 112,
            SetDvcInfo = 114,

            OpticState = 126,
            TestConn = 118,
            GetDevices = 128,
            BootRequest = 132,
            ChgMfrKeys = 130,
            FindPerIdx = 134,
            ChgLogo = 120,
            RefleshLcd = 122,
            ChgDvcName = 138,
            SetVersion = 140,
            GetVersion = 142,
            FindPerson = 144,
            Busy = 145,
            RequestDoor = 146,
            RequestEmergency = 148,
            BootRequestBck = 152,
            GetSpareLog = 154,
            EraseSpareLog = 156,
            GetEvent = 158,
            EraseEvent = 160,
            OfflineLog = 162,
            Cmd_AddStaffGroup = 164,
            Cmd_GetStaffGroup = 166,
            Cmd_EraseStaffGroup = 168,
            Cmd_EraseAllStaffGroup = 170,
            Cmd_AddTimeToStaff = 172,
            Cmd_GetTimeFromStaff = 174,
            Cmd_WinterTime = 176,
        }

        #endregion

        #region Events

        public enum Events
        {
            OpenDoor = 0,
            CloseDoor = 1,
            ErrRWBlack = 2,
            ErrRWPerson = 3,
            ErrRWSpare = 4,
            UpdateYearTime = 5,
            ErrRWIndexLog = 6,
            ErrRWIndexPerson = 7,
            ErrRWIndexBlack = 8,
            ErrRWIndexSpare = 9,
            ErrRWIndexErsPerson = 10,
            AccessWithButton = 11,
            ErrRWStaffGroup = 12,
            ChangedWinterTime = 13,
            ErasedAllUsers = 14,
            ErasedAllBlackList = 15,
            ChangedDeviceType = 16,
            StartedBootload = 17,
            DeviceRestart = 18,
            ChangedDeviceMode = 19,
            ChangedDeviceConfig = 20,
            ChangedDeviceAddress = 21,
            LoadedCardKeys = 22,
            ChangedDateTime = 23,
            ErasedAllLogs = 24,
            ErasedAllEventLogs = 25,
            LoadedDeviceInfo = 26,
            ErasedAllStaffGr = 27,
            ErasedAllSpareLog = 28,
            SecurityAlarmCase = 29,
            ErrReadTime = 30,
            ErrRWLog = 31,
            TemperatureHot = 32,
            TemperatureCold = 33,
            RunModeFromTemperature = 34,
            NoEvent = 255
        };

        #endregion

        #region Returns


        public enum PersonState
        {
            Erased = 85,
            Active = 255
        }

        public enum ReturnValues
        {
            UndefinedError = 0,
            StatusOK = 0,
            Success = 1,

            PersonIndexOvf = 2,
            PersonNotRegistered = 3,
            PersonAlreadyRegistered = 4,
            PersonTimeNotAvailable = 5,
            PersonTimeNotRegistered = 6,
            PersonNameTooLong = 7,
            PersonNotFound = 8,
            PersonEraseNotAvailable = 9,

            LogIndexOvf = 10,
            LogNotRegistered = 11,
            LogNotFound = 12,
            LogDateNotAvailable = 13,
            LogEraseNotAvailable = 14,

            PhysicalError = 15,
            CommunicationError = 16,
            MemoryNotFound = 17,
            MemoryError = 18,

            /* Mifare Errors */
            AuthenticationError = 20,
            WriteError = 21,
            ReadError = 22,
            FromatSuccess = 23,
            Fail = 24,
            CRC_Error = 25,
            ManufacturerError = 26,
            ParameterError = 27,
            //    ACCESSTIMEEARLY			= 25,
            //    ACCESSTIMELATE			= 26,
            //    NOSTAFFGROUP			= 27,
            //    NOTIMEGROUP				= 28,

            Pn5321SpiErr = 100,
            PnReadPacketErr = 101,
            ScanForCardErr = 102,

            Failed,
            Failed1,
            Failed2,
            Successful,// Succesfull,
            LineBusy,
            InvalidDevice,
            InvalidResponse,
            DeviceNotFound,
            TimeOut,
            NoAnswer,
            NoAnswer2,
            PingError,
            PortError,
            DateTimeError,
            QueryFound,
            QueryNotFound,
            NoAnswerFromCnv,
            TimeError,
            MessageLengthIsTooBig,
            NoAccessRecord,
            AccessNotOccured,
            AccessOccured,
            EraseOfRecordsError,
            PacketError,
            RepeatPack,
            NoLicanceFile,

            StringLengthIsLess,
            NoRequest,
            ErrorData,
            DeviceBusy = 145
        }

        #endregion

        #region ConfigEnums

        public enum ScreenModes
        {
            SCREEN_UPLOADING_LOG,
            SCREEN_UPLOADING_LIST,
            SCREEN_DOWNLOADING_LIST,
            SCREEN_IDLE,
            SCREEN_CARD_EVENT,
            SCREEN_HOME_REFRESH,
            REASON_WAIT_MODE,
            CARD_DEFINE_MODE
        }
        public enum KeyCtrl
        {
            NotUsed,
            OnlyKey,
            Format
        }

        public enum SequentialState
        {
            NotUsed,
            OneRead,
            IdBased
        }

        public enum PassBackState
        {
            PassBackOff,
            PassBackOn,
            PassBackPc
        }
        public enum AsciiState
        {
            AsciiOff,
            AsciiOn
        }

        public enum SmrtRelayState
        {
            sRelayOff,
            sRelayOn
        }

        public enum BuzzerState
        {
            BuzzerOff,
            BuzzerOn
        }
        public enum RelayState
        {
            RelayOff,
            RelayOn
        }
        public enum BaudRate
        {
            _115200,
            _57600,
            _38400,
            _19200,
            _9600
        }
        public enum WorkingModes
        {
            DefineMode,
            NormalMode,
            ServiceMode,
            OfflineMode,
            OnlineInfoMode,
            OnlineAuthorMode,
        }

        public enum TurnStileFeedBack
        {
            Passed,
            NotPassed
        }

        public enum Contact
        {
            NO = 0,
            NC = 1
        }

        public enum Language
        {
            Turkish = 0,
            English = 1,
            Arabic = 2
        }

        public enum Logging
        {
            NoLogs = 1,
            SaveLogs = 0
        }

        public enum Direction
        {
            Both = 13,
            Exit = 14,
            Entry = 15
        }

        public enum Authorization
        {
            Unlimited = 0,
            OnlyAuthorized = 1,
            FullAuthorized = 2
        }

        public enum OpticalInput
        {
            Passive = 0,
            Button = 1,
            FeedBack = 2,
            Door = 3
        }

        public enum OpticalInputs
        {
            Passive  = 0,
            Button   = 1,
            Gate     = 2,
            FeedBack = 4,
        }

        public enum AccessDirection
        {
            DenFdBackEntry = 85,
            DenFdBackExit = 84,
            AcceptedEntry = 65,
            AcceptedExit = 64,
            DeniedEntry = 75,
            DeniedExit = 74,
            DenDirEntry = 55,
            DenDirExit = 54,
        }

        public enum DenyWithReason
        {
            Accepted = 0x10,
            Denied = 0x20,
            DeniedDir = 0x30,
            AccessTimeEarly = 0x40,
            AccessTimeLate = 0x50,
            NoStaffGroup = 0x60,
            NoTimeGroup = 0x70,
            SeqReadOne = 0x80,
            SeqIdBased = 0x90,
            CardBlocked = 0xA0,
        }

        public enum FeedBackControl
        {
            True = 1,
            False = 255
        }

        public enum DeviceVersion
        {
            IdTimeAccess = 1,//Basic = 1,
            IdProAccess = 2,//Puls  = 2,
            IdSmartAccess = 3,//Premium = 3,
            Undefined
        }

        public enum AccessType
        {
            Deny,
            Accept,
            Wait,
            Lock
        }

        public enum ClockDir
        {
            Back = 0,
            Forth = 1
        }

        public enum DoorStatus
        {
            CloseDoor = 0,
            OpenDoor = 1,
            NotActive = 2
        }

        public enum LogsProcess
        {
            StopSendingLog = 0,
            StartSendingLog = 1,
            AckLog = 2,
            NoConnection = 3,
        }

        public enum EmergencySts
        {
            No = 0,
            Yes = 1,
            NotActive = 2
        }

        public enum OnlineRequests
        {
            ID,
            Key,
            Log
        }

        public enum Converter
        {
            NewConv = 2,
            Tibbo = 1,
            Tac = 0
        }

        public enum LiftKeys
        {
            AllClose = 0,
            AllOpen = 1
        }

        #endregion

        #region StringConversion

        public byte[] MakeStringCompatible(string strArray)
        {
            //char[] strArray = strMessage.ToCharArray();
            byte[] Buffer = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                #region Switch statement
                switch (strArray[i])
                {
                    case ' ': Buffer[i] = 32; break;
                    case '!': Buffer[i] = 33; break;
                    case '"': Buffer[i] = 34; break;
                    case '#': Buffer[i] = 35; break;
                    case '$': Buffer[i] = 36; break;
                    case '%': Buffer[i] = 37; break;
                    case '&': Buffer[i] = 38; break;
                    case '(': Buffer[i] = 40; break;
                    case ')': Buffer[i] = 41; break;
                    case '*': Buffer[i] = 42; break;
                    case '+': Buffer[i] = 43; break;
                    case ',': Buffer[i] = 44; break;
                    case '-': Buffer[i] = 45; break;
                    case '.': Buffer[i] = 46; break;
                    case '/': Buffer[i] = 47; break;
                    case '0': Buffer[i] = 48; break;
                    case '1': Buffer[i] = 49; break;
                    case '2': Buffer[i] = 50; break;
                    case '3': Buffer[i] = 51; break;
                    case '4': Buffer[i] = 52; break;
                    case '5': Buffer[i] = 53; break;
                    case '6': Buffer[i] = 54; break;
                    case '7': Buffer[i] = 55; break;
                    case '8': Buffer[i] = 56; break;
                    case '9': Buffer[i] = 57; break;
                    case ':': Buffer[i] = 58; break;
                    case ';': Buffer[i] = 59; break;
                    case '<': Buffer[i] = 60; break;
                    case '=': Buffer[i] = 61; break;
                    case '>': Buffer[i] = 62; break;
                    case '?': Buffer[i] = 63; break;
                    case '@': Buffer[i] = 64; break;
                    case 'A': Buffer[i] = 65; break;
                    case 'B': Buffer[i] = 66; break;
                    case 'C': Buffer[i] = 67; break;
                    case 'D': Buffer[i] = 68; break;
                    case 'E': Buffer[i] = 69; break;
                    case 'F': Buffer[i] = 70; break;
                    case 'G': Buffer[i] = 71; break;
                    case 'H': Buffer[i] = 72; break;
                    case 'I': Buffer[i] = 73; break;
                    case 'J': Buffer[i] = 74; break;
                    case 'K': Buffer[i] = 75; break;
                    case 'L': Buffer[i] = 76; break;
                    case 'M': Buffer[i] = 77; break;
                    case 'N': Buffer[i] = 78; break;
                    case 'O': Buffer[i] = 79; break;
                    case 'P': Buffer[i] = 80; break;
                    case 'Q': Buffer[i] = 81; break;
                    case 'R': Buffer[i] = 82; break;
                    case 'S': Buffer[i] = 83; break;
                    case 'T': Buffer[i] = 84; break;
                    case 'U': Buffer[i] = 85; break;
                    case 'V': Buffer[i] = 86; break;
                    case 'W': Buffer[i] = 87; break;
                    case 'X': Buffer[i] = 88; break;
                    case 'Y': Buffer[i] = 89; break;
                    case 'Z': Buffer[i] = 90; break;
                    case '[': Buffer[i] = 91; break;
                    case ']': Buffer[i] = 93; break;
                    case '^': Buffer[i] = 94; break;
                    case '_': Buffer[i] = 95; break;
                    case '`': Buffer[i] = 96; break;
                    case 'a': Buffer[i] = 97; break;
                    case 'b': Buffer[i] = 98; break;
                    case 'c': Buffer[i] = 99; break;
                    case 'd': Buffer[i] = 100; break;
                    case 'e': Buffer[i] = 101; break;
                    case 'f': Buffer[i] = 102; break;
                    case 'g': Buffer[i] = 103; break;
                    case 'h': Buffer[i] = 104; break;
                    case 'i': Buffer[i] = 105; break;
                    case 'j': Buffer[i] = 106; break;
                    case 'k': Buffer[i] = 107; break;
                    case 'l': Buffer[i] = 108; break;
                    case 'm': Buffer[i] = 109; break;
                    case 'n': Buffer[i] = 110; break;
                    case 'o': Buffer[i] = 111; break;
                    case 'p': Buffer[i] = 112; break;
                    case 'q': Buffer[i] = 113; break;
                    case 'r': Buffer[i] = 114; break;
                    case 's': Buffer[i] = 115; break;
                    case 't': Buffer[i] = 116; break;
                    case 'u': Buffer[i] = 117; break;
                    case 'v': Buffer[i] = 118; break;
                    case 'w': Buffer[i] = 119; break;
                    case 'x': Buffer[i] = 120; break;
                    case 'y': Buffer[i] = 121; break;
                    case 'z': Buffer[i] = 122; break;
                    case '{': Buffer[i] = 123; break;
                    case '|': Buffer[i] = 124; break;
                    case '}': Buffer[i] = 125; break;
                    case '~': Buffer[i] = 126; break;
                    case '': Buffer[i] = 127; break;
                    case '€': Buffer[i] = 128; break;
                    case '': Buffer[i] = 129; break;
                    case '‚': Buffer[i] = 130; break;
                    case 'ƒ': Buffer[i] = 131; break;
                    case '„': Buffer[i] = 132; break;
                    case '…': Buffer[i] = 133; break;
                    case '†': Buffer[i] = 134; break;
                    case '‡': Buffer[i] = 135; break;
                    case 'ˆ': Buffer[i] = 136; break;
                    case '‰': Buffer[i] = 137; break;
                    case 'Š': Buffer[i] = 138; break;
                    case '‹': Buffer[i] = 139; break;
                    case 'Œ': Buffer[i] = 140; break;
                    case '': Buffer[i] = 141; break;
                    case '': Buffer[i] = 142; break;
                    case '': Buffer[i] = 143; break;
                    case '': Buffer[i] = 144; break;
                    case '‘': Buffer[i] = 145; break;
                    case '’': Buffer[i] = 146; break;
                    case '“': Buffer[i] = 147; break;
                    case '”': Buffer[i] = 148; break;
                    case '•': Buffer[i] = 149; break;
                    case '–': Buffer[i] = 150; break;
                    case '—': Buffer[i] = 151; break;
                    case '˜': Buffer[i] = 152; break;
                    case '™': Buffer[i] = 153; break;
                    case 'š': Buffer[i] = 154; break;
                    case '›': Buffer[i] = 155; break;
                    case 'œ': Buffer[i] = 156; break;
                    case '': Buffer[i] = 157; break;
                    case '': Buffer[i] = 158; break;
                    case 'Ÿ': Buffer[i] = 159; break;
                    case ' ': Buffer[i] = 160; break;
                    case '¡': Buffer[i] = 161; break;
                    case '¢': Buffer[i] = 162; break;
                    case '£': Buffer[i] = 163; break;
                    case '¤': Buffer[i] = 164; break;
                    case '¥': Buffer[i] = 165; break;
                    case '¦': Buffer[i] = 166; break;
                    case '§': Buffer[i] = 167; break;
                    case '¨': Buffer[i] = 168; break;
                    case '©': Buffer[i] = 169; break;
                    case 'ª': Buffer[i] = 170; break;
                    case '«': Buffer[i] = 171; break;
                    case '¬': Buffer[i] = 172; break;
                    case '­': Buffer[i] = 173; break;
                    case '®': Buffer[i] = 174; break;
                    case '¯': Buffer[i] = 175; break;
                    case '°': Buffer[i] = 176; break;
                    case '±': Buffer[i] = 177; break;
                    case '²': Buffer[i] = 178; break;
                    case '³': Buffer[i] = 179; break;
                    case '´': Buffer[i] = 180; break;
                    case 'µ': Buffer[i] = 181; break;
                    case '¶': Buffer[i] = 182; break;
                    case '·': Buffer[i] = 183; break;
                    case '¸': Buffer[i] = 184; break;
                    case '¹': Buffer[i] = 185; break;
                    case 'º': Buffer[i] = 186; break;
                    case '»': Buffer[i] = 187; break;
                    case '¼': Buffer[i] = 188; break;
                    case '½': Buffer[i] = 189; break;
                    case '¾': Buffer[i] = 190; break;
                    case '¿': Buffer[i] = 191; break;
                    case 'À': Buffer[i] = 192; break;
                    case 'Á': Buffer[i] = 193; break;
                    case 'Â': Buffer[i] = 194; break;
                    case 'Ã': Buffer[i] = 195; break;
                    case 'Ä': Buffer[i] = 196; break;
                    case 'Å': Buffer[i] = 197; break;
                    case 'Æ': Buffer[i] = 198; break;
                    case 'Ç': Buffer[i] = 199; break;
                    case 'È': Buffer[i] = 200; break;
                    case 'É': Buffer[i] = 201; break;
                    case 'Ê': Buffer[i] = 202; break;
                    case 'Ë': Buffer[i] = 203; break;
                    case 'Ì': Buffer[i] = 204; break;
                    case 'Í': Buffer[i] = 205; break;
                    case 'Î': Buffer[i] = 206; break;
                    case 'Ï': Buffer[i] = 207; break;
                    case 'Ğ': Buffer[i] = 208; break;
                    case 'Ñ': Buffer[i] = 209; break;
                    case 'Ò': Buffer[i] = 210; break;
                    case 'Ó': Buffer[i] = 211; break;
                    case 'Ô': Buffer[i] = 212; break;
                    case 'Õ': Buffer[i] = 213; break;
                    case 'Ö': Buffer[i] = 214; break;
                    case '×': Buffer[i] = 215; break;
                    case 'Ø': Buffer[i] = 216; break;
                    case 'Ù': Buffer[i] = 217; break;
                    case 'Ú': Buffer[i] = 218; break;
                    case 'Û': Buffer[i] = 219; break;
                    case 'Ü': Buffer[i] = 220; break;
                    case 'İ': Buffer[i] = 221; break;
                    case 'Ş': Buffer[i] = 222; break;
                    case 'ß': Buffer[i] = 223; break;
                    case 'à': Buffer[i] = 224; break;
                    case 'á': Buffer[i] = 225; break;
                    case 'â': Buffer[i] = 226; break;
                    case 'ã': Buffer[i] = 227; break;
                    case 'ä': Buffer[i] = 228; break;
                    case 'å': Buffer[i] = 229; break;
                    case 'æ': Buffer[i] = 230; break;
                    case 'ç': Buffer[i] = 231; break;
                    case 'è': Buffer[i] = 232; break;
                    case 'é': Buffer[i] = 233; break;
                    case 'ê': Buffer[i] = 234; break;
                    case 'ë': Buffer[i] = 235; break;
                    case 'ì': Buffer[i] = 236; break;
                    case 'í': Buffer[i] = 237; break;
                    case 'î': Buffer[i] = 238; break;
                    case 'ï': Buffer[i] = 239; break;
                    case 'ğ': Buffer[i] = 240; break;
                    case 'ñ': Buffer[i] = 241; break;
                    case 'ò': Buffer[i] = 242; break;
                    case 'ó': Buffer[i] = 243; break;
                    case 'ô': Buffer[i] = 244; break;
                    case 'õ': Buffer[i] = 245; break;
                    case 'ö': Buffer[i] = 246; break;
                    case '÷': Buffer[i] = 247; break;
                    case 'ø': Buffer[i] = 248; break;
                    case 'ù': Buffer[i] = 249; break;
                    case 'ú': Buffer[i] = 250; break;
                    case 'û': Buffer[i] = 251; break;
                    case 'ü': Buffer[i] = 252; break;
                    case 'ı': Buffer[i] = 253; break;
                    case 'ş': Buffer[i] = 254; break;
                    case 'ÿ': Buffer[i] = 255; break;
                    default: Buffer[i] = 32; break;
                }
                #endregion
            }
            return Buffer;
        }

        #endregion

        //private void GetMacAddress_v2(int SelectedPort, out string[] Macs, out string[] IPs, out string[] GWs, out string[] SMs, out string[] Ports, out string[] BaudRates, out string[] Parities, out string[] Datasizes, out string[] Stopbits, out string[] Flowcontrol, out string[] Protocols, out string[] Names, out int Length)
        //{
        //    Macs = new string[100];
        //    IPs = new string[100];
        //    Names = new string[100];
        //    GWs = new string[100];
        //    SMs = new string[100];
        //    Ports = new string[100];
        //    BaudRates = new string[100];
        //    Parities = new string[100];
        //    Datasizes = new string[100];
        //    Stopbits = new string[100];
        //    Flowcontrol = new string[100];
        //    Protocols = new string[100];
        //    Length = 0;

        //    byte[] LocatorData = new byte[4];
        //    LocatorData[0] = 72;
        //    LocatorData[1] = (byte)LocatorData.Length;
        //    if (SelectedPort == 0) LocatorData[2] = 64;
        //    else LocatorData[2] = 66;
        //    LocatorData[3] = (byte)((0 - LocatorData[0] - LocatorData[1] - LocatorData[2]) & 0xff);

        //    IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, 1930);
        //    UdpClient ucl = new UdpClient(1930);

        //    ucl.Client.ReceiveTimeout = 1000;
        //    ucl.EnableBroadcast = true;

        //    ucl.Send(LocatorData, LocatorData.Length, ipend);
        //    int i = 0;
        //    try
        //    {
        //        while (true)
        //        {
        //            byte[] bfr = ucl.Receive(ref ipend);
        //            if ((byte)bfr[0] == 77)
        //            {
        //                byte[] mac = new byte[6];
        //                Array.Copy(bfr, 9, mac, 0, mac.Length);
        //                Macs[i] = mac[0].ToString("X2") + ":" + mac[1].ToString("X2") + ":" + mac[2].ToString("X2") + ":" + mac[3].ToString("X2") + ":" + mac[4].ToString("X2") + ":" + mac[5].ToString("X2");
        //                if (bfr.Length > 84)
        //                {
        //                    Array.Copy(bfr, 84, mac, 0, 4);
        //                    GWs[i] = mac[3].ToString() + "." + mac[2].ToString() + "." + mac[1].ToString() + "." + mac[0].ToString();

        //                    Array.Copy(bfr, 88, mac, 0, 4);
        //                    SMs[i] = mac[3].ToString() + "." + mac[2].ToString() + "." + mac[1].ToString() + "." + mac[0].ToString();

        //                    int tmp = bfr[93];
        //                    tmp = (tmp << 8) | bfr[92];
        //                    Ports[i] = tmp.ToString();

        //                    tmp = bfr[96];
        //                    tmp = (tmp << 8) | bfr[95];
        //                    tmp = (tmp << 8) | bfr[94];
        //                    BaudRates[i] = tmp.ToString();

        //                    Datasizes[i] = bfr[97].ToString();

        //                    Parities[i] = bfr[98].ToString();

        //                    Stopbits[i] = bfr[99].ToString();

        //                    Flowcontrol[i] = bfr[100].ToString();

        //                    Protocols[i] = (bfr[101] & 0x03).ToString();
        //                }
        //                string resp = ASCIIEncoding.ASCII.GetString(bfr);
        //                string[] strArr = resp.Split('\0');
        //                for (int tmp = 19; tmp < 83; tmp++)
        //                {
        //                    Names[i] += Convert.ToBoolean(bfr[tmp]) ? Convert.ToChar(bfr[tmp]) : char.Parse(" "); ;
        //                }
        //                Names[i] = Names[i].Trim();
        //                IPs[i] = ipend.Address.ToString();
        //                i++;
        //            }
        //        }
        //        ucl.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Length = i;
        //        ucl.Close();
        //    }
        //}

        //public ReturnValues GetLocalDevices_v2()
        //{
        //    int SelectedPort = 1;
        //    int Length; string[] _Macs, _IPs, _GWs, _Names, _SMs, _Ports, _Bauds, _Datas, _Pars, _Stops, _Flows, _Protocols;
        //    GetMacAddress_v2(SelectedPort, out _Macs, out _IPs, out _GWs, out _SMs, out _Ports, out _Bauds, out _Pars, out _Datas, out _Stops, out _Flows, out _Protocols, out _Names, out Length);
        //    string[] Macs = new string[Length];
        //    string[] IPs = new string[Length];
        //    string[] GWs = new string[Length];
        //    string[] Names = new string[Length];
        //    string[] SMs = new string[Length];
        //    string[] Ports = new string[Length];
        //    string[] Bauds = new string[Length];
        //    string[] Pars = new string[Length];
        //    string[] Datas = new string[Length];
        //    string[] Stops = new string[Length];
        //    string[] Flows = new string[Length];
        //    string[] Protocols = new string[Length];

        //    Array.Copy(_Macs, 0, Macs, 0, Length);
        //    Array.Copy(_IPs, 0, IPs, 0, Length);
        //    Array.Copy(_GWs, 0, GWs, 0, Length);
        //    Array.Copy(_Names, 0, Names, 0, Length);
        //    Array.Copy(_SMs, 0, SMs, 0, Length);
        //    Array.Copy(_Ports, 0, Ports, 0, Length);
        //    Array.Copy(_Bauds, 0, Bauds, 0, Length);
        //    Array.Copy(_Pars, 0, Pars, 0, Length);
        //    Array.Copy(_Datas, 0, Datas, 0, Length);
        //    Array.Copy(_Stops, 0, Stops, 0, Length);
        //    Array.Copy(_Flows, 0, Flows, 0, Length);
        //    Array.Copy(_Protocols, 0, Protocols, 0, Length);


        //    return ReturnValues.Succesfull;
        //}

        //public ReturnValues Rst ()
        //{
        //    byte[] data = new byte[] { 72, 4, 54, 0 };
        //    // IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort);
        //    // UdpClient client = new UdpClient();
        //    // EndPoint ep = (EndPoint)ipend;
        //    UdpClient client = new UdpClient(1930);
        //    IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, Convert.ToInt32(1930));
        //    for (int k = 0; k < data.Length - 1; k++)
        //    {
        //        data[data.Length - 1] -= data[k];
        //    }
        //    client.Client.ReceiveTimeout = 1000;
        //    client.EnableBroadcast = true;
        //    client.Send(data, data.Length, ipend);
        //    client.Close();
        //    return ReturnValues.Succesfull;
        //    /*                byte[] data = new byte[3] {72,72,52};
        //            UdpClient ucl = new UdpClient(1930);
        //            IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, Convert.ToInt32(1930));
        //            ucl.Client.ReceiveTimeout = 1000;
        //            ucl.EnableBroadcast = true;
        //            ucl.Send(data, data.Length, ipend);
        //            ucl.Close();
        //            return ReturnValues.Succesfull;*/
        //}


        #region ExistingTcpClients
        private static readonly Dictionary<string, TcpClient> ExistingTcpClients = new Dictionary<string, TcpClient>();
        public static void SetExistingTcpClient(TcpClient tcpClient)
        {
            if (tcpClient != null && tcpClient.Client != null)
            {
                lock (ExistingTcpClients)
                {
                    var ipAddress = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
                    if (ExistingTcpClients.ContainsKey(ipAddress)) ExistingTcpClients[ipAddress] = tcpClient; else ExistingTcpClients.Add(ipAddress, tcpClient);
                }
            }
        }
        public static TcpClient GetExistingTcpClient(string ipAddress)
        {
            TcpClient tcpClient = null;
            lock (ExistingTcpClients)
            {
                if (ExistingTcpClients.ContainsKey(ipAddress)) tcpClient = ExistingTcpClients[ipAddress];
            }
            return tcpClient;
        }
        public static void DeleteExistingTcpClient(string ipAddress)
        {
            lock (ExistingTcpClients)
            {
                if (ExistingTcpClients.ContainsKey(ipAddress)) ExistingTcpClients.Remove(ipAddress);
            }
        }
        #endregion

        #region Private

        public ReturnValues PingAndPortTest(string TargetIP, int TargetPort, TcpClient client)
        {
            //Ping ping = new Ping();
            //PingReply pingresult = ping.Send(TargetIP);
            //try
            //{
            //    if (pingresult.Status != IPStatus.Success)
            //        return ReturnValues.DeviceNotFound;
            //}
            //catch (Exception ex)
            //{
            //    return ReturnValues.PortError;
            //}
            try
            {

                client.Connect(TargetIP, TargetPort);
                if (!client.Connected)
                    /* {
                         if (Rst() != ReturnValues.Succesfull)*/
                    return ReturnValues.DeviceNotFound;
                //}

            }
            catch (Exception ex)
            {
                //if (Rst() != ReturnValues.Succesfull)
                return ReturnValues.PortError;


            }
            return ReturnValues.Successful;
        }

        public ReturnValues SendDataStreamWithPostamble(byte[] Buffer, int Offset, int Length, TcpClient client, Converter cnv)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    byte[] sb = new byte[Buffer.Length + 2];
                    Array.Copy(Buffer, sb, Buffer.Length);
                    NetStream.Write(sb, Offset, sb.Length);
                    if (cnv == Converter.Tac)
                    {
                        int msTimeOut = 1000;
                        int count = 0;
                        do
                        {
                            if (count++ > msTimeOut) break;
                            if (NetStream.DataAvailable) break;
                            System.Threading.Thread.Sleep(1);
                        } while (true);
                        if (NetStream.DataAvailable)
                        {
                            byte[] ReturnFrame = new byte[255];
                            if (NetStream.CanRead)
                            {
                                int i = 0;
                                do
                                {
                                    if (NetStream.DataAvailable)
                                    {
                                        ReturnFrame[i] = (byte)NetStream.ReadByte();
                                        if (ReturnFrame[i] == (byte)DataStruct.PCToReader)
                                            break;
                                        else i++;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        count++;
                                    }
                                } while (count < msTimeOut);
                                if (count >= msTimeOut)
                                    return ReturnValues.NoAnswerFromCnv;
                                else
                                {
                                    byte[] tmp = new byte[ReturnFrame[i - 1]];
                                    tmp[0] = ReturnFrame[i - 1];
                                    tmp[1] = ReturnFrame[i];
                                    for (int k = 2; k < tmp.Length; )
                                    {
                                        if (NetStream.DataAvailable)
                                        {
                                            tmp[k] = (byte)NetStream.ReadByte();
                                            k++;
                                        }
                                        else
                                        {
                                            if (count++ < msTimeOut)
                                                System.Threading.Thread.Sleep(1);
                                            else return ReturnValues.NoAnswerFromCnv;
                                        }
                                    }
                                    return ReturnValues.Successful;
                                }
                            }
                            else
                            {
                                return ReturnValues.TimeOut;
                            }
                        }
                        else
                        {
                            return ReturnValues.NoAnswerFromCnv;
                        }
                    }
                    else return ReturnValues.Successful;
                }
                return ReturnValues.LineBusy;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues AllSendDataStream(byte[] Buffer, int Offset, int Length, out byte[] packetOut, out int lenOut, TcpClient client, Converter cnv)
        {
            packetOut = null; lenOut = 0;

            try
            {
                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    //NetStream.WriteByte(0x55); NetStream.WriteByte(0x65);
                    NetStream.Write(Buffer, Offset, Buffer.Length);
                    //NetStream.WriteByte(0x55); NetStream.WriteByte(0x55);
                    if (NetStream.DataAvailable)
                        NetStream.Read(Buffer, Offset, Buffer.Length);
                    //return ReturnValues.Succesfull;

                    if (cnv == Converter.Tac)
                    {
                        int msTimeOut = 500;
                        int count = 0;
                        do
                        {
                            if (count++ > msTimeOut) break;
                            if (NetStream.DataAvailable) break;
                            System.Threading.Thread.Sleep(1);
                        } while (true);
                        if (NetStream.DataAvailable)
                        {
                            byte[] ReturnFrame = new byte[255];
                            if (NetStream.CanRead)
                            {
                                int i = 0;
                                do
                                {
                                    if (NetStream.DataAvailable)
                                    {
                                        ReturnFrame[i] = (byte)NetStream.ReadByte();
                                        if (ReturnFrame[i] == (byte)DataStruct.PCToReader)
                                            break;
                                        else i++;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        count++;
                                    }
                                } while (count < msTimeOut);
                                if (count >= msTimeOut)
                                {
                                    NetStream.Close();
                                    return ReturnValues.NoAnswerFromCnv;
                                }
                                else
                                {
                                    byte[] tmp = new byte[ReturnFrame[i - 1]];
                                    tmp[0] = ReturnFrame[i - 1];
                                    tmp[1] = ReturnFrame[i];
                                    for (int k = 2; k < tmp.Length; )
                                    {
                                        if (NetStream.DataAvailable)
                                        {
                                            tmp[k] = (byte)NetStream.ReadByte();
                                            k++;
                                        }
                                        else
                                        {
                                            if (count++ < msTimeOut)
                                                System.Threading.Thread.Sleep(1);
                                            else
                                            {
                                                NetStream.Close();
                                                return ReturnValues.NoAnswerFromCnv;
                                            }
                                        }
                                    }
                                    if (Buffer[5] == 0xa0 && Buffer[6] == 0xd5)
                                        Thread.Sleep(100);
                                    if (GetDataStream(out packetOut, out lenOut, client, 15) == ReturnValues.Successful)//msTimeOut
                                        return ReturnValues.Successful;
                                    else
                                        return ReturnValues.NoAnswer2;
                                }
                            }
                            else
                            {
                                NetStream.Close();
                                return ReturnValues.TimeOut;
                            }
                        }
                        else
                        {
                            NetStream.Close();
                            return ReturnValues.NoAnswerFromCnv;
                        }
                    }
                    else { NetStream.Close(); return ReturnValues.Successful; }
                }
                else { NetStream.Close(); return ReturnValues.LineBusy; }
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed1;
            }
        }

        private ReturnValues VerifyReceiveData(byte cmd, byte[] sBuffer, out byte[] rBuffer, out int rlen)
        {
            byte crc = 0; rlen = 0; rBuffer = null;
            for (int i = 0; i < sBuffer.Length; i++)
            {
                if (sBuffer[i] == (byte)DataStruct.ReaderTOPC && (cmd + 1) == (sBuffer[i + 3]))
                {
                    int len = sBuffer[i - 1];
                    for (int x = i - 1; x < len - 1; x++)
                    {
                        crc ^= sBuffer[x];
                    }
                    crc = (byte)(255 - crc);
                    if (crc == sBuffer[((i - 1) + len) - 1])
                    {
                        rlen = len;
                        rBuffer = new byte[len];
                        Array.Copy(sBuffer, i - 1, rBuffer, 0, len);
                        return ReturnValues.Successful;
                    }
                }
            }
            return ReturnValues.Failed;
        }

        public ReturnValues SendReceiveDataStream(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut, Converter cnv)
        {
            RcvBuf = null; RcvLen = 0;
            try
            {
                int TimeO = msTimeOut;

                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);

                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;

                try
                {
                    client.Connect(ipend);

                    if (client.Connected)
                    {
                        NetworkStream ns = client.GetStream();
                        ns.Write(SndBuf, 0, SndBuf.Length);
                        try
                        {
                            byte[] buf = new byte[client.ReceiveBufferSize];

                            while (!ns.DataAvailable)
                            {
                                if (--TimeO == 0)
                                {
                                    client.Close();
                                    return ReturnValues.NoAnswer;
                                }
                                Thread.Sleep(1);
                            }
                            RcvLen = client.Client.ReceiveFrom(buf, ref ep);

                            //while (!ns.DataAvailable)
                            //{
                            //    if (--TimeO == 0)
                            //    {
                            //        client.Close();
                            //        return ReturnValues.NoAnswer;
                            //    }
                            //    Thread.Sleep(1);
                            //}
                            //RcvLen = client.Client.ReceiveFrom(buf, ref ep);

                            client.Close();
                            RcvBuf = new byte[RcvLen];
                            Array.Copy(buf, RcvBuf, RcvLen);
                            return ReturnValues.Successful;
                        }
                        catch (Exception ex)
                        {
                            client.Close();
                            return ReturnValues.NoAnswer;
                        }
                    }
                    else
                    {
                        client.Close();
                        return ReturnValues.PortError;
                    }
                }
                catch (Exception ex)
                {
                    client.Close();
                    return ReturnValues.Failed;
                }

            }
            catch (Exception ex)
            {
                return ReturnValues.Failed1;
            }
        }

        public ReturnValues SendDataStream(byte[] exBuffer, int Offset, int Length, TcpClient client, Converter cnv)
        {
            try
            {
                byte[] Buffer;

                if (CommVersion)
                {
                    byte[] manf;
                    Buffer = new byte[exBuffer.Length + 8];
                    ReturnValues result = getManufacturerCode(out manf);
                    if (result == ReturnValues.Successful)
                    {
                        Array.Copy(exBuffer, 0, Buffer, 0, 6);
                        Array.Copy(manf, 0, Buffer, 6, manf.Length);
                        Array.Copy(exBuffer, 6, Buffer, 14, (exBuffer.Length - 6));
                        Buffer[0] += 8;
                        byte crc = 0;
                        for (int i = 0; i < Buffer.Length - 1; i++)
                        {
                            crc ^= Buffer[i];
                        }
                        Buffer[Buffer.Length - 1] = (byte)(255 - crc);
                    }
                    else
                        return result;
                }
                else
                {
                    Buffer = new byte[exBuffer.Length];
                    Buffer = exBuffer;
                }

                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    if (cnv == Converter.Tac)
                    {
                        NetStream.Write(Buffer, Offset, Buffer.Length);
                        if (NetStream.DataAvailable)
                            NetStream.Read(Buffer, Offset, Buffer.Length);

                        int msTimeOut = 1500;
                        int count = 0;
                        do
                        {
                            if (count++ > msTimeOut) break;
                            if (NetStream.DataAvailable) break;
                            System.Threading.Thread.Sleep(1);
                        } while (true);
                        if (NetStream.DataAvailable)
                        {
                            byte[] ReturnFrame = new byte[255];
                            if (NetStream.CanRead)
                            {
                                int i = 0;
                                do
                                {
                                    if (NetStream.DataAvailable)
                                    {
                                        ReturnFrame[i] = (byte)NetStream.ReadByte();
                                        if (ReturnFrame[i] == (byte)DataStruct.PCToReader)
                                            break;
                                        else i++;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        count++;
                                    }
                                } while (count < msTimeOut);
                                if (count >= msTimeOut)
                                {
                                    NetStream.Close();
                                    return ReturnValues.NoAnswerFromCnv;
                                }
                                else
                                {
                                    byte[] tmp = new byte[ReturnFrame[i - 1]];
                                    tmp[0] = ReturnFrame[i - 1];
                                    tmp[1] = ReturnFrame[i];
                                    for (int k = 2; k < tmp.Length; )
                                    {
                                        if (NetStream.DataAvailable)
                                        {
                                            tmp[k] = (byte)NetStream.ReadByte();
                                            k++;
                                        }
                                        else
                                        {
                                            if (count++ < msTimeOut)
                                                System.Threading.Thread.Sleep(1);
                                            else
                                            {
                                                NetStream.Close();
                                                return ReturnValues.NoAnswerFromCnv;
                                            }
                                        }
                                    }
                                    return ReturnValues.Successful;
                                }
                            }
                            else
                            {
                                NetStream.Close();
                                return ReturnValues.TimeOut;
                            }
                        }
                        else
                        {
                            NetStream.Close();
                            return ReturnValues.NoAnswerFromCnv;
                        }
                    }
                    else if (cnv == Converter.NewConv)
                    {
                        byte[] rBuffer = new byte[Buffer.Length + 3];
                        rBuffer[0] = 0x55; rBuffer[1] = 0x55; rBuffer[2] = 0x55;
                        Array.Copy(Buffer, 0, rBuffer, 3, Buffer.Length);
                        //rBuffer[0] = 0x55; rBuffer[1] = 0x55;
                        //Array.Copy(Buffer, 0, rBuffer, 0, Buffer.Length);
                        NetStream.Write(rBuffer, Offset, rBuffer.Length);
                        if (NetStream.DataAvailable)
                            NetStream.Read(Buffer, Offset, Buffer.Length);
                        return ReturnValues.Successful;
                    }
                    else { NetStream.Close(); return ReturnValues.Successful; }
                }
                else { NetStream.Close(); return ReturnValues.LineBusy; }
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed1;
            }
        }

        public ReturnValues SendDataStreamFree(byte[] Buffer, int Offset, int Length, TcpClient client, Converter cnv)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    if (cnv == Converter.Tac)
                    {
                        NetStream.Write(Buffer, Offset, Buffer.Length);
                        if (NetStream.DataAvailable)
                            NetStream.Read(Buffer, Offset, Buffer.Length);

                        int msTimeOut = 1500;
                        int count = 0;
                        do
                        {
                            if (count++ > msTimeOut) break;
                            if (NetStream.DataAvailable) break;
                            System.Threading.Thread.Sleep(1);
                        } while (true);
                        if (NetStream.DataAvailable)
                        {
                            byte[] ReturnFrame = new byte[255];
                            if (NetStream.CanRead)
                            {
                                int i = 0;
                                do
                                {
                                    if (NetStream.DataAvailable)
                                    {
                                        ReturnFrame[i] = (byte)NetStream.ReadByte();
                                        if (ReturnFrame[i] == (byte)DataStruct.PCToReader)
                                            break;
                                        else i++;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        count++;
                                    }
                                } while (count < msTimeOut);
                                if (count >= msTimeOut)
                                {
                                    NetStream.Close();
                                    return ReturnValues.NoAnswerFromCnv;
                                }
                                else
                                {
                                    byte[] tmp = new byte[ReturnFrame[i - 1]];
                                    tmp[0] = ReturnFrame[i - 1];
                                    tmp[1] = ReturnFrame[i];
                                    for (int k = 2; k < tmp.Length; )
                                    {
                                        if (NetStream.DataAvailable)
                                        {
                                            tmp[k] = (byte)NetStream.ReadByte();
                                            k++;
                                        }
                                        else
                                        {
                                            if (count++ < msTimeOut)
                                                System.Threading.Thread.Sleep(1);
                                            else
                                            {
                                                NetStream.Close();
                                                return ReturnValues.NoAnswerFromCnv;
                                            }
                                        }
                                    }
                                    return ReturnValues.Successful;
                                }
                            }
                            else
                            {
                                NetStream.Close();
                                return ReturnValues.TimeOut;
                            }
                        }
                        else
                        {
                            NetStream.Close();
                            return ReturnValues.NoAnswerFromCnv;
                        }
                    }
                    else if (cnv == Converter.NewConv)
                    {
                        byte[] rBuffer = new byte[Buffer.Length + 3];
                        rBuffer[0] = 0x55; rBuffer[1] = 0x55; rBuffer[2] = 0x55;
                        Array.Copy(Buffer, 0, rBuffer, 3, Buffer.Length);
                        //rBuffer[0] = 0x55; rBuffer[1] = 0x55;
                        //Array.Copy(Buffer, 0, rBuffer, 0, Buffer.Length);
                        NetStream.Write(rBuffer, Offset, rBuffer.Length);
                        if (NetStream.DataAvailable)
                            NetStream.Read(Buffer, Offset, Buffer.Length);
                        return ReturnValues.Successful;
                    }
                    else { NetStream.Close(); return ReturnValues.Successful; }
                }
                else { NetStream.Close(); return ReturnValues.LineBusy; }
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed1;
            }
        }

        private ReturnValues GetOnlineDataStream(out byte[] Buf, out int Length, TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                byte[] ReceiveBuffer = new byte[client.ReceiveBufferSize];
                Buf = null;
                Length = 0;
                int count = 0;
                do
                {
                    if (count++ > msTimeOut) break;
                    if (NetStream.DataAvailable) break;
                    System.Threading.Thread.Sleep(1);
                } while (true);
                if (NetStream.DataAvailable)
                {
                    if (NetStream.CanRead)
                    {
                        int i = 0;
                        do
                        {
                            System.Threading.Thread.Sleep(1);
                            if (NetStream.DataAvailable)
                            {
                                ReceiveBuffer[i] = (byte)NetStream.ReadByte();
                                if (ReceiveBuffer[i] == (byte)DataStruct.ReaderTOPC)
                                    break;
                                else i++;
                            }
                            else count++;
                        } while (count < msTimeOut);
                        Length = ReceiveBuffer[i - 1];
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        Buf[1] = ReceiveBuffer[i];
                        for (i = 2; i < Length; )
                        {
                            if (NetStream.DataAvailable)
                            {
                                Buf[i] = (byte)NetStream.ReadByte();
                                i++;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(1);
                                if (count++ > (msTimeOut))
                                    return ReturnValues.TimeOut;
                            }
                        }
                        return ReturnValues.Successful;
                    }
                    else return ReturnValues.LineBusy;
                }
                else return ReturnValues.NoRequest;
            }
            catch (Exception ex)
            {
                Length = 0;
                Buf = null;
                return ReturnValues.Failed;
            }
        }

        private ReturnValues GetDataStream(out byte[] Buf, out int Length, TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                byte[] ReceiveBuffer = new byte[client.ReceiveBufferSize];
                Stopwatch sw = new Stopwatch();
                Buf = null;
                Length = 0;
                sw.Start();
                do
                {
                    if (NetStream.DataAvailable)
                        break;
                    else
                    {
                        if (sw.ElapsedMilliseconds > msTimeOut)
                        {
                            sw.Stop();
                            if (NetStream.DataAvailable)
                                break;
                            return ReturnValues.TimeOut;
                        }
                    }
                } while (true);

                sw.Reset();
                sw.Start();
                if (NetStream.DataAvailable)
                {
                    if (NetStream.CanRead)
                    {
                        int i = 0;
                        do
                        {
                            if (NetStream.DataAvailable)
                            {
                                ReceiveBuffer[i] = (byte)NetStream.ReadByte();
                                if (ReceiveBuffer[i] == (byte)DataStruct.ReaderTOPC)
                                    break;
                                else i++;
                            }
                            else
                            {
                                if (sw.ElapsedMilliseconds > msTimeOut)
                                {
                                    sw.Stop();
                                    return ReturnValues.TimeOut;
                                }
                            }
                        } while (true);
                        Length = ReceiveBuffer[i - 1];
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        Buf[1] = ReceiveBuffer[i];
                        for (i = 2; i < Length; )
                        {
                            if (NetStream.DataAvailable)
                            {
                                Buf[i] = (byte)NetStream.ReadByte();
                                i++;
                            }
                            else
                            {
                                if (sw.ElapsedMilliseconds > msTimeOut)
                                {
                                    sw.Stop();
                                    return ReturnValues.TimeOut;
                                }
                            }
                        }
                        sw.Stop();
                        return ResponseState(Buf[5]);//ReturnValues.Successful;
                    }
                    else
                    {
                        sw.Stop();
                        return ReturnValues.TimeOut;
                    }
                }
                else
                {
                    sw.Stop();
                    return ReturnValues.LineBusy;
                }
            }
            catch (Exception)
            {
                Length = 0;
                Buf = null;
                return ReturnValues.Failed2;
            }
        }

        private ReturnValues GetDataStreamFree(out byte[] Buf, out int Length, TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                byte[] ReceiveBuffer = new byte[client.ReceiveBufferSize];
                Stopwatch sw = new Stopwatch();
                Buf = null;
                Length = 0;
                sw.Start();
                do
                {
                    if (NetStream.DataAvailable)
                        break;
                    else
                    {
                        if (sw.ElapsedMilliseconds > msTimeOut)
                        {
                            sw.Stop();
                            if (NetStream.DataAvailable)
                                break;
                            return ReturnValues.TimeOut;
                        }
                    }
                } while (true);

                sw.Reset();
                sw.Start();
                if (NetStream.DataAvailable)
                {
                    if (NetStream.CanRead)
                    {
                        int i = 0;
                        do
                        {
                            if (NetStream.DataAvailable)
                            {
                                ReceiveBuffer[i] = (byte)NetStream.ReadByte();
                                if (ReceiveBuffer[i] == (byte)DataStruct.ReaderTOPC)
                                    break;
                                else i++;
                            }
                            else
                            {
                                if (sw.ElapsedMilliseconds > msTimeOut)
                                {
                                    sw.Stop();
                                    return ReturnValues.TimeOut;
                                }
                            }
                        } while (true);
                        Length = ReceiveBuffer[i - 1];
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        Buf[1] = ReceiveBuffer[i];
                        for (i = 2; i < Length; )
                        {
                            if (NetStream.DataAvailable)
                            {
                                Buf[i] = (byte)NetStream.ReadByte();
                                i++;
                            }
                            else
                            {
                                if (sw.ElapsedMilliseconds > msTimeOut)
                                {
                                    sw.Stop();
                                    return ReturnValues.TimeOut;
                                }
                            }
                        }
                        sw.Stop();
                        return ReturnValues.Successful;
                    }
                    else
                    {
                        sw.Stop();
                        return ReturnValues.TimeOut;
                    }
                }
                else
                {
                    sw.Stop();
                    return ReturnValues.LineBusy;
                }
            }
            catch (Exception)
            {
                Length = 0;
                Buf = null;
                return ReturnValues.Failed2;
            }
        }

        private ReturnValues GetDataStream_Test(out byte[] Buf, out int Length, TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                byte[] ReceiveBuffer = new byte[client.ReceiveBufferSize];
                Buf = null;
                Length = 0;
                int timeCount = 0;

                do
                {
                    if (timeCount++ >= msTimeOut)
                        return ReturnValues.TimeOut;

                    if (NetStream.DataAvailable)
                        break;
                    Thread.Sleep(1);

                } while (true);

                timeCount = 0;
                if (NetStream.DataAvailable)
                {
                    if (NetStream.CanRead)
                    {
                        int i = 0;
                        do
                        {
                            if (NetStream.DataAvailable)
                            {
                                ReceiveBuffer[i] = (byte)NetStream.ReadByte();
                                if (ReceiveBuffer[i] == (byte)DataStruct.ReaderTOPC)
                                    break;
                                else i++;
                            }

                            if (timeCount++ >= msTimeOut)
                                return ReturnValues.TimeOut;
                            Thread.Sleep(1);

                        } while (true);

                        timeCount = 0;
                        Length = ReceiveBuffer[i - 1];
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        Buf[1] = ReceiveBuffer[i];
                        for (i = 2; i < Length; )
                        {
                            //if (NetStream.DataAvailable)
                            //{
                            Buf[i] = (byte)NetStream.ReadByte();
                            i++;
                            //}

                            //if (timeCount++ >= msTimeOut)
                            //    return ReturnValues.TimeOut;
                        }



                        i = 0;
                        do
                        {
                            if (NetStream.DataAvailable)
                            {
                                ReceiveBuffer[i] = (byte)NetStream.ReadByte();
                                if (ReceiveBuffer[i] == (byte)DataStruct.ReaderTOPC)
                                    break;
                                else i++;
                            }

                            if (timeCount++ >= msTimeOut)
                                return ReturnValues.TimeOut;
                            Thread.Sleep(1);

                        } while (true);

                        timeCount = 0;
                        Length = ReceiveBuffer[i - 1];
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        Buf[1] = ReceiveBuffer[i];
                        for (i = 2; i < Length; )
                        {
                            //if (NetStream.DataAvailable)
                            //{
                            Buf[i] = (byte)NetStream.ReadByte();
                            i++;
                            //}

                            //if (timeCount++ >= msTimeOut)
                            //    return ReturnValues.TimeOut;
                        }

                        return ReturnValues.Successful;
                    }
                    else
                    {
                        return ReturnValues.TimeOut;
                    }
                }
                else
                {
                    return ReturnValues.LineBusy;
                }
            }
            catch (Exception)
            {
                Length = 0;
                Buf = null;
                return ReturnValues.Failed2;
            }
        }

        private ReturnValues GetTibboDataStream(out byte[] Buf, out int Length, TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                byte[] ReceiveBuffer = new byte[client.ReceiveBufferSize];
                Buf = null;
                Length = 0;
                int count = 0;
                do
                {
                    if (NetStream.DataAvailable)
                        break;
                    else
                    {
                        if (count++ > msTimeOut)
                            return ReturnValues.TimeOut;
                        Thread.Sleep(1);
                    }
                } while (true);
                if (NetStream.DataAvailable)
                {
                    if (NetStream.CanRead)
                    {
                        Length = NetStream.ReadByte();
                        Buf = new byte[Length];
                        Buf[0] = (byte)Length;
                        NetStream.Read(Buf, 1, Length - 1);
                        return ReturnValues.Successful;
                    }
                    else return ReturnValues.TimeOut;
                }
                else return ReturnValues.LineBusy;
            }
            catch (Exception)
            {
                Length = 0;
                Buf = null;
                return ReturnValues.Failed;
            }
        }

        private ReturnValues CommandStream(string command, TcpClient client)
        {
            string streamx = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 255, 2 }) + command + System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 13 });

            byte[] arr = System.Text.ASCIIEncoding.ASCII.GetBytes(streamx.ToCharArray());

            return (SendDataStream(arr, 0, arr.Length, client, Converter.Tibbo));
        }

        public ReturnValues TibboDataMode(TcpClient client, int msTimeOut, Converter cnv)
        {
            if (cnv == Converter.Tibbo)
            {
                byte[] ReceiveBuffer, SendBuffer; int length = 0; SendBuffer = new byte[1];
                if (CommandStream("L", client) == ReturnValues.Successful)
                    if (GetTibboDataStream(out ReceiveBuffer, out length, client, msTimeOut) == ReturnValues.Successful)
                    {
                        if (ReceiveBuffer[1] == System.Text.ASCIIEncoding.ASCII.GetBytes(new char[] { 'A' })[0])
                        {
                            CommandStream("O", client);
                            GetTibboDataStream(out ReceiveBuffer, out length, client, msTimeOut);
                            return ReturnValues.Successful;
                        }
                        return ReturnValues.InvalidResponse;
                    }
                return ReturnValues.NoAnswer;
            }
            else
                return ReturnValues.Successful;
        }

        public ReturnValues ListenSmarRelay(out int Address, out int Log, TcpClient client, int msTimeOut, Converter cnv)
        {
            Address = 0;
            byte[] temp = null;
            Log = 0;
            try
            {
                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;
                    rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                    if (rv == ReturnValues.Successful)
                    {
                        if (temp != null)
                        {
                            byte crc = 0;
                            for (int i = 0; i < temp[0] - 1; i++)
                            {
                                crc ^= temp[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == temp[temp[0] - 1])
                            {
                                if ((temp[1] == (byte)DataStruct.ReaderTOPC) & (temp[4] == ((byte)Commands.OpticState + 1)))
                                {
                                    Address = temp[2]; Log = temp[5];
                                    return ReturnValues.Successful;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return ReturnValues.PacketError;
                        }
                    }
                    else return rv;
                }
            }
            catch (Exception)
            {
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ListenOnlineRequest(TcpClient client, int Addrss, out UInt64 ID, out int Address, AccessType Accss, BuzzerState Buzzer, int RelayTime, out int OfflineLogCount, int msTimeOut, Converter cnv, out byte[] temp)
        {
            int b = 0;
            ID = 0;
            Address = 0;
            temp = null;
            OfflineLogCount = 0;
            ReturnValues Result;
            try
            {
                //Deneme için ekledim //clientt diede değişti
                // StartAgain:
                // TcpClient client = new TcpClient();
                // ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);

                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;

                    if (client.Connected == true)
                    {
                        rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                        if (rv == ReturnValues.Successful)
                        {
                            if (temp != null)
                            {
                                byte crc = 0;
                                for (int i = 0; i < temp[0] - 1; i++)
                                {
                                    crc ^= temp[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == temp[temp[0] - 1])
                                {
                                    byte length = temp[0];
                                    if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length > 15)
                                    {
                                        ID = temp[6];
                                        ID = (ID << 8) | (temp[7]);
                                        ID = (ID << 8) | (temp[8]);
                                        ID = (ID << 8) | (temp[9]);
                                        ID = (ID << 8) | (temp[10]);
                                        ID = (ID << 8) | (temp[11]);
                                        ID = (ID << 8) | (temp[12]);
                                        Address = temp[2];

                                        OfflineLogCount = temp[16];
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[15]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[14]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[13]);

                                        Result = Access(client, Address, Accss, RelayTime, Buzzer, msTimeOut, cnv);//Addrss
                                        if (Result == ConnectionManager.ReturnValues.Successful)
                                        {
                                            return ReturnValues.Successful;
                                        }
                                        else
                                            return Result;
                                        //return ReturnValues.Successful;
                                        //client.Close();
                                    }
                                    else if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length < 16)
                                    {
                                        ID = temp[6];
                                        ID = (ID << 8) | (temp[7]);
                                        ID = (ID << 8) | (temp[8]);
                                        ID = (ID << 8) | (temp[9]);
                                        // ID = (ID << 8) | (temp[10]);
                                        // ID = (ID << 8) | (temp[11]);
                                        // ID = (ID << 8) | (temp[12]);
                                        Address = temp[2];

                                        OfflineLogCount = temp[13];
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[12]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[11]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[10]);

                                        Result = Access(client, Addrss, Accss, RelayTime, Buzzer, msTimeOut, cnv);
                                        if (Result == ConnectionManager.ReturnValues.Successful)
                                        {
                                            return ReturnValues.Successful;
                                        }
                                        else
                                            return Result;
                                    }

                                    if ((temp[4] - 1 == (byte)Commands.AcsAccept) | (temp[4] - 1 == (byte)Commands.AcsCmd) | (temp[4] - 1 == (byte)Commands.AcsDeny))
                                    {
                                        crc = 0;
                                    }
                                }
                                else
                                {
                                    return ReturnValues.PacketError;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return rv;
                        }
                    }
                    else
                    {
                        client.Close();
                        // b++;
                        // if ((b < 10) && client.Connected != true) goto StartAgain;
                        return rv;
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ListenOnlineRequest(out UInt64 ID, out int Address, out int OfflineLogCount, TcpClient client, int msTimeOut, Converter cnv, out byte[] temp)
        {
            ID = 0;
            Address = 0;
            temp = null;
            OfflineLogCount = 0;
            try
            {
                //Deneme için ekledim //clientt diede değişti
                //TcpClient client = new TcpClient();
                /*ReturnValues Result = PingAndPortTest("192.168.1.175", 1001, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }*/
                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;
                    rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                    if (rv == ReturnValues.Successful)
                    {
                        if (temp != null)
                        {
                            byte crc = 0;
                            for (int i = 0; i < temp[0] - 1; i++)
                            {
                                crc ^= temp[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == temp[temp[0] - 1])
                            {
                                byte length = temp[0];
                                if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length > 15)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    ID = (ID << 8) | (temp[10]);
                                    ID = (ID << 8) | (temp[11]);
                                    ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[16];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[15]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[14]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[13]);

                                    return ReturnValues.Successful;
                                    //client.Close();
                                }
                                else if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length < 16)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    // ID = (ID << 8) | (temp[10]);
                                    // ID = (ID << 8) | (temp[11]);
                                    // ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[13];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[12]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[11]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[10]);

                                    return ReturnValues.Successful;
                                }

                                if ((temp[4] - 1 == (byte)Commands.AcsAccept) | (temp[4] - 1 == (byte)Commands.AcsCmd) | (temp[4] - 1 == (byte)Commands.AcsDeny))
                                {
                                    crc = 0;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return ReturnValues.PacketError;
                        }
                    }
                    else
                    {
                        return rv;
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ListenOnlineRequest(out UInt64 ID, out int Address, out int OfflineLogCount, out DoorStatus Door, TcpClient client, int msTimeOut, Converter cnv, out byte[] temp)
        {
            ID = 0;
            Address = 0;
            temp = null;
            OfflineLogCount = 0;
            Door = DoorStatus.CloseDoor;

            try
            {
                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;
                    rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                    if (rv == ReturnValues.Successful)
                    {
                        if (temp != null)
                        {
                            byte crc = 0;
                            for (int i = 0; i < temp[0] - 1; i++)
                            {
                                crc ^= temp[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == temp[temp[0] - 1])
                            {
                                byte length = temp[0];
                                if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length > 15)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    ID = (ID << 8) | (temp[10]);
                                    ID = (ID << 8) | (temp[11]);
                                    ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[16];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[15]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[14]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[13]);

                                    return ReturnValues.Successful;
                                    //client.Close();
                                }
                                else if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length < 16)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    // ID = (ID << 8) | (temp[10]);
                                    // ID = (ID << 8) | (temp[11]);
                                    // ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[13];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[12]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[11]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[10]);

                                    return ReturnValues.Successful;
                                }
                                else if (temp[0] == 8 && ((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestDoor)) && length > 7)
                                {
                                    ID = 0;
                                    OfflineLogCount = 0;
                                    Address = temp[2];
                                    Door = (DoorStatus)temp[6];

                                    return ReturnValues.Successful;
                                }

                                if ((temp[4] - 1 == (byte)Commands.AcsAccept) | (temp[4] - 1 == (byte)Commands.AcsCmd) | (temp[4] - 1 == (byte)Commands.AcsDeny))
                                {
                                    crc = 0;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return ReturnValues.PacketError;
                        }
                    }
                    else
                    {
                        return rv;
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ListenOnlineRequest(out UInt64 ID, out int Address, out int OfflineLogCount, out EmergencySts Emergency, out DoorStatus Door, TcpClient client, int msTimeOut, Converter cnv, out byte[] temp)
        {
            ID = 0;
            Address = 0;
            temp = null;
            OfflineLogCount = 0;
            Door = DoorStatus.CloseDoor;
            Emergency = EmergencySts.No;

            try
            {
                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;
                    rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                    if (rv == ReturnValues.Successful)
                    {
                        if (temp != null)
                        {
                            byte crc = 0;
                            for (int i = 0; i < temp[0] - 1; i++)
                            {
                                crc ^= temp[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == temp[temp[0] - 1])
                            {
                                byte length = temp[0];
                                if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length > 15)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    ID = (ID << 8) | (temp[10]);
                                    ID = (ID << 8) | (temp[11]);
                                    ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[16];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[15]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[14]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[13]);

                                    return ReturnValues.Successful;
                                    //client.Close();
                                }
                                else if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length < 16)
                                {
                                    ID = temp[6];
                                    ID = (ID << 8) | (temp[7]);
                                    ID = (ID << 8) | (temp[8]);
                                    ID = (ID << 8) | (temp[9]);
                                    // ID = (ID << 8) | (temp[10]);
                                    // ID = (ID << 8) | (temp[11]);
                                    // ID = (ID << 8) | (temp[12]);
                                    Address = temp[2];

                                    OfflineLogCount = temp[13];
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[12]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[11]);
                                    OfflineLogCount = (OfflineLogCount << 8) | (temp[10]);

                                    return ReturnValues.Successful;
                                }
                                else if (temp[0] == 8 && ((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestDoor)) && length > 7)
                                {
                                    ID = 0;
                                    OfflineLogCount = 0;
                                    Address = temp[2];
                                    Door = (DoorStatus)temp[6];
                                    Emergency = EmergencySts.NotActive;

                                    return ReturnValues.Successful;
                                }
                                else if (temp[0] == 8 && ((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestEmergency)) && length > 7)
                                {
                                    ID = 0;
                                    OfflineLogCount = 0;
                                    Address = temp[2];
                                    Door = DoorStatus.NotActive;
                                    Emergency = (EmergencySts)temp[6];

                                    return ReturnValues.Successful;
                                }

                                if ((temp[4] - 1 == (byte)Commands.AcsAccept) | (temp[4] - 1 == (byte)Commands.AcsCmd) | (temp[4] - 1 == (byte)Commands.AcsDeny))
                                {
                                    crc = 0;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return ReturnValues.PacketError;
                        }
                    }
                    else
                    {
                        return rv;
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        #endregion

        #region Public

        #region GetValue
        public static long GetValue(byte[] Data, int Begin)
        {
            long value;

            value = Data[Begin + 3];
            value = (value << 8 | Data[Begin + 2]);
            value = (value << 8 | Data[Begin + 1]);
            value = (value << 8 | Data[Begin + 0]);

            return value;
        }
        #endregion

        public ReturnValues GetFsmDevices(string TargetIP, int TargetPort, out int[] TargetAddress, out int[] TarAddress, int msTimeOut, Converter cnv)
        {
            TargetAddress = null; TarAddress = null;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	    /* Prefix  */
                    stream[2] = (byte)255;                              /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetDevices;                  /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStreamFree(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        //---------------------------------------------------
                        NetworkStream NetStream = client.GetStream();
                        byte[] Buff = new byte[5000];
                        int Length = 0;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        while (true)
                        {
                            if (sw.ElapsedMilliseconds >= 5500)
                                break;
                        }

                        sw.Stop();

                        int i = 0;
                        while (true)
                        {
                            if (NetStream.DataAvailable)
                            {
                                Buff[i] = (byte)NetStream.ReadByte();
                                i++;
                            }
                            else
                            {
                                Length = i;
                                break;
                            }
                        }
                        //---------------------------------------------------
                        int Inx = 0;
                        int[] TempAddresses = new int[100];
                        int[] TempAddres = new int[100];
                        for (i = 0; i < 100; i++) { TempAddresses[i] = 255; TempAddres[i] = 255; }
                        for (int z = 0; z < Length; z++)
                        {
                            byte crc = 0;

                            if (Buff[z] == (byte)DataStruct.ReaderTOPC && Buff[z - 1] == 5 && Buff[z + 2] == 82)
                            {
                                for (i = 0; i < 4; i++)
                                {
                                    crc ^= Buff[i + (z - 1)];
                                }
                                crc = (byte)(255 - crc);

                                if (crc == Buff[z + 3])
                                {
                                    if (Array.IndexOf(TempAddresses, Buff[z + 1]) < 0)  //Adres Yoksa
                                    {
                                        TempAddresses[Inx] = Buff[z + 1];
                                        TempAddres[Inx] = Buff[z + 2];
                                        Inx++;
                                    }
                                }
                            }
                            else if (Buff[z] == (byte)DataStruct.ReaderTOPC && Buff[z - 1] == 4 && Buff[z + 2] != 82)
                            {
                                for (i = 0; i < 3; i++)
                                {
                                    crc ^= Buff[i + (z - 1)];
                                }
                                crc = (byte)(255 - crc);

                                if (crc == Buff[z + 2])
                                {
                                    if (Array.IndexOf(TempAddresses, Buff[z + 1]) < 0)  //Adres Yoksa
                                    {
                                        TempAddresses[Inx] = Buff[z + 1];
                                        TempAddres[Inx] = 32;
                                        Inx++;
                                    }
                                }
                            }
                        }
                        if (Inx > 0)
                        {
                            TargetAddress = new int[Inx]; TarAddress = new int[Inx];
                            Array.Copy(TempAddresses, TargetAddress, Inx);
                            Array.Copy(TempAddres, TarAddress, Inx);
                            client.Close();
                            return ReturnValues.Successful;
                        }
                        client.Close();
                        return ReturnValues.DeviceNotFound;
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   SEND BOOT REQUEST
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        public ReturnValues SendBootRequest(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;            /* Packet Length */
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      /* Length  */
                    stream[4] = (byte)Commands.BootRequest;     /* Command */
                    stream[5] = 0;     				            /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);


                    byte[] packet; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < packet[0] - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[packet[0] - 1])
                            {
                                if ((packet[1] == (byte)154) & (packet[2] == (byte)TargetAddress))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    else
                    {
                        client.Close();
                        b++;
                        if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                        return ReturnValues.DeviceNotFound;
                    }
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        #endregion

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   SEND BOOT BYTES
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        public ReturnValues SendBootBytes(string TargetIP, int TargetPort, int TargetAddress, byte BootCommand, byte[] data, uint Address, byte DataLength, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8 + DataLength];
                    stream[0] = (byte)stream.Length;
                    stream[1] = (byte)DataStruct.PCToReader;
                    stream[2] = (byte)DataLength;
                    stream[3] = (byte)TargetAddress;
                    stream[4] = BootCommand;

                    stream[5] = (byte)(Address >> 0);
                    stream[6] = (byte)(Address >> 8);
                    // stream[7] = (byte)(Address >> 16);
                    //  stream[8] = (byte)(Address >> 24);

                    for (int i = 0; i < DataLength; i++)
                        stream[i + 7] = data[i];


                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);



                    byte[] packet; int length = 0;

                    //ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    //if (Result1 == ReturnValues.Successful)
                    //{
                    //    ReturnValues rv = GetDataStream(out packet, out length, client, 15);//msTimeOut
                    //    if (rv == ReturnValues.Successful)
                    //    {
                    ReturnValues Result1 = AllSendDataStream(stream, 0, stream.Length, out packet, out length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        byte crc = 0;
                        for (int i = 0; i < packet[0] - 1; i++)
                        {
                            crc ^= packet[i];
                        }
                        crc = (byte)(255 - crc);
                        if (crc == packet[packet[0] - 1])
                        {
                            if (packet[1] == (byte)154)
                            {
                                if (packet[2] == 'O')
                                {
                                    if (packet[3] == stream[5] && packet[4] == stream[6])
                                    { client.Close(); return ReturnValues.Successful; }
                                }
                                else if (packet[2] == 'E' && packet[3] == 'R')
                                { client.Close(); return ReturnValues.RepeatPack; }
                            }
                            else { client.Close(); return ReturnValues.InvalidResponse; }
                        }
                        else { client.Close(); return ReturnValues.PacketError; }
                        //}
                        //else
                        //{
                        //    client.Close();
                        //    b++;
                        //    if ((b < 10) && rv != ReturnValues.Successful)
                        //        goto StartAgain;
                        //    return rv;
                        //}//ReturnValues.NoAnswer; }
                    }
                    else
                    {
                        client.Close();
                        b++;
                        if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                        return ReturnValues.DeviceNotFound;
                    }
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        #endregion

        public ReturnValues SetSmartRelayRedLeds(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     	    /*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      					    /* Length  */
                    stream[4] = (byte)Commands.TestConn;         /* Command */
                    stream[5] = 0;     						    /* SubCommand */


                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    //SendDataStream(stream, 0, stream.Length, client, cnv);
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Successful)
                        {

                            client.Close();
                            return ReturnValues.Successful;
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues DeviceTestConnection(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[14];
                    stream[0] = (byte)stream.Length;     	    /*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      					    /* Length  */
                    stream[4] = (byte)Commands.Ping;         /* Command */
                    stream[5] = 0;     						    /* SubCommand */

                    /* stream[6] = (byte)DateTime.Now.Second;
                     stream[7] = (byte)DateTime.Now.Minute;
                     stream[8] = (byte)DateTime.Now.Hour;
                     stream[9] = (byte)DateTime.Now.DayOfWeek;
                     stream[10] = (byte)DateTime.Now.Day;
                     stream[11] = (byte)DateTime.Now.Month;
                     stream[12] = (byte)(DateTime.Now.Year % 2000);*/

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    //SendDataStream(stream, 0, stream.Length, client, cnv);
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ConverterReset(string TargetIP)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            try
            {
                byte[] data = new byte[] { 72, 4, 54, 0 };
                // IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort);
                UdpClient client = new UdpClient(1930);
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), Convert.ToInt32(1930));

                for (int k = 0; k < data.Length - 1; k++)
                {
                    data[data.Length - 1] -= data[k];
                }

                client.Client.ReceiveTimeout = 1000;
                client.EnableBroadcast = true;
                client.Send(data, data.Length, ipend);
                client.Close();

                return ReturnValues.Successful;

            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        //public ReturnValues EmergencyExit_UDP(string TargetIP)//, int TargetAddress, int msTimeOut, Converter cnv)
        //{
        //    try
        //    {
        //        byte[] data = new byte[] { 72, 5, 58, 2 };
        //        // IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort);
        //        UdpClient client = new UdpClient(1930);
        //        IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), Convert.ToInt32(1930));

        //        for (int k = 0; k < data.Length - 1; k++)
        //        {
        //            data[data.Length - 1] -= data[k];
        //        }

        //        client.Client.ReceiveTimeout = 1000;
        //        client.EnableBroadcast = true;
        //        client.Send(data, data.Length, ipend);
        //        client.Close();

        //        return ReturnValues.Successful;

        //    }
        //    catch (Exception ex)
        //    {
        //        return ReturnValues.Failed;
        //    }
        //}

        public ReturnValues ConverterReset(string TargetIP, int TargetPort)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] data = new byte[] { 72, 4, 54, 0 };

                Converter cnv = ConnectionManager.Converter.Tac;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, 1000, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[4];
                    stream[0] = data[0];     				/*Packet Length*/
                    stream[1] = data[1];      	/* Prefix  */
                    stream[2] = data[2];              /* Device Address  */
                    stream[3] = data[3];        						/* Length  */

                    SendDataStream(stream, 0, stream.Length, client, cnv);
                    client.Close();
                    return ReturnValues.Successful;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EmergencyExit_TCP(string TargetIP, int TargetPort)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] data = new byte[] { 4, 5, 56, 1 };

                Converter cnv = ConnectionManager.Converter.Tac;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, 1000, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[4];
                    stream[0] = data[0];     				/*Packet Length*/
                    stream[1] = data[1];      	/* Prefix  */
                    stream[2] = data[2];              /* Device Address  */
                    stream[3] = data[3];        						/* Length  */

                    SendDataStream(stream, 0, stream.Length, client, cnv);
                    client.Close();
                    return ReturnValues.Successful;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EmergencyExit_TCP(TcpClient client, string TargetIP, int TargetPort)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] data = new byte[] { 4, 5, 56, 1 };

                Converter cnv = ConnectionManager.Converter.Tac;

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, 1000, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[4];
                    stream[0] = data[0];     				/*Packet Length*/
                    stream[1] = data[1];      	/* Prefix  */
                    stream[2] = data[2];              /* Device Address  */
                    stream[3] = data[3];        						/* Length  */

                    SendDataStream(stream, 0, stream.Length, client, cnv);
                    return ReturnValues.Successful;
                }
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EmergencyExitClose_TCP(string TargetIP, int TargetPort)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] data = new byte[] { 4, 5, 58, 2 };

                Converter cnv = ConnectionManager.Converter.Tac;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, 1000, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[4];
                    stream[0] = data[0];     				/*Packet Length*/
                    stream[1] = data[1];      	/* Prefix  */
                    stream[2] = data[2];              /* Device Address  */
                    stream[3] = data[3];        						/* Length  */

                    SendDataStream(stream, 0, stream.Length, client, cnv);
                    client.Close();
                    return ReturnValues.Successful;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EmergencyExitClose_TCP(TcpClient client, string TargetIP, int TargetPort)//, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] data = new byte[] { 4, 5, 58, 2 };

                Converter cnv = ConnectionManager.Converter.Tac;

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, 1000, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[4];
                    stream[0] = data[0];     				/*Packet Length*/
                    stream[1] = data[1];      	/* Prefix  */
                    stream[2] = data[2];              /* Device Address  */
                    stream[3] = data[3];        						/* Length  */

                    SendDataStream(stream, 0, stream.Length, client, cnv);
                    return ReturnValues.Successful;
                }
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues Buzz(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     	/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      					/* Length  */
                    stream[4] = (byte)Commands.Buzzing;     /* Command */
                    stream[5] = 0;     						/* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < packet[0] - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[packet[0] - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ResetDevice(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.RstDvc;          /* Command */
                    stream[5] = 0;     			                /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        client.Close();
                        return ReturnValues.Successful;
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ResetTCPConverter(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.RstConverter;    /* Command */
                    stream[5] = 0;     			                /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues Buzzer(string TargetIP, int TargetPort, int TargetAddress, BuzzerState Buzz, int secBuzzTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     		/* Packet Length */
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    if (Buzz == BuzzerState.BuzzerOn)
                        stream[4] = (byte)Commands.SetBeep;     /* Command */
                    else
                        stream[4] = (byte)Commands.ClrBeep;     /* Command */
                    stream[5] = (byte)secBuzzTime;     			/* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues Relay(string TargetIP, int TargetPort, int TargetAddress, RelayState Relay, int secRelayTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    if (Relay == RelayState.RelayOn)
                        stream[4] = (byte)Commands.SetRly;     /* Command */
                    else
                        stream[4] = (byte)Commands.ClrRly;     /* Command */
                    stream[5] = (byte)secRelayTime;     							/* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Successful;
            }
        }

        public ReturnValues LiftAllOpen(string TargetIP, int TargetPort, int TargetAddress, LiftKeys Keys, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6];
                    stream[0] = (byte)stream.Length;     		/*  Packet Length    */
                    stream[1] = 0x0C;    /*  Prefix           */
                    stream[2] = (byte)Keys;            /*  Device Address   */
                    stream[3] = 0;      						/*  Length           */
                    stream[4] = 0;         /*  Command          */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    client.Close();
                    return ReturnValues.Successful;
                    //if (Result1 == ReturnValues.Successful)
                    //{
                    //    if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Successful)
                    //    {
                    //        byte crc = 0;
                    //        for (int i = 0; i < length - 1; i++)
                    //        {
                    //            crc ^= packet[i];
                    //        }
                    //        crc = (byte)(255 - crc);
                    //        if (crc == packet[length - 1])
                    //            if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                    //                if (packet[4] == (byte)(stream[4]) + 1)
                    //                {
                    //                    client.Close();
                    //                    return ReturnValues.Successful;
                    //                }
                    //                else { client.Close(); return ReturnValues.InvalidResponse; }
                    //            else { client.Close(); return ReturnValues.InvalidDevice; }
                    //        else { client.Close(); return ReturnValues.PacketError; }
                    //    }
                    //    else { client.Close(); return ReturnValues.NoAnswer; }
                    //}
                    //client.Close();
                    //b++;
                    //if ((b < 2) && Result1 != ReturnValues.Successful) goto StartAgain;
                    //return ReturnValues.DeviceNotFound;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeWorkingMode(string TargetIP, int TargetPort, int TargetAddress, WorkingModes Mode, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     		/*  Packet Length    */
                    stream[1] = (byte)DataStruct.PCToReader;    /*  Prefix           */
                    stream[2] = (byte)TargetAddress;            /*  Device Address   */
                    stream[3] = 4;      						/*  Length           */
                    stream[4] = (byte)Commands.ChgMode;         /*  Command          */
                    stream[5] = (byte)Mode;     				/*  SubCommand       */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeDeviceAddress(string TargetIP, int TargetPort, int TargetAddress, int Address, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgDvcAdr;         /* Command */
                    stream[5] = (byte)Address;     				/* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues GetAuthRulesPerHour(string TargetIP, int TargetPort, int TargetAddress, out byte[] AcsRules, int msTimeOut, Converter cnv)
        {
            AcsRules = null;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						        /* Length  */
                    stream[4] = (byte)Commands.GetAcsRules;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            try
                            {
                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            AcsRules = new byte[Response[3]];
                                            Array.Copy(Response, 6, AcsRules, 0, Response[3]);


                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            catch (Exception)
                            {
                                client.Close();
                                return ReturnValues.Failed;
                            }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues EraseAuthRulesPerHour(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    //Time = DateTime.Now;
                    stream = new byte[37];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(24);      						        /* Length  */
                    stream[4] = (byte)Commands.ChgAcsRules;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    try
                    {
                        for (int i = 0; i < 30; i++)
                            stream[6 + i] = 0;
                    }
                    catch (Exception Ex)
                    {
                        client.Close();
                        return ReturnValues.DateTimeError;
                    }

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeAuthRulesPerHour(string TargetIP, int TargetPort, int TargetAddress, byte[] AcsRules, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    //Time = DateTime.Now;
                    stream = new byte[37];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(24);      						        /* Length  */
                    stream[4] = (byte)Commands.ChgAcsRules;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    try
                    {
                        for (int i = 0; i < 30; i++)
                            stream[6 + i] = (byte)AcsRules[i];
                    }
                    catch (Exception Ex)
                    {
                        client.Close();
                        return ReturnValues.DateTimeError;
                    }

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeDateTime(string TargetIP, int TargetPort, int TargetAddress, DateTime Time, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                /*<Second>-<Minute>-<Hour>-<WeekDay>-<Day>-<Month>-<Year>*/

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    //Time = DateTime.Now;
                    stream = new byte[14];
                    stream[0] = (byte)stream.Length;     		/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ChgDateTime;     /* Command */
                    stream[5] = 0;     				            /* SubCommand */
                    try
                    {
                        stream[6] = (byte)Time.Second;
                        stream[7] = (byte)Time.Minute;
                        stream[8] = (byte)Time.Hour;
                        stream[9] = (byte)Time.DayOfWeek;
                        stream[10] = (byte)Time.Day;
                        stream[11] = (byte)Time.Month;
                        stream[12] = (byte)(Time.Year % 2000);
                    }
                    catch (Exception Ex)
                    {
                        client.Close();
                        return ReturnValues.DateTimeError;
                    }

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++) stream[stream.Length - 1] ^= stream[i];

                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeDateTime_Mini(string TargetIP, int TargetPort, int TargetAddress, DateTime Time, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                /*<Second>-<Minute>-<Hour>-<WeekDay>-<Day>-<Month>-<Year>*/

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    //Time = DateTime.Now;
                    stream = new byte[14];
                    stream[0] = (byte)stream.Length;     		/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ChgDateTime;     /* Command */
                    stream[5] = 0;     				            /* SubCommand */
                    try
                    {
                        stream[12] = (byte)Time.Second;
                        stream[11] = (byte)Time.Minute;
                        stream[10] = (byte)Time.Hour;
                        stream[9] = (byte)Time.Day;
                        stream[8] = (byte)Time.Month;
                        stream[7] = (byte)(Time.Year % 2000);
                        //stream[9] = (byte)Time.DayOfWeek;
                        //stream[8] = (byte)Time.Day;
                        //stream[7] = (byte)Time.Month;
                        //stream[6] = (byte)(Time.Year % 2000);
                    }
                    catch (Exception Ex)
                    {
                        client.Close();
                        return ReturnValues.DateTimeError;
                    }

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }


        public ReturnValues GetDateTime(string TargetIP, int TargetPort, int TargetAddress, out DateTime Time, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                /*<Second>-<Minute>-<Hour>-<WeekDay>-<Day>-<Month>-<Year>*/
                Time = new DateTime();
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						        /* Length  */
                    stream[4] = (byte)Commands.GetDateTime;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            try
                            {
                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            try
                                            {
                                                if (Response[0] == 15)
                                                {
                                                    DateTime time = new DateTime((Response[7] * 256) + Response[6], Response[8], Response[9], Response[11], Response[12], Response[13]);
                                                    //TRYME DateTime time = new DateTime(2000 + Response[7], Response[8], Response[9], Response[10], Response[11], Response[12]);
                                                    Time = time;
                                                }
                                                else
                                                {
                                                    DateTime time = new DateTime(2000 + Response[6], Response[7], Response[8], Response[10], Response[11], Response[12]);
                                                    //TRYME DateTime time = new DateTime(2000 + Response[7], Response[8], Response[9], Response[10], Response[11], Response[12]);
                                                    Time = time;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            catch (Exception)
                            {
                                client.Close();
                                return ReturnValues.Failed;
                            }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                Time = new DateTime();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDateTimeUdp(string TargetIP, int TargetPort, int TargetAddress, out DateTime Time, int msTimeOut, Converter cnv)
        {
            try
            {
                Time = new DateTime();
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.Client.SendTimeout = msTimeOut;
                client.Connect(ipend);
                if (client.Client.Connected)
                {
                    byte[] stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						        /* Length  */
                    stream[4] = (byte)Commands.GetDateTime;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    client.Send(stream, stream.Length);
                    try
                    {
                        byte[] Response = new byte[client.Client.ReceiveBufferSize];
                        int length = 0;
                        length = client.Client.ReceiveFrom(Response, ref ep);

                        byte crc = 0;
                        for (int i = 0; i < length - 1; i++)
                        {
                            crc ^= Response[i];
                        }
                        crc = (byte)(255 - crc);
                        if (crc == Response[length - 1])
                        {
                            if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                            {
                                if (Response[4] == (byte)(stream[4]) + 1)
                                {
                                    try
                                    {
                                        DateTime time = new DateTime(2000 + Response[6], Response[7], Response[8], Response[10], Response[11], Response[12]);
                                        Time = time;
                                    }
                                    catch (Exception ex)
                                    {
                                        client.Close();
                                        return ReturnValues.DateTimeError;
                                    }
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else
                                {
                                    client.Close();
                                    return ReturnValues.InvalidResponse;
                                }
                            }
                            else
                            {
                                client.Close();
                                return ReturnValues.InvalidDevice;
                            }
                        }
                        else
                        {
                            client.Close();
                            return ReturnValues.PacketError;
                        }
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        return ReturnValues.NoAnswer;
                    }
                }
                else
                {
                    return ReturnValues.PortError;
                }
            }
            catch (Exception ex)
            {
                Time = new DateTime();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDateTime_Mini(string TargetIP, int TargetPort, int TargetAddress, out DateTime Time, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                /*<Second>-<Minute>-<Hour>-<WeekDay>-<Day>-<Month>-<Year>*/
                Time = new DateTime();
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						        /* Length  */
                    stream[4] = (byte)Commands.GetDateTime;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            try
                            {
                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            string sTime = null;
                                            sTime = Response[9].ToString("D2") + "." +
                                                Response[8].ToString("D2") + "." + "20" +
                                                (Response[7] + 6).ToString("D2") + " " +
                                                //Response[10].ToString() + " " +           // Day of Week 
                                                Response[11].ToString("D2") + ":" +
                                                Response[12].ToString("D2") + ":" +
                                                Response[13].ToString("D2");

                                            try
                                            {
                                                Time = Convert.ToDateTime(sTime);
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            catch (Exception)
                            {
                                client.Close();
                                return ReturnValues.Failed;
                            }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                Time = new DateTime();
                return ReturnValues.Failed;
            }
        }


        private ReturnValues SendDataStream(byte[] stream, int p, int p_3, TcpClient client)
        {
            throw new NotImplementedException();
        }

        public ReturnValues ChangeIntervals(string TargetIP, int TargetPort, int TargetAddress, int secOnlineTimeout, int secRelayTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[9];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      			/* Length  */
                    stream[4] = (byte)Commands.ChgInterval;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)secRelayTime;
                    stream[7] = (byte)secOnlineTimeout;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        int PerNameSize = 10;

        public ReturnValues RecordAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, string PersonName, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] ChgPerName = MakeStringCompatible(PersonName);
                for (a = ChgPerName.Length; a < 10; a++) ;
                byte[] ChgPerName1 = new byte[a - ChgPerName.Length];
                for (int i = 0; i < a - ChgPerName.Length; i++) ChgPerName1[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11 + PerNameSize];                           /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(PersonId);
                    stream[8] = (byte)((PersonId >> 8));
                    stream[7] = (byte)((PersonId >> 16));
                    stream[6] = (byte)((PersonId >> 24));

                    Array.Copy(ChgPerName, 0, stream, 10, ChgPerName.Length);
                    Array.Copy(ChgPerName1, 0, stream, ChgPerName.Length + 10, a - ChgPerName.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SavePerson(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, byte StaffGroup, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 7 + 1 + 1];                           /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    stream[13] = StaffGroup;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues RecordAPersonNew(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, string PersonName, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] ChgPerName = MakeStringCompatible(PersonName);
                for (a = ChgPerName.Length; a < 10; a++) ;
                byte[] ChgPerName1 = new byte[a - ChgPerName.Length];
                for (int i = 0; i < a - ChgPerName.Length; i++) ChgPerName1[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[14 + PerNameSize];                           /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    Array.Copy(ChgPerName, 0, stream, 13, ChgPerName.Length);
                    Array.Copy(ChgPerName1, 0, stream, ChgPerName.Length + 13, a - ChgPerName.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDeviceVersionFsm(string IP, int TcpPort, int TargetAddress, out DeviceVersion Version, int msTimeOut, Converter cnv)
        {
            Version = 0;
            Version = DeviceVersion.Undefined;

            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(IP, TcpPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream = new byte[7];

                stream[0] = (byte)stream.Length;     				/*Packet Length*/
                stream[1] = 153;    	    /* Prefix  */
                stream[2] = (byte)TargetAddress;                              /* Device Address  */
                stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                stream[4] = (byte)Commands.GetVersion;                  /* Command */
                stream[5] = 0;
                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                byte[] Response; int length = 0;

                ReturnValues Result1 = SendDataStreamFree(stream, 0, stream.Length, client, cnv);
                if (Result1 == ReturnValues.Successful)
                {
                    Result1 = GetDataStreamFree(out Response, out length, client, msTimeOut);
                    if (Result1 == ReturnValues.Successful)
                    {
                        byte crc = 0;
                        for (int i = 0; i < length - 1; i++)
                        {
                            crc ^= Response[i];
                        }
                        crc = (byte)(255 - crc);
                        if (crc == Response[length - 1])
                            if ((Response[1] == 154) & (Response[2] == (byte)TargetAddress))
                                if (Response[4] == (byte)(stream[4]) + 1)
                                {
                                    try
                                    {

                                        Version = (DeviceVersion)Enum.Parse(typeof(DeviceVersion), Response[6].ToString());
                                        // Version = Response[6];

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    catch (Exception)
                                    {
                                        client.Close();
                                        return ReturnValues.Failed;
                                    }

                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            else { client.Close(); return ReturnValues.InvalidDevice; }
                        else { client.Close(); return ReturnValues.PacketError; }
                    }
                    else { client.Close(); return Result1; }
                }


                client.Close();
                b++;
                if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                return Result1;
            }
            catch
            {
                Version = 0;
                Version = DeviceVersion.Undefined;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues RecordAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, string PersonName, DateTime PersonClockEn, DateTime PersonClockEx, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] ChgPerName = MakeStringCompatible(PersonName);
                // byte[] ChgPerClock = MakeStringCompatible(PersonClock);
                for (a = ChgPerName.Length; a < 10; a++) ;
                byte[] ChgPerName1 = new byte[a - ChgPerName.Length];
                for (int i = 0; i < a - ChgPerName.Length; i++) ChgPerName1[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12 + PerNameSize + 10];                           /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	    /* Prefix  */
                    stream[2] = (byte)TargetAddress;                    /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(PersonId);
                    stream[8] = (byte)((PersonId >> 8));
                    stream[7] = (byte)((PersonId >> 16));
                    stream[6] = (byte)((PersonId >> 24));

                    Array.Copy(ChgPerName, 0, stream, 10, ChgPerName.Length);
                    Array.Copy(ChgPerName1, 0, stream, ChgPerName.Length + 10, a - ChgPerName.Length);

                    stream[20] = (byte)(PersonClockEn.Year % 2000);
                    stream[21] = (byte)PersonClockEn.Month;
                    stream[22] = (byte)PersonClockEn.Day;
                    stream[23] = (byte)PersonClockEn.Hour;
                    stream[24] = (byte)PersonClockEn.Minute;

                    stream[25] = (byte)(PersonClockEx.Year % 2000);
                    stream[26] = (byte)PersonClockEx.Month;
                    stream[27] = (byte)PersonClockEx.Day;
                    stream[28] = (byte)PersonClockEx.Hour;
                    stream[29] = (byte)PersonClockEx.Minute;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }
        /*
        public ReturnValues RecordAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, string PersonName, int msTimeOut, Converter cnv)
                {
                    try
                    {
                
                        TcpClient client = new TcpClient();
                        ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                        if (Result != ReturnValues.Succesfull)
                        {
                            client.Close();
                            return Result;
                        }

                        Queue<byte> TotalMessage = new Queue<byte>();
                        byte[] stream;

                        if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Succesfull)
                        {
                            stream = new byte[11 + PerNameSize];                           //PersonalDataSize+DefaultLength
                            stream[0] = (byte)stream.Length;     				//Packet Length
                            stream[1] = (byte)DataStruct.PCToReader;    	    // Prefix  
                            stream[2] = (byte)TargetAddress;                    // Device Address 
                            stream[3] = (byte)(stream.Length - 4);      	   // Length 
                            stream[4] = (byte)Commands.RcdPerson;               // Command 
                            stream[5] = 0;     				                    // SubCommand 

                            stream[9] = (byte)(PersonId);
                            stream[8] = (byte)((PersonId >> 8));
                            stream[7] = (byte)((PersonId >> 16));
                            stream[6] = (byte)((PersonId >> 24));

                            byte[] name = MakeStringCompatible(PersonName);
                            for (int i = 0; i < PerNameSize; i++)
                            {   
                              stream[10 + i] = (byte)name[i];
                            }


                            stream[stream.Length - 1] = 0;
                            for (int i = 0; i < stream.Length - 1; i++)
                            {
                                stream[stream.Length - 1] ^= stream[i];
                            }
                            stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                            byte[] packet; int length = 0;

                            if (SendDataStream(stream, 0, stream.Length, client, cnv) == ReturnValues.Succesfull)
                            {
                                if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Succesfull)
                                {
                                    byte crc = 0;
                                    for (int i = 0; i < length - 1; i++)
                                    {
                                        crc ^= packet[i];
                                    }
                                    crc = (byte)(255 - crc);
                                    if (crc == packet[length - 1])
                                        if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                            if (packet[4] == (byte)(stream[4]) + 1)
                                            {
                                                client.Close();
                                                return ReturnValues.Succesfull;
                                            }
                                            else { client.Close(); return ReturnValues.InvalidResponse; }
                                        else { client.Close(); return ReturnValues.InvalidDevice; }
                                    else { client.Close(); return ReturnValues.PacketError; }
                                }
                                else { client.Close(); return ReturnValues.NoAnswer; }
                            }
                            client.Close();
                            return ReturnValues.DeviceNotFound;
                        }
                        client.Close();
                        return ReturnValues.NoAnswerFromCnv;
                    }
                    catch (Exception)
                    {
                        return ReturnValues.Failed;
                    }

                } */
        public ReturnValues FindPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, out UInt32 PerIndex, int msTimeOut, Converter cnv)
        {
            PerIndex = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.FindPerIdx;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(PersonId);
                    stream[8] = (byte)((PersonId >> 8));
                    stream[7] = (byte)((PersonId >> 16));
                    stream[6] = (byte)((PersonId >> 24));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        { client.Close(); return ReturnValues.PersonNotFound; }

                                        byte[] pc = new byte[4];
                                        Array.Copy(packet, 6, pc, 0, 4);

                                        PerIndex = pc[3];
                                        PerIndex = (PerIndex << 8) | pc[2];
                                        PerIndex = (PerIndex << 8) | pc[1];
                                        PerIndex = (PerIndex << 8) | pc[0];

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues FindPersonNew(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, out UInt32 PerIndex, int msTimeOut, Converter cnv)
        {
            PerIndex = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[14];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.FindPerIdx;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        { client.Close(); return ReturnValues.PersonNotFound; }

                                        byte[] pc = new byte[4];
                                        Array.Copy(packet, 6, pc, 0, 4);

                                        PerIndex = pc[3];
                                        PerIndex = (PerIndex << 8) | pc[2];
                                        PerIndex = (PerIndex << 8) | pc[1];
                                        PerIndex = (PerIndex << 8) | pc[0];

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues RecordAPersonNew(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, byte GrpDoor0, byte GrpDoor1, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[16];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    stream[13] = GrpDoor0;
                    stream[14] = GrpDoor1;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues RecordAPersonNew(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[14];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues RecordAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.RcdPerson;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(PersonId);
                    stream[8] = (byte)((PersonId >> 8));
                    stream[7] = (byte)((PersonId >> 16));
                    stream[6] = (byte)((PersonId >> 24));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues ChangeDeviceName(string TargetIP, int TargetPort, int TargetAddress, string DeviceName, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] ChgDvcName = MakeStringCompatible(DeviceName);
                for (a = ChgDvcName.Length; a < 15; a++) ;
                byte[] ChgDvcName1 = new byte[a - ChgDvcName.Length];
                for (int i = 0; i < a - ChgDvcName.Length; i++) ChgDvcName1[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11 + 12];                          /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	    /* Length  */
                    stream[4] = (byte)Commands.ChgDvcName;               /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    Array.Copy(ChgDvcName, 0, stream, 6, ChgDvcName.Length);
                    Array.Copy(ChgDvcName1, 0, stream, ChgDvcName.Length + 6, a - ChgDvcName.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues RecordPeople(string TargetIP, int TargetPort, int TargetAddress, UInt32[] People, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    int OneTimeRecord = 32; int MaxIDSize = 4;

                    stream = new byte[7 + (People.Length * MaxIDSize)];       /*PersonalDataSize+DefaultLength*/
                    stream[0] = (byte)stream.Length;     				    /*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;                        /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	        /* Length  */
                    stream[4] = (byte)Commands.RcdPeople;                   /* Command */
                    if (People.Length > OneTimeRecord)
                    {
                        client.Close();
                        return ReturnValues.MessageLengthIsTooBig;
                    }
                    stream[5] = (byte)People.Length;     				                        /* SubCommand */
                    int j = 0;
                    byte[][] id = new byte[People.Length][];
                    for (int i = 0; i < id.Length; i++)
                    {
                        id[i] = new byte[4];
                        id[i][0] = (byte)(People[i] >> 0);
                        id[i][1] = (byte)(People[i] >> 8);
                        id[i][2] = (byte)(People[i] >> 16);
                        id[i][3] = (byte)(People[i] >> 24);
                    }
                    byte[] ids = new byte[id.Length * 4];
                    for (int i = 0; i < id.Length; i++)
                    {
                        ids[i] = id[i][0];
                    }
                    for (int i = 0; i < id.Length; i++)
                    {
                        ids[i + id.Length] = id[i][1];
                    }
                    for (int i = 0; i < id.Length; i++)
                    {
                        ids[i + id.Length * 2] = id[i][2];
                    }
                    for (int i = 0; i < id.Length; i++)
                    {
                        ids[i + id.Length * 3] = id[i][3];
                    }
                    Array.Copy(ids, 0, stream, 6, ids.Length);
                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }

        }

        public ReturnValues EraseAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7 + 8];                             /*default + ID.Length*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ErsPerson;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((PersonId >> 28) & 0x0f);
                    stream[7] = (byte)((PersonId >> 24) & 0x0f);
                    stream[8] = (byte)((PersonId >> 20) & 0x0f);
                    stream[9] = (byte)((PersonId >> 16) & 0x0f);
                    stream[10] = (byte)((PersonId >> 12) & 0x0f);
                    stream[11] = (byte)((PersonId >> 8) & 0x0f);
                    stream[12] = (byte)((PersonId >> 4) & 0x0f);
                    stream[13] = (byte)((PersonId) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseAPersonNew(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7 + 7];                             /*default + ID.Length*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ErsPerson;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[12] = (byte)(PersonId);
                    stream[11] = (byte)((PersonId >> 8));
                    stream[10] = (byte)((PersonId >> 16));
                    stream[9] = (byte)((PersonId >> 24));
                    stream[8] = (byte)((PersonId >> 32));
                    stream[7] = (byte)((PersonId >> 40));
                    stream[6] = (byte)((PersonId >> 48));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }


        public ReturnValues EraseAPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, DateTime PersonClockEn, DateTime PersonClockEx, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7 + 8 + 10];                             /*default + ID.Length*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ErsPerson;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((PersonId >> 28) & 0x0f);
                    stream[7] = (byte)((PersonId >> 24) & 0x0f);
                    stream[8] = (byte)((PersonId >> 20) & 0x0f);
                    stream[9] = (byte)((PersonId >> 16) & 0x0f);
                    stream[10] = (byte)((PersonId >> 12) & 0x0f);
                    stream[11] = (byte)((PersonId >> 8) & 0x0f);
                    stream[12] = (byte)((PersonId >> 4) & 0x0f);
                    stream[13] = (byte)((PersonId) & 0x0f);

                    stream[14] = (byte)(PersonClockEn.Year % 2000);
                    stream[15] = (byte)PersonClockEn.Month;
                    stream[16] = (byte)PersonClockEn.Day;
                    stream[17] = (byte)PersonClockEn.Hour;
                    stream[18] = (byte)PersonClockEn.Minute;

                    stream[19] = (byte)(PersonClockEx.Year % 2000);
                    stream[20] = (byte)PersonClockEx.Month;
                    stream[21] = (byte)PersonClockEx.Day;
                    stream[22] = (byte)PersonClockEx.Hour;
                    stream[23] = (byte)PersonClockEx.Minute;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseABalckListPerson(string TargetIP, int TargetPort, int TargetAddress, UInt32 PersonId, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7 + 8];                             /*default + ID.Length*/
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      						        /* Length  */
                    stream[4] = (byte)Commands.ErsBlackPerson;             /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((PersonId >> 28) & 0x0f);
                    stream[7] = (byte)((PersonId >> 24) & 0x0f);
                    stream[8] = (byte)((PersonId >> 20) & 0x0f);
                    stream[9] = (byte)((PersonId >> 16) & 0x0f);
                    stream[10] = (byte)((PersonId >> 12) & 0x0f);
                    stream[11] = (byte)((PersonId >> 8) & 0x0f);
                    stream[12] = (byte)((PersonId >> 4) & 0x0f);
                    stream[13] = (byte)((PersonId) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        if (packet[5] == (byte)ReturnValues.PersonNotFound)
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseAllPerson(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ErsAllPerson;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReturnValues EraseBlackList(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     			/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;                /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      	/* Length  */
                    stream[4] = (byte)Commands.ErsBlackIndex;       /* Command */
                    stream[5] = 0;     				                /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ReturnValues EraseAllLogData(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ErsAllLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (packet[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseAllAccessSpareLog(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.EraseSpareLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (packet[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseAllEventLog(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.EraseEvent;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (packet[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetLastLogData(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out String strLogTime, out AccessDirection AccessDir, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                LogPacket = null;
                strLogTime = null;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(LogIndex & 0x0f);
                    stream[7] = (byte)((LogIndex >> 4) & 0x0f);
                    stream[8] = (byte)((LogIndex >> 8) & 0x0f);
                    stream[9] = (byte)((LogIndex >> 12) & 0x0f);
                    stream[10] = (byte)((LogIndex >> 16) & 0x0f);

                    if (stream[10] == 0) stream[10] = 10;
                    stream[stream.Length - 1] = 10;
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];
                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                                if (Response[length - 1] == 10/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.LogNotFound;
                                            }

                                            LogID = (UInt64)(Response[13] & 0x0f);
                                            LogID = (UInt64)((LogID << 4) | Response[12]);
                                            LogID = (UInt64)((LogID << 4) | Response[11]);
                                            LogID = (UInt64)((LogID << 4) | Response[10]);
                                            LogID = (UInt64)((LogID << 4) | Response[9]);
                                            LogID = (UInt64)((LogID << 4) | Response[8]);
                                            LogID = (UInt64)((LogID << 4) | Response[7]);
                                            LogID = (UInt64)((LogID << 4) | Response[6]);
                                            //LogID = (UInt32)(Response[13]);
                                            //LogID = (UInt32)((LogID << 4) | Response[12]);
                                            //LogID = (UInt32)((LogID << 4) | Response[11]);
                                            //LogID = (UInt32)((LogID << 4) | Response[10]);
                                            //LogID = (UInt32)((LogID << 4) | Response[9]);
                                            //LogID = (UInt32)((LogID << 4) | Response[8]);
                                            //LogID = (UInt32)((LogID << 4) | Response[7]);
                                            //LogID = (UInt32)((LogID << 4) | Response[6]);

                                            //strLogTime = null;
                                            //strLogTime = Response[16].ToString("D2") + "." +
                                            //             Response[15].ToString("D2") + "." + "20" +
                                            //             Response[14].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //             Response[17].ToString("D2") + ":" +
                                            //             Response[18].ToString("D2") + ":" +
                                            //             Response[19].ToString("D2");
                                            try
                                            {
                                                //LogTime = Convert.ToDateTime(strLogTime);
                                                DateTime time = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], Response[19]);
                                                LogTime = time;
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }

                                            AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[20].ToString());
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.LogIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                strLogTime = null;
                return ReturnValues.Failed;
            }
        }


        public ReturnValues GetLastLogDataNew(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out String strLogTime, out AccessDirection AccessDir, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                LogPacket = null;
                strLogTime = null;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(LogIndex & 0x0f);
                    stream[7] = (byte)((LogIndex >> 4) & 0x0f);
                    stream[8] = (byte)((LogIndex >> 8) & 0x0f);
                    stream[9] = (byte)((LogIndex >> 12) & 0x0f);
                    stream[10] = (byte)((LogIndex >> 16) & 0x0f);

                    if (stream[10] == 0) stream[10] = 10;
                    stream[stream.Length - 1] = 10;
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];
                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                                if (Response[length - 1] == 10/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.LogNotFound;
                                            }

                                            LogID = (UInt64)(Response[13]);
                                            LogID = (UInt64)((LogID << 8) | Response[12]);
                                            LogID = (UInt64)((LogID << 8) | Response[11]);
                                            LogID = (UInt64)((LogID << 8) | Response[10]);
                                            LogID = (UInt64)((LogID << 8) | Response[9]);
                                            LogID = (UInt64)((LogID << 8) | Response[8]);
                                            LogID = (UInt64)((LogID << 8) | Response[7]);
                                            LogID = (UInt64)((LogID << 8) | Response[6]);

                                            //strLogTime = null;
                                            //strLogTime = Response[16].ToString("D2") + "." +
                                            //             Response[15].ToString("D2") + "." + "20" +
                                            //             Response[14].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //             Response[17].ToString("D2") + ":" +
                                            //             Response[18].ToString("D2") + ":" +
                                            //             Response[19].ToString("D2");
                                            try
                                            {
                                                DateTime time = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], Response[19]);
                                                LogTime = time;
                                                // LogTime = Convert.ToDateTime(strLogTime);
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }

                                            AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[20].ToString());
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.LogIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                strLogTime = null;
                return ReturnValues.Failed;
            }
        }

        public bool AgainTryGoToStar(int count)
        {
            if (count < 5)
                return true;
            return false;
        }
        public byte[] timebuf = new byte[6];
        public ReturnValues GetLastLogDataNew(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out AccessDirection AccessDir, out FeedBackControl fb, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                LogPacket = null; fb = FeedBackControl.False;

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(LogIndex & 0x0f);
                    stream[7] = (byte)((LogIndex >> 4) & 0x0f);
                    stream[8] = (byte)((LogIndex >> 8) & 0x0f);
                    stream[9] = (byte)((LogIndex >> 12) & 0x0f);
                    stream[10] = (byte)((LogIndex >> 16) & 0x0f);

                    if (stream[10] == 0) stream[10] = 10;
                    stream[stream.Length - 1] = 10;
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];
                            if (Response.Length >= 20)
                            {
                                Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                                if (Response[length - 1] == 10/*true*/)
                                {
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    {
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close(); 
                                                if (AgainTryGoToStar(b++)) goto StartAgain;
                                                return ReturnValues.LogNotFound;
                                            }
                                            if (Response.Length == 20)
                                            {
                                                LogID = (UInt64)((LogID << 8) | Response[12]);
                                                LogID = (UInt64)((LogID << 8) | Response[11]);
                                                LogID = (UInt64)((LogID << 8) | Response[10]);
                                                LogID = (UInt64)((LogID << 8) | Response[9]);
                                                LogID = (UInt64)((LogID << 8) | Response[8]);
                                                LogID = (UInt64)((LogID << 8) | Response[7]);
                                                LogID = (UInt64)((LogID << 8) | Response[6]);

                                                try
                                                {
                                                    long dt = Response[16];
                                                    dt = (dt << 8) | Response[15];
                                                    dt = (dt << 8) | Response[14];
                                                    dt = (dt << 8) | Response[13];

                                                    DateTime time = new DateTime(1970, 1, 1);
                                                    time = time.AddSeconds(dt);
                                                    time = time.AddHours(0);   // GMT + 3

                                                    //DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                                    LogTime = time;
                                                }
                                                catch (Exception ex)
                                                {
                                                    client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                                    return ReturnValues.DateTimeError;
                                                }

                                                AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[17].ToString());
                                                fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[18].ToString());

                                                client.Close();
                                                return ReturnValues.Successful;
                                            }
                                            else
                                            {
                                                if (Response[13] != 0)
                                                {
                                                    LogID = (UInt64)((LogID << 8) | Response[12]);
                                                    LogID = (UInt64)((LogID << 8) | Response[11]);
                                                    LogID = (UInt64)((LogID << 8) | Response[10]);
                                                    LogID = (UInt64)((LogID << 8) | Response[9]);
                                                    LogID = (UInt64)((LogID << 8) | Response[8]);
                                                    LogID = (UInt64)((LogID << 8) | Response[7]);
                                                    LogID = (UInt64)((LogID << 8) | Response[6]);

                                                    if (Response[20] == 100)
                                                        return ReturnValues.ErrorData;
                                                    try
                                                    {
                                                        DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                                        LogTime = time;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                                        Array.Copy(Response, 13, timebuf, 0, 6);
                                                        return ReturnValues.DateTimeError;
                                                    }

                                                    AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[19].ToString());
                                                    fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[20].ToString());

                                                    client.Close();
                                                    return ReturnValues.Successful;
                                                }
                                                else
                                                {
                                                    LogID = 0;
                                                    LogID = (UInt64)(Response[13] << 28);
                                                    LogID |= (UInt64)(Response[12] << 24);
                                                    LogID |= (UInt64)(Response[11] << 20);
                                                    LogID |= (UInt64)(Response[10] << 16);
                                                    LogID |= (UInt64)(Response[9] << 12);
                                                    LogID |= (UInt64)(Response[8] << 8);
                                                    LogID |= (UInt64)(Response[7] << 4);
                                                    LogID |= (UInt64)(Response[6] << 0);

                                                    //LogID = (UInt64)(Response[13]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[12]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[11]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[10]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[9]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[8]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[7]);
                                                    //LogID = (UInt64)((LogID << 8) | Response[6]);

                                                    try
                                                    {
                                                        DateTime time = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], Response[19]);
                                                        LogTime = time;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                                        return ReturnValues.DateTimeError;
                                                    }

                                                    AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[20].ToString());
                                                    fb = FeedBackControl.True;
                                                    client.Close();
                                                    return ReturnValues.Successful;
                                                }
                                            }
                                        }
                                        else if (Response[4] == (byte)Commands.Busy + 1)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.DeviceBusy;
                                        }
                                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.LogIndexOvf; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                fb = FeedBackControl.False;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetAccessLog(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out DenyWithReason Reason, out Direction DirState, out FeedBackControl fb, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); Reason = DenyWithReason.Denied; DirState = Direction.Both;
                LogPacket = null; fb = FeedBackControl.False;

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((LogIndex >> 0) & 0xFF);
                    stream[7] = (byte)((LogIndex >> 8) & 0xFF);
                    stream[8] = (byte)((LogIndex >> 16) & 0xFF);
                    stream[9] = (byte)((LogIndex >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];

                            Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                            if (Response[length - 1] == lrc)
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.LogNotFound;
                                        }

                                        LogID = (UInt64)((LogID << 8) | Response[12]);
                                        LogID = (UInt64)((LogID << 8) | Response[11]);
                                        LogID = (UInt64)((LogID << 8) | Response[10]);
                                        LogID = (UInt64)((LogID << 8) | Response[9]);
                                        LogID = (UInt64)((LogID << 8) | Response[8]);
                                        LogID = (UInt64)((LogID << 8) | Response[7]);
                                        LogID = (UInt64)((LogID << 8) | Response[6]);

                                        if (Response[20] == 100)
                                            return ReturnValues.ErrorData;
                                        try
                                        {
                                            DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                            LogTime = time;
                                        }
                                        catch (Exception ex)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            Array.Copy(Response, 13, timebuf, 0, 6);
                                            return ReturnValues.DateTimeError;
                                        }
                                        int Rsn = Response[19] & 0xF0;
                                        int dir = Response[19] & 0x0F;
                                        Reason = (DenyWithReason)Enum.Parse(typeof(DenyWithReason), Rsn.ToString());
                                        DirState = (Direction)Enum.Parse(typeof(Direction), dir.ToString());
                                        fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[20].ToString());

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime();
                Reason = DenyWithReason.Denied;
                DirState = Direction.Both;
                fb = FeedBackControl.False;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetAccessLog(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out AccessDirection AccessDir, out FeedBackControl fb, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                LogPacket = null; fb = FeedBackControl.False;

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((LogIndex >> 0) & 0xFF);
                    stream[7] = (byte)((LogIndex >> 8) & 0xFF);
                    stream[8] = (byte)((LogIndex >> 16) & 0xFF);
                    stream[9] = (byte)((LogIndex >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];

                            Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                            if (Response[length - 1] == lrc)
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.LogNotFound;
                                        }

                                        LogID = (UInt64)((LogID << 8) | Response[12]);
                                        LogID = (UInt64)((LogID << 8) | Response[11]);
                                        LogID = (UInt64)((LogID << 8) | Response[10]);
                                        LogID = (UInt64)((LogID << 8) | Response[9]);
                                        LogID = (UInt64)((LogID << 8) | Response[8]);
                                        LogID = (UInt64)((LogID << 8) | Response[7]);
                                        LogID = (UInt64)((LogID << 8) | Response[6]);

                                        if (Response[20] == 100)
                                            return ReturnValues.ErrorData;
                                        try
                                        {
                                            DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                            LogTime = time;
                                        }
                                        catch (Exception ex)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            Array.Copy(Response, 13, timebuf, 0, 6);
                                            return ReturnValues.DateTimeError;
                                        }

                                        AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[19].ToString());
                                        fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[20].ToString());

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                fb = FeedBackControl.False;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetAccessSpareLog(string TargetIP, int TargetPort, int TargetAddress, long LogIndex, bool part, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out AccessDirection AccessDir, out FeedBackControl fb, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.DeniedExit;
                LogPacket = null; fb = FeedBackControl.False;

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetSpareLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = Convert.ToByte(part);
                    stream[7] = (byte)((LogIndex >> 0) & 0xFF);
                    stream[8] = (byte)((LogIndex >> 8) & 0xFF);
                    stream[9] = (byte)((LogIndex >> 16) & 0xFF);
                    stream[10] = (byte)((LogIndex >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];

                            Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                            if (Response[length - 1] == lrc)
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.LogNotFound;
                                        }

                                        LogID = (UInt64)((LogID << 8) | Response[12]);
                                        LogID = (UInt64)((LogID << 8) | Response[11]);
                                        LogID = (UInt64)((LogID << 8) | Response[10]);
                                        LogID = (UInt64)((LogID << 8) | Response[9]);
                                        LogID = (UInt64)((LogID << 8) | Response[8]);
                                        LogID = (UInt64)((LogID << 8) | Response[7]);
                                        LogID = (UInt64)((LogID << 8) | Response[6]);

                                        if (Response[20] == 100)
                                            return ReturnValues.ErrorData;
                                        try
                                        {
                                            DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                            LogTime = time;
                                        }
                                        catch (Exception ex)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            Array.Copy(Response, 13, timebuf, 0, 6);
                                            return ReturnValues.DateTimeError;
                                        }

                                        AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[19].ToString());
                                        fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[20].ToString());

                                        client.Close();
                                        return ReturnValues.Successful;

                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime();
                AccessDir = AccessDirection.DeniedExit;
                fb = FeedBackControl.False;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetAccessSpareLogNew(string TargetIP, int TargetPort, int TargetAddress, long LogIndex, bool part, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out DenyWithReason Reason, out Direction DirState, out FeedBackControl fb, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                StartAgain:
                LogID = 0; LogTime = new DateTime(); Reason = DenyWithReason.Denied; DirState = Direction.Both;
                LogPacket = null; fb = FeedBackControl.False;

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetSpareLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = Convert.ToByte(part);
                    stream[7] = (byte)((LogIndex >> 0) & 0xFF);
                    stream[8] = (byte)((LogIndex >> 8) & 0xFF);
                    stream[9] = (byte)((LogIndex >> 16) & 0xFF);
                    stream[10] = (byte)((LogIndex >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];

                            Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                            if (Response[length - 1] == lrc)
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.LogNotFound;
                                        }
                                        LogID = (UInt64)((LogID << 8) | Response[12]);
                                        LogID = (UInt64)((LogID << 8) | Response[11]);
                                        LogID = (UInt64)((LogID << 8) | Response[10]);
                                        LogID = (UInt64)((LogID << 8) | Response[9]);
                                        LogID = (UInt64)((LogID << 8) | Response[8]);
                                        LogID = (UInt64)((LogID << 8) | Response[7]);
                                        LogID = (UInt64)((LogID << 8) | Response[6]);

                                        if (Response[20] == 100)
                                            return ReturnValues.ErrorData;
                                        try
                                        {
                                            DateTime time = new DateTime(2000 + Response[13], Response[14], Response[15], Response[16], Response[17], Response[18]);
                                            LogTime = time;
                                        }
                                        catch (Exception ex)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.DateTimeError;
                                        }

                                        int Rsn = Response[19] & 0xF0;
                                        int dir = Response[19] & 0x0F;
                                        Reason = (DenyWithReason)Enum.Parse(typeof(DenyWithReason), Rsn.ToString());
                                        DirState = (Direction)Enum.Parse(typeof(Direction), dir.ToString());
                                        fb = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[20].ToString());

                                        client.Close();
                                        return ReturnValues.Successful;

                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime();
                Reason = DenyWithReason.Denied;
                DirState = Direction.Both;
                fb = FeedBackControl.False;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetEventLog(string TargetIP, int TargetPort, int TargetAddress, long Index, out Events code, out DateTime EventTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                code = Events.NoEvent;
                EventTime = new DateTime();

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetEvent;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((Index >> 0) & 0xFF);
                    stream[7] = (byte)((Index >> 8) & 0xFF);
                    stream[8] = (byte)((Index >> 16) & 0xFF);
                    stream[9] = (byte)((Index >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);

                            if (Response[length - 1] == lrc)
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.LogNotFound;
                                        }

                                        code = (Events)Response[6];
                                        try
                                        {
                                            DateTime time = new DateTime(2000 + Response[7], Response[8], Response[9], Response[10], Response[11], Response[12]);
                                            EventTime = time;
                                        }
                                        catch (Exception ex)
                                        {
                                            client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                            return ReturnValues.DateTimeError;
                                        }

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain;
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidResponse; }
                                }
                                else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.InvalidDevice; }
                            }
                            else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return ReturnValues.PacketError; }
                        }
                        else { client.Close(); if (AgainTryGoToStar(b++)) goto StartAgain; return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                code = Events.NoEvent;
                EventTime = new DateTime();
                return ReturnValues.Failed;
            }
        }


        public ReturnValues GetLastLogDataArray(string TargetIP, int TargetPort, int TargetAddress, int BgnIndex, int FnsIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out AccessDirection AccessDir, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                LogPacket = null;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[15];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(BgnIndex);
                    stream[8] = (byte)((BgnIndex >> 8));
                    stream[7] = (byte)((BgnIndex >> 16));
                    stream[6] = (byte)((BgnIndex >> 24));

                    stream[13] = (byte)(FnsIndex);
                    stream[12] = (byte)((FnsIndex >> 8));
                    stream[11] = (byte)((FnsIndex >> 16));
                    stream[10] = (byte)((FnsIndex >> 24));

                    //stream[6] = (byte)(FnsIndex & 0x0f);
                    //stream[7] = (byte)((FnsIndex >> 4) & 0x0f);
                    //stream[8] = (byte)((FnsIndex >> 8) & 0x0f);
                    //stream[9] = (byte)((FnsIndex >> 12) & 0x0f);
                    //stream[10] = (byte)((FnsIndex >> 16) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    //if (stream[10] == 0) stream[10] = 10;
                    //stream[stream.Length - 1] = 10;

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];
                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                                if (Response[length - 1] == 10/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.LogNotFound;
                                            }

                                            LogID = (UInt64)(Response[13]);
                                            LogID = (UInt64)((LogID << 8) | Response[12]);
                                            LogID = (UInt64)((LogID << 8) | Response[11]);
                                            LogID = (UInt64)((LogID << 8) | Response[10]);
                                            LogID = (UInt64)((LogID << 8) | Response[9]);
                                            LogID = (UInt64)((LogID << 8) | Response[8]);
                                            LogID = (UInt64)((LogID << 8) | Response[7]);
                                            LogID = (UInt64)((LogID << 8) | Response[6]);

                                            try
                                            {
                                                DateTime time = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], Response[19]);
                                                LogTime = time;
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }

                                            AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[20].ToString());
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.LogIndexOvf; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetLastLogData(string TargetIP, int TargetPort, int TargetAddress, int LogIndex, out byte[] LogPacket, out UInt64 LogID, out DateTime LogTime, out String strLogTime, out AccessDirection AccessDir, out FeedBackControl FBack, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                FBack = FeedBackControl.True;
                LogPacket = null;
                strLogTime = null;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetLog;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(LogIndex & 0x0f);
                    stream[7] = (byte)((LogIndex >> 4) & 0x0f);
                    stream[8] = (byte)((LogIndex >> 8) & 0x0f);
                    stream[9] = (byte)((LogIndex >> 12) & 0x0f);
                    stream[10] = (byte)((LogIndex >> 16) & 0x0f);

                    if (stream[10] == 0) stream[10] = 10;
                    stream[stream.Length - 1] = 10;
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            LogPacket = new byte[11];
                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, LogPacket, 0, LogPacket.Length);

                                if (Response[length - 1] == 10/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.LogNotFound;
                                            }

                                            LogID = (UInt64)(Response[13] & 0x0f);
                                            LogID = (UInt64)((LogID << 4) | Response[12]);
                                            LogID = (UInt64)((LogID << 4) | Response[11]);
                                            LogID = (UInt64)((LogID << 4) | Response[10]);
                                            LogID = (UInt64)((LogID << 4) | Response[9]);
                                            LogID = (UInt64)((LogID << 4) | Response[8]);
                                            LogID = (UInt64)((LogID << 4) | Response[7]);
                                            LogID = (UInt64)((LogID << 4) | Response[6]);

                                            strLogTime = null;
                                            strLogTime = Response[16].ToString("D2") + "." +
                                                         Response[15].ToString("D2") + "." + "20" +
                                                         Response[14].ToString("D2") + " " +
                                                //Response[9].ToString() + " " +           // Day of Week 
                                                         Response[17].ToString("D2") + ":" +
                                                         Response[18].ToString("D2") + ":" +
                                                         Response[19].ToString("D2");
                                            try
                                            {
                                                //LogTime = Convert.ToDateTime(strLogTime);
                                                DateTime time = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], Response[19]);
                                                LogTime = time;
                                            }
                                            catch (Exception ex)
                                            {
                                                client.Close();
                                                return ReturnValues.DateTimeError;
                                            }

                                            AccessDir = (AccessDirection)Enum.Parse(typeof(AccessDirection), Response[20].ToString());
                                            FBack = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[21].ToString());

                                            if (Response[20] == 65)
                                            {
                                                if (Response[21] != 1 && Response[21] != 0)
                                                {
                                                    Response[21] = 1;
                                                    FBack = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[21].ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (Response[21] != 1 && Response[21] != 0)
                                                {
                                                    Response[21] = 0;
                                                    FBack = (FeedBackControl)Enum.Parse(typeof(FeedBackControl), Response[21].ToString());
                                                }
                                            }


                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.LogIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogPacket = null;
                LogID = 0; LogTime = new DateTime(); AccessDir = AccessDirection.AcceptedEntry;
                FBack = FeedBackControl.True;
                strLogTime = null;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetPersonID(string TargetIP, int TargetPort, int TargetAddress, int PerIndex, out byte[] PerPacket, out UInt32 PerID, out string PerName, out DateTime StartDate, out DateTime EndDate, int msTimeOut, Converter cnv)
        {
            PerID = 0;
            PerPacket = null;
            PerName = null;
            StartDate = new DateTime();
            EndDate = new DateTime();
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12 + 20];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetPerson;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(PerIndex & 0x0f);
                    stream[7] = (byte)((PerIndex >> 4) & 0x0f);
                    stream[8] = (byte)((PerIndex >> 8) & 0x0f);
                    stream[9] = (byte)((PerIndex >> 12) & 0x0f);
                    stream[10] = (byte)((PerIndex >> 16) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];

                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1]/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            PerID = (UInt32)(Response[13] & 0x0f);
                                            PerID = (UInt32)((PerID << 4) | (Response[12] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[11] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[10] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[9] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[8] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[7] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[6] & 0x0f));

                                            StartDate = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], 0);
                                            EndDate = new DateTime(2000 + Response[19], Response[20], Response[21], Response[22], Response[23], 0);
                                            PerName = System.Text.ASCIIEncoding.ASCII.GetString(Response, 24, 10);

                                            //string sTime = null;
                                            //sTime = Response[14].ToString("D2") + "." +
                                            //        Response[15].ToString("D2") + "." + "20" +
                                            //        Response[16].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //        Response[17].ToString("D2") + ":" +
                                            //        Response[18].ToString("D2") + ":" +
                                            //        Response[19].ToString("D2") + "." +
                                            //        Response[20].ToString("D2") + "." + "20" +
                                            //        Response[21].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //        Response[22].ToString("D2") + ":" +
                                            //        Response[23].ToString("D2");


                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetFindPersonID(string TargetIP, int TargetPort, int TargetAddress, UInt64 PersonId, int ChgIndx, out int PerIndex, out byte[] PerPacket, out UInt32 PerID, out string PerName, out DateTime StartDate, out DateTime EndDate, int msTimeOut, Converter cnv)
        {
            PerID = 0;
            PerIndex = 0;
            PerPacket = null;
            PerName = null;
            StartDate = new DateTime();
            EndDate = new DateTime();
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[13];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.FindPerson;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */

                    stream[9] = (byte)(PersonId);
                    stream[8] = (byte)((PersonId >> 8));
                    stream[7] = (byte)((PersonId >> 16));
                    stream[6] = (byte)((PersonId >> 24));

                    stream[11] = (byte)(ChgIndx);
                    stream[10] = (byte)((ChgIndx >> 8));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];

                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1]/*true*/)
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            /*
                                            PerIndex = pc[3];
                                            PerIndex = (PerIndex << 8) | pc[2];
                                            PerIndex = (PerIndex << 8) | pc[1];
                                            PerIndex = (PerIndex << 8) | pc[0];*/
                                            PerID = (UInt32)(Response[13] & 0x0f);
                                            PerID = (UInt32)((PerID << 4) | (Response[12] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[11] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[10] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[9] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[8] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[7] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[6] & 0x0f));

                                            StartDate = new DateTime(2000 + Response[14], Response[15], Response[16], Response[17], Response[18], 0);
                                            EndDate = new DateTime(2000 + Response[19], Response[20], Response[21], Response[22], Response[23], 0);
                                            PerName = System.Text.ASCIIEncoding.ASCII.GetString(Response, 24, 10);

                                            PerIndex = (int)Response[34];
                                            PerIndex = (int)PerIndex | (Response[35] << 8);

                                            //string sTime = null;
                                            //sTime = Response[14].ToString("D2") + "." +
                                            //        Response[15].ToString("D2") + "." + "20" +
                                            //        Response[16].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //        Response[17].ToString("D2") + ":" +
                                            //        Response[18].ToString("D2") + ":" +
                                            //        Response[19].ToString("D2") + "." +
                                            //        Response[20].ToString("D2") + "." + "20" +
                                            //        Response[21].ToString("D2") + " " +
                                            //    //Response[9].ToString() + " " +           // Day of Week 
                                            //        Response[22].ToString("D2") + ":" +
                                            //        Response[23].ToString("D2");


                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0; PerIndex = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetPersonIDNew(string TargetIP, int TargetPort, int TargetAddress, int PerIndex, out byte[] PerPacket, out UInt64 PerID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                PerID = 0;
                PerPacket = null;


                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				///Packet Length
                    stream[1] = (byte)DataStruct.PCToReader;    	        // Prefix 
                    stream[2] = (byte)TargetAddress;            // Device Address  
                    stream[3] = (byte)(stream.Length - 4);      		// Length
                    stream[4] = (byte)Commands.GetPerson;            // Command
                    stream[5] = 0;     				                    // SubCommand 
                    //stream[6] = (byte)(PerIndex & 0x0f);
                    //stream[7] = (byte)((PerIndex >> 4) & 0x0f);
                    //stream[8] = (byte)((PerIndex >> 8) & 0x0f);
                    //stream[9] = (byte)((PerIndex >> 12) & 0x0f);
                    //stream[10] = (byte)((PerIndex >> 16) & 0x0f);

                    stream[6] = (byte)((PerIndex >> 0) & 0xff);
                    stream[7] = (byte)((PerIndex >> 8) & 0xff);
                    stream[8] = (byte)((PerIndex >> 16) & 0xff);
                    stream[9] = (byte)((PerIndex >> 24) & 0xff);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];

                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])//true
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            PerID = (UInt64)(Response[13]);
                                            PerID = (UInt64)((PerID << 8) | Response[12]);
                                            PerID = (UInt64)((PerID << 8) | Response[11]);
                                            PerID = (UInt64)((PerID << 8) | Response[10]);
                                            PerID = (UInt64)((PerID << 8) | Response[9]);
                                            PerID = (UInt64)((PerID << 8) | Response[8]);
                                            PerID = (UInt64)((PerID << 8) | Response[7]);
                                            PerID = (UInt64)((PerID << 8) | Response[6]);

                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetPersonCardID(string TargetIP, int TargetPort, int TargetAddress, int Index, out byte[] PerPacket, out UInt64 PerID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                PerID = 0;
                PerPacket = null;


                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				///Packet Length
                    stream[1] = (byte)DataStruct.PCToReader;    	        // Prefix 
                    stream[2] = (byte)TargetAddress;            // Device Address  
                    stream[3] = (byte)(stream.Length - 4);      		// Length
                    stream[4] = (byte)Commands.GetPerson;            // Command
                    stream[5] = 0;     				                    // SubCommand 
                    stream[6] = (byte)((Index >> 0) & 0xFF);
                    stream[7] = (byte)((Index >> 8) & 0xFF);
                    stream[8] = (byte)((Index >> 16) & 0xFF);
                    stream[9] = (byte)((Index >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];

                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])//true
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            PerID = (UInt64)(Response[13]);
                                            PerID = (UInt64)((PerID << 8) | Response[12]);
                                            PerID = (UInt64)((PerID << 8) | Response[11]);
                                            PerID = (UInt64)((PerID << 8) | Response[10]);
                                            PerID = (UInt64)((PerID << 8) | Response[9]);
                                            PerID = (UInt64)((PerID << 8) | Response[8]);
                                            PerID = (UInt64)((PerID << 8) | Response[7]);
                                            PerID = (UInt64)((PerID << 8) | Response[6]);

                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetPersonCardID(string TargetIP, int TargetPort, int TargetAddress, int Index, out UInt64 PerID, out PersonState State, int msTimeOut, Converter cnv)
        {
            int b = 0;
            PerID = 0; State = PersonState.Active;

            try
            {
            StartAgain:

                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				///Packet Length
                    stream[1] = (byte)DataStruct.PCToReader;    	        // Prefix 
                    stream[2] = (byte)TargetAddress;            // Device Address  
                    stream[3] = (byte)(stream.Length - 4);      		// Length
                    stream[4] = (byte)Commands.GetPerson;            // Command
                    stream[5] = 0;     				                    // SubCommand 
                    stream[6] = (byte)((Index >> 0) & 0xFF);
                    stream[7] = (byte)((Index >> 8) & 0xFF);
                    stream[8] = (byte)((Index >> 16) & 0xFF);
                    stream[9] = (byte)((Index >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            if (Response.Length > 12)
                            {

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])//true
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            if (Response.Length == 15)
                                            {
                                                PerID = (UInt64)(Response[12]);
                                                PerID = (UInt64)((PerID << 8) | Response[11]);
                                                PerID = (UInt64)((PerID << 8) | Response[10]);
                                                PerID = (UInt64)((PerID << 8) | Response[9]);
                                                PerID = (UInt64)((PerID << 8) | Response[8]);
                                                PerID = (UInt64)((PerID << 8) | Response[7]);
                                                PerID = (UInt64)((PerID << 8) | Response[6]);

                                                State = (PersonState)Response[13];
                                            }
                                            else
                                            {
                                                PerID = (UInt64)(Response[13]);
                                                PerID = (UInt64)((PerID << 8) | Response[12]);
                                                PerID = (UInt64)((PerID << 8) | Response[11]);
                                                PerID = (UInt64)((PerID << 8) | Response[10]);
                                                PerID = (UInt64)((PerID << 8) | Response[9]);
                                                PerID = (UInt64)((PerID << 8) | Response[8]);
                                                PerID = (UInt64)((PerID << 8) | Response[7]);
                                                PerID = (UInt64)((PerID << 8) | Response[6]);
                                            }

                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetPersonID(string TargetIP, int TargetPort, int TargetAddress, int PerIndex, out byte[] PerPacket, out UInt32 PerID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                PerID = 0;
                PerPacket = null;


                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[12];
                    stream[0] = (byte)stream.Length;     				///Packet Length
                    stream[1] = (byte)DataStruct.PCToReader;    	        // Prefix 
                    stream[2] = (byte)TargetAddress;            // Device Address  
                    stream[3] = (byte)(stream.Length - 4);      		// Length
                    stream[4] = (byte)Commands.GetPerson;            // Command
                    stream[5] = 0;     				                    // SubCommand 
                    stream[6] = (byte)(PerIndex & 0x0f);
                    stream[7] = (byte)((PerIndex >> 4) & 0x0f);
                    stream[8] = (byte)((PerIndex >> 8) & 0x0f);
                    stream[9] = (byte)((PerIndex >> 12) & 0x0f);
                    stream[10] = (byte)((PerIndex >> 16) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];

                            if (Response.Length > 12)
                            {
                                Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                                byte crc = 0;
                                for (int i = 0; i < length - 1; i++)
                                {
                                    crc ^= Response[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == Response[length - 1])//true
                                    if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                        if (Response[4] == (byte)(stream[4]) + 1)
                                        {
                                            if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                            {
                                                client.Close();
                                                return ReturnValues.PersonNotFound;
                                            }

                                            PerID = (UInt32)(Response[13] & 0x0f);
                                            PerID = (UInt32)((PerID << 4) | (Response[12] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[11] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[10] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[9] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[8] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[7] & 0x0f));
                                            PerID = (UInt32)((PerID << 4) | (Response[6] & 0x0f));
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else { client.Close(); return ReturnValues.InvalidResponse; }
                                    else { client.Close(); return ReturnValues.InvalidDevice; }
                                else { client.Close(); return ReturnValues.PacketError; }
                            }
                            else { client.Close(); return ReturnValues.PersonIndexOvf; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetBlackPersonID(string TargetIP, int TargetPort, int TargetAddress, int PerIndex, out byte[] PerPacket, out UInt32 PerID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                PerID = 0;
                PerPacket = null;


                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetBlackPerson;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)(PerIndex & 0x0f);
                    stream[7] = (byte)((PerIndex >> 4) & 0x0f);
                    stream[8] = (byte)((PerIndex >> 8) & 0x0f);
                    stream[9] = (byte)((PerIndex >> 12) & 0x0f);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];
                            Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }

                                        PerID = (UInt32)(Response[13] & 0x0f);
                                        PerID = (UInt32)((PerID << 4) | (Response[12] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[11] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[10] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[9] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[8] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[7] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[6] & 0x0f));
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetBlackList(string TargetIP, int TargetPort, int TargetAddress, int Index, out byte[] PerPacket, out UInt32 PerID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                PerID = 0;
                PerPacket = null;


                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetBlackPerson;            /* Command */
                    stream[5] = 0;     				                    /* SubCommand */
                    stream[6] = (byte)((Index >> 0) & 0xFF);
                    stream[7] = (byte)((Index >> 8) & 0xFF);
                    stream[8] = (byte)((Index >> 16) & 0xFF);
                    stream[9] = (byte)((Index >> 24) & 0xFF);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte lrc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                lrc ^= Response[i];
                            }
                            lrc = (byte)(255 - lrc);
                            PerPacket = new byte[4];
                            Array.Copy(Response, 6, PerPacket, 0, PerPacket.Length);

                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        if ((Response[5] == (byte)ReturnValues.LogNotFound) & (length < 10))
                                        {
                                            client.Close();
                                            return ReturnValues.PersonNotFound;
                                        }

                                        PerID = (UInt32)(Response[13] & 0x0f);
                                        PerID = (UInt32)((PerID << 4) | (Response[12] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[11] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[10] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[9] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[8] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[7] & 0x0f));
                                        PerID = (UInt32)((PerID << 4) | (Response[6] & 0x0f));
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                PerPacket = null;
                PerID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetLastID(string TargetIP, int TargetPort, int TargetAddress, out UInt32 LogID, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                LogID = 0;
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     		/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      /* Length  */
                    stream[4] = (byte)Commands.GetLstID;        /* Command */
                    stream[5] = 0;     				            /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        LogID = (UInt32)(Response[6] & 0x0f);
                                        LogID = (UInt32)((LogID << 4) | (Response[7] & 0x0f));
                                        LogID = (UInt32)((LogID << 8) | (Response[8] & 0x0f));
                                        LogID = (UInt32)((LogID << 12) | (Response[9] & 0x0f));
                                        LogID = (UInt32)((LogID << 16) | (Response[10] & 0x0f));
                                        LogID = (UInt32)((LogID << 20) | (Response[11] & 0x0f));
                                        LogID = (UInt32)((LogID << 24) | (Response[12] & 0x0f));
                                        LogID = (UInt32)((LogID << 28) | (Response[13] & 0x0f));

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }


                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogID = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ListenOnlineRequest(TcpClient client, int Addrss, out UInt64 ID, out int Address, AccessType Accss, BuzzerState Buzzer, byte GrpDoor0, byte GrpDoor1, int RelayTime, out int OfflineLogCount, int msTimeOut, Converter cnv, out byte[] temp)
        {
            int b = 0;
            ID = 0;
            Address = 0;
            temp = null;
            OfflineLogCount = 0;
            ReturnValues Result;
            try
            {
                //Deneme için ekledim //clientt diede değişti
                // StartAgain:
                // TcpClient client = new TcpClient();
                // ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);

                if (TibboDataMode(client, 100, cnv) != ReturnValues.Successful)
                    return ReturnValues.NoAnswerFromCnv;
                while (true)
                {
                    ReturnValues rv = ReturnValues.NoAnswer;
                    int Length;

                    if (client.Connected == true)
                    {
                        rv = GetOnlineDataStream(out temp, out Length, client, msTimeOut);
                        if (rv == ReturnValues.Successful)
                        {
                            if (temp != null)
                            {
                                byte crc = 0;
                                for (int i = 0; i < temp[0] - 1; i++)
                                {
                                    crc ^= temp[i];
                                }
                                crc = (byte)(255 - crc);
                                if (crc == temp[temp[0] - 1])
                                {
                                    byte length = temp[0];
                                    if ((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID) && length > 15)
                                    {
                                        ID = temp[6];
                                        ID = (ID << 8) | (temp[7]);
                                        ID = (ID << 8) | (temp[8]);
                                        ID = (ID << 8) | (temp[9]);
                                        ID = (ID << 8) | (temp[10]);
                                        ID = (ID << 8) | (temp[11]);
                                        ID = (ID << 8) | (temp[12]);
                                        Address = temp[2];

                                        OfflineLogCount = temp[16];
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[15]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[14]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[13]);

                                        Result = Access(client, Addrss, Accss, RelayTime, Buzzer, GrpDoor0, GrpDoor1, msTimeOut, cnv);
                                        if (Result == ConnectionManager.ReturnValues.Successful)
                                        {
                                            return ReturnValues.Successful;
                                        }
                                        else
                                            return Result;
                                        //return ReturnValues.Successful;
                                        //client.Close();
                                    }
                                    else if (((temp[4] == (byte)Commands.RequestPending) & (temp[5] == (byte)Commands.RequestID)) && length < 16)
                                    {
                                        ID = temp[6];
                                        ID = (ID << 8) | (temp[7]);
                                        ID = (ID << 8) | (temp[8]);
                                        ID = (ID << 8) | (temp[9]);
                                        // ID = (ID << 8) | (temp[10]);
                                        // ID = (ID << 8) | (temp[11]);
                                        // ID = (ID << 8) | (temp[12]);
                                        Address = temp[2];

                                        OfflineLogCount = temp[13];
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[12]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[11]);
                                        OfflineLogCount = (OfflineLogCount << 8) | (temp[10]);

                                        Result = Access(client, Addrss, Accss, RelayTime, Buzzer, GrpDoor0, GrpDoor1, msTimeOut, cnv);
                                        if (Result == ConnectionManager.ReturnValues.Successful)
                                        {
                                            return ReturnValues.Successful;
                                        }
                                        else
                                            return Result;
                                    }

                                    if ((temp[4] - 1 == (byte)Commands.AcsAccept) | (temp[4] - 1 == (byte)Commands.AcsCmd) | (temp[4] - 1 == (byte)Commands.AcsDeny))
                                    {
                                        crc = 0;
                                    }
                                }
                                else
                                {
                                    return ReturnValues.PacketError;
                                }
                            }
                            else
                            {
                                return ReturnValues.PacketError;
                            }
                        }
                        else
                        {
                            return rv;
                        }
                    }
                    else
                    {
                        client.Close();
                        // b++;
                        // if ((b < 10) && client.Connected != true) goto StartAgain;
                        return rv;
                    }
                }
            }
            catch (Exception)
            {
                ID = 0;
                Address = 0;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues Access(string TargetIP, int TargetPort, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, byte GrpDoors0, byte GrpDoors1, int msTimeOut, Converter cnv)
        {
            int b = 0;


            try
            {
            StartAgain:

                TcpClient client = new TcpClient();

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[11];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    switch (Access)
                    {
                        case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                        case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                        case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                        case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                        default: break;
                    }
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)RelayTime;
                    stream[7] = GrpDoors0;
                    stream[8] = GrpDoors1;
                    stream[9] = (byte)Buzzer;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                //client.Close();
                return ReturnValues.Failed;
            }

        }

        public ReturnValues SendAccess(string TargetIP, int TargetPort, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {
            int b = 0;


            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[9];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    switch (Access)
                    {
                        case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                        case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                        case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                        case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                        default: break;
                    }
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)RelayTime;
                    stream[7] = (byte)Buzzer;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful || stream[6] == 255)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                //client.Close();
                return ReturnValues.Failed;
            }

        }

        public ReturnValues SendToStartOfflineLogProcess(string TargetIP, int TargetPort, int TargetAddress, LogsProcess state, int msTimeOut, Converter cnv)
        {
            int b = 0;


            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.OfflineLog;
                    stream[5] = 0;     				                   /* SubCommand */
                    stream[6] = (byte)state;
                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful || stream[6] == 255)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                //client.Close();
                return ReturnValues.Failed;
            }

        }

        public byte[] ReadAccessData(int type, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, LogsProcess state)
        {
            byte[] stream;
            stream = new byte[9];
            stream[0] = (byte)stream.Length;     				/*Packet Length*/
            stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
            stream[2] = (byte)TargetAddress;            /* Device Address  */
            stream[3] = (byte)(stream.Length - 4);      		/* Length  */

            if (type == 0)
            {
                switch (Access)
                {
                    case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                    case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                    case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                    case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                    default: break;
                }
                stream[5] = 0;     				                   /* SubCommand */

                stream[6] = (byte)RelayTime;
                stream[7] = (byte)Buzzer;
            }
            else if (type == 3)
            {
                stream[4] = (byte)Commands.OfflineLog;
                stream[5] = 0;     				                   /* SubCommand */
                stream[6] = (byte)state;
                stream[7] = (byte)0;
            }

            stream[stream.Length - 1] = 0;
            for (int i = 0; i < stream.Length - 1; i++)
            {
                stream[stream.Length - 1] ^= stream[i];
            }
            stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

            byte[] stream1 = new byte[11]; stream1[0] = 0x55; stream1[1] = 0x55;
            Array.Copy(stream, 0, stream1, 2, stream.Length);
            return stream1;
        }

        public ReturnValues Access(string TargetIP, int TargetPort, int TargetAddress, TcpClient client, AccessType Access, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {
            try
            {

                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[9];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    switch (Access)
                    {
                        case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                        case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                        case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                        case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                        default: break;
                    }
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)RelayTime;
                    stream[7] = (byte)Buzzer;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        return ReturnValues.Successful;
                                    }
                                    else { return ReturnValues.InvalidResponse; }
                                else { return ReturnValues.InvalidDevice; }
                            else { return ReturnValues.PacketError; }
                        }
                        else { return Result1; }
                    }
                    return Result1;
                }
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                //client.Close();
                return ReturnValues.Failed;
            }

        }

        public ReturnValues Access(string TargetIP, int TargetPort, int TargetAddress, AccessType Access, string PersonName, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {

            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();
            int a = 0;
            try
            {
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] ChgPerName = MakeStringCompatible(PersonName);
                for (a = ChgPerName.Length; a < 10; a++) ;
                byte[] ChgPerName1 = new byte[a - ChgPerName.Length];
                for (int i = 0; i < a - ChgPerName.Length; i++) ChgPerName1[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[19];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    switch (Access)
                    {
                        case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                        case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                        case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                        case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                        default: break;
                    }
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)RelayTime;
                    stream[7] = (byte)Buzzer;

                    Array.Copy(ChgPerName, 0, stream, 8, ChgPerName.Length);
                    Array.Copy(ChgPerName1, 0, stream, ChgPerName.Length + 8, a - ChgPerName.Length);


                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception ex)
            {
                client.Close();
                return ReturnValues.Failed;
            }

        }

        public ReturnValues Access(TcpClient client, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] stream;
                stream = new byte[9];
                stream[0] = (byte)stream.Length;     		/* Packet Length    */
                stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  			*/
                stream[2] = (byte)TargetAddress;            /* Device Address  	*/
                stream[3] = (byte)(stream.Length - 4);      /* Length  			*/
                switch (Access)
                {
                    case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                    case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                    case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                    case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                    default: break;
                }
                stream[5] = 0;     				                   /* SubCommand */

                stream[6] = (byte)RelayTime;
                stream[7] = (byte)Buzzer;

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                byte[] Response; int length = 0;

                for (int j = 0; j < 6; j++)
                {
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream(out Response, out length, client, msTimeOut / 6) == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    // if (Response[4] == (byte)(stream[4]) + 1)
                                    // {
                                    return ReturnValues.Successful;
                                    // }
                                }
                            }
                        }
                    }

                    else return Result1;
                }
                return ReturnValues.NoAnswer;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues Access(TcpClient client, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, byte GrpDoor0, byte GrpDoor1, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
                byte[] stream;
                stream = new byte[11];
                stream[0] = (byte)stream.Length;     		/* Packet Length    */
                stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  			*/
                stream[2] = (byte)TargetAddress;            /* Device Address  	*/
                stream[3] = (byte)(stream.Length - 4);      /* Length  			*/
                switch (Access)
                {
                    case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                    case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                    case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                    case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                    default: break;
                }
                stream[5] = 0;
                /* SubCommand */
                stream[6] = (byte)RelayTime;
                stream[7] = GrpDoor0;
                stream[8] = GrpDoor1;
                stream[9] = (byte)Buzzer;

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                byte[] Response; int length = 0;

                for (int j = 0; j < 6; j++)
                {
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream(out Response, out length, client, msTimeOut / 6) == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    // if (Response[4] == (byte)(stream[4]) + 1)
                                    // {
                                    return ReturnValues.Successful;
                                    // }
                                }
                            }
                        }
                    }

                    else return Result1;
                }
                return ReturnValues.NoAnswer;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }
        public ReturnValues Access(TcpClient client, int TargetAddress, AccessType Access, string PersonName, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {

            try
            {
                byte[] stream;
                stream = new byte[19];
                stream[0] = (byte)stream.Length;     		/* Packet Length    */
                stream[1] = (byte)DataStruct.PCToReader;    /* Prefix  			*/
                stream[2] = (byte)TargetAddress;            /* Device Address  	*/
                stream[3] = (byte)(stream.Length - 4);      /* Length  			*/
                switch (Access)
                {
                    case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                    case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                    case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                    case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                    default: break;
                }
                stream[5] = 0;     				                   /* SubCommand */

                stream[6] = (byte)RelayTime;
                stream[7] = (byte)Buzzer;

                byte[] name = MakeStringCompatible(PersonName);
                for (int i = 0; i < PerNameSize; i++)
                {
                    stream[8 + i] = (byte)name[i];
                }

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                byte[] Response; int length = 0;

                for (int j = 0; j < 6; j++)
                {
                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream(out Response, out length, client, msTimeOut / 6) == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                {
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        return ReturnValues.Successful;
                                    }
                                }
                            }
                        }
                    }
                    else return Result1;
                }
                return ReturnValues.NoAnswer;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        public Byte SmartRelayAccess(string TargetIP, int TargetPort, int TargetAddress, AccessType Access, int RelayTime, BuzzerState Buzzer, int msTimeOut, Converter cnv)
        {
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return 0;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[9];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = 34;// (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    /* switch (Access)
                     {
                         case AccessType.Accept: stream[4] = (byte)Commands.AcsAccept; break;
                         case AccessType.Deny: stream[4] = (byte)Commands.AcsDeny; break;
                         case AccessType.Lock: stream[4] = (byte)Commands.AcsLock; break;
                         case AccessType.Wait: stream[4] = (byte)Commands.AcsWait; break;
                         default: break;
                     }*/
                    stream[4] = 86;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = 10;// (byte)RelayTime;
                    stream[7] = (byte)Buzzer;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return Response[2];
                                        //return ReturnValues.Succesfull;
                                    }
                                    else { client.Close(); return 0; }
                                else { client.Close(); return 0; }
                            else { client.Close(); return 0; }
                        }
                        else { client.Close(); return 0; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return 0;
                }
                client.Close();
                return 0;
            }
            catch (Exception ex)
            {
                client.Close();
                return 0;
            }

        }

        public ReturnValues SaveWinSumTime(string TargetIP, int TargetPort, int TargetAddress, DateTime Time, int hour, ClockDir dir, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 7 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_WinterTime;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)(Time.Year - 2000);
                    stream[7] = (byte)Time.Month;
                    stream[8] = (byte)Time.Day;
                    stream[9] = (byte)Time.Hour;
                    stream[10] = (byte)Time.Minute;
                    stream[11] = (byte)Time.Second;
                    stream[12] = (byte)((hour & 0x7f) | ((int)dir << 7));

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1) && Response[5] == 0)
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else if (Response[5] == 1)
                                {
                                    client.Close();
                                    return ReturnValues.Failed;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SaveStaffGroup(string TargetIP, int TargetPort, int TargetAddress, byte StaffGroup, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_AddStaffGroup;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = StaffGroup;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetStaffGroup(string TargetIP, int TargetPort, int TargetAddress, out byte[] StaffGroups, int msTimeOut, Converter cnv)
        {
            int b = 0;
            StaffGroups = null;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_GetStaffGroup;
                    stream[5] = 0;     				                   /* SubCommand */
                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    StaffGroups = new byte[length - 7];
                                    Array.Copy(Response, 6, StaffGroups, 0, length - 7);
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseStaffGroup(string TargetIP, int TargetPort, int TargetAddress, byte StaffGroup, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_EraseStaffGroup;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = StaffGroup;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues EraseAllStaffGroup(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_EraseAllStaffGroup;
                    stream[5] = 0;     				                   /* SubCommand */
                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SaveTimeToStaffGroup(string TargetIP, int TargetPort, int TargetAddress, byte StaffGroup, byte DayOfWeek, int BeginTime, int FinishTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1 + 1 + 2 + 2 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_AddTimeToStaff;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = StaffGroup;
                    stream[7] = DayOfWeek;
                    stream[8] = (byte)(BeginTime >> 0);
                    stream[9] = (byte)(BeginTime >> 8);
                    stream[10] = (byte)(FinishTime >> 0);
                    stream[11] = (byte)(FinishTime >> 8);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetTimeFromStaffGroup(string TargetIP, int TargetPort, int TargetAddress, byte StaffGroup, byte DayOfWeek, out int[] BeginTime, out int[] FinishTime, int msTimeOut, Converter cnv)
        {
            int b = 0;
            BeginTime = FinishTime = null;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1 + 1 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.Cmd_GetTimeFromStaff;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = StaffGroup;
                    stream[7] = DayOfWeek;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    BeginTime = new int[((length - 7) / 4)];
                                    FinishTime = new int[((length - 7) / 4)];
                                    int len = ((length - 7) / 4);
                                    for (int i = 0; i < len; i++)
                                    {
                                        BeginTime[i] = (Response[(i * 4) + 1 + 6] * 256) + Response[(i * 4) + 0 + 6];
                                        FinishTime[i] = (Response[(i * 4) + 3 + 6] * 256) + Response[(i * 4) + 2 + 6];
                                    }
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeMfrKeysAndBlocks(string TargetIP, int TargetPort, int TargetAddress, Array Keys, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    int CfgLength = 19;
                    stream = new byte[CfgLength + 7];
                    stream[0] = (byte)stream.Length;     				/* Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	    /* Prefix  */
                    stream[2] = (byte)TargetAddress;                    /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ChgMfrKeys;
                    stream[5] = 0;     				                    /* SubCommand */

                    Array.Copy(Keys, 0, stream, 6, CfgLength);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeSmartRelayConfigParameters(string TargetIP, int TargetPort, int TargetAddress, byte[] cfg, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
                StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[5 + 7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ChgConfig;
                    stream[5] = 0;     				                   /* SubCommand */

                    Array.Copy(cfg, 0, stream, 6, cfg.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    client.Close();
                                    return ReturnValues.Successful;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeConfigParameters(string TargetIP, int TargetPort, int TargetAddress, byte[] cfg, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] IPAddrress = MakeStringCompatible(TargetIP);
                for (a = IPAddrress.Length; a < 15; a++) ;
                byte[] IPCnt = new byte[a - IPAddrress.Length];
                for (int i = 0; i < a - IPAddrress.Length; i++) IPCnt[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    int CfgLength = 17;
                    stream = new byte[CfgLength + 7 + 20];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ChgConfig;
                    stream[5] = 0;     				                   /* SubCommand */

                    Array.Copy(cfg, 0, stream, 6, 21);
                    //stream[26] = Convert.ToByte(ChgYear % 2000);
                    Array.Copy(IPAddrress, 0, stream, 27, IPAddrress.Length);
                    Array.Copy(IPCnt, 0, stream, 27 + IPAddrress.Length, a - IPAddrress.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                            {
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) && (Response[4] == (byte)(stream[4]) + 1))
                                {
                                    if (Response[5] == 0)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else
                                    {
                                        client.Close();
                                        return (ReturnValues)Response[5];
                                    }
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public ReturnValues ChangeConfigParameters(string TargetIP, int TargetPort, int TargetAddress, byte[] cfg, string IpAddress, int msTimeOut, Converter cnv)
        {
            int a = 0;
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                int ChgYear = DateTime.Now.Year;
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                byte[] IPAddrress = MakeStringCompatible(IpAddress);
                for (a = IPAddrress.Length; a < 15; a++) ;
                byte[] IPCnt = new byte[a - IPAddrress.Length];
                for (int i = 0; i < a - IPAddrress.Length; i++) IPCnt[i] = (byte)32;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    int CfgLength = 17;
                    stream = new byte[CfgLength + 7 + 20];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ChgConfig;
                    stream[5] = 0;     				                   /* SubCommand */

                    Array.Copy(cfg, 0, stream, 6, 20);
                    //stream[26] = Convert.ToByte(ChgYear % 2000);
                    Array.Copy(IPAddrress, 0, stream, 26, IPAddrress.Length);
                    Array.Copy(IPCnt, 0, stream, 26 + IPAddrress.Length, a - IPAddrress.Length);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }

            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues SaveLogoImage(string TargetIP, int TargetPort, int TargetAddress, byte Index, byte[] Image, int msTimeOut, Converter cnv)
        {

            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[40];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.ChgLogo;
                    stream[5] = 0;

                    stream[6] = Index;

                    if (Image.Length < 32)
                    {
                        return ReturnValues.MessageLengthIsTooBig;
                    }
                    Array.Copy(Image, 0, stream, 7, 32);

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues RefleshScreen(string TargetIP, int TargetPort, int TargetAddress, int msTimeOut, Converter cnv)
        {

            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.RefleshLcd;
                    stream[5] = 0;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues ChangeBaudRate(string TargetIP, int TargetPort, int TargetAddress, BaudRate baud, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgBaudRate;         /* Command */
                    stream[5] = 0;     				/* SubCommand */

                    switch (baud)
                    {
                        case BaudRate._115200: stream[6] = 0; break;
                        case BaudRate._57600: stream[6] = 1; break;
                        case BaudRate._38400: stream[6] = 2; break;
                        case BaudRate._19200: stream[6] = 3; break;
                        case BaudRate._9600: stream[6] = 4; break;
                    }

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues ConverterControlTac(int msTimeOut, Converter cnv)
        {
            byte[] packet; int length = 0; byte[] stream = new byte[5];

            try
            {
                TcpClient client = new TcpClient();

                if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Successful)
                {
                    byte crc = 0;
                    for (int i = 0; i < length - 1; i++)
                    {
                        crc ^= packet[i];
                    }
                    crc = (byte)(255 - crc);
                    if (crc == packet[length - 1])
                        if (packet[1] == 9)
                        {
                            if (packet[4] == (byte)(stream[4]) + 1)
                            {
                                client.Close();
                                return ReturnValues.Successful;
                            }
                            else { client.Close(); return ReturnValues.InvalidResponse; }
                        }
                        else { client.Close(); return ReturnValues.InvalidDevice; }
                    else { client.Close(); return ReturnValues.PacketError; }
                }
                else { client.Close(); return ReturnValues.NoAnswer; }

                client.Close();
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }


        public ReturnValues ChangeRelayContact(string TargetIP, int TargetPort, int TargetAddress, Contact RelayContact, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgRelayContact;         /* Command */
                    stream[5] = 0;     				/* SubCommand */
                    stream[6] = (byte)RelayContact;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangePassBackState(string TargetIP, int TargetPort, int TargetAddress, PassBackState PassBack, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgPassBack;         /* Command */
                    stream[5] = 0;     				/* SubCommand */
                    stream[6] = (byte)PassBack;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeOnlineInfoLogState(string TargetIP, int TargetPort, int TargetAddress, Logging NoLog, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgLoging;         /* Command */
                    stream[5] = 0;     				/* SubCommand */
                    stream[6] = (byte)NoLog;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeOnlineInfoAsciiState(string TargetIP, int TargetPort, int TargetAddress, AsciiState isAscii, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgisAscii;         /* Command */
                    stream[5] = 0;     				/* SubCommand */
                    stream[6] = (byte)isAscii;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;

                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public bool CommVersion = false;

        private ReturnValues ResponseState(byte result)
        {
            if (CommVersion)
            {
                if (result == 0)
                    return ReturnValues.Successful;
                else
                    return (ReturnValues)result;
            }
            else return ReturnValues.Successful;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ReturnValues GetConfigParameters(string TargetIP, int TargetPort, int TargetAddress, out FsmConfig cfg, int msTimeOut, Converter cnv)
        {
            cfg = new FsmConfig();
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetConfig;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        byte[] CfgParams = new byte[21];
                                        Array.Copy(Response, 6, CfgParams, 0, 21);

                                        cfg.SetFsmConfig(CfgParams);

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetConfigParameters_v5(string TargetIP, int TargetPort, int TargetAddress, out FsmConfig cfg, int msTimeOut, Converter cnv)
        {
            cfg = new FsmConfig();
            int b = 0;
            StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetConfig;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        byte[] CfgParams = new byte[21];
                                        Array.Copy(Response, 6, CfgParams, 0, 21);

                                        cfg.SetFsmConfig_v5(CfgParams);

                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDatabaseParameters(string TargetIP, int TargetPort, int TargetAddress, out int PersonIndex, out int LogIndex, out int BlackListIndex, out int ErasedPerIndex, int msTimeOut, Converter cnv)
        {
            LogIndex = 0;
            PersonIndex = 0;
            BlackListIndex = 0;
            ErasedPerIndex = 0;


            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetIndexData;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        try
                                        {
                                            byte[] Configuration = new byte[length - 7];
                                            Array.Copy(Response, 6, Configuration, 0, length - 7);
                                            byte[] pc = new byte[4];
                                            byte[] lc = new byte[4];
                                            byte[] bc = new byte[4];
                                            byte[] ec = new byte[4];

                                            Array.Copy(Configuration, 0, pc, 0, 4);
                                            Array.Copy(Configuration, 4, lc, 0, 4);
                                            Array.Copy(Configuration, 12, bc, 0, 4);
                                            Array.Copy(Configuration, 16, ec, 0, 4);

                                            LogIndex = lc[3];
                                            LogIndex = (LogIndex << 8) | lc[2];
                                            LogIndex = (LogIndex << 8) | lc[1];
                                            LogIndex = (LogIndex << 8) | lc[0];

                                            PersonIndex = pc[3];
                                            PersonIndex = (PersonIndex << 8) | pc[2];
                                            PersonIndex = (PersonIndex << 8) | pc[1];
                                            PersonIndex = (PersonIndex << 8) | pc[0];

                                            BlackListIndex = bc[3];
                                            BlackListIndex = (BlackListIndex << 8) | bc[2];
                                            BlackListIndex = (BlackListIndex << 8) | bc[1];
                                            BlackListIndex = (BlackListIndex << 8) | bc[0];

                                            ErasedPerIndex = ec[3];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | ec[2];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | ec[1];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | ec[0];

                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        catch (Exception)
                                        {
                                            client.Close();
                                            return ReturnValues.Failed;
                                        }
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogIndex = 0;
                PersonIndex = 0;
                client.Close();
                return ReturnValues.Failed;
            }
        }



        public ReturnValues GetDatabaseParametersTest(string TargetIP, int TargetPort, int TargetAddress, out long PersonIndex, out long LogIndex, out long BlackListIndex, out long ErasedPerIndex, out long EventIndex, out long Spare1Index, out long Spare2Index, int msTimeOut, Converter cnv)
        {
            LogIndex = 0;
            PersonIndex = 0;
            BlackListIndex = 0;
            ErasedPerIndex = 0;
            Spare1Index = 0;
            Spare2Index = 0;
            EventIndex = 0;

            try
            {
                byte[] stream;
                stream = new byte[7];
                stream[0] = (byte)stream.Length;     				/*Packet Length*/
                stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                stream[2] = (byte)TargetAddress;            /* Device Address  */
                stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                stream[4] = (byte)Commands.GetIndexData;
                stream[5] = 0;     				                   /* SubCommand */

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                byte[] Response; byte[] rResponse; int length = 0;

                ReturnValues Result1 = SendReceiveDataStream(TargetIP, TargetPort, stream, out rResponse, out length, msTimeOut, cnv);
                if (Result1 == ReturnValues.Successful)
                {
                    if (VerifyReceiveData(stream[4], rResponse, out Response, out length) == ReturnValues.Successful)
                    {
                        try
                        {
                            byte[] Configuration = new byte[length - 7];
                            Array.Copy(Response, 6, Configuration, 0, length - 7);
                            byte[] data = new byte[4];

                            Array.Copy(Configuration, 0, data, 0, 4);
                            PersonIndex = data[3];
                            PersonIndex = (PersonIndex << 8) | data[2];
                            PersonIndex = (PersonIndex << 8) | data[1];
                            PersonIndex = (PersonIndex << 8) | data[0];

                            Array.Copy(Configuration, 4, data, 0, 4);
                            LogIndex = data[3];
                            LogIndex = (LogIndex << 8) | data[2];
                            LogIndex = (LogIndex << 8) | data[1];
                            LogIndex = (LogIndex << 8) | data[0];

                            Array.Copy(Configuration, 8, data, 0, 4);
                            EventIndex = data[3];
                            EventIndex = (EventIndex << 8) | data[2];
                            EventIndex = (EventIndex << 8) | data[1];
                            EventIndex = (EventIndex << 8) | data[0];

                            Array.Copy(Configuration, 12, data, 0, 4);
                            BlackListIndex = data[3];
                            BlackListIndex = (BlackListIndex << 8) | data[2];
                            BlackListIndex = (BlackListIndex << 8) | data[1];
                            BlackListIndex = (BlackListIndex << 8) | data[0];

                            Array.Copy(Configuration, 16, data, 0, 4);
                            ErasedPerIndex = data[3];
                            ErasedPerIndex = (ErasedPerIndex << 8) | data[2];
                            ErasedPerIndex = (ErasedPerIndex << 8) | data[1];
                            ErasedPerIndex = (ErasedPerIndex << 8) | data[0];

                            if (Configuration.Length > 20)
                            {
                                Array.Copy(Configuration, 20, data, 0, 4);
                                Spare1Index = data[3];
                                Spare1Index = (Spare1Index << 8) | data[2];
                                Spare1Index = (Spare1Index << 8) | data[1];
                                Spare1Index = (Spare1Index << 8) | data[0];

                                Array.Copy(Configuration, 24, data, 0, 4);
                                Spare2Index = data[3];
                                Spare2Index = (Spare2Index << 8) | data[2];
                                Spare2Index = (Spare2Index << 8) | data[1];
                                Spare2Index = (Spare2Index << 8) | data[0];
                            }
                            else
                            {
                                Spare1Index = 0;
                                Spare2Index = 0;
                            }
                            return ReturnValues.Successful;
                        }
                        catch (Exception)
                        {
                            return ReturnValues.Failed;
                        }
                    }
                }
                return Result1;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDatabaseParameters(string TargetIP, int TargetPort, int TargetAddress, out long PersonIndex, out long LogIndex, out long BlackListIndex, out long ErasedPerIndex, out long EventIndex, out long Spare1Index, out long Spare2Index, int msTimeOut, Converter cnv)
        {
            LogIndex = 0;
            PersonIndex = 0;
            BlackListIndex = 0;
            ErasedPerIndex = 0;
            Spare1Index = 0;
            Spare2Index = 0;
            EventIndex = 0;

            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetIndexData;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        try
                                        {
                                            byte[] Configuration = new byte[length - 7];
                                            Array.Copy(Response, 6, Configuration, 0, length - 7);
                                            byte[] data = new byte[4];

                                            Array.Copy(Configuration, 0, data, 0, 4);
                                            PersonIndex = data[3];
                                            PersonIndex = (PersonIndex << 8) | data[2];
                                            PersonIndex = (PersonIndex << 8) | data[1];
                                            PersonIndex = (PersonIndex << 8) | data[0];

                                            Array.Copy(Configuration, 4, data, 0, 4);
                                            LogIndex = data[3];
                                            LogIndex = (LogIndex << 8) | data[2];
                                            LogIndex = (LogIndex << 8) | data[1];
                                            LogIndex = (LogIndex << 8) | data[0];

                                            Array.Copy(Configuration, 8, data, 0, 4);
                                            EventIndex = data[3];
                                            EventIndex = (EventIndex << 8) | data[2];
                                            EventIndex = (EventIndex << 8) | data[1];
                                            EventIndex = (EventIndex << 8) | data[0];

                                            Array.Copy(Configuration, 12, data, 0, 4);
                                            BlackListIndex = data[3];
                                            BlackListIndex = (BlackListIndex << 8) | data[2];
                                            BlackListIndex = (BlackListIndex << 8) | data[1];
                                            BlackListIndex = (BlackListIndex << 8) | data[0];

                                            Array.Copy(Configuration, 16, data, 0, 4);
                                            ErasedPerIndex = data[3];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[2];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[1];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[0];

                                            if (Configuration.Length > 20)
                                            {
                                                Array.Copy(Configuration, 20, data, 0, 4);
                                                Spare1Index = data[3];
                                                Spare1Index = (Spare1Index << 8) | data[2];
                                                Spare1Index = (Spare1Index << 8) | data[1];
                                                Spare1Index = (Spare1Index << 8) | data[0];

                                                Array.Copy(Configuration, 24, data, 0, 4);
                                                Spare2Index = data[3];
                                                Spare2Index = (Spare2Index << 8) | data[2];
                                                Spare2Index = (Spare2Index << 8) | data[1];
                                                Spare2Index = (Spare2Index << 8) | data[0];
                                            }
                                            else
                                            {
                                                Spare1Index = 0;
                                                Spare2Index = 0;
                                            }

                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        catch (Exception)
                                        {
                                            client.Close();
                                            return ReturnValues.Failed;
                                        }
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else
                        {
                            client.Close();
                            b++;
                            if (b < 2) goto StartAgain;
                            return Result1;
                        }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogIndex = 0;
                PersonIndex = 0;
                client.Close();
                return ReturnValues.Failed;
            }
        }

        TcpClient TestClient;
        public ReturnValues GetDatabaseParameters_Test(string TargetIP, int TargetPort, int TargetAddress, out long PersonIndex, out long LogIndex, out long BlackListIndex, out long ErasedPerIndex, out long EventIndex, out long Spare1Index, out long Spare2Index, int msTimeOut, Converter cnv)
        {
            LogIndex = 0;
            PersonIndex = 0;
            BlackListIndex = 0;
            ErasedPerIndex = 0;
            Spare1Index = 0;
            Spare2Index = 0;
            EventIndex = 0;

            int b = 0;

            try
            {
                if (TestClient == null || TestClient.Connected == false)
                {
                    TestClient = new TcpClient();
                    TestClient.Connect(TargetIP, TargetPort);
                    if (!TestClient.Connected)
                        return ReturnValues.DeviceNotFound;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(TestClient, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetIndexData;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, TestClient, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        if (GetDataStream_Test(out Response, out length, TestClient, msTimeOut) == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        try
                                        {
                                            byte[] Configuration = new byte[length - 7];
                                            Array.Copy(Response, 6, Configuration, 0, length - 7);
                                            byte[] data = new byte[4];

                                            Array.Copy(Configuration, 0, data, 0, 4);
                                            PersonIndex = data[3];
                                            PersonIndex = (PersonIndex << 8) | data[2];
                                            PersonIndex = (PersonIndex << 8) | data[1];
                                            PersonIndex = (PersonIndex << 8) | data[0];

                                            Array.Copy(Configuration, 4, data, 0, 4);
                                            LogIndex = data[3];
                                            LogIndex = (LogIndex << 8) | data[2];
                                            LogIndex = (LogIndex << 8) | data[1];
                                            LogIndex = (LogIndex << 8) | data[0];

                                            Array.Copy(Configuration, 8, data, 0, 4);
                                            EventIndex = data[3];
                                            EventIndex = (EventIndex << 8) | data[2];
                                            EventIndex = (EventIndex << 8) | data[1];
                                            EventIndex = (EventIndex << 8) | data[0];

                                            Array.Copy(Configuration, 12, data, 0, 4);
                                            BlackListIndex = data[3];
                                            BlackListIndex = (BlackListIndex << 8) | data[2];
                                            BlackListIndex = (BlackListIndex << 8) | data[1];
                                            BlackListIndex = (BlackListIndex << 8) | data[0];

                                            Array.Copy(Configuration, 16, data, 0, 4);
                                            ErasedPerIndex = data[3];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[2];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[1];
                                            ErasedPerIndex = (ErasedPerIndex << 8) | data[0];

                                            if (Configuration.Length > 20)
                                            {
                                                Array.Copy(Configuration, 20, data, 0, 4);
                                                Spare1Index = data[3];
                                                Spare1Index = (Spare1Index << 8) | data[2];
                                                Spare1Index = (Spare1Index << 8) | data[1];
                                                Spare1Index = (Spare1Index << 8) | data[0];

                                                Array.Copy(Configuration, 24, data, 0, 4);
                                                Spare2Index = data[3];
                                                Spare2Index = (Spare2Index << 8) | data[2];
                                                Spare2Index = (Spare2Index << 8) | data[1];
                                                Spare2Index = (Spare2Index << 8) | data[0];
                                            }
                                            else
                                            {
                                                Spare1Index = 0;
                                                Spare2Index = 0;
                                            }

                                            //TestClient.Close();
                                            return ReturnValues.Successful;
                                        }
                                        catch (Exception)
                                        {
                                            TestClient.Close();
                                            return ReturnValues.Failed;
                                        }
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        TestClient.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { TestClient.Close(); return ReturnValues.InvalidResponse; }
                                else { TestClient.Close(); return ReturnValues.InvalidDevice; }
                            else { TestClient.Close(); return ReturnValues.PacketError; }
                        }
                        else
                        {
                            TestClient.Close();
                            return ReturnValues.NoAnswer;
                        }
                    }
                    TestClient.Close();
                    return Result1;
                }
                TestClient.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                LogIndex = 0;
                PersonIndex = 0;
                TestClient.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SendToStartOfflineLogsProcess(string TargetIP, int TargetPort, int TargetAddress, LogsProcess logsProcess, int msTimeOut, Converter cnv)
        {
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[6 + 1 + 1];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.OfflineLog;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[6] = (byte)logsProcess;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else if (Response[4] == (byte)Commands.Busy + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.DeviceBusy;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else
                        {
                            client.Close();
                            b++;
                            if (b < 2) goto StartAgain;
                            return ReturnValues.NoAnswer;
                        }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }


        public ReturnValues ChangeAuthorizationRule(string TargetIP, int TargetPort, int TargetAddress, Authorization AuthorType, int msTimeOut, Converter cnv)
        {

            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgAuthorization;         /* Command */
                    stream[5] = 0;     				            /* SubCommand */
                    stream[6] = (byte)AuthorType;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return ReturnValues.NoAnswer; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues ChangeDirection(string TargetIP, int TargetPort, int TargetAddress, Direction DirectionType, int msTimeOut, Converter cnv)
        {
            int b = 0;
            try
            {
            StartAgain:
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }

                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;

                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[8];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	/* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = 4;      						/* Length  */
                    stream[4] = (byte)Commands.ChgDirection;         /* Command */
                    stream[5] = 0;     				/* SubCommand */
                    stream[6] = (byte)DirectionType;

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] packet; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out packet, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == (byte)DataStruct.ReaderTOPC) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }
                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;

                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }

        //Result = fsm.ChangeDeviceInfo(cmbIPs.Text, Convert.ToInt32(textPort.Text), Convert.ToInt32(textAddress.Text), fsmInfo.ProductDate, fsmInfo.Tester, fsmInfo.Serial, Convert.ToInt32(textTimeOut.Text), Cnv);
        public ReturnValues ChangeDeviceInfo(string TargetIP, int TargetPort, int TargetAddress, DateTime ProductionTime, string TesterName, string SerialNumber, int msTimeOut, Converter cnv)
        {
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();

            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[16 + 7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.SetDvcInfo;
                    stream[5] = 0;     				                   /* SubCommand */

                    byte[] pt = ASCIIEncoding.ASCII.GetBytes(ProductionTime.Date.ToString());
                    stream[6] = pt[0];
                    stream[7] = pt[1];
                    stream[8] = pt[3];
                    stream[9] = pt[4];
                    stream[10] = pt[8];
                    stream[11] = pt[9];
                    try
                    {
                        while (TesterName.Length <= 5) TesterName += " ";
                        byte[] tester = ASCIIEncoding.ASCII.GetBytes(TesterName);
                        for (int i = 0; i < 5; i++)
                        {
                            stream[12 + i] = tester[i];
                        }

                        while (SerialNumber.Length < 5) SerialNumber = "0" + SerialNumber;
                        byte[] serial = ASCIIEncoding.ASCII.GetBytes(SerialNumber);
                        for (int i = 0; i < 5; i++)
                        {
                            stream[17 + i] = serial[i];
                        }
                        stream[stream.Length - 1] = 0;
                        for (int i = 0; i < stream.Length - 1; i++)
                        {
                            stream[stream.Length - 1] ^= stream[i];
                        }
                    }
                    catch (Exception)
                    {
                        return ReturnValues.StringLengthIsLess;
                    }

                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1])
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDeviceInfo(string TargetIP, int TargetPort, int TargetAddress, out string Manufacturer, out string Device, out string Application, out string PcbVersion, out DateTime ProductionTime, out DateTime TestDate, out string FirmwareVersion, out string TesterName, out string SerialNumber, int msTimeOut, Converter cnv)
        {
            Manufacturer = null;
            Device = null;
            PcbVersion = null;
            Application = null;
            ProductionTime = new DateTime();
            TestDate = new DateTime();
            FirmwareVersion = null;
            TesterName = null;
            SerialNumber = null;
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();


            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetDvcInfo;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStreamFree(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStreamFree(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        int i = 6;
                                        Device = ASCIIEncoding.ASCII.GetString(Response, i, 8);
                                        if (Device == "BARKODES")
                                        {
                                            Manufacturer = ASCIIEncoding.ASCII.GetString(Response, i, 8);
                                            Device = ASCIIEncoding.ASCII.GetString(Response, 8 + i, 10);
                                            Application = ASCIIEncoding.ASCII.GetString(Response, 18 + i, 10);
                                            PcbVersion = ASCIIEncoding.ASCII.GetString(Response, 28 + i, 4);
                                            FirmwareVersion = ASCIIEncoding.ASCII.GetString(Response, 32 + i, 4);
                                            string pt = ASCIIEncoding.ASCII.GetString(Response, 36 + i, 6);
                                            pt = pt.Insert(2, "."); pt = pt.Insert(5, ".20");
                                            ProductionTime = Convert.ToDateTime(pt);
                                            pt = ASCIIEncoding.ASCII.GetString(Response, 42 + i, 6);
                                            pt = pt.Insert(2, "."); pt = pt.Insert(5, ".20");
                                            TestDate = Convert.ToDateTime(pt);
                                            TesterName = ASCIIEncoding.ASCII.GetString(Response, 48 + i, 10);
                                            SerialNumber = GetValue(Response, 58 + i).ToString(); //ASCIIEncoding.ASCII.GetString(Response, 58 + i, 5);
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                        else
                                        {
                                            Device = ASCIIEncoding.ASCII.GetString(Response, i, 10);
                                            Application = ASCIIEncoding.ASCII.GetString(Response, 10 + i, 7);
                                            PcbVersion = ASCIIEncoding.ASCII.GetString(Response, 17 + i, 3);
                                            PcbVersion = PcbVersion.Replace('p', '.');
                                            FirmwareVersion = ASCIIEncoding.ASCII.GetString(Response, 20 + i, 3);
                                            FirmwareVersion = FirmwareVersion.Replace('f', '.');
                                            if (!FirmwareVersion.Contains(".")) FirmwareVersion = FirmwareVersion.Insert(1, ".");
                                            string pt = ASCIIEncoding.ASCII.GetString(Response, 23 + i, 6);
                                            pt = pt.Insert(2, ".");
                                            pt = pt.Insert(5, ".20");
                                            TesterName = ASCIIEncoding.ASCII.GetString(Response, 29 + i, 5);
                                            SerialNumber = ASCIIEncoding.ASCII.GetString(Response, 34 + i, 5);
                                            ProductionTime = Convert.ToDateTime(pt);
                                            client.Close();
                                            return ReturnValues.Successful;
                                        }
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetSmartDeviceInfo(string TargetIP, int TargetPort, int TargetAddress, out byte[] config, out string Device, out string Application, out string PcbVersion, out DateTime ProductionTime, out string FirmwareVersion, out string TesterName, out string SerialNumber, int msTimeOut, Converter cnv)
        {
            config = null;
            Device = null;
            PcbVersion = null;
            Application = null;
            ProductionTime = new DateTime();
            FirmwareVersion = null;
            TesterName = null;
            SerialNumber = null;
            int b = 0;
        StartAgain:
            TcpClient client = new TcpClient();


            try
            {

                ReturnValues Result = PingAndPortTest(TargetIP, TargetPort, client);
                if (Result != ReturnValues.Successful)
                {
                    client.Close();
                    return Result;
                }
                Queue<byte> TotalMessage = new Queue<byte>();
                byte[] stream;
                if (TibboDataMode(client, msTimeOut, cnv) == ReturnValues.Successful)
                {
                    stream = new byte[7];
                    stream[0] = (byte)stream.Length;     				/*Packet Length*/
                    stream[1] = (byte)DataStruct.PCToReader;    	        /* Prefix  */
                    stream[2] = (byte)TargetAddress;            /* Device Address  */
                    stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                    stream[4] = (byte)Commands.GetDvcInfo;
                    stream[5] = 0;     				                   /* SubCommand */

                    stream[stream.Length - 1] = 0;
                    for (int i = 0; i < stream.Length - 1; i++)
                    {
                        stream[stream.Length - 1] ^= stream[i];
                    }
                    stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                    byte[] Response; int length = 0;

                    ReturnValues Result1 = SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (Result1 == ReturnValues.Successful)
                    {
                        Result1 = GetDataStream(out Response, out length, client, msTimeOut);
                        if (Result1 == ReturnValues.Successful)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= Response[i];
                            }
                            crc = (byte)(255 - crc);
                            if (crc == Response[length - 1]/*true*/)
                                if ((Response[1] == (byte)DataStruct.ReaderTOPC) & (Response[2] == (byte)TargetAddress))
                                    if (Response[4] == (byte)(stream[4]) + 1)
                                    {
                                        config = new byte[7];

                                        Array.Copy(Response, 6, config, 0, 7);
                                        int i = 13;
                                        Device = ASCIIEncoding.ASCII.GetString(Response, i, 10);
                                        Application = ASCIIEncoding.ASCII.GetString(Response, 10 + i, 7);
                                        PcbVersion = ASCIIEncoding.ASCII.GetString(Response, 17 + i, 3);
                                        PcbVersion = PcbVersion.Insert(1, ".");
                                        FirmwareVersion = ASCIIEncoding.ASCII.GetString(Response, 20 + i, 3);
                                        FirmwareVersion = FirmwareVersion.Insert(1, ".");
                                        string pt = ASCIIEncoding.ASCII.GetString(Response, 23 + i, 6);
                                        pt = pt.Insert(2, ".");
                                        pt = pt.Insert(5, ".20");
                                        TesterName = ASCIIEncoding.ASCII.GetString(Response, 29 + i, 5);
                                        SerialNumber = ASCIIEncoding.ASCII.GetString(Response, 34 + i, 5);
                                        ProductionTime = Convert.ToDateTime(pt);
                                        client.Close();
                                        return ReturnValues.Successful;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.InvalidDevice; }
                            else { client.Close(); return ReturnValues.PacketError; }
                        }
                        else { client.Close(); return Result1; }
                    }

                    client.Close();
                    b++;
                    if ((b < 10) && Result1 != ReturnValues.Successful) goto StartAgain;
                    return Result1;
                }
                client.Close();
                return ReturnValues.NoAnswerFromCnv;
            }
            catch (Exception)
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }
        #endregion

    }

    #endregion

    #region Taç Tibbo

    public class PhysicalDevices
    {
        #region DataStructer
        /*
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<Data0....DataN>-<CRC.High>-<CRC.Low>
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<SubCommand>-<CRC.High>-<CRC.Low>
         * <PacketLength>-<Prefix>-<Header>-<DataLength>-<Command>-<Value0...ValueN>-<CRC.High>-<CRC.Low>
        */
        public enum DataStruct
        {
            Prefix = 77,
            Header = 65
        }

        #endregion

        #region Commands

        public enum Commands
        {

        }

        #endregion

        #region Returns

        public enum ReturnValues
        {
            Failed,
            Succesfull,
            LineBusy,
            InvalidDevice,
            InvalidResponse,
            DeviceNotFound,
            TimeOut,
            NoAnswer,
            PingError,
            PortError,
            DateTimeError,
            QueryFound,
            QueryNotFound,
            NoAnswerFromTibbo,
            TimeError,
            MessageLengthIsTooBig,
            NoAccessRecord,
            AccessNotOccured,
            AccessOccured,
            EraseOfRecordsError,
            PacketError
        }

        #endregion

        #region ConfigEnums

        public enum Parity
        {
            None = 1,
            Odd,
            Even
        }

        public enum StopBits
        {
            One = 1,
            Two = 2
        }

        public enum FlowControl
        {
            None = 1,
            Hardware = 3
        }


        #endregion

        #region StringConversion

        private byte[] MakeStringCompatible(string strMessage)
        {
            char[] strArray = strMessage.ToCharArray();
            byte[] Buffer = new byte[strMessage.Length];
            for (int i = 0; i < strMessage.Length; i++)
            {
                #region Switch statement
                switch (strArray[i])
                {
                    case ' ': Buffer[i] = 32; break;
                    case '!': Buffer[i] = 33; break;
                    case '"': Buffer[i] = 34; break;
                    case '#': Buffer[i] = 35; break;
                    case '$': Buffer[i] = 36; break;
                    case '%': Buffer[i] = 37; break;
                    case '&': Buffer[i] = 38; break;
                    case '(': Buffer[i] = 40; break;
                    case ')': Buffer[i] = 41; break;
                    case '*': Buffer[i] = 42; break;
                    case '+': Buffer[i] = 43; break;
                    case ',': Buffer[i] = 44; break;
                    case '-': Buffer[i] = 45; break;
                    case '.': Buffer[i] = 46; break;
                    case '/': Buffer[i] = 47; break;
                    case '0': Buffer[i] = 48; break;
                    case '1': Buffer[i] = 49; break;
                    case '2': Buffer[i] = 50; break;
                    case '3': Buffer[i] = 51; break;
                    case '4': Buffer[i] = 52; break;
                    case '5': Buffer[i] = 53; break;
                    case '6': Buffer[i] = 54; break;
                    case '7': Buffer[i] = 55; break;
                    case '8': Buffer[i] = 56; break;
                    case '9': Buffer[i] = 57; break;
                    case ':': Buffer[i] = 58; break;
                    case ';': Buffer[i] = 59; break;
                    case '<': Buffer[i] = 60; break;
                    case '=': Buffer[i] = 61; break;
                    case '>': Buffer[i] = 62; break;
                    case '?': Buffer[i] = 63; break;
                    case '@': Buffer[i] = 64; break;
                    case 'A': Buffer[i] = 65; break;
                    case 'B': Buffer[i] = 66; break;
                    case 'C': Buffer[i] = 67; break;
                    case 'D': Buffer[i] = 68; break;
                    case 'E': Buffer[i] = 69; break;
                    case 'F': Buffer[i] = 70; break;
                    case 'G': Buffer[i] = 71; break;
                    case 'H': Buffer[i] = 72; break;
                    case 'I': Buffer[i] = 73; break;
                    case 'J': Buffer[i] = 74; break;
                    case 'K': Buffer[i] = 75; break;
                    case 'L': Buffer[i] = 76; break;
                    case 'M': Buffer[i] = 77; break;
                    case 'N': Buffer[i] = 78; break;
                    case 'O': Buffer[i] = 79; break;
                    case 'P': Buffer[i] = 80; break;
                    case 'Q': Buffer[i] = 81; break;
                    case 'R': Buffer[i] = 82; break;
                    case 'S': Buffer[i] = 83; break;
                    case 'T': Buffer[i] = 84; break;
                    case 'U': Buffer[i] = 85; break;
                    case 'V': Buffer[i] = 86; break;
                    case 'W': Buffer[i] = 87; break;
                    case 'X': Buffer[i] = 88; break;
                    case 'Y': Buffer[i] = 89; break;
                    case 'Z': Buffer[i] = 90; break;
                    case '[': Buffer[i] = 91; break;
                    case ']': Buffer[i] = 93; break;
                    case '^': Buffer[i] = 94; break;
                    case '_': Buffer[i] = 95; break;
                    case '`': Buffer[i] = 96; break;
                    case 'a': Buffer[i] = 97; break;
                    case 'b': Buffer[i] = 98; break;
                    case 'c': Buffer[i] = 99; break;
                    case 'd': Buffer[i] = 100; break;
                    case 'e': Buffer[i] = 101; break;
                    case 'f': Buffer[i] = 102; break;
                    case 'g': Buffer[i] = 103; break;
                    case 'h': Buffer[i] = 104; break;
                    case 'i': Buffer[i] = 105; break;
                    case 'j': Buffer[i] = 106; break;
                    case 'k': Buffer[i] = 107; break;
                    case 'l': Buffer[i] = 108; break;
                    case 'm': Buffer[i] = 109; break;
                    case 'n': Buffer[i] = 110; break;
                    case 'o': Buffer[i] = 111; break;
                    case 'p': Buffer[i] = 112; break;
                    case 'q': Buffer[i] = 113; break;
                    case 'r': Buffer[i] = 114; break;
                    case 's': Buffer[i] = 115; break;
                    case 't': Buffer[i] = 116; break;
                    case 'u': Buffer[i] = 117; break;
                    case 'v': Buffer[i] = 118; break;
                    case 'w': Buffer[i] = 119; break;
                    case 'x': Buffer[i] = 120; break;
                    case 'y': Buffer[i] = 121; break;
                    case 'z': Buffer[i] = 122; break;
                    case '{': Buffer[i] = 123; break;
                    case '|': Buffer[i] = 124; break;
                    case '}': Buffer[i] = 125; break;
                    case '~': Buffer[i] = 126; break;
                    case '': Buffer[i] = 127; break;
                    case '€': Buffer[i] = 128; break;
                    case '': Buffer[i] = 129; break;
                    case '‚': Buffer[i] = 130; break;
                    case 'ƒ': Buffer[i] = 131; break;
                    case '„': Buffer[i] = 132; break;
                    case '…': Buffer[i] = 133; break;
                    case '†': Buffer[i] = 134; break;
                    case '‡': Buffer[i] = 135; break;
                    case 'ˆ': Buffer[i] = 136; break;
                    case '‰': Buffer[i] = 137; break;
                    case 'Š': Buffer[i] = 138; break;
                    case '‹': Buffer[i] = 139; break;
                    case 'Œ': Buffer[i] = 140; break;
                    case '': Buffer[i] = 141; break;
                    case '': Buffer[i] = 142; break;
                    case '': Buffer[i] = 143; break;
                    case '': Buffer[i] = 144; break;
                    case '‘': Buffer[i] = 145; break;
                    case '’': Buffer[i] = 146; break;
                    case '“': Buffer[i] = 147; break;
                    case '”': Buffer[i] = 148; break;
                    case '•': Buffer[i] = 149; break;
                    case '–': Buffer[i] = 150; break;
                    case '—': Buffer[i] = 151; break;
                    case '˜': Buffer[i] = 152; break;
                    case '™': Buffer[i] = 153; break;
                    case 'š': Buffer[i] = 154; break;
                    case '›': Buffer[i] = 155; break;
                    case 'œ': Buffer[i] = 156; break;
                    case '': Buffer[i] = 157; break;
                    case '': Buffer[i] = 158; break;
                    case 'Ÿ': Buffer[i] = 159; break;
                    case ' ': Buffer[i] = 160; break;
                    case '¡': Buffer[i] = 161; break;
                    case '¢': Buffer[i] = 162; break;
                    case '£': Buffer[i] = 163; break;
                    case '¤': Buffer[i] = 164; break;
                    case '¥': Buffer[i] = 165; break;
                    case '¦': Buffer[i] = 166; break;
                    case '§': Buffer[i] = 167; break;
                    case '¨': Buffer[i] = 168; break;
                    case '©': Buffer[i] = 169; break;
                    case 'ª': Buffer[i] = 170; break;
                    case '«': Buffer[i] = 171; break;
                    case '¬': Buffer[i] = 172; break;
                    case '­': Buffer[i] = 173; break;
                    case '®': Buffer[i] = 174; break;
                    case '¯': Buffer[i] = 175; break;
                    case '°': Buffer[i] = 176; break;
                    case '±': Buffer[i] = 177; break;
                    case '²': Buffer[i] = 178; break;
                    case '³': Buffer[i] = 179; break;
                    case '´': Buffer[i] = 180; break;
                    case 'µ': Buffer[i] = 181; break;
                    case '¶': Buffer[i] = 182; break;
                    case '·': Buffer[i] = 183; break;
                    case '¸': Buffer[i] = 184; break;
                    case '¹': Buffer[i] = 185; break;
                    case 'º': Buffer[i] = 186; break;
                    case '»': Buffer[i] = 187; break;
                    case '¼': Buffer[i] = 188; break;
                    case '½': Buffer[i] = 189; break;
                    case '¾': Buffer[i] = 190; break;
                    case '¿': Buffer[i] = 191; break;
                    case 'À': Buffer[i] = 192; break;
                    case 'Á': Buffer[i] = 193; break;
                    case 'Â': Buffer[i] = 194; break;
                    case 'Ã': Buffer[i] = 195; break;
                    case 'Ä': Buffer[i] = 196; break;
                    case 'Å': Buffer[i] = 197; break;
                    case 'Æ': Buffer[i] = 198; break;
                    case 'Ç': Buffer[i] = 199; break;
                    case 'È': Buffer[i] = 200; break;
                    case 'É': Buffer[i] = 201; break;
                    case 'Ê': Buffer[i] = 202; break;
                    case 'Ë': Buffer[i] = 203; break;
                    case 'Ì': Buffer[i] = 204; break;
                    case 'Í': Buffer[i] = 205; break;
                    case 'Î': Buffer[i] = 206; break;
                    case 'Ï': Buffer[i] = 207; break;
                    case 'Ğ': Buffer[i] = 208; break;
                    case 'Ñ': Buffer[i] = 209; break;
                    case 'Ò': Buffer[i] = 210; break;
                    case 'Ó': Buffer[i] = 211; break;
                    case 'Ô': Buffer[i] = 212; break;
                    case 'Õ': Buffer[i] = 213; break;
                    case 'Ö': Buffer[i] = 214; break;
                    case '×': Buffer[i] = 215; break;
                    case 'Ø': Buffer[i] = 216; break;
                    case 'Ù': Buffer[i] = 217; break;
                    case 'Ú': Buffer[i] = 218; break;
                    case 'Û': Buffer[i] = 219; break;
                    case 'Ü': Buffer[i] = 220; break;
                    case 'İ': Buffer[i] = 221; break;
                    case 'Ş': Buffer[i] = 222; break;
                    case 'ß': Buffer[i] = 223; break;
                    case 'à': Buffer[i] = 224; break;
                    case 'á': Buffer[i] = 225; break;
                    case 'â': Buffer[i] = 226; break;
                    case 'ã': Buffer[i] = 227; break;
                    case 'ä': Buffer[i] = 228; break;
                    case 'å': Buffer[i] = 229; break;
                    case 'æ': Buffer[i] = 230; break;
                    case 'ç': Buffer[i] = 231; break;
                    case 'è': Buffer[i] = 232; break;
                    case 'é': Buffer[i] = 233; break;
                    case 'ê': Buffer[i] = 234; break;
                    case 'ë': Buffer[i] = 235; break;
                    case 'ì': Buffer[i] = 236; break;
                    case 'í': Buffer[i] = 237; break;
                    case 'î': Buffer[i] = 238; break;
                    case 'ï': Buffer[i] = 239; break;
                    case 'ğ': Buffer[i] = 240; break;
                    case 'ñ': Buffer[i] = 241; break;
                    case 'ò': Buffer[i] = 242; break;
                    case 'ó': Buffer[i] = 243; break;
                    case 'ô': Buffer[i] = 244; break;
                    case 'õ': Buffer[i] = 245; break;
                    case 'ö': Buffer[i] = 246; break;
                    case '÷': Buffer[i] = 247; break;
                    case 'ø': Buffer[i] = 248; break;
                    case 'ù': Buffer[i] = 249; break;
                    case 'ú': Buffer[i] = 250; break;
                    case 'û': Buffer[i] = 251; break;
                    case 'ü': Buffer[i] = 252; break;
                    case 'ı': Buffer[i] = 253; break;
                    case 'ş': Buffer[i] = 254; break;
                    case 'ÿ': Buffer[i] = 255; break;
                    default: Buffer[i] = 32; break;
                }
                #endregion
            }
            return Buffer;
        }

        #endregion

        #region UDPprotocol

        public string Mac;
        public string Ip;
        public string Port;
        public string DeviceName;

        #region ConversionMethods

        #region HexToByte

        public byte[] HexToByte(string msg)
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

        #region StringToByte

        public byte[] StringToByte(string msg)
        {
            try
            {
                byte[] rt = System.Text.Encoding.ASCII.GetBytes(msg);
                return rt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region ByteToHex

        public string ByteToHex(byte[] comByte)
        {
            try
            {
                StringBuilder builder = new StringBuilder(comByte.Length * 3);
                foreach (byte data in comByte)
                    builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
                return builder.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

        #endregion

        private enum UDPCommands
        {
            TagCmd = 0xff,
            TagStatus = 0xfe,
            Discover = 0x02,

            ChangeIP = 0x10,
            ChangeMac = 0x12,
            ChangePort = 0x14,
            ChangeName = 0x16,
            ChangeCIP = 0x18,

            ChangeBaud = 0x20,
            ChangeDataBits = 0x22,
            ChangeHandShake = 0x24,
            ChangeStopBit = 0x26,
            ChangeParity = 0x28,

            Update = 0x30,
            ReBoot = 0x32,

            ChangeParameters = 0x34
        }

        private void GetMacAddress(out string[] Macs, out string[] IPs, out string[] GWs, out string[] SMs, out string[] Ports, out string[] BaudRates, out string[] Parities, out string[] Datasizes, out string[] Stopbits, out string[] Flowcontrol, out string[] Names, out int Length)
        {
            Macs = new string[100];
            IPs = new string[100];
            Names = new string[100];
            GWs = new string[100];
            SMs = new string[100];
            Ports = new string[100];
            BaudRates = new string[100];
            Parities = new string[100];
            Datasizes = new string[100];
            Stopbits = new string[100];
            Flowcontrol = new string[100];
            Length = 0;

            byte[] LocatorData = new byte[4];
            LocatorData[0] = (byte)UDPCommands.TagCmd;
            LocatorData[1] = (byte)LocatorData.Length;
            LocatorData[2] = (byte)UDPCommands.Discover;
            LocatorData[3] = (byte)((0 - LocatorData[0] - LocatorData[1] - LocatorData[2]) & 0xff);

            IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, 23);
            UdpClient ucl = new UdpClient(23);

            ucl.Client.ReceiveTimeout = 1000;
            ucl.EnableBroadcast = true;

            ucl.Send(LocatorData, LocatorData.Length, ipend);

            int i = 0;
            //System.Threading.Thread.Sleep(1000);
            try
            {
                while (true)
                {
                    ipend = new IPEndPoint(IPAddress.Broadcast, 23);
                    byte[] bfr = ucl.Receive(ref ipend);
                    if ((byte)bfr[0] == (byte)UDPCommands.TagStatus)
                    {
                        byte[] mac = new byte[6];
                        Array.Copy(bfr, 9, mac, 0, mac.Length);
                        Macs[i] = mac[0].ToString("X2") + ":" + mac[1].ToString("X2") + ":" + mac[2].ToString("X2") + ":" + mac[3].ToString("X2") + ":" + mac[4].ToString("X2") + ":" + mac[5].ToString("X2");
                        if (bfr.Length > 84)
                        {
                            Array.Copy(bfr, 84, mac, 0, 4);
                            GWs[i] = mac[3].ToString() + "." + mac[2].ToString() + "." + mac[1].ToString() + "." + mac[0].ToString();

                            Array.Copy(bfr, 88, mac, 0, 4);
                            SMs[i] = mac[3].ToString() + "." + mac[2].ToString() + "." + mac[1].ToString() + "." + mac[0].ToString();

                            int tmp = bfr[93];
                            tmp = (tmp << 8) | bfr[92];
                            Ports[i] = tmp.ToString();

                            tmp = bfr[96];
                            tmp = (tmp << 8) | bfr[95];
                            tmp = (tmp << 8) | bfr[94];
                            BaudRates[i] = tmp.ToString();

                            Datasizes[i] = bfr[97].ToString();

                            Parities[i] = bfr[98].ToString();

                            Stopbits[i] = bfr[99].ToString();

                            Flowcontrol[i] = bfr[100].ToString();
                        }
                        string resp = ASCIIEncoding.ASCII.GetString(bfr);
                        string[] strArr = resp.Split('\0');
                        for (int tmp = 19; tmp < 83; tmp++)
                        {
                            Names[i] += Convert.ToBoolean(bfr[tmp]) ? Convert.ToChar(bfr[tmp]) : char.Parse(" "); ;
                        }
                        Names[i] = Names[i].Trim();
                        IPs[i] = ipend.Address.ToString();
                        i++;
                    }
                }
                ucl.Close();
            }
            catch (Exception ex)
            {
                Length = i;
                ucl.Close();
            }
        }

        public ReturnValues GetLocalDevices(out string[] Macs, out string[] IPs, out string[] GWs, out string[] SMs, out string[] Ports, out string[] Bauds, out string[] Pars, out string[] Datas, out string[] Stops, out string[] Flows, out string[] Names)
        {
            int Length; string[] _Macs, _IPs, _GWs, _Names, _SMs, _Ports, _Bauds, _Datas, _Pars, _Stops, _Flows;
            GetMacAddress(out _Macs, out _IPs, out _GWs, out _SMs, out _Ports, out _Bauds, out _Pars, out _Datas, out _Stops, out _Flows, out _Names, out Length);
            Macs = new string[Length];
            IPs = new string[Length];
            GWs = new string[Length];
            Names = new string[Length];
            SMs = new string[Length];
            Ports = new string[Length];
            Bauds = new string[Length];
            Pars = new string[Length];
            Datas = new string[Length];
            Stops = new string[Length];
            Flows = new string[Length];

            Array.Copy(_Macs, 0, Macs, 0, Length);
            Array.Copy(_IPs, 0, IPs, 0, Length);
            Array.Copy(_GWs, 0, GWs, 0, Length);
            Array.Copy(_Names, 0, Names, 0, Length);
            Array.Copy(_SMs, 0, SMs, 0, Length);
            Array.Copy(_Ports, 0, Ports, 0, Length);
            Array.Copy(_Bauds, 0, Bauds, 0, Length);
            Array.Copy(_Pars, 0, Pars, 0, Length);
            Array.Copy(_Datas, 0, Datas, 0, Length);
            Array.Copy(_Stops, 0, Stops, 0, Length);
            Array.Copy(_Flows, 0, Flows, 0, Length);

            return ReturnValues.Succesfull;
        }

        private ReturnValues ChangeDeviceMacAddress(string MacAddress, string IPAddr, string PortNumber, string NewMac)
        {
            byte[] LocatorData = new byte[10];
            LocatorData[0] = (byte)UDPCommands.TagCmd;
            LocatorData[1] = (byte)LocatorData.Length;
            LocatorData[2] = (byte)UDPCommands.ChangeMac;
            string[] strmac = NewMac.Split(':');

            byte[] nmac = HexToByte(NewMac);

            char[] cmac = null; byte bmac;
            for (int j = 0; j < nmac.Length; j++)
            {
                LocatorData[j + 3] = nmac[j];
            }
            for (int k = 0; k < LocatorData.Length - 1; k++)
            {
                LocatorData[LocatorData.Length - 1] -= LocatorData[k];
            }

            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(IPAddr), 23);
            UdpClient ucl = new UdpClient(23);
            ucl.Client.ReceiveTimeout = 1000;
            ucl.EnableBroadcast = true;
            ucl.Send(LocatorData, LocatorData.Length, ipend);

            string[] Macs = new string[100];
            string[] IPs = new string[100];
            string[] CIPs = new string[100];
            string[] Names = new string[100];
            int Length = 0;
            int i = 0;
            System.Threading.Thread.Sleep(1000);
            try
            {
                while (true)
                {
                    ipend = new IPEndPoint(0, 0);
                    byte[] bfr = ucl.Receive(ref ipend);
                    if ((byte)bfr[0] == (byte)UDPCommands.TagStatus)
                    {
                        byte[] mac = new byte[6];
                        Array.Copy(bfr, 9, mac, 0, mac.Length);
                        Macs[i] = mac[0].ToString("X2") + ":" + mac[1].ToString("X2") + ":" + mac[2].ToString("X2") + ":" + mac[3].ToString("X2") + ":" + mac[4].ToString("X2") + ":" + mac[5].ToString("X2");
                        string resp = ASCIIEncoding.ASCII.GetString(bfr);
                        string[] strArr = resp.Split('\0');
                        Names[i] = strArr[10];
                        IPs[i] = ipend.Address.ToString();
                        i++;
                    }
                }
                ucl.Close();
                return ReturnValues.Succesfull;
            }
            catch (Exception ex)
            {
                Length = i;
                ucl.Close();
                return ReturnValues.Failed;
            }

        }

        public ReturnValues ChangeDeviceParameters(string Mac, string OldIP, string newIP, string newGateWay, string newSubnetMask, string PortNumber, string newPort, string newBaud, string newDataSize, Parity newParity, StopBits newStopBits, FlowControl newFlowControl, string newName)
        {
            try
            {
                byte[] LocatorData = new byte[4 + 68];
                LocatorData[0] = (byte)UDPCommands.TagCmd;
                LocatorData[1] = (byte)LocatorData.Length;
                LocatorData[2] = (byte)UDPCommands.ChangeParameters;

                /* IP */
                string[] ip = newIP.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    LocatorData[j + 3] = Convert.ToByte(ip[j]);
                }

                /* GateWay  */
                ip = newGateWay.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    LocatorData[j + 7] = Convert.ToByte(ip[j]);
                }

                /*  SubnetMask*/
                ip = newSubnetMask.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    LocatorData[j + 11] = Convert.ToByte(ip[j]);
                }

                /*  PortNumber  */
                LocatorData[15] = Convert.ToByte(PortNumber);

                /*  Port    */
                int port = Convert.ToInt32(newPort);
                LocatorData[16] = (byte)port;
                LocatorData[17] = (byte)((port >> 8) & 0x00FF);

                /* BaudRate */
                int baud = Convert.ToInt32(newBaud);
                LocatorData[18] = (byte)baud;
                LocatorData[19] = (byte)((baud >> 8) & 0x00FF);
                LocatorData[20] = (byte)((baud >> 16) & 0x00FF);

                /*  DataSize    */
                LocatorData[21] = Convert.ToByte(newDataSize);

                /*  Parity      */
                LocatorData[22] = (byte)newParity;

                /*  FlowControl */
                LocatorData[23] = (byte)newFlowControl;

                /*  StopBits    */
                LocatorData[24] = (byte)newStopBits;

                /*  Mac Address */
                string mc = Mac.Replace(":", "");
                byte[] mac = HexToByte(mc);
                for (int j = 0; j < mac.Length; j++)
                {
                    LocatorData[j + 25] = mac[j];
                }
                /*  Name        */
                char[] name = newName.ToCharArray();
                for (int j = 0; j < name.Length; j++)
                {
                    LocatorData[j + 31] = Convert.ToByte(name[j]);
                }

                /*  CRC         */
                for (int k = 0; k < LocatorData.Length - 1; k++)
                {
                    LocatorData[LocatorData.Length - 1] -= LocatorData[k];
                }

                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(OldIP), Convert.ToInt32(23));
                UdpClient ucl = new UdpClient(23);
                ucl.Client.ReceiveTimeout = 1000;
                ucl.EnableBroadcast = true;
                ucl.Send(LocatorData, LocatorData.Length, ipend);
                ucl.Close();
                return ReturnValues.Succesfull;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        #endregion
    }

    #endregion

    #region PyhsicalCommunication

    public class PhysicalTibboDevices
    {
        public string Mac;
        public string Ip;
        public string Port;
        public string Baud;
        public string DevName;
        public string OwnName;
        public string Flag;
        private const string Protocol = "1";    // TCP
        private const string DataLogin = "1";   // Enabled

        public PhysicalTibboDevices()
        {
            Mac = null;
            Ip = null;
            Port = null;
            Baud = null;
            DevName = null;
            OwnName = null;
            Flag = null;
        }

        public enum PhysicalReturns
        {
            Failed,
            Succesfull,
            PingError,
            InvalidPort,
            InvalidMac,
            InvalidIP,
            InvalidDeviceName,
            InvalidOwnerName,
            DeviceNotSelected,
            LogInError,
            LogOutError,
            SelectError,
            ReBootError
        }

        static string[] GetMacAddress()
        {
            UdpClient ucl = new UdpClient();
            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            byte[] echo_cmd = ASCIIEncoding.ASCII.GetBytes(new char[] { 'X' });

            ucl.Client.ReceiveTimeout = 500;

            ucl.Send(echo_cmd, echo_cmd.Length, ipend);

            Queue<string> _macs = new Queue<string>();

            try
            {
                while (true)
                {
                    byte[] bfr = ucl.Receive(ref ipend);
                    string resp = ASCIIEncoding.ASCII.GetString(bfr);
                    if (resp[0] == 'A')
                    {
                        string[] arr = resp.Split('/');
                        _macs.Enqueue(arr[0].Substring(1, arr[0].Length - 1));
                    }
                }
            }
            catch (Exception ex)
            {
                if (_macs == null)
                {
                    return _macs.ToArray();
                }
            }
            return _macs.ToArray();
        }

        static void write_parameter(UdpClient cl, string prm)
        {
            byte[] _prm = ASCIIEncoding.ASCII.GetBytes(prm);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            cl.Send(_prm, _prm.Length, ep);
            if (ASCIIEncoding.ASCII.GetString(new byte[] { cl.Receive(ref ep)[0] }) != "A")
                throw (new Exception(false.ToString()));
        }

        static string read_parameter(UdpClient cl, string prm)
        {
            byte[] _prm = ASCIIEncoding.ASCII.GetBytes(prm);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            cl.Send(_prm, _prm.Length, ep);

            string response = ASCIIEncoding.ASCII.GetString(cl.Receive(ref ep));

            if (response[0] != 'A')
                throw (new Exception(false.ToString()));

            return response.Substring(1, response.Length - 1);
        }

        private PhysicalReturns TibboSelect(UdpClient client, IPEndPoint endpoint, string Device)
        {
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes("W" + Device);
            client.Send(cmd, cmd.Length, endpoint);
            try
            {
                byte[] response = client.Receive(ref endpoint);
                if (ASCIIEncoding.ASCII.GetString(new byte[] { response[0] }) != "A")
                {
                    return PhysicalReturns.DeviceNotSelected;
                }
                return PhysicalReturns.Succesfull;
            }
            catch
            {
                return PhysicalReturns.SelectError;
            }
        }

        private PhysicalReturns TibboLogIn(UdpClient client, IPEndPoint endpoint)
        {
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes(new char[] { 'L' });
            client.Send(cmd, cmd.Length, endpoint);
            try
            {
                byte[] response = client.Receive(ref endpoint);
                if (ASCIIEncoding.ASCII.GetString(new byte[] { response[0] }) != "A")
                {
                    return PhysicalReturns.LogInError;
                }
                return PhysicalReturns.Succesfull;
            }
            catch
            {
                return PhysicalReturns.LogInError;
            }
        }

        private PhysicalReturns TibboLogOut(UdpClient client, IPEndPoint endpoint)
        {
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes(new char[] { 'O' });
            client.Send(cmd, cmd.Length, endpoint);
            try
            {
                byte[] response = client.Receive(ref endpoint);
                if (ASCIIEncoding.ASCII.GetString(new byte[] { response[0] }) != "A")
                {
                    return PhysicalReturns.Succesfull;
                }
                return PhysicalReturns.LogOutError;
            }
            catch
            {
                return PhysicalReturns.LogInError;
            }
        }

        private PhysicalReturns TibboReBoot(UdpClient client, IPEndPoint endpoint)
        {
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes("E");
            client.Send(cmd, cmd.Length, endpoint);

            try
            {
                byte[] __x = client.Receive(ref endpoint);
                return PhysicalReturns.ReBootError;
            }
            catch
            {
                return PhysicalReturns.Succesfull;
            }
        }

        private PhysicalTibboDevices[] GetParameters(PhysicalTibboDevices[] Devices)
        {
            for (int i = 0; i < Devices.Length; i++)
            {
                UdpClient cl = new UdpClient();
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
                cl.Client.ReceiveTimeout = 1000;

                #region SELECT DEVICE
                byte[] select_cmd = ASCIIEncoding.ASCII.GetBytes("W" + Devices[i].Mac);
                cl.Send(select_cmd, select_cmd.Length, ep);

                try
                {
                    byte[] _rp = cl.Receive(ref ep);
                    if (ASCIIEncoding.ASCII.GetString(new byte[] { _rp[0] }) != "A")
                    {
                        Devices[i].Flag = false.ToString();
                        cl.Close();
                        continue;
                    }
                }
                catch
                {
                    Devices[i].Flag = false.ToString();
                    cl.Close();
                    continue;
                }
                #endregion

                #region LOGIN DEVICE
                ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);

                byte[] login_cmd = ASCIIEncoding.ASCII.GetBytes(new char[] { 'L' });
                cl.Send(login_cmd, login_cmd.Length, ep);

                try
                {
                    byte[] _rp = cl.Receive(ref ep);
                    if (ASCIIEncoding.ASCII.GetString(new byte[] { _rp[0] }) != "A")
                    {
                        Devices[i].Flag = false.ToString();
                        cl.Close();
                        continue;
                    }
                }
                catch
                {
                    Devices[i].Flag = false.ToString();
                    cl.Close();
                    continue;
                }
                #endregion

                #region GETTING PARAMETERS
                try
                {
                    Devices[i].Ip = read_parameter(cl, "GIP");
                    Devices[i].Port = read_parameter(cl, "GPN");
                    Devices[i].Baud = read_parameter(cl, "GBR");
                    Devices[i].DevName = read_parameter(cl, "GDN");
                    Devices[i].OwnName = read_parameter(cl, "GON");
                }
                catch
                {
                    Devices[i].Flag = false.ToString();
                    continue;
                }
                #endregion

                #region LOGOUT DEVICE

                ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);

                byte[] logout_cmd = ASCIIEncoding.ASCII.GetBytes(new char[] { 'O' });
                cl.Send(logout_cmd, logout_cmd.Length, ep);


                try
                {
                    byte[] _rp = cl.Receive(ref ep);
                    if (ASCIIEncoding.ASCII.GetString(new byte[] { _rp[0] }) != "A")
                    {
                        Devices[i].Flag = false.ToString();
                        cl.Close();
                        continue;
                    }
                }
                catch
                {
                    Devices[i].Flag = false.ToString();
                    cl.Close();
                    continue;
                }
                #endregion

                Devices[i].Flag = true.ToString();
            }
            return Devices;
        }

        public PhysicalReturns GetLocalDevices(out PhysicalTibboDevices[] Devices)
        {
            string[] macs = GetMacAddress();
            Devices = new PhysicalTibboDevices[macs.Length];
            for (int i = 0; i < macs.Length; i++)
            {
                Devices[i] = new PhysicalTibboDevices();
                Devices[i].Mac = macs[i];
            }
            Devices = GetParameters(Devices);
            return PhysicalReturns.Succesfull;
        }

        public PhysicalReturns ChangeDeviceName(string MacAddress, string newName)
        {
            UdpClient udpclient = new UdpClient();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);

            if (TibboSelect(udpclient, endpoint, MacAddress) == PhysicalReturns.Succesfull)
            {
                if (TibboLogIn(udpclient, endpoint) == PhysicalReturns.Succesfull)
                {
                    string[] sName = new string[2];
                    try
                    {
                        sName = newName.Split('/');
                    }
                    catch (Exception ex)
                    {
                        return PhysicalReturns.InvalidOwnerName;
                    }
                    write_parameter(udpclient, "SDN" + sName[0]);
                    write_parameter(udpclient, "SON" + sName[1]);
                    if (TibboReBoot(udpclient, endpoint) == PhysicalReturns.Succesfull)
                    {
                        udpclient.Close();
                        return PhysicalReturns.Succesfull;
                    }
                }
                udpclient.Close();
                return PhysicalReturns.LogInError;
            }
            udpclient.Close();
            return PhysicalReturns.SelectError;
        }

        public PhysicalReturns ChangeDeviceIP(string MacAddress, string newIP)
        {
            UdpClient udpclient = new UdpClient();

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            udpclient.Client.ReceiveTimeout = 1000;
            if (TibboSelect(udpclient, endpoint, MacAddress) == PhysicalReturns.Succesfull)
                if (TibboLogIn(udpclient, endpoint) == PhysicalReturns.Succesfull)
                {
                    write_parameter(udpclient, "SIP" + newIP);
                    if (TibboReBoot(udpclient, endpoint) == PhysicalReturns.Succesfull)
                    {
                        udpclient.Close();
                        return PhysicalReturns.Succesfull;
                    }
                }
            udpclient.Close();
            return PhysicalReturns.SelectError;
        }

        public PhysicalReturns ChangeDevicePort(string MacAddress, string newPort)
        {
            UdpClient udpclient = new UdpClient();

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            udpclient.Client.ReceiveTimeout = 1000;
            if (TibboSelect(udpclient, endpoint, MacAddress) == PhysicalReturns.Succesfull)
                if (TibboLogIn(udpclient, endpoint) == PhysicalReturns.Succesfull)
                {
                    write_parameter(udpclient, "SPN" + newPort);
                    if (TibboReBoot(udpclient, endpoint) == PhysicalReturns.Succesfull)
                    {
                        udpclient.Close();
                        return PhysicalReturns.Succesfull;
                    }
                }
            udpclient.Close();
            return PhysicalReturns.SelectError;
        }

        public PhysicalReturns ChangeDeviceBaud(string MacAddress, string newBaud)
        {
            UdpClient udpclient = new UdpClient();

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            udpclient.Client.ReceiveTimeout = 1000;
            if (TibboSelect(udpclient, endpoint, MacAddress) == PhysicalReturns.Succesfull)
                if (TibboLogIn(udpclient, endpoint) == PhysicalReturns.Succesfull)
                {
                    write_parameter(udpclient, "SBR" + newBaud);
                    if (TibboReBoot(udpclient, endpoint) == PhysicalReturns.Succesfull)
                    {
                        udpclient.Close();
                        return PhysicalReturns.Succesfull;
                    }
                }
            udpclient.Close();
            return PhysicalReturns.SelectError;
        }

        static void send_commandUDP(UdpClient cl, string prm, bool get_ack)
        {
            byte[] _prm = ASCIIEncoding.ASCII.GetBytes(prm);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 65535);
            cl.Send(_prm, _prm.Length, ep);

            if (get_ack == true)
            {
                if (ASCIIEncoding.ASCII.GetString(new byte[] { cl.Receive(ref ep)[0] }) != "A")
                    throw (new Exception(false.ToString()));
            }
        }

        public PhysicalReturns Buzz(string mac_adr)
        {
            UdpClient cl = new UdpClient();
            cl.Client.ReceiveTimeout = 1000;
            cl.Client.SendTimeout = 1000;

            int _err_cntr = 0;

        LBL_RETRY_BUZZ:

            try
            {
                //Select in broadcast mode
                send_commandUDP(cl, "W" + mac_adr, true);
                //Send buzz command
                send_commandUDP(cl, "B", true);
                //Release in broadcast mode
                send_commandUDP(cl, "W", false);
                return PhysicalReturns.Succesfull;
            }
            catch (Exception ex)
            {
                _err_cntr += 1;

                if (_err_cntr >= 5)
                    return PhysicalReturns.Failed;
                else
                    goto LBL_RETRY_BUZZ;
            }

        }

    }

    #endregion
}
