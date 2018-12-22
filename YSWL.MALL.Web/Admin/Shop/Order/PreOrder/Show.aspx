<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="YSWL.MALL.Web.Shop.Order.PreOrder.Show" Title="显示页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="预定详情" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以查看<asp:Literal ID="Literal3" runat="server" Text="预定信息" />
                    </td>
                </tr>
            </table>
        </div>
   
   <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>                   
                    <td class="tdbg">
                               
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
	<td class="td_class">
		预定ID
	：</td>
	<td   height="25">
		<asp:Label id="lblPreOrderId" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		产品ID
	：</td>
	<td   height="25">
		<asp:Label id="lblProductId" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		商品名称
	：</td>
	<td   height="25">
		<asp:Label id="lblProductName" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		数量
	：</td>
	<td   height="25">
		<asp:Label id="lblCount" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		SKU
	：</td>
	<td   height="25">
		<asp:Label id="lblSKU" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		电话
	：</td>
	<td   height="25">
		<asp:Label id="lblPhone" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		创建者
	：</td>
	<td   height="25">
		<asp:Label id="lblUserId" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		UserName
	：</td>
	<td   height="25">
		<asp:Label id="lblUserName" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		创建时间
	：</td>
	<td   height="25">
		<asp:Label id="lblCreatedDate" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		处理人
	：</td>
	<td   height="25">
		<asp:Label id="lblHandleUserId" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		处理时间
	：</td>
	<td   height="25">
		<asp:Label id="lblHandleDate" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		交货时间
	：</td>
	<td   height="25">
		<asp:Label id="lblDeliveryDate" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		预定状态
	：</td>
	<td   height="25">
		<asp:Label id="lblStatus" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_class">
		备注
	：</td>
	<td   height="25">
		<asp:Label id="lblRemark" runat="server"></asp:Label>
	</td></tr>
       <tr>
            <td  class="td_class">
              
            </td>
            <td>   
                                    <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                        class="adminsubmit_short" OnClick="btnCancle_Click" OnClientClick=" javascript: parent.$.colorbox.close();"></asp:Button>
                                        </td>
                                        </tr>
</table>

                    </td>
                </tr>
            </table>

            </div>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>




