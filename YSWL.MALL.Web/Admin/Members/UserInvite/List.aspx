<%@ Page Title="用户层级管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserInvite.List" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("span:contains('否')").css("color", "#d71345");
            $("span:contains('是')").css("color", "#006400");

            $(".iframe").colorbox({ iframe: true, width: "800", height: "560", overlayClose: false });


            $(".btnStatus").each(function () {
                var ruleId = $(this).attr("ruleid");
                var status = parseInt($(this).attr("status"));
                if (status == 0) {
                    $(this).css("color", "red");
                } else {
                    $(this).css("color", "green");
                }
            });
            $(".btnStatus").click(function () {
                var InviteId = $(this).attr("InviteId");
                var status = parseInt($(this).attr("status"));
                status = status == 0 ? 1 : 0;
                var self = $(this);
                $.ajax({
                    url: ("List.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST',
                    dataType: 'json',
                    timeout: 10000,
                    data: { Action: "UpdateStatus", Callback: "true", InviteId: InviteId, Status: status },
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
        function GetDeleteM() {
            $("[id$='btnDelete']").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="用户层级管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以查看，删除用户层级信息
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="邀请用户昵称" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist ul-pd0 mar-bt">
            <div class="newsicon">
                <ul class="list">
                    <li id="liRevert" runat="server" style="padding-left: 0px">
                        <asp:Button ID="Button2" OnClientClick="return confirm('你确定要删除吗？')" runat="server"
                            Text="<%$ Resources:Site, btnDeleteListText %>" class="add-btn" OnClick="btnDelete_Click" />
                    </li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="True" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="InviteId">
            <Columns>
                <asp:BoundField DataField="UserId" HeaderText="用户编号" SortExpression="UserId" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="80px" />
                <asp:BoundField DataField="UserNick" HeaderText="用户昵称" SortExpression="UserNick"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="InviteUserId" HeaderText="上级用户编号" SortExpression="InviteUserId"
                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                <asp:BoundField DataField="InviteNick" HeaderText="上级用户昵称" SortExpression="InviteNick"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ItemStyle-Width="140" HeaderText="创建时间" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <span>
                            <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a class="btnStatus" inviteid='<%# Eval("InviteId") %>' status='<%# Eval("Status") %>'>
                            <%# GetStatus(Eval("Status")) %>
                        </a>
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
        <table class="mar-t-15" border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="add-btn" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
