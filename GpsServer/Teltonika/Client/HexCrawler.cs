namespace GpsServer.Teltonika.Client
{
    using System;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="HexCrawler" />.
    /// </summary>
    public class HexCrawler
    {
        /// <summary>
        /// Defines the hexBytes.
        /// </summary>
        private readonly byte[] hexBytes;

        /// <summary>
        /// Defines the hexData.
        /// </summary>
        private readonly string hexData;

        /// <summary>
        /// Defines the pointer.
        /// </summary>
        private int pointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HexCrawler"/> class.
        /// </summary>
        /// <param name="hex">The hex<see cref="string"/>.</param>
        public HexCrawler(string hex)
        {
            hexData = hex;
            hexBytes = HexString2Bytes(hex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexCrawler"/> class.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        public HexCrawler(byte[] bytes)
        {
            hexBytes = bytes;
            hexData = Bytes2HexString(bytes);
        }

        /// <summary>
        /// The PopAsString.
        /// </summary>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string PopAsString(int length)
        {
            var bytes = getBytes(length);
            var result = Encoding.ASCII.GetString(bytes);
            return result;
        }

        /// <summary>
        /// The PopAsShort.
        /// </summary>
        /// <returns>The <see cref="short"/>.</returns>
        public short PopAsShort()
        {
            var hex = getHex(2);
            var result = short.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// The PopAsByte.
        /// </summary>
        /// <returns>The <see cref="byte"/>.</returns>
        public byte PopAsByte()
        {
            return getBytes(1)[0];
        }

        /// <summary>
        /// The PopAsInt.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public int PopAsInt()
        {
            var hex = getHex(4);
            var result = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// The PopAsLong.
        /// </summary>
        /// <returns>The <see cref="long"/>.</returns>
        public long PopAsLong()
        {
            var hex = getHex(8);
            var result = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// The getBytes.
        /// </summary>
        /// <param name="bytesCount">The bytesCount<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private byte[] getBytes(int bytesCount)
        {
            var endIndex = pointer + bytesCount;
            var bytes = hexBytes[new Range(pointer, endIndex)];
            pointer += bytesCount;
            return bytes;
        }

        /// <summary>
        /// The getHex.
        /// </summary>
        /// <param name="bytesCount">The bytesCount<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string getHex(int bytesCount)
        {
            var result = hexData.Substring(pointer * 2, bytesCount * 2);
            pointer += bytesCount;
            return result;
        }

        /// <summary>
        /// The Bytes2HexString.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string Bytes2HexString(byte[] bytes)
        {
            var result = BitConverter.ToString(bytes).Replace("-", "");
            return result;
        }

        /// <summary>
        /// The HexString2Bytes.
        /// </summary>
        /// <param name="hex">The hex<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
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
