<%@ Page Language="C#" Title="编辑一键会员价格规则" MasterPageFile="~/Admin/BasicNoFoot.Master"
    AutoEventWireup="true" CodeBehind="UpdateRankRule.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.WholeSale.UpdateRankRule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .td_class
        {
            width: 80px;
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
            padding-bottom: 4px;
            padding-top: 4px;
        }
        .td_content
        {
            border-right: 1px solid #DBE2E7;
            border-left: 1px solid #fff;
            border-bottom: 1px solid #ddd;
            border-top: 1px solid #fff;
        }
        a.pffule_button
        {
            width: 81px;
            background: url(/admin/images/addpfru.png) no-repeat;
            cursor: pointer;
            height: 25px;
            float: left;
            padding-top: 3px;
        }
        .marginstyle
        {
            margin-bottom: 2px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#btnAddItem").click(function () {
                $("#txtRuleItem").append($("#txtTemplate").html());
            });
            var isEnable = $("#ctl00_ContentPlaceHolder1_hfEnable").val();
            if (isEnable == "True") {
                $("#tabUserRank").show();
            }
            var type = $("#ctl00_ContentPlaceHolder1_radItemType").find('input[checked="checked"]').val();
            if (type == 0) {
                $(".ItemType").text("按");
                $(".ItemUnit").html("%&#12288;计算");
            }
            if (type == 1) {
                $(".ItemType").text("总减价");
                $(".ItemUnit").text("元");
            }
            if (type == 2) {
                $(".ItemType").text("每个减");
                $(".ItemUnit").text("元");
            }
            $("#ctl00_ContentPlaceHolder1_radItemType").find('input').click(function () {
                var value = $(this).val();
                if (value == 0) {
                    $(".ItemType").text("按");
                    $(".ItemUnit").html("%&#12288;计算");
                }
                if (value == 1) {
                    $(".ItemType").text("总减价");
                    $(".ItemUnit").text("元");
                }
                if (value == 2) {
                    $(".ItemType").text("每个减");
                    $(".ItemUnit").text("元");
                }
            });
            $("#ctl00_ContentPlaceHolder1_radUnit").find('input').click(function () {
                var value = $(this).val();
                if (value == 0) {
                    $(".RuleUnit").text("个。");
                }
                if (value == 1) {
                    $(".RuleUnit").text("元。");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfSuccess" runat="server" />
    <asp:HiddenField ID="hfEnable" runat="server" />
    <div class="newslistabout">
        <div class="TabContent">
            <%--基本信息--%>
            <table style="width: 100%; border-bottom: none; border-top: none; float: left;" cellpadding="2"
                cellspacing="1" class="border">
                <tr>
                    <td style="vertical-align: top;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                            <tr>
                                <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                    <span style="font-size: 16px; padding-left: 20px">会员价格规则设置</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_class border-bno" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal2" runat="server" Text=" 规则名称" />：
                                </td>
                                <td height="35" class="td_content border-bno">
                                    <asp:TextBox ID="txtRuleName" runat="server" CssClass="marginstyle mar-bt5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_class border-bno" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal21" runat="server" Text=" 优惠方式" />：
                                </td>
                                <td height="25" class="td_content border-bno">
                                    <asp:RadioButtonList ID="radItemType" runat="server" RepeatDirection="Horizontal"
                                        align="left">
                                        <asp:ListItem Value="0" Text="打折" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="固定价"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_class" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal3" runat="server" Text=" 优惠值" />：
                                </td>
                                <td style="width: auto" class="RuleItems td_content">
                                    <span class="ItemType">按</span>
                                   <asp:TextBox  ID="txtRateValue"  runat="server"   class="RateValue mar-bt5" Width="80"></asp:TextBox><span class="ItemUnit">%&#12288;计算</span>&nbsp;<span
                                        class="hy d_default"></span>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="td_class" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal1" runat="server" Text=" 是否启用" />：
                                </td>
                                <td height="25" class="td_content">
                                    <asp:RadioButtonList ID="radStatus" runat="server" RepeatDirection="Horizontal" align="left">
                                        <asp:ListItem Value="1" Text="是" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tabUserRank" style="display: none">
                    <td style="vertical-align: top;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                            <tr>
                                <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                    <span style="font-size: 16px; padding-left: 20px">优惠等级对象</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="80">
                                </td>
                                <td height="25">
                                    会员等级：
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td height="25">
                                    <asp:CheckBoxList ID="ChkUserRank" runat="server" RepeatDirection="Horizontal" align="left" RepeatColumns="4">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="newslistabout">
        <table style="width: 100%; border-top: none; float: left;" cellpadding="2" cellspacing="1"
            class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="height: 6px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td height="25" style="text-align: center">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="保存" class="adminsubmit" ></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>

