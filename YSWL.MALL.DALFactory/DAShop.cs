namespace YSWL.MALL.DALFactory
{
    public sealed class DAShop: DataAccessBase
    {
        /// <summary>
        /// 创建Favorite数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.IFavorite CreateFavorite()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Favorite";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.IFavorite)objType;
        }

        /// <summary>
        /// 创建Shippers数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.IShippers CreateShippers()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Shippers";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.IShippers)objType;
        }
        

    }
}