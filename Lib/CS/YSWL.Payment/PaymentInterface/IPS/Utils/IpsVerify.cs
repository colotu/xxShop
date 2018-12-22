using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// MD5加密类
/// </summary>
public class IpsVerify
{
    private static object lockRsaVerify = new object();
    public static RSACryptoServiceProvider rsaVerify;

    public IpsVerify()
    {
    }

    /// <summary>
    /// 取得输入字符串的MD5哈希值
    /// </summary>
    /// <param name="argInput">输入字符串</param>
    /// <returns>MD5哈希值</returns>
    public static string MD5Sign(string argInput)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(argInput));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }


    /// <summary>
    /// MD5
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsMD5Sign(string argSign, string argContent)
    {
        string strMd5Sign = MD5Sign(argContent);
        if (strMd5Sign.Equals(argSign))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Md5WithRsa
    /// </summary>
    /// <param name="argPubKey">公钥</param>
    /// <param name="content">内容</param>
    /// <param name="signmessage">签名</param>
    /// <returns></returns>
    public static bool VerifyMessageByKeyUTF8(string argPubKey, string content, string signmessage)
    {
        try
        {
            lock (lockRsaVerify)
            {
                rsaVerify = GetX509PublicKey(argPubKey);
                if (rsaVerify == null)
                {
                    return false;
                }
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                byte[] signature = new byte[signmessage.Length / 2];
                int startIndex = 0;
                for (int i = 0; startIndex < signmessage.Length; i++)
                {
                    signature[i] = byte.Parse(signmessage.Substring(startIndex, 2), NumberStyles.HexNumber);
                    startIndex += 2;
                }
                return rsaVerify.VerifyData(bytes, new MD5CryptoServiceProvider(), signature);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private static RSACryptoServiceProvider GetX509PublicKey(string key)
    {
        byte[] buffer = DecodeOpenSSLPublicKey(key);
        if (buffer != null)
        {
            return DecodeX509PublicKey(buffer);
        }
        return null;
    }
    public static byte[] DecodeOpenSSLPublicKey(string instr)
    {
        byte[] buffer;
        string str = instr.Trim();
        if (!(str.StartsWith("-----BEGIN PUBLIC KEY-----") && str.EndsWith("-----END PUBLIC KEY-----")))
        {
            return null;
        }
        StringBuilder builder = new StringBuilder(str);
        builder.Replace("-----BEGIN PUBLIC KEY-----", "");
        builder.Replace("-----END PUBLIC KEY-----", "");
        string s = builder.ToString().Trim();
        try
        {
            buffer = Convert.FromBase64String(s);
        }
        catch (FormatException)
        {
            return null;
        }
        return buffer;
    }

    public static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
    {
        RSACryptoServiceProvider provider;
        byte[] b = new byte[] { 0x30, 13, 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 1, 1, 5, 0 };
        MemoryStream input = new MemoryStream(x509key);
        BinaryReader reader = new BinaryReader(input);
        ushort num = 0;
        try
        {
            switch (reader.ReadUInt16())
            {
                case 0x8130:
                    reader.ReadByte();
                    break;

                case 0x8230:
                    reader.ReadInt16();
                    break;

                default:
                    return null;
            }
            if (CompareBytearrays(reader.ReadBytes(15), b))
            {
                switch (reader.ReadUInt16())
                {
                    case 0x8103:
                        reader.ReadByte();
                        goto Label_00AB;

                    case 0x8203:
                        reader.ReadInt16();
                        goto Label_00AB;
                }
            }
            return null;
        Label_00AB:
            if (reader.ReadByte() == 0)
            {
                switch (reader.ReadUInt16())
                {
                    case 0x8130:
                        reader.ReadByte();
                        goto Label_00F5;

                    case 0x8230:
                        reader.ReadInt16();
                        goto Label_00F5;
                }
            }
            return null;
        Label_00F5:
            num = reader.ReadUInt16();
            byte num2 = 0;
            byte num3 = 0;
            switch (num)
            {
                case 0x8102:
                    num2 = reader.ReadByte();
                    break;

                case 0x8202:
                    num3 = reader.ReadByte();
                    num2 = reader.ReadByte();
                    break;

                default:
                    return null;
            }
            byte[] buffer2 = new byte[4];
            buffer2[0] = num2;
            buffer2[1] = num3;
            byte[] buffer3 = buffer2;
            int count = BitConverter.ToInt32(buffer3, 0);
            if (reader.PeekChar() == 0)
            {
                reader.ReadByte();
                count--;
            }
            byte[] buffer4 = reader.ReadBytes(count);
            if (reader.ReadByte() != 2)
            {
                return null;
            }
            int num5 = reader.ReadByte();
            byte[] buffer5 = reader.ReadBytes(num5);
            RSACryptoServiceProvider provider2 = new RSACryptoServiceProvider();
            RSAParameters parameters = new RSAParameters
            {
                Modulus = buffer4,
                Exponent = buffer5
            };
            provider2.ImportParameters(parameters);
            provider = provider2;
        }
        catch (Exception)
        {
            provider = null;
        }
        finally
        {
            reader.Close();
        }
        return provider;
    }

    private static bool CompareBytearrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        int index = 0;
        foreach (byte num2 in a)
        {
            if (num2 != b[index])
            {
                return false;
            }
            index++;
        }
        return true;
    }
}
