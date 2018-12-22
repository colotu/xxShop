/**
* VideoHelper.cs
*
* 功 能： 网络视频帮助类
* 类 名： VideoHelper
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/24 15:37:36  蒋海滨    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using YSWL.Common;

namespace YSWL.Common.Video
{
    public class VideoHelper
    {
        /// <summary>
        /// 优库网返回json格式的视频信息的ApiUrl
        /// </summary>
        static string youKuJsonDataApiUrl = "http://v.youku.com/player/getPlayList/VideoIDS/{0}/version/5/source/out?onData=%5Btype%20Function%5D&n=3";
        /// <summary>
        /// 土豆网返回xml格式的视频信息的数据ApiUrl
        /// </summary>
        // string tuDouXmlDataApiUrl = "http://api.tudou.com/v3/gw?method=item.info.get&appKey={0}&format=xml&itemCodes={1}";
        /// <summary>
        /// 土豆网返回json格式的视频信息的ApiUrl
        /// </summary>
        //static string tuDouJsonDataApiUrl = "http://api.tudou.com/v3/gw?method=item.info.get&appKey={0}&format=json&itemCodes={1}";
        /// <summary>
        /// 土豆密钥
        /// </summary>
        //static string tuDouAppKey = "7ee7a34f14a4c74b";
        /// <summary>
        /// 酷6网返回json格式的视频信息的ApiUrl
        /// </summary>
        static string ku6XmlDataApiUrl = "http://v.ku6.com/repaste.htm?url={0}";

        public VideoHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region 判断是否为优酷视频链接
        /// <summary>
        /// 判断是否为优酷视频链接
        /// </summary>
        /// <returns></returns>
        public static bool IsYouKuVideoUrl(string url)
        {
            if (url.StartsWith("http://v.youku.com/v_show/id_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 得到优库视频ID
        /// <summary>
        /// 得到优库视频ID
        /// </summary>
        public static string GetYouKuVideoId(string url)
        {
            String vid = "";
            //if (StringPlus.IsNullOrEmpty(url) || !IsYouKuVideoUrl(url))
            //{
            //    return "";
            //}
            if (StringPlus.IsNullOrEmpty(url))
            {
                return "";
            }
            vid = StringPlus.TrimStart(url, "http://v.youku.com/v_show/id_");
            if (StringPlus.IsNullOrEmpty(vid))
            {
                return "";
            }
            int FirstPoint = vid.IndexOf(".html");
            if (FirstPoint > 0)
            {
                vid = vid.Substring(0, FirstPoint);
            }
            return vid;
        }
        #endregion

        #region 得到优库视频信息

        /// <summary>
        /// 得到优库视频信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static YouKuInfo GetYouKuInfo(string url)
        {
            string vid = GetYouKuVideoId(url);
            if (StringPlus.IsNullOrEmpty(vid))
            {
                return null;
            }

            string jsonData = PageLoader.Download(string.Format(youKuJsonDataApiUrl, vid));
            if (StringPlus.IsNullOrEmpty(jsonData))
            {
                return null;
            }

            string strStart = "{\"data\":[";
            if (jsonData.StartsWith(strStart))
            {
                jsonData = jsonData.Replace(strStart, "");
            }

            string strEnd = "}";
            if (jsonData.EndsWith(strEnd))
            {
                int FirstPoint = jsonData.LastIndexOf(strEnd);
                if (FirstPoint > 0)
                {
                    jsonData = jsonData.Substring(0, FirstPoint);
                }
            }

            YouKuInfo info = (YouKuInfo)YSWL.Json.Conversion.JsonConvert.Import(typeof(YouKuInfo), jsonData);
            return info;
        }
        #endregion

        //#region 判断是否为土豆视频链接
        ///// <summary>
        ///// 判断是否为土豆视频链接
        ///// </summary>
        ///// <returns></returns>
        //public static bool IsTuDouVideoUrl(string url)
        //{
        //    if (url.StartsWith("http://www.tudou.com/programs/view/"))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region 得到土豆视频的视频编码
        ///// <summary>
        ///// 得到土豆视频的视频编码
        ///// </summary>
        //public static string GetTuDouVideoItemCode(string url)
        //{
        //    String vid = "";
        //    //if (StringPlus.IsNullOrEmpty(url) || !IsTuDouVideoUrl(url))
        //    //{
        //    //    return "";
        //    //}
        //    if (StringPlus.IsNullOrEmpty(url))
        //    {
        //        return "";
        //    }
        //    vid = StringPlus.TrimStart(url, "http://www.tudou.com/programs/view/");
        //    if (StringPlus.IsNullOrEmpty(vid))
        //    {
        //        return "";
        //    }
        //    int FirstPoint = vid.IndexOf("/");
        //    if (FirstPoint > 0)
        //    {
        //        vid = vid.Substring(0, FirstPoint);
        //    }
        //    return vid;
        //}
        //#endregion

        //#region 得到土豆视频信息

        ///// <summary>
        ///// 得到土豆视频信息
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static TuDouInfo GetTuDouInfo(string url)
        //{
        //    string itemCode = GetTuDouVideoItemCode(url);
        //    if (StringPlus.IsNullOrEmpty(itemCode))
        //    {
        //        return null;
        //    }

        //    string jsonData = PageLoader.Download(string.Format(tuDouJsonDataApiUrl, tuDouAppKey, itemCode));
        //    if (StringPlus.IsNullOrEmpty(jsonData))
        //    {
        //        return null;
        //    }

        //    string strStart = "{\"multiResult\":{\"results\":[";
        //    if (jsonData.StartsWith(strStart))
        //    {
        //        jsonData = jsonData.Replace(strStart, "");
        //    }

        //    string strEnd = "]}}";
        //    if (jsonData.EndsWith(strEnd))
        //    {
        //        int FirstPoint = jsonData.LastIndexOf(strEnd);
        //        if (FirstPoint > 0)
        //        {
        //            jsonData = jsonData.Substring(0, FirstPoint);
        //        }
        //    }

        //    TuDouInfo info = (TuDouInfo)Jayrock.Json.Conversion.JsonConvert.Import(typeof(TuDouInfo), jsonData);

        //    return info;
        //}
        //#endregion

        #region 判断是否是酷6视频链接
        /// <summary>
        /// 判断是否是酷6视频链接
        /// </summary>
        /// <returns></returns>
        public static bool IsKu6VideoUrl(string url)
        {
            string xmlData = PageLoader.Download(string.Format(ku6XmlDataApiUrl, url));
            if (!string.IsNullOrEmpty(xmlData))
            {
                XmlDocument xmldoc = new XmlDocument();

                xmldoc.LoadXml(xmlData);

                XmlNode xmlNodeType = xmldoc.SelectSingleNode("root/result");

                int type = -1;
                if (null != xmlNodeType && null != xmlNodeType.Attributes.GetNamedItem("type"))
                {
                    type = Convert.ToInt32(xmlNodeType.Attributes.GetNamedItem("type").Value);
                }
                else
                {
                    return false;
                }
                if (type == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 得到Ku6视频信息
        /// <summary>
        /// 得到Ku6视频信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Ku6Info GetKu6Info(string url)
        {
            Ku6Info info = new Ku6Info();
            //if (!IsKu6VideoUrl(url))
            //{
            //    return null;
            //}
            string xmlData = PageLoader.Download(string.Format(ku6XmlDataApiUrl, url));

            XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml(xmlData);

            XmlNode xmlNodeType = xmldoc.SelectSingleNode("root/result");

            int type = -1;
            if (null != xmlNodeType && null != xmlNodeType.Attributes.GetNamedItem("type"))
            {
                type = Convert.ToInt32(xmlNodeType.Attributes.GetNamedItem("type").Value);
            }
            else
            {
                return null;
            }
            if (type == -1)
            {
                return null;
            }
            info.type = type;

            XmlNode xmlNodeVid = xmldoc.SelectSingleNode("root/result/vid");
            if (null != xmlNodeVid)
            {
                info.vid = xmlNodeVid.InnerText;
            }

            XmlNode xmlNodeCoverurl = xmldoc.SelectSingleNode("root/result/coverurl");
            if (null != xmlNodeCoverurl)
            {
                info.coverurl = xmlNodeCoverurl.InnerText;
            }

            XmlNode xmlNodeFlash = xmldoc.SelectSingleNode("root/result/flash");
            if (null != xmlNodeFlash)
            {
                info.flash = xmlNodeFlash.InnerText;
            }

            XmlNode xmlNodeTitle = xmldoc.SelectSingleNode("root/result/title");
            if (null != xmlNodeTitle)
            {
                info.title = xmlNodeTitle.InnerText;
            }

            XmlNode xmlNodeDesc = xmldoc.SelectSingleNode("root/result/desc");
            if (null != xmlNodeDesc)
            {
                info.desc = xmlNodeDesc.InnerText;
            }
            return info;
        }
        #endregion
    }
}
