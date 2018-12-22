<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Step1.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ProductType.Step1"
    Title="商品类型管理" %>
    
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<%@ Register Src="~/Controls/UCDroplistPermission.ascx" TagName="UCDroplistPermission"
    TagPrefix="uc2" %>
<%@ Register Assembly="YSWL.Controls" Namespace="YSWL.Controls" TagPrefix="YSWL" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/js/validate/pagevalidator.css" type="text/css" />
    <script type="text/javascript" src="/admin/js/validate/pagevalidator.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="新增新的商品类型" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以<asp:Literal ID="Literal3" runat="server" Text="商品类型是一系列属性的组合，可以用来向客户展示某些商品具有的特有的属性。" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="newslist">
            <div class="newsicon">
                <ul>
                    <li style="background: url(/admin/images/step1.jpg) no-repeat; width: 100%!important;"></li>
                </ul>
            </div>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                商品类型名称 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtTypeName" runat="server" Width="372px"></asp:TextBox>
                            </td>
                        </tr>
                                <tr>
                                    <td class="td_class">
                                    </td>
                                    <td height="25">
                                        <div id="txtTypeNameTip" runat="server">
                                        </div>
                                        <YSWL:ValidateTarget ID="ValidateTargetName" runat="server"  OkMessage="输入正确！"  Description="商品类型名称不能为空，长度限制在1-50个字符之间！"
                                            ControlToValidate="txtTypeName" ContainerId="ValidatorContainer">
                                            <Validators>
                                                <YSWL:InputStringClientValidator ErrorMessage="商品类型名称不能为空，长度限制在1-50个字符之间！" LowerBound="1"
                                                    UpperBound="50" />
                                            </Validators>
                                        </YSWL:ValidateTarget>
                                    </td>
                                </tr>
                                
                        <tr>
                            <td  style="height:8px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class" style="vertical-align:top;">
                                关联商品品牌 ：
                            </td>
                            <td height="25">
                                <YSWL:BrandsCheckBoxList ID="chkBrandsCheckBox" runat="server">
                                </YSWL:BrandsCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <label class="msgNormal" style="width: 200px">
                                    <asp:Literal ID="Literal1" runat="server" Text="选择与些商品类型关联的商品品牌" /></label>
                            </td>
                        </tr>
                        <tr>
                            <td  style="height:8px;">
                            </td>
                            <td height="6">
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                备注 ：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtRemark" runat="server" Width="372px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                    class="adminsubmit_short" OnClick="btnCancle_Click" title="返回列表页"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="下一步" title="下一步，新增扩展属性"
                                    class="adminsubmit_short"  OnClientClick="return PageIsValid();" OnClick="btnSave_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
