<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="YSWL.MALL.Web.Ms.Regions.Add" Title="增加页" %>

<%@ Register src="/Admin/../Controls/Region.ascx" tagname="Region" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="新增地区" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以新增不同的地区 " />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                选择地区 ：
                            </td>
                            <td height="25">
                            <uc1:Region ID="Regions1" runat="server" />
                            (新增地区说明 :新增省份：不需要选择任何地区；新增市区：选择省份即可；新增县城：选择省份和市区)
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                地区名称 ：
                            </td>
                            <td height="25">
                             
                            <asp:TextBox ID="txtRegionName" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td class="td_class">
                                地区简称 ：
                            </td>
                            <td height="25">
                            <asp:TextBox ID="txtSpellShort" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                显示顺序 ：
                            </td>
                            <td height="25">
                            <asp:TextBox ID="txtDisplaySequence" runat="server" Width="200px" Text="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                    class="adminsubmit_short" OnClick="btnCancle_Click"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    class="adminsubmit_short" OnClick="btnSave_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
