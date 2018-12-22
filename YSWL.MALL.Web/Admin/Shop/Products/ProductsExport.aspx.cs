using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using System.Text;
using System.IO;
using YSWL.Json;
using NPOI.HSSF.UserModel;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class ProductsExport :PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 469; } } //Shop_导出商品页
        YSWL.MALL.BLL.Shop.Products.ProductInfo bll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.ProductCategories productCategory = new BLL.Shop.Products.ProductCategories();
        private BLL.Shop.Products.CategoryInfo manage = new BLL.Shop.Products.CategoryInfo();
        //private string ExFiled = "ExtendCategoryPath,MaxQuantity,MinQuantity,LineId,Meta_Title,Meta_Description,Meta_Keywords,LineId,PenetrationStatus";
        private BLL.Shop.Supplier.SupplierInfo suppBll = new BLL.Shop.Supplier.SupplierInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               // BindProductField();
               // BindCategories();
                BindBrands();
            }
        }
        private void BindBrands()
        {
            YSWL.MALL.BLL.Shop.Products.BrandInfo bll=new BrandInfo();
            DataSet ds = bll.GetAllList();

            if (!DataSetTools.DataSetIsNull(ds))
            {
                drpProductBrand.DataSource = ds;
                drpProductBrand.DataTextField = "BrandName";
                drpProductBrand.DataValueField = "BrandId";
                drpProductBrand.DataBind();
            }

            this.drpProductBrand.Items.Insert(0, new ListItem("请选择", string.Empty));
        }
 
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(GetSelIDlist()))
            {
                string idlist = GetSelIDlist();
                if (idlist.Trim().Length == 0) return;
                bll.UpdateList(idlist, YSWL.MALL.Model.Shop.Products.ProductSaleStatus.Deleted);               
                suppBll.UpdateProductCountList(idlist);
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
                gridView.OnBind();
            }
            else
            {
                gridView.OnBind(); return;
            }
        }
        #region DataBind
        public void BindData()
        {
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            int IntCate = Common.Globals.SafeInt(CategoriesDropList1.SelectedValue, 0);
            int IntBrand = Common.Globals.SafeInt(drpProductBrand.SelectedValue, 0);
            int IntRegion = Common.Globals.SafeInt(hfSelectedNode.Value, 0);
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" ProductName like '%{0}%'", Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append(" AddedDate>='" + Common.InjectionFilter.SqlFilter(txtBeginTime.Text.Trim()) + "' ");
            }
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                if (YSWL.DBUtility.PubConstant.IsSQLServer)
                {
                    strWhere.AppendFormat("  AddedDate< dateadd(day,1,'{0}')", txtEndTime.Text.Trim());
                }
                else
                {
                    strWhere.AppendFormat("  AddedDate< DATE_ADD('{0}',INTERVAL 1 DAY)", txtEndTime.Text.Trim());
                }
            }
            if (IntCate > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(
                   "  EXISTS ( SELECT *  FROM   PMS_ProductCategories WHERE  ProductId =PMS_Products.ProductId  ");
                strWhere.AppendFormat(
              "   AND ( CategoryPath LIKE ( SELECT Path FROM PMS_Categories WHERE CategoryId = {0}  ) + '|%' ",
              IntCate);
                strWhere.AppendFormat(" OR PMS_ProductCategories.CategoryId = {0}))", IntCate);

            }
            if (IntRegion > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  RegionId=" + IntRegion + " ");
            }

            if (IntBrand > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  BrandId=" + IntBrand + " ");
            }

            ds = bll.GetList(strWhere.ToString());
            gridView.DataSetSource = ds;
        }
        
        #endregion
        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HidSelectValue.Value = string.IsNullOrEmpty(HidSelectValue.Value)
                                       ? HidSelectValue.Value.TrimEnd(',').TrimStart(',')
                                       : "" + GetSelIDlist();
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkbtnDel = (LinkButton)e.Row.FindControl("LinkButton1");

                Literal litProductCate = (Literal)e.Row.FindControl("litProductCate");
                object productId = DataBinder.Eval(e.Row.DataItem, "ProductId");
                if (productId != null)
                {
                    litProductCate.Text = ProductCategories(Common.Globals.SafeLong(productId.ToString(), 0));
                }
            }
        }


        /// <summary>
        /// 获取商品所在分类信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private string ProductCategories(long productId)
        {
            List<Model.Shop.Products.ProductCategories> list = productCategory.GetModelList(productId);
            StringBuilder strName = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (Model.Shop.Products.ProductCategories productCategoriese in list)
                {
                    strName.Append(manage.GetFullNameByCache(productCategoriese.CategoryId));
                    strName.Append("</br>");
                }
            }
            return strName.ToString();
        }

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (gridView.DataKeys[i].Value != null)
                    {
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }





        //public void BindProductField()
        //{
           // StringBuilder strbData = new StringBuilder();
           //// DataTable dt = bll.GetTableSchemaEx().Tables[0];
           // DataTable dt = bll.GetTableHead().Tables[0];
           //// string text;
           //// StringBuilder  value;
           // ListItem item1 = new ListItem("类别ID", "CategoryId");
           // ListItem item2 = new ListItem();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (!ExFiled.Contains(dr["column"].ToString()))
            //    {
            //        //text = dr["column"].ToString() + "[" + dr["column_desc"].ToString() + "]";
            //        text = "[" + dr["column_desc"].ToString() + "]";
            //        value = new StringBuilder();
            //        value.Append(dr["column"].ToString());
            //        value.Append(" As ");
            //        value.Append(GetTrueValue(dr["column_desc"].ToString()));
            //        ListItem item = new ListItem(text, value.ToString());
            //        item.Attributes[dr["column"].ToString()] = dr["column_desc"].ToString();
            //        chkTableField.Items.Add(item);
            //    }
            //    text = "";
            //    value=new StringBuilder();

            //}
            //chkTableField.DataBind();
        //}
        

        public string GetTrueValue(string str)
        {
            if (str.Contains(" "))
            {
                return str.Split(' ')[0];
            }
            return str;
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HidSelectValue.Value))
            {
                Common.MessageBox.ShowFailTip(this,"请选择要导出的数据");
                return;
            }
            StringBuilder sbSql = new StringBuilder();
            //sbSql.Append(" ProductId as 商品ID , ");
            //sbSql.Append(" '' as 商品分类 , ");
            foreach (ListItem listItem in chkTableField.Items)
            {
                if (listItem.Selected)
                {
                    sbSql.Append(listItem.Value);
                    sbSql.Append(" As "+listItem.Text);
                    sbSql.Append(",");
                }
            }
            if (sbSql.Length<=0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择要导出的字段");
                return;
            }
            string sqlFiled = " ProductId as 商品ID,'' as 商品分类," + sbSql;
            DataSet ds=new DataSet();
            string Ids = !string.IsNullOrEmpty(HidSelectValue.Value) ? HidSelectValue.Value.TrimEnd(',').TrimStart(',') : "";
            ds= bll.GetList(Ids, sqlFiled);
            DataSetToExcel(ds);

        }

        protected long StockNum(object obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    return StockNum( Common.Globals.SafeLong(obj.ToString(), 0));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        protected long StockNum(long productId)
        {
             return bll.StockNum(productId);
        }

        public string StrCate(object obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    BLL.Shop.Products.CategoryInfo manage = new BLL.Shop.Products.CategoryInfo();
                    return manage.GetFullNameByCache(int.Parse(obj.ToString()));
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DS"></param>
        public void DataSetToExcel(DataSet DS)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("商品表");
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in DS.Tables[0].Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            int rowIndex = 1;
            long pid = 0;
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in DS.Tables[0].Columns)
                {
                    switch (column.ColumnName)
                    { 
                        // 商品库存
                        case "商品库存":
                            pid=  Globals.SafeLong(row["商品ID"], 0);
                        if (pid > 0) {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(StockNum(pid));
                        }
                            break;
                        case "是否有SKU":
                            dataRow.CreateCell(column.Ordinal).SetCellValue(Globals.SafeBool(row[column].ToString(), false)?"有":"无");
                            break;
                        default:
                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            break;
                    }
                    if (column.ColumnName == "商品库存")
                    {
                        pid=  Globals.SafeLong(row["商品ID"], 0);
                        if (pid > 0) {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(StockNum(pid));
                        }
                    }
                    else if (column.ColumnName == "商品分类")
                    {
                        pid = Globals.SafeLong(row["商品ID"], 0);
                        if (pid > 0)
                        {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(ProductCategoriesExport(pid));
                        }
                    }
                    else if (column.ColumnName == "是否有SKU")
                    {

                    } else {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                }
                dataRow = null;
                rowIndex++;
            }
            Response.Clear();
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            headerRow = null;
            sheet = null;
            workbook = null;
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Product.xls"));
            Response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
            Response.End();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        protected void btnFresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductsExport.aspx");

        }

        /// <summary>
        /// 一键导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void exportAll_Click(object sender, EventArgs e)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@" ProductId as 商品ID 
                          ,TypeId As 类型ID,BrandId As 品牌ID,ProductName As 名称,'' as 商品分类
                          ,ProductCode As 编码,SupplierId As 供应商Id,RegionId As 地区Id
                          ,Unit As 单位,Description As 描述,SaleStatus As 状态,AddedDate As 新增日期
                          ,VistiCounts As 访问次数,SaleCounts As 售出总数,Stock As 商品库存,MarketPrice As 市场价
                          ,LowestSalePrice As 最低价,HasSKU As 是否有SKU,Points As 积分,ImageUrl As 原图路径
                          ,ThumbnailUrl1 As 缩略图路径");
            DataSet ds = new DataSet();
            ds = bll.GetList("", sbSql.ToString());
            DataSetToExcel(ds);
        }

        /// <summary>
        /// 导出上传文件模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void exportImport_Click(object sender, EventArgs e)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@" T.ProductId,SPC.CategoryId,TypeId,BrandId,ProductName,SS.SKU as ProductCode
                            ,Description,SaleStatus,AddedDate,SS.Stock,SS.Weight,MarketPrice,SS.CostPrice,SS.SalePrice as LowestSalePrice
                        ,Points,ImageUrl,ThumbnailUrl1");
            DataSet ds = new DataSet();
            ds = bll.GetListExport("", sbSql.ToString());
            DataSetToTemplateExcel(ds);
        }

        public void DataSetToTemplateExcel(DataSet DS)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("ImportProduct");
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in DS.Tables[0].Columns)
            {
                //if (column.ColumnName == "ProductId") continue;
                if(column.ColumnName== "CategoryId")
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName+"s");
                    continue;
                }
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            int rowIndex = 1;
            long pid = 0;
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in DS.Tables[0].Columns)
                {
                    switch (column.ColumnName)
                    {
                        // 商品库存
                        case "Stock":
                            pid = Globals.SafeLong(row["ProductId"], 0);
                            if (pid > 0)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(StockNum(pid));
                            }
                            break;
                        default:
                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            break;
                    }

                    if (column.ColumnName == "Stock")
                    {
                        pid = Globals.SafeLong(row["ProductId"], 0);
                        if (pid > 0)
                        {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(StockNum(pid));
                        }
                    }
                    else
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                }
                dataRow = null;
                rowIndex++;
            }
            Response.Clear();
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            headerRow = null;
            sheet = null;
            workbook = null;
            Response.AddHeader("Content-Disposition", "attachment; filename=Product.xls");
            Response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
            Response.End();

        }
        /// <summary>
        /// 获取商品所在分类信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private string ProductCategoriesExport(long productId)
        {
            List<Model.Shop.Products.ProductCategories> list = productCategory.GetModelList(productId);
            StringBuilder strName = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (Model.Shop.Products.ProductCategories productCategoriese in list)
                {
                    strName.Append(manage.GetFullNameByCache(productCategoriese.CategoryId));
                    strName.Append("》");
                }
            }
            return strName.ToString().Replace("&raquo;","》").TrimEnd('》');
        }
    }
}