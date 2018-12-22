using System;

namespace YSWL.Common.WebApi
{
    public static class JsonExtends
    {
        public static string ToJson<T>(this T obj)
            where T : class, new()
        {
            try
            {
                return YSWL.Json.Conversion.JsonConvert.ExportToString(obj);
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("实体转换失败："+ex.Message,ex.StackTrace);
                throw;
            }
         
        }

        public static T JsonToModel<T>(this string str)  
        {
            return YSWL.Json.Conversion.JsonConvert.Import<T>(str);
        }
    }
}