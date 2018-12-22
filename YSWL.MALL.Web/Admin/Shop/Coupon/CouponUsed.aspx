<%@ Page Title="优惠券管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="CouponUsed.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Coupon.CouponUsed" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="优惠券使用" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以进行使用优惠券操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td>
                    会员编号或卡号
                    <asp:TextBox ID="txtUser" runat="server" OnTextChanged="txtUser_Change" AutoPostBack="True"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
             <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="60px">
                    用户名  ：
                </td>
                 <td width="120px">
                     <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                </td>
                 <td width="60px">
                    昵称    ：
                </td>
                 <td>
                     <asp:Label ID="lblNickName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="80px">
                    真实姓名：
                </td>
                 <td>
                     <asp:Label ID="lblTrueName" runat="server" Text=""></asp:Label>
                </td>
                 <td>
                    联系方式：
                </td>
                 <td>
                     <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" DataKeyNames="CouponCode" ShowExportExcel="True" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="true">
            <Columns>
                <asp:TemplateField HeaderText="优惠券卡号" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("CouponCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="优惠券密码" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center"
                    Visible="false">
                    <ItemTemplate>
                        <%#Eval("CouponPwd")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CouponName" HeaderText="活动名称" ItemStyle-HorizontalAlign="Left"
                    Visible="False" />
                <asp:TemplateField HeaderText="优惠券分类" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#GetClassName(Eval("ClassId"))%>
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
                <asp:TemplateField HeaderText="积分" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("NeedPoint")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品分类" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center"
                    Visible="False">
                    <ItemTemplate>
                        <%#GetCategoryName(Eval("CategoryId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用者" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center"
                    Visible="False">
                    <ItemTemplate>
                        <%#GetUserName(Eval("UserId"))%>
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
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </Grv:GridViewEx>
        <div class="def-wrapper">
             <asp:Button ID="btnUsed" runat="server" Text="批量使用" OnClick="btnUsed_Click"  class="adminsubmit" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
