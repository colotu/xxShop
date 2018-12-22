using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Order
{
    public class SalesOrderCount
    {
        public SalesOrderCount()
        { }
        #region Model
        //业务员名称
        public string SalesName;
        //业务员Id
        public int SalesId = 0;
        //PC用户总数
        public int PcUserCount = 0;
        //微信用户总数
        public int WeUserCount = 0;
        //代下单用户总数
        public int RenUserCount = 0;
        //客服代下单用户总数
        public int CustUserCount = 0;
        //APP用户总数
        public int AppUserCount = 0;
        //PC日订单数
        public int PcDayCount = 0;
        //微信日订单数
        public int WeDayCount = 0;
        //代下单日订单数
        public int RenDayCount = 0;
        //客服
        public int CustDayCount = 0;
        //App日订单数
        public int AppDayCount = 0;
        //PC日销售额
        public Decimal PcAmount = 0;
        //微信日销售额
        public Decimal WeAmount = 0;
        //代下单日销售额
        public Decimal RenAmount = 0;
        //客服日销售额
        public Decimal CustAmount = 0;
        //App日销售额
        public Decimal AppAmount = 0;
        //PC月订单数
        public int PcMonthCount = 0;
        //微信月订单数
        public int WeMonthCount = 0;
        //代下单月订单数
        public int RenMonthCount = 0;
        //客服代下单月订单数
        public int CustMonthCount = 0;
        //APP月订单数
        public int AppMonthCount = 0;
        //PC 月销售额
        public Decimal PcMonthAmount = 0;
        //微信月销售额
        public Decimal WeMonthAmount = 0;
        //代下单月销售额
        public Decimal RenMonthAmount = 0;
        //客服代下单月销售额
        public Decimal CustMonthAmount = 0;
        //App月销售额
        public Decimal AppMonthAmount = 0;
        //PC  月客户活跃数
        public int PcMUserCount = 0;
        //微信  月客户活跃数
        public int WeMUserCount = 0;
        //代下单  月客户活跃数
        public int RenMUserCount = 0;
        //客服代下单  月客户活跃数
        public int CustMUserCount = 0;
        //APP  月客户活跃数
        public int AppMUserCount = 0;
        //日订单总数
        public int DayCount = 0;
        //日销售额
        public Decimal Amount = 0;
        //当月订单数
        public int MonthCount = 0;
        //当月销售额
        public Decimal MonthAmount = 0;

        //新增日客户数
        public int DayCustomCount = 0;
        //新增月客户数
        public int MonthCustomCount = 0;

        //新增日注册数
        public int DayRegisterCount = 0;
        //新增月注册数
        public int MonthRegisterCount = 0;

        //用户总数
        public int UserCount = 0;
        //活跃数
        public int MActiveCount = 0;

        #endregion Model
    }

    public class CustCount
    {
        //业务员Id
        public int SalesId = 0;
        public int Count = 0;
    }
    //活跃数
    public class ActiveCount
    {
        //业务员Id
        public int SalesId = 0;
        public int Count = 0;
    }

    public class SalesCount
    {
        public int SalesId;
        public int Count;
        public decimal Amount;
    }

    public class DayCount
    {
        public int SalesId;
        public string DateStr;
        public int Count;
        public decimal Amount;
    }
}
