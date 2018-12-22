using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace YSWL.Web.Controls
{
    public enum CheckColumnAlign
    {
        Left, Right, Center
    }
    public enum CheckColumnVAlign
    {
        Top, Middle, Bottom
    }
    #region 多语言文字
    public interface IGridViewUIText
    {
        string ExportExcel { get; }
        string ExportWord { get; }
        string First { get; }
        string Previous { get; }
        string Next { get; }
        string Last { get; }
        string Page { get; }
        string Record { get; }
    }
    #endregion

    public delegate void BindEventHandler();
    [ToolboxData(@"<{0}:GridViewEx runat='server'></{0}:GridViewEx>")]
    public abstract class GridViewExBase : GridView
    {
        private IGridViewUIText _gridViewUiText;
        private SortTip sortTip;
        /// <summary>
        /// 排序提示信息
        /// </summary>
        [Description("排序提示信息"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public SortTip SortTip
        {
            get
            {
                if (sortTip == null)
                {
                    sortTip = new SortTip();
                }
                return sortTip;
            }
            set
            {
                sortTip = value;
            }
        }
        public string CheckBoxID
        {
            get
            {
                return "ItemCheckBox";
            }
        }
        //增加了一个设置是否显示“导出Word”按钮的属性
        /// <summary>
        /// 排序提示信息
        /// </summary>
        [Description("显示导出到Word"), Category("扩展"), DefaultValue(true)]
        public virtual bool ShowExportWord
        {
            get
            {
                object obj2 = this.ViewState["ShowExportWord"];
                if (obj2 != null)
                {
                    return (bool)obj2;
                }
                return true;
            }
            set
            {
                bool aShowExportWord = this.ShowExportWord;
                if (value != aShowExportWord)
                {
                    this.ViewState["ShowExportWord"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }
        //增加了一个设置是否显示“导出Excel”按钮的属性
        [Description("显示导出到Excel"), Category("扩展"), DefaultValue(true)]
        public virtual bool ShowExportExcel
        {
            get
            {
                object obj2 = this.ViewState["ShowExportExcel"];
                if (obj2 != null)
                {
                    return (bool)obj2;
                }
                return true;
            }
            set
            {
                bool aShowExportExcel = this.ShowExportExcel;
                if (value != aShowExportExcel)
                {
                    this.ViewState["ShowExportExcel"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }

        //是否显示全选
        [Description("显示全选列"), Category("扩展"), DefaultValue(false)]
        public virtual bool ShowCheckAll
        {
            get
            {
                object obj2 = this.ViewState["ShowCheckAll"];
                if (obj2 != null)
                {
                    return (bool)obj2;
                }
                return false;

            }
            set
            {
                bool aShowCheckAll = this.ShowCheckAll;
                if (value != aShowCheckAll)
                {
                    this.ViewState["ShowCheckAll"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }


        [Description("全选列的位置"), Category("扩展"), DefaultValue(CheckColumnAlign.Left)]
        public virtual CheckColumnAlign CheckColumnAlign
        {
            get
            {
                object obj2 = this.ViewState["CheckColumnAlign"];
                if (obj2 != null)
                {
                    return (CheckColumnAlign)obj2;
                }
                return CheckColumnAlign.Left;
            }
            set
            {
                CheckColumnAlign aCheckColumnAlign = this.CheckColumnAlign;
                if (value != aCheckColumnAlign)
                {
                    this.ViewState["CheckColumnAlign"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }

        [Description("选择列的位置"), Category("扩展"), DefaultValue(CheckColumnVAlign.Middle)]
        public virtual CheckColumnVAlign CheckColumnVAlign
        {
            get
            {
                object obj2 = this.ViewState["CheckColumnVAlign"];
                if (obj2 != null)
                {
                    return (CheckColumnVAlign)obj2;
                }
                return CheckColumnVAlign.Middle;
            }
            set
            {
                CheckColumnVAlign aCheckColumnAlign = this.CheckColumnVAlign;
                if (value != aCheckColumnAlign)
                {
                    this.ViewState["CheckColumnVAlign"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }

        [Browsable(false)]
        [Description("自定义分页-页码"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int PageIndex
        {
            get
            {
                int pageIndex = Common.Globals.SafeInt(this.ViewState["CustomPageIndex"], -1);
                if (pageIndex < 0)
                    return base.PageIndex;
                return pageIndex;
            }
            set
            {
                this.ViewState["CustomPageIndex"] = value;
                base.PageIndex = value;
            }
        }
        [Browsable(false)]
        [Description("自定义分页-总页数"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int PageCount
        {
            get
            {
                int pageCount = Common.Globals.SafeInt(this.ViewState["CustomPageCount"], 0);
                if (pageCount < 1)
                    return base.PageCount;
                return pageCount;
            }
        }

        [Browsable(false)]
        [Description("自定义分页-页码数"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageNum
        {
            get
            {
                int pageNum = Common.Globals.SafeInt(this.ViewState["CustomPageNum"], 0);
                if (pageNum < 1) return 10;
                return pageNum;
            }
            set
            {
                this.ViewState["CustomPageNum"] = value;
            }
        }

        [Browsable(false)]
        [Description("自定义分页-总条数"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ToalCount
        {
            get
            {
                long toalCount = Common.Globals.SafeLong(this.ViewState["CustomPageToalCount"], 0);
                if (toalCount < 1)
                    return DataSetSource.Tables[0].Rows.Count;
                return toalCount;
            }
            set
            {
                this.ViewState["CustomPageToalCount"] = value;
                this.ViewState["CustomPageCount"] = (int)Math.Ceiling((value * 1.0) / this.PageSize);
            }
        }

        private bool hasCustomPage { get { return this.ViewState["CustomPageToalCount"] != null; } }
        private bool UseRowStyle = true;
        private bool ClearRowStyle = false;

        public GridViewExBase(IGridViewUIText uiText, bool useRowStyle, bool clearRowStyle)
        {
            _gridViewUiText = uiText;
            this.AutoGenerateColumns = false;
            this.AllowSorting = true;
            this.AllowPaging = true;
            this.UseRowStyle = useRowStyle;
            this.ClearRowStyle = clearRowStyle;
            this.ShowHeaderWhenEmpty = true;
        }
        public GridViewExBase(IGridViewUIText uiText)
        {
            _gridViewUiText = uiText;
            this.AutoGenerateColumns = false;
            this.AllowSorting = true;
            this.AllowPaging = true;
            this.ShowHeaderWhenEmpty = true;
        }

        #region OnBind
        public event BindEventHandler Bind;
        public virtual void OnBind()
        {
            if (Bind != null)
            {
                Bind();

                if (DataSetSource != null)
                {
                    long rows_Count = ToalCount;
                    DataView dv = DataSetSource.Tables[0].DefaultView;
                    string sortStr = "";
                    if (rows_Count != 0)
                    {
                        if (!string.IsNullOrWhiteSpace(SortExpressionStr))
                        {
                            sortStr = SortExpressionStr + " " + SortDirectionStr;
                        }

                        if (!string.IsNullOrWhiteSpace(sortStr))
                        {
                            dv.Sort = sortStr;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(FilterExpressionStr))
                    {
                        dv.RowFilter = FilterExpressionStr;
                    }
                    this.DataSource = dv;
                    //rows_Count = dv.ToTable().Rows.Count;
                    this.DataBind();
                    if (this.Controls.Count > 0)
                    {
                        Table t = this.Controls[0] as Table;
                        if (t != null)
                        {
                            foreach (TableRow r in t.Rows)
                            {

                                foreach (TableCell c in r.Cells)
                                {
                                    c.Wrap = Wrap;
                                }
                            }
                        }
                    }

                    //分页
                    int page_Size = this.PageSize;
                    int page_Count = this.PageCount;
                    int page_Current = this.PageIndex + 1;

                    lblRowsCount.Text = rows_Count.ToString();
                    lblPageCount.Text = page_Count.ToString();
                    lblCurrentPage.Text = page_Current.ToString();

                    #region 显示页导航

                    btnFirst.Enabled = true;
                    btnPrev.Enabled = true;
                    btnNext.Enabled = true;
                    btnLast.Enabled = true;

                    if (this.PageIndex == 0)
                    {
                        btnFirst.Enabled = false;
                        btnPrev.Enabled = false;
                        if (page_Count == 0)
                        {
                            btnNext.Enabled = false;
                            btnLast.Enabled = false;
                        }
                        if (page_Count == 1)
                        {
                            btnLast.Enabled = false;
                            btnNext.Enabled = false;
                        }
                    }
                    else if (this.PageIndex == page_Count - 1)
                    {
                        btnLast.Enabled = false;
                        btnNext.Enabled = false;
                    }
                    #endregion


                    #region 显示Foot页导航

                    btnFirstFoot.Enabled = true;
                    btnPrevFoot.Enabled = true;
                    btnNextFoot.Enabled = true;
                    btnLastFoot.Enabled = true;

                    if (this.PageIndex == 0)
                    {
                        btnFirstFoot.Enabled = false;
                        btnPrevFoot.Enabled = false;
                        if (page_Count == 0)
                        {
                            btnNextFoot.Enabled = false;
                            btnLastFoot.Enabled = false;
                        }
                        if (page_Count == 1)
                        {
                            btnLastFoot.Enabled = false;
                            btnNextFoot.Enabled = false;
                        }
                    }
                    else if (this.PageIndex == page_Count - 1)
                    {
                        btnLastFoot.Enabled = false;
                        btnNextFoot.Enabled = false;
                    }
                    #endregion
                }

            }
        }
        #endregion
        [Description("过滤条件表达式"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual string FilterExpressionStr
        {
            get
            {
                if (ViewState["FilterExpression"] == null)
                {
                    return null;
                }
                return ViewState["FilterExpression"].ToString();
            }
            set
            {
                ViewState["FilterExpression"] = value;
            }
        }

        [Description("排序表达式"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual string SortExpressionStr
        {
            get
            {
                if (ViewState["SortExpression"] == null)
                {
                    return null;
                }
                return ViewState["SortExpression"].ToString();
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }
        [Description("排序方向"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual string SortDirectionStr
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    return "DESC";
                }
                if (ViewState["SortDirection"].ToString().ToLower() != "asc" && ViewState["SortDirection"].ToString().ToLower() != "desc")
                {
                    return "DESC";
                }
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        [Description("显示上部的工具栏"), Category("扩展"), DefaultValue(true)]
        public bool ShowToolBar
        {
            get
            {
                if (ViewState["ShowToolBar"] != null)
                {
                    return Convert.ToBoolean(ViewState["ShowToolBar"]);
                }
                return true;
            }
            set
            {
                ViewState["ShowToolBar"] = value;
            }
        }

        [Description("显示页脚的上下页导航"), Category("扩展"), DefaultValue(false)]
        public bool ShowFootPageButton
        {
            get
            {
                if (ViewState["ShowFootPageButton"] != null)
                {
                    return Convert.ToBoolean(ViewState["ShowFootPageButton"]);
                }
                return false;
            }
            set
            {
                ViewState["ShowFootPageButton"] = value;
            }
        }


        [Description("显示网格线"), Category("扩展"), DefaultValue(true)]
        public bool ShowGridLine
        {
            get
            {
                if (ViewState["ShowGridLine"] != null)
                {
                    return Convert.ToBoolean(ViewState["ShowGridLine"]);
                }
                return true;
            }
            set
            {
                ViewState["ShowGridLine"] = value;
            }
        }

        [Description("显示头样式"), Category("扩展"), DefaultValue(true)]
        public bool ShowHeaderStyle
        {
            get
            {
                if (ViewState["ShowHeaderStyle"] != null)
                {
                    return Convert.ToBoolean(ViewState["ShowHeaderStyle"]);
                }
                return true;
            }
            set
            {
                ViewState["ShowHeaderStyle"] = value;
            }
        }

        [Description("行高"), Category("扩展"), DefaultValue(27)]
        public string RowHeight
        {
            get
            {
                if (ViewState["RowHeight"] != null)
                {
                    return ViewState["RowHeight"].ToString();
                }
                return "27px";
            }
            set
            {
                ViewState["RowHeight"] = value;
            }
        }




        [Description("单元格是否换行"), Category("扩展"), DefaultValue(true)]
        public bool Wrap
        {
            get
            {
                if (ViewState["Wrap"] != null)
                {
                    return Convert.ToBoolean(ViewState["Wrap"]);
                }
                return true;
            }
            set
            {
                ViewState["Wrap"] = value;
            }
        }
        Table table = new Table();



        #region OnInit
        LinkButton btnExportWord;
        LinkButton btnExport;
        Label lblCurrentPage;
        Label lblPageCount;
        Label lblRowsCount;
        LinkButton btnFirst;
        LinkButton btnPrev;
        LinkButton btnNext;
        LinkButton btnLast;
        LinkButton btnFirstFoot;
        LinkButton btnPrevFoot;
        LinkButton btnNextFoot;
        LinkButton btnLastFoot;
        protected override void OnInit(EventArgs e)
        {
            this.EnableViewState = true;


            btnExport = new LinkButton();
            btnExport.CommandName = "ExportToExcel";
            btnExport.EnableViewState = true;
            btnExport.Text = _gridViewUiText.ExportExcel;
            btnExport.CausesValidation = false;

            btnExportWord = new LinkButton();
            btnExportWord.CommandName = "ExportToWord";
            btnExportWord.EnableViewState = true;
            btnExportWord.Text = _gridViewUiText.ExportWord;
            btnExportWord.CausesValidation = false;

            lblCurrentPage = new Label();
            if (UseRowStyle)
            {
                lblCurrentPage.ForeColor = ColorTranslator.FromHtml("#e78a29");
            }

            lblCurrentPage.Text = "1";

            lblPageCount = new Label();
            lblPageCount.Text = "1";


            lblRowsCount = new Label();
            if (UseRowStyle)
            {
                lblRowsCount.ForeColor = ColorTranslator.FromHtml("#e78a29");
            }

            btnFirst = new LinkButton();
            btnFirst.Text = _gridViewUiText.First;
            btnFirst.Command += new CommandEventHandler(NavigateToPage);
            btnFirst.CommandName = "Pager";
            btnFirst.CommandArgument = "First";
            btnFirst.CausesValidation = false;

            btnPrev = new LinkButton();
            btnPrev.Text = _gridViewUiText.Previous;
            btnPrev.Command += new CommandEventHandler(NavigateToPage);
            btnPrev.CommandName = "Pager";
            btnPrev.CommandArgument = "Prev";
            btnPrev.CausesValidation = false;

            btnNext = new LinkButton();
            btnNext.Text = _gridViewUiText.Next;
            btnNext.Command += new CommandEventHandler(NavigateToPage);
            btnNext.CommandName = "Pager";
            btnNext.CommandArgument = "Next";
            btnNext.CausesValidation = false;

            btnLast = new LinkButton();
            btnLast.Text = _gridViewUiText.Last;
            btnLast.Command += new CommandEventHandler(NavigateToPage);
            btnLast.CommandName = "Pager";
            btnLast.CommandArgument = "Last";
            btnLast.CausesValidation = false;

            #region foot
            btnFirstFoot = new LinkButton();
            btnFirstFoot.Text = _gridViewUiText.First;
            btnFirstFoot.Command += new CommandEventHandler(NavigateToPageFoot);
            btnFirstFoot.CommandName = "Pager";
            btnFirstFoot.CommandArgument = "First";
            btnFirstFoot.CausesValidation = false;

            btnPrevFoot = new LinkButton();
            btnPrevFoot.Text = _gridViewUiText.Previous;
            btnPrevFoot.Command += new CommandEventHandler(NavigateToPageFoot);
            btnPrevFoot.CommandName = "Pager";
            btnPrevFoot.CommandArgument = "Prev";
            btnPrevFoot.CausesValidation = false;

            btnNextFoot = new LinkButton();
            btnNextFoot.Text = _gridViewUiText.Next;
            btnNextFoot.Command += new CommandEventHandler(NavigateToPageFoot);
            btnNextFoot.CommandName = "Pager";
            btnNextFoot.CommandArgument = "Next";
            btnNextFoot.CausesValidation = false;

            btnLastFoot = new LinkButton();
            btnLastFoot.Text = _gridViewUiText.Last;
            btnLastFoot.Command += new CommandEventHandler(NavigateToPageFoot);
            btnLastFoot.CommandName = "Pager";
            btnLastFoot.CommandArgument = "Last";
            btnLastFoot.CausesValidation = false;
            #endregion



            base.OnInit(e);

            this.BorderWidth = new Unit(1);
        }
        #endregion


        DataSet _ds;
        [Description("自定义的DataSet类型数据源"), Category("扩展"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual DataSet DataSetSource
        {
            get
            {
                return _ds;
            }
            set
            {
                _ds = value;
            }
        }
        #region NavigateToPage
        public void NavigateToPage(object sender, CommandEventArgs e)
        {
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            switch (e.CommandArgument.ToString())
            {
                case "Prev":
                    if (this.PageIndex > 0)
                    {
                        this.PageIndex -= 1;

                    }
                    break;
                case "Next":
                    if (this.PageIndex < (this.PageCount - 1))
                    {
                        this.PageIndex += 1;

                    }
                    break;
                case "First":
                    this.PageIndex = 0;
                    break;
                case "Last":
                    this.PageIndex = this.PageCount - 1;
                    break;
            }
            if (this.PageIndex == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                if (this.PageCount == 1)
                {
                    btnLast.Enabled = false;
                    btnNext.Enabled = false;
                }
            }
            else if (this.PageIndex == this.PageCount - 1)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            OnBind();
        }

        public void NavigateToPageFoot(object sender, CommandEventArgs e)
        {
            btnFirstFoot.Enabled = true;
            btnPrevFoot.Enabled = true;
            btnNextFoot.Enabled = true;
            btnLastFoot.Enabled = true;
            switch (e.CommandArgument.ToString())
            {
                case "Prev":
                    if (this.PageIndex > 0)
                    {
                        this.PageIndex -= 1;

                    }
                    break;
                case "Next":
                    if (this.PageIndex < (this.PageCount - 1))
                    {
                        this.PageIndex += 1;

                    }
                    break;
                case "First":
                    this.PageIndex = 0;
                    break;
                case "Last":
                    this.PageIndex = this.PageCount - 1;
                    break;
            }
            if (this.PageIndex == 0)
            {
                btnFirstFoot.Enabled = false;
                btnPrevFoot.Enabled = false;
                if (this.PageCount == 1)
                {
                    btnLastFoot.Enabled = false;
                    btnNextFoot.Enabled = false;
                }
            }
            else if (this.PageIndex == this.PageCount - 1)
            {
                btnLastFoot.Enabled = false;
                btnNextFoot.Enabled = false;
            }
            OnBind();
        }


        #endregion


        #region OnRowCreated
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (UseRowStyle)
            {
                //this.Style.Add("background-color", "#e6eff8");
                this.Attributes.Add("border", "0px");
                this.Attributes.Add("cellpadding", "4px");
                this.Attributes.Add("cellspacing", "1px");
                e.Row.Style.Add("height", RowHeight);
                e.Row.Attributes.Add("height", RowHeight);
            }
            if (ClearRowStyle)
            {
                e.Row.Style.Clear();
                e.Row.Attributes.Remove("height");
                e.Row.Attributes.Remove("style");
            }
            this.Attributes.Add("class", "GridViewTyle");

            try
            {
                //页眉
                //if (e.Row.RowType == DataControlRowType.Header)
                //    AddGlyph(GridView1, e.Row);

                #region 页导航 Pager
                if (e.Row.RowType == DataControlRowType.Pager)
                {
                    TableRow row = e.Row.Controls[0].Controls[0].Controls[0] as TableRow;
                    e.Row.CssClass = "GridViewFootPager";
                    if (hasCustomPage)
                    {
                        row.Controls.Clear();
                        int pageCount = this.PageCount;
                        int pageIndex = this.PageIndex + 1;
                        int pageNum = this.PageNum;

                        // start page index
                        int startPageIndex = pageIndex - (pageNum / 2);
                        if (startPageIndex + pageNum > pageCount)
                            startPageIndex = pageCount + 1 - pageNum;
                        if (startPageIndex < 1)
                            startPageIndex = 1;

                        // end page index
                        int endPageIndex = startPageIndex + pageNum - 1;
                        if (endPageIndex > pageCount)
                            endPageIndex = pageCount;

                        TableCell tc = new TableCell();
                        for (int i = startPageIndex; i <= endPageIndex; i++)
                        {
                            if (i == startPageIndex && startPageIndex > 1)
                            {
                                var index = startPageIndex - 1;
                                if (index < 1) index = 1;
                                LinkButton lbMore = new LinkButton();
                                lbMore.Font.Bold = true;
                                lbMore.Text = "[..]";
                                if (UseRowStyle)
                                {
                                    lbMore.Style.Add("color", "#1317fc");
                                }
                                lbMore.CommandName = "Page";
                                lbMore.CommandArgument = (index).ToString();
                                tc.Controls.Add(lbMore);
                                continue;
                            }
                            if (pageIndex == i)
                            {
                                Label lblpage = new Label();
                                if (UseRowStyle)
                                {
                                    lblpage.ForeColor = ColorTranslator.FromHtml("#e78a29");
                                    lblpage.Font.Bold = true;
                                }
                                lblpage.CssClass = "cur";
                                lblpage.Text = "[" + (pageIndex) + "]";
                                tc.Controls.Add(lblpage);
                                continue;
                            }
                            LinkButton lbpage = new LinkButton();

                            lbpage.Text = "[" + (i) + "]";
                            if (UseRowStyle)
                            {
                                lbpage.Font.Bold = true;
                                lbpage.Style.Add("color", "#1317fc");
                            }
                            lbpage.CommandName = "Page";
                            lbpage.CommandArgument = (i).ToString();
                            tc.Controls.Add(lbpage);

                            if (i == endPageIndex && endPageIndex < pageCount)
                            {
                                var index = startPageIndex + pageNum;
                                if (index > pageCount) { index = pageCount; }

                                LinkButton lbMore = new LinkButton();
                                lbMore.Font.Bold = true;
                                lbMore.Text = "[..]";
                                if (UseRowStyle)
                                {
                                    lbMore.Style.Add("color", "#1317fc");
                                }

                                lbMore.CommandName = "Page";
                                lbMore.CommandArgument = (index).ToString();
                                tc.Controls.Add(lbMore);
                                break;
                            }
                        }
                        row.Controls.Add(tc);
                    }
                    else
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            Control lb = cell.Controls[0];
                            if (lb is Label)
                            {
                                Label lblpage = (Label)lb;
                                if (UseRowStyle)
                                {
                                    lblpage.ForeColor = ColorTranslator.FromHtml("#e78a29");
                                    lblpage.Font.Bold = true;
                                }
                                lblpage.Text = "[" + lblpage.Text + "]";
                            }
                            else
                                if (lb is LinkButton)
                            {
                                LinkButton lblpage = (LinkButton)lb;
                                if (UseRowStyle)
                                {
                                    lblpage.Style.Add("color", "#1317fc");
                                    lblpage.Font.Bold = true;
                                }
                                lblpage.Text = "[" + lblpage.Text + "]";
                            }
                        }
                    }

                    #region 分页导航
                    if (ShowFootPageButton)
                    {
                        TableCell cellbtnFirst = new TableCell();
                        cellbtnFirst.Controls.Add(btnFirstFoot);
                        row.Cells.AddAt(0, cellbtnFirst);

                        TableCell cellbtnPrev = new TableCell();
                        cellbtnPrev.Controls.Add(btnPrevFoot);
                        row.Cells.AddAt(1, cellbtnPrev);


                        TableCell cellbtnNext = new TableCell();
                        cellbtnNext.Controls.Add(btnNextFoot);
                        row.Cells.Add(cellbtnNext);


                        TableCell cellbtnLast = new TableCell();
                        cellbtnLast.Controls.Add(btnLastFoot);
                        row.Cells.Add(cellbtnLast);

                    }
                    #endregion


                }
                #endregion


                if (ShowCheckAll)
                {
                    #region
                    GridViewRow row = e.Row;

                    if (row.RowType == DataControlRowType.Header)
                    {
                        row.CssClass = "GridViewHeader";
                        TableCell cell = new TableCell();
                        cell.Wrap = Wrap;
                        cell.Style.Clear();
                        cell.Width = Unit.Pixel(18);
                        if (UseRowStyle)
                        {
                            cell.Style.Add("padding-left", "5px");
                        }
                        if (ClearRowStyle) cell.Style.Clear();
                        cell.CssClass = "input_middle";
                        cell.Text = " <input id='Checkbox2' type='checkbox' onclick='CheckAll(this)'/>";
                        if (CheckColumnAlign == CheckColumnAlign.Left)
                        {
                            row.Cells.AddAt(0, cell);
                        }
                        else
                        {
                            int index = row.Cells.Count;
                            row.Cells.AddAt(index, cell);
                        }
                    }
                    else if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.CssClass = "GridViewDataRow";
                        TableCell cell = new TableCell();

                        cell.Wrap = Wrap;
                        switch (CheckColumnVAlign)
                        {
                            case CheckColumnVAlign.Top:
                                cell.VerticalAlign = VerticalAlign.Top;
                                break;
                            case CheckColumnVAlign.Middle:
                                cell.VerticalAlign = VerticalAlign.Middle;
                                break;
                            case CheckColumnVAlign.Bottom:
                                cell.VerticalAlign = VerticalAlign.Bottom;
                                break;
                        }

                        CheckBox cb = new CheckBox();
                        //if (UseRowStyle)
                        //{
                            cell.Width = Unit.Pixel(18);
                        //}
                        cell.Style.Clear();
                        cell.CssClass = "input_middle";
                        cb.ID = "ItemCheckBox";
                        cell.Controls.Add(cb);
                        if (CheckColumnAlign == CheckColumnAlign.Left)
                        {
                            row.Cells.AddAt(0, cell);
                        }
                        else
                        {
                            int index = row.Cells.Count;
                            row.Cells.AddAt(index, cell);
                        }
                    }
                    #endregion
                }


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region
                    if (UseRowStyle)
                    {
                        e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#CBE3F4';this.style.cursor='pointer';");//#F4F4F4
                                                                                                                                                                         //当鼠标移走时还原该行的背景色
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                        //e.Row.Style.Add("background-color", "#FFFFFF");
                        //e.Row.Style.Add("color", "#666666");
                    }



                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (ShowGridLine && UseRowStyle)
                        {
                            //tc.Style.Add("border-bottom", "1px dashed #b8dae9");
                        }
                        if (UseRowStyle)
                        {
                            tc.Style.Add("padding-left", "5px");

                            tc.Style.Add("height", RowHeight);
                            tc.Attributes.Add("height", RowHeight);
                        }
                        if (ClearRowStyle) tc.Style.Clear();
                        tc.Attributes.Add("class", "text");
                    }

                    if (UseRowStyle)
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            foreach (Control c in e.Row.Cells[i].Controls)
                            {
                                if (c is LinkButton)
                                {
                                    LinkButton aLinkButton = c as LinkButton;
                                    aLinkButton.Style.Add("color", "#1317fc");
                                }
                                if (c is HtmlAnchor)
                                {
                                    HtmlAnchor aHtmlAnchor = c as HtmlAnchor;
                                    aHtmlAnchor.Style.Add("color", "#1317fc");
                                }
                            }
                        }
                    }

                    #endregion

                }


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    #region

                    if (UseRowStyle)
                    {
                        string headerRowHeight = (this.HeaderStyle.Height == Unit.Empty
                                                ? Unit.Pixel(35)
                                                : this.HeaderStyle.Height).ToString();
                        e.Row.Style.Add("height", headerRowHeight);
                        e.Row.Attributes.Add("height", headerRowHeight);
                        //e.Row.Style.Add("background-color", "#E3EFFF");//f4f6f8
                        e.Row.Style.Add("color", "#003366");
                        //e.Row.Style.Add("font-weight", "bold");
                    }

                    if (UseRowStyle)
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            if (i == 0)
                            {
                                if (ShowGridLine)
                                {
                                    e.Row.Cells[i].Style.Add("border", "1px solid #dae2e8");
                                }
                                e.Row.Cells[i].Style.Add("border-right", "0px");
                            }
                            else if (i == e.Row.Cells.Count)
                            {
                                if (ShowGridLine)
                                {
                                    e.Row.Cells[i].Style.Add("border", "1px solid #dae2e8");
                                }
                                e.Row.Cells[i].Style.Add("border-left", "0px");
                            }
                            else
                            {
                                if (ShowGridLine)
                                {
                                    e.Row.Cells[i].Style.Add("border", "1px solid #dae2e8");
                                }
                                e.Row.Cells[i].Style.Add("border-left", "0px");
                                e.Row.Cells[i].Style.Add("border-right", "0px");
                            }
                            foreach (Control c in e.Row.Cells[i].Controls)
                            {
                                if (c is LinkButton)
                                {
                                    LinkButton aLinkButton = c as LinkButton;
                                    //aLinkButton.Style.Add("font-weight", "bold");//标题字体加粗
                                    aLinkButton.Style.Add("color", "#003366");
                                }
                                if (c is HtmlAnchor)
                                {
                                    HtmlAnchor aHtmlAnchor = c as HtmlAnchor;
                                    aHtmlAnchor.Style.Add("color", "#003366");
                                }
                            }
                        }
                    }
                    #endregion

                }

                if (this.AllowSorting && !SortTip.IsNotSet)
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            foreach (Control c in cell.Controls)
                            {
                                if (c is LinkButton)
                                {
                                    LinkButton lb = c as LinkButton;
                                    if (lb == null)
                                    {
                                        continue;
                                    }
                                    if (lb.CommandArgument == this.SortExpressionStr)
                                    {
                                        System.Web.UI.WebControls.Image img;
                                        if (this.SortDirectionStr == "DESC")
                                        {
                                            img = new System.Web.UI.WebControls.Image();
                                            img.ImageAlign = ImageAlign.AbsMiddle;
                                            img.ImageUrl = base.ResolveUrl(SortTip.DescImg);
                                        }
                                        else
                                        {
                                            img = new System.Web.UI.WebControls.Image();
                                            img.ImageAlign = ImageAlign.AbsMiddle;
                                            img.ImageUrl = base.ResolveUrl(SortTip.AscImg);
                                        }
                                        if (img != null)
                                        {
                                            cell.Controls.Add(img);
                                        }
                                    }
                                }

                            }
                        }
                    }

                base.OnRowCreated(e);
                if (ClearRowStyle)
                {
                    e.Row.Style.Clear();
                    e.Row.Attributes.Remove("height");
                    e.Row.Attributes.Remove("style");
                }
            }
            catch
            {
            }
        }
        #endregion



        #region CreateChildControls
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int res = base.CreateChildControls(dataSource, dataBinding);
            if (ShowToolBar)
            {
                try
                {
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Pager, DataControlRowState.Normal);
                    TableCell c = new TableCell();
                    c.Width = Unit.Empty;
                    c.ColumnSpan = this.Columns.Count;
                    if (ShowCheckAll)
                    {
                        c.ColumnSpan++;
                    }
                    row.Cells.Add(c);
                    TableCell cell1 = new TableCell();
                    Table table = new Table();
                    TableRow r = new TableRow();
                    table.Rows.Add(r);
                    table.Width = Unit.Percentage(100);
                    c.Controls.Add(table);
                    r.Cells.Add(cell1);

                    Literal l1 = new Literal();
                    l1.Text = _gridViewUiText.Page + "：";
                    cell1.Controls.Add(l1);
                    cell1.Wrap = false;
                    cell1.Controls.Add(lblCurrentPage);
                    l1 = new Literal();
                    l1.Text = "/";
                    cell1.Controls.Add(l1);
                    cell1.Controls.Add(lblPageCount);
                    l1 = new Literal();
                    l1.Text = "，" + _gridViewUiText.Record + ":";
                    cell1.Controls.Add(l1);
                    cell1.Controls.Add(lblRowsCount);
                    l1 = new Literal();
                    l1.Text = "";
                    cell1.HorizontalAlign = HorizontalAlign.Left;
                    cell1.Controls.Add(l1);
                    TableCell cell2 = new TableCell();
                    cell2.HorizontalAlign = HorizontalAlign.Right;
                    cell2.Wrap = false;

                    if (this.ShowExportExcel == true)
                    {
                        l1 = new Literal();
                        l1.Text = " [";
                        cell2.Controls.Add(l1);
                        cell2.Controls.Add(btnExport);
                        l1 = new Literal();
                        l1.Text = "] ";
                        cell2.Controls.Add(l1);
                    }

                    if (this.ShowExportWord == true)
                    {
                        l1 = new Literal();
                        l1.Text = " [";
                        cell2.Controls.Add(l1);
                        cell2.Controls.Add(btnExportWord);
                        l1 = new Literal();
                        l1.Text = "] ";
                        cell2.Controls.Add(l1);
                    }


                    l1 = new Literal();
                    l1.Text = " [";
                    cell2.Controls.Add(l1);
                    cell2.Controls.Add(btnFirst);
                    l1 = new Literal();
                    l1.Text = "] ";
                    cell2.Controls.Add(l1);

                    l1 = new Literal();
                    l1.Text = " [";
                    cell2.Controls.Add(l1);
                    cell2.Controls.Add(btnPrev);
                    l1 = new Literal();
                    l1.Text = "] ";
                    cell2.Controls.Add(l1);

                    l1 = new Literal();
                    l1.Text = " [";
                    cell2.Controls.Add(l1);
                    cell2.Controls.Add(btnNext);
                    l1 = new Literal();
                    l1.Text = "] ";
                    cell2.Controls.Add(l1);

                    l1 = new Literal();
                    l1.Text = " [";
                    cell2.Controls.Add(l1);
                    cell2.Controls.Add(btnLast);
                    l1 = new Literal();
                    l1.Text = "] ";
                    cell2.Controls.Add(l1);
                    r.Cells.Add(cell2);
                    row.CssClass = "GridViewToolBar";
                    this.Controls[0].Controls.AddAt(0, row);
                }
                catch
                {
                }
            }
            return res;
        }
        #endregion


        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            this.PageIndex = e.NewPageIndex;
            this.OnBind();
        }

        protected override void ExtractRowValues(System.Collections.Specialized.IOrderedDictionary fieldValues, GridViewRow row, bool includeReadOnlyFields, bool includePrimaryKey)
        {
            TableCell expCell = row.Cells[0];
            row.Cells.Remove(expCell);
            base.ExtractRowValues(fieldValues, row, includeReadOnlyFields, includePrimaryKey);
        }

        #region OnLoad
        protected override void OnLoad(EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <script type=\"text/javascript\">");
            sb.Append("function CheckAll(oCheckbox)");
            sb.Append("{");
            sb.Append("var GridView2 = document.getElementById(\"" + this.ClientID + "\");");
            sb.Append(" for(i = 1;i < GridView2.rows.length; i++)");
            sb.Append("{");
            sb.Append("var inputArray = GridView2.rows[i].getElementsByTagName(\"INPUT\");");
            sb.Append("for(var j=0;j<inputArray.length;j++)");
            sb.Append("{ if(inputArray[j].type=='checkbox')");
            sb.Append("{if(inputArray[j].id.indexOf('ItemCheckBox',0)>-1){inputArray[j].checked =oCheckbox.checked; }}  }");
            sb.Append("}");
            sb.Append(" }");
            sb.Append("</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CheckFun", sb.ToString(), false);
            if (!Page.IsPostBack)
            {
                try
                {
                    if (UseRowStyle && HttpContext.Current.Session["Style"] != null)
                    {
                        string style = HttpContext.Current.Session["Style"] + "xtable_bordercolorlight";
                        if (!string.IsNullOrWhiteSpace(style))
                        {
                            string strApplication = HttpContext.Current.Application[style].ToString();
                            if (ShowGridLine)
                            {
                                this.BorderColor = ColorTranslator.FromHtml(strApplication);
                            }
                            if (ShowHeaderStyle)
                            {
                                this.HeaderStyle.BackColor = ColorTranslator.FromHtml(strApplication);
                            }
                        }
                        string headerStyle = HttpContext.Current.Session["Style"] + "xtable_titlebgcolor";
                        if (!string.IsNullOrWhiteSpace(headerStyle))
                        {
                            string headerStyleApp = HttpContext.Current.Application[headerStyle].ToString();
                            if (ShowHeaderStyle)
                            {
                                this.HeaderStyle.BackColor = ColorTranslator.FromHtml(headerStyleApp);
                            }
                        }
                    }
                    OnBind();

                }
                catch (Exception) { }
            }
            base.OnLoad(e);

            if (ClearRowStyle)
            {
                this.Style.Clear();
                this.BorderColor = Color.Empty;
                this.BorderWidth = Unit.Empty;
                this.BorderStyle = BorderStyle.NotSet;
                this.HeaderStyle.Reset();
                this.FooterStyle.Reset();
                this.PagerStyle.Reset();
                this.RowStyle.Reset();
            }
        }
        #endregion

        //public string SortExpressionEx
        //{
        //    get
        //    {
        //        if (ViewState["SortExpressionEx"] == null)
        //        {
        //            return null;
        //        }
        //        return ViewState["SortExpressionEx"].ToString();
        //    }
        //    set
        //    {
        //        ViewState["SortExpressionEx"] = value;
        //    }
        //}


        #region PrepareControlHierarchy
        protected override void PrepareControlHierarchy()
        {
            if (ShowCheckAll)
            {
                if (this.Controls.Count != 0)
                {
                    bool controlStyleCreated = base.ControlStyleCreated;
                    Table table = (Table)this.Controls[0];
                    table.CopyBaseAttributes(this);
                    if (UseRowStyle)
                    {
                        if (controlStyleCreated && !base.ControlStyle.IsEmpty)
                        {
                            table.ApplyStyle(base.ControlStyle);
                        }
                        else
                        {
                            table.GridLines = GridLines.Both;
                            table.CellSpacing = 0;
                        }
                        table.Caption = this.Caption;
                        table.CaptionAlign = this.CaptionAlign;
                    }

                    TableRowCollection rows = table.Rows;
                    Style s = null;
                    if (UseRowStyle)
                    {
                        if (this.AlternatingRowStyle != null)
                        {
                            s = new TableItemStyle();
                            s.CopyFrom(this.RowStyle);
                            s.CopyFrom(this.AlternatingRowStyle);
                        }
                        else
                        {
                            s = this.RowStyle;
                        }
                    }

                    int num = 0;
                    bool flag2 = true;
                    foreach (GridViewRow row in rows)
                    {
                        Style style2 = new TableItemStyle();
                        switch (row.RowType)
                        {
                            case DataControlRowType.Header:
                                if (UseRowStyle && this.ShowHeader && (this.HeaderStyle != null))
                                {
                                    row.MergeStyle(this.HeaderStyle);
                                }
                                goto Label_0256;

                            case DataControlRowType.Footer:
                                if (UseRowStyle && this.ShowFooter && (this.FooterStyle != null))
                                {
                                    row.MergeStyle(this.FooterStyle);
                                }
                                goto Label_0256;

                            case DataControlRowType.DataRow:
                                if (!UseRowStyle) break;
                                if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Normal)
                                {
                                    if ((row.RowState & DataControlRowState.Selected) != DataControlRowState.Normal)
                                    {
                                        Style style3 = new TableItemStyle();
                                        if ((row.RowIndex % 2) != 0)
                                        {
                                            style3.CopyFrom(s);
                                        }
                                        else
                                        {
                                            style3.CopyFrom(this.RowStyle);
                                        }
                                        style3.CopyFrom(this.SelectedRowStyle);
                                        row.MergeStyle(style3);
                                    }
                                    else if ((row.RowState & DataControlRowState.Alternate) != DataControlRowState.Normal)
                                    {
                                        row.MergeStyle(s);
                                    }
                                    else
                                    {
                                        row.MergeStyle(this.RowStyle);
                                    }
                                }
                                if ((row.RowIndex % 2) == 0)
                                {
                                    break;
                                }
                                style2.CopyFrom(s);

                                if (row.RowIndex == this.SelectedIndex)
                                {
                                    style2.CopyFrom(this.SelectedRowStyle);
                                }
                                style2.CopyFrom(this.EditRowStyle);
                                row.MergeStyle(style2);
                                break;
                            case DataControlRowType.Pager:
                                if (UseRowStyle && row.Visible && (this.PagerStyle != null))
                                {
                                    row.MergeStyle(this.PagerStyle);
                                }
                                goto Label_0256;

                            case DataControlRowType.EmptyDataRow:
                                if (UseRowStyle) row.MergeStyle(this.EmptyDataRowStyle);
                                goto Label_0256;

                            default:
                                goto Label_0256;
                        }
                        if (UseRowStyle) style2.CopyFrom(this.RowStyle);
                        goto Label_0256;

                        Label_0256:
                        if ((row.RowType != DataControlRowType.Pager) && (row.RowType != DataControlRowType.EmptyDataRow))
                        {
                            foreach (TableCell cell in row.Cells)
                            {
                                DataControlFieldCell cell2 = cell as DataControlFieldCell;
                                if (cell2 != null)
                                {
                                    DataControlField containingField = cell2.ContainingField;
                                    if (containingField != null)
                                    {
                                        if (!containingField.Visible)
                                        {
                                            cell.Visible = false;
                                            continue;
                                        }
                                        if ((row.RowType == DataControlRowType.DataRow) && flag2)
                                        {
                                            num++;
                                        }
                                        if (UseRowStyle)
                                        {
                                            Style headerStyleInternal = null;
                                            switch (row.RowType)
                                            {
                                                case DataControlRowType.Header:
                                                    headerStyleInternal = containingField.HeaderStyle;
                                                    break;

                                                case DataControlRowType.Footer:
                                                    headerStyleInternal = containingField.FooterStyle;
                                                    break;

                                                default:
                                                    headerStyleInternal = containingField.ItemStyle;
                                                    break;
                                            }
                                            if (headerStyleInternal != null)
                                            {
                                                cell.MergeStyle(headerStyleInternal);
                                            }
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                foreach (Control control in cell.Controls)
                                                {
                                                    WebControl control2 = control as WebControl;
                                                    Style controlStyleInternal = containingField.ControlStyle;
                                                    if (((control2 != null) && (controlStyleInternal != null)) && !controlStyleInternal.IsEmpty)
                                                    {
                                                        control2.ControlStyle.CopyFrom(controlStyleInternal);
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                flag2 = false;
                            }
                        }
                    }
                    if ((this.Rows.Count > 0) && (num != this.Rows[0].Cells.Count))
                    {
                        if (ShowCheckAll)
                        {
                            num++;
                        }
                        if ((this.TopPagerRow != null) && (this.TopPagerRow.Cells.Count > 0))
                        {
                            this.TopPagerRow.Cells[0].ColumnSpan = num;
                        }
                        if ((this.BottomPagerRow != null) && (this.BottomPagerRow.Cells.Count > 0))
                        {
                            this.BottomPagerRow.Cells[0].ColumnSpan = num;
                        }
                    }
                }
            }
            else
            {
                base.PrepareControlHierarchy();
            }
            if (ClearRowStyle) this.Style.Clear();
        }
        #endregion

        #region OnSorting
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            SortExpressionStr = e.SortExpression;
            if (SortDirectionStr.ToLower() == "asc")
            {
                SortDirectionStr = "DESC";
            }
            else
            {
                SortDirectionStr = "ASC";
            }
            OnBind();
        }
        #endregion


        string _excelName = "FileName1";
        [Category("扩展"), DefaultValue("FileName1")]
        public string ExcelFileName
        {
            get
            {
                return _excelName;
            }
            set
            {
                _excelName = value;
            }
        }
        string _UnExportedColumnNames = "";
        [Description("不导出的数据列集合,将HeaderText用,隔开"), Category("扩展"), PersistenceMode(PersistenceMode.InnerProperty)]
        public string UnExportedColumnNames
        {
            get
            {
                return _UnExportedColumnNames.Trim();
            }
            set
            {
                _UnExportedColumnNames = value;
            }
        }

        private void DisableControls(Control gv)
        {

            LinkButton lb = new LinkButton();

            Literal l = new Literal();

            string name = String.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {

                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {

                    l.Text = (gv.Controls[i] as LinkButton).Text;

                    gv.Controls.Remove(gv.Controls[i]);

                    gv.Controls.AddAt(i, l);

                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;

                    gv.Controls.Remove(gv.Controls[i]);

                    gv.Controls.AddAt(i, l);

                }

                if (gv.Controls[i].HasControls())
                {
                    DisableControls(gv.Controls[i]);
                }

            }
        }

        #region OnRowDataBound
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (UseRowStyle)
            {
                e.Row.Attributes.Add("style", "background:#FFF");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex % 2 == 0)
                    {
                        e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                    }
                    else
                    {
                        e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                    }
                }
            }
            if (ClearRowStyle)
            {
                e.Row.Style.Clear();
                e.Row.Attributes.Remove("height");
                e.Row.Attributes.Remove("style");
            }
        }
        #endregion

        #region OnRowCommand
        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            base.OnRowCommand(e);
            if (e.CommandName == "ExportToExcel")
            {
                string[] ss = UnExportedColumnNames.Split(',');
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

                foreach (string s in ss)
                {
                    if (s != ",")
                    {
                        list.Add(s);
                    }
                }
                ShowToolBar = false;
                this.AllowSorting = false;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("<meta   http-equiv=Content-Type   content=text/html;charset=utf-8>");
                string fileName = HttpUtility.UrlEncode(ExcelFileName + ".xls", Encoding.UTF8);
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                HttpContext.Current.Response.Charset = Encoding.UTF8.WebName;
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;//设置输出流为UTF-8

                HttpContext.Current.Response.ContentType = "application/vnd.xls";

                System.IO.StringWriter stringWrite = new System.IO.StringWriter();

                System.Web.UI.HtmlTextWriter htmlWrite =
                new HtmlTextWriter(stringWrite);
                bool showCheckAll = ShowCheckAll;
                this.ShowCheckAll = false;
                this.AllowPaging = false;
                OnBind();
                DisableControls(this);
                foreach (DataControlField c in this.Columns)
                {
                    if (list.Contains(c.HeaderText) && !string.IsNullOrWhiteSpace(c.HeaderText))
                    {
                        c.Visible = false;
                    }
                }
                this.RenderControl(htmlWrite);
                string content = System.Text.RegularExpressions.Regex.Replace(stringWrite.ToString(), "(<a[^>]+>)|(</a>)", "");
                HttpContext.Current.Response.Write(content);
                HttpContext.Current.Response.End();

                this.AllowPaging = true;
                this.AllowSorting = true;
                ShowToolBar = true;
                this.ShowCheckAll = showCheckAll;
                OnBind();
            }
            else if (e.CommandName == "ExportToWord")
            {
                string[] ss = UnExportedColumnNames.Split(',');
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

                foreach (string s in ss)
                {
                    if (s != ",")
                    {
                        list.Add(s);
                    }
                }
                ShowToolBar = false;
                this.AllowSorting = false;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("<meta   http-equiv=Content-Type   content=text/html;charset=utf-8>");
                string fileName = HttpUtility.UrlEncode(ExcelFileName + ".doc", Encoding.UTF8);
                HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + fileName);

                HttpContext.Current.Response.Charset = Encoding.UTF8.WebName;
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;//设置输出流为UTF-8

                HttpContext.Current.Response.ContentType = "application/ms-word";


                System.IO.StringWriter stringWrite = new System.IO.StringWriter();

                System.Web.UI.HtmlTextWriter htmlWrite =
                new HtmlTextWriter(stringWrite);

                bool showCheckAll = ShowCheckAll;
                this.ShowCheckAll = false;
                this.AllowPaging = false;
                OnBind();

                DisableControls(this);
                foreach (DataControlField c in this.Columns)
                {
                    if (list.Contains(c.HeaderText) && !string.IsNullOrWhiteSpace(c.HeaderText))
                    {
                        c.Visible = false;
                    }
                }
                this.RenderControl(htmlWrite);
                string content = System.Text.RegularExpressions.Regex.Replace(stringWrite.ToString(), "(<a[^>]+>)|(</a>)", "");
                HttpContext.Current.Response.Write(content);
                HttpContext.Current.Response.End();
                this.AllowPaging = true;
                this.AllowSorting = true;
                ShowToolBar = true;
                ShowCheckAll = showCheckAll;
                OnBind();
            }
        }
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            if (UseRowStyle)
            { //string style = @"<style> .text { mso-number-format:\@; } </style> ";

                ////text-decoration: underline; 
                writer.Write(@"
<style type='text/css'>
.GridViewTyle .text { mso-number-format:\@; }
.GridViewTyle tr td { border: 1px solid #CCCCCC;}
.GridViewTyle tr td table tr td{ border: none;}
.GridViewTyle tr td { border-spacing: 2px; border-color: #CCCCCC;border-collapse: collapse;empty-cells: show; }
.GridViewTyle a{ color:#1317fc;text-decoration: none;}
.GridViewTyle a:hover{ color:#1317fc;}
.GridViewTyle span{  text-align:center;}
</style>
");
            }

            base.Render(writer);
        }
    }
}
