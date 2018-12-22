<%@ Page Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="Modify.aspx.cs" Inherits="YSWL.MALL.Web.Shop.Order.PreOrder.Modify" Title="修改页" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $('[id$="txtCount"]').OnlyNum();
         });
    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="预定编辑" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以编辑<asp:Literal ID="Literal3" runat="server" Text="预定信息" />
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
		<asp:label id="lblPreOrderId" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td class="td_class">
		商品名称
	：</td>
	<td   height="25">
    <asp:label id="lblProductName" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td class="td_class">
		数量
	：</td>
	<td   height="25">
		<asp:TextBox id="txtCount" runat="server" Width="50px"></asp:TextBox>
	</td></tr>
	<tr>
	<td class="td_class">
		电话
	：</td>
	<td   height="25">
      <asp:label id="lblPhone" runat="server"></asp:label>
	 
	</td></tr>
	<tr>
	<td class="td_class">
		创建人
	：</td>
	<td   height="25">
      <asp:label id="lblUserName" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td class="td_class">
		创建时间
	：</td>
	<td   height="25">
    <asp:label ID="lblCreatedDate" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td class="td_class">
		处理人
	：</td>
	<td   height="25">
    <asp:label ID="lblHandleUserId" runat="server"></asp:label>
 	</td></tr>
	<tr>
	<td class="td_class">
		处理时间
	：</td>
	<td   height="25">
        <asp:label ID="lblHandleDate" runat="server"></asp:label>
	</td></tr>
	<tr style="display:none;">
	<td class="td_class">
		预定配送提示
	：</td>
	<td   height="25">
    <asp:label ID="lblDeliveryTip" runat="server"></asp:label>
	</td></tr>
	<tr >
	<td class="td_class">
		状态
	：</td>
	<td   height="25">
     <asp:DropDownList ID="dropStatus" runat="server">
                    <asp:ListItem Value="0">请选择</asp:ListItem>
                    <asp:ListItem Value="1">未处理</asp:ListItem>
                    <asp:ListItem Value="2">已处理</asp:ListItem>
                </asp:DropDownList>
	</td></tr>
	<tr style="display:none;">
	<td class="td_class">
		备注
	：</td>
	<td   height="25">
		<asp:TextBox id="txtRemark" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
    <tr>
            <td  class="td_class">
              
            </td>
            <td>
                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                    class="adminsubmit_short" OnClick="btnCancle_Click" OnClientClick=" javascript: parent.$.colorbox.close();"></asp:Button>
                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>" class="adminsubmit_short"  OnClick="btnSave_Click">
                </asp:Button>
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
 

