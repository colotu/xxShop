using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
  
   public class DAShopCommission : DataAccessBase
    {
        /// <summary>
        /// 创建CommissionPro数据层接口。
        /// </summary>
       public static YSWL.MALL.IDAL.Shop.Commission.ICommissionPro CreateCommissionPro()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Commission.CommissionPro";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Commission.ICommissionPro)objType;
        }

       /// <summary>
       /// 创建CommissionPro数据层接口。
       /// </summary>
       public static YSWL.MALL.IDAL.Shop.Commission.ICommissionRule CreateCommissionRule()
       {
           string ClassNamespace = AssemblyPath + ".Shop.Commission.CommissionRule";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.MALL.IDAL.Shop.Commission.ICommissionRule)objType;
       }


       /// <summary>
       /// 创建CommissionDetail数据层接口。
       /// </summary>
       public static YSWL.MALL.IDAL.Shop.Commission.ICommissionDetail CreateCommissionDetail()
       {
           string ClassNamespace = AssemblyPath + ".Shop.Commission.CommissionDetail";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.MALL.IDAL.Shop.Commission.ICommissionDetail)objType;
       }
    }
}
