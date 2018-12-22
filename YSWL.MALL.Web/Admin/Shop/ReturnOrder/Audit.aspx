<%@ Page Language="C#"  Title="详情" AutoEventWireup="true" MasterPageFile="~/Admin/BasicNoFoot.Master" CodeBehind="Audit.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ReturnOrder.Audit" %>

 
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
            width: 108px;
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
            padding-bottom: 0;
            padding-top: 0;
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
        $(function () {
            var tabIndex = $("[id$=hfTabIndex]").val();
            // nTabs(this, 1);
            $("#myTab1").find("li").each(function () {
                var index = $(this).attr("tabindex");
                if (tabIndex == index) {
                    $(this).click();
                }
            });

            //tab 定位
            $("#myTab1").find("li").click(function () {
                var tab = $(this).attr("tabindex");

                $("[id$=hfTabIndex]").val(tab);
            });


            $('.OnlyFloat').OnlyFloat();
        });


        function UpdateAmountAdjusted(sender) {
            var amount = $(sender).parent().find('[id$=lblAmountAdjusted]').text();
            var context = $(sender).hide().parent();
            context.find('[id$=txtAmountAdjusted]').show().val(amount);
            context.find('[id$=lnkSaveAmountAdjusted]').show();
            context.find('[id$=lblAmountAdjusted]').hide();
        }

        //应退金额提示
        function AmountAdjustedTip() {
            var amount = parseFloat($('[id$="txtAmountAdjusted"]').val());
            if (confirm('您确认应退金额为 ' + amount.toFixed(2) + ' 吗？')) {
                return true;
            } else {
                return false;
            }
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
                        <asp:Literal ID="lblTitle" runat="server" Text="正在查看退单详细信息" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="nTab4">
            <div class="TabTitle">
                <ul id="myTab1">
                    <li class="active" onclick="nTabs(this,0);" tabindex="0"><a href="javascript:;">基本信息</a></li>
                    <li class="normal" onclick="nTabs(this,1);" tabindex="1"><a href="javascript:;">商品清单</a></li>
                    <li class="normal" onclick="nTabs(this,2);" tabindex="2"><a href="javascript:;">取货信息</a></li>
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
                                        <asp:Literal ID="Literal4" runat="server" Text=" 订单号" />：
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
                                <tr style="display:none;">
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
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal14" runat="server" Text=" 退货原因" />：
                                    </td>
                                    <td height="25" class="td_content">
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                 
                                    </td>
                                </tr>
                              <tr><td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal1" runat="server" Text="图片信息" />：
                                    </td>
                                    <td height="25" class="td_content" >
                                           <%=ImageUrls%> 
                                    </td></tr>
                                     <tr  runat="server" id="trCoupon" visible="false"><td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal11" runat="server" Text="原订单优惠劵信息" />：
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
                                        <asp:Literal ID="Literal7" runat="server" Text="商品出售总额" />：
                                    </td>
                                    <td height="25" class="td_content">
                                     <asp:Label ID="lblActualSalesTotal" runat="server"></asp:Label>
                                    </td>
                                </tr> 
                                <tr style="display:none;">
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal2" runat="server" Text="实付金额" />：
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
                                        <asp:TextBox ID="txtAmountAdjusted" MaxLength="10" CssClass="OnlyFloat" runat="server" style="width: 80px;display: none;" ></asp:TextBox>
                                        <asp:Image ID="imgModifyAmountAdjusted" ToolTip="修改" AlternateText="修改" ImageUrl="/admin/Images/up_xiaobi.png" Visible="False" OnClick="return UpdateAmountAdjusted(this);"  runat="server"/>
                                        <asp:LinkButton ID="lnkSaveAmountAdjusted" OnClick="lnkSaveAmountAdjusted_OnClick" style="display: none;" runat="server" Text="保存"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal9" runat="server" Text="是否需要取货" />：
                                    </td>
                                    <td height="25" class="td_content">
                                    <asp:CheckBox ID="chkReturngoods" runat="server" Checked="true" />是
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                       <tr runat="server" >
                        <td style="vertical-align: top;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                                          <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal10" runat="server" Text="拒绝理由" />：
                                    </td>
                                    <td height="25" class="td_content">
                                         <asp:TextBox ID="txtRefuseReason" TextMode="MultiLine"  Width="400px" Height="100px"  MaxLength="500"   runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_class" style="background-color: #E2E8EB">
                                        <asp:Literal ID="Literal5" runat="server" Text="备注" />：
                                    </td>
                                    <td height="25" class="td_content">
                                         <asp:TextBox ID="txtRemark" TextMode="MultiLine"  Width="400px" Height="100px"    runat="server" ></asp:TextBox> 
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
 <tr><td height="25" colspan="2" align="center">
                    <asp:Button ID="btnPass" runat="server" Text="审核通过"
   OnClick="btnPass_OnClick" class="adminsubmit_short"   OnClientClick="return AmountAdjustedTip();" ></asp:Button>
   <asp:Button ID="btnRefusal" runat="server" Text="拒绝" OnClick="btnRefusal_OnClick" class="adminsubmit_short"></asp:Button></td></tr>
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
            <%--取货信息--%>
            <div id="myTab1_Content2" tabindex="2" class="none4">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
