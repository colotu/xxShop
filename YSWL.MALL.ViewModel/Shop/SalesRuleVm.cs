using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.Shop
{
    public class SalesRuleVm
    {
        /// <summary>
        /// 批发规则ID
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 批发规则名称
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 应用方式  0：单个商品 1：商品总计
        /// </summary>
        public int RuleMode { get; set; }
        /// <summary>
        /// 规则单位 0：个 1：元
        /// </summary>
        public int RuleUnit { get; set; }
        /// <summary>
        /// 状态 0：不启用，1：启用
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 类型 0：批发规则  1：一键会员规则
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedUserID { get; set; }
        /// <summary>
        /// 规则
        /// </summary>
        public List<SaleRuleItemVm> RuleItem { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public List<int> RankItem { get; set; }
    }

    public class SaleRuleItemVm
    {
        /// <summary>
        /// 规则项ID
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 对应规则ID
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 规则类型 0：打折 1：减价 2：固定价格
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 单位数值 比如  100个或者 100元
        /// </summary>
        public int UnitValue { get; set; }
        /// <summary>
        /// 优惠数值
        /// </summary>
        public int RateValue { get; set; }
    }

    public class SaleUserRankVm
    {
        /// <summary>
        /// 批发规则ID
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 用户等级ID
        /// </summary>
        public int RankId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark{ get; set; }
    }
}
