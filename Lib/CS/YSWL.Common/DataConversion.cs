using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace YSWL.Common
{
    /// <summary>
    /// 导出excel表格
    /// </summary>
    public class DataConversion
    {
        const string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;";

        /// <summary>
        /// 导出EXECL 表格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        public string DataTableToExcel(DataTable dt, string excelPath)
        {


            if (dt == null)
            {
                return "DataTable不能为空";
            }

            //命名sheet1表名
            dt.TableName = "Sheet1";

            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            StringBuilder sb;
            string connString;

            if (rows == 0)
            {
                return "没有数据";
            }

            sb = new StringBuilder();
            connString = string.Format(ConnectionString, excelPath);

            //生成创建表的脚本
            sb.Append("CREATE TABLE ");
            sb.Append(dt.TableName + " ( ");

            for (int i = 0; i < cols; i++)
            {
                if (i < cols - 1)
                    sb.Append(string.Format("{0} nvarchar,", dt.Columns[i].ColumnName));
                else
                    sb.Append(string.Format("{0} nvarchar)", dt.Columns[i].ColumnName));
            }

            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;

                objCmd.CommandText = sb.ToString();

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return "在Excel中创建表失败，错误信息：" + e.Message;
                }

                #region 生成插入数据脚本
                sb.Remove(0, sb.Length);
                sb.Append("INSERT INTO ");
                sb.Append(dt.TableName + " ( ");

                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append(dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append(dt.Columns[i].ColumnName + ") values (");
                }

                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append("@" + dt.Columns[i].ColumnName + ",");
                    else
                        sb.Append("@" + dt.Columns[i].ColumnName + ")");
                }
                #endregion


                //建立插入动作的Command
                objCmd.CommandText = sb.ToString();
                OleDbParameterCollection param = objCmd.Parameters;

                for (int i = 0; i < cols; i++)
                {
                    param.Add(new OleDbParameter("@" + dt.Columns[i].ColumnName, OleDbType.VarChar));
                }

                //遍历DataTable将数据插入新建的Excel文件中
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }

                    objCmd.ExecuteNonQuery();
                }

                return "数据已成功导入Excel";
            }//end using
        }


        /// <summary> 
        /// 读取Excel文档 
        /// </summary> 
        /// <param name="Path">文件名称</param> 
        /// <returns>返回一个数据集</returns> 
        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            System.Data.OleDb.OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [Sheet1$]";
            myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            conn.Close();
            return ds;
        }

    }
}
