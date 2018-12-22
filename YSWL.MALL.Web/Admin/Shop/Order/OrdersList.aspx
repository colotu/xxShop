<%@ Page Title="订单管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="OrdersList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Order.OrdersList" %>

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
    <script type="text/javascript">
        $(document).ready(function () {
            //是否对接OMS
            if ($('[id$=hidIsConnectionOMS]').val().toLocaleLowerCase() == 'true') {
                //移除配货和发货
                $('.ShipedAction').remove(); 
                $('.ShipAction').remove();
            }

            //显示、移除 除查看外的其他操作  
            $(".operate").each(function () {
                //主订单  有子订单  并且已支付  或者 是 货到付款/银行转账  ( 就是可以看到子单了)  就不能再对主单操作，只能查看
                var ordertype = parseInt($(this).attr("ordertype"));
                var hasChildren = $(this).attr("hasChildren").toLocaleLowerCase();
                var paymentStatus = parseInt($(this).attr("paymentstatus"));
                var paymentGateway = $(this).attr("paymentgateway").toLocaleLowerCase();
                if (ordertype == 1 && hasChildren == "true" && (paymentStatus > 1 || paymentGateway == "cod" || paymentGateway == "bank")) {
                    $(this).remove();
                } else {
                    $(this).show();
                }
            });


            $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });

            var SelectedCss = "active";
            var NotSelectedCss = "normal";
            var type = $.getUrlParam("type");
            //            if (type) {
            //                $("#myTab1 li[id='Li" + type + "']").removeClass(NotSelectedCss).addClass(SelectedCss).siblings().removeClass(SelectedCss).addClass(NotSelectedCss);
            //            }
            if (type != null) {
                $("a:[href='OrdersList.aspx?type=" + type + "']").parents("li").removeClass(NotSelectedCss);
                $("a:[href='OrdersList.aspx?type=" + type + "']").parents("li").addClass(SelectedCss);
            } else {
                $("a:[href='OrdersList.aspx']").parents("li").removeClass(NotSelectedCss);
                $("a:[href='OrdersList.aspx']").parents("li").addClass(SelectedCss);
            }
            //            $("#myTab1 li").click(function () {
            //                $(this).removeClass(NotSelectedCss).addClass(SelectedCss).siblings().removeClass(NotSelectedCss);
            //            });

            //是否对接oms 
            if ($('[id$="hidIsConnectionOMS"]').val().toLocaleLowerCase() == 'true') {
                //对接oms
                //取消 (在线支付未支付时可取消)   
                $(".CancelAction").each(function () {
                    var OrderStatus = $(this).attr("OrderStatus");
                    var payGateway = $(this).attr("payGateway");
                    var payStatus = $(this).attr("payStatus");
                    if (OrderStatus == 0 &&  payStatus==0  &&    payGateway != 'cod') {
                        $(this).show();
                    }
                });
            } else {
                //未对接oms
                //取消 //未发货时 可取消
                $(".CancelAction").each(function () {
                    var OrderStatus = $(this).attr("OrderStatus");
                    var shippingStatus = $(this).attr("shippingStatus"); 
                    if ((OrderStatus == 0 || OrderStatus == 1) && shippingStatus<2) {
                        $(this).show();
                    }
                });
            }

            //支付
            $(".PayAction").each(function () {
                var OrderStatus = $(this).attr("OrderStatus");
                var payGateway = $(this).attr("payGateway");
                var payStatus = $(this).attr("payStatus");
                //payGateway==cod 是货到付款，货到付款不显示支付按钮
                //在线支付  未支付时显示支付按钮
                if (OrderStatus == 0 && payStatus == 0 && payGateway != 'cod') {
                    $(this).show();
                }
            });
 
            //配货
            $(".ShipAction").each(function () {
                var ShippingStatus = $(this).attr("ShippingStatus");
                var OrderStatus = $(this).attr("OrderStatus");
                var payGateway = $(this).attr("payGateway");
                var payStatus = $(this).attr("payStatus");
                //1:货到付款 未处理时  2:在线支付已支付时  可配货
                if (ShippingStatus < 1 && OrderStatus == 0 && (payGateway == 'cod' || (payGateway != 'cod' && payStatus==2))) {
                    $(this).show();
                }
            });

            //发货
            $(".ShipedAction").each(function () {
                var ShippingStatus = $(this).attr("ShippingStatus");
                var OrderStatus = $(this).attr("OrderStatus");
                if (ShippingStatus<=1 &&  OrderStatus == 1) {
                    $(this).show();
                }
            });


            $(".iframe").colorbox({ iframe: true, width: "840", height: "700", overlayClose: false });
            $(".iframeShiped").colorbox({ iframe: true, width: "900", height: "800", overlayClose: false });

            $(".ordertype").each(function () {
                var tab = $(this).attr("value");
                var value = $(this).children().val();
                if (value == "True") {
                    $("#tab" + tab).show();
                }
            });

            //完成
            $(".CompleteAction").each(function () {
                var OrderStatus = $(this).attr("OrderStatus");
                var shippingStatus = $(this).attr("shippingStatus");
                if (OrderStatus != 2 && OrderStatus != -1 && shippingStatus == 2) {
                    $(this).show();
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


        });
        function GetDeleteM() {
            $("[id$='btnDelete']").click();
        }

        function ExportConfirm() {
            if (confirm('您确认要导出么？')) {
                $.jBox.tip("正在导出订单数据，请稍候...", 'loading', { timeout: 3000 });
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <input type="hidden"  id="hidIsMultiDepot"  runat="server" value="" />
    <input type="hidden"  id="hidIsConnectionOMS"  runat="server" value="" />
    
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal3" runat="server" Text="订单管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以根据订单状态查询订单，配送货等操作" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <%--<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：--%>
                    <asp:Literal ID="Literal1" runat="server" Text="订单号" />：
                    <asp:TextBox ID="txtOrderCode" runat="server"></asp:TextBox>
                    <asp:Literal ID="LiteralShipName" runat="server" Text="收货人" />：
                    <asp:TextBox ID="txtShipName" runat="server"></asp:TextBox>
                    <asp:Literal ID="LiteralBuyerName" runat="server" Text="用户名" />：
                    <asp:TextBox ID="txtBuyerName" runat="server"></asp:TextBox>
                    <asp:Literal ID="LiteralCreatedDate" runat="server" Text="下单日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="150px" CssClass="mar-r0">
                        
                    </asp:TextBox> - <asp:TextBox ID="txtCreatedDateEnd" Width="150px" runat="server"></asp:TextBox>
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
                   <span style="display: none"> &nbsp;&nbsp;<asp:Literal ID="LiteralSupplier" runat="server" Text="商家" />：
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="200px">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkshowMainOrder" runat="server" />显示主单
                       </span>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <br />
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
                <ul id="myTab1" class="mar-b0">
                    <asp:HiddenField runat="server" ID="orderType" />
                    <%--<li class="active" id="Li0">
                        <asp:LinkButton runat="server" ID="o0" Text="近三个月订单" OnClick="btnBindData_Click"  ></asp:LinkButton></li>
                    <li class="normal" id="Li1">
                        <asp:LinkButton runat="server" ID="o1" Text="等待付款" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li2">
                        <asp:LinkButton runat="server" ID="o2" Text="等待处理" OnClick="btnBindData_Click"  ></asp:LinkButton></li>
                    <li class="normal" id="Li3">
                        <asp:LinkButton runat="server" ID="o3" Text="取消订单" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li4">
                        <asp:LinkButton runat="server" ID="o4" Text="订单锁定" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li5">
                        <asp:LinkButton runat="server" ID="o5" Text="等待付款确认" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li6">
                        <asp:LinkButton runat="server" ID="o6" Text="正在处理" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li7">
                        <asp:LinkButton runat="server" ID="o7" Text="配货中" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li8">
                        <asp:LinkButton runat="server" ID="o8" Text="已发货" OnClick="btnBindData_Click"></asp:LinkButton></li>
                    <li class="normal" id="Li9">
                        <asp:LinkButton runat="server" ID="o9" Text="已完成" OnClick="btnBindData_Click"></asp:LinkButton></li>--%>
                    <%--                                            <li class="normal" id="Li10" >
                        <asp:LinkButton runat="server" ID="o10" Text="历史订单" OnClick="btnWaitPay_Click" ></asp:LinkButton></li>--%>
                    <li class="normal"><a href="OrdersList.aspx?type=0" >
                        <asp:Literal ID="Literal7" runat="server" Text="近三个月订单"></asp:Literal></a></li>
                    <li class="normal" id="tabPaying" style="display: none"><a href="OrdersList.aspx?type=1">
                        <asp:Literal ID="Literal8" runat="server" Text="等待付款"></asp:Literal></a></li>
                    <li class="normal" id="tabPreHandle"><a href="OrdersList.aspx?type=2">
                        <asp:Literal ID="Literal9" runat="server" Text="待审核"></asp:Literal></a></li>　
                    <li class="normal" id="tabCancel" style="display: none"><a href="OrdersList.aspx?type=3">
                        <asp:Literal ID="Literal12" runat="server" Text="取消订单"></asp:Literal></a></li>
                    <li runat="server" visible="false" class="normal" id="tabLocking" style="display: none">
                        <a href="OrdersList.aspx?type=4">
                            <asp:Literal ID="Literal13" runat="server" Text="订单锁定"></asp:Literal></a></li>
                   <%-- <li class="normal" id="tabPreConfirm" style="display: none"><a href="OrdersList.aspx?type=5">
                        <asp:Literal ID="Literal14" runat="server" Text="等待付款确认"></asp:Literal></a></li>--%>
                    <li class="normal" id="tabHandling" style="display: none"><a href="OrdersList.aspx?type=6">
                        <asp:Literal ID="Literal15" runat="server" Text="正在处理"></asp:Literal></a></li>
                    <li class="normal" id="tabShipping" style="display: none"><a href="OrdersList.aspx?type=7">
                        <asp:Literal ID="Literal16" runat="server" Text="配货中"></asp:Literal></a></li>
                    <li class="normal" id="tabShiped" style="display: none"><a href="OrdersList.aspx?type=8">
                        <asp:Literal ID="Literal17" runat="server" Text="已发货"></asp:Literal></a></li>
                    <li class="normal" id="tabSuccess" style="display: none"><a href="OrdersList.aspx?type=9">
                        <asp:Literal ID="Literal5" runat="server" Text="已完成"></asp:Literal></a></li>
                    <li class="normal" id="tabHistory" style="display: none"><a href="OrdersList.aspx?type=-1">
                        <asp:Literal ID="Literal18" runat="server" Text="历史订单"></asp:Literal></a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            OnRowCommand="gridView_RowCommand" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="False" DataKeyNames="OrderId" Style="float: left;">
            <Columns>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <a href="OrderShow.aspx?orderId=<%#Eval("OrderId") %>&type=<%#Type%>">
                            <%# Eval("OrderCode")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--      <asp:TemplateField ControlStyle-Width="50" HeaderText="订单来源" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#  Eval("ReferURL")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="下单时间" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
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
                 <asp:TemplateField ControlStyle-Width="50" HeaderText="订单所属" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120"  Visible="False">
                    <ItemTemplate>
                     <%#GetOrderTypeSub(Eval("OrderTypeSub"), Eval("SupplierId"), Eval("SupplierName"))%>
                       <%-- <%# (Eval("SupplierName")==null  || String.IsNullOrWhiteSpace(Eval("SupplierName").ToString())) ? "平台" : Eval("SupplierName")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="仓库" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" Visible="false" >
                    <ItemTemplate>
                    <%#Eval("depotname")%>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField ControlStyle-Width="50" HeaderText="来源" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="100">
                    <ItemTemplate>
                        <%#GetReferType( Eval("referType"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="160" HeaderText="订单操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%--   <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm%>"
                            Text="<%$Resources:Site,btnDeleteText%>"></asp:LinkButton>--%>
                        <a class="iframe" style="border: 1px transparent solid; padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;"
                            href="OrderShow.aspx?orderId=<%#Eval("OrderId") %>&type=<%#Type%>">查看</a>
                            
                      <span class="operate"  style="display:none;"  hasChildren="<%# Eval("HasChildren")%>"  orderstatus='<%# Eval("OrderStatus")%>'  ordertype="<%# Eval("OrderType")%>"  paymentstatus="<%# Eval("PaymentStatus")%>"  paymentgateway="<%# Eval("PaymentGateway")%>"  >
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Pay"
                            Style="display: none; border: 1px transparent solid; padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px;" CommandArgument='<%#Eval("OrderId")+","+Eval("OrderCode")%>'
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="您确定要将订单设置为已支付吗？请在支付网站方确认用户已支付."
                            Text="支付" class="PayAction"   payStatus='<%# Eval("PaymentStatus")%>'  payGateway='<%# Eval("PaymentGateway")%>'  OrderStatus='<%# Eval("OrderStatus")%>'></asp:LinkButton>
                        <a class="iframe ShipAction" shippingstatus="<%# Eval("ShippingStatus")%>" orderstatus='<%# Eval("OrderStatus")%>'  payGateway='<%# Eval("PaymentGateway")%>'   payStatus="<%# Eval("PaymentStatus")%>" 
                            href="OrderItemInfo.aspx?orderId=<%#Eval("OrderId") %>" style="display: none;
                            border: 1px transparent solid; padding-left: 5px; padding-right: 5px; margin-right: 5px;
                            padding-top: 2px; padding-bottom: 2px; white-space: nowrap;" >配货</a> 
                            <a class="iframeShiped ShipedAction"
                                shippingstatus="<%# Eval("ShippingStatus")%>" orderstatus='<%# Eval("OrderStatus")%>'
                                href="OrderShip.aspx?orderId=<%#Eval("OrderId") %>" style="display: none; border: 1px transparent solid;
                                 padding-left: 5px; padding-right: 5px; margin-right: 5px; padding-top: 2px; padding-bottom: 2px;
                                white-space: nowrap;">发货</a>
                     <%--   <asp:LinkButton ID="linkReturn" Visible="False" runat="server" CausesValidation="False"
                            CommandName="Return" CommandArgument='<%#Eval("OrderId") %>' OnClientClick='return confirm($(this).attr("ConfirmText"))'
                            ConfirmText="您确定要退货吗？" Text="退货"></asp:LinkButton>--%>
                        <asp:LinkButton ID="linkCancel" Style="display: none; border: 1px transparent solid;
                            padding-left: 5px; padding-right: 5px; margin-right: 5px; padding-top: 2px; padding-bottom: 2px;
                            white-space: nowrap;" OrderStatus='<%# Eval("OrderStatus")%>' class="CancelAction"
                           payGateway='<%# Eval("PaymentGateway")%>'    payStatus='<%# Eval("PaymentStatus")%>'  shippingStatus='<%# Eval("ShippingStatus")%>' 
                            runat="server" CausesValidation="False" CommandName="CancelOrder" CommandArgument='<%#Eval("OrderId")+","+Eval("OrderCode")%>'
                            Text="取消" OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="您确定要取消吗？"></asp:LinkButton>
<%--                        <asp:LinkButton ID="linkComplete" OnClientClick='return confirm($(this).attr("ConfirmText"))'
                            ConfirmText="您确定要完成吗？" Style="display: none; border: 1px #1317fc solid; padding-left: 5px;
                            padding-right: 5px; margin-right: 5px; padding-top: 2px; padding-bottom: 2px;
                            white-space: nowrap;" OrderStatus='<%# Eval("OrderStatus")%>' class="CompleteAction"
                            runat="server" CausesValidation="False" CommandName="Success" CommandArgument='<%#Eval("OrderId")+","+Eval("OrderCode")%>'
                            Text="完成"  shippingStatus='<%# Eval("ShippingStatus")%>'  ></asp:LinkButton>--%>
                            </span>

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
            <asp:Button ID="btnExport" runat="server" Text="一键导出" OnClientClick="return ExportConfirm();"
            OnClick="btnExport_Click" class="adminsubmit"></asp:Button>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
