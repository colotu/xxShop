
<%@ Page Title="仓库地区管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" CodeBehind="DepotRegionList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.DepotRegionList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$='ddlDepot']").select2({ placeholder: "请选择" });
            $(".iframe").colorbox({ iframe: true, width: "580", height: "360", overlayClose: false });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="仓库地区管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以对仓库进行地区分配管理操作" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Search -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no mar-bt">
        <tr>
            <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
            <asp:Literal ID="LiteralDepot" runat="server" Text="仓库" />：
                    <asp:DropDownList ID="ddlDepot" runat="server" Width="200px">
                    </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                    OnClick="btnSearch_Click" class="add-btn"></asp:Button>
            </td>
        </tr>
    </table>
    <!--Search end-->
        <div class="newslist mar-bt" id="liAdd" runat="server" visible="false">
            <div class="newsicon">
                <ul>
                    <li class="add-btn">
                    <a href="AddDepotRegion.aspx" class="iframe">
                        <asp:Literal ID="Literal5" runat="server" Text="新增" />
                        </a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound"   Width="100%" OnRowDeleting="gridView_RowDeleting"
            PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="DepotId,RegionId"
            Style="float: left;" ShowGridLine="true" ShowHeaderStyle="true"  >
            <Columns>
                <asp:TemplateField HeaderText="仓库名称" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                           <div class="tx-l">
                        <%#GetDepotName(Eval("DepotId"))%>
                               </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地区" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                           <div class="tx-l">
                        <%#Eval("RegionName")%>
                               </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#GetboolText(Eval("Status"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center"   Visible="false">
                    <ItemTemplate>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="linkDel" runat="server" OnClientClick="return confirm('是否确定删除？')" CommandArgument='<%#Eval("DepotId")+","+Eval("RegionId")%>'
                            CommandName="Delete" ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" Text="<%$ Resources:Site, btnDeleteText %>">
                        </asp:LinkButton>
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
        <%--<table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click"  Visible="false"/>
                </td>
            </tr>
        </table>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>


