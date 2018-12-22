<%@ Page Title="配送方式管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" CodeBehind="ShippingType.aspx.cs"
    Inherits="YSWL.MALL.Web.Admin.Shop.Shipping.ShippingType" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <!--Select2 Start-->
    <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <!--Select2 End-->
 <script type="text/javascript">
     $(function () {
         $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });
     });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="配送方式管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以对网站配送方式进行新增，编辑，删除等操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                 <asp:Literal ID="Literal4" runat="server" Text="配送方式" />：
                <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Literal ID="Literal5" runat="server" Text="支付方式" />：
                   <asp:DropDownList ID="ddlPayment" runat="server" Width="150px">  </asp:DropDownList>
                    <span style="display: none"><asp:Literal ID="Literal3" runat="server" Text="商家" />：
                <asp:DropDownList ID="ddlSupplier" runat="server" Width="150px">  </asp:DropDownList></span>
                 <asp:Button ID="Button1"  runat="server" Text="<%$ Resources:Site, btnSearchText %>" OnClick="btnSearch_Click"
                 class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                       <li class="add-btn" id="liAdd" runat="server">
                        <a href="AddShippingType.aspx" >新增</a></li>
                 
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="ModeId"
            Style="float: left;" ShowGridLine="true" ShowHeaderStyle="true" >
            <Columns>
            <asp:TemplateField HeaderText="所属商家" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="120" Visible="False">
                    <ItemTemplate>
                        <%# GetSupplier( Eval("SupplierId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="配送方式名称" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="120">
                    <ItemTemplate>
                        
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="首重" ItemStyle-HorizontalAlign="Center" ControlStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("Weight")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="缺省价" ItemStyle-HorizontalAlign="Right" ControlStyle-Width="80">
                    <ItemTemplate>
                        <div class="tx-r">￥<%# YSWL.Common.Globals.SafeDecimal(Eval("Price").ToString(),0).ToString("F")%></div>
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="加重" ItemStyle-HorizontalAlign="Center" ControlStyle-Width="80">
                    <ItemTemplate>
                        <%#Eval("AddWeight")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="加价" ItemStyle-HorizontalAlign="Right" ControlStyle-Width="80">
                    <ItemTemplate>
                        <div class="tx-r">￥<%# YSWL.Common.Globals.SafeDecimal(Eval("AddPrice").ToString(), 0).ToString("F")%></div>
                             
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="物流公司" ItemStyle-HorizontalAlign="Center"  Visible="false">
                    <ItemTemplate>
                        <%#Eval("ExpressCompanyName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="40" HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                         <a style="display: none;" href="ShowShippingType.aspx?id=<%#Eval("ModeId") %>">查看</a> &nbsp;&nbsp;
                      <span id="lbtnModify"  runat="server" >  <a href="UpdateShipType.aspx?id=<%#Eval("ModeId") %>">编辑</a> &nbsp;&nbsp;</span>
                        <asp:LinkButton ID="linkDel" CssClass="iframe-modf" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>"> </asp:LinkButton>
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
