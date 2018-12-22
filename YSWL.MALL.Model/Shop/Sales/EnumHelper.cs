using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.Model.Shop.Sales
{
  public  static  class EnumHelper
    {
        public enum RuleType
        {
            None = -1,
            /// <summary>
            /// 打折
            /// </summary>
            Discount = 0,
            /// <summary>
            /// 减价
            /// </summary>
            Cut = 1,
            /// <summary>
            /// 直降
            /// </summary>
            Reduce = 2

        }
    }


}
