<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreDetail.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserRank.ScoreDetail" %>

<%@ Page Title="会员成长值明细" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="ScoreDetail.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserRank.ScoreDetail" %>

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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:DropDownList ID="DropPointsType" runat="server" class="dropSelect">
                        <asp:ListItem Value="-1">全部</asp:ListItem>
                        <asp:ListItem Value="0">成长值获取</asp:ListItem>
                        <asp:ListItem Value="1">成长值扣除</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div class="newslist">
        </div>
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" DataKeyNames="DetailID" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
            <Columns>
              <asp:TemplateField ControlStyle-Width="120" HeaderText="成长值日期" SortExpression="CreatedDate"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("CreatedDate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField ControlStyle-Width="120" HeaderText="成长值类型" SortExpression="Type"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#GetTypeName(Convert.ToInt32(Eval("Type")))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Score" HeaderText="成长值" SortExpression="Score" ItemStyle-HorizontalAlign="center" />
                <asp:TemplateField ControlStyle-Width="120" HeaderText="说明" SortExpression="Type"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#GetRuleName(Eval("RuleId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="Description" HeaderText="成长值详情" SortExpression="Description"
                    ItemStyle-HorizontalAlign="center" />
             
              
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

