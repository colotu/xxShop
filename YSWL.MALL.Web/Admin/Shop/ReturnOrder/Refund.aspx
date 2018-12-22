
<%@ Page Language="C#" Title="退货单退款" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="Refund.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ReturnOrder.Refund" %>

<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/tab.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .td_class
        {
            width: 108px;
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
    
    <script type="text/javascript">
       
        function UpdateAmountActual(sender) {
            var amount = $(sender).parent().find('[id$=lblAmountActual]').text();
            var context = $(sender).hide().parent();
            context.find('[id$=txtAmountActual]').show().val(amount);
            context.find('[id$=lnkSaveAmountActual]').show();
            context.find('[id$=lblAmountActual]').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfSuccess" runat="server" />
    <asp:HiddenField ID="hfSupplierId" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="lblTitle" runat="server" Text="" />
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
                                        <asp:Literal ID="Literal6" runat="server" Text=" 退货类型" />：
                                    </td>
                                    <td height="25" class="td_content">
                                    <asp:Label ID="lblReturnGoodsType" runat="server"></asp:Label>
                                  <div style="margin-left: 50px;margin-top: -25px;color:#E29E25;"> 提示：1.如果退货类型为【整单退】,应退金额等于原订单的实际支付金额</br>
                                                                     &#12288;&#12288;&#12288;2.如果退货类型为【部分退】,应退金额默认为0</br>
                                                                     &#12288;&#12288;&#12288;可根据实际情况来调整应退金额。</div> 
                                    </td>
                                </tr>
                                  <tr  runat="server" id="trCoupon" visible="false"><td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal1" runat="server" Text="原订单优惠劵信息" />：
                                    </td>
                                    <td height="25" class="td_content" style="padding-left: 0px;">
                                        <table>       
                                                <tr>
                                    <td  >
                                        <asp:Literal ID="Literal25" runat="server" Text=" 优惠劵号" />：  
                                    </td>
                                    <td  >
                                        <asp:Label ID="lblCouponCode" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  >
                                        <asp:Literal ID="Literal26" runat="server" Text=" 优惠劵名称" />：  
                                    </td>
                                    <td >
                                        <asp:Label ID="lblCouponName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td  >
                                        <asp:Literal ID="Literal27" runat="server" Text=" 优惠劵金额" />：  
                                    </td>
                                    <td >
                                        <asp:Label ID="labelCouponAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                        
                                        </table>
                                    </td></tr>  
                               <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal4" runat="server" Text="商品出售总额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                     <asp:Label ID="lblActualSalesTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>    
                                 <tr  style="display:none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal7" runat="server" Text="实付金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                     <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>                                  
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal8" runat="server" Text="应退金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                    <asp:Label ID="lblAmountAdjusted" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr  runat="server" id="trReturnCoupon" visible="false"> 
                                <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal5" runat="server" Text="是否退回优惠劵" />：
                                    </td>
                                    <td height="25" class="td_content">
                                      <asp:CheckBox ID="chkReturnCoupon" runat="server"/>是
                                    </td></tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal11" runat="server" Text=" 实退金额" />：
                                    </td>
                                     <td height="25" class="td_content">
                                        <asp:TextBox ID="txtAmountActual" MaxLength="10" CssClass="OnlyFloat" runat="server" style="width: 80px;" ></asp:TextBox>     
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
                            <div  style="color:Red;"  id="divtip"  >确认后款项将自动退到账户余额中.</div>
                                <asp:Button ID="btnSave" runat="server" style="display: none;" OnClientClick="return Save($(this))"  OnClick="btnSave_OnClick" Text="确认退款" class="adminsubmit" ></asp:Button>
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

            if (parseInt($('[id$=hfSupplierId]').val())>0) {
                $('#divtip').text('确认退款后款项将自动从商家的账户余额中扣除，退到用户账户余额中.');
            }

        })()




        function Save(_self) {
            _self.hide();
            var suppId = parseInt($('[id$=hfSupplierId]').val());
            var tipval = '确认退款￥' + $('[id$="txtAmountActual"]').val() + ' 吗？\n\n确认后款项将自动退到账户余额中';
            if (suppId > 0) {
                tipval = '确认退款￥' + $('[id$="txtAmountActual"]').val() + ' 吗？\n\n确认退款后款项将自动从商家的账户余额中扣除，退到用户账户余额中.';
            }
            if (confirm(tipval)) { 
                return true;
            }
            _self.show();
            return false;
        }
    </script>
</asp:Content>

