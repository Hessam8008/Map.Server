using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GpsServer.Teltonika.Server.Args;

namespace GpsServer.Teltonika.Client
{
    public delegate void OnAuthenticated(object sender, AuthenticatedArgs e);
    public delegate void OnPacketReceived(object sender, PacketReceivedArgs e);
    public delegate void OnError(object sender, ErrorArgs e);
    public delegate void OnDisconnected(object sender, EventArgs e);


    public class MyDevice
    {
        public event OnAuthenticated Authenticated;
        public event OnPacketReceived PacketReceived;
        public event OnError Error;
        public event OnDisconnected Disconnected;

        private readonly TcpClient client;

        public MyDevice(TcpClient client)
        {
            this.client = client;
        }

        public async Task GetData()
        {
            try
            {
                var bytes = new byte[2048];
                var stream = client.GetStream();

                /*
                 * --► PHASE 01 ◄--
                 */
                var counter = await stream.ReadAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

                /* First 2 bytes are IMEI length + next 15 bytes are IMEI  == 17 bytes */
                if (counter != 17)
                {
                    Error?.Invoke(this, new ErrorArgs("Authentication failed."));
                    return;
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", "");
                var imeiLen = int.Parse(hexImeiLen, NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    Error?.Invoke(this, new ErrorArgs($"Invalid IMEI length. IMEI must be 15 but it is {imeiLen}."));
                    return;
                }

                /* Decode IMEI */
                var imei = Encoding.ASCII.GetString(bytes, 2, 15);

                var connectedArg = new AuthenticatedArgs(imei);
                Authenticated?.Invoke(this, connectedArg);

                if (!connectedArg.Accepted)
                {
                    await stream.WriteAsync(BitConverter.GetBytes(0)).ConfigureAwait(false);
                    return;
                }

                /* Reply acknowledge byte (01 for accept, 00 for reject) */
                await stream.WriteAsync(BitConverter.GetBytes(1)).ConfigureAwait(false);

                /*
                 * --► PHASE 02 ◄--
                 */
                while ((counter = await stream.ReadAsync(bytes, 0, bytes.Length).ConfigureAwait(false)) != 0)
                {
                    var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", "");
                    var parser = new FmxParserCodec8();
                    var data = parser.Parse(hexData);
                    data.IMEI = imei;
                    var packetArgs = new PacketReceivedArgs(data);
                    PacketReceived?.Invoke(this, packetArgs);
                    if (packetArgs.Accepted)
                        await stream.WriteAsync(BitConverter.GetBytes((int)data.NumberOfData1)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, new ErrorArgs(ex));
            }
            finally
            {
                CloseConnection();
            }
        }

        private void CloseConnection()
        {
            try
            {
                client?.Close();
                client?.Dispose();
                Disconnected?.Invoke(this, new EventArgs());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
