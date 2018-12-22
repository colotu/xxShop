using System;
using System.Reflection;
using System.Configuration;
using YSWL.MALL.IDAL.Pay;
namespace YSWL.MALL.DALFactory
{
	/// <summary>
	/// 抽象工厂模式创建DAL。
	/// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口) 
	/// DataCache类在导出代码的文件夹里
	/// <appSettings> 
	/// <add key="DAL" value="YSWL.MALL.SQLServerDAL.Pay" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)
	/// </appSettings> 
	/// </summary>
    public sealed class DAPay : DataAccessBase//<t>
	{
	 
		/// <summary>
		/// 创建RechargeRequest数据层接口。
		/// </summary>
		public static YSWL.MALL.IDAL.Pay.IRechargeRequest CreateRechargeRequest()
		{

			string ClassNamespace = AssemblyPath +".Pay.RechargeRequest";
			object objType=CreateObject(AssemblyPath,ClassNamespace);
			return (YSWL.MALL.IDAL.Pay.IRechargeRequest)objType;
		}


		/// <summary>
		/// 创建BalanceDetails数据层接口。
		/// </summary>
		public static YSWL.MALL.IDAL.Pay.IBalanceDetails CreateBalanceDetails()
		{

			string ClassNamespace = AssemblyPath +".Pay.BalanceDetails";
			object objType=CreateObject(AssemblyPath,ClassNamespace);
			return (YSWL.MALL.IDAL.Pay.IBalanceDetails)objType;
		}
        /// <summary>
        /// 创建BalanceDrawRequest数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Pay.IBalanceDrawRequest CreateBalanceDrawRequest()
        {

            string ClassNamespace = AssemblyPath + ".Pay.BalanceDrawRequest";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Pay.IBalanceDrawRequest)objType;
        }

        /// <summary>
        /// 创建BalanceDrawRequest数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Pay.IGwjfDetails CreateGwjfDrawRequest()
        {

            string ClassNamespace = AssemblyPath + ".Pay.GwjfDrawRequest";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Pay.IGwjfDetails)objType;
        }

    }
}