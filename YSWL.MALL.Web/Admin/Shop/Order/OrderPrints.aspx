<%@ Page Title="订单打印管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="OrderPrints.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Order.OrderPrints" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/css/tab.css" rel="stylesheet" type="text/css" charset="utf-8" />
    <script src="/Admin/js/tab.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.js" type="text/javascript"></script>
    <link href="/Scripts/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
    <script src="/Scripts/jBox/i18n/jquery.jBox-zh-CN.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="/Scripts/jquery/maticsoft.printarea.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });
            var SelectedCss = "active";
            var NotSelectedCss = "normal";
            var type = $.getUrlParam("type");
            //            if (type) {
            //                $("#myTab1 li[id='Li" + type + "']").removeClass(NotSelectedCss).addClass(SelectedCss).siblings().removeClass(SelectedCss).addClass(NotSelectedCss);
            //            }
            if (type != null) {
                $("a:[href='OrderPrints.aspx?type=" + type + "']").parents("li").removeClass(NotSelectedCss);
                $("a:[href='OrderPrints.aspx?type=" + type + "']").parents("li").addClass(SelectedCss);
            } else {
                $("a:[href='OrderPrints.aspx']").parents("li").removeClass(NotSelectedCss);
                $("a:[href='OrderPrints.aspx']").parents("li").addClass(SelectedCss);
            }
            //            $("#myTab1 li").click(function () {
            //                $(this).removeClass(NotSelectedCss).addClass(SelectedCss).siblings().removeClass(NotSelectedCss);
            //            });

            $(".iframe").colorbox({ iframe: true, width: "840", height: "700", overlayClose: false });

            $(".ordertype").each(function () {
                var tab = $(this).attr("value");
                var value = $(this).children().val();
                if (value == "True") {
                    $("#tab" + tab).show();
                }
            });

            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);

            $("[id$='txtCreatedDateStart']").prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtCreatedDateEnd']").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$='txtCreatedDateEnd']").prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$='txtCreatedDateStart']").datepicker("option", "maxDate", selectedDate);
                    $("[id$='txtCreatedDateEnd']").val($(this).val());
                }
            });

            $("#btnPrintMore").click(function () {
                var ids = [];
                // $("#ctl00_ContentPlaceHolder1_gridView").find("tr").find(".hfOrderId");
                $("#ctl00_ContentPlaceHolder1_gridView").find("tr").each(function () {
                    var checked = $(this).find("input[type='checkbox']").attr("checked");
                    //alert(checked);
                    if (checked) {
                        var orderId = $(this).find(".hfOrderId").val();
                        if (orderId) {
                            ids.push(orderId);
                        }
                    }
                });
                if (ids.length <= 0) {
                    ShowFailTip("请选择要打印的订单！");
                    return false;
                }
                printOrder(ids.length, 0, ids);
            });

            $(".btnPrint").click(function () {
                var orderid = $(this).attr("orderId");
                if (orderid) {
                    var ids = [];
                    ids.push(orderid);
                    printOrder(1, 0, ids);
                }
            });
        });
        function printOrder(count, i, ids) {
            var orderid = ids[i];
            $("#PrintArea").printIframe('/Com/Order/PrintOrder/' + orderid, function () {
                if (i < count - 1) {
                    printOrder(count, ++i, ids);
                } else {
                    ShowSuccessTip("生成成功");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div id="PrintArea" style="display: none">
    </div>
    <div class="newslistabout">
        <div class="newslist_title">
            
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal3" runat="server" Text="订单打印管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以根据订单状态查询订单，打印订单操作" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal1" runat="server" Text="订单号" />：
                    <asp:TextBox ID="txtOrderCode" runat="server"></asp:TextBox>
                    <asp:Literal ID="LiteralShipName" runat="server" Text="收货人" />：
                    <asp:TextBox ID="txtShipName" runat="server"></asp:TextBox>
                    <asp:Literal ID="LiteralBuyerName" runat="server" Text="用户名" />：
                    <asp:TextBox ID="txtBuyerName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="pad-t10">
                    <asp:Literal ID="LiteralPaymentStatus"
                        runat="server" Text="付款状态" />：
                    <asp:DropDownList ID="dropPaymentStatus" runat="server">
                        <asp:ListItem Value="-1">全部</asp:ListItem>
                        <asp:ListItem Value="0">未支付</asp:ListItem>
                        <asp:ListItem Value="2">已支付</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Literal ID="LiteralShippingStatus" runat="server" Text="发货状态" />：
                    <asp:DropDownList ID="dropShippingStatus" runat="server">
                        <asp:ListItem Value="-1">全部</asp:ListItem>
                        <asp:ListItem Value="0">未发货</asp:ListItem>
                        <asp:ListItem Value="1">打包中</asp:ListItem>
                        <asp:ListItem Value="2">已发货</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Literal ID="LiteralSupplier" runat="server" Text="商家" />：
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="200px">
                    </asp:DropDownList>
                    <asp:Literal ID="LiteralCreatedDate" runat="server" Text="下单日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="80px" CssClass="mar-r0">
                        
                    </asp:TextBox>-<asp:TextBox ID="txtCreatedDateEnd" Width="80px" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <span class="ordertype" value="Paying">
            <asp:HiddenField ID="hfPaying" runat="server" Value="True" />
        </span><span class="ordertype" value="PreHandle">
            <asp:HiddenField ID="hfPreHandle" runat="server" Value="True" />
        </span><span class="ordertype" value="Cancel">
            <asp:HiddenField ID="hfCancel" runat="server" Value="True" />
        </span><span class="ordertype" value="Locking">
            <asp:HiddenField ID="hfLocking" runat="server" Value="True" />
        </span><span class="ordertype" value="PreConfirm">
            <asp:HiddenField ID="hfPreConfirm" runat="server" Value="True" />
        </span><span class="ordertype" value="Handling">
            <asp:HiddenField ID="hfHandling" runat="server" Value="True" />
        </span><span class="ordertype" value="Shipping">
            <asp:HiddenField ID="hfShipping" runat="server" Value="True" />
        </span><span class="ordertype" value="Shiped">
            <asp:HiddenField ID="hfShiped" runat="server" Value="True" />
        </span><span class="ordertype" value="Success">
            <asp:HiddenField ID="hfSuccess" runat="server" Value="True" />
        </span>
        <div class="nTab4">
            <div class="TabTitle">
                <ul id="myTab1">
                    <asp:HiddenField runat="server" ID="orderType" />
                    <li class="normal"><a href="OrderPrints.aspx?type=0" style="padding-top: 5px;">
                        <asp:Literal ID="Literal7" runat="server" Text="近三个月订单"></asp:Literal></a></li>
                    <li class="normal" id="tabPaying" style="display: none"><a href="OrderPrints.aspx?type=1">
                        <asp:Literal ID="Literal8" runat="server" Text="等待付款"></asp:Literal></a></li>
                    <li class="normal" id="tabPreHandle" style="display: none"><a href="OrderPrints.aspx?type=2">
                        <asp:Literal ID="Literal9" runat="server" Text="等待处理"></asp:Literal></a></li>
                    <li class="normal" id="tabCancel" style="display: none"><a href="OrderPrints.aspx?type=3">
                        <asp:Literal ID="Literal12" runat="server" Text="取消订单"></asp:Literal></a></li>
                    <li runat="server" visible="false" class="normal" id="tabLocking" style="display: none">
                        <a href="OrderPrints.aspx?type=4">
                            <asp:Literal ID="Literal13" runat="server" Text="订单锁定"></asp:Literal></a></li>
                    <li class="normal" id="tabPreConfirm" style="display: none"><a href="OrderPrints.aspx?type=5">
                        <asp:Literal ID="Literal14" runat="server" Text="等待付款确认"></asp:Literal></a></li>
                    <li class="normal" id="tabHandling" style="display: none"><a href="OrderPrints.aspx?type=6">
                        <asp:Literal ID="Literal15" runat="server" Text="正在处理"></asp:Literal></a></li>
                    <li class="normal" id="tabShipping" style="display: none"><a href="OrderPrints.aspx?type=7">
                        <asp:Literal ID="Literal16" runat="server" Text="配货中"></asp:Literal></a></li>
                    <li class="normal" id="tabShiped" style="display: none"><a href="OrderPrints.aspx?type=8">
                        <asp:Literal ID="Literal17" runat="server" Text="已发货"></asp:Literal></a></li>
                    <li class="normal" id="tabSuccess" style="display: none"><a href="OrderPrints.aspx?type=9">
                        <asp:Literal ID="Literal5" runat="server" Text="已完成"></asp:Literal></a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            OnRowCommand="gridView_RowCommand" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="True" DataKeyNames="OrderId" Style="float: left;">
            
            <Columns>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <input type="hidden" class="hfOrderId"  value="<%#Eval("OrderId") %>"/>
                        <a class="iframe" href="OrderShow.aspx?orderId=<%#Eval("OrderId") %>&type=<%#Type%>">
                            <%# Eval("OrderCode")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="下单时间" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单总额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("OrderTotal", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="应付总额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("Amount", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="用户名" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("BuyerName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="收货人" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("ShipName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单状态" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" Visible="False">
                    <ItemTemplate>
                        <%#GetOrderStatus(Eval("OrderStatus"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="发货状态" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# GetShippingStatus(Eval("ShippingStatus"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="配送方式" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("ShippingModeName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="支付方式" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("PaymentTypeName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单状态" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#GetOrderType(Eval("PaymentGateway"), Eval("OrderStatus"), Eval("PaymentStatus"), Eval("ShippingStatus"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="所属商家" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" Visible="false">
                    <ItemTemplate>
                        <%# (Eval("SupplierName")==null  || String.IsNullOrWhiteSpace(Eval("SupplierName").ToString())) ? "平台" : Eval("SupplierName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="160" HeaderText="订单操作" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <a class="iframe" style=" padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;"
                            href="OrderShow.aspx?orderId=<%#Eval("OrderId") %>&type=<%#Type%>">查看</a>
                             <a class="btnPrint" style="padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;"
                            href="javascript:;" orderId="<%#Eval("OrderId") %>">打印</a>
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
        <div class="def-wrapper">
<input id="btnPrintMore" type="button" value="批量打印"  class="adminsubmit"/>
        </div>
            
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
