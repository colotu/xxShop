using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using YSWL.Json.Conversion;
using YSWL.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using YSWL.Common;
using YSWL.Components;

namespace YSWL.WeChat.BLL.Core
{
   public class Utils
    {
        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


       #region  辅助方法
       /// <summary>
        /// 获取授权码
       /// </summary>
       /// <param name="AppId"></param>
       /// <param name="AppSecret"></param>
       /// <returns></returns>
        public static string GetToken(string AppId,string AppSecret)
        {
            string CacheKey = "WeChatToken-" + AppId + AppSecret;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    string tokenUrl =  String.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppId,  AppSecret);
                    StreamReader reader = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
                        HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                        reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                        string content = reader.ReadToEnd();//得到结果
                        YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                        if (jsonObject["errcode"] != null)
                        {
                            return jsonObject["errcode"].ToString();
                        }
                        else
                        {
                            objModel = jsonObject["access_token"];
                            if (objModel != null)
                            {
                                int ModelCache =120;
                                YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch { }
            }
            return objModel.ToString();
        }
       /// <summary>
       /// 获取用户OpenId
       /// </summary>
       /// <param name="AppId"></param>
       /// <param name="AppSecret"></param>
       /// <param name="Code"></param>
       /// <returns></returns>
        public static string GetUserOpenId(string AppId, string AppSecret, string Code)
        {
            string tokenUrl =
            String.Format(
                "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AppId,
                AppSecret,Code);
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                if (jsonObject["openid"] != null)
                {
                    return jsonObject["openid"].ToString();
                }
                return jsonObject.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
       /// <summary>
        /// 获取Ticket
       /// </summary>
       /// <param name="access_token"></param>
       /// <returns></returns>
        public static string GetTicket(string access_token,int sceneId)
        {
            StreamReader reader = null;
            Stream newStream = null;

            string posturl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            JsonObject json = new JsonObject();
            json.Accumulate("action_name", "QR_LIMIT_SCENE");
            JsonObject scene = new JsonObject();
            scene.Accumulate("scene_id", sceneId);
            JsonObject info = new JsonObject();
            info.Accumulate("scene", scene);
            json.Accumulate("action_info", info);

            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(json.ToString());
            request.ContentLength = postdata.Length;
            newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
            return jsonObject["ticket"] == null ? "" :Common.Globals.UrlEncode(jsonObject["ticket"].ToString());
        }
       /// <summary>
       /// 获取媒体文件ID
       /// </summary>
       /// <param name="access_token"></param>
       /// <param name="url"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        public static string GetMediaId(string access_token, string url, string type)
        {
            string CacheKey = "WeChat-GetMediaId" + access_token+ url+ type;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    string posturl = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
                    System.Net.WebClient webclient = new System.Net.WebClient();
                    string media =url;
                    byte[] result = webclient.UploadFile(new Uri(String.Format(posturl, access_token, type)), media);

                    string content = Encoding.Default.GetString(result);
                    YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                    if (jsonObject != null)
                    {
                        objModel = jsonObject["media_id"] == null ? "" : jsonObject["media_id"].ToString();
                        if (objModel != null)
                        {
                            YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddDays(3), TimeSpan.Zero);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return  objModel.ToString();
       
        }

        public static string GetWCUrl(YSWL.WeChat.Model.Core.RequestMsg msg, string returnUrl,bool isRepeat=true)
        {
            string baseUrl = "http://" + Common.Globals.DomainFullName + "/wcreturn.aspx?returnUrl={0}&mp={1}&rep={2}";

            if (msg == null || String.IsNullOrWhiteSpace(returnUrl))
            {
                return "";
            }
            if (returnUrl.Contains("http://"))
            {
               return returnUrl ;
            }
           
            string key = YSWL.Common.DEncrypt.DESEncrypt.Encrypt(msg.UserName + "|" + msg.OpenId);
            string weChatLink = String.Format(baseUrl, Common.Globals.UrlEncode(returnUrl), key, isRepeat);
            //加入微信链接地址表
            if (!isRepeat)
            {
                YSWL.WeChat.BLL.Core.LinkLog.Add(weChatLink);
            }
            return weChatLink;
        }

        public static string GetWCUrl(string openId, string userName, string returnUrl, bool isRepeat = true)
        {
            string baseUrl = "http://" + Common.Globals.DomainFullName + "/wcreturn.aspx?returnUrl={0}&mp={1}&rep={2}";

            if (String.IsNullOrWhiteSpace(returnUrl))
            {
                return "";
            }
            if (returnUrl.Contains("http://"))
            {
                return returnUrl;
            }
            string key = YSWL.Common.DEncrypt.DESEncrypt.Encrypt(userName + "|" + openId);
            string weChatLink = String.Format(baseUrl, Common.Globals.UrlEncode(returnUrl), key, isRepeat);
            //加入微信链接地址表
            if (!isRepeat)
            {
                YSWL.WeChat.BLL.Core.LinkLog.Add(weChatLink);
            }
            return weChatLink;
        }

        public static string GetDescUrl(YSWL.WeChat.Model.Core.RequestMsg msg, string desc, bool isRepeat = true)
        {
            
            string tmpStr = string.Format("<{0}[^>]*?{1}=(['\"\"]?)(?<url>[^'\"\"\\s>]+)\\1[^>]*>", "a", "href");
            System.Text.RegularExpressions.Match TitleMatch = Regex.Match(desc, tmpStr, RegexOptions.IgnoreCase);
            string returnUrl = TitleMatch.Groups["url"].Value;
            string baseUrl = "http://" + Common.Globals.DomainFullName + "/wcreturn.aspx?returnUrl={0}&mp={1}&rep={2}";
            
            string key = YSWL.Common.DEncrypt.DESEncrypt.Encrypt(msg.UserName + "|" + msg.OpenId);
            string weChatLink = String.Format(baseUrl, Common.Globals.UrlEncode(returnUrl), key, isRepeat);
            if (!isRepeat)
            {
                YSWL.WeChat.BLL.Core.LinkLog.Add(weChatLink);
            }
            return String.IsNullOrWhiteSpace(returnUrl) ? desc : desc.Replace(returnUrl, weChatLink);
        }

        /// <summary>
        /// 获取模版信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetTemplate(string access_token)
        {
            string CacheKey = "WeChat-GetTemplate" + access_token ;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            StreamReader reader = null;
            if (objModel == null)
            {
                try
                {
                    string posturl = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + access_token;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
                    HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                    reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();//得到结果
                    YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                   
                    return jsonObject.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objModel.ToString();

        }

       public static bool SendTemplate(string access_token, string userOpen, string templateId,string user,string nick,string url)
       {
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
                request.Method = "POST";

                #region   构造模板消息
                JsonObject json = new JsonObject();
                json.Accumulate("touser", userOpen);
                json.Accumulate("template_id", templateId);
                json.Accumulate("url", url);
                JsonObject data = new JsonObject();
                //json.Accumulate("data");
              
                JsonObject userjson = new JsonObject();
                userjson.Accumulate("value", user);
                data.Accumulate("User", userjson);
                JsonObject nickjson = new JsonObject();
                nickjson.Accumulate("value", nick);
                data.Accumulate("Nick", nickjson);
                JsonObject dateJson = new JsonObject();
                dateJson.Accumulate("value", DateTime.Now.ToString("yyyy-MM-DD"));
                data.Accumulate("Date", dateJson);
                json.Accumulate("data", data);
                #endregion 

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string postData = json.ToString();
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(postData);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//得到结果
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                int code = Common.Globals.SafeInt(jsonObject["errcode"].ToString(), 0);
                string errmsg = jsonObject["errmsg"].ToString();
                return code==0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }

       #endregion

    }
}
