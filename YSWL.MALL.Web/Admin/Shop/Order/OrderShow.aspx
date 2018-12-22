<%@ Page Language="C#" Title="订单详情" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="OrderShow.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Order.OrderShow" %>

<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<%@ Register TagPrefix="uc1" TagName="Region" Src="~/Controls/Region.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/admin/js/tab.js" type="text/javascript"></script>
    <link href="/admin/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="/ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="/ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <!--jQueryUploadify Start-->
    <!--jQueryUploadify End-->
    <style type="text/css">
        .td_class
        {
            width: 100px;
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
            padding-bottom: 0;
            padding-top: 0;
        }
        .td_content
        {
            border:1px solid #ddd;
            /*border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;*/
        }
        td.td_content select,
        td.td_content input{
            margin-top:2px;
            margin-bottom:2px;
            height:28px;
        }
        .font_color{ color: #118C01;}
    </style>
    <script type="text/javascript">
        $(function () {
            var total = 0.00;
            $(".txtprice").each(function () {
                total += parseFloat($(this).text());
            });
           // $("#lblTotalPrice").text(parseFloat(total * 100 * 1.00 / 100).toFixed(2));

            var value = parseInt($("[id$='hfOrderMainStatus']").val());
            var url = $("[id$='hfExpressUrl']").val();
            $("#txtExpress").attr("src", url);
            if (value >= 8) {
                $("#btnAddSave").hide();
            }
            //$(".kd-bottom", window.frames['txtExpress'].document).hide();
            //lblFreightAdjusted
            //span_Freight
            if ($.trim($("[id$='lblFreight']").text()) != $.trim($("[id$='lblFreightAdjusted']").text())) {
                $("#span_Freight").show();
            }
            if ($.trim($("[id$='lblFreight']").text()) != $.trim($("[id$='lblFreightActual']").text())) {
                $("#trFreightActual").show();
            }
            if ($("[id$=hidShippingModeId]").val() == $("[id$=hidRealShippingModeId]").val()) {
                $("#trRealShippingModeName").hide();
            }
            var tabIndex = $("[id$=hfTabIndex]").val();
           // nTabs(this, 1);
            $("#myTab1").find("li").each(function() {
                var index = $(this).attr("tabindex");
                if (tabIndex == index) {
                    $(this).click();
                }
            });

            //tab 定位
            $("#myTab1").find("li").click(function() {
                var tab = $(this).attr("tabindex");
                
                $("[id$=hfTabIndex]").val(tab);
            });
        });
        $('.OnlyFloat').OnlyFloat();
        function UpdateAmount(sender) {
            var amount = $(sender).parent().find('[id$=lblAmount]').text();
            var context = $(sender).hide().parent();
            context.find('[id$=txtAmount]').show().val(amount);
            context.find('[id$=lnkSaveAmount]').show();
            context.find('[id$=lblAmount]').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfOrderMainStatus" runat="server" />
     <asp:HiddenField ID="hfTabIndex" runat="server" Value="" />
    <div class="newslistabout">
        
        <div class="newslist_title">
            
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="lblTitle" runat="server" Text="正在查看订单详细信息" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="nTab4 mar-b0">
            <div class="TabTitle">
                <ul id="myTab1">
                    <li class="active" onclick="nTabs(this,0);" tabindex="0"><a href="javascript:;">基本信息</a></li>
                    <li class="normal" onclick="nTabs(this,1);" tabindex="1"><a href="javascript:;">商品清单</a></li>
                    <li class="normal" onclick="nTabs(this,2);" tabindex="2"><a href="javascript:;">配送信息</a></li>
                    <li class="normal" onclick="nTabs(this,3);" tabindex="3"><a href="javascript:;">订单跟踪</a></li>
                    <li class="normal" onclick="nTabs(this,4);" style="display: none" tabindex="4"><a href="javascript:;">订单备注</a></li>
                     <li class="normal" onclick="nTabs(this,5);" tabindex="5" style="display: none"><a href="javascript:;">物流跟踪</a></li>
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
                                    <td colspan="2" class="newstitle pad-t0" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">订单商品价格</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal2" runat="server" Text=" 商品总额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                     <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trFreight">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal28" runat="server" Text="配送费用" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblFreightAdjusted" runat="server"></asp:Label>
                                        <span   id="span_Freight" style="display: none;">
                                           &nbsp;&nbsp;( <asp:Literal ID="Literal29" runat="server" Text="原配送费用" /><asp:Label ID="lblFreight" runat="server"></asp:Label> )
                                        </span>
                                    </td>        
                                </tr>
                                <tr id="trFreightActual" style="display: none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal21" runat="server" Text=" 实际配送费用" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblFreightActual" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                
                                <tr style="display: none">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal15" runat="server" Text=" 手续费" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal8" runat="server" Text=" 订单总额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal11" runat="server" Text=" 折扣金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblDiscountAdjusted" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr  style="display:none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal12" runat="server" Text=" 优惠金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblCouponAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal13" runat="server" Text=" 应付金额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtAmount" MaxLength="10" CssClass="OnlyFloat" runat="server" style="width: 80px;display: none;" Visible="False"></asp:TextBox>
                                        <asp:Image ID="imgModifyAmount" ToolTip="修改" AlternateText="修改" ImageUrl="/admin/Images/up_xiaobi.png" Visible="False" OnClick="return UpdateAmount(this)" runat="server"/>
                                        <asp:LinkButton ID="lnkSaveAmount" OnClick="lnkSaveAmount_OnClick" style="display: none;" runat="server" Visible="False" Text="保存"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                     <tr runat="server"  id="trCoupon" visible="false">
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                                <tr>
                                    <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">优惠劵信息</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal25" runat="server" Text=" 优惠劵号" />：  
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblCouponCode" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal26" runat="server" Text=" 优惠劵名称" />：  
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblCouponName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal27" runat="server" Text=" 优惠劵金额" />：  
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="labelCouponAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                                <tr>
                                    <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">订单其它信息</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal5" runat="server" Text=" 配送方式" />：
                                        <asp:HiddenField ID="hidShippingModeId" runat="server" />
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblShippingModeName" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr id="trRealShippingModeName">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal23" runat="server" Text=" 实际配送方式" />：
                                        <asp:HiddenField ID="hidRealShippingModeId" runat="server" />
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblRealShippingModeName" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal20" runat="server" Text=" 物流公司" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblExpressCompanyName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal1" runat="server" Text=" 物流单号" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblShipOrderNumber" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal17" runat="server" Text=" 支付方式" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPaymentTypeName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal19" runat="server" Text=" 商品重量" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblWeight" runat="server"></asp:Label>克
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal6" runat="server" Text=" 发票抬头" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lbITitle" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal30" runat="server" Text=" 发票内容" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lbIContent" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="td_class" style="background-color: #E2E8EB; ">
                                        <asp:Literal ID="Literal115" runat="server" Text=" 可得积分" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblPoint" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="newstitle" style="color: #666">
                            <span style="font-size: 16px; padding-left: 20px">订单备注</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="float: left; padding-left: 30px">
                             <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine"  Width="400px" Height="100px"  ></asp:TextBox>
                            
                        </td>
                    </tr>  
                    <tr><td height="25" colspan="2" align="center"> <asp:Button ID="btnSaveRemark" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                            OnClick="btnSave_Click" class="adminsubmit_short"  Visible="false"></asp:Button></td></tr>
                </table>
            </div>
            <%--     商品清单--%>
            <div id="myTab1_Content1" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"  cellspacing="1" class="border">
                    <tr>
                        <td class="tdbg">
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
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="商品编号" ItemStyle-HorizontalAlign="Center"
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
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="购买数量" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%# Eval("Quantity")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="80" HeaderText="商品单价" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <span class="txtprice">
                                                <%# YSWL.Common.Globals.SafeDecimal(Eval("AdjustedPrice").ToString(),0).ToString("F")%></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="发货数量" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="80" Visible="False">
                                        <ItemTemplate>
                                            <%#Eval("ShipmentQuantity")%>
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
            <%--配送信息--%>
            <div id="myTab1_Content2" tabindex="2" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="">
                    <tr>
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang"
                                style="padding-top: 8px">
                                <tr>
                                    <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">收货人信息</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal3" runat="server" Text="收货人" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:TextBox ID="txtShipName" runat="server" Width="320px"></asp:TextBox>
                                        <%--   <asp:Label ID="lblShipName" runat="server"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal4" runat="server" Text="收货人地区" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <%--      <asp:Label ID="lblShipRegion" runat="server"></asp:Label>--%>
                                        <%--         <YSWL:RegionDropList ID="RegionList" runat="server" IsNull="true" />--%>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <uc1:Region ID="RegionList" runat="server" VisibleAll="true" VisibleAllText="--请选择--" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal7" runat="server" Text="详细地址" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:TextBox ID="txtShipAddress" runat="server" Width="320px"></asp:TextBox>
                                        <%--    <asp:Label ID="lblShipAddress" runat="server"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal9" runat="server" Text="电话号码" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:TextBox ID="txtShipTelPhone" runat="server" Width="320px"></asp:TextBox>
                                        <%--  <asp:Label ID="lblShipTelPhone" runat="server"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal10" runat="server" Text="手机号码" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:TextBox ID="txtShipCellPhone" runat="server" Width="320px"></asp:TextBox>
                                        <%--   <asp:Label ID="lblShipCellPhone" runat="server"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal14" runat="server" Text="邮政编码" />：
                                    </td>
                                    <td height="25" class="td_content">
                                          <asp:TextBox ID="txtShipZipCode" runat="server" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal16" runat="server" Text="邮箱地址" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblShipEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang"
                                style="padding-top: 8px">
                                <tr>
                                    <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                        <span style="font-size: 16px; padding-left: 20px">购买人信息</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal18" runat="server" Text=" 购买人" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblBuyerName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal22" runat="server" Text="手机号码" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblBuyerCellPhone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal24" runat="server" Text="邮箱地址" />：
                                    </td>
                                    <td height="25" class="td_content">
                                        <asp:Label ID="lblBuyerEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
                                    <td height="25" style="text-align: center" id="btnAddSave">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="保存" class="adminsubmit_short" Visible="false">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <%--订单跟踪--%>
            <div id="myTab1_Content3" tabindex="3" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td class="tdbg">
                            <cc1:GridViewEx ID="gridView_Action" runat="server" AllowPaging="True" AllowSorting="True"
                                ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindAction" OnPageIndexChanging="gridView_Action_PageIndexChanging"
                                OnRowDataBound="gridView_RowDataBound" Width="100%" PageSize="10" ShowExportExcel="False"
                                ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
                                ShowCheckAll="False" Style="float: left;">
                                <Columns>
                                    <asp:TemplateField ControlStyle-Width="200" HeaderText="处理时间" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="200">
                                        <ItemTemplate>
                                            <%#YSWL.Common.Globals.SafeDateTime(Eval("ActionDate").ToString(),DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="处理信息" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#GetActionCode(Eval("ActionCode"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  ControlStyle-Width="50" HeaderText="处理明细" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Eval("Remark")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="操作人" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%#Eval("Username")%>
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
            <%--订单备注--%>
            <div id="myTab1_Content4" tabindex="4" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td class="tdbg">
                            <cc1:GridViewEx ID="gridView_Remark" runat="server" AllowPaging="True" AllowSorting="True"
                                ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindRemark" OnPageIndexChanging="gridView_Remark_PageIndexChanging"
                                OnRowDataBound="gridView_RowDataBound" Width="100%" PageSize="10" ShowExportExcel="False"
                                ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
                                ShowCheckAll="False" Style="float: left;">
                                <Columns>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="操作时间" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#YSWL.Common.Globals.SafeDateTime(Eval("CreatedDate").ToString(), DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ControlStyle-Width="50" HeaderText="操作人" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("UserName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="备注信息" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Eval("Remark")%>
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
             <%--物流跟踪--%>
            <div id="myTab1_Content5"  tabindex="5" class="none4">
                <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                    cellspacing="1" class="border">
                    <tr>
                        <td class="tdbg">
                            <cc1:GridViewEx ID="gridView_Express" runat="server" AllowPaging="True" AllowSorting="True"
                                ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindExpress" OnPageIndexChanging="gridView_Express_PageIndexChanging"
                                OnRowDataBound="gridView_RowDataBound" Width="100%" PageSize="25" ShowExportExcel="False"
                                ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
                                ShowCheckAll="False"  Style="float: left;">
                                <Columns>
                                    <asp:TemplateField HeaderText="内容" ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            【<%#Eval("Date")%>】  <%#Eval("Content")%>
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
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfExpressUrl" runat="server" />
                            
                            <iframe id="txtExpress" name="txtExpress" src=""    width="600" height="380" marginwidth="0" marginheight="0" hspace="0" vspace="0" frameborder="0" scrolling="no"></iframe>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
