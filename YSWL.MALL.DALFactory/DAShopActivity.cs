using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public class DAShopActivity : DataAccessBase
    {
        /// <summary>
        /// 创建ActivityInfo数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Activity.IActivityInfo CreateActivityInfo()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Activity.ActivityInfo";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Activity.IActivityInfo)objType;
        }


        /// <summary>
        /// 创建ActivityRule数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Activity.IActivityRule CreateActivityRule()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Activity.ActivityRule";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Activity.IActivityRule)objType;
        }
        /// <summary>
        /// 创建ActivityDetail数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Activity.IActivityDetail CreateActivityDetail()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Activity.ActivityDetail";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Activity.IActivityDetail)objType;
        }
    }
}
