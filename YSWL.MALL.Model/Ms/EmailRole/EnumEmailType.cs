using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.Model.Ms.EmailRole
{
    public static class EnumEmailType
    {
        public enum EmailType
        {
            OrderCount=1,//订单数量
            FavCount=2,//关注人数
            ActivityUserCount=3,//当月活跃客户数
            UserActivityFrequency=4,//客户当月活跃频次
            SalePerDayAndMonthAct=5,
            SalePerDay=6
        }
    }
}
/*1.日/月 订单数
2.关注人数
3.当月活跃客户数
4.客户当月活跃频次
5.业务员每日业绩统计报表+月活跃数
6.业务员每日业绩统计报表*/