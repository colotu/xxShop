using System;
using System.Reflection;
using System.Configuration;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.DALFactory
{
	/// <summary>
	/// 抽象工厂模式创建DAL。
	/// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口) 
	/// DataCache类在导出代码的文件夹里
	/// <appSettings> 
	/// <add key="DAL" value="YSWL.MALL.SQLServerDAL.Members" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)
	/// </appSettings> 
	/// </summary>
	public sealed class DAShopPackage:DataAccessBase
	{

        /// <summary>
        /// 创建Package数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Package.IPackage CreatePackage()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Package.Package";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Package.IPackage)objType;
        }


        /// <summary>
        /// 创建PackageCategory数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Package.IPackageCategory CreatePackageCategory()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Package.PackageCategory";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Package.IPackageCategory)objType;
        }

        /// <summary>
        /// 创建ProductPackage数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Package.IProductPackage CreateProductPackage()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Package.ProductPackage";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Package.IProductPackage)objType;
        }


}
}