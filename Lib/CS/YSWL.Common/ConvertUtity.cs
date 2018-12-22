using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using YSWL.Json;

namespace YSWL.Common
{
    public class ConvertUtity
    {
        /// <summary>
        /// 转换实体集合为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> entitys) where T : new()
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        /// 将table转换为实体集合（后期重构将其提取到公共层，为提高性能后期重构可以考虑首次反射后进行缓存）  
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="dt">查询出的对象</param>
        /// <returns></returns>
        public static IList<T> ConvertToEntity<T>(DataTable dt) where T : new()
        {
            if (dt.Rows.Count < 1)
            {
                return null;
            }

            IList<T> ts = new List<T>();
            //属性字段名
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (System.Reflection.PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType.FullName.Contains("System.Nullable"))
                            {
                                if (pi.PropertyType.FullName.Contains("System.Int32"))
                                {
                                    pi.SetValue(t, Convert.ToInt32(value), null);
                                }
                                else if (pi.PropertyType.FullName.Contains("System.Decimal"))
                                {
                                    pi.SetValue(t, Convert.ToDecimal(value), null);
                                }
                                else if (pi.PropertyType.FullName.Contains("System.DateTime"))
                                {
                                    pi.SetValue(t, Convert.ToDateTime(value), null);
                                }
                            }
                            else
                            {
                                pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);
                            }
                        }
                    }
                }

                ts.Add(t);
            }

            return ts;
        }

        public static T ConvertToModel<T>(DataTable dt) where T : new()
        {
            T t = new T();
            if (dt.Rows.Count < 1)
            {
                return t;
            }
            //属性字段名
            string tempName = "";
        
            // 获得此模型的公共属性
            System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo pi in propertys)
            {
                tempName = pi.Name;
                // 检查DataTable是否包含此列
                if (dt.Columns.Contains(tempName))
                {
                    // 判断此属性是否有Setter
                    if (!pi.CanWrite) continue;
                    object value = dt.Rows[0][tempName];
                    if (value != DBNull.Value)
                    {
                        if (pi.PropertyType.FullName.Contains("System.Nullable"))
                        {
                            if (pi.PropertyType.FullName.Contains("System.Int32"))
                            {
                                pi.SetValue(t, Convert.ToInt32(value), null);
                            }
                            else if (pi.PropertyType.FullName.Contains("System.Decimal"))
                            {
                                pi.SetValue(t, Convert.ToDecimal(value), null);
                            }
                            else if (pi.PropertyType.FullName.Contains("System.DateTime"))
                            {
                                pi.SetValue(t, Convert.ToDateTime(value), null);
                            }
                        }
                        else
                        {
                            pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);
                        }
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 将实体转换为目标对象实体
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <typeparam name="K">待转换的对象</typeparam>
        /// <param name="obj">待转换的实体对象</param>
        /// <returns>返回转换后的目标实体对象</returns>
        public static T ConvertToTempModel<T, K>(K obj)
            where T : new()
            where K : class
        {
            Type k_type = obj.GetType();

            Type t_type = typeof(T);

            PropertyInfo[] k_propertyInfos = k_type.GetProperties();

            PropertyInfo[] t_propertyInfos = t_type.GetProperties();
            T temp = new T();
            foreach (var item in k_propertyInfos)
            {
                var t_propertyInfo = t_propertyInfos.Where(p => p.Name.ToLower().Equals(item.Name.ToLower())).FirstOrDefault();
                if (t_propertyInfo == null || !item.CanRead || !item.CanWrite)
                {
                    continue;
                }
                object value = item.GetValue(obj, null);
                if (item.PropertyType.FullName.Contains("System.Nullable"))
                {
                    if (item.PropertyType.FullName.Contains("System.Int32"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToInt32(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.Decimal"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToDecimal(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToDateTime(value).ToShortDateString(), null);
                    }
                }
                else
                {
                    t_propertyInfo.SetValue(temp, Convert.ChangeType(value, t_propertyInfo.PropertyType), null);
                }


            }
            return temp;
        }

        public static List<T> ConvertToListTempModel<T, K>(List<K> objs)
            where T : new()
            where K : class
        {
            if (objs == null || !objs.Any())
            {
                return null;
            }
            List<T> tList = new List<T>();

            foreach (var item in objs)
            {
                tList.Add(ConvertToTempModel<T, K>(item));
            }

            return tList;
        }

        /// <summary>
        /// json对应转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertToModel<T>(JsonObject obj)
            where T : new()
        {
            Type t_type = typeof(T);
            PropertyInfo[] t_propertyInfos = t_type.GetProperties();
            T temp = new T();
            foreach (var item in t_propertyInfos)
            {
                string key = item.Name;
                if (!obj.Contains(key))
                {
                    continue;
                }
                object value = obj[key];
                if (item.PropertyType.FullName.Contains("System.Nullable"))
                {
                    if (item.PropertyType.FullName.Contains("System.Int32"))
                    {
                        item.SetValue(temp, Convert.ToInt32(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.Decimal"))
                    {
                        item.SetValue(temp, Convert.ToDecimal(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        item.SetValue(temp, Convert.ToDateTime(value), null);
                    }
                }
                else
                {
                    item.SetValue(temp, Convert.ChangeType(value, item.PropertyType), null);
                }
            }
            return temp;
        }

        public static T JsonConvertToObject<T>(string json)
        {
            return YSWL.Json.Conversion.JsonConvert.Import<T>(json);
        }

        #region WCF Model 转化
        /// <summary>
        /// WCF 专用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<T> ConvertToWCFList<T, K>(List<K> objs)
    where T : new()
    where K : class
        {
            if (objs == null || !objs.Any())
            {
                return null;
            }
            List<T> tList = new List<T>();

            foreach (var item in objs)
            {
                tList.Add(ConvertToWCFModel<T, K>(item));
            }

            return tList;
        }

        public static T ConvertToWCFModel<T, K>(K obj)
    where T : new()
    where K : class
        {
            Type k_type = obj.GetType();

            Type t_type = typeof(T);

            PropertyInfo[] k_propertyInfos = k_type.GetProperties();

            PropertyInfo[] t_propertyInfos = t_type.GetProperties();
            T temp = new T();
            foreach (var item in k_propertyInfos)
            {
                var t_propertyInfo = t_propertyInfos.Where(p => p.Name.Replace("_", "").ToLower().Equals(item.Name.ToLower())).FirstOrDefault(); //替换WCF的“_”;
                if (t_propertyInfo == null || !item.CanRead || !item.CanWrite)
                {
                    continue;
                }
                object value = item.GetValue(obj, null);
                if (item.PropertyType.FullName.Contains("System.Nullable"))
                {
                    if (item.PropertyType.FullName.Contains("System.Int32"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToInt32(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.Decimal"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToDecimal(value), null);
                    }
                    else if (item.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        t_propertyInfo.SetValue(temp, Convert.ToDateTime(value).ToShortDateString(), null);
                    }
                }
                else if (!item.PropertyType.FullName.Contains("System.Collections.Generic")) //排除复杂属性
                {
                    t_propertyInfo.SetValue(temp, Convert.ChangeType(value, t_propertyInfo.PropertyType), null);
                }


            }
            return temp;
        }
        #endregion 
    }
}
