<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="YSWL.MALL.Web.Shop.Products.ActivityRule.Modify" Title="修改页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="促销规则管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行编辑促销规则操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                   <table  cellspacing="0" cellpadding="3" width="100%" border="0">
	<tr>
	<td class="td_class">
		规则Id
	：</td>
	<td height="25">
		<asp:label id="lblRuleId" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td class="td_class">
		规则名称
	：</td>
	<td height="25">
		<asp:TextBox id="txtRuleName" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td class="td_class">
		优先级
	：</td>
	<td height="25">
		<asp:TextBox id="txtPriority" runat="server"  Width="50px" ></asp:TextBox>
	</td></tr>
	<tr>
	<td class="td_class">
		活动状态
	：</td>
	<td height="25">
		 <asp:CheckBox ID="checkboxStatus" Checked="true"  runat="server" />是否启用
	</td></tr>
	<tr>
	<td class="td_class">
		创建人
	：</td>
	<td height="25">
    <asp:label id="lblCreatedUserId" runat="server"></asp:label>
 
	</td></tr>
	<tr>
	<td class="td_class">
		创建时间
	：</td>
	<td height="25">
    <asp:label id="lblCreatedDate" runat="server"></asp:label>
	 
	</td></tr>
     <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                 <asp:Button ID="btnCancle" runat="server" Text="取消"
                    OnClick="btnCancle_Click" class="adminsubmit_short"  ></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnSave" runat="server" Text="确定" OnClick="btnSave_Click" class="adminsubmit_short">
                                </asp:Button>
                            </td>
                        </tr>
</table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
 