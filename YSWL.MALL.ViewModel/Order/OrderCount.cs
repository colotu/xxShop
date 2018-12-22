using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Order
{
    public class OrderCount
    {
        /// <summary>
        /// 时间字符串
        /// </summary>
        public string DateStr;
        /// <summary>
        /// 人数
        /// </summary>
        public int Count;
    }
    public class OrderPriceCount
    {
        /// <summary>
        /// 时间字符串
        /// </summary>
        public string DateStr { get; set; }
        public int Count{get;set;}
        public decimal Price { get; set; }
    }
    public class OrderFreCount
    {
        /// <summary>
        /// 时间字符串
        /// </summary>
        public string DateStr { get; set; }
        /// <summary>
        /// 活跃数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 频次
        /// </summary>
        public double FreCount { get; set; }
    }
    /// <summary>
    /// 店铺排行统计
    /// </summary>
    public class ShopSaleInfo
    {
        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Amount { get; set; }
    }
    /// <summary>
    /// 产品销量
    /// </summary>
    public class ProductSale
    {
        /// <summary>
        /// 时间字符串
        /// </summary>
        public string DateStr { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
    /// <summary>
    /// 产品销量排行统计
    /// </summary>
    public class ProductSaleInfo
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int Count { get; set; }
    }
    /// <summary>
    /// 品牌排行统计
    /// </summary>
    public class BrandSaleInfo
    {
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Amount { get; set; }
    }
    /// <summary>
    /// 商品分类统计
    /// </summary>
    public class CategoryCount
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 在售数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 下架数量
        /// </summary>
        public int OffCount { get; set; }
    }
    /// <summary>
    /// 支付方式
    /// </summary>
    public class Payment
    {
        public string PaymentName { get; set; }
        public decimal Amount { get; set; }
    }
}
