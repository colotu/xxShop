using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Member
{
   public class UserCount
    {
       /// <summary>
       /// 时间字符串
       /// </summary>
       public string DateStr;
       /// <summary>
       /// 关注数
       /// </summary>
       public int DayCount;
       /// <summary>
       /// 取消关注数
       /// </summary>
       public int CancelCount;
       /// <summary>
       /// 累积关注数
       /// </summary>
       public int TotalCount;
       /// <summary>
       /// 数量
       /// </summary>
       public int Count;
    }

   public class CancelCount
   {
       /// <summary>
       /// 时间字符串
       /// </summary>
       public string DateStr;
       /// <summary>
       /// 取消关注数
       /// </summary>
       public int Count;
   }

   public class DayCount
   {
       /// <summary>
       /// 时间字符串
       /// </summary>
       public string DateStr;
       /// <summary>
       /// 取消关注数
       /// </summary>
       public int Count;
   }
}
