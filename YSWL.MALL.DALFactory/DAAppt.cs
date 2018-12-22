namespace YSWL.MALL.DALFactory
{
    public sealed class DAAppt : DataAccessBase
    {
        /// <summary>
        /// 创建Services数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Appt.IServices CreateServices()
        {
            string ClassNamespace = AssemblyPath + ".Appt.Services";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Appt.IServices)objType;
        }
        /// <summary>
        /// 创建Reservation数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Appt.IReservation CreateReservation()
        {
            string ClassNamespace = AssemblyPath + ".Appt.Reservation";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Appt.IReservation)objType;
        }
    }
}