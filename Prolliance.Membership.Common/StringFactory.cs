using System;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace Prolliance.Membership.Common
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class StringFactory
    {
        private static string SOLT = ConfigurationManager.AppSettings["SOLT"] ?? "Membership";

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string HashBySolt(string str)
        {
            str = (str ?? "") + SOLT;
            return Hash(str);
        }
        public static string Hash(string str)
        {
            str = (str ?? "");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string NewStr32()
        {
            return Hash(NewGuid());
        }
        public static string NewStr16()
        {
            return NewStr32().Substring(8, 16);
        }

    }
}
