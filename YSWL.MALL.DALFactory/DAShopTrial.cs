using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
      public sealed class DAShopTrial : DataAccessBase
    {
        /// <summary>
        /// 创建Trials数据层接口。
        /// </summary>
          public static YSWL.MALL.IDAL.Shop.Trial.ITrials CreateTrials()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Trial.Trials";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Trial.ITrials)objType;
        }


        /// <summary>
          /// 创建TrialReports数据层接口。
        /// </summary>
          public static YSWL.MALL.IDAL.Shop.Trial.ITrialReports CreateTrialReports()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Trial.TrialReports";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Trial.ITrialReports)objType;
        }

    }
}
