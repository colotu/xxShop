<%@ Page Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="Show.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.Show" Title="仓库详细页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="仓库信息" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="仓库信息" />查看
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    
<table cellSpacing="0" cellPadding="3" width="100%" border="0">
 <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="仓库名称" />：
                            </td>
                            <td height="25" Width="250px">
                                <asp:Label id="lblName" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="仓库编码" />：
                            </td>
                            <td height="25">
                              <asp:Label id="lblCode" runat="server"></asp:Label>
                                </td>
                        </tr>

	  <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal1" runat="server" Text="地区" />：
                            </td>
                            <td height="25" Width="250px">
                               <asp:Label id="lblRegionId" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="详细地址" />：
                            </td>
                            <td height="25">
                              	<asp:Label id="lblAddress" runat="server"></asp:Label>
                                </td>
                        </tr>
	  <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="联系人" />：
                            </td>
                            <td height="25" Width="250px">
                             	<asp:Label id="lblContactName" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal6" runat="server" Text="联系手机" />：
                            </td>
                            <td height="25">
                            	<asp:Label id="lblPhone" runat="server"></asp:Label>
                                </td>
                        </tr>
 
	 <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal7" runat="server" Text="是否启用" />：
                            </td>
                            <td height="25" Width="250px">
                             <asp:Label id="lblStatus" runat="server"></asp:Label>
                          
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="邮箱" />：
                            </td>
                            <td height="25">
                             <asp:Label id="lblEmail" runat="server"></asp:Label>
                                </td>
                        </tr>
  <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal11" runat="server" Text="备注" />：
                            </td>
                            <td height="25" Width="250px">
                              <asp:Label id="lblRemark" runat="server"></asp:Label>
                            
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal12" runat="server" Text="创建时间" />：
                            </td>
                            <td height="25">
                            <asp:Label id="lblCreatedDate" runat="server"></asp:Label>
                                </td>
                        </tr>
   <tr style="display:none;">
                            <td class="td_class">
                                <asp:Literal ID="Literal13" runat="server" Text="纬度" />：
                            </td>
                            <td height="25" Width="250px">
                            	<asp:Label id="lblLatitude" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal14" runat="server" Text="经度" />：
                            </td>
                            <td height="25">
                           <asp:Label id="lblLongitude" runat="server"></asp:Label>
                                </td>
                        </tr>
   <tr style="display:none;"> 
                            <td class="td_class">
                                <asp:Literal ID="Literal15" runat="server" Text="仓库类型" />：
                            </td>
                            <td height="25" Width="250px">
                            	<asp:Label id="lblType" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                                <asp:Literal ID="Literal16" runat="server" Text="仓库属性" />：
                            </td>
                            <td height="25">
                        	<asp:Label id="lblDepotAttr" runat="server"></asp:Label>
                                </td>
                        </tr>
 <tr  style="display:none;">
                            <td class="td_class">
                                <asp:Literal ID="Literal17" runat="server" Text="助记码" />：
                            </td>
                            <td height="25" Width="250px">
                          	<asp:Label id="lblHelpCode" runat="server"></asp:Label>
                                </td>
                                            <td class="td_class">
                             
                            </td>
                            <td height="25">
                         
                                </td>
                        </tr>
	  
 <tr>
                             
                            <td height="25" colspan="4" style="text-align:center;">
                                 <asp:Button ID="btnCancle" runat="server" CausesValidation="false" Text="<%$Resources:Site,btnBackText%>" class="adminsubmit_short"   OnClientClick="javascript:parent.$.colorbox.close();"></asp:Button>
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
