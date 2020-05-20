﻿namespace GpsServer.Teltonika.Client
{
    using System;
    using System.Collections.Generic;

    using GpsServer.Models;

    /*     ► Codec 8 protocol sending over TCP ◄
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

           Timestamp  |  Priority  | GPS Element | IO Element
            8 bytes   |   1 byte   |  15 bytes	 |   X bytes

           Timestamp   : A difference, in milliseconds, between the current time and midnight, January, 1970 UTC (UNIX time).
           Priority    : Field which define AVL data priority (more information below).
           GPS Element : Location information of the AVL data (more information below).
           IO Element  : Additional configurable information from device (more information visit reference site).

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

           Examples:
           1'st example
           000000000000003608010000016B40D8EA30010000000000000000000000000000000105021503010101425E0F01F10000601A014E0000000000000000010000C7CF

           2'nd example
           000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A

           3'rd example
           000000000000004308020000016B40D57B480100000000000000000000000000000001010101000000000000016B40D5C198010000000000000000000000000000000101010101000000020000252C

        */
    public class FmxParserCodec8
    {
        /// <summary>
        /// Defines the _utcDate.
        /// </summary>
        private readonly DateTime _utcDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /*
         * ▓▓▓▒▒░░ Parser for Phase 2 ░░▒▒▓▓▓
         */
        /// <summary>
        /// The Parse.
        /// </summary>
        /// <param name="hexData">The hexData<see cref="string"/>.</param>
        /// <returns>The <see cref="TeltonikaTcpPacket"/>.</returns>
        public TeltonikaTcpPacket Parse(string hexData)
        {
            var result = new TeltonikaTcpPacket { HexMessage = hexData };
            var hc = new HexCrawler(hexData);

            try
            {
                /* ■■ Warning : Order of variables and assumptions is important.
                 * ■■           Don't reorder them.
                 */

                result.Preamble = hc.PopAsInt();
                result.DataFieldLength = hc.PopAsInt();
                result.Codec = hc.PopAsByte();
                result.NumberOfData1 = hc.PopAsByte();

                result.AvlData = new List<TeltonikaAvlData>();

                for (var i = 0; i < result.NumberOfData1; i++)
                {
                    var avl = new TeltonikaAvlData
                    {
                        Time = _utcDate.AddMilliseconds(hc.PopAsLong()).ToUniversalTime(),
                        Priority = (Priority)hc.PopAsByte(),
                        Longitude = hc.PopAsInt() / 10000000d,
                        Latitude = hc.PopAsInt() / 10000000d,
                        Altitude = hc.PopAsShort(),
                        Angle = hc.PopAsShort(),
                        Satellites = hc.PopAsByte(),
                        Speed = hc.PopAsShort(),

                        /*
                         * Event IO ID – if data is acquired on event – this field defines which IO property has changed and generated an event.
                         * For example, when ignition state changed and it generate event, Event IO ID will be 0xEF (AVL ID: 239).
                         * If it’s not eventual record – the value is 0.
                         */
                        EventIOID = hc.PopAsByte(),

                        /* N – a total number of properties coming with record (N = N1 + N2 + N4 + N8). */
                        TotalIOElement = hc.PopAsByte(),


                        IoElements = new List<TeltonikaIoElement>()
                    };

                    /* N1 – number of properties, which length is 1 byte. */
                    var N1 = hc.PopAsByte();
                    for (var j = 0; j < N1; j++)
                    {
                        var element = new TeltonikaIoElement
                        {
                            IoID = hc.PopAsByte(),
                            IoValue = hc.PopAsByte()
                        };
                        avl.IoElements.Add(element);
                    }

                    /* N2 – number of properties, which length is 2 bytes. */
                    var N2 = hc.PopAsByte();
                    for (var j = 0; j < N2; j++)
                    {
                        var element = new TeltonikaIoElement
                        {
                            IoID = hc.PopAsByte(),
                            IoValue = hc.PopAsShort()
                        };
                        avl.IoElements.Add(element);
                    }

                    /* N4 – number of properties, which length is 4 bytes. */
                    var N4 = hc.PopAsByte();
                    for (var j = 0; j < N4; j++)
                    {
                        var element = new TeltonikaIoElement
                        {
                            IoID = hc.PopAsByte(),
                            IoValue = hc.PopAsInt()
                        };
                        avl.IoElements.Add(element);
                    }

                    /* N8 – number of properties, which length is 8 bytes. */
                    var N8 = hc.PopAsByte();
                    for (var j = 0; j < N8; j++)
                    {
                        var element = new TeltonikaIoElement
                        {
                            IoID = hc.PopAsByte(),
                            IoValue = hc.PopAsLong()
                        };
                        avl.IoElements.Add(element);
                    }

                    result.AvlData.Add(avl);
                }

                /*
                 A number which defines how many records is in the packet. 
                 This number must be the same as “Number of Data 1”.
                 */
                result.NumberOfData2 = hc.PopAsByte();

                /*
                 CRC - 16 – calculated from Codec ID to the Second Number of Data.
                 CRC(Cyclic Redundancy Check) is an error - detecting code using for detect accidental changes to RAW data.
                 For calculation we are using CRC-16 / IBM. 
                 */
                result.CRC16 = hc.PopAsInt();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
