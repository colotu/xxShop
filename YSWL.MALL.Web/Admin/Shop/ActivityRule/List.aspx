<%@ Page Title="促销规则" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Shop.Products.ActivityRule.List" %>
<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<%@ Register TagPrefix="YSWL" TagName="AjaxRegion" Src="~/Controls/AjaxRegion.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //$(".iframe").colorbox({ iframe: true, width: "680", height: "488", overlayClose: false });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="促销规则管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        促销规则管理，该数据为系统初始化数据，仅开发人员可以新增和编辑。
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal3" runat="server" Text="状态" />：
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text=" 全   部" Value=""></asp:ListItem>
                        <asp:ListItem Text=" 启   用" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" 未启用" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                  <%--   &nbsp;&nbsp;开始时间：
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;结束时间：
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>--%>
                    &nbsp;&nbsp;<asp:Literal ID="Literal2" runat="server" Text="规则名称" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" runat="server" class="add-btn">
                        <a href="add.aspx" >
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Site, lblAdd%>" /></a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="def-wrapper">
            <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
                Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
                CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="RuleId">
                <Columns>
                    <asp:BoundField DataField="RuleName" HeaderText="规则名称" SortExpression="RuleName" ItemStyle-HorizontalAlign="Center"  /> 
		    <asp:BoundField DataField="Priority" HeaderText="优先级" SortExpression="Priority" ItemStyle-HorizontalAlign="Center"  /> 
		    <asp:BoundField DataField="Status" HeaderText="活动状态" SortExpression="Status" ItemStyle-HorizontalAlign="Center"  /> 
		    <asp:BoundField DataField="CreatedUserId" HeaderText="创建人" SortExpression="CreatedUserId" ItemStyle-HorizontalAlign="Center"  /> 
		    <asp:BoundField DataField="CreatedDate" HeaderText="创建时间" SortExpression="CreatedDate" ItemStyle-HorizontalAlign="Center"  /> 
 
                                <asp:HyperLinkField HeaderText="编辑"  ControlStyle-CssClass="iframe" ControlStyle-Width="50"  ItemStyle-HorizontalAlign="Center"   DataNavigateUrlFields="RuleId" DataNavigateUrlFormatString="Modify.aspx?id={0}"
                                    Text="编辑"  />
                                <asp:TemplateField ControlStyle-Width="50" HeaderText="删除"   Visible="false"  >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                             Text="删除"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                </Columns>
                <FooterStyle Height="25px" HorizontalAlign="Right" />
                <HeaderStyle Height="35px" />
                <PagerStyle Height="25px" HorizontalAlign="Right" />
                <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
                <RowStyle Height="25px" />
                <SortDirectionStr>DESC</SortDirectionStr>
            </cc1:GridViewEx>
        </div>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" Visible="false"  runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit"  />
                      
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--Title -->

            <!--Title end -->

            <!--Add  -->

            <!--Add end -->

            <!--Search -->
            <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>
                    <td style="width: 80px" align="right" class="tdbg">
                         <b>关键字：</b>
                    </td>
                    <td class="tdbg">                       
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="查询"  OnClick="btnSearch_Click" >
                    </asp:Button>                    
                        
                    </td>
                    <td class="tdbg">
                    </td>
                </tr>
            </table>
            <!--Search end-->
            <br />
            <asp:GridView ID="gridView" runat="server" AllowPaging="True" Width="100%" CellPadding="3"  OnPageIndexChanging ="gridView_PageIndexChanging"
                    BorderWidth="1px" DataKeyNames="RuleId" OnRowDataBound="gridView_RowDataBound"
                    AutoGenerateColumns="false" PageSize="10"  RowStyle-HorizontalAlign="Center" OnRowCreated="gridView_OnRowCreated">
                    <Columns>
                    <asp:TemplateField ControlStyle-Width="30" HeaderText="选择"    >
                                <ItemTemplate>
                                    <asp:CheckBox ID="DeleteThis" onclick="javascript:CCA(this);" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            
		<asp:BoundField DataField="RuleName" HeaderText="规则名称" SortExpression="RuleName" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Priority" HeaderText="优先级" SortExpression="Priority" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Status" HeaderText="活动状态 0：不启用" SortExpression="Status" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CreatedUserId" HeaderText="创建人" SortExpression="CreatedUserId" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CreatedDate" HeaderText="创建时间" SortExpression="CreatedDate" ItemStyle-HorizontalAlign="Center"  /> 
                            
                            <asp:HyperLinkField HeaderText="详细" ControlStyle-Width="50" DataNavigateUrlFields="RuleId" DataNavigateUrlFormatString="Show.aspx?id={0}"
                                Text="详细"  />
                            <asp:HyperLinkField HeaderText="编辑" ControlStyle-Width="50" DataNavigateUrlFields="RuleId" DataNavigateUrlFormatString="Modify.aspx?id={0}"
                                Text="编辑"  />
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="删除"   Visible="false"  >
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                         Text="删除"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                </asp:GridView>
               <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                <tr>
                    <td style="width: 1px;">                        
                    </td>
                    <td align="left">
                        <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"/>                       
                    </td>
                </tr>
            </table>
</asp:Content>--%>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>
