<%@ Page Title="收藏管理" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="FavoriteList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.FavoriteList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <link href="/Admin/js/select2-3.4.6/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.6/select2.min.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript">
    $(function () {
        $(".select2-container").css("vertical-align", "middle");
        $("[id$='txtUserId']").select2({
            placeholder: "输入用户手机号",
            minimumInputLength: 1,
            formatInputTooShort: "请输入至少一个字符",
            formatNoMatches: "没有匹配项",
            formatSearching: "正在查询......",
            ajax: {
                url: "/User.aspx",
                type: "POST",
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) { // page is the one-based page number tracked by Select2
                    return {
                        Action: "GetUserList",
                        q: term, //search term
                        page_limit: 10, // page size
                        page: page // page number
                    };
                },
                results: function (data, page) {
                    var more = (page * 10) < data.total; // whether or not there are more results available
                    return { results: data.List, more: more };
                }
            },
            formatResult: Format, // omitted for brevity, see the source of this page
            escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results

        });
    });
    function Format(data) {
        return data.text;
    }
</script>
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtTitle" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td height="35" class="newstitlebody">
                    
                        <asp:Literal ID="LiteralSupplier" runat="server" Text="选择用户" />：
                      <asp:TextBox ID="txtUserId" runat="server"  style="width: 320px" ></asp:TextBox>

            <%--        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:TextBox ID="txtKeyword" runat="server" class="admininput_1"></asp:TextBox>--%>


                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short add-btn mar-le"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div class="newslist">
        </div>
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound"  Width="100%"
            PageSize="10" DataKeyNames="FavoriteId" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="True">
            <Columns>
                <asp:TemplateField   HeaderText="商品名称" SortExpression="TargetId"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <div class="tx-l"> <%# GetTargetName(YSWL.Common.Globals.SafeLong(Eval("TargetId"),0), YSWL.Common.Globals.SafeInt(Eval("Type"),0))%> </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="120" HeaderText="用户" SortExpression="UserId"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%# GetUserName(Convert.ToInt32(Eval("UserId")))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="120" HeaderText="收藏时间" SortExpression="CreatedDate"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("CreatedDate"))%>
                    </ItemTemplate>
                </asp:TemplateField>
              
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </Grv:GridViewEx>
           <asp:Button ID="btnRemove" runat="server" Text="批量移除" OnClientClick="return  confirm('您确认要移除收藏吗？');"
            OnClick="btnRemove_Click" class="adminsubmit mar-t15"></asp:Button>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
