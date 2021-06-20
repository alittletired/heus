using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Heus.Util
{
    /// <summary>
    /// 哈希算法工具类，提供哈希算法包装。
    /// </summary>
    public static class HashUtils
    {

        /// <summary>
        /// 使用指定哈希算法，指定编码，计算哈希值并返回字符串结果
        /// </summary>
        /// <param name="algorithm">具体的哈希算法实现类</param>
        /// <param name="input">输入字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>哈希值</returns>
        private static string ComputeHash(HashAlgorithm algorithm, string input, Encoding encoding)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            //空字符串是有哈希值的。这里只能判断null
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            byte[] buffer = encoding.GetBytes(input);

            byte[] result = algorithm.ComputeHash(buffer);

            return BitConverter.ToString(result).Replace("-", "");
        }

        /// <summary>
        /// 使用MD5算法，计算一个字符串的哈希值。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <param name="encoding">编码方式。</param>
        /// <returns>MD5哈希值。</returns>
        public static string MD5(string input, Encoding encoding)
        {
            return ComputeHash(new MD5CryptoServiceProvider(), input, encoding);
        }

        /// <summary>
        /// 使用MD5算法，UTF-8编码，计算一个字符串的哈希值。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <returns>MD5哈希值。</returns>
        public static string MD5(string input)
        {
            return MD5(input, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 使用SHA1算法，计算一个字符串的哈希值。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <param name="encoding">编码方式。</param>
        /// <returns>SHA1哈希值。</returns>
        public static string SHA1(string input, Encoding encoding)
        {
            return ComputeHash(new SHA1CryptoServiceProvider(), input, encoding);
        }

        /// <summary>
        /// 使用SHA1算法，UTF-8编码，计算一个字符串的哈希值。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <returns>SHA1哈希值。</returns>
        public static string SHA1(string input)
        {
            return SHA1(input, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 使用Time33算法进行hash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Time33Hash(string str)
        {
            if (str == null)
                return string.Empty;

            int hash = 5381;
            for (int i = 0, len = str.Length; i < len; ++i)
            {
                char[] sub = str.Substring(i, 1).ToCharArray();
                hash += (hash << 5) + sub[0];
            }
            hash &= 0x7fffffff;
            return hash.ToString();
        }
    }
}
