
<%@ Page Title="佣金规则管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="RuleList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Commission.RuleList" %>


<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".iframe").colorbox({ iframe: true, width: "600", height: "480", overlayClose: false });
            $(".SetProduct").colorbox({ iframe: true, width: "1080", height: "800", overlayClose: false });

            $(".SetProduct").each(function () {
                var isAll = $(this).attr("isAll");
                if (isAll == 'True') {
                    $(this).hide();
                }
            });

            $(".btnStatus").each(function () {
                var ruleId = $(this).attr("ruleid");
                var status = parseInt($(this).attr("status"));
                if (status == 0) {
                    $(this).css("color", "red");
                }
                else {
                    $(this).css("color", "green");
                }
            })
            $(".btnStatus").click(function () {
                var ruleId = $(this).attr("ruleid");
                var status = parseInt($(this).attr("status"));
                status = status == 0 ? 1 : 0;
                var self = $(this);
                $.ajax({
                    url: ("RuleList.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST',
                    dataType: 'json',
                    timeout: 10000,
                    data: { Action: "UpdateStatus", Callback: "true", RuleId: ruleId, Status: status },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            ShowSuccessTip("操作成功");
                            self.attr("status", status);
                            if (status == 0) {
                                self.text("未启用");
                                self.css("color", "red");
                            } else {
                                self.text("启用");
                                self.css("color", "green");
                            }
                        } else {
                            alert("系统忙请稍后再试！");
                        }
                    }
                });
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
                        <asp:Literal ID="Literal1" runat="server" Text="佣金规则设置" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        可以设置佣金规则和参与佣金分配的商品。
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="规则名称查询" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" runat="server" class="add-btn">
                        <a class="iframe" href="AddRule.aspx">新增</a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="RuleId"
            Style="float: left;" ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="规则名称" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("RuleName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="计算方式" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# GetRuleMode(Eval("RuleMode")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="第一级用户" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                           <%#YSWL.Common.Globals.SafeInt(Eval("RuleMode"), 0) ==1? Eval("FirstValue", "{0:N2}") + "%" : "￥" + Eval("FirstValue", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="第二级用户" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                           <%#YSWL.Common.Globals.SafeInt(Eval("RuleMode"), 0) == 1 ? Eval("SecondValue", "{0:N2}") + "%" : "￥" + Eval("SecondValue", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreatedDate" HeaderText="创建时间" DataFormatString="{0:yyyy-MM-dd}"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                       <a class="btnStatus" ruleid='<%# Eval("RuleId") %>'  status='<%# Eval("Status") %>'> <%# GetStatus(Eval("Status")) %> </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="全部参与" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                      <%#YSWL.Common.Globals.SafeBool(Eval("IsAll"), true) ? "是" : "否"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建者" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# GetCreatedName(Eval("CreatedUserID")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a class="SetProduct" href='RuleProList.aspx?id=<%# Eval("RuleId") %>' isAll="<%# Eval("IsAll") %>">设置应用商品 </a>
                        &nbsp;&nbsp; <span id="lbtnModify" runat="server"><a class="iframe" href='UpdateRule.aspx?id=<%# Eval("RuleId") %>'>
                            修改</a>&nbsp;</span>
                        <asp:LinkButton ID="linkDel" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" BackColor="#FFF" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </cc1:GridViewEx>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td style="display: none">
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>

