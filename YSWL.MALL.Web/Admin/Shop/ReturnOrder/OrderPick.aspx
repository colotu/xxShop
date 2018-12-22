
<%@ Page Language="C#" Title="订单取货" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="OrderPick.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ReturnOrder.OrderPick" %>

<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/tab.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .td_class
        {
            width: 80px;
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
            padding-bottom: 4px;
            padding-top: 4px;
        }
        .td_content
        {
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfSuccess" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="lblTitle" runat="server" Text="正在进行订单发货操作" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="nTab4">
            <div class="TabTitle">
                <ul id="myTab1">
                    <li class="active" onclick="nTabs(this,0);"><a href="javascript:;">商品清单</a></li>
                    <li class="normal" onclick="nTabs(this,1);"><a href="javascript:;">其它信息</a></li>
                </ul>
            </div>
        </div>
        <div class="TabContent">
            <%--基本信息--%>
            <div id="myTab1_Content0">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal2" runat="server" Text=" 订单号" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblOrderCode" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal23" runat="server" Text=" 创建日期" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr  style="display:none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal21" runat="server" Text=" 退货方式" />：
                                    </td>
                                    <td height="25" class="td_content">
                                          <asp:Literal ID="lblReturnType" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal1" runat="server" Text=" 应退金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Literal ID="lblAmountAdjusted" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                                ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                                OnRowDataBound="gridView_RowDataBound" Width="100%" PageSize="10" ShowExportExcel="False"
                                ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
                                ShowCheckAll="False" DataKeyNames="ItemId" Style="float: left;">
                                <Columns>
                                    <asp:TemplateField ControlStyle-Width="60" HeaderText="商品图片" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                                <asp:Image ID="Image1" runat="server" Width="60px" Height="60px" ImageAlign="Middle"
                                                    ImageUrl='<%# YSWL.MALL.Web.Components.FileHelper.GeThumbImage(Eval("ThumbnailsUrl").ToString(), "T128X130_")%>' />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="商品名称" ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                            <%#Eval("Name")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="商品编号" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%#Eval("SKU")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="商品属性" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="150">
                                        <ItemTemplate>
                                          <%#GetSKUStr(Eval("SKU"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="退货数量" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%# Eval("Quantity")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="出售单价" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <span class="txtprice">
                                                <%# YSWL.Common.Globals.SafeDecimal(Eval("ReturnPrice").ToString(), 0).ToString("F")%></span>
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
                        </td>
                    </tr>
                </table>
            </div>
            <%--     收货人信息--%>
            <div id="myTab1_Content1" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang"
                                style="padding-top: 8px">
                                <tr>
                                    <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">退货信息</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal18" runat="server" Text=" 联系人" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPickName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal22" runat="server" Text="手机号码" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPickCellPhone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr  >
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal24" runat="server" Text="退货地区" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPickRegion" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal3" runat="server" Text="详细地址" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPickAddress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="newslistabout">
        <table style="width: 100%; border-top: none; float: left;" cellpadding="2" cellspacing="1"
            class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="height: 6px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td height="25" style="text-align: center">
                                <asp:Button ID="btnSave" runat="server" style="display: none;" OnClientClick="$(this).hide();return true;"  OnClick="btnSave_OnClick" Text="确认取货" class="adminsubmit" ></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
    <script type="text/javascript">
        (function () {
            setTimeout("$('[id$=btnSave]').show()", 3000);
        })()
    </script>
</asp:Content>

