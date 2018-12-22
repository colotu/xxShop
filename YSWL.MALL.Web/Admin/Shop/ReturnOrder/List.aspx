<%@ Page Title="退货单管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ReturnOrder.List" %>

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
            $("span:contains('已完成')").css("color", "green");
            $("span:contains('等待退款')").css("color", "#C27512");
            $("span:contains('取货中')").css("color", "#C27512");
            $("span:contains('审核未通过')").css("color", "red");
            $("span:contains('等待审核')").css("color", "#C27512");
            $("span:contains('已取消')").css("color", "red");
            
            
            $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });

            var SelectedCss = "active";
            var NotSelectedCss = "normal";

            $(".iframe").colorbox({ iframe: true, width: "840", height: "700", overlayClose: false });
            $(".iframeShiped").colorbox({ iframe: true, width: "900", height: "800", overlayClose: false });

            //  audit      pick      refund   
            $(".div_states").each(function () {
                var status =parseInt( $(this).attr("Status"));
                var logisticStatus =parseInt( $(this).attr("LogisticStatus"));
                var refundStatus = parseInt($(this).attr("RefundStatus"));
                if (status == 0) {//待审核
                    $(this).find('.audit').show();
                }
                if (status == 1 && logisticStatus > 0 && logisticStatus < 4) {
                    $(this).find('.pick').show();
                }
                if (status == 1 && refundStatus > 0 && refundStatus<4) {
                    $(this).find('.refund').show();
                }
            });
            // Status="<%#Eval("Status") %>"  LogisticStatus="<%#Eval("LogisticStatus") %>"   RefundStatus
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
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal3" runat="server" Text="退单管理" />
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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no mar-bt">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal1" runat="server" Text="退单号" />：
                    <asp:TextBox ID="txtReturnOrderCode" runat="server"></asp:TextBox>
                      &nbsp;&nbsp;<asp:Literal ID="Literal6" runat="server" Text="订单号" />：
                    <asp:TextBox ID="txtOrderCode" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;<asp:Literal ID="LiteralContactName" runat="server" Text="联系人" />：
                    <asp:TextBox ID="txtContactName" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;<asp:Literal ID="LiteralReturnUserName" runat="server" Text="用户名" />：
                    <asp:TextBox ID="txtReturnUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="pad-t10"> 
 
                    <asp:Literal ID="LiteralCreatedDate" runat="server" Text="申请日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="80px" CssClass="mar-r0">
                        
                    </asp:TextBox> - <asp:TextBox ID="txtCreatedDateEnd" Width="80px" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
  
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True" 
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            OnRowCommand="gridView_RowCommand" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="False " DataKeyNames="OrderId" Style="float: left;">
            <Columns>
             <asp:TemplateField ControlStyle-Width="50" HeaderText="退单号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <a class="iframe" href="Show.aspx?returnorderId=<%#Eval("ReturnOrderId") %>">
                            <%# Eval("ReturnOrderCode")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="订单号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                            <%# Eval("OrderCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField ControlStyle-Width="50" HeaderText="申请时间" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="240">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="商品出售总额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="150">
                    <ItemTemplate>
                        <%#Eval("ActualSalesTotal", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField ControlStyle-Width="50" Visible="false" HeaderText="实付金额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("Amount", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="应退金额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("AmountAdjusted", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="实退金额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("AmountActual", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="用户名" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("ReturnUserName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="联系人" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("ContactName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="状态" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" >
                    <ItemTemplate>
                      <span><%#GetMainStatus(Eval("Status"), Eval("LogisticStatus"), Eval("RefundStatus"))%></span>  
                    </ItemTemplate>
                </asp:TemplateField>       
                 <asp:TemplateField ControlStyle-Width="50" HeaderText="所属商家" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" >
                    <ItemTemplate>
                        <%# ( YSWL.Common.Globals.SafeInt(Eval("SupplierId"),0 )<=0) ? "平台" : Eval("SupplierName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="160" HeaderText="操作" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>    
                       <div class="div_states" Status="<%#Eval("Status") %>"  LogisticStatus="<%#Eval("LogisticStatus") %>"   RefundStatus="<%#Eval("RefundStatus") %>">
                        <a class="iframe" style="padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;"
                            href="Show.aspx?returnorderId=<%#Eval("ReturnOrderId") %>">查看</a>
                        <a class="iframe audit" style="padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;display:none;"
                            href="Audit.aspx?returnorderId=<%#Eval("ReturnOrderId") %>">审核</a>
                            <a class="iframe pick" style="padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;display:none;"
                            href="OrderPick.aspx?returnId=<%#Eval("ReturnOrderId") %>">已取货</a>
                            <a class="iframe  refund" style="padding-left: 5px; padding-right: 5px;
                            margin-right: 5px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;display:none;"
                            href="Refund.aspx?returnId=<%#Eval("ReturnOrderId") %>">退款</a>
                            </div>
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
