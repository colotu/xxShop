<%@ Page Title="促销活动" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ActivityInfo.List" %>
<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<%@ Register TagPrefix="YSWL" TagName="AjaxRegion" Src="~/Controls/AjaxRegion.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
      <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
         <%--<link href="/Admin/js/select2-2.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-2.1/select2.min.js" type="text/javascript"></script>--%>
        <link href="/Scripts/jqueryui/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(function () {
       //  $(".iframe").colorbox({ iframe: true, width: "680", height: "488", overlayClose: false });
         $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
         $("[id$='txtStartDate']").prop("readonly", true).datepicker({
            
             changeMonth: true,
             dateFormat: "yy-mm-dd",
             onClose: function (selectedDate) {
                 $("[id$='txtEndDate']").datepicker("option", "minDate", selectedDate);
             }
         });
         $("[id$='txtEndDate']").prop("readonly", true).datepicker({
            
             changeMonth: true,
             dateFormat: "yy-mm-dd",
             onClose: function (selectedDate) {
                 $("[id$='txtStartDate']").datepicker("option", "maxDate", selectedDate);
                 $("[id$='txtEndDate']").val($(this).val());
             }
         });
 
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
                        <asp:Literal ID="Literal1" runat="server" Text="促销活动管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以对促销活动进行新增，编辑和删除操作
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
                    <asp:Literal ID="Literal3" runat="server" Text="状态" />：
                    <asp:DropDownList ID="ddlStatus" runat="server">
                         <asp:ListItem Text=" 全   部" Value=""></asp:ListItem>
                        <asp:ListItem Text=" 启   用" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" 未启用" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                     规则：
                     <asp:DropDownList ID="ddlRule" runat="server"  >      
                    </asp:DropDownList>
                    开始时间：
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                   结束时间：
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                  
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" runat="server" class="add-btn">
                        <a href="add.aspx" >
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Site, lblAdd%>" /></a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="def-wrapper">
                    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="ActivityId">
            <Columns>
             <asp:TemplateField ControlStyle-Width="50" HeaderText="规则"    >
                                <ItemTemplate>
                                  <%# GetRuleName( Eval("RuleId"))%>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="指定分类"   ItemStyle-HorizontalAlign="left"   >
                                <ItemTemplate>       
                               <%#  GetCategoryName(Eval("BuyCategoryId"))%> 
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField ControlStyle-Width="50" HeaderText="指定商品"   ItemStyle-HorizontalAlign="left"   >
                                <ItemTemplate>
                                   <%#Eval("BuyProductName")%> 
                                </br>
                                    数量: <%#Eval("BuyCount")%>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="赠送"   ItemStyle-HorizontalAlign="left"   >
                                <ItemTemplate>
                                      <%# YSWL.Common.Globals.SafeInt(Eval("RuleId"), 0) == 3 ? ("优惠劵:" + GetCpRuleName(Eval("CpRuleId"))) : ("商品:" + Eval("ProductName") )%> 
                            <%--  <%# YSWL.Common.Globals.SafeInt(Eval("RuleId"), 0) == 3 ? ("优惠劵:" + GetCpRuleName(Eval("CpRuleId"))) : ("商品:<a  href=\""+ YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop)+"product/Detail/"+Eval("ProductId")+"\"  target=\"_blank\"  >"   + Eval("ProductName") +" </a>"  )%> --%>
                               </br> 数量: <%# Eval("Count") %>
                                </ItemTemplate>
                            </asp:TemplateField> 
         <asp:TemplateField   ItemStyle-Width="100"  HeaderText="消费金额区间"  ItemStyle-HorizontalAlign="center"   >
                                <ItemTemplate>
                                  <span style="font-family: -webkit-pictograph;"><%#Eval("LimitPrice", "{0:C2}")%>—<%#Eval("LimitMaxPrice", "{0:C2}")%></span> 
                                </ItemTemplate>
                            </asp:TemplateField> 	 
		
       <asp:TemplateField HeaderText="活动时间" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="160"  >
                    <ItemTemplate>
                      <%#Eval("StartDate", "{0:yyyy-MM-dd}")%> 至  <%#Eval("EndDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="创建人" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90"  >
                    <ItemTemplate>
                      <%#  GetUserName(Eval("CreatedUserId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
	 
		<asp:BoundField DataField="CreatedDate" HeaderText="创建时间" SortExpression="CreatedDate" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="110"  /> 
        <asp:TemplateField ControlStyle-Width="50" HeaderText="状态"  ItemStyle-HorizontalAlign="Center"    >
                                <ItemTemplate>
                                  <%#(Eval("Status").ToString()=="1"?"启用":"未启用")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField  ItemStyle-HorizontalAlign="Center"  ControlStyle-CssClass="iframe"   HeaderText="编辑" ControlStyle-Width="50" DataNavigateUrlFields="ActivityId" DataNavigateUrlFormatString="Modify.aspx?id={0}"
                                Text="编辑"  />
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="删除"   ItemStyle-HorizontalAlign="Center"   >
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                         Text="删除"></asp:LinkButton>
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

        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--Title -->

            <!--Title end -->

            <!--Add  -->

            <!--Add end -->

            <!--Search -->
            <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>
                    <td style="width: 80px" align="right" class="tdbg">
                         <b>关键字：</b>
                    </td>
                    <td class="tdbg">                       
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="查询"  OnClick="btnSearch_Click" >
                    </asp:Button>                    
                        
                    </td>
                    <td class="tdbg">
                    </td>
                </tr>
            </table>
            <!--Search end-->
            <br />
            <asp:GridView ID="gridView" runat="server" AllowPaging="True" Width="100%" CellPadding="3"  OnPageIndexChanging ="gridView_PageIndexChanging"
                    BorderWidth="1px" DataKeyNames="ActivityId" OnRowDataBound="gridView_RowDataBound"
                    AutoGenerateColumns="false" PageSize="10"  RowStyle-HorizontalAlign="Center" OnRowCreated="gridView_OnRowCreated">
                    <Columns>
                    <asp:TemplateField ControlStyle-Width="30" HeaderText="选择"    >
                                <ItemTemplate>
                                    <asp:CheckBox ID="DeleteThis" onclick="javascript:CCA(this);" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            
		<asp:BoundField DataField="RuleId" HeaderText="规则Id" SortExpression="RuleId" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="BuyProductId" HeaderText="购买指定的商品" SortExpression="BuyProductId" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="BuySKU" HeaderText="购买指定的商品SKU" SortExpression="BuySKU" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="BuyCount" HeaderText="购买指定商品的数量" SortExpression="BuyCount" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="ProductId" HeaderText="赠送商品Id" SortExpression="ProductId" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="SKU" HeaderText="商品SKU" SortExpression="SKU" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="SalePrice" HeaderText="促销总价" SortExpression="SalePrice" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="LimitPrice" HeaderText="最低消费金额" SortExpression="LimitPrice" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Count" HeaderText="数量" SortExpression="Count" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="MaxCount" HeaderText="限售总数量" SortExpression="MaxCount" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Status" HeaderText="状态" SortExpression="Status" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="StartDate" HeaderText="开始时间" SortExpression="StartDate" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="EndDate" HeaderText="结束时间" SortExpression="EndDate" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CreatedUserId" HeaderText="创建人ID" SortExpression="CreatedUserId" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CreatedDate" HeaderText="创建时间" SortExpression="CreatedDate" ItemStyle-HorizontalAlign="Center"  /> 
                            
                            <asp:HyperLinkField HeaderText="详细" ControlStyle-Width="50" DataNavigateUrlFields="ActivityId" DataNavigateUrlFormatString="Show.aspx?id={0}"
                                Text="详细"  />
                            <asp:HyperLinkField HeaderText="编辑" ControlStyle-Width="50" DataNavigateUrlFields="ActivityId" DataNavigateUrlFormatString="Modify.aspx?id={0}"
                                Text="编辑"  />
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="删除"   Visible="false"  >
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                         Text="删除"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                </asp:GridView>
               <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                <tr>
                    <td style="width: 1px;">                        
                    </td>
                    <td align="left">
                        <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"/>                       
                    </td>
                </tr>
            </table>
</asp:Content>--%>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>
