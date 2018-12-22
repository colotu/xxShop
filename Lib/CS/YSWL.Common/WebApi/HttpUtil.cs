using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace YSWL.Common.WebApi
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public class HttpUtil
    {
        /// <summary>
        /// 请求http方法
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="data">请求的参数</param>
        /// <param name="method">请求方式(post/get)</param>
        /// <returns>请求内容</returns>
        public static string PostDataToServer(string url, string data, string key = null, string method = "POST", bool isGzip = false)
        {
            HttpWebRequest request = null;
            string result = "";
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 100000;
                request.KeepAlive = true;
                System.Net.ServicePointManager.Expect100Continue = false;
                if (!string.IsNullOrEmpty(key))
                {
                    request.Headers.Add("Enterprise", key);
                }
                switch (method.ToUpper())
                {
                    case "GET":
                        request.Method = "GET";
                        break;
                    case "POST":
                        {
                            request.Method = "POST";
                            if (isGzip)
                            {
                                request.Headers.Add("Accept-Encoding", "gzip");
                                request.AutomaticDecompression = DecompressionMethods.GZip;
                            }

                            if (!string.IsNullOrEmpty(data))
                            {
                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/json;charset=utf-8";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                        }
                        break;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamIn = response.GetResponseStream();
                if (isGzip)
                {
                    var cmpTypes = response.Headers.GetValues("Content-Encoding");
                    using (GZipStream steam = new GZipStream(streamIn, CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(steam))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(streamIn))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                streamIn.Close();
                response.Close();

                return result;
            }
            catch(Exception ex)
            {
                YSWL.Log.LogHelper.AddTextLog("API调用类", "错误:" + ex.Message + "-----" +ex.StackTrace +string.Format("url:{0},data:{1},key:{2}",url,data,key));
                throw;
            }
            finally
            {

            }
        }
    }
}