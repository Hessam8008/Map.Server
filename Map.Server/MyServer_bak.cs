using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GpsServer
{

    /*
     
        ► Codec 8 protocol sending over TCP ◄
        ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄

        Source: https://wiki.teltonika-gps.com/view/Codec
        ─────────────────────────────────────────────────
        
        ☼ Phase 1: 
        First when module connects to server, module sends its IMEI. IMEI is sent the same way as encoding
        barcode. First comes short identifying number of bytes written and then goes IMEI as text (bytes).
        For example IMEI 123456789012345 would be sent as 000F313233343536373839303132333435
        After receiving IMEI, server should determine if it would accept data from this module. If yes server will reply
        to module 01, if not 00. Note that confirmation should be sent as binary packet.
        

        ☼ Phase 2:
        After IMEI confirmation, module starts to send first AVL data packet. After server receives packet and parses it, server must
        report to module the number of data received as integer (four bytes).
        If sent data number and reported by server doesn’t match module resends data. 
        
         * Below table represents AVL Data Packet structure:

        0x00000000 
        (Preamble)	| Data Field Length 	| Codec ID	| Number Of Data 1 |	AVL Data	| Number Of Data 2	| CRC-16
        4 bytes	    |      4 bytes	        |  1 byte	|      1 byte      |     X bytes	|     1 byte	    | 4 bytes      

        Preamble          : The packet starts with four zero bytes.
        Data Field Length : Size is calculated starting from Codec ID to Number of Data 2.
        Codec ID          : In Codec8 it is always 0x08.
        Number of Data 1  : A number which defines how many records is in the packet.
        AVL Data          : Actual data in the packet (more information below).
        Number of Data 2  : A number which defines how many records is in the packet. This number must be the same as “Number of Data 1”.
        CRC-16            : Calculated from Codec ID to the Second Number of Data. 
                            CRC (Cyclic Redundancy Check) is an error-detecting code using for detect accidental changes to RAW data. 
                            For calculation we are using CRC-16/IBM.


        * Below table represents AVL Data structure.

        Timestamp  |  Priority  |	GPS Element	| IO Element
         8 bytes   |   1 byte	|     15 bytes	|   X bytes

        Timestamp   : A difference, in milliseconds, between the current time and midnight, January, 1970 UTC (UNIX time).
        Priority    : Field which define AVL data priority (more information below).
        GPS Element : Location information of the AVL data (more information below).
        IO Element  : Additional configurable information from device (more information below).
        
            Priority
            0	| Low
            1	| High
            2	| Panic


            GPS element
            Longitude |	Latitude |	Altitude  |	 Angle	| Satellites |  Speed
             4 bytes  |	4 bytes	 |  2 bytes	  | 2 bytes	|   1 byte	 | 2 bytes

            Longitude  : East – west position.
            Latitude   : North – south position.
            Altitude   : Meters above sea level.
            Angle      : Degrees from north pole.
            Satellites : Number of visible satellites.
            Speed      : Speed calculated from satellites.

            Note: Speed will be 0x0000 if GPS data is invalid.
            Note: For FMB630, FMB640 and FM63XY, minimum AVL packet size is 45 bytes (all IO elements disabled). Maximum AVL packet size is 255 bytes. 
                  For other devices, minimum AVL packet size is 45 bytes (all IO elements disabled). Maximum AVL packet size is 1280 bytes.
     */

    public sealed class TeltonikaServer_bak
    {
        private TcpListener _server;
        private bool _stop = false;

        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, 3343);
            _server.Start();
            Log($"Start listening on {_server.LocalEndpoint}");
            new Thread(StartListener).Start();
        }

        public void Stop()
        {
            _server.Stop();
            _stop = true;
        }

        private static void Log(string str, ConsoleColor color = ConsoleColor.Gray)
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1).GetMethod().Name;
            
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0:HH:mm:ss} | {1}> {2}", DateTime.Now, methodName, str);
            Console.ForegroundColor = tempColor;
        }

        private void StartListener()
        {
            try
            {

                while (!_stop)
                {
                    Log("Waiting for an incoming connection...", ConsoleColor.Green);
                    var client = _server.AcceptTcpClient();
                    Log("New connection accepted.", ConsoleColor.Yellow);
                    var t = new Thread(HandleDevice);
                    t.Start(client);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                _server.Stop();
            }
        }
        
        private void HandleDevice(object obj)
        {
            Log("Handling connection...");
            var client = (TcpClient)obj;

            try
            {
                var bytes = new byte[2048];
                var stream = client.GetStream();
                
                /*
                 * --► PHASE 01 ◄--
                 */

                var counter = stream.Read(bytes, 0, bytes.Length);
                
                /* First 2 bytes are IMEI length + next 15 bytes are IMEI  == 17 bytes */
                if (counter != 17)
                {
                    Log($"Authentication failed. bytes received {counter}");
                    CloseConnection(client);
                    return;
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", "");
                var imeiLen = int.Parse(hexImeiLen,  NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    Log($"Invalid IMEI format. imei length: {imeiLen}");
                    CloseConnection(client);
                    return;
                }

                /* Decode IMEI */
                var imei = Encoding.ASCII.GetString(bytes, 2,15);
                Log($"IMEI: {imei}");

                /* Reply acknowledge byte (01 for accept, 00 for reject) */
                stream.Write(BitConverter.GetBytes(1));
                Log("Replied acknowledge byte.");


                /*
                 * --► PHASE 02 ◄--
                 */
                while ((counter = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", "");
                    var parser = new FmxParserCodec8();
                    var data = parser.Parse(hexData);
                    Log($"{imei} > Read packet success. {data}");
                    stream.Write(BitConverter.GetBytes((int)data.NumberOfData1));
                    try
                    {
                        //MessageCenter.BroadcastMessage($"IMEI: {imei} [{data.NumberOfData1}]\n{data}");
                    }
                    catch (Exception e)
                    {
                        Log(e.Message, ConsoleColor.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            finally
            {
                CloseConnection(client);
            }
        }

        private void CloseConnection(TcpClient client)
        {
            Log("Closing connection...", ConsoleColor.Yellow);
            try
            {
                client.Close();
                client.Dispose();
                Log("Closed", ConsoleColor.DarkYellow);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

    }
}
