using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Data.OleDb;
using System.Diagnostics;
using rfMultiLibrary;

namespace BarkodesDeviceVerison
{
    public class ComMng
    {
        private string Version = "v6.5";

        public string GetVersion()
        {
            return Version;
        }

        #region Delegates
        
        public delegate void PosLogReceived(PosLog Log);
        public event PosLogReceived onPosLogReceived;

        public delegate void AcsLogReceived(AcsLog Log);
        public event AcsLogReceived onAcsLogReceived;
        
        public delegate void DeviceFound(Device dvc);
        public event DeviceFound onDeviceFound;

        #endregion

        Device dvc = new Device();

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
            SetVersion = 140,
            GetVersion = 142
        }

        #endregion

        #region Returns

        public enum ReturnValues
        {
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

            Pn5321SpiErr = 100,
            PnReadPacketErr = 101,
            ScanForCardErr = 102,

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
            NoAnswerFromCnv,
            NoAccessRecord,
            AccessNotOccured,
            AccessOccured,
            EraseOfRecordsError,
            PacketError,
            //LogNotFound,
            //PersonNotFound,
            IDNotFound,
            DeviceBusy,
            NoStaffGroup,
            OverFlow
        }

        #endregion

        #region ConfigEnums

        public enum ManufacturerCode
        {
            Barkodes = 1,
            Bayi = 2,
            Proje = 3,
            Degerlendirme = 4,
            Test = 5
        }

        public enum MealService
        {
            Undefined,
            Closed,
            Open
        }

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

        public enum Protocols
        {
            TelNet_Server = 0x00,
            TelNet_Client = 0x01,
            Raw = 0x02
        }

        public enum Converter
        {
            Tibbo = 1,
            Tac = 0,
            NewCon = 2
        }

        #endregion

        #region StringConversion

        public byte[] MakeStringCompatible(string strMessage)
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

        #region PrivateMethods

        private ReturnValues PingAndPortTest(string TargetIP, int TargetPort, TcpClient client)
        {

            Ping ping = new Ping();
            PingReply pingresult = ping.Send(TargetIP, 1500);
            try
            {
                if (pingresult.Status != IPStatus.Success)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PingError;
            }

            try
            {
                client.Connect(TargetIP, TargetPort);
                if (!client.Connected)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PortError;
            }
            return ReturnValues.Succesfull;
        }

        private ReturnValues PingAndPortTestFsm(string TargetIP, int TargetPort, TcpClient client)
        {

            //Ping ping = new Ping();
            //PingReply pingresult = ping.Send(TargetIP, 1500);
            //try
            //{
            //    if (pingresult.Status != IPStatus.Success)
            //        return ReturnValues.DeviceNotFound;
            //}
            //catch (Exception ex)
            //{
            //    return ReturnValues.PingError;
            //}

            try
            {
                client.Connect(TargetIP, TargetPort);
                if (!client.Connected)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PortError;
            }
            return ReturnValues.Succesfull;
        }

        private ReturnValues PingAndPortTestUDP(string TargetIP, int TargetPort, UdpClient client)
        {
            Ping ping = new Ping();
            PingReply pingresult = ping.Send(TargetIP, 1000);
            try
            {
                if (pingresult.Status != IPStatus.Success)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PingError;
            }

            try
            {
                client.Connect(TargetIP, TargetPort);
                if (!client.Client.Connected)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PortError;
            }
            return ReturnValues.Succesfull;
        }

        private ReturnValues SendDataStream(byte[] Buffer, TcpClient client)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    NetStream.Write(Buffer, 0, Buffer.Length);
                    return ReturnValues.Succesfull;
                }
                else return ReturnValues.LineBusy;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        private ReturnValues GetDataStream(out byte[] Buffer,TcpClient client, int msTimeOut)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                int count = 0; Buffer = null;                
                do
                {
                    if (NetStream.DataAvailable)
                    {
                        if (NetStream.DataAvailable)
                        {
                            if (NetStream.CanRead)
                            {
                                Buffer = new byte[NetStream.ReadByte()];
                                Buffer[0] = (byte)Buffer.Length;
                                NetStream.Read(Buffer, 1, Buffer.Length-1);
                                return ReturnValues.Succesfull;
                            }
                            else return ReturnValues.TimeOut;
                        }
                        else return ReturnValues.LineBusy;
                    }
                    else
                    {
                        if (count++ > msTimeOut)
                            return ReturnValues.TimeOut;
                        Thread.Sleep(1);
                    }
                } while (true);                
            }
            catch (Exception)
            {
                Buffer = null;                
                return ReturnValues.Failed;
            }
        }

        public TcpClient TcpClinetGlb;
        public UdpClient UdpClinetGlb;
        public EndPoint EpGlb;

        int ClientRefresh = 0;
        private ReturnValues SendAndReceiveDataGLb(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;
            int TimeO = msTimeOut;

            try
            {

                if (ClientRefresh++ > 80)
                {
                    ClientRefresh = 0;
                    TcpClinetGlb.Close();
                }

                if ( TcpClinetGlb == null || !TcpClinetGlb.Connected )
                {
                    IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                    TcpClinetGlb = new TcpClient();
                    EpGlb = (EndPoint)ipend;
                    ClientRefresh = 0;

                    TcpClinetGlb.Connect(ipend);
                }

                if (TcpClinetGlb.Connected)
                {
                    NetworkStream ns = TcpClinetGlb.GetStream();
                    ns.Write(SndBuf, 0, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[TcpClinetGlb.ReceiveBufferSize];

                        while (!ns.DataAvailable)
                        {
                            if (--TimeO == 0)
                            {
                                return ReturnValues.NoAnswer;
                            }
                            Thread.Sleep(1);
                        }
                        RcvLen = TcpClinetGlb.Client.ReceiveFrom(buf, ref EpGlb);

                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
                    }
                    catch (Exception ex)
                    {
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

                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveDataUdpGlb(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;
            int TimeO = msTimeOut;

            try
            {

                if (ClientRefresh++ > 80)
                {
                    ClientRefresh = 0;
                    UdpClinetGlb.Close();
                }

                if (UdpClinetGlb == null || UdpClinetGlb.Client == null || !UdpClinetGlb.Client.Connected)
                {
                    IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                    UdpClinetGlb = new UdpClient();
                    EpGlb = (EndPoint)ipend;
                    ClientRefresh = 0;

                    UdpClinetGlb.Connect(ipend);
                }

                if (UdpClinetGlb.Client.Connected)
                {
                    UdpClinetGlb.Send(SndBuf, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[UdpClinetGlb.Client.ReceiveBufferSize];

                        while (UdpClinetGlb.Available <= 0)
                        {
                            if (--TimeO == 0)
                            {
                                return ReturnValues.NoAnswer;
                            }
                            Thread.Sleep(1);
                        }
                        RcvLen = UdpClinetGlb.Client.ReceiveFrom(buf, ref EpGlb);
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
                    }
                    catch (Exception ex)
                    {
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
                            return ReturnValues.TimeOut;
                        }
                    }
                } while (true);

                //sw.Reset();
                //sw.Start();
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
                                if (ReceiveBuffer[i] == 154)
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
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SendDataStream(byte[] Buffer, int Offset, int Length, TcpClient client, Converter cnv)
        {
            try
            {
                NetworkStream NetStream = client.GetStream();
                if (NetStream.CanWrite)
                {
                    //NetStream.WriteByte(0x55); NetStream.WriteByte(0x55);
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
                                        if (ReturnFrame[i] == 153)
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
                                    return ReturnValues.Succesfull;
                                }
                            }
                            else
                            {
                                return ReturnValues.TimeOut;
                            }
                        }
                        else if (cnv == Converter.NewCon)
                        {
                            return ReturnValues.Succesfull;
                        }
                        else
                        {
                            return ReturnValues.NoAnswerFromCnv;
                        }
                    }
                    else return ReturnValues.Succesfull;
                }
                return ReturnValues.LineBusy;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveData(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;
            int TimeO = msTimeOut;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;


                //ReturnValues Result = PingAndPortTest(ip, port, client);
                //if (Result != ReturnValues.Succesfull)
                //{
                //    client.Close();
                //    return Result;
                //}
                TcpClinetGlb = null;
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
                        
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
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
                
                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveDataUdp(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.Client.SendTimeout = msTimeOut;
                //ReturnValues Result = PingAndPortTestUDP(ip, port, client);
                //if (Result != ReturnValues.Succesfull)
                //{
                //    client.Close();
                //    return Result;
                //}
                UdpClinetGlb = null;
                client.Connect(ipend);

                if (client.Client.Connected)
                {
                    client.Send(SndBuf, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.Client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }


        private void GetMacAddress(out byte[][] cfg, out int Length)
        {
            Length = 0;
            cfg = new byte[100][];

            byte[] LocatorData = new byte[4];
            LocatorData[0] = (byte)NETCommands.TagCmd;
            LocatorData[1] = (byte)LocatorData.Length;
            LocatorData[2] = (byte)NETCommands.Discover;
            LocatorData[3] = (byte)((0 - LocatorData[0] - LocatorData[1] - LocatorData[2]) & 0xff);

            IPEndPoint ipend = new IPEndPoint(IPAddress.Broadcast, UdpDiscoverPort);
            UdpClient ucl = new UdpClient(UdpDiscoverPort);

            ucl.Client.ReceiveTimeout = 1000;
            ucl.EnableBroadcast = true;

            ucl.Send(LocatorData, LocatorData.Length, ipend);
            int i = 0;
            try
            {
                while (true)
                {
                    byte[] bfr = ucl.Receive(ref ipend);
                    if ((byte)bfr[0] == (byte)NETCommands.TagStatus)
                    {
                        cfg[i] = new byte[bfr[1] - 3];
                        Array.Copy(bfr, 3, cfg[i], 0, cfg[i].Length);
                        Length = i++;
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

        #endregion

        #region Discoverprotocol

        public string Mac;
        public string Ip;
        public string Port;
        public string DeviceName;

        private int UdpDiscoverPort = 1931;

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

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        private enum NETCommands
        {
            TagCmd = 72,
            TagStatus = 77,
            Discover = 66,

            Undefined = 0x00,
            AckResponse = 0x01,
            UpdNetParams = 2,
            UpdCfgParams = 4,
            UpdSerialParams = 6,
            UpdDateTime = 8,
            RstDvc = 10,
            GetParameters = 12,
            UpdMfgParams = 14,
            GetAcsLogs = 16,
            GetPosLogs = 18,
            GetEventLogs = 20,
            UpdCafeTable = 22,
            UpdCafeTimes = 24,
            UpdCafeMaps = 26,
            GetCafeParams = 28,
            SaveNewAuthor = 30,
            EraseAllLogs = 32,
            ErasAllPeople = 34,
            SaveImage = 36,
            GetDateTime = 38,
            AddBlackList = 40,
            SaveNewUser = 42,
            ClearUserTable = 44,
            UpdServiceTimes = 46,
            UpdMappings = 48,
            GetEndofDayLogs = 50,
            FindAuthor = 52,
            GetAuthor = 54,
            ChangeMode = 56,
            FindBlackID = 58,
            EraseAuthor = 60,
            ModifyAuthor = 62,
            AddToReasonList = 64,
            RemoveFromReasonList = 66,
            EraseAllReasonList = 68,
            AddToStaffGroup = 70,
            RemoveFromStaffGroup = 72,
            EraseAllStaffGroup = 74,
            AddTimesToGroup = 76,
            GetLastID   = 78,
            ClearCampaigns	= 80,
            AddCampaign		= 82,
            GetTheCampaign	= 84,
            GetCfgParams = 86,
            GetCafeUser = 88,
            GetServiceTimes = 90,
            StartFrimwareUpdate = 92,
            GetTimesToGroup = 94,
            GetStaffGroups = 96,
            SendAccess = 98,
            GetAcsessLog = 100,
            SetMarketingPolicy = 102,
            GetMarketingPolicy = 104
        }

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

        #endregion

        public enum Access
        {
            Accept = 1,
	        Deny,
	        Lock,
	        Wait,
	        Forbidden,
	        OnlineAccept
        }

        private byte[] getManufacturerCode(ManufacturerCode code)
        {
            byte[] manf = new byte[8];
            switch (code)
            {
                case ManufacturerCode.Barkodes: manf = new byte[8] { 0xA1, 0x39, 0x7C, 0x11, 0xAB, 0xF8, 0x24, 0xCB }; break;
                case ManufacturerCode.Bayi: manf = new byte[8] { 0xB2, 0xCA, 0x33, 0x28, 0xA6, 0x01, 0xB5, 0x84 }; break;
                case ManufacturerCode.Proje: manf = new byte[8] { 0xC3, 0x28, 0x17, 0xBC, 0x07, 0x44, 0x22, 0x10 }; break;
                case ManufacturerCode.Degerlendirme: manf = new byte[8] { 0xD4, 0x12, 0x42, 0x04, 0x31, 0x69, 0x14, 0x62 }; break;
                case ManufacturerCode.Test: manf = new byte[8] { 0x54, 0x45, 0x53, 0x54, 0x30, 0x30, 0x30, 0x31 }; break;
            }
            return manf;
        }

        private ManufacturerCode getCompareManufacturerCode(string code)
        {
            ManufacturerCode manf;
            switch (code)
            {
                case "A1397C11ABF824CB": manf = ManufacturerCode.Barkodes; break;
                case "B2CA3328A601B584": manf = ManufacturerCode.Bayi; break;
                case "C32817BC07442210": manf = ManufacturerCode.Proje; break;
                case "D412420431691462": manf = ManufacturerCode.Degerlendirme; break;
                case "5445535430303031": manf = ManufacturerCode.Test; break;
                default: manf = 0; break;
            }
            return manf;
        }

        #region PublicMethods



        public ReturnValues GetLocalDevices(out Device[] Dvcs)
        {
            try
            {
                byte[][] cfg = new byte[100][];
                int length;
                GetMacAddress(out cfg, out length);

                Dvcs = new Device[length];


                for (int i = 0; i < length; i++)
                {

                    Dvcs[i] = dvc.GetDeviceParams(cfg[i]);
                }

                return ReturnValues.Succesfull;
            }
            catch (Exception ex)
            {
                Dvcs = null;
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetFsmDevices(string IP, int TcpPort, out int[] TargetAddress, int msTimeOut, Converter cnv)
        {
            TargetAddress = null;
            try
            {
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                if (Result != ReturnValues.Succesfull)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream = new byte[7];

                stream[0] = (byte)stream.Length;     				/*Packet Length*/
                stream[1] = 153;    	    /* Prefix  */
                stream[2] = (byte)255;                              /* Device Address  */
                stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                stream[4] = 128;                  /* Command */
                stream[5] = 0;     				                    /* SubCommand */

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                byte[] RcvBuffer; int len = 0;

                //ReturnValues rv = SendAndReceiveData(IP, TcpPort, stream, out RcvBuffer, out len, msTimeOut);
                ReturnValues rv = SendDataStream(stream, 0, stream.Length, client, cnv);
                if (rv == ReturnValues.Succesfull)
                {
                    
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
                    for (i = 0; i < 100; i++) TempAddresses[i] = 255;
                    for (int z = 0; z < Length; z++)
                    {
                        byte crc = 0;
                        if (Buff[z] == 154 && Buff[z - 1] == 4)
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
                                    Inx++;
                                }
                            }
                        }

                    }
                    if (Inx > 0)
                    {
                        TargetAddress = new int[Inx];
                        Array.Copy(TempAddresses, TargetAddress, Inx);
                        client.Close();
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return ReturnValues.InvalidResponse;
            }
            catch(Exception ex) 
            {
              return ReturnValues.InvalidResponse;
            }
         }

        public ReturnValues GetDateTimeUDP(string Mac, string IP, int TcpPort, int UdpPort,
                                        out DateTime dt,
                                        int msTimeOut)
        {
            dt = new DateTime();

            byte[] SndBuffer = new byte[6 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.GetDateTime;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveDataUdp(IP, UdpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[0] == SndBuffer[2] + 1)
                    {
                        dt = new DateTime();
                        dt = dt.AddYears(RcvBuffer[1] + RcvBuffer[2] * 256 - 1);
                        dt = dt.AddMonths(RcvBuffer[3] - 1);
                        dt = dt.AddDays(RcvBuffer[4] - 1);
                        dt = dt.AddHours(RcvBuffer[6]);
                        dt = dt.AddMinutes(RcvBuffer[7]);
                        dt = dt.AddSeconds(RcvBuffer[8]);
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDateTime(string Mac, string IP, int TcpPort, int UdpPort, 
                                        out DateTime dt, 
                                        int msTimeOut)
        {
            dt = new DateTime();

            byte[] SndBuffer = new byte[6 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.GetDateTime;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }
            
            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveData(IP, TcpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[0] == SndBuffer[2] + 1)
                    {
                        dt = new DateTime();
                        dt = dt.AddYears(RcvBuffer[1] + RcvBuffer[2] * 256 - 1);
                        dt = dt.AddMonths(RcvBuffer[3] - 1);
                        dt = dt.AddDays(RcvBuffer[4] - 1);
                        dt = dt.AddHours(RcvBuffer[6]);
                        dt = dt.AddMinutes(RcvBuffer[7]);
                        dt = dt.AddSeconds(RcvBuffer[8]);
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }            
        }

        public ReturnValues DeviceTestConnection(string IP, int TcpPort, int TargetAddress, int msTimeOut, Converter cnv)
        {
            try
            {
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                if (Result != ReturnValues.Succesfull)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream;
                stream = new byte[14];
                stream[0] = (byte)stream.Length;     	    /*Packet Length*/
                stream[1] = 153;    /* Prefix  */
                stream[2] = (byte)TargetAddress;            /* Device Address  */
                stream[3] = 4;      					    /* Length  */
                stream[4] = 96;         /* Command */
                stream[5] = 0;
                stream[6] = (byte)DateTime.Now.Second;
                stream[7] = (byte)DateTime.Now.Minute;
                stream[8] = (byte)DateTime.Now.Hour;
                stream[9] = (byte)DateTime.Now.DayOfWeek;
                stream[10] = (byte)DateTime.Now.Day;
                stream[11] = (byte)DateTime.Now.Month;
                stream[12] = (byte)(DateTime.Now.Year % 2000);

                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

               // byte[] RcvBuffer; int len;
               // ReturnValues rv = SendAndReceiveData(IP, TcpPort, stream, out RcvBuffer, out len, msTimeOut);
               byte[] packet; int length = 0;

                    //SendDataStream(stream, 0, stream.Length, client, cnv);
                    if (SendDataStream(stream, 0, stream.Length, client, cnv) == ReturnValues.Succesfull)
                    {
                        if (GetDataStream(out packet, out length, client, msTimeOut) == ReturnValues.Succesfull)
                        {
                            byte crc = 0;
                            for (int i = 0; i < length - 1; i++)
                            {
                                crc ^= packet[i];
                            }
                            crc = (byte)(255-crc);
                            if (crc == packet[length - 1])
                                if ((packet[1] == 154) & (packet[2] == (byte)TargetAddress))
                                    if (packet[4] == (byte)(stream[4]) + 1)
                                    {
                                        client.Close();
                                        return ReturnValues.Succesfull;
                                    }
                                    else { client.Close(); return ReturnValues.InvalidResponse; }
                                else { client.Close(); return ReturnValues.Failed; }
                            else { client.Close(); return ReturnValues.Failed; }
                        }
                        else { client.Close(); return ReturnValues.Failed; }
                    }
                else { client.Close(); return ReturnValues.Failed; }
            }
            catch
            {
                return ReturnValues.Failed;
            } 
          
        }

        public ReturnValues GetDeviceVersion(string Mac, string IP, int TcpPort, int UdpPort,
                                            out byte Version,
                                            int msTimeOut)
        {

            Version = 0;
            byte[] SndBuffer = new byte[14 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.GetMarketingPolicy;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  DateTime */


            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveData(IP, TcpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[0] == SndBuffer[2] + 1)
                    {
                        Version = RcvBuffer[1];
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDeviceVersionFsm(string IP, int TcpPort, int TargetAddress,out byte Version, int msTimeOut, Converter cnv)
        {
            Version = 0;

            TcpClient client = new TcpClient(); 
            try
            {
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                if (Result != ReturnValues.Succesfull)
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
                stream[stream.Length - 1]= 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);

                byte[] Response; int length = 0;

                if (SendDataStream(stream, 0, stream.Length, client, cnv) == ReturnValues.Succesfull)
	            {
                    if (GetDataStream(out Response, out length, client, msTimeOut) == ReturnValues.Succesfull)
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
                                    try {

                                        Version = Response[6];

                                        client.Close();
                                        return ReturnValues.Succesfull;
                                    }
                                    catch (Exception)
                                    {
                                        client.Close();
                                        return ReturnValues.Failed;
                                    }

                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            else { client.Close(); return ReturnValues.Failed; }
                        else { client.Close(); return ReturnValues.Failed; }
                    }
                    else { client.Close(); return ReturnValues.Failed; }
                }
                else { client.Close(); return ReturnValues.Failed; }
            }
            catch 
            {
                Version = 0;
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDeviceVersionFsm(string IP, int TcpPort, int TargetAddress, out byte Version, out ManufacturerCode code, int msTimeOut, Converter cnv)
        {
            Version = 0; code = 0;

            TcpClient client = new TcpClient();
            try
            {
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                if (Result != ReturnValues.Succesfull)
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

                if (SendDataStream(stream, 0, stream.Length, client, cnv) == ReturnValues.Succesfull)
                {
                    if (GetDataStream(out Response, out length, client, msTimeOut) == ReturnValues.Succesfull)
                    {
                        byte crc = 0;
                        for (int i = 0; i < length - 1; i++)
                        {
                            crc ^= Response[i];
                        }
                        crc = (byte)(255 - crc);
                        if (crc == Response[length - 1])
                            if ((Response[1] == 154) & (Response[2] == (byte)TargetAddress))
                                if (Response[4] == (byte)(stream[4] + 1) && Response[5] == 0)
                                {
                                    try
                                    {

                                        Version = Response[6];
                                        if (Response.Length > 8)
                                        {
                                            byte[] gManfCode = new byte[8];
                                            Array.Copy(Response, 7, gManfCode, 0, 8);
                                            string scode = Conversion.Byte2Hex(gManfCode);
                                            code = getCompareManufacturerCode(scode);
                                        }
                                        client.Close();
                                        return ReturnValues.Succesfull;
                                    }
                                    catch (Exception)
                                    {
                                        client.Close();
                                        return ReturnValues.Failed;
                                    }

                                }
                                else { client.Close(); return (ReturnValues)Response[5]; }
                            else { client.Close(); return ReturnValues.Failed; }
                        else { client.Close(); return ReturnValues.Failed; }
                    }
                    else { client.Close(); return ReturnValues.Failed; }
                }
                else { client.Close(); return ReturnValues.Failed; }
            }
            catch
            {
                client.Close();
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetDeviceVersionUDP(string Mac, string IP, int TcpPort, int UdpPort,
                                            out byte Version,
                                            int msTimeOut)
        {

            Version = 0;
            byte[] SndBuffer = new byte[14 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.GetMarketingPolicy;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  DateTime */


            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveDataUdp(IP, UdpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[0] == SndBuffer[2] + 1)
                    {
                        Version = RcvBuffer[1];
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SetDeviceVersion(string Mac, string IP, int TcpPort, int UdpPort,
                                    byte Version,
                                    int msTimeOut)
        {

            byte[] SndBuffer = new byte[7 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.SetMarketingPolicy;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  Version */
            SndBuffer[10] = Version;

            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveData(IP, TcpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if ((len == 6) & (RcvBuffer[3] == SndBuffer[2] + 1))
                    {
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues SetDeviceVersionFsm(string IP, int TcpPort, int  TargetAddress, byte version, int msTimeOut, Converter cnv)
        {
            try
            {
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                if (Result != ReturnValues.Succesfull)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream = new byte[8];

                stream[0] = (byte)stream.Length;     				/*Packet Length*/
                stream[1] = 153;    	    /* Prefix  */
                stream[2] = (byte)TargetAddress;                             /* Device Address  */
                stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                stream[4] = (byte)Commands.SetVersion;                  /* Command */
                stream[5] = 0;
                stream[6] = version;

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
                            if ((packet[1] == 154) & (packet[2] == (byte)TargetAddress))
                                if (packet[4] == (byte)(stream[4]) + 1)
                                {
                                    client.Close();
                                    return ReturnValues.Succesfull;
                                }
                                else { client.Close(); return ReturnValues.InvalidResponse; }
                            else { client.Close(); return ReturnValues.Failed; }
                        else { client.Close(); return ReturnValues.Failed; }
                    }
                    else { client.Close(); return ReturnValues.Failed; }
                }
                client.Close();
                return ReturnValues.Failed;
            }
            catch (Exception ex)
            {
                return ReturnValues.InvalidResponse;
            }
        }

        public ReturnValues SetDeviceVersionFsm(string IP, int TcpPort, int TargetAddress, byte version, ManufacturerCode code, int msTimeOut, Converter cnv)
        {
            try
            {
                TcpClient client = new TcpClient();
                ReturnValues Result = PingAndPortTestFsm(IP, TcpPort, client);
                byte[] ManfCode = getManufacturerCode(code); 
                if (Result != ReturnValues.Succesfull)
                {
                    client.Close();
                    return Result;
                }

                byte[] stream = new byte[6+1+8+1];

                stream[0] = (byte)stream.Length;     				/*Packet Length*/
                stream[1] = 153;    	    /* Prefix  */
                stream[2] = (byte)TargetAddress;                             /* Device Address  */
                stream[3] = (byte)(stream.Length - 4);      		/* Length  */
                stream[4] = (byte)Commands.SetVersion;                  /* Command */
                stream[5] = 0;
                stream[6] = version;
                Array.Copy(ManfCode, 0, stream, 7, 8);

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
                            if ((packet[1] == 154) & (packet[2] == (byte)TargetAddress))
                                if (packet[4] == (byte)((stream[4]) + 1) && packet[5] == 0)
                                {
                                    client.Close();
                                    return ReturnValues.Succesfull;
                                }
                                else { client.Close(); return (ReturnValues)packet[5]; }
                            else { client.Close(); return ReturnValues.Failed; }
                        else { client.Close(); return ReturnValues.Failed; }
                    }
                    else { client.Close(); return ReturnValues.Failed; }
                }
                client.Close();
                return ReturnValues.Failed;
            }
            catch (Exception ex)
            {
                return ReturnValues.InvalidResponse;
            }
        }

        public ReturnValues SetDeviceVersionUDP(string Mac, string IP, int TcpPort, int UdpPort,
                                    byte Version,
                                    int msTimeOut)
        {

            byte[] SndBuffer = new byte[7 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.SetMarketingPolicy;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  Version */
            SndBuffer[10] = Version;

            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveDataUdp(IP, UdpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[2] == (byte)CommDirection.ReaderToPc && RcvBuffer[3] == 1 && RcvBuffer[6] == 0xBA && RcvBuffer[7] == 0xBA)
                    {
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        public ReturnValues GetConfigParameters(string Mac, string IP, int TcpPort, int UdpPort, out Device ndvc,
                                    int msTimeOut)
        {
            ndvc = null;
            byte[] SndBuffer = new byte[14 + 6];
            SndBuffer[0] = (byte)SndBuffer.Length;
            SndBuffer[1] = (byte)CommDirection.PcToReader;
            SndBuffer[2] = (byte)NETCommands.GetCfgParams;
            SndBuffer[3] = 0x00; // Reserve

            /*  Mac Address */
            string mc = Mac.Replace("-", "");
            byte[] mac = Conversion.HexToByte(mc);
            for (int j = 0; j < mac.Length; j++)
            {
                SndBuffer[j + 4] = mac[5 - j];
            }

            /*  DateTime */


            /*  CRC         */
            ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
            SndBuffer[SndBuffer.Length - 2] = (byte)crc;
            SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

            try
            {
                byte[] RcvBuffer; int len;
                ReturnValues rv = SendAndReceiveData(IP, TcpPort, SndBuffer, out RcvBuffer, out len, msTimeOut);
                if (rv == ReturnValues.Succesfull)
                {
                    if (RcvBuffer[0] == SndBuffer[2] + 1)
                    {

                        byte[] cfg = new byte[100];
                        Array.Copy(RcvBuffer, 1, cfg, 0, (RcvBuffer.Length - 1));
                        ndvc = dvc.GetDeviceParams(cfg);
                        return rv;
                    }
                    else
                        return ReturnValues.InvalidResponse;
                }
                else
                    return rv;
            }
            catch
            {
                return ReturnValues.Failed;
            }
        }

        #endregion  
    }

    public class NetworkSettings
    {
        private enum UDPCommands
        {
            Undefined = 0x00,
            UpdateNetParams = 0x02,
            UpdateCfgParams = 0x04,
            UpdateSerialParams = 0x06,
            UpdateDateTime = 0x08,
            RstDvc = 0x0A             
        }

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        public string Mac;
        public string IP;
        public string SM;
        public string GW;
        public int TcpPort;
        public string ClientIP;
        public int ClientPort;
        public int UdpPort;
        public int UdpClientPort;

        public string Name;

        public NetworkSettings()
        {

        }

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
            PacketError,
            LogNotFound,
            PersonNotFound,
            IDNotFound,
            DeviceBusy
        }

        #endregion

        private ReturnValues SendAndReceiveData(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;

                client.ReceiveTimeout = msTimeOut;
                client.SendTimeout = msTimeOut;

                client.Connect(ipend);

                if (client.Connected)
                {
                    NetworkStream ns = client.GetStream();
                    ns.Write(SndBuf, 0, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveDataUdp(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.Client.SendTimeout = msTimeOut;

                client.Connect(ipend);

                if (client.Client.Connected)
                {
                    client.Send(SndBuf, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.Client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        public NetworkSettings GetNetworkParameters(byte[] bParameters)
        {
            NetworkSettings dvc = new NetworkSettings();
            try
            {
                dvc.Mac =   bParameters[0].ToString("X2") + "-" +
                            bParameters[1].ToString("X2") + "-" +
                            bParameters[2].ToString("X2") + "-" +
                            bParameters[3].ToString("X2") + "-" +
                            bParameters[4].ToString("X2") + "-" +
                            bParameters[5].ToString("X2");


                dvc.IP =    bParameters[9].ToString() + "." +
                            bParameters[8].ToString() + "." +
                            bParameters[7].ToString() + "." +
                            bParameters[6].ToString();

                dvc.ClientIP =  bParameters[13].ToString() + "." +
                                bParameters[12].ToString() + "." +
                                bParameters[11].ToString() + "." +
                                bParameters[10].ToString();

                dvc.SM =    bParameters[17].ToString() + "." +
                            bParameters[16].ToString() + "." +
                            bParameters[15].ToString() + "." +
                            bParameters[14].ToString();

                dvc.GW =    bParameters[21].ToString() + "." +
                            bParameters[20].ToString() + "." +
                            bParameters[19].ToString() + "." +
                            bParameters[18].ToString();

                dvc.TcpPort = (bParameters[23] * 256) + bParameters[22];
                
                dvc.ClientPort = (bParameters[25] * 256) + bParameters[24];
                
                dvc.UdpPort = (bParameters[27] * 256) + bParameters[26];
                
                dvc.UdpClientPort = (bParameters[29] * 256) + bParameters[28];

                ComMng cm = new ComMng();

                dvc.Name = ASCIIEncoding.ASCII.GetString(bParameters, 32, 20);

                dvc.Name = ASCIIEncoding.Default.GetString(bParameters, 32, 20);

                return dvc;
            }
            catch (Exception ex)
            {
                dvc = null;
                return dvc;
            }
        }

        public bool SetNetworkParametersUDP(NetworkSettings ns, string OldIP, int OldUdpPort, int OldTcpPort)
        {
            try
            {
                byte[] SndBuffer = new byte[52 + 6];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = (byte)UDPCommands.UpdateNetParams;
                SndBuffer[3] = 0x00; // Reserve

                /*  Mac Address */
                string mc = ns.Mac.Replace("-", "");
                byte[] mac = Conversion.HexToByte(mc);
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5 - j];
                }

                /* IP */
                string[] ip = ns.IP.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 10] = Convert.ToByte(ip[3 - j]);
                }

                /* Client IP */
                ip = ns.ClientIP.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 14] = Convert.ToByte(ip[3 - j]);
                }

                /*  SubnetMask*/
                ip = ns.SM.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 18] = Convert.ToByte(ip[3 - j]);
                }

                /* GateWay  */
                ip = ns.GW.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 22] = Convert.ToByte(ip[3 - j]);
                }

                /*  Tcp Port    */
                int port = Convert.ToInt32(ns.TcpPort);
                SndBuffer[26] = (byte)port;
                SndBuffer[27] = (byte)((port >> 8) & 0x00FF);

                /*  Tcp Client Port    */
                port = Convert.ToInt32(ns.ClientPort);
                SndBuffer[28] = (byte)port;
                SndBuffer[29] = (byte)((port >> 8) & 0x00FF);

                /*  Udp Port    */
                port = Convert.ToInt32(ns.UdpPort);
                SndBuffer[30] = (byte)port;
                SndBuffer[31] = (byte)((port >> 8) & 0x00FF);

                /*  Udp Client Port    */
                port = Convert.ToInt32(ns.UdpClientPort);
                SndBuffer[32] = (byte)port;
                SndBuffer[33] = (byte)((port >> 8) & 0x00FF);

                // Reserve
                SndBuffer[34] = 0x01;
                // Reserve
                SndBuffer[35] = 0x00;

                /*  Name        */
                ComMng cm = new ComMng();
                byte[] nm = cm.MakeStringCompatible(ns.Name);
                if (nm.Length > 20)
                    Array.Copy(nm, 0, SndBuffer, 36, 20);
                else
                {
                    Array.Copy(nm, 0, SndBuffer, 36, nm.Length);
                    for (int i = nm.Length; i < 20; i++)
                    {
                        SndBuffer[36 + i] = 32;
                    }
                }
                //char[] name = ns.Name.ToCharArray();
                //for (int j = 0; j < ns.Name.Length; j++)
                //{
                //    LocatorData[j + 36] = Convert.ToByte(name[j]);
                //}

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = (byte)crc;
                SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv = SendAndReceiveDataUdp("255.255.255.255", ns.UdpPort, SndBuffer, out RcvBuffer, out len, 1000);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[2] == (byte)CommDirection.ReaderToPc && RcvBuffer[3] == 1 && RcvBuffer[6] == 0xBA && RcvBuffer[7] == 0xBA)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetNetworkParameters(NetworkSettings ns, string OldIP,int OldUdpPort,int OldTcpPort)
        {
            try
            {
                byte[] SndBuffer = new byte[52+6];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = (byte)UDPCommands.UpdateNetParams;
                SndBuffer[3] =  0x00; // Reserve

                /*  Mac Address */
                string mc = ns.Mac.Replace("-", "");
                byte[] mac = Conversion.HexToByte(mc);
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5-j];
                }

                /* IP */
                string[] ip = ns.IP.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 10] = Convert.ToByte(ip[3-j]);
                }

                /* Client IP */
                ip = ns.ClientIP.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 14] = Convert.ToByte(ip[3-j]);
                }

                /*  SubnetMask*/
                ip = ns.SM.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 18] = Convert.ToByte(ip[3-j]);
                }

                /* GateWay  */
                ip = ns.GW.Split('.');
                for (int j = 0; j < ip.Length; j++)
                {
                    SndBuffer[j + 22] = Convert.ToByte(ip[3-j]);
                }
                                
                /*  Tcp Port    */
                int port = Convert.ToInt32(ns.TcpPort);
                SndBuffer[26] = (byte)port;
                SndBuffer[27] = (byte)((port >> 8) & 0x00FF);

                /*  Tcp Client Port    */
                port = Convert.ToInt32(ns.ClientPort);
                SndBuffer[28] = (byte)port;
                SndBuffer[29] = (byte)((port >> 8) & 0x00FF);

                /*  Udp Port    */
                port = Convert.ToInt32(ns.UdpPort);
                SndBuffer[30] = (byte)port;
                SndBuffer[31] = (byte)((port >> 8) & 0x00FF);

                /*  Udp Client Port    */
                port = Convert.ToInt32(ns.UdpClientPort);
                SndBuffer[32] = (byte)port;
                SndBuffer[33] = (byte)((port >> 8) & 0x00FF);
                
                // Reserve
                SndBuffer[34] = 0x01;
                // Reserve
                SndBuffer[35] = 0x00;

                /*  Name        */
                ComMng cm = new ComMng();
                byte[] nm = cm.MakeStringCompatible(ns.Name);
                if (nm.Length > 20)
                    Array.Copy(nm,0,SndBuffer,36,20);
                else
                {
                    Array.Copy(nm,0,SndBuffer,36,nm.Length);
                    for (int i = nm.Length; i < 20; i++)
                    {
                        SndBuffer[36 + i] = 32;
                    }
                }
                //char[] name = ns.Name.ToCharArray();
                //for (int j = 0; j < ns.Name.Length; j++)
                //{
                //    LocatorData[j + 36] = Convert.ToByte(name[j]);
                //}

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = (byte)crc;
                SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv = SendAndReceiveData(OldIP, OldTcpPort, SndBuffer, out RcvBuffer, out len, 1000);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[3] == SndBuffer[2] + 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }           
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class ConfigSettings
    {
        public enum Mode
        {
            Undefined = 0,
            Offline,
            Online,
            Reserved,
            Service
        }

        public enum AccessRule
        {
            Undefined,
            Unlimited,
            OnlyAuthorized,
            ShiftAuthorized
        }

        public enum Direction
        {
            Undefined,
            Entry,
            Exit,
            Emergency
        }

        public enum OpticalInput
        {
            Undefined,
            NotUsed,
            Used
        }

        public enum SeqRead
        {
            Undefined,
            NotUsed,
            OneRead,
            IdBased
        }

        public enum MultipleRead
        {
            Undefined,
            NotUsed,
            Used
        }

        public enum AntiPassBack
        {
            Undefined,
            NotUsed,
            Used
        }

        public enum SmartRelay
        {
            Undefined,
            NotUsed,
            Used
        }

        private enum UDPCommands
        {
            Undefined = 0x00,
            AckResponse = 0x01,
            UpdateNetParams = 0x02,
            UpdateCfgParams = 0x04,
            UpdateSerialParams = 0x06,
            UpdateDateTime = 0x08,
            RstDvc = 0x0A
        }

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        public enum BlackListControl
        {
            Undefined = 0,
            OnBoard = 1,
            OnPos = 2
        }

        public enum CardKeyControl
        {
            Undefined       = 0,
            NotUsed         = 1,
            KeyOnly         = 2,
            KeyFormat       = 3
        }

        public enum PosIntegration
        {
            Undefined = 0,
            NotUsed = 1,
            Used = 2
        }

        public int ReservedTime;
        public int RelayTime;
        public int OnlineTimeOut;
        public Mode RunTimeMode;
        public AccessRule AccesRule;
        public Direction DirectionType;
        public OpticalInput OpticInput;
        public int SeqReadTimeOut;
        public SeqRead SequentialMode;
        public MultipleRead MultipleReadMode;
        public AntiPassBack AntiPassBackMode;
        public BlackListControl BlackListControlMethod;
        public PosIntegration IntegratedPos;
        public CardKeyControl KeyCtrlMode;
        public SmartRelay SmartRelayMode;
        public int PosMaxErrCnt;

        public ConfigSettings()
        {

        }

        public ConfigSettings GetConfigParameters(byte[] bParameters)
        {
            ConfigSettings dvc = new ConfigSettings();

            try
            {
                dvc.RelayTime = bParameters[0];

                dvc.OnlineTimeOut = bParameters[1];

                //bParameters[2];       //Reserved

                dvc.RunTimeMode = (Mode)bParameters[3];

                dvc.AccesRule = (AccessRule)bParameters[4];

                dvc.DirectionType = (Direction)bParameters[5];

                dvc.OpticInput = (OpticalInput)bParameters[6];

                dvc.SequentialMode = (SeqRead)bParameters[7];

                dvc.MultipleReadMode = (MultipleRead)bParameters[8];

                dvc.AntiPassBackMode = (AntiPassBack)bParameters[9];

                dvc.BlackListControlMethod = (BlackListControl)bParameters[10];    //Cezeri için Card Key Control

                dvc.IntegratedPos = (PosIntegration)bParameters[11]; //Cezeri Smart Relay Mode

                dvc.PosMaxErrCnt = bParameters[12];

                dvc.SeqReadTimeOut = bParameters[15] * 256 + bParameters[14];

                // Reserved Bytes 10,,,19

                return dvc;
            }
            catch (Exception ex)
            {
                dvc = null;
                return dvc;
            }
        }

        public bool SetConfigParameters(NetworkSettings ns, ConfigSettings cfg)
        {
            byte[] RcvBuf = null;
            int RcvLen = 0;

            try
            {
                byte[] LocatorData = new byte[20 + 12];
                LocatorData[0] = (byte)LocatorData.Length;
                LocatorData[1] = (byte)CommDirection.PcToReader;
                LocatorData[2] = (byte)UDPCommands.UpdateCfgParams;
                LocatorData[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    LocatorData[j + 4] = mac[5 - j];
                }

                /*  usRelayTicks    */
                LocatorData[10] = (byte)cfg.RelayTime;

                /*  usOnlineTimeOut    */
                LocatorData[11] = (byte)cfg.OnlineTimeOut;

                LocatorData[12] = 0x00;                           //---------------Reserved----------------

                /* ucMode */
                LocatorData[13] = (byte)cfg.RunTimeMode;

                /* ucAcsRule */
                LocatorData[14] = (byte)cfg.AccesRule;

                /* ucDir */
                LocatorData[15] = (byte)cfg.DirectionType;

                /* ucOpticInput */
                LocatorData[16] = (byte)cfg.OpticInput;

                /* ucSequentialReadMode */
                LocatorData[17] = (byte)cfg.SequentialMode;

                /* ucMultipleReadMode */
                LocatorData[18] = (byte)cfg.MultipleReadMode;

                /* ucAntiPassBackMode */
                LocatorData[19] = (byte)cfg.AntiPassBackMode;

                /* ucReserved 1,,,5 */
                LocatorData[20] = (byte)cfg.BlackListControlMethod;     //Cezeri için Card Key Control
                LocatorData[21] = (byte)cfg.IntegratedPos;              //Cezeri için Smart Relay Mode
                LocatorData[22] = (byte)cfg.PosMaxErrCnt;

                LocatorData[23] = 0x00;
                /*  usSequentialTimeOut    */
                LocatorData[24] = (byte)(cfg.SeqReadTimeOut % 256);
                LocatorData[25] = (byte)(cfg.SeqReadTimeOut / 256);

                LocatorData[26] = 0x00;
                LocatorData[27] = 0x00;
                LocatorData[28] = 0x00;
                LocatorData[29] = 0x00;

                /*  CRC         */
                ushort crc = CRC.Crc16(0, LocatorData, LocatorData.Length - 2);
                LocatorData[LocatorData.Length - 2] = (byte)crc;
                LocatorData[LocatorData.Length - 1] = (byte)(crc >> 8);

                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ns.IP), ns.TcpPort);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;
                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;

                try
                {
                client.Connect(ipend);

                if (client.Connected)
                {
                    NetworkStream nws = client.GetStream();
                    nws.Write(LocatorData, 0, LocatorData.Length);
                    try
                    {
                        byte[] buf = new byte[client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();

                        if (RcvLen > 3)
                        {
                            if (buf[0] == 6 && buf[3] == (byte)(UDPCommands.UpdateCfgParams+1))
                            {
                                client.Close();
                                return true;
                            }
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        return false;
                    }
                }
                else
                {
                    client.Close();
                    return false;
                }

                }
                catch (Exception)
                {
                    client.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetConfigParametersUDP(NetworkSettings ns, ConfigSettings cfg)
        {
            try
            {
                byte[] LocatorData = new byte[20 + 12];
                LocatorData[0] = (byte)LocatorData.Length;
                LocatorData[1] = (byte)CommDirection.PcToReader;
                LocatorData[2] = (byte)UDPCommands.UpdateCfgParams;
                LocatorData[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    LocatorData[j + 4] = mac[5 - j];
                }

                /*  usRelayTicks    */
                LocatorData[10] = (byte)cfg.RelayTime;

                /*  usOnlineTimeOut    */
                LocatorData[11] = (byte)cfg.OnlineTimeOut;


                LocatorData[12] = 0x00;                           //---------------Reserved----------------

                /* ucMode */
                LocatorData[13] = (byte)cfg.RunTimeMode;

                /* ucAcsRule */
                LocatorData[14] = (byte)cfg.AccesRule;

                /* ucDir */
                LocatorData[15] = (byte)cfg.DirectionType;

                /* ucOpticInput */
                LocatorData[16] = (byte)cfg.OpticInput;

                /* ucSequentialReadMode */
                LocatorData[17] = (byte)cfg.SequentialMode;

                /* ucMultipleReadMode */
                LocatorData[18] = (byte)cfg.MultipleReadMode;

                /* ucAntiPassBackMode */
                LocatorData[19] = (byte)cfg.AntiPassBackMode;

                /* ucReserved 1,,,5 */
                LocatorData[20] = (byte)cfg.BlackListControlMethod;     //Cezeri için Card Key Control
                LocatorData[21] = (byte)cfg.IntegratedPos;              //Cezeri için Smart Relay Mode        
                LocatorData[22] = (byte)cfg.PosMaxErrCnt;

                LocatorData[23] = 0x00;
                /*  usSequentialTimeOut    */
                LocatorData[24] = (byte)(cfg.SeqReadTimeOut % 256);
                LocatorData[25] = (byte)(cfg.SeqReadTimeOut / 256);

                LocatorData[26] = 0x00;
                LocatorData[27] = 0x00;
                LocatorData[28] = 0x00;
                LocatorData[29] = 0x00;

                /*  CRC         */
                ushort crc = CRC.Crc16(0, LocatorData, LocatorData.Length - 2);
                LocatorData[LocatorData.Length - 2] = (byte)crc;
                LocatorData[LocatorData.Length - 1] = (byte)(crc >> 8);

                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ns.IP), ns.UdpPort);
                UdpClient ucl = new UdpClient(ns.UdpPort);

                try
                {
                    ucl.Client.ReceiveTimeout = 1000;
                    ucl.EnableBroadcast = true;

                    ucl.Send(LocatorData, LocatorData.Length, ipend);
                    while (true)
                    {
                        byte[] bfr = ucl.Receive(ref ipend);
                        if (bfr[3] == (byte)UDPCommands.AckResponse)
                        {
                            ucl.Close();
                            return true;
                        }
                    }
                    ucl.Close();
                    return true;
                }
                catch (Exception)
                {
                    ucl.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class SerialSettings
    {

        public enum BuadRate
        {
            Br_115200 = 0,
            Br_56000,
            Br_38400,
            Br_19200,
            Br_9600,
            Br_4800,        
        }

        public enum SerialType
        {
            Undefined,
            Rs232,
            Wiegand
        }

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
            PacketError,
            LogNotFound,
            PersonNotFound,
            IDNotFound,
            DeviceBusy
        }

        #endregion

        private enum UDPCommands
        {
            Undefined = 0x00,
            AckResponse = 0x01,
            UpdateNetParams = 0x02,
            UpdateCfgParams = 0x04,
            UpdateSerialParams = 0x06,
            UpdateDateTime = 0x08,
            RstDvc = 0x0A
        }

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        public BuadRate Rs485Baud;
        public BuadRate Rs232Baud;
        public int SerialAddress;
        public SerialType SerialComType;

        public SerialSettings()
        {

        }

        public SerialSettings GetSerialParameters(byte[] bParameters)
        {
            SerialSettings dvc = new SerialSettings();

            try
            {
                dvc.Rs485Baud = (SerialSettings.BuadRate)bParameters[0];
                dvc.Rs232Baud = (SerialSettings.BuadRate)bParameters[1];
                dvc.SerialAddress = bParameters[2];
                dvc.SerialComType = (SerialType)bParameters[3];
                
                // Reserved 4
                // Reserved 5
                // Reserved 6

                return dvc;
            }
            catch (Exception ex)
            {
                dvc = null;
                return dvc;
            }
        }

        private ReturnValues PingAndPortTest(string TargetIP, int TargetPort, TcpClient client)
        {

            Ping ping = new Ping();
            PingReply pingresult = ping.Send(TargetIP, 1500);
            try
            {
                if (pingresult.Status != IPStatus.Success)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PingError;
            }

            try
            {
                client.Connect(TargetIP, TargetPort);
                if (!client.Connected)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PortError;
            }
            return ReturnValues.Succesfull;
        }

        private ReturnValues PingAndPortTestUDP(string TargetIP, int TargetPort, UdpClient client)
        {
            Ping ping = new Ping();
            PingReply pingresult = ping.Send(TargetIP, 1000);
            try
            {
                if (pingresult.Status != IPStatus.Success)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PingError;
            }

            try
            {
                client.Connect(TargetIP, TargetPort);
                if (!client.Client.Connected)
                    return ReturnValues.DeviceNotFound;
            }
            catch (Exception ex)
            {
                return ReturnValues.PortError;
            }
            return ReturnValues.Succesfull;
        }

        private ReturnValues SendAndReceiveData(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.SendTimeout = msTimeOut;

                //ReturnValues Result = PingAndPortTest(ip, port, client);
                //if (Result != ReturnValues.Succesfull)
                //{
                //    client.Close();
                //    return Result;
                //}
                client.Connect(ipend);

                if (client.Connected)
                {
                    NetworkStream ns = client.GetStream();
                    ns.Write(SndBuf, 0, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveDataUdp(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.Client.SendTimeout = msTimeOut;
                //ReturnValues Result = PingAndPortTestUDP(ip, port, client);
                //if (Result != ReturnValues.Succesfull)
                //{
                //    client.Close();
                //    return Result;
                //}
                client.Connect(ipend);

                if (client.Client.Connected)
                {
                    client.Send(SndBuf, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.Client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        if (RcvBuf[1] == 0xF0 && RcvBuf[4] == 0xF0 && RcvBuf[5] == 0xF0)
                            return ReturnValues.DeviceBusy;
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        public bool SetSerialParametersUDP(NetworkSettings ns, SerialSettings ss)
        {
            try
            {
                byte[] SndBuffer = new byte[7 + 12];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = (byte)UDPCommands.UpdateSerialParams;
                SndBuffer[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5 - j];
                }

                /*  ulBaudRate    */
                SndBuffer[10] = (byte)ss.Rs485Baud;
                SndBuffer[11] = (byte)ss.Rs232Baud;

                /*  ucDvcAddr    */
                SndBuffer[12] = (byte)ss.SerialAddress;

                /*  ucComType    */
                SndBuffer[13] = (byte)ss.SerialComType;

                /*  ucReserved    */
                SndBuffer[14] = 0x00;
                SndBuffer[15] = 0x00;
                SndBuffer[16] = 0x00;

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = (byte)crc;
                SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv = SendAndReceiveDataUdp(ns.IP, ns.UdpPort, SndBuffer, out RcvBuffer, out len, 500);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[3] == SndBuffer[2] + 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ss = null;
                return false;
            }
        }

        public bool SetSerialParameters(NetworkSettings ns, SerialSettings ss)
        {
            try
            {
                byte[] SndBuffer = new byte[7 + 12];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = (byte)UDPCommands.UpdateSerialParams;
                SndBuffer[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5 - j];
                }

                /*  ulBaudRate    */
                SndBuffer[10] = (byte)ss.Rs485Baud;
                SndBuffer[11] = (byte)ss.Rs232Baud;

                /*  ucDvcAddr    */
                SndBuffer[12] = (byte)ss.SerialAddress;
                
                /*  ucComType    */
                SndBuffer[13] = (byte)ss.SerialComType;

                /*  ucReserved    */
                SndBuffer[14] = 0x00;
                SndBuffer[15] = 0x00;
                SndBuffer[16] = 0x00;

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = (byte)crc;
                SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv =  SendAndReceiveData(ns.IP, ns.TcpPort, SndBuffer, out RcvBuffer, out len, 500);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[3] == SndBuffer[2] + 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }                
            }
            catch (Exception ex)
            {
                ss = null;
                return false;
            }
        }
    }

    public class MfgSettings
    {
        public enum TesterCode
        {
            Undefined   = 0x00,
            Abdulkadir  = 0xAA,
            Sinan       = 0xAB,
            Umit        = 0xAC,
            Arif        = 0xAD,
            Ferhat      = 0XAE
        }

        public enum ProductionType
        {
            DESK_TEST = 0x01,
            AREA_TEST = 0x02,
            PRODUCTION = 0x04,
            EVALUATION = 0x08
        }

        public enum DeviceType
        {
            TimeAttendace = 0x10,
            PosReader = 0x20,
            CafeReader = 0x30,
            AccessPro = 0x40
        }

        private enum UDPCommands
        {
            Undefined = 0x00,
            AckResponse = 0x01,
            UpdateNetParams = 0x02,
            UpdateCfgParams = 0x04,
            UpdateSerialParams = 0x06,
            UpdateDateTime = 0x08,
            RstDvc = 0x0A
        }

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        public string           Password;
        public string           FirmwareVersion;
        public string           PcbVersion;
        public DateTime         ProductionTime;
        public TesterCode       Tester;
        public ProductionType   Production;
        public DeviceType       Device;

        public MfgSettings()
        {

        }


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
            PacketError,
            LogNotFound,
            PersonNotFound,
            IDNotFound,
            DeviceBusy
        }

        #endregion

        private ReturnValues SendAndReceiveData(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;

                client.ReceiveTimeout = msTimeOut;
                client.SendTimeout = msTimeOut;

                client.Connect(ipend);

                if (client.Connected)
                {
                    NetworkStream ns = client.GetStream();
                    ns.Write(SndBuf, 0, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        private ReturnValues SendAndReceiveDataUdp(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                UdpClient client = new UdpClient();
                EndPoint ep = (EndPoint)ipend;

                client.Client.ReceiveTimeout = msTimeOut;
                client.Client.SendTimeout = msTimeOut;

                client.Connect(ipend);

                if (client.Client.Connected)
                {
                    client.Send(SndBuf, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.Client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }

        public MfgSettings GetMfgParameters(byte[] bParameters)
        {
            MfgSettings dvc = new MfgSettings();

            try
            {
                dvc.FirmwareVersion = (bParameters[0] >>4 ).ToString() + "." + (bParameters[0] & 0x0F).ToString();

                dvc.PcbVersion = (bParameters[1] >> 4).ToString() + "." + (bParameters[1] & 0x0F).ToString();

                dvc.Tester = (TesterCode)bParameters[2];

                dvc.Production = (ProductionType)(bParameters[3]&0x0f);
                
                dvc.Device = (DeviceType)(bParameters[3]&0xf0);

                ulong pw = bParameters[7];
                pw = (pw << 8) | bParameters[6];
                pw = (pw << 8) | bParameters[5];
                pw = (pw << 8) | bParameters[4];

                dvc.Password = pw.ToString();

                long dt = bParameters[11];
                dt = (dt << 8) | bParameters[10];
                dt = (dt << 8) | bParameters[9];
                dt = (dt << 8) | bParameters[8];

                dvc.ProductionTime = new DateTime(1970, 1, 1);
                dvc.ProductionTime = dvc.ProductionTime.AddSeconds(dt);
                dvc.ProductionTime = dvc.ProductionTime.AddHours(-3);   // GMT + 3
                return dvc;
            }
            catch (Exception ex)
            {
                dvc = null;
                return dvc;
            }
        }

        public bool SetMfgParameters(NetworkSettings ns, MfgSettings ms)
        {
            try
            {
                //if (Properties.Settings.Default.BuildType != "Production")
                //{
                //    return true;
                //}
                byte[] SndBuffer = new byte[12 + 12];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = 0x0E;
                SndBuffer[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5 - j];
                }

                /*  ucFirmwareVersion    ! NOT CHANGEABLE ! */
                //SndBuffer[10] = 0x00;

                string FrmVer = ms.FirmwareVersion.Substring(0, 1) + ms.FirmwareVersion.Substring(2, 1);
                SndBuffer[10] = (byte)Convert.ToByte(FrmVer, 16);

                /*  PcbVersion ! NOT CHANGEABLE ! */
                SndBuffer[11] = 0x00;

                /*  ucTester    */
                SndBuffer[12] = (byte)ms.Tester;

                /*  ucProduction */
                SndBuffer[13] = (byte)ms.Production;

                /*  ulPassword    */
                int tmp = Convert.ToInt32(ms.Password);
                SndBuffer[14] = (byte)tmp;
                SndBuffer[15] = (byte)(tmp >> 8);
                SndBuffer[16] = (byte)(tmp >> 16);
                SndBuffer[17] = (byte)(tmp >> 24);

                /*  ulTestTime    */

                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddHours(3);
                long tm = (ms.ProductionTime.Ticks - dt.Ticks)/10000000;
                SndBuffer[18] = (byte)tm;
                SndBuffer[19] = (byte)(tm >> 8);
                SndBuffer[20] = (byte)(tm >> 16);
                SndBuffer[21] = (byte)(tm >> 24);

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = (byte)crc;
                SndBuffer[SndBuffer.Length - 1] = (byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv = SendAndReceiveData(ns.IP, ns.TcpPort, SndBuffer, out RcvBuffer, out len, 500);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[3] == SndBuffer[2] + 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }            
            }
            catch (Exception ex)
            {
                ms = null;
                return false;
            }
        }
    }

    public class Device
    {
        public NetworkSettings NetParams;
        public ConfigSettings CfgParams;
        public SerialSettings SerialParams;
        public MfgSettings MfgParams;

        public Device()
        {
            NetParams = new NetworkSettings();
            CfgParams = new ConfigSettings();
            SerialParams = new SerialSettings();
            MfgParams = new MfgSettings();
        }

        public Device GetDeviceParams(byte[] LocatorData)
        {
            try
            {
                Device dvc = new Device();

                dvc.NetParams = new NetworkSettings();
                dvc.SerialParams = new SerialSettings();
                dvc.CfgParams = new ConfigSettings();
                dvc.MfgParams = new MfgSettings();

                byte[] net = new byte[52];
                byte[] cfg = new byte[20];
                byte[] serial = new byte[6];
                byte[] mfg = new byte[12];

                Array.Copy(LocatorData, 0, net, 0, net.Length);
                NetworkSettings ns = new NetworkSettings();
                dvc.NetParams = ns.GetNetworkParameters(net);

                Array.Copy(LocatorData, net.Length, serial, 0, serial.Length);
                SerialSettings ss = new SerialSettings();
                dvc.SerialParams = ss.GetSerialParameters(serial);

                Array.Copy(LocatorData, net.Length+serial.Length, cfg, 0, cfg.Length);
                ConfigSettings cs = new ConfigSettings();
                dvc.CfgParams = cs.GetConfigParameters(cfg);

                Array.Copy(LocatorData, net.Length + serial.Length + cfg.Length, mfg, 0, mfg.Length);
                MfgSettings ms = new MfgSettings();
                dvc.MfgParams = ms.GetMfgParameters(mfg);

                return dvc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public static class CRC
    {
        static byte[] g_pucCrc8CCITT =
        {
            0x00, 0x07, 0x0E, 0x09, 0x1C, 0x1B, 0x12, 0x15,
            0x38, 0x3F, 0x36, 0x31, 0x24, 0x23, 0x2A, 0x2D,
            0x70, 0x77, 0x7E, 0x79, 0x6C, 0x6B, 0x62, 0x65,
            0x48, 0x4F, 0x46, 0x41, 0x54, 0x53, 0x5A, 0x5D,
            0xE0, 0xE7, 0xEE, 0xE9, 0xFC, 0xFB, 0xF2, 0xF5,
            0xD8, 0xDF, 0xD6, 0xD1, 0xC4, 0xC3, 0xCA, 0xCD,
            0x90, 0x97, 0x9E, 0x99, 0x8C, 0x8B, 0x82, 0x85,
            0xA8, 0xAF, 0xA6, 0xA1, 0xB4, 0xB3, 0xBA, 0xBD,
            0xC7, 0xC0, 0xC9, 0xCE, 0xDB, 0xDC, 0xD5, 0xD2,
            0xFF, 0xF8, 0xF1, 0xF6, 0xE3, 0xE4, 0xED, 0xEA,
            0xB7, 0xB0, 0xB9, 0xBE, 0xAB, 0xAC, 0xA5, 0xA2,
            0x8F, 0x88, 0x81, 0x86, 0x93, 0x94, 0x9D, 0x9A,
            0x27, 0x20, 0x29, 0x2E, 0x3B, 0x3C, 0x35, 0x32,
            0x1F, 0x18, 0x11, 0x16, 0x03, 0x04, 0x0D, 0x0A,
            0x57, 0x50, 0x59, 0x5E, 0x4B, 0x4C, 0x45, 0x42,
            0x6F, 0x68, 0x61, 0x66, 0x73, 0x74, 0x7D, 0x7A,
            0x89, 0x8E, 0x87, 0x80, 0x95, 0x92, 0x9B, 0x9C,
            0xB1, 0xB6, 0xBF, 0xB8, 0xAD, 0xAA, 0xA3, 0xA4,
            0xF9, 0xFE, 0xF7, 0xF0, 0xE5, 0xE2, 0xEB, 0xEC,
            0xC1, 0xC6, 0xCF, 0xC8, 0xDD, 0xDA, 0xD3, 0xD4,
            0x69, 0x6E, 0x67, 0x60, 0x75, 0x72, 0x7B, 0x7C,
            0x51, 0x56, 0x5F, 0x58, 0x4D, 0x4A, 0x43, 0x44,
            0x19, 0x1E, 0x17, 0x10, 0x05, 0x02, 0x0B, 0x0C,
            0x21, 0x26, 0x2F, 0x28, 0x3D, 0x3A, 0x33, 0x34,
            0x4E, 0x49, 0x40, 0x47, 0x52, 0x55, 0x5C, 0x5B,
            0x76, 0x71, 0x78, 0x7F, 0x6A, 0x6D, 0x64, 0x63,
            0x3E, 0x39, 0x30, 0x37, 0x22, 0x25, 0x2C, 0x2B,
            0x06, 0x01, 0x08, 0x0F, 0x1A, 0x1D, 0x14, 0x13,
            0xAE, 0xA9, 0xA0, 0xA7, 0xB2, 0xB5, 0xBC, 0xBB,
            0x96, 0x91, 0x98, 0x9F, 0x8A, 0x8D, 0x84, 0x83,
            0xDE, 0xD9, 0xD0, 0xD7, 0xC2, 0xC5, 0xCC, 0xCB,
            0xE6, 0xE1, 0xE8, 0xEF, 0xFA, 0xFD, 0xF4, 0xF3
        };

        static ushort[] g_pusCrc16 =
        {
            0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
            0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
            0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
            0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
            0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
            0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
            0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
            0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
            0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
            0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
            0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
            0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
            0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
            0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
            0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
            0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
            0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
            0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
            0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
            0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
            0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
            0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
            0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
            0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
            0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
            0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
            0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
            0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
            0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
            0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
            0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
            0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
        };

        static byte CRC8_ITER(byte crc, byte data)
        {
            return g_pucCrc8CCITT[(byte)((crc) ^ (data))];
        }

        static ushort CRC16_ITER(ushort crc, byte data)
        {
            return (ushort)((ushort)((crc) >> 8) ^ g_pusCrc16[(byte)((crc) ^ (data))]);
        }

        public static byte Crc8(byte ucCrc, byte[] pucData, int ulCount)
        {
            for (int i = 0; i < ulCount; i++)
            {
                ucCrc = CRC8_ITER(ucCrc, pucData[i]);
            }
            return ucCrc;
        }

        public static ushort Crc16(ushort usCrc, byte[] pucData, int ulCount)
        {
            for (int i = 0; i < ulCount; i++)
            {
                usCrc = CRC16_ITER(usCrc, pucData[i]);
            }
            return usCrc;
        }

    }

    public static class Conversion
    {
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
        #endregion
    }

    public class Authors
    {
        public enum UserTypes
        {
            User = 0,
            Administrator
            
        }

        public enum IdLength
        {
            FourBytes = 0,
            SevenBytes
        }
    }

    public class AcsLog
	{
        public enum AccessTypes
	    {
            Undefined       = 0,
            Entry           = 1,
            Exit            = 2,
            
            Accepted        = 10,
            Denied          = 20,
            IDAccepted      = 30,
            IDDenied        = 40,
            IDForbidden     = 50,

            AcceptedEntry   = 11,
            AcceptedExit    = 12,

            DeniedEntry     = 21,
            DeniedExit      = 22,
            
            IDAcceptedEntry = 31,
            IDAcceptedExit  = 32,
            
            IDDeniedEntry   = 41,
            IDDeniedExit    = 42,
            
            IDForbiddenEntry= 51,
            IDForbiddenExit = 52
	    }

        public string CardID;
        public AccessTypes AccessType;
        public bool FeedBack;
        public DateTime AcsTime;
        public string DvcIP;
        public int Index;
        public int ReasonCode;

        public AcsLog GetLog(int idx,byte[] bBuffer,string IP)
        {
            try 
	        {	        
		        AcsLog log = new AcsLog();

                log.Index = idx;

                ulong id = bBuffer[0];
                id = (id<<8)|bBuffer[1];
                id = (id<<8)|bBuffer[2];
                id = (id<<8)|bBuffer[3];

                log.CardID = id.ToString();

                long dt = bBuffer[7];
                dt = (dt << 8) | bBuffer[6];
                dt = (dt << 8) | bBuffer[5];
                dt = (dt << 8) | bBuffer[4];

                log.AcsTime = new DateTime(1970, 1, 1);
                log.AcsTime = log.AcsTime.AddSeconds(dt);
                log.AcsTime = log.AcsTime.AddHours(0);   // GMT + 3

                log.AccessType = (AccessTypes)bBuffer[8];
                log.ReasonCode = (int)bBuffer[9];
                log.FeedBack = true;

                log.DvcIP = IP;

                return log;
	        }
	        catch (Exception)
	        {
                return null;
	        }
        }

        public AcsLog GetLogMfPlus(int idx,byte[] bBuffer,string IP)
        {
            try 
	        {	        
		        AcsLog log = new AcsLog();

                log.Index = idx;


                long dt = bBuffer[3];
                dt = (dt << 8) | bBuffer[2];
                dt = (dt << 8) | bBuffer[1];
                dt = (dt << 8) | bBuffer[0];

                log.AcsTime = new DateTime(1970, 1, 1);
                log.AcsTime = log.AcsTime.AddSeconds(dt);
                log.AcsTime = log.AcsTime.AddHours(0);   // GMT + 3

                ulong id = 0;
                id = (id<<8)|bBuffer[4];
                id = (id<<8)|bBuffer[5];
                id = (id<<8)|bBuffer[6];
                id = (id << 8) | bBuffer[7];
                id = (id << 8) | bBuffer[8];
                id = (id << 8) | bBuffer[9];
                id = (id << 8) | bBuffer[10];

                log.CardID = id.ToString();

                log.AccessType = (AccessTypes)bBuffer[11];
                log.ReasonCode = (int)bBuffer[12];
                log.FeedBack = Convert.ToBoolean(bBuffer[13]);

                log.DvcIP = IP;

                return log;
	        }
	        catch (Exception)
	        {
                return null;
	        }
        }
	}
     
    public class PosLog
    {
        public enum AccessTypes
        {
            Undefined = 0,
            Entry,
            Exit,
            Denied,
            Entired,
            Exited
        }

        public enum PosError
        {
            PosAck = 0xAA,
            PosNack,
            PosTimeOut,
            NoAnswer,
            WrongAnswer,
            InWhiteList,
            InBlackList,
            Done = 0xff
        }

        public string CardID;
        public string Fee;
        public string Balance;
        public DateTime Time;
        public string UserType;
        public string Times;
        public string DvcIP;
        public string SpecialData;
        public bool FeedBack;
        public int Index;
        public PosError PosResponse;
        public PosError SentMessage;

        public PosLog GetLog(int index, byte[] bBuffer, string IP)
        {
            try
            {
                PosLog log = new PosLog();

                log.Index = index;

                ulong ultmp = bBuffer[0];
                ultmp = (ultmp << 8) | bBuffer[1];
                ultmp = (ultmp << 8) | bBuffer[2];
                ultmp = (ultmp << 8) | bBuffer[3];

                log.CardID = ultmp.ToString();

                ultmp = bBuffer[7];
                ultmp = (ultmp << 8) | bBuffer[6];
                ultmp = (ultmp << 8) | bBuffer[5];
                ultmp = (ultmp << 8) | bBuffer[4];

                log.Fee = ultmp.ToString();

                ultmp = bBuffer[11];
                ultmp = (ultmp << 8) | bBuffer[10];
                ultmp = (ultmp << 8) | bBuffer[9];
                ultmp = (ultmp << 8) | bBuffer[8];

                log.Balance = ultmp.ToString();

                long dt = bBuffer[15];
                dt = (dt << 8) | bBuffer[14];
                dt = (dt << 8) | bBuffer[13];
                dt = (dt << 8) | bBuffer[12];

                log.Time = new DateTime(1970, 1, 1);
                log.Time = log.Time.AddSeconds(dt);
                //log.Time = log.Time.AddHours(-3);   // GMT + 3

                log.UserType = bBuffer[16].ToString();

                log.Times = bBuffer[17].ToString();
                
                log.FeedBack = Convert.ToBoolean(bBuffer[18]);

                log.SpecialData = "";

                for (int i = 0; i < 16; i++)
                {
                    log.SpecialData += bBuffer[i + 19].ToString("X2");
                }
                
                log.PosResponse = (PosError)bBuffer[35];

                log.SentMessage = (PosError)bBuffer[36];

                log.DvcIP = IP;

                return log;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class CafeTable
    {
        public string[] UserCodes;
        public string[] Prices;

        private enum CommDirection
        {
            PcToReader = 0x02,
            ReaderToPc = 0x04,
            ReaderToReader = 0x06
        }

        private enum UDPCommands
        {
            TagCmd = 72,
            TagStatus = 77,
            Discover = 66,

            Undefined = 0x00,
            AckResponse = 0x01,
            UpdNetParams = 2,
            UpdCfgParams = 4,
            UpdSerialParams = 6,
            UpdDateTime = 8,
            RstDvc = 10,
            GetParameters = 12,
            UpdMfgParams = 14,
            GetAcsLogs = 16,
            GetPosLogs = 18,
            GetEventLogs = 20,
            UpdCafeTable = 22,
            UpdCafeTimes = 24,
            UpdCafeMaps = 26
        }

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
            PacketError,
            LogNotFound,
            PersonNotFound,
            IDNotFound,
            DeviceBusy
        }

        #endregion

        private ReturnValues SendAndReceiveData(string ip, int port, byte[] SndBuf, out byte[] RcvBuf, out int RcvLen, int msTimeOut)
        {
            RcvLen = 0;
            RcvBuf = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ip), port);
                TcpClient client = new TcpClient();
                EndPoint ep = (EndPoint)ipend;

                client.ReceiveTimeout = msTimeOut;
                client.SendTimeout = msTimeOut;

                client.Connect(ipend);

                if (client.Connected)
                {
                    NetworkStream ns = client.GetStream();
                    ns.Write(SndBuf, 0, SndBuf.Length);
                    try
                    {
                        byte[] buf = new byte[client.ReceiveBufferSize];
                        RcvLen = client.Client.ReceiveFrom(buf, ref ep);
                        client.Close();
                        RcvBuf = new byte[RcvLen];
                        Array.Copy(buf, RcvBuf, RcvLen);
                        return ReturnValues.Succesfull;
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
                return ReturnValues.Failed;
            }
        }
        
        public CafeTable GetTables(byte[] bBuffer, string IP)
        {
            CafeTable ct = new CafeTable();

            ct.UserCodes = new string[bBuffer.Length/4];
            ct.Prices = new string[UserCodes.Length*3];

            int j=0;
            for (int i = 0; i < UserCodes.Length; i++)
            {
                ct.UserCodes[i] = bBuffer[i].ToString();
                j++;
            }

            for (int i = 0; i < Prices.Length; i++)
            {
                int tmp = bBuffer[j + i]; j++;
                tmp = (tmp << 8) | bBuffer[j + i]; j++;
                ct.Prices[i] = tmp.ToString();
            }
            return ct;
        }

        public bool SetTables(NetworkSettings ns, CafeTable ct)
        {
            try
            {
                byte[] SndBuffer = new byte[12 + 12];
                SndBuffer[0] = (byte)SndBuffer.Length;
                SndBuffer[1] = (byte)CommDirection.PcToReader;
                SndBuffer[2] = (byte)UDPCommands.UpdCafeTable;
                SndBuffer[3] = 0x00; // Reserve

                /*  Mac Address */
                byte[] mac = Conversion.HexToByte(ns.Mac.Replace("-", ""));
                for (int j = 0; j < mac.Length; j++)
                {
                    SndBuffer[j + 4] = mac[5 - j];
                }

                int k=0;
                for (int i = 0; i < ct.UserCodes.Length; i++)
                {
                    SndBuffer[i + 10] = Convert.ToByte(ct.UserCodes[i]);
                    k++;
                }

                for (int i = 0; i < ct.Prices.Length; i++)
                {
                    int tmp = Convert.ToInt32(ct.Prices[i]);
                    SndBuffer[i + k] = (byte)tmp; k++;
                    SndBuffer[i + k] = (byte)(tmp>>8); k++;
                }

                /*  CRC         */
                ushort crc = CRC.Crc16(0, SndBuffer, SndBuffer.Length - 2);
                SndBuffer[SndBuffer.Length - 2] = 0xBA;//(byte)crc;
                SndBuffer[SndBuffer.Length - 1] = 0xBA;//(byte)(crc >> 8);

                try
                {
                    byte[] RcvBuffer; int len;
                    ReturnValues rv = SendAndReceiveData(ns.IP, ns.TcpPort, SndBuffer, out RcvBuffer, out len, 500);
                    if (rv == ReturnValues.Succesfull)
                    {
                        if (RcvBuffer[3] == SndBuffer[2] + 1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }            
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
