<%@ Page Title="优惠券管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
 CodeBehind="CouponHistorys.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Coupon.CouponHistorys" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("#ctl00_ContentPlaceHolder1_txtStartDate").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtEndDate").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ctl00_ContentPlaceHolder1_txtEndDate").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtStartDate").datepicker("option", "maxDate", selectedDate);
                    $("#ctl00_ContentPlaceHolder1_txtEndDate").val($(this).val());
                }
            });
        })
    </script>
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="过期优惠券管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以进行查看过期优惠券操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
        <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    优惠券分类：<asp:DropDownList ID="ddlClass" runat="server" Width="124px">
                    </asp:DropDownList>
                    &#12288;优惠券规则：<asp:DropDownList ID="ddlRule" runat="server" Width="124px">
                    </asp:DropDownList>
                    &#12288;&#12288;使用状态：<asp:DropDownList ID="ddlStatus" runat="server" Width="124px">
                        <asp:ListItem Value="" Text="请选择"> </asp:ListItem>
                        <asp:ListItem Value="0" Text="未分配"> </asp:ListItem>
                        <asp:ListItem Value="1" Text="已分配"> </asp:ListItem>
                        <asp:ListItem Value="2" Text="已使用"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td  height="35" bgcolor="#FFFFFF" class="newstitlebody">
                   &#12288;开始时间：<asp:TextBox ID="txtStartDate" runat="server" Width="124px"></asp:TextBox>
                    &#12288;&#12288;结束时间：<asp:TextBox ID="txtEndDate" runat="server" Width="124px"></asp:TextBox>
                     &#12288;<asp:Literal ID="Literal3" runat="server" Text="优惠券卡号：" /><asp:TextBox
                            ID="txtKeyword" runat="server"  Width="124px"></asp:TextBox>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td  height="35" bgcolor="#FFFFFF" class="newstitlebody">
                          <asp:CheckBox ID="chkRule" runat="server" />
                    显示活动名称 &nbsp;&nbsp;
                    <asp:CheckBox ID="chkCategory" runat="server" />
                    显示商品分类 &nbsp;&nbsp;
                    <asp:CheckBox ID="chkSupplier" runat="server" />
                    显示商家 &nbsp;&nbsp;
                    <asp:CheckBox ID="chkUser" runat="server" />
                    显示使用者 &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <br />

        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" DataKeyNames="CouponCode" ShowExportExcel="True"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="true">
            <Columns>
                <asp:TemplateField HeaderText="优惠券卡号" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("CouponCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="优惠券密码" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("CouponPwd")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CouponName" HeaderText="活动名称" ItemStyle-HorizontalAlign="Left" Visible="False" />
                
                <asp:TemplateField HeaderText="优惠券分类" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#GetClassName(Eval("ClassId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商家名称" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" Visible="False">
                    <ItemTemplate>
                        <%#GetSupplierName(Eval("SupplierId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="面值" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("CouponPrice", "￥{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="最低消费金额" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("LimitPrice", "￥{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="积分" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("NeedPoint")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品分类" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" Visible="False">
                    <ItemTemplate>
                        <%#GetCategoryName(Eval("CategoryId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用者" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" Visible="False">
                    <ItemTemplate>
                        <%#GetUserName(Eval("UserId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="用户邮箱" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("UserEmail")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动时间" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("StartDate","{0:yyyy-MM-dd}")%>
                        至
                        <%#Eval("EndDate", "{0:yyyy-MM-dd}")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="生成时间" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("GenerateTime", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用时间" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("UsedDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#GeStatusName(Eval("Status"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="操作" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="120" Visible="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </Grv:GridViewEx>
        <div class="def-wrapper">
<asp:Button ID="btnDelete" runat="server" Text="批量删除" OnClick="btnDelete_Click"  CssClass="adminsubmit"/>
        </div>
        

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
