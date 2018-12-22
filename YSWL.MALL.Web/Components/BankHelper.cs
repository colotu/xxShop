using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using YSWL.MALL.ViewModel.Shop;

namespace YSWL.MALL.Web.Components
{
    public class BankHelper
    {
        /// <summary>
        /// 获取可用的网银银行卡信息
        /// </summary>
        /// <returns></returns>
        public static List<BankType> GetAllBankList()
        {
            List<BankType> bankList = new List<BankType>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/Bank.config"));
            //取根结点
            var root = xmlDoc.DocumentElement;//取到根结点
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("banks/bank");
            if (nodes != null)
            {
                BankType model = null;
                foreach (var item in nodes)
                {
                    XmlElement node = (XmlElement)item;
                    model = new BankType();
                    model.BankName = node.GetAttribute("name");
                    model.Code = node.GetAttribute("code");
                    model.Enabled = Common.Globals.SafeBool(node.GetAttribute("enabled"), false);
                    if (model.Enabled)
                    {
                        bankList.Add(model);
                    }
                }
            }
            return bankList;
        }
    }
}