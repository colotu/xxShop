/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrders.cs
*
* 功 能： N/A
* 类 名： ReturnOrders
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:36   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace YSWL.MALL.Model.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrders:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public partial class ReturnOrders
	{
 
        #region 扩展属性
        /// <summary>
        /// 退单项
        /// </summary>
        public List<Model.Shop.ReturnOrder.ReturnOrderItems> Items { get; set; }
        /// <summary>
        /// 服务类型字符串
        /// </summary>
        public string ServiceTypeStr { get; set; }
        /// <summary>
        /// 返回方式字符串
        /// </summary>
        public string ReturnTypeStr { get; set; }
        /// <summary>
        /// 组合状态字符串
        /// </summary>
        public string MainStatusStr { get; set; }

        /// <summary>
        /// 退货类型字符串
        /// </summary>
        public string ReturnGoodsTypeStr { get; set; }
        #endregion

        /// <summary>
        /// 服务类型的退/换单编号前缀 T：表示退单，H：表示换单，W：维修
        /// </summary>
        /// <remarks>退/换单编号使用</remarks>
        public string ReturnOrderPrefix
        {
            get
            {
                switch (_servicetype)
                {
                    case 1:
                        return "T";
                    case 2:
                        return "H";
                    case 3:
                        return "W";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}

