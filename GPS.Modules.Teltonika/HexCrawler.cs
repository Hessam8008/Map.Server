// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="HexCrawler.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika
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
        /// Initializes a new instance of the <see cref="HexCrawler" /> class.
        /// </summary>
        /// <param name="hex">The hex<see cref="string" />.</param>
        public HexCrawler(string hex)
        {
            this.hexData = hex;
            this.hexBytes = HexString2Bytes(hex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexCrawler" /> class.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte" />.</param>
        public HexCrawler(byte[] bytes)
        {
            this.hexBytes = bytes;
            this.hexData = Bytes2HexString(bytes);
        }

        /// <summary>
        /// The PopAsString.
        /// </summary>
        /// <param name="length">The length<see cref="int" />.</param>
        /// <returns>The <see cref="string" />.</returns>
        public string PopAsString(int length)
        {
            var bytes = this.GetBytes(length);
            var result = Encoding.ASCII.GetString(bytes);
            return result;
        }

        /// <summary>
        /// The PopAsShort.
        /// </summary>
        /// <returns>The <see cref="short" />.</returns>
        public short PopAsShort()
        {
            var hex = this.GetHex(2);
            var result = short.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// Pop a byte.
        /// </summary>
        /// <returns>The <see cref="byte" />.</returns>
        public byte PopAsByte()
        {
            return this.GetBytes(1)[0];
        }

        /// <summary>
        /// Pop a value of four bytes as integer.
        /// </summary>
        /// <returns>The <see cref="int" />.</returns>
        public int PopAsInt()
        {
            var hex = this.GetHex(4);
            var result = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// Pop a value of eight bytes as long.
        /// </summary>
        /// <returns>The <see cref="long" />.</returns>
        public long PopAsLong()
        {
            var hex = this.GetHex(8);
            var result = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        /// <summary>
        /// Convert bytes to hex string.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte" />.</param>
        /// <returns>The <see cref="string" />.</returns>
        private static string Bytes2HexString(byte[] bytes)
        {
            var result = BitConverter.ToString(bytes).Replace("-", string.Empty);
            return result;
        }

        /// <summary>
        /// Convert hex string to byte.
        /// </summary>
        /// <param name="hex">Hex string<see cref="string" />.</param>
        /// <returns>The <see cref="byte" />.</returns>
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

        /// <summary>
        /// Get bytes from last position.
        /// </summary>
        /// <param name="bytesCount">Count of bytes to return<see cref="int" />.</param>
        /// <returns>The <see cref="byte" />.</returns>
        private byte[] GetBytes(int bytesCount)
        {
            var endIndex = this.pointer + bytesCount;
            var bytes = this.hexBytes[new Range(this.pointer, endIndex)];
            this.pointer += bytesCount;
            return bytes;
        }

        /// <summary>
        /// Get hex string from last position.
        /// </summary>
        /// <param name="bytesCount">Count of bytes to return<see cref="int" />.</param>
        /// <returns>The <see cref="string" />.</returns>
        private string GetHex(int bytesCount)
        {
            var result = this.hexData.Substring(this.pointer * 2, bytesCount * 2);
            this.pointer += bytesCount;
            return result;
        }
    }
}