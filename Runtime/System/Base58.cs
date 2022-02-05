using System;
using System.Linq;
using System.Numerics;


namespace Ipfs
{
    /// <summary>
    ///   A codec for IPFS Base-58.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   A codec for Base-58, <see cref="Encode"/> and <see cref="Decode"/>.  Adds the extension method <see cref="ToBase58"/>
    ///   to encode a byte array and <see cref="FromBase58"/> to decode a Base-58 string.
    ///   </para>
    ///   <para>
    ///   This is just thin wrapper of <see href="https://github.com/ssg/SimpleBase"/>.
    ///   </para>
    ///   <para>
    ///   This codec uses the BitCoin alphabet <b>not Flickr's</b>.
    ///   </para>
    /// </remarks>
    public static class Base58
    {
        
        private static readonly string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        
        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-58 characters.
        /// </summary>s
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 58, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string Encode(byte[] bytes)
        {
            // Decode byte[] to BigInteger
            BigInteger intData = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                intData = intData * 256 + bytes[i];
            }

            // Encode BigInteger to Base58 string
            string result = "";
            while (intData > 0)
            {
                int remainder = (int)(intData % 58);
                intData /= 58;
                result = alphabet[remainder] + result;
            }

            // Append `1` for each leading 0 byte
            for (int i = 0; i < bytes.Length && bytes[i] == 0; i++)
            {
                result = '1' + result;
            }
            return result;
        }

        /// <summary>
        ///   Converts an array of 8-bit unsigned integers to its equivalent string representation that is 
        ///   encoded with base-58 digits.
        /// </summary>
        /// <param name="bytes">
        ///   An array of 8-bit unsigned integers.
        /// </param>
        /// <returns>
        ///   The string representation, in base 58, of the contents of <paramref name="bytes"/>.
        /// </returns>
        public static string ToBase58(this byte[] bytes)
        {
            return Encode(bytes);
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 58 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 58 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] Decode(string s)
        {
            // Decode Base58 string to BigInteger 
            BigInteger intData = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int digit = alphabet.IndexOf(s[i]); //Slow
                if (digit < 0)
                    throw new FormatException(string.Format("Invalid Base58 character `{0}` at position {1}", s[i], i));
                intData = intData * 58 + digit;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            int leadingZeroCount = s.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
                intData.ToByteArray()
                    .Reverse()// to big endian
                    .SkipWhile(b => b == 0);//strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
            return result;
        }

        /// <summary>
        ///   Converts the specified <see cref="string"/>, which encodes binary data as base 58 digits, 
        ///   to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">
        ///   The base 58 string to convert.
        /// </param>
        /// <returns>
        ///   An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.
        /// </returns>
        public static byte[] FromBase58(this string s)
        {
            return Decode(s);
        }
    }
}
