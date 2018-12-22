<%@ Page Title="<%$ Resources:Site, ptUserAdmin %>" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="BlanceDetail.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.Points.BlanceDetail" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtUserName" runat="server" />
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
            PageSize="10" DataKeyNames="JournalNumber" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
            <Columns>
              <asp:TemplateField ControlStyle-Width="120" HeaderText="操作日期" SortExpression="TradeDate"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("TradeDate"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Income" HeaderText="收入" SortExpression="Income" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Expenses" HeaderText="支出" SortExpression="Expenses" ItemStyle-HorizontalAlign="center" />
                 <asp:BoundField DataField="Balance" HeaderText="当前金额" SortExpression="Expenses" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Remark" HeaderText="说明" SortExpression="Description"
                    ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="CurrentPoints" HeaderText="当前资金数" SortExpression="CurrentPoints"
                    ItemStyle-HorizontalAlign="center"  Visible="false"/>
              
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
