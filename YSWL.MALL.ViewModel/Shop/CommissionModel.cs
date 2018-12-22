using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace YSWL.MALL.ViewModel.Shop
{
    public class CommissionModel
    {
        /// <summary>
        /// 商品数
        /// </summary>
        public int ProductCount;
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount;
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal AllFee;
        /// <summary>
        /// 盟友数
        /// </summary>
        public int AllyCount;
    }
}
