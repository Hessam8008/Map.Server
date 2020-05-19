using System;
using System.Text;

namespace GpsServer.Teltonika.Client
{
    public class HexCrawler
    {
        private readonly byte[] hexBytes;
        private readonly string hexData;
        private int pointer;

        public HexCrawler(string hex)
        {
            hexData = hex;
            hexBytes = HexString2Bytes(hex);
        }

        public HexCrawler(byte[] bytes)
        {
            hexBytes = bytes;
            hexData = Bytes2HexString(bytes);
        }

        public string PopAsString(int length)
        {
            var bytes = getBytes(length);
            var result = Encoding.ASCII.GetString(bytes);
            return result;
        }
        public short PopAsShort()
        {
            var hex = getHex(2);
            var result = short.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }
        public byte PopAsByte()
        {
            return getBytes(1)[0];
        }
        public int PopAsInt()
        {
            var hex = getHex(4);
            var result = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }
        public long PopAsLong()
        {
            var hex = getHex(8);
            var result = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        private byte[] getBytes(int bytesCount)
        {
            var endIndex = pointer + bytesCount;
            var bytes = hexBytes[new Range(pointer, endIndex)];
            pointer += bytesCount;
            return bytes;
        }

        private string getHex(int bytesCount)
        {
            var result = hexData.Substring(pointer * 2, bytesCount * 2);
            pointer += bytesCount;
            return result;
        }

        private string Bytes2HexString(byte[] bytes)
        {
            var result = BitConverter.ToString(bytes).Replace("-", "");
            return result;
        }
        private static byte[] HexString2Bytes(string hex)
        {
            var raw = new byte[hex.Length / 2];
            for (var i = 0; i < raw.Length; i++)
            {
                var b = hex.Substring(i * 2, 2);
                raw[i] = Convert.ToByte(b, 16);
            }
            return raw;
        }
    }
}