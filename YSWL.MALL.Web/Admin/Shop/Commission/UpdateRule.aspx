<%@ Page Language="C#" Title="编辑佣金规则" MasterPageFile="~/Admin/BasicNoFoot.Master"
    AutoEventWireup="true"  CodeBehind="UpdateRule.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Commission.UpdateRule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
   <style type="text/css">
        .td_class
        {
            width: 120px;
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
  
        .marginstyle {
            margin-bottom: 2px;
            width: 60px;
        }
    </style>
    <script type="text/javascript">
        $(function() {
            var mode = $("#ctl00_ContentPlaceHolder1_radMode").find("input[checked='checked']").val();
            if (mode == 0) {
                $(".txtunit").text("  元");
            }
            if (mode == 1) {
                $(".txtunit").text("  %");
            }
            $("#ctl00_ContentPlaceHolder1_radMode").find('input').click(function () {
                var value = $(this).val();
                if (value == 0) {
                    $(".txtunit").text("  元");
                }
                if (value == 1) {
                    $(".txtunit").text("  %");
                }
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfSuccess" runat="server" />
    <asp:HiddenField ID="hfItems" runat="server" />
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
                                    <span style="font-size: 16px; padding-left: 20px">佣金规则设置</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_class" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal2" runat="server" Text=" 规则名称" />：
                                </td>
                                <td height="35" class="td_content">
                                    <asp:TextBox ID="txtRuleName" runat="server" CssClass="marginstyle" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                                 <tr  >
                                <td class="td_class" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal3" runat="server" Text="全部商品参与" />：
                                </td>
                                <td height="25" class="td_content">
                                        <asp:CheckBox ID="chkIsAll" runat="server" /> 是
                                </td>
                             
                            </tr>
                            <tr  >
                                <td class="td_class" style="background-color: #E2E8EB">
                                    <asp:Literal ID="Literal1" runat="server" Text="是否启用" />：
                                </td>
                                <td height="25" class="td_content">
                                       <asp:CheckBox ID="chkStatus" runat="server"  /> 是
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                            <tr>
                                <td colspan="2" class="newstitle" bgcolor="#FFFFFF">
                                    <span style="font-size: 16px; padding-left: 20px">设置优惠规则</span>
                                </td>
                            </tr>
                            <tr>
                                  <td class="td_class" style="background-color: #E2E8EB">
                                    计算方式：
                                </td>
                                <td height="25" class="td_content">
                                    <asp:RadioButtonList ID="radMode" runat="server" RepeatDirection="Horizontal" align="left">
                                        <asp:ListItem Value="0" Text="固定金额" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="百分比"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_class" style="background-color: #E2E8EB">
                                    第一级用户
                                </td>
                                <td height="25" class="td_content">
                                       <asp:TextBox ID="txtFirst" runat="server" CssClass="marginstyle"></asp:TextBox> <span class="txtunit">元</span>
                                </td>
                            </tr>
                            <tr>
                                  <td class="td_class" style="background-color: #E2E8EB">
                                    第二级用户
                                </td>
                                <td height="25" class="td_content">
                                       <asp:TextBox ID="txtSecond" runat="server" CssClass="marginstyle"></asp:TextBox> <span class="txtunit">元</span>
                                </td>
                            </tr>
                             <tr style="display: none">
                                   <td class="td_class" style="background-color: #E2E8EB">
                                    第三级用户
                                </td>
                                <td height="25" class="td_content">
                                       <asp:TextBox ID="txtThird" runat="server" CssClass="marginstyle"></asp:TextBox><span class="txtunit">元</span>
                                </td>
                            </tr>
                                <tr style="display: none">
                                <td class="td_class" style="background-color: #E2E8EB">
                                    第四级用户
                                </td>
                                <td height="25" class="td_content">
                                       <asp:TextBox ID="txtFourth" runat="server" CssClass="marginstyle"></asp:TextBox><span class="txtunit">元</span>
                                </td>
                            </tr>
                               <tr style="display: none">
                                  <td class="td_class" style="background-color: #E2E8EB">
                                    第五级用户
                                </td>
                                <td height="25" class="td_content">
                                       <asp:TextBox ID="txtFifth" runat="server" CssClass="marginstyle"></asp:TextBox><span class="txtunit">元</span>
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
                            <td>
                            </td>
                            <td height="25" style="text-align: center">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="保存" class="adminsubmit"
                                    OnClientClick="return SubForm();"></asp:Button>
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


