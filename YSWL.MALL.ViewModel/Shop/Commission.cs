using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
    /// <summary>
    /// 佣金层级用户
    /// </summary>
    public class CommissionUser
    {
        public YSWL.MALL.Model.Members.Users FirstUser;
        public YSWL.MALL.Model.Members.Users SecondUser;
        public YSWL.MALL.Model.Members.Users ThirdUser;
        public YSWL.MALL.Model.Members.Users FourUser;

    }

    public class ProComModel
    {
        /// <summary>
        /// 规则ID
        /// </summary>
        public int  RuleId ;
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId;
        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName;
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal ProductPrice;
        /// <summary>
        /// 图片
        /// </summary>
        public string ThumbnailUrl;
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName;
        /// <summary>
        /// 应用方式
        /// </summary>
        public int RuleMode;
        /// <summary>
        /// 一级佣金
        /// </summary>
        public decimal FirstValue;
        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal FeeRate;
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal FirstFee;
        /// <summary>
        /// 推广链接码
        /// </summary>
        public string PromoCode;

    }


    public class CommissionStat
    {
        public int UserId;
        public string NickName;
        public Decimal TotalFee;
        public int OrderCount;
    }

    public class CommissionProStat
    {
        public int ProductId;
        public string ProName;   
        public Decimal TotalFee;
        public int TotalProduct;
        public int OrderCount;
        public int UserCount;
    }

    public class StatCommissionFee
    {
        public string  DateStr;
        public Decimal TotalFee;
        public int OrderCount;
        public int UserCount;
    }
    public class CommissionRuleStat
    {
        public int RuleId;
        public string RuleName;
        public Decimal TotalFee;
        public int TotalProduct;
    }
}
