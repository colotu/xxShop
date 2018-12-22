<%@ Page Title="预定记录" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Shop.Order.PreOrder.List" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
        <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
     <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            $("span:contains('未处理')").css("color", "#d71345");
            $("span:contains('已处理')").css("color", "#006400");
            $("[id$='ddlUserId']").select2({ placeholder: "请选择" });
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

            $(".iframe").colorbox({ iframe: true, width: "800", height: "560", overlayClose: false });
        });
        function GetDeleteM() {
            $("[id$='btnDelete']").click();
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
                         <asp:Literal ID="Literal1" runat="server" Text="预定记录" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以处理<asp:Literal ID="Literal3" runat="server" Text="预定记录" />
                    </td>
                </tr>
            </table>
        </div>
    <!--Title end -->
    <!--Add  -->
    <!--Add end -->
    <!--Search -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
        <tr>
            <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                状态：
                <asp:DropDownList ID="dropStatus" runat="server">
                    <asp:ListItem Value="0">请选择</asp:ListItem>
                    <asp:ListItem Value="1">未处理</asp:ListItem>
                    <asp:ListItem Value="2">已处理</asp:ListItem>
                </asp:DropDownList>
               <asp:Literal ID="LiteralSupplier" runat="server" Text="用户" />：
                    <asp:DropDownList ID="ddlUserId" runat="server" Width="180">
                    </asp:DropDownList>
                <asp:Literal ID="LiteralCreatedDate" runat="server" Text="订购日期" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="80px">                     
                    </asp:TextBox>-<asp:TextBox ID="txtCreatedDateEnd" Width="80px" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                    OnClick="btnSearch_Click" class="adminsubmit"></asp:Button>
            </td>
        </tr>
    </table>
    <!--Search end-->
    <br />
   <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
        ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
        OnRowDataBound="gridView_RowDataBound"  UnExportedColumnNames="Modify"
        Width="100%" PageSize="10" ShowExportExcel="True" ShowExportWord="False" ExcelFileName="FileName1"
        CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="PreOrderId" OnRowCommand="gridView_RowCommand" >
          <Columns> 
           <asp:TemplateField   HeaderText="商品名称" ItemStyle-HorizontalAlign="left">
                <ItemTemplate>
                   <a target="_blank" href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/detail/<%# Eval("ProductId")%>">
                      <%# Eval("ProductName")%></a>
                </ItemTemplate>
            </asp:TemplateField>
		<asp:BoundField DataField="Count" HeaderText="数量" SortExpression="Count" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Phone" HeaderText="电话" SortExpression="Phone" ItemStyle-HorizontalAlign="Center"  /> 
        <asp:BoundField DataField="UserName" HeaderText="所属用户" SortExpression="UserId" ItemStyle-HorizontalAlign="Center"  /> 
			<asp:TemplateField ItemStyle-Width="180" HeaderText="创建时间" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>  
                 <%# ( (DateTime)Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                  </ItemTemplate>                
            </asp:TemplateField>
		<asp:TemplateField ItemStyle-Width="180" HeaderText="处理人" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>  
                   <%# GetUserName (Eval("HandleUserId"))%>
                  </ItemTemplate>                
            </asp:TemplateField>
		<asp:BoundField DataField="HandleDate" HeaderText="处理时间" SortExpression="HandleDate" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="DeliveryDate" HeaderText="配送提示" Visible="false" SortExpression="DeliveryDate" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:TemplateField ItemStyle-Width="180" HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>                    
                     <asp:LinkButton Visible="false"  ID="lbtnHandle" runat="server" CommandArgument='<%#Eval("PreOrderId")+",1"%>'
                            Style="color: #0063dc;" CommandName="Status">已处理</asp:LinkButton>  
                         <asp:LinkButton Visible="false"  ID="lbtnNotHandle" runat="server" CommandArgument='<%#Eval("PreOrderId")+",2"%>'
                            Style="color: #0063dc;" CommandName="Status">未处理</asp:LinkButton>                 
                  <%--<span > <a class='iframe' href="Show.aspx?id=<%#Eval("PreOrderId") %> ">
                        <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Site, btnDetailText %>" /></a></span> --%>
                    <span id="btnEdit">   <a class='iframe' href="Modify.aspx?id=<%#Eval("PreOrderId") %> ">
                        <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Site, btnEditText %>" /></a></span>
                </ItemTemplate>                
            </asp:TemplateField>
 
                        </Columns>
        <footerstyle height="25px" horizontalalign="Right" />
         <HeaderStyle Height="35px" />
        <pagerstyle height="25px" horizontalalign="Right" />
        <sorttip ascimg="~/Images/up.JPG" descimg="~/Images/down.JPG" />
        <rowstyle height="25px" />
        <sortdirectionstr>DESC</sortdirectionstr>
    </cc1:GridViewEx>
     <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
        <tr>
            <td>
               批量操作： <asp:DropDownList ID="dropStatusList" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="dropStatusList_SelectedIndexChanged">
                    <asp:ListItem Value="0">请选择</asp:ListItem>
                    <asp:ListItem Value="1">未处理</asp:ListItem>
                    <asp:ListItem Value="2">已处理</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>


 