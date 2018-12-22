
<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
  CodeBehind="GroupBuyList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.PromoteSales.GroupBuyList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<%@ Register TagPrefix="YSWL" TagName="AjaxRegion" Src="~/Controls/AjaxRegion.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
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

            $(".iframe").colorbox({ iframe: true, width: "680", height: "488", overlayClose: false });
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
                        <asp:Literal ID="Literal1" runat="server" Text="团购活动商品管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以对团购活动商品进行新增，编辑，删除，上架和下架操作
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
                    <asp:Literal ID="Literal3" runat="server" Text="状态" />：
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text=" 全部" Value=""></asp:ListItem>
                        <asp:ListItem Text=" 上架" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" 下架" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                     &nbsp;&nbsp;开始时间：
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;结束时间：
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;<asp:Literal ID="Literal2" runat="server" Text="活动说明" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="mar-t-15  mar-bt">
                  <asp:Button ID="butAdd" runat="server" Text="新增" class="adminsubmit_short"  OnClientClick="window.location='AddGroupBuy.aspx';return false;"/> 
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="GroupBuyId">
            <Columns>
                <asp:TemplateField HeaderText="商品名称" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="360">
                    <ItemTemplate>
                            <%# GetProductName(Eval("ProductId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="团购价格" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="Price">
                    <ItemTemplate>
                        <%#Eval("Price", "￥{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品总限购数量" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="MaxCount">
                    <ItemTemplate>
                        <%#Eval("MaxCount")%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                                    <asp:TemplateField HeaderText="单次购买限购数量" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="MaxCount">
                    <ItemTemplate>
                        <%#Eval("LimitQty")%>
                    </ItemTemplate>
                </asp:TemplateField>

                   <asp:TemplateField HeaderText="违约金" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FinePrice">
                    <ItemTemplate>
                        <%#Eval("FinePrice", "￥{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="团购满足数量" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="GroupCount">
                    <ItemTemplate>
                        <%#Eval("GroupCount")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动时间" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="160"  >
                    <ItemTemplate>
                      <%#Eval("StartDate", "{0:yyyy-MM-dd}")%> 至  <%#Eval("EndDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动说明" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%# Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                    <ItemTemplate>
                        <%# GetStatusName(Eval("Status"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                      <asp:TemplateField HeaderText="商品状态" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                    <ItemTemplate>
                       <%# GetProductStatus(Eval("ProductId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="操作" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <a href='UpdateGroupBuy.aspx?id=<%# Eval("GroupBuyId")%>' class="iframe">编辑</a>
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
        <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
                        <asp:Button ID="Button1" runat="server" Text="批量上架"
                        class="adminsubmit" OnClick="btnOn_Click" />
                        <asp:Button ID="Button2" runat="server" Text="批量下架"
                        class="adminsubmit" OnClick="btnOff_Click" />
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>

