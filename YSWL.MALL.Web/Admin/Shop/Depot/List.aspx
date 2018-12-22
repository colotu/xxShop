<%@ Page Title="仓库" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.List" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%--<link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>--%>
  <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".iframe").colorbox({ iframe: true, width: "840", height: "550", overlayClose: false });
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
                         <asp:Literal ID="Literal1" runat="server" Text="仓库" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以查看 <asp:Literal ID="Literal3" runat="server" Text="仓库信息" />
                    </td>
                </tr>
            </table>
        </div>
    <!--Title end -->
    <!--Add  -->
    <!--Add end -->
    <!--Search -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no mar-bt">
        <tr>
            <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                    OnClick="btnSearch_Click" class="add-btn"></asp:Button>
            </td>
        </tr>
    </table>
    <!--Search end-->
 <div class="newslist mar-bt" id="liAdd" runat="server" visible="false">
            <div class="newsicon">
                <ul>
                    <li class="add-btn" >
                    <a href="AddDepot.aspx" class="iframe"   >
                        <asp:Literal ID="ld" runat="server" Text="新增"  />
                        </a></li>
                </ul>
            </div>
        </div>
    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
        ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
        OnRowDataBound="gridView_RowDataBound"  OnRowCommand="gridView_RowCommand"     UnExportedColumnNames="Modify"
        Width="100%" PageSize="10" ShowExportExcel="True" ShowExportWord="False" ExcelFileName="FileName1"
        CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="DepotId">
        <columns>
               <asp:TemplateField HeaderText="仓库名称"   SortExpression="Name"  >
                    <ItemTemplate>
                        <div class="tx-l">
                            <%#Eval("Name")%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
		<asp:BoundField DataField="Code" HeaderText="仓库编码" SortExpression="Code" ItemStyle-HorizontalAlign="left"  /> 
        <asp:TemplateField ControlStyle-Width="50" HeaderText="地区" ItemStyle-HorizontalAlign="left"
                    ItemStyle-Width="150">
                    <ItemTemplate>
                        <%#GetRegionName(Eval("RegionId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
		<asp:BoundField DataField="ContactName" HeaderText="联系人" SortExpression="ContactName" ItemStyle-HorizontalAlign="left"  /> 
		<asp:BoundField DataField="Phone" HeaderText="联系方式" SortExpression="Phone" ItemStyle-HorizontalAlign="left" Visible="false"  /> 
		<asp:BoundField DataField="Email" HeaderText="邮箱" SortExpression="Email" ItemStyle-HorizontalAlign="left" Visible="false"  />
         <asp:TemplateField ControlStyle-Width="50" HeaderText="状态" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#GetStatus(Eval("Status"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="创建时间" ItemStyle-HorizontalAlign="Center" 
                    ItemStyle-Width="120" SortExpression="CreatedDate" >
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
 <asp:TemplateField ControlStyle-Width="50" HeaderText="操作" ItemStyle-HorizontalAlign="Center" 
                    ItemStyle-Width="140" >
                    <ItemTemplate>
                    <asp:HyperLink  runat="server" style="white-space: nowrap;"   NavigateUrl='<%# Eval("DepotId","UpdateDepot.aspx?id={0}") %>'     ID="modifyBut" Visible="false" class="iframe" >编辑</asp:HyperLink>
                                <a   style="white-space: nowrap;" href="Show.aspx?id=<%#Eval("DepotId") %>"  class="iframe">详情</a>
                               <asp:LinkButton ID="linkDel1" Visible="false" runat="server" OnClientClick="return confirm('您确定删除么？')"   CommandArgument='<%#Eval("DepotId")%>'
                            CommandName="Del" ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" Text="<%$ Resources:Site, btnDeleteText %>">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                           
                          
                        </columns>
        <footerstyle height="25px" horizontalalign="Right" />
        <headerstyle height="25px" />
        <pagerstyle height="25px" horizontalalign="Right" />
        <sorttip ascimg="~/Images/up.JPG" descimg="~/Images/down.JPG" />
        <rowstyle height="25px" />
        <sortdirectionstr>DESC</sortdirectionstr>
    </cc1:GridViewEx>
     </div>
</asp:Content>
 