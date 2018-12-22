using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Threading;
using System.Globalization;

namespace YSWL.Common
{
    public class PageLoader
    {
        private static Encoding _encoding = Encoding.Default;
        private static readonly String strAgentInfo = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

        #region 下载某个网址的页面内容
        /// <summary>
        /// 下载某个网址的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>返回页面内容</returns>
        public static String Download(String url)
        {
            return Download(url, strAgentInfo);
        } 
        #endregion

        #region 下载某个网址的页面内容
        /// <summary>
        /// 下载某个网址的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="agentInfo">客户端信息</param>
        /// <returns>返回页面内容</returns>
        public static String Download(String url, String agentInfo)
        {
            String str;
            try
            {
                WebResponse res = getResponse(url, agentInfo);

                StreamReader reader = new StreamReader(res.GetResponseStream(), getEncoding(res));
                str = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="agentInfo"></param>
        /// <returns></returns>
        private static WebResponse getResponse(String url, String agentInfo)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = agentInfo;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request.GetResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static Encoding getEncoding(WebResponse response)
        {
            try
            {
                String contentType = response.ContentType;
                if (contentType == null)
                {
                    return _encoding;
                }
                String[] strArray = contentType.ToLower(CultureInfo.InvariantCulture).Split(new char[] { ';', '=', ' ' });
                Boolean isFind = false;
                foreach (String item in strArray)
                {
                    if (item == "charset")
                    {
                        isFind = true;
                    }
                    else if (isFind)
                    {
                        return Encoding.GetEncoding(item);
                    }
                }
            }
            catch (Exception exception)
            {
                if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
            }
            return _encoding;
        }
    }
}
