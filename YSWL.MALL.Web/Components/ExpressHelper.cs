using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using YSWL.Common.Mail;
using YSWL.Json.Conversion;
using YSWL.Json;
using YSWL.MALL.ViewModel.Shop;
using System.Xml;

namespace YSWL.MALL.Web.Components
{
    public class ExpressHelper
    {
        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="typeCom"></param>
        /// <param name="nu"></param>
        /// <returns></returns>
        public static List<YSWL.MALL.ViewModel.Shop.Express> GetExpress(string typeCom, string nu)
        {
            List<YSWL.MALL.ViewModel.Shop.Express> expressList = new List<ViewModel.Shop.Express>();
            string ApiKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiKey");
            string apiurl = "http://api.kuaidi100.com/api?id=" + ApiKey + "&com=" + typeCom + "&nu=" + nu + "&show=0&muti=1&order=desc";
            try
            {
                WebRequest request = WebRequest.Create(@apiurl);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string content = reader.ReadToEnd();
                // 解析json 数据
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                if (jsonObject["data"] == null || String.IsNullOrWhiteSpace(jsonObject["data"].ToString()))
                {
                    return null;
                }
                var datas = jsonObject["data"].ToString();
                JsonArray jsonArray = JsonConvert.Import<JsonArray>(datas);
                YSWL.MALL.ViewModel.Shop.Express express = null;
                foreach (JsonObject item in jsonArray)
                {
                    express = new YSWL.MALL.ViewModel.Shop.Express();
                    express.Date = Common.Globals.SafeDateTime(item["time"],DateTime.Now);
                    express.Content = item["context"].ToString();
                    expressList.Add(express);
                }
                return expressList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static List<YSWL.MALL.ViewModel.Shop.Express> GetExpress(long orderId)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();

            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetModelByCache(orderId);
            if (orderInfo == null)
            {
                return null;
            }
            List<YSWL.MALL.ViewModel.Shop.Express> expressList = new List<ViewModel.Shop.Express>();
            string ApiKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiKey");
            if (String.IsNullOrWhiteSpace(orderInfo.ExpressCompanyAbb) || String.IsNullOrWhiteSpace(orderInfo.ShipOrderNumber))
            {
                return null;
            }
            string apiurl = "http://api.kuaidi100.com/api?id=" + ApiKey + "&com=" + orderInfo.ExpressCompanyAbb + "&nu=" + orderInfo.ShipOrderNumber + "&show=0&muti=1&order=desc";
            try
            {
                WebRequest request = WebRequest.Create(@apiurl);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string content = reader.ReadToEnd();
                // 解析json 数据
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                if (jsonObject["data"] == null || String.IsNullOrWhiteSpace(jsonObject["data"].ToString()))
                {
                    return null;
                }
                var datas = jsonObject["data"].ToString();
                JsonArray jsonArray = JsonConvert.Import<JsonArray>(datas);
                YSWL.MALL.ViewModel.Shop.Express express = null;
                foreach (JsonObject item in jsonArray)
                {
                    express = new YSWL.MALL.ViewModel.Shop.Express();
                    express.Date = Common.Globals.SafeDateTime(item["time"], DateTime.Now);
                    express.Content = item["context"].ToString();
                    expressList.Add(express);
                }
                return expressList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static List<YSWL.MALL.ViewModel.Shop.Express> GetExpress(string  ordercode)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();

            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetOrderInfo(ordercode);

            List<YSWL.MALL.ViewModel.Shop.Express> expressList = new List<ViewModel.Shop.Express>();
            string ApiKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiKey");
            if (orderInfo == null)
            {
                return null;
            }
            if (String.IsNullOrWhiteSpace(orderInfo.ExpressCompanyAbb) || String.IsNullOrWhiteSpace(orderInfo.ShipOrderNumber))
            {
                return null;
            }
            string apiurl = "http://api.kuaidi100.com/api?id=" + ApiKey + "&com=" + orderInfo.ExpressCompanyAbb + "&nu=" + orderInfo.ShipOrderNumber + "&show=0&muti=1&order=desc";
            try
            {
                WebRequest request = WebRequest.Create(@apiurl);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string content = reader.ReadToEnd();
                // 解析json 数据
                YSWL.Json.JsonObject jsonObject = JsonConvert.Import<JsonObject>(content);
                if (jsonObject["data"] == null || String.IsNullOrWhiteSpace(jsonObject["data"].ToString()))
                {
                    return null;
                }
                var datas = jsonObject["data"].ToString();
                JsonArray jsonArray = JsonConvert.Import<JsonArray>(datas);
                YSWL.MALL.ViewModel.Shop.Express express = null;
                foreach (JsonObject item in jsonArray)
                {
                    express = new YSWL.MALL.ViewModel.Shop.Express();
                    express.Date = Common.Globals.SafeDateTime(item["time"], DateTime.Now);
                    express.Content = item["context"].ToString();
                    expressList.Add(express);
                }
                return expressList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取Html物流信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string GetHtmlExpress(long orderId)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();
            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetModelByCache(orderId);
            if (orderInfo == null)
            {
                return null;
            }
            string ApiKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiKey");
            if (String.IsNullOrWhiteSpace(orderInfo.ExpressCompanyAbb) || String.IsNullOrWhiteSpace(orderInfo.ShipOrderNumber))
            {
                return null;
            }
            string apiurl = "http://www.kuaidi100.com/applyurl?key=" + ApiKey + "&com=" + orderInfo.ExpressCompanyAbb + "&nu=" + orderInfo.ShipOrderNumber ;
            try
            {
                WebRequest request = WebRequest.Create(@apiurl);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string content = reader.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ComType> GetAllComType()
        {
            List<ComType> expressList = new List<ComType>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/Express.config"));
            //取根结点
            var root = xmlDoc.DocumentElement;//取到根结点
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("expresses/express");
            if (nodes != null)
            {
                ComType model = null;
                foreach (var item in nodes)
                {
                     XmlElement node = (XmlElement)item;
                     model = new ComType();
                     model.ComName = node.GetAttribute("name");
                     model.ComEn = node.GetAttribute("code");
                     model.Enabled = Common.Globals.SafeBool(node.GetAttribute("enabled"), false);
                     if (model.Enabled)
                     {
                         expressList.Add(model);
                     }
                }
            }
            return expressList;
        }
    }
}