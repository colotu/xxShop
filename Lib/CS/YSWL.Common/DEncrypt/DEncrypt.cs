using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace YSWL.Common.DEncrypt
{
    /// <summary>
    /// Encrypt 的摘要说明。
    /// </summary>
    public class DEncrypt
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public DEncrypt()
        {
        }

        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "YSWL");
        }
        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "YSWL", System.Text.Encoding.Default);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string
        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }
        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }
        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]
        /// <summary>
        /// 使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("YSWL");
            return Decrypt(encrypted, key);
        }
        /// <summary>
        /// 使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("YSWL");
            return Encrypt(original, key);
        }
        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }
        /// <summary>
        /// 生成MD5加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5FromStr(string str)
        {
            byte[] by = System.Text.Encoding.Default.GetBytes(str); //将字符串读到字节数组中
            byte[] by1 = MD5.Create().ComputeHash(by);
            StringBuilder builder = new StringBuilder();//可变的字符串 用来存放最终的MD5值
            for (int i = 0; i < by1.Length; i++)
            {
                builder.Append(by1[i].ToString("x2"));  //by1[i].ToString ("x2")是设定格式
            }
            return builder.ToString();
        }
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        #endregion


        /// <summary>
        /// 获取企业加密串
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <param name="sjc"></param>
        /// <returns></returns>
        public static string GetEncryptionStr(long enterpriseId)
        {
            string temp = ((enterpriseId + 31) * 19) + "";
            int lenth = temp.ToString().Length;

            string[] strs = new string[] { };

            if (lenth > 3 && lenth <= 6)
            {
                lenth = 4;
            }
            else if (lenth > 6)
            {
                lenth = 5;
            }

            StringBuilder sb = new StringBuilder();
            int i = 0;
            for (; i < lenth; i++)
            {
                var t = int.Parse(temp.Substring(i, 1));
                //根据数字大小获取字符串  
                if (t <= 5)
                {
                    sb.Append(t + GetRandomStr(1, t));
                }
                else
                {
                    sb.Append(t + GetRandomStr(2, t));
                }
                //0-5 取1  6-9取2 
            }
            sb.Append(temp.Substring(i));
            return sb.ToString();
        }

        /// <summary>
        /// 获取随机串
        /// </summary>
        /// <param name="codeCount">获取多少位随机串</param>
        /// <param name="number">原串中截取的数字</param>
        /// <returns></returns>
        public static string GetRandomStr(int codeCount, int number)
        {
            string allChar = "c d e f A B I J K T U V x y W X a b g L M N O h i E j k l C D F G H m n o p P Q R S q r s t Y Z u v w z";
            string[] allCharArray = allChar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string randomCode = "";
            Random rand = new Random(codeCount * ((int)(DateTime.Now.AddMonths(number).Ticks)));
            int temp = rand.Next(43);
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(43);
                if (temp == t)
                {
                    return GetRandomStr(codeCount, number);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        /// <summary>
        /// 强转数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ConvertToNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            string regex = @"[0-9]";
            Regex rgClass = new Regex(regex, RegexOptions.Singleline);

            MatchCollection matchs = rgClass.Matches(str);
            StringBuilder sb = new StringBuilder();
            foreach (var item in matchs)
            {
                sb.Append(item);
            }
            if (String.IsNullOrWhiteSpace(sb.ToString()))
            {
                return 0;
            }
            double temp = double.Parse(sb + "") / 19 - 31;
            if (temp <= 0 || (temp + "").IndexOf('.') >= 0)
            {
                return 0;
            }
            return long.Parse(temp + "");
        }
    }
}
