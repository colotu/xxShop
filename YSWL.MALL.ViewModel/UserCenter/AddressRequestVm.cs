using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.UserCenter
{
    public class AddressRequestVm
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string CelPhone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
