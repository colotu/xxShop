<%@ Page Title="<%$ Resources:Site, ptUserAdmin %>" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="suppPointsDetail.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Supplier.SupplierInfo.suppPointsDetail" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">商家管理
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">您可以新增、修改、查询商家信息
                    </td>
                </tr>
            </table>
        </div>
        <div class="newslist">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td>消费积分明细查询</td>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtUserName" runat="server" />
                        <asp:Label ID="lbsuppid" runat="server" Text="Label" Visible="false"></asp:Label>
                        <asp:Literal ID="Literal3" runat="server" Text="消费日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="150px" CssClass="mar-r0">                        
                    </asp:TextBox>
                        -
                        <asp:TextBox ID="txtCreatedDateEnd" Width="150px" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                            OnClick="btnSearch_Click" class="adminsubmit_short  mar-le"></asp:Button>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" DataKeyNames="OrderId" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
            <Columns>
                <asp:BoundField DataField="BuyerName" HeaderText="用户名" SortExpression="BuyerName" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="OrderCode" HeaderText="订单编号" SortExpression="OrderCode" ItemStyle-HorizontalAlign="center" />
                 <asp:TemplateField ControlStyle-Width="50" HeaderText="订单总额" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#Eval("OrderTotal", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="应付总额" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#Eval("Amount", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Dpxfjf" HeaderText="消费积分" SortExpression="Dpxfjf" ItemStyle-HorizontalAlign="center" />
                
                <asp:TemplateField ControlStyle-Width="120" HeaderText="下单日期" SortExpression="CreatedDate"
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
