<%@ Page Title="代退货" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="OrdersList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.SubstituteReturn.OrdersList" %>

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

            var SelectedCss = "active";
            var NotSelectedCss = "normal";

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

            //申请退货
            $('.Apply').each(function () {
                var _self = $(this);
                var oi = _self.attr("oi");
                var os = parseInt(_self.attr("os"));
                var suppid = parseInt(_self.attr("suppid"));
//                if (!isNaN(suppid) && suppid > 0) { 
//                    _self.parents('td').text('第三方商品暂不支持退货');
//                } else {
                    if (os == 2) {
                        $.ajax({
                            url: ("OrdersList.aspx?timestamp={0}").format(new Date().getTime()),
                            type: 'POST', dataType: 'json', timeout: 10000,
                            data: { Action: "AjaxRetuOrder", Callback: "true", oi: oi, os: os },
                            success: function (resultData) {
                                if (resultData.STATUS == "True") {
                                    _self.show();
                                }
                                else {
                                    //_self.parents('tr').css('backgroundColor', 'rgb(250, 250, 250)').attr('title', '已申请');                                  
                                    _self.parents('td').text('已申请');
                                    _self.remove();
                                }
                            }
                        });
                    }
                //}
                
            });
            $(".iframe").colorbox({ iframe: true, width: "840", height: "700", overlayClose: false });
            
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal3" runat="server" Text="代退货" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以查询订单" />
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
                    <asp:Literal ID="LiteralCreatedDate" runat="server" Text="下单日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="80px" CssClass="mar-r0">
                        
                    </asp:TextBox>-<asp:TextBox ID="txtCreatedDateEnd" Width="80px" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <br />
        
  
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            OnRowCommand="gridView_RowCommand" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="False " DataKeyNames="OrderId" Style="float: left;">
            <Columns>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>    
                            <%# Eval("OrderCode")%>
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
                    ItemStyle-Width="120"  Visible="false">
                    <ItemTemplate>
                        <%# (Eval("SupplierName")==null  || String.IsNullOrWhiteSpace(Eval("SupplierName").ToString())) ? "平台" : Eval("SupplierName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="80" HeaderText="操作" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                    
                        <a suppid="<%# Eval("SupplierId")%>"   class="Apply iframe" oi="<%# Eval("OrderId")%>"  os="<%# Eval("OrderStatus")%>"  href="Apply.aspx?OrderId=<%#Eval("OrderId") %>"  style="display:none; padding-left: 5px; padding-right: 5px; margin-right: 5px;
                            padding-top: 2px; padding-bottom: 2px; white-space: nowrap;">退货</a>
                   
                       
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
