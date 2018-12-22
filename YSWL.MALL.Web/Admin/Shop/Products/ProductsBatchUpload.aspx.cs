using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using System.Data.OleDb;
using System.IO;
using System.Data.Odbc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class ProductsBatchUpload : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 470; } } //Shop_批量上传页
        YSWL.MALL.BLL.Shop.Products.ProductInfo bll = new ProductInfo();
        protected string UploadPath = "/Upload/Shop/Files/";
        protected string ExField = "";

        protected void Page_Load(object sender, EventArgs e)
        {
        }
       
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            downTipBut.Visible = false;
            hidDownUrl.Value = ""; 
            string FileName = Path.GetFileName(upload.PostedFile.FileName);
            string ErrorMsg = "出现异常，请检查您的数据格式";
            int successCount=0;
            string errorfileurl="";
            int errorCount=0;
            if (!upload.HasFile)
            {
                Common.MessageBox.ShowSuccessTip(this, "请上传文件");
                return;
            }
            string[] AllowFileExt = ".xls|.xlsx|.csv".Split('|');
            string ext = System.IO.Path.GetExtension(FileName);
            ext = String.IsNullOrWhiteSpace(ext) ? "" : ext.ToLower();
            if (!AllowFileExt.Contains(ext) || String.IsNullOrWhiteSpace(ext))
            {
                Common.MessageBox.ShowSuccessTip(this, "上传文件格式不正确");
                return;
            } 
            upload.PostedFile.SaveAs(Server.MapPath(UploadPath) + FileName);
            if (Excel(UploadPath, FileName, ref ErrorMsg, ref successCount ,ref errorfileurl, ref errorCount))
            {
                string tip="成功插入" + successCount + "条数据";
                if (errorCount > 0)
                {
                    tip += "失败"+errorCount+"条数据";
                    hidDownUrl.Value = errorfileurl;
                    downTipBut.Visible = true;
                }
                Common.MessageBox.ShowSuccessTip(this, tip);
               
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "插入失败,信息:" + ErrorMsg + "提示：检查填写的数据格式，请严格按照原模板数据填写");
            }
        }
        /// <summary>
        /// 读取文件，导入DataSet
        /// </summary>
        /// <param name="Path">csv的路径</param>
        /// <param name="FileName">文件的名称</param>
        /// <param name="ErrorMsg">错误信息</param>
        /// <returns></returns>
        public bool Excel(string Path, string FileName, ref string ErrorMsg, ref  int successCount, ref string errorfileurl, ref int errorCount)
        {
            string sheetName = "ImportProduct";
            DataSet data = new DataSet();
            using (FileStream fs = new FileStream(Server.MapPath(Path + FileName), FileMode.OpenOrCreate))
            {
                try
                {
                //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                //获取excel的第一个sheet
                ISheet sheet = workbook.GetSheet(sheetName);

                DataTable table = new DataTable();
                //获取sheet的首行
                IRow headerRow = sheet.GetRow(0);

                //一行最后一个方格的编号 即总的列数
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                //最后一列的标号  即总的行数
                //int rowCount = sheet.LastRowNum;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType==CellType.NUMERIC&& DateUtil.IsCellDateFormatted(row.GetCell(j)))
                            {
                                dataRow[j] = row.GetCell(j).DateCellValue;
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }

                    }
                    table.Rows.Add(dataRow);
                }
                data.Tables.Add(table);
                BLL.Shop.Products.ProductManage.ImportProduct(table, ref successCount, ref  errorfileurl, ref  errorCount);
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                    return false;
                }
            }
             
        }

        //下载新增失败数据
        protected void downTipBut_Click(object sender, EventArgs e)
        {
            string path = hidDownUrl.Value;
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }
            try
            {
                Response.ClearHeaders();
                Response.Clear();
                Response.ContentType = "text/HTML;charset=gbk";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.txt", System.Web.HttpUtility.UrlEncode("ImportProductLog_"+DateTime.Now.ToString("yyyyMMdd"), System.Text.Encoding.UTF8)));
                string filename = Server.MapPath(path);
                Response.TransmitFile(filename);
                Response.End();    
            }
            catch (Exception)
            {
                throw;
            }
        }

          //下载说明文件
        protected void downExplanationBut_Click(object sender, EventArgs e)
        {
                string path = "/Upload/Template/ImportProductExplanation.txt";
                Response.ClearHeaders();
                Response.Clear();
                Response.ContentType = "text/HTML;charset=gbk";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.txt", System.Web.HttpUtility.UrlEncode("ImportProductExplanation", System.Text.Encoding.UTF8)));
                string filename = Server.MapPath(path);
                Response.TransmitFile(filename);
                Response.End();    
        }



        
    }
}