using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.DALFactory
{
    public class DAERPProducts : DataAccessBase
    {
        /// <summary>
        /// 创建Depot数据层接口。
        /// </summary>
       public static YSWL.IDAL.ERP.Products.IDepot CreateDepot()
        {

            string ClassNamespace = AssemblyPath + ".ERP.Products.Depot";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.IDAL.ERP.Products.IDepot)objType;
        }

       /// <summary>
       /// 创建Depot数据层接口。
       /// </summary>
       public static YSWL.IDAL.ERP.Products.IStore CreateStore()
       {

           string ClassNamespace = AssemblyPath + ".ERP.Products.Store";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.IDAL.ERP.Products.IStore)objType;
       }
    }
}
