<%@ Page Title="会员信息管理" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="List2.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.MembershipManage.List2" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='txtEndTime']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:" + new Date().getFullYear()) });
            $("[id$='txtBeginTime']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:" + new Date().getFullYear()) });

            $("span:contains('已冻结')").css("color", "red");
            $("span:contains('已激活')").css("color", "#006400");
            $("span:contains('未认证')").css("color", "red");
            $("span:contains('已认证')").css("color", "#006400");

            $(".iframeAdd").colorbox({ iframe: true, width: "800", height: "580", overlayClose: false });
        });
        function ExportConfirm() {
            if (confirm('您确认要导出吗？')) {
                $.jBox.tip("正在导出用户数据，请稍候...", 'loading', { timeout: 3000 });
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
                    <td bgcolor="#FFFFFF" class="newstitle">会员信息管理
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">您可以查看用户标签信息操作
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="mar-bt">
            <tr>
                <td height="35" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="关键字" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="dropType" runat="server">
                        <asp:ListItem Value="-1" Text="--请选择--"></asp:ListItem>
                        <asp:ListItem Value="0" Text="冻结"></asp:ListItem>
                        <asp:ListItem Value="1" Text="激活"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Literal ID="Literal5" runat="server" Text="注册时间："></asp:Literal>
                    <asp:TextBox ID="txtBeginTime" runat="server" CssClass="PostDate"></asp:TextBox>
                    <asp:Literal ID="Literal6" runat="server" Text="--"></asp:Literal>
                    <asp:TextBox ID="txtEndTime" runat="server" CssClass="PostDate"> </asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short add-btn mar-le"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->

        <div class="newslist mar-bt" style="display: none">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" class="add-btn" runat="server">
                        <a href="AddUser.aspx" class="iframeAdd">新增</a> <%--<b>|</b>--%> </li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowCommand="gridView_RowCommand" OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting"
            UnExportedColumnNames="Modify" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="true" DataKeyNames="UserID">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="编号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="40" SortExpression="UserID" />
                <asp:TemplateField HeaderText="用户" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table class="tab-tc">
                            <tr>
                                <td style="width: 240px">用 户 名：
                                    <%#Eval("UserName")%>
                                </td>
                                <td style="width: 240px">性&#12288;&#12288;别：
                                    <%#Eval("Sex").ToString().Trim() == "0" ? "女" : "男"%>
                                </td>
                            </tr>
                            <tr>
                                <td>真实姓名：<span style="color: Gray">
                                    <%#Eval("TrueName")%></span>
                                </td>
                                <td>联系方式： <span style="color: Gray">
                                    <%#Eval("Phone")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td>邮&#12288;&#12288;箱：<span style="color: Gray">
                                    <%#Eval("Email")%></span>
                                </td>
                                <td>
                                    <%--   所 在 地： <span style="color: Gray">
                                        <%# GetRegion(Eval("RegionId"))%></span>--%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Balance" HeaderText="余额" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="40" SortExpression="Balance" DataFormatString="{0:F}" Visible="false" />
                
               <asp:TemplateField HeaderText="业务员" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#GetSalesName(Eval("EmployeeID"))%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="RankScore" HeaderText="成长值" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="70" SortExpression="RankScore" Visible="false" />
                <asp:BoundField DataField="User_dateCreate" HeaderText="注册时间" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="100" SortExpression="User_dateCreate" />

                <asp:TemplateField HeaderText="用户状态" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="lbtnID" runat="server" CausesValidation="False" CommandName="Status"
                            CommandArgument='<%#Eval("UserID")+","+Eval("Activity")%>' Style="color: #0063dc;">
                            <span ><%#(bool)Eval("Activity") ? "已激活" : "已冻结"%></span>
                        </asp:LinkButton>--%>
                        <span Style="color: #0063dc;"><%#(bool)Eval("Activity") ? "已激活" : "已冻结"%></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180" Visible="false">
                    <ItemTemplate>
                        <a class="iframe" href='ModifyBalance.aspx?id=<%#Eval("UserID") %>'>账户充值</a>&nbsp;
                        <span runat="server" id="hlRankScore" visible="False">
                            <a class="iframe" href='ModifyRankScore.aspx?id=<%#Eval("UserID") %>'>成长值充值</a>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"
                    ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
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
        <table border="0" cellpadding="0" cellspacing="1" style="width: 500px; height: 100%" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnActivity" runat="server" Text="批量激活" class="adminsubmit mar-t10" Visible="False" OnClick="btnActivity_Click" />
                    <asp:Button ID="btnUnActivity" runat="server" Text="批量冻结" class="adminsubmit mar-t10" Visible="False" OnClick="btnUnActivity_Click" />
                   <asp:Button ID="btnExport" runat="server" Text="一键导出" OnClientClick="return ExportConfirm();" OnClick="btnExport_Click" class="adminsubmit mar-t10"></asp:Button>
                </td>
                <td>
                        <asp:DropDownList ID="ddlSalesList" CssClass="mar-t10" runat="server">
                        <asp:ListItem Value="0" Text="--请选择业务员--"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                     <td>
                      <asp:Button ID="btnMove" runat="server" Text="批量转移" OnClientClick="return  confirm('您确认要转移吗？');" OnClick="btnMove_Click" class="adminsubmit mar-t10"></asp:Button>
                    </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".iframe").colorbox({ iframe: true, width: "800", height: "420", overlayClose: false });
            $(".iframeshow").colorbox({ iframe: true, width: "800", height: "660", overlayClose: false });
        });
    </script>
</asp:Content>
