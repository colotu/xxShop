<%@ Page Title="新增仓库操作" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master"
    AutoEventWireup="true" CodeBehind="AddDepot.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.AddDepot" %>

<%@ Register TagPrefix="uc1" TagName="Region" Src="~/Controls/Region.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select {
            width: 252px;
            height: 30px;
            margin: 2px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="新增仓库操作" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal6" runat="server" Text="您可以进行新增仓库操作" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">

                        <tr>
                            <td class="td_class" style="color: red;">*
                               <asp:Literal ID="Literal3" runat="server" Text="仓库名称" />：
                            </td>
                            <td height="25" width="250px">
                                <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                            <td class="td_class">
                                <asp:Literal ID="Literal2" runat="server" Text="仓库编码" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtCode" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="td_class" style="color: red;">*
                                <asp:Literal ID="Literal5" runat="server" Text="所属区域" />：
                            </td>
                            <td height="25" width="300px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <uc1:Region ID="RegionID" runat="server" VisibleAll="true" VisibleAllText="--请选择--" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="td_class" style="color: red;">*
                                <asp:Literal ID="Literal7" runat="server" Text="详细地址" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtAddress" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class" style="color: red;">*
                                <asp:Literal ID="Literal8" runat="server" Text="联系人" />：
                            </td>
                            <td height="25" width="250px">
                                <asp:TextBox ID="txtContactName" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="联系人手机" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPhone" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal10" runat="server" Text="是否启用" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />启用
                            </td>
                            <td class="td_class">
                                <asp:Literal ID="Literal11" runat="server" Text="联系人邮箱" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="仓库描述" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtRemark" runat="server" Width="250px" MaxLength="50" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>
                            </td>
                            <td class="td_class"></td>
                            <td height="25"></td>
                        </tr>
                        <tr>
                            <td height="25" colspan="4" style="text-align: center;">
                                <asp:Button ID="btnSave" runat="server" Text="保存"
                                    OnClick="btnSave_Click" class="adminsubmit"></asp:Button>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>

