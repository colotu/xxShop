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
    public sealed class DAShopSample : DataAccessBase
    {
        /// <summary>
        /// 创建Sample数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Sample.ISample CreateSample()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Sample.Sample";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Sample.ISample)objType;
        }

        /// <summary>
        /// 创建SampleDetail数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Sample.ISampleDetail CreateSampleDetail()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Sample.SampleDetail";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Sample.ISampleDetail)objType;
        }
    }
}