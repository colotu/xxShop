using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.Model.Members.Enum
{
    public enum SourceType
    {
        PC = 1,//PC注册
        WeChat = 2,//微信注册
        /// <summary>
        ///业务员代替注册
        /// </summary>
        SalesMan = 3,
        /// <summary>
        /// 400客服代注册
        /// </summary>
        Cust = 4,
        /// <summary>
        /// 订货注册
        /// </summary>
        Ding = 5,
        /// <summary>
        /// SAAS 云平台
        /// </summary>
        SAAS = 6,
    }
}
