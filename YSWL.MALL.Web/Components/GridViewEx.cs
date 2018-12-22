using System.Web.UI;
using YSWL.Web.Controls;

namespace YSWL.MALL.Web.Controls
{
    #region 多语言文字
    public class GridViewUIText : IGridViewUIText
    {
        public string ExportExcel
        {
            get
            {
                return Resources.Site.GVTextExportExcel;
            }
        }
        public string ExportWord
        {
            get
            {
                return Resources.Site.GVTextExportWord;
            }
        }
        public string First
        {
            get
            {
                return Resources.Site.GVTextFirst;
            }
        }
        public string Previous
        {
            get
            {
                return Resources.Site.GVTextPrevious;
            }
        }
        public string Next
        {
            get
            {
                return Resources.Site.GVTextNext;
            }
        }
        public string Last
        {
            get
            {
                return Resources.Site.GVTextLast;
            }
        }
        public string Page
        {
            get
            {
                return Resources.Site.GVTextPage;
            }
        }
        public string Record
        {
            get
            {
                return Resources.Site.GVTextRecord;
            }
        }

    }
    #endregion

    [ToolboxData(@"<{0}:GridViewEx runat='server'></{0}:GridViewEx>")]
    public class GridViewEx : GridViewExBase
    {
        //去除原样式
        public GridViewEx() : base(new GridViewUIText(),false,true) { }
    }
}
