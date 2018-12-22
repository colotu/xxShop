using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using YSWL.MALL.ViewModel.Shop;

namespace YSWL.MALL.Web.Components
{
    public class SupplierAdPHelper
    {
        /// <summary>
        /// 获取可用的状态
        /// </summary>
        /// <returns></returns>
        public static List<YSWL.MALL.ViewModel.Supplier.SupplierAdP> GetAllList()
        {
            List<YSWL.MALL.ViewModel.Supplier.SupplierAdP> list = new List<YSWL.MALL.ViewModel.Supplier.SupplierAdP>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("/SupplierAdP.config"));
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("AdvPositions/AdvPosition");
            if (nodes != null)
            {
                YSWL.MALL.ViewModel.Supplier.SupplierAdP model;
                foreach (var item in nodes)
                {
                    XmlElement node = (XmlElement)item;
                    model = new ViewModel.Supplier.SupplierAdP();
                    model.Name = node.GetAttribute("name");
                    model.AdvPositionId = Common.Globals.SafeInt(node.GetAttribute("advpositionId"),0) ;
                        list.Add(model);
                }
            }
            return list;
        }
    }
}