using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using FSM.Properties;
using System.Threading;
using System.IO;
using System.Drawing;



namespace FSM
{

  class Class2
    {
        #region Variables - Enumators - Constructor
        public enum ReturnValues
        {
            UndefinedError,
            Success,
            Failed,
            Successful,
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
            NoAnswerFromCnv,
            TimeError,
            MessageLengthIsTooBig,
            NoAccessRecord,
            AccessNotOccured,
            AccessOccured,
            EraseOfRecordsError,
            PacketError,
            RepeatPack,

            StringLengthIsLess,
            NoRequest
        }
        #endregion

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   COMPORT GET DATA
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        private ReturnValues GetDataStream(out byte[] Buf, out int Length, SerialPort client, int msTimeOut)
        {
            try
            {
                byte[] ReceiveBuffer = new byte[client.ReadBufferSize];
                Buf = null;
                Length = 0;
                int count = 0;
                do
                {
                    if (client.BytesToRead > 0)
                        break;
                    else
                    {
                        if (count++ > msTimeOut)
                            return ReturnValues.TimeOut;
                        Thread.Sleep(1);
                    }
                } while (true);
                if (client.BytesToRead > 0)
                {
                    int i = 0;
                    do
                    {
                        if (client.BytesToRead > 0)
                        {
                            ReceiveBuffer[i] = (byte)client.ReadByte();
                            if (ReceiveBuffer[i] == (byte)154)
                                break;
                            else i++;
                        }
                        else
                        {
                            Thread.Sleep(1);
                            if (count++ > msTimeOut)
                                return ReturnValues.TimeOut;
                        }
                    } while (true);
                    Length = ReceiveBuffer[i - 1];
                    Buf = new byte[Length];
                    Buf[0] = (byte)Length;
                    Buf[1] = ReceiveBuffer[i];
                    for (i = 2; i < Length; )
                    {
                        if (client.BytesToRead > 0)
                        {
                            Buf[i] = (byte)client.ReadByte();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(1);
                            if (count++ > msTimeOut)
                                return ReturnValues.TimeOut;
                        }
                    }
                    return ReturnValues.Successful;
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
        #endregion

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   COMPORT SEND DATA
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        private ReturnValues SendDataStream(byte[] Buffer, int Offset, int Length, SerialPort client)
        {
            try
            {
                client.Write(Buffer, Offset, Buffer.Length);
                return ReturnValues.Successful;
            }
            catch (Exception ex)
            {
                return ReturnValues.Failed;
            }
        }
        #endregion

        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region                   SEND BOOT REQUEST
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        ReturnValues SendBootRequest(int msTimeOut)
        {
            try
            {
                //-------------------BARKODES----------------------------------------------------------
                //byte[] stream;
                //stream = new byte[7];
                //stream[0] = (byte)stream.Length;  /*Packet Length*/
                //stream[1] = (byte)153;    	    /* Prefix  */
                //stream[2] = (byte)34;             /* Device Address  */
                //stream[3] = 4;      				/* Length  */
                //stream[4] = (byte)126;            /* Command */
                //stream[5] = 0;     				/* SubCommand */

                //stream[stream.Length - 1] = 0;
                //for (int i = 0; i < stream.Length - 1; i++)
                //{
                //    stream[stream.Length - 1] ^= stream[i];
                //}
                //stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);
                //-------------------BENİM PORJE------------------------------------------------------
                byte[] stream;
                stream = new byte[7];
                stream[0] = (byte)153;    	        /* Prefix  */
                stream[1] = (byte)stream.Length;    /*Packet Length*/
                stream[2] = (byte)34;               /* Device Address  */
                stream[3] = (byte)126;              /* Command */
                stream[4] = (byte)24;     			/* SubCommand */
                //WaitAck((byte)126);
                stream[stream[1] - 2] = Convert.ToByte("155");

                byte CheckSum = stream[0];
                for (int i = 1; i < (stream.Length - 1); i++)
                {
                    CheckSum ^= stream[i];
                }
                stream[stream.Length - 1] = CheckSum;
                //------------------------------------------------------------------------------------

                byte[] packet; int length = 0;
                if (SendDataStream(stream, 0, stream.Length, comport) == ReturnValues.Successful)
                {
                    if (GetDataStream(out packet, out length, comport, msTimeOut) == ReturnValues.Successful)
                    {
                        byte crc = 0;
                        for (int i = 0; i < packet[0] - 1; i++)
                        {
                            crc ^= packet[i];
                        }
                        crc = (byte)(255 - crc);
                        if (crc == packet[packet[0] - 1])
                        {
                            if ((packet[1] == (byte)154) & (packet[2] == (byte)79))
                            {
                                return ReturnValues.Successful;
                            }
                            else { return ReturnValues.InvalidResponse; }
                        }
                        else { return ReturnValues.PacketError; }
                    }
                    else { return ReturnValues.NoAnswer; }
                }
                return ReturnValues.DeviceNotFound;
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
        ReturnValues SendBootBytes(byte Command, byte[] data, uint Address, byte DataLength, int msTimeOut)
        {
            try
            {
                byte[] stream;
                stream = new byte[10 + DataLength];
                stream[0] = (byte)153;
                stream[1] = (byte)stream.Length;
                stream[2] = (byte)DataLength;
                stream[3] = Command;
                stream[4] = (byte)(Command + 10);

                stream[5] = (byte)(Address >> 0);
                stream[6] = (byte)(Address >> 8);
                stream[7] = (byte)(Address >> 16);
                stream[8] = (byte)(Address >> 24);

                for (int i = 0; i < DataLength; i++)
                    stream[i + 9] = data[i];


                stream[stream.Length - 1] = 0;
                for (int i = 0; i < stream.Length - 1; i++)
                {
                    stream[stream.Length - 1] ^= stream[i];
                }
                stream[stream.Length - 1] = (byte)(255 - stream[stream.Length - 1]);



                byte[] packet; int length = 0;

                if (SendDataStream(stream, 0, stream.Length, comport) == ReturnValues.Successful)
                {
                    if (GetDataStream(out packet, out length, comport, msTimeOut) == ReturnValues.Successful)
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
                                    if (packet[3] == stream[5] && packet[4] == stream[6] && packet[5] == stream[7] && packet[6] == stream[8])
                                        return ReturnValues.Successful;
                                }
                                else if (packet[2] == 'E' && packet[3] == 'R')
                                    return ReturnValues.RepeatPack;
                            }
                            else { return ReturnValues.InvalidResponse; }
                        }
                        else { return ReturnValues.PacketError; }
                    }
                    else { return ReturnValues.NoAnswer; }
                }
                return ReturnValues.DeviceNotFound;
            }
            catch (Exception)
            {
                return ReturnValues.Failed;
            }
        }
        #endregion

         //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//
        #region SerialPort
        //<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>//

        private SerialPort comport = new SerialPort();

        private void Form1_Load(object sender, EventArgs e)
        {
            //settings.Reload();
            //RefreshComPortList();
            //if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
            //else if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
        }


        private string[] OrderedPortNames()
        {
            int num;

            return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
        }

        private void RefreshComPortList()
        {
            //cmbPortName.Items.Clear();
            //string[] ports = SerialPort.GetPortNames();
            //for (int i = 0; i < ports.Length; i++)
            //    cmbPortName.Items.Add(ports[i]);
        }

        private void cmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //settings.PortName = cmbPortName.SelectedItem.ToString();
            //settings.Save();
        }

        bool OpenComPort()
        {
            if (!comport.IsOpen)
            {
                try
                {
                    //comport.Close();
                    //comport = new SerialPort();
                    //comport.BaudRate = 115200;
                    //comport.PortName = cmbPortName.SelectedItem.ToString();
                    // Open the port
                    comport.Open();
                }
                catch { }

                if (!comport.IsOpen)
                    return false;
                else
                    return true;
            }
            return false;
        }
        #endregion
    

        
    
    
    }

  public class CLogoManager
  {
      private const int MAX_WIDTH = 128;
      private const int MAX_HEIGHT = 64;
      private const string PROPER_COLORDEPTH = "Format24bppRgb";
      private string _file_name;
      private int _width;
      private int _height;
      private string _color_depth;
      private string header;
      private Bitmap _bmp;

      public string LogoFile
      {
          get
          {
              return this._file_name;
          }
          set
          {
              this._file_name = value;
          }
      }

      public int Width
      {
          get
          {
              return this._width;
          }
          set
          {
              this._width = value;
          }
      }

      public int Height
      {
          get
          {
              return this._height;
          }
          set
          {
              this._height = value;
          }
      }

      public string ColorDepth
      {
          get
          {
              return this._color_depth;
          }
          set
          {
              this._color_depth = value;
          }
      }

      public string HeaderFile
      {
          get
          {
              return this.header;
          }
          set
          {
              this.header = value;
          }
      }

      public CLogoManager(string file_name)
      {
          this._file_name = file_name;
          if (this._bmp != null)
              this._bmp.Dispose();
          this._bmp = new Bitmap(this._file_name);
          this.get_bitmap_information();
      }
      public CLogoManager(Image img, bool contol)
      {
          if (this._bmp != null)
              this._bmp.Dispose();
          this._bmp = new Bitmap(img);
            if (contol)
                this.get_bitmap_information();
      }

      private void get_bitmap_information()
      {
          this._width = this._bmp.Width;
          this._height = this._bmp.Height;
          this._color_depth = ((object)this._bmp.PixelFormat).ToString();
          //if (this._width > 128)
          //    throw new Exception("Resim genişliği " + 128.ToString() + " pixelden büyük olmamalıdır.");
          //if (this._height > 64)
          //    throw new Exception("Resim yüksekliği " + 64.ToString() + " pixelden büyük olmamalıdır.");
          //if (this._height % 8 > 0)
          //    throw new Exception("Resim yükleği 8 ve katları şeklinde olmalıdır.");
          //if (!this._color_depth.StartsWith("Format24") && !this._color_depth.StartsWith("Format32"))
          //    throw new Exception("Resim 24 bit renk derinliğine sahip olmalıdır.");
      }

      public void BuildHeader(string header_file, int top, int left)
      {
          this.header = header_file;
          int num1 = this._bmp.Height / 8;
          StreamWriter streamWriter = new StreamWriter(this.header, false);
          FileInfo fileInfo = new FileInfo(this.header);
          top /= 8;
          string str = fileInfo.Name.Replace(".h", "").Replace(" ", "");
          streamWriter.Write("code unsigned char " + str + "_width=" + this._bmp.Width.ToString() + ";");
          streamWriter.Write("code unsigned char " + (object)str + "_height=" + num1.ToString() + ";");
          streamWriter.Write("code unsigned char " + str + "_top=" + top.ToString() + ";");
          streamWriter.Write("code unsigned char " + str + "_left=" + left.ToString() + ";");
          streamWriter.Write("code unsigned char " + str + "[]={");
          for (int index1 = 0; index1 < num1; ++index1)
          {
              for (int x = 0; x < this._bmp.Width; ++x)
              {
                  int num2 = 0;
                  for (int index2 = 0; index2 < 8; ++index2)
                  {
                      int num3 = num2 >> 1;
                      Color pixel = this._bmp.GetPixel(x, index1 * 8 + index2);
                      num2 = (int)pixel.R != (int)byte.MaxValue || (int)pixel.G != (int)byte.MaxValue || (int)pixel.B != (int)byte.MaxValue ? num3 + 128 : num3;
                  }
                  streamWriter.Write(num2.ToString() + ",");
              }
          }
          streamWriter.Write("0};");
          streamWriter.Close();
      }
      public List<byte> BuildLogoGetBytes()
      {
          List<byte> Liste = new List<byte>();
          int num1 = this._bmp.Height / 8;
          for (int index1 = 0; index1 < num1; ++index1)
          {
              for (int x = 0; x < this._bmp.Width; ++x)
              {
                  int num2 = 0;
                  for (int index2 = 0; index2 < 8; ++index2)
                  {
                      int num3 = num2 >> 1;
                      Color pixel = this._bmp.GetPixel(x, index1 * 8 + index2);
                      num2 = (int)pixel.R != (int)byte.MaxValue || (int)pixel.G != (int)byte.MaxValue || (int)pixel.B != (int)byte.MaxValue ? num3 + 128 : num3;
                  }
                  Liste.Add((byte)num2);
              }
          }
          return Liste;
      }

      public List<byte> BuildLogoGetReverseBytes()
      {
          List<byte> Liste = new List<byte>();
          int num1 = this._bmp.Height / 8;
          //for (int index1 = 0; index1 < num1; ++index1)
          for (int index1 = 5; index1 > num1 - num1; --index1)
          {
              for (int x = 0; x < this._bmp.Width; ++x)
              {
                  int num2 = 0;
                  for (int index2 = 0; index2 < 8; ++index2)
                  {
                      int num3 = num2 >> 1;
                      Color pixel = this._bmp.GetPixel((0 + x), (num1 - index1) * 8 + index2);
                     // Color pixel = this._bmp.GetPixel((127 - x), ((num1 - 1) - index1) * 8 + (7 - index2));
                      num2 = (int)pixel.R != (int)byte.MaxValue || (int)pixel.G != (int)byte.MaxValue || (int)pixel.B != (int)byte.MaxValue ? num3 + 128 : num3;
                  }
                  Liste.Add((byte)num2);
              }
          }
          return Liste;
      }

        public List<byte> BuildLogoVerticalGetBytes()
        {
            List<byte> Liste = new List<byte>();

            int w, h, rX, rY, heigth;
            w = this._bmp.Width;
            h = this._bmp.Height;

            Liste.Add((byte)w);
            Liste.Add((byte)(w>>8));
            Liste.Add((byte)h);
            Liste.Add((byte)(h >> 8));

            if ((h % 8) != 0) heigth = (h / 8) + 1;
            else heigth = (h / 8);

            for (int j = 0; j < heigth; j++)
            {
                for (int i = 0; i < w; i++)
                {
                   
                    int num2 = 0;

                    int a = 7;
                    if (j == (heigth - 1) && (h % 8 != 0))
                        a = (h % 8);

                    int num3 = 0;
                    for (; a >= 0; a--)
                    {
                        rX = i;
                        rY = (j * 8) + a;
                        if ((rY == h) && (j == (heigth-1)))
                            rY -= 1;
                        try
                        {
                            Color pixel = this._bmp.GetPixel(rX, rY);

                            if (((int)pixel.R != (int)byte.MaxValue) || ((int)pixel.G != (int)byte.MaxValue) || ((int)pixel.B != (int)byte.MaxValue))
                            {
                                num3 |= (1 << a);
                            }
                            else num3 |= (0 << a);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        


                    }
                    Liste.Add((byte)num3);
                }
            }
            return Liste;
        }

        public List<short> BuildLogoRGBVerticalGetBytes(out int numberofcolor)
        {
            numberofcolor = 0;
            List<short> Liste = new List<short>();

            int w, h, rX, rY, heigth;
            w = this._bmp.Width;
            h = this._bmp.Height;

            Liste.Add((short)w);
            Liste.Add((short)h);

            if ((h % 8) != 0) heigth = (h / 8) + 1;
            else heigth = (h / 8);

            for (int j = 0; j < heigth; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    int a = 7;
                    if (j == (heigth - 1) && (h % 8 != 0))
                        a = (h % 8)-1;

                    short num3 = 0;
                    for (; a >= 0; a--)
                    {
                        rX = i;
                        rY = (j * 8) + a;

                        Color pixel = this._bmp.GetPixel(rX, rY);

                        num3 = (short)(((pixel.R & 0xF8) << 0x08) | ((pixel.G & 0xFC) << 0x03) | (pixel.B >> 0x03));

                        bool macth = false;
                        for (int clr = 0; clr < Liste.Count; clr++)
                        {
                            if (num3 == Liste[clr])
                            {
                                macth = true;
                                break;
                            }
                        }

                        if(!macth)
                            numberofcolor++;

                        Liste.Add((short)num3);
                    }
                    
                }
            }
            return Liste;
        }

        public List<short> GetRGB(out int numberofcolor)
        {
            numberofcolor = 0;
            List<short> Liste = new List<short>();

            int w, h;
            w = this._bmp.Width;
            h = this._bmp.Height;

            Liste.Add((short)w);
            Liste.Add((short)h);

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    ushort clr = 0, nclr;
                    Color pixel = this._bmp.GetPixel(i, j);
                    //RGB565
                    clr = (ushort)(((pixel.R & 0xF8) << 0x08) | ((pixel.G & 0xFC) << 0x03) | (pixel.B >> 0x03));
                    //Swap
                    clr = (ushort)((clr << 0x000B) | (clr & 0x07E0) | (clr >> 0x000B));
                    nclr = (ushort)(clr >> 8); nclr |= (ushort)(clr << 8);
                    Liste.Add((short)nclr);
                }
            }
            return Liste;
        }
    }
}
