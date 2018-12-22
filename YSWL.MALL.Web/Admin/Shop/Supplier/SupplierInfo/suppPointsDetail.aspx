<%@ Page Title="<%$ Resources:Site, ptUserAdmin %>" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="suppPointsDetail.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Supplier.SupplierInfo.suppPointsDetail" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtUserName" runat="server" Visible="false" />
                    </td>
                    <td bgcolor="#FFFFFF">
                        <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="newslist">
        </div>
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" DataKeyNames="DetailID" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="用户名" SortExpression="Score" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Score" HeaderText="积分" SortExpression="Score" ItemStyle-HorizontalAlign="center" />

                <asp:BoundField DataField="Description" HeaderText="积分详情" SortExpression="Description"
                    ItemStyle-HorizontalAlign="center" />
                <asp:TemplateField ControlStyle-Width="120" HeaderText="积分日期" SortExpression="CreatedDate"
                    ItemStyle-HorizontalAlign="Center">
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
