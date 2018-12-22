using ServiceStack.DesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ServiceStack.Common.Web;

namespace YSWL.RedisClientManager
{
    /// <summary>
    /// redis扩展
    /// </summary>
    public static class RedisExtensionsExt
    {
        public static string[] GetIds(this IHasStringId[] itemsWithId)
        {
            string[] strArray = new string[itemsWithId.Length];
            for (int i = 0; i < itemsWithId.Length; i++)
            {
                strArray[i] = itemsWithId[i].Id;
            }
            return strArray;
        }

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return (!socket.Poll(1, SelectMode.SelectRead) || (socket.Available != 0));
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public static List<EndPoint> ToIpEndPoints(this IEnumerable<string> hosts)
        {
            if (hosts == null)
            {
                return new List<EndPoint>();
            }
            List<EndPoint> list = new List<EndPoint>();
            foreach (string str in hosts)
            {
                string[] strArray = str.Split(new char[] { ':' });
                if (strArray.Length == 0)
                {
                    throw new ArgumentException("'{0}' is not a valid Host or IP Address: e.g. '127.0.0.0[:11211]'");
                }
                int port = (strArray.Length == 1) ? 0x18eb : int.Parse(strArray[1]);
                EndPoint item = new EndPoint(strArray[0], port);
                list.Add(item);
            }
            return list;
        }

        public static List<string> ToStringList(this byte[][] multiDataList)
        {
            if (multiDataList == null)
            {
                return new List<string>();
            }
            List<string> list = new List<string>();
            foreach (byte[] buffer in multiDataList)
            {
                list.Add(buffer.FromUtf8Bytes());
            }
            return list;
        }
    }
}
