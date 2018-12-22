namespace YSWL.DALFactory
{
    /// <summary>
    /// 抽象工厂模式创建DAL。
    /// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口) 
    /// DataCache类在导出代码的文件夹里
    /// <appSettings> 
    /// <add key="DAL" value="YSWL.SQLServerDAL.JLT" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)
    /// </appSettings> 
    /// </summary>
    public sealed class DAERP : DataAccessBase
    {
        
       /// <summary>
       /// 创建ERP_Lines数据层接口。
       /// </summary>
        public static YSWL.IDAL.ERP.Packing.ILines CreateERPLines()
       {

           string ClassNamespace = AssemblyPath + ".ERP.Packing.Lines";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.IDAL.ERP.Packing.ILines)objType;
       }

       /// <summary>
       /// 创建UsersExp数据层接口。
       /// </summary>
       public static YSWL.IDAL.ERP.Member.IUsersExp CreateUsersExp()
       {
           string ClassNamespace = AssemblyPath + ".ERP.Member.UsersExp";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.IDAL.ERP.Member.IUsersExp)objType;
       }

       /// <summary>
       /// 创建Vehicles数据层接口。
       /// </summary>
       public static YSWL.IDAL.ERP.Packing.IVehicles CreateVehicles()
       {

           string ClassNamespace = AssemblyPath + ".ERP.Packing.Vehicles";
           object objType = CreateObject(AssemblyPath, ClassNamespace);
           return (YSWL.IDAL.ERP.Packing.IVehicles)objType;
       }

    }
}
