
<%@ Page Title="<%$ Resources:SysManage,ptClearCache%>" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="AutoUpdate.aspx.cs" Inherits="YSWL.MALL.Web.Admin.AutoUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="自动更新" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="自动更新功能" />
                    </td>
                </tr>
            </table>
        </div>
            <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>
                    <td style="width: 80px" align="right" class="tdbg">
                        
                    </td>
                    <td class="tdbg" align="left">                    
                    <asp:Button ID="btnClear" runat="server" Text="检测版本" OnClick="btnCheck_Click"
                    class="adminsubmit" />                        
                    </td>                    
                </tr>
                 <tr>
                  <td style="width: 80px" align="right" class="tdbg">
                        
                    </td>
                    <td style="text-align:left; padding-left:20px" class="tdbg">
                <asp:Label ID="Label1" runat="server" ForeColor="Green"></asp:Label>
                </td>
                </tr>
            </table>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>

